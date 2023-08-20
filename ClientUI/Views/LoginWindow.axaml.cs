using System.IO;
using Avalonia.Controls;
using ClientUI.Extensions;
using ClientUI.ViewModels;
using QRCoder;

namespace ClientUI.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        this.TryTranslateSelf();
        this.DataContext = new LoginWindowViewModel();
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode("https://s.team/dfgdfsgdfgdf", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        byte[] png = qrCode.GetGraphic(20);
        using (Stream stream = new MemoryStream(png)) {
            var image = new Avalonia.Media.Imaging.Bitmap(stream);
            this.Find<Image>("QRCode")!.Source = image;
        }
    }
}