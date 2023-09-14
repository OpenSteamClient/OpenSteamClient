using Avalonia.Controls;
using ClientUI.Extensions;
using ClientUI.ViewModels;
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
        AvaloniaApp.Container.GetComponent<LoginManager>().StopQRAuthLoop();
        base.OnClosing(e);
    }

    public void ShowSecondFactorDialog(SecondFactorNeededEventArgs e) {
        SecondFactorNeededDialog dialog = new()
        {
            DataContext = AvaloniaApp.Container.ConstructOnly<SecondFactorNeededDialogViewModel>(new object[1] {e}),
        };
        dialog.ShowDialog(this);
    }
}