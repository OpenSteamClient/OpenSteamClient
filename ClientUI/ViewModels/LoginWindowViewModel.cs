using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media.Imaging;
using ClientUI.Translation;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using QRCoder;

namespace ClientUI.ViewModels;

public partial class LoginWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private bool rememberPassword = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasQRCode))]
    private Bitmap? qRCode;

    [ObservableProperty]
    private bool credentialLoginVisible = true;

    [ObservableProperty]
    private bool qRLoginVisible = true;

    [ObservableProperty]
    private bool canLogin = true;

    public bool HasQRCode {
        get {
            return QRCode != null;
        }
    }

    private IClientUser iClientUser;
    private TranslationManager tm;
    private LoginManager loginManager;
    private QRCodeGenerator qrGenerator;

    //TODO: make a better system for communicating certain things to the views. This is hacky, and feels like we're reimplementing the wheel. ReactiveUI does not have a better solution for this unfortunately either, as it also adds a ton of spaghetti
    public Action<SecondFactorNeededEventArgs>? ShowSecondFactorDialog;

    public LoginWindowViewModel(IClientUser iClientUser, TranslationManager tm, LoginManager loginManager, LoginUser? user = null) {
        this.iClientUser = iClientUser;
        this.tm = tm;
        this.loginManager = loginManager;
        this.loginManager.QRGenerated += this.OnQRGenerated;
        this.loginManager.SecondFactorNeeded += this.OnSecondFactorNeeded;
        this.loginManager.LogOnFailed += this.OnLogonFailed;
        this.loginManager.LoggedOn += this.OnLoggedOn;

        qrGenerator = new QRCodeGenerator();

        if (user != null) {
            this.Username = user.AccountName;
        }

        this.loginManager.StartQRAuthLoop();
    }

    private void OnQRGenerated(object sender, QRGeneratedEventArgs e) {
        this.SetQRCode(e.URL);
    }

    private void OnLogonFailed(object sender, LogOnFailedEventArgs e) {
        // Showing errors is handled in AvaloniaApp.axaml.cs
        CanLogin = true;
    }

    private void OnLoggedOn(object sender, LoggedOnEventArgs e) {
        // Hiding and destroying this window is done elsewhere, do this just to be sure though
        CanLogin = true;
    }

    private void OnSecondFactorNeeded(object sender, SecondFactorNeededEventArgs e) {
        Console.WriteLine("Second factor needed:");
        foreach (var item in e.AllowedConfirmations)
        {
            Console.WriteLine("confirmation: " + item.ConfirmationType + ": " + item.AssociatedMessage);
        }
        
        ShowSecondFactorDialog?.Invoke(e);
    }

    public void SetQRCode(string url) {
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        byte[] png = qrCode.GetGraphic(20);
        using (Stream stream = new MemoryStream(png)) {
            QRCode = new Avalonia.Media.Imaging.Bitmap(stream);
        }
    }

    public void RegisterPressed() {
        MessageBox.Show(tm.GetTranslationForKey("#Unsupported"), string.Format(tm.GetTranslationForKey("#LoginWindow_AccountCreationUnsupported"), "https://store.steampowered.com/join/"));
    }

    public async void LoginPressed() {
        CanLogin = false;
        EResult result = await this.loginManager.StartAuthSessionWithCredentials(this.Username, this.Password, this.RememberPassword);
        if (result != EResult.k_EResultOK) {
            CanLogin = true;
            MessageBox.Show(tm.GetTranslationForKey("#LoginFailed"), string.Format(tm.GetTranslationForKey("#LoginFailed_Description"), this.Username, result));
        }
    }
}
