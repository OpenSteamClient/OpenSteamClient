using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenSteamworks.Callbacks;
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