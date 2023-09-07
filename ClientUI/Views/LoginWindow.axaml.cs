using Avalonia.Controls;
using ClientUI.Extensions;
using OpenSteamworks.Client.Managers;

namespace ClientUI.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        this.TryTranslateSelf();
    }

    protected override void OnClosing(WindowClosingEventArgs e) {
        App.Container.GetComponent<LoginManager>().StopQRAuthLoop();
        base.OnClosing(e);
    }
}