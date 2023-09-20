using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;

namespace OpenSteamworks.ClientInterfaces;

public class ClientApps : ClientInterface
{
    private SteamClient client;
    public bool IsAnyGameRunning {
        get {
            return this.client.NativeClient.IClientUser.BIsAnyGameOrServiceAppRunning();
        }
    }
    public List<AppId_t> RunningApps {
        get {
            var list = new List<AppId_t>();
            for (int i = 0; i < this.client.NativeClient.IClientUser.NumGamesRunning(); i++)
            {
                CGameID gameID = this.client.NativeClient.IClientUser.GetRunningGameID(i, 0);
                list.Add(gameID.GetAppId());
            }
            return list;
        }
    }

    [CallbackListener<AppLicensesChanged_t>]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:AvoidUncalledPrivateCode")]
    private void OnAppLicensesChanged(AppLicensesChanged_t appLicensesChanged) {
        Console.WriteLine("ReloadAll: " + appLicensesChanged.bReloadAll);
        Console.WriteLine("Count: " + appLicensesChanged.m_unAppsUpdated);
        for (int i = 0; i < appLicensesChanged.m_unAppsUpdated; i++)
        {
            Console.WriteLine(i + ": " + appLicensesChanged.m_rgAppsUpdated[i]);
        }
    }

    public ClientApps(SteamClient client) : base(client)
    {
        this.client = client;
        
        foreach (var app in RunningApps)
        {
            Console.WriteLine(app + " is running");
        }
    }

    public void StopRunningApp(AppId_t app, bool force) {
        //TODO: Steam fails at killing the app fairly often, we should handle this ourselves (get PID and kill)
        this.client.NativeClient.IClientAppManager.ShutdownApp(app, force);
    }

    public bool IsAppRunning(AppId_t app) {
        return this.RunningApps.Contains(app);
    }
    
    internal override void RunShutdownTasks()
    {
        base.RunShutdownTasks();
        if (this.client.NativeClient.ConnectedWith == SteamClient.ConnectionType.NewClient) {
            if (this.IsAnyGameRunning) {
                foreach (var app in this.RunningApps)
                {
                    Console.WriteLine("killing " + app);
                    this.StopRunningApp(app, false);
                    do
                    {
                        System.Threading.Thread.Sleep(50);
                    } while (this.IsAppRunning(app));
                }
            }
        }
    }

}