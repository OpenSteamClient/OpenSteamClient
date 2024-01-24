using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using ClientUI.Views;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Enums;

namespace ClientUI.ViewModels;

public class AvaloniaAppViewModel : ViewModelBase
{
    public bool IsDebug => AvaloniaApp.DebugEnabled;
    //TODO
    public bool IsSteamVRInstalled => false;
    public void ExitEventually()
    {
        AvaloniaApp.Current?.ExitEventually();
    }

    public void OpenInterfaceList() => AvaloniaApp.Current?.OpenInterfaceList();

    public void OpenSettings() => AvaloniaApp.Current?.OpenSettingsWindow();

    public void OpenLibrary()
    {
        AvaloniaApp.Current?.ActivateMainWindow();
    }

    public void OpenSteamVR()
    {
        AvaloniaApp.Container.Get<AppsManager>().LaunchApp(250820).ContinueWith((res) => {
            if (res.Result != EResult.OK) {
                Dispatcher.UIThread.Invoke(() => MessageBox.Show("Launching SteamVR failed", "Launching SteamVR failed with error " + res.Result));
            }
        });
    }
}