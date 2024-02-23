using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaCommon;
using OpenSteamClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;

namespace OpenSteamClient.ViewModels;

public partial class AvaloniaAppViewModel : AvaloniaCommon.ViewModelBase
{
    public bool IsDebug => AvaloniaApp.DebugEnabled;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVRAvailable))]
    private bool isLoggedIn;
    public bool IsVRAvailable => IsLoggedIn && AvaloniaApp.Container.TryGet(out AppsManager? mgr) && mgr.IsAppInstalled(250820);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecent5))]
    private string recent5 = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecent4))]
    private string recent4 = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecent3))]
    private string recent3 = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecent2))]
    private string recent2 = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasRecent1))]
    private string recent1 = "";


    public bool HasRecent5 => !string.IsNullOrEmpty(Recent5);
    public bool HasRecent4 => !string.IsNullOrEmpty(Recent4);
    public bool HasRecent3 => !string.IsNullOrEmpty(Recent3);
    public bool HasRecent2 => !string.IsNullOrEmpty(Recent2);
    public bool HasRecent1 => !string.IsNullOrEmpty(Recent1);

    public void PlayRecent5() { }
    public void PlayRecent4() { }
    public void PlayRecent3() { }
    public void PlayRecent2() { }
    public void PlayRecent1() { }

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

    public void OpenFriendsList()
    {
        if (AvaloniaApp.Container.TryGet(out IClientFriends? friends)) {
            friends.OpenFriendsDialog();
        }
    }

    public void OpenSteamVR()
    {
        AvaloniaApp.Container.Get<AppsManager>().LaunchApp(250820).ContinueWith((res) => {
            if (res.Result != EAppError.NoError) {
                Dispatcher.UIThread.Invoke(() => MessageBox.Show("Launching SteamVR failed", "Launching SteamVR failed with error " + res.Result));
            }
        });
    }
}