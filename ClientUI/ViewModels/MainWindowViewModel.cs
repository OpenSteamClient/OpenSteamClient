using System;
using Autofac;
using OpenSteamworks;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    public void DBG_Crash() {
        throw new Exception("test");
    }
    public void DBG_LaunchFactorio() {
        var client = App.DIContainer!.Resolve<SteamClient>();

        var gameid = new CGameID(427520);
        EAppUpdateError launchresult = client.NativeClient.IClientAppManager.LaunchApp(gameid, 3, 0, "");
        MessageBox.Show("result", launchresult.ToString());
    }
}
