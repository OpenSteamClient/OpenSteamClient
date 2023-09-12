using Avalonia.Controls;
using ClientUI.Extensions;
using OpenSteamworks.Client.Managers;

namespace ClientUI.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        this.TranslatableInit();
    }

    protected override void OnClosing(WindowClosingEventArgs e) {
        App.Container.GetComponent<LoginManager>().StopQRAuthLoop();
        base.OnClosing(e);
    }

    public void ShowSecondFactorDialog(SecondFactorNeededEventArgs e) {
        SecondFactorNeededDialog dialog = new()
        {
            DataContext = new SecondFactorNeededEventArgs(e.AllowedConfirmations)
        };
        dialog.ShowDialog(this);
    }
}