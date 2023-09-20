using System;
using System.Threading.Tasks;
using ClientUI.Views;

namespace ClientUI.ViewModels;

public class AvaloniaAppViewModel : ViewModelBase {
    public bool IsDebug => AvaloniaApp.DebugEnabled;
    //TODO
    public bool IsSteamVRInstalled => false;
    public void ExitEventually() {
        AvaloniaApp.Current?.ExitEventually();
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