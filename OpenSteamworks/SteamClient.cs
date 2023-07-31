using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenSteamworks.Callbacks;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Native;

namespace OpenSteamworks;
public class SteamClient
{
    [Flags]
    public enum ConnectionType {
        ExistingClient,
        NewClient
    }

    // Non-native interfaces
    private List<IClientInterface> interfaces = new List<IClientInterface>();
    public ClientConfigStore ClientConfigStore;


    public CallbackManager CallbackManager { get; private set; }
    public Native.ClientNative NativeClient;

    /// <summary>
    /// Constructs a OpenSteamworks.Client. 
    /// </summary>
    public SteamClient(string steamclientLibPath, ConnectionType connectionType)
    {
        this.NativeClient = new ClientNative(steamclientLibPath, connectionType);
        var log = false;
#if DEBUG
        log = true;
#endif
        this.CallbackManager = new CallbackManager(this, log, log);

        this.ClientConfigStore = new ClientConfigStore(this.NativeClient.IClientConfigStore);
        this.interfaces.Add(this.ClientConfigStore);
    }

    public void Shutdown() {
        // Shutdown non-native interfaces first
        foreach (var iface in this.interfaces)
        {
            iface.RunShutdownTasks();
        }

        this.CallbackManager.RequestStopAndWaitForExit();
        this.NativeClient.native_Steam_ReleaseUser(this.NativeClient.pipe, this.NativeClient.user);
        this.NativeClient.native_Steam_BReleaseSteamPipe(this.NativeClient.pipe);
        this.NativeClient.IClientEngine.BShutdownIfAllPipesClosed();
        this.NativeClient.Unload();
    }

    public void LogClientState() {
        Console.WriteLine("ConnectionType: " + this.NativeClient.ConnectedWith);

        Console.WriteLine("Pipe: " + this.NativeClient.pipe);

        Console.WriteLine("User: " + this.NativeClient.user);

        Console.WriteLine("Logged on: " + this.NativeClient.IClientUser.BConnected());

        string username = "";
        using (NativeString str = NativeString.Allocate(1024)) {
            this.NativeClient.IClientUser.GetAccountName(str.c_str, str.size);
            str.CopyTo(out username);
        }

        Console.WriteLine("Username: " + username);
        Console.WriteLine("HasCachedCredentials: " + this.NativeClient.IClientUser.BHasCachedCredentials(username));

        string token = "";
        using (NativeString str = NativeString.Allocate(1024)) {
            this.NativeClient.IClientUser.GetCurrentWebAuthToken(str.c_str, str.size);
            str.CopyTo(out token);
        }

        
        Console.WriteLine("CurrentWebAuthToken: " + token);
        Console.WriteLine("IsAnyGameOrServiceAppRunning: " + this.NativeClient.IClientUser.BIsAnyGameOrServiceAppRunning());
        Console.WriteLine("NumGamesRunning: " + this.NativeClient.IClientUser.NumGamesRunning());
        Console.WriteLine("InstallPath: " + this.NativeClient.IClientUtils.GetInstallPath());

        EUniverse universe = this.NativeClient.IClientUtils.GetConnectedUniverse();
        Console.WriteLine("Universe: " + ((int)universe));
        Console.WriteLine("Universe (name): " + this.NativeClient.IClientEngine.GetUniverseName(universe));

        Console.WriteLine("SecondsSinceComputerActive: " + this.NativeClient.IClientUtils.GetSecondsSinceComputerActive());
        Console.WriteLine("SecondsSinceAppActive: " + this.NativeClient.IClientUtils.GetSecondsSinceAppActive());
    }
}