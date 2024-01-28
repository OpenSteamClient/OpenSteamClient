using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Enums;

namespace ClientUI.ViewModels;

public partial class AvaloniaAppViewModel : ViewModelBase
{
    public bool IsDebug => AvaloniaApp.DebugEnabled;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVRAvailable))]
    private bool isLoggedIn;

    public bool IsVRAvailable => IsLoggedIn && AvaloniaApp.Container.TryGet(out AppsManager? mgr) && mgr.IsAppInstalled(250820);
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
            if (res.Result != EAppUpdateError.NoError) {
                Dispatcher.UIThread.Invoke(() => MessageBox.Show("Launching SteamVR failed", "Launching SteamVR failed with error " + res.Result));
            }
        });
    }
}