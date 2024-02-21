using Avalonia.Controls;
using OpenSteamClient.Extensions;
using OpenSteamClient.ViewModels;
using OpenSteamworks.Client.Managers;

namespace OpenSteamClient.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        this.TranslatableInit();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        AvaloniaApp.Container.Get<LoginManager>().StopQRAuthLoop();
        base.OnClosing(e);
    }

    public void ShowSecondFactorDialog(SecondFactorNeededEventArgs e)
    {
        SecondFactorNeededDialog dialog = new()
        {
            DataContext = AvaloniaApp.Container.ConstructOnly<SecondFactorNeededDialogViewModel>(e),
        };
        dialog.ShowDialog(this);
    }
}