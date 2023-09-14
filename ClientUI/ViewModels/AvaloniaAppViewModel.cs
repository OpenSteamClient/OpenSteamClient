using System;
using System.Threading.Tasks;
using ClientUI.Views;

namespace ClientUI.ViewModels;

public class AvaloniaAppViewModel : ViewModelBase {
    public bool IsDebug => AvaloniaApp.DebugEnabled;
    //TODO
    public bool IsSteamVRInstalled => false;
    public async Task Exit() {
        // This is stupid. TODO: Pending support for "await?" to clean up.
        await (AvaloniaApp.Current == null ? Task.CompletedTask : AvaloniaApp.Current.Exit(1));
    }

    public void OpenInterfaceList() => AvaloniaApp.Current?.OpenInterfaceList();

    public void OpenSettings() => AvaloniaApp.Current?.OpenSettingsWindow();

    public void OpenLibrary()
    {
        //TODO
    }

    public void OpenSteamVR() {
        //TODO
    }
}