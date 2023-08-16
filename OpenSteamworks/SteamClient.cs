using System;
using System.Text;
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
        ExistingClient = 1 << 1,
        NewClient = 1 << 2
    }

    // Non-native interfaces
    private List<ClientInterface> clientinterfaces = new List<ClientInterface>();
    public ClientConfigStore ClientConfigStore;
    public ClientApps ClientApps;
    public ClientMessaging ClientMessaging;


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
        for (int i = 0; i < (int)ESpewGroup.k_ESpew_ArraySize; i++)
        {
            this.NativeClient.IClientUtils.SetSpew((ESpewGroup)i, 9, 9);
        }
#endif

        // Sets this process as the UI process
        // Doing this with an existing client causes the windows to disappear, and never reappear
        if (this.NativeClient.ConnectedWith == ConnectionType.NewClient) {
            RunServiceHack();
            this.NativeClient.IClientUtils.SetLauncherType(ELauncherType.k_ELauncherTypeClientui);
            this.NativeClient.IClientUtils.SetCurrentUIMode(EUIMode.k_EUIModeNormal);
            this.NativeClient.IClientUtils.SetAppIDForCurrentPipe(7);
            this.NativeClient.IClientUtils.SetClientUIProcess();
        }

        this.CallbackManager = new CallbackManager(this, log, log);
        
        this.ClientConfigStore = new ClientConfigStore(this);
        this.ClientMessaging = new ClientMessaging(this);
        this.ClientApps = new ClientApps(this);
        this.clientinterfaces.Add(this.ClientConfigStore);

        // Before this, most important callbacks should be registered
        this.CallbackManager.StartThread();
    }

    /// <summary>
    /// Does trickery to allow running an external steamservice on Linux. Unused on Windows, as it's the default configuration there.
    /// You'll still need to provide your own host for the steamservice (an example is available at Rosentti/OpenSteamClient/Native/serviced/main.cpp)
    /// </summary>
    private void RunServiceHack() {
        // TODO: find out a better way (probably in IClientNetworkingUtils) to set bIsServiceLocal to true instead of doing this
        // Currently, we create a mock steamservice.so(src/service/fakeservice.cpp) that contains all functions needed to get the steam client to init steamservice far enough.

        // 1. When the IPC server is first initializing, it calls BSetIpPortFromName. That function hard-codes Steam3Master and SteamClientService.
        // 2. If the name passed to it doesn't match either, it uses getenv to try and find it.
        // If it returns an IP:PORT that is in use, it will connect to it instead of trying to start it's own IPCServer

        // 1. When the service in-process is initializing, it tries to find SteamClientService_<thispid> with BSetIpPortFromName
        // 2. This normally fails, and makes steamclient initialize steamservice.
        // We abuse the getenv call to make it go to a locally running service instead.

        // This call sets SteamClientService_<thispid> envvar to point to 127.0.0.1:57344 (default for SteamClientService in the shared steam codebase between all steam bins),
        // thus it finds out that a service is already running and it doesn't try to init further, which would fail as we don't have a full steam service impl.
        // Is this VAC bannable?
        if (OperatingSystem.IsLinux()) {
            // C#'s SetEnvironmentVariable doesn't immediately change the environment variables. Use the native function to compensate
            [DllImport("libc")]
            static extern int setenv([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, int overwrite);

            setenv("SteamClientService_" + Environment.ProcessId, "127.0.0.1:57344", 1);
        }
    }

    public void Shutdown() {
        // Shutdown non-native interfaces first
        foreach (var iface in this.clientinterfaces)
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

        string username;
        {
            StringBuilder sb = new StringBuilder("", 1024);
            this.NativeClient.IClientUser.GetAccountName(sb, (uint)sb.Capacity);
            username = sb.ToString();
        }

        Console.WriteLine("Username: " + username);
        Console.WriteLine("HasCachedCredentials: " + this.NativeClient.IClientUser.BHasCachedCredentials(username));

        string token;
        {
            StringBuilder sb = new StringBuilder("", 1024);
            this.NativeClient.IClientUser.GetCurrentWebAuthToken(sb, (uint)sb.Capacity);
            token = sb.ToString();
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