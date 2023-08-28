using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media.Imaging;
using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public LoginWindowViewModel(IClientUser iClientUser, TranslationManager tm) {
        this.iClientUser = iClientUser;
        this.tm = tm;
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode("https://s.team/dfgdfsgdfgdf", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        byte[] png = qrCode.GetGraphic(20);
        using (Stream stream = new MemoryStream(png)) {
            QRCode = new Avalonia.Media.Imaging.Bitmap(stream);
        }
    }

    public void OnClosed() {
        
    }

    public void RegisterPressed() {
        MessageBox.Show(tm.GetTranslationForKey("#Unsupported"), string.Format(tm.GetTranslationForKey("#LoginWindow_AccountCreationUnsupported"), "https://store.steampowered.com/join/"));
    }
    public void LoginPressed() {
        
    }
}
