using System;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media.Imaging;
using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
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
    private Bitmap? qRCode;

    private IClientUser iClientUser;
    private TranslationManager tm;
    private LoginManager loginManager;
    private QRCodeGenerator qrGenerator;

    public LoginWindowViewModel(IClientUser iClientUser, TranslationManager tm, LoginManager loginManager, LoginUser? user = null) {
        this.iClientUser = iClientUser;
        this.tm = tm;
        this.loginManager = loginManager;
        this.loginManager.QRGenerated += this.OnQRGenerated;

        qrGenerator = new QRCodeGenerator();

        if (user != null) {
            this.Username = user.AccountName;
        }

        this.loginManager.StartQRAuthLoop();
    }
    private void OnQRGenerated(object sender, QRGeneratedEventArgs e) {
        this.SetQRCode(e.URL);
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

    public void LoginPressed() {
        this.loginManager.BeginLogonToUser(new OpenSteamworks.Client.Config.LoginUser(this.Username, this.Password, this.RememberPassword));
    }
}
