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
using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Native.JIT;
using OpenSteamworks.Native.Platform;

namespace OpenSteamworks;
public class SteamClient
{
    // Attributes don't like non-const fields, so we can't determine this at runtime sadly
    /// <summary>
    /// The packing used for all structs
    /// On Windows: 8, On all other platforms: 4
    /// </summary>
    // public static readonly int Pack = -1;
    // static SteamClient() {
    //     if (OperatingSystem.IsWindows()) {
    //         Pack = 8;
    //     } else {
    //         Pack = 4;
    //     }
    // }

    /// <summary>
    /// The packing used for all structs
    /// On Windows: 8, On all other platforms: 4
    /// </summary>
#if _WINDOWS
    public const int Pack = 8;
#else
    public const int Pack = 4;
#endif



    [Flags]
    public enum ConnectionType {
        ExistingClient = 1 << 1,
        NewClient = 1 << 2
    }

    public ClientApps ClientApps;
    public ClientConfigStore ClientConfigStore;
    public ClientMessaging ClientMessaging;


    public CallbackManager CallbackManager { get; private set; }
    public ClientNative NativeClient;

    private string steamclientLibPath;
    internal ConnectionType connectionType;

    internal static SteamClient? instance;

    // Logging
    public static ILogger GeneralLogger { internal get; set; } = new DefaultConsoleLogger();
    /// <summary>
    /// The logger used explicitly for messages coming straight from the underlying steamclient library.
    /// </summary>
    public static ILogger NativeClientLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger CallbackLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger JITLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger ConCommandsLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger MessagingLogger { internal get; set; } = new DefaultConsoleLogger();
    /// <summary>
    /// The logger used for CUtl types
    /// </summary>
    public static ILogger CUtlLogger { internal get; set; } = new DefaultConsoleLogger();
    public static bool LogIncomingCallbacks { internal get; set; } = false;
    public static bool LogCallbackContents { internal get; set; } = false;

    internal static readonly IPlatform platform;
    private ClientAPI_WarningMessageHook_t warningMessageHook;

    delegate bool SpewOutputFunc_p(int nSeverity, string logMsg);

    static SteamClient() {
        if (OperatingSystem.IsWindows()) {
            platform = new WindowsPlatform();
        } else if (OperatingSystem.IsLinux()) {
            platform = new LinuxPlatform();
        } else {
            throw new NotSupportedException("OS unsupported");
        }
    }

    /// <summary>
    /// Constructs a OpenSteamworks.Client. 
    /// </summary>
    public SteamClient(string steamclientLibPath, ConnectionType connectionType, bool enableSpew = false)
    {
        if (instance != null) {
            throw new InvalidOperationException("A SteamClient instance has been constructed already. Free it before creating another.");
        }

        instance = this;

        warningMessageHook = (int nSeverity, string pchDebugText) =>
        {
            NativeClientLogger.Warning("[CLIENT_API WARN s:" + nSeverity + "] " + pchDebugText);
        };

        this.steamclientLibPath = steamclientLibPath;
        this.connectionType = connectionType;

        this.CallbackManager = new CallbackManager(this);
        this.NativeClient = new ClientNative(steamclientLibPath, connectionType);

        if (enableSpew) {
            for (int i = 0; i < (int)ESpewGroup.k_ESpew_ArraySize; i++)
            {
                this.NativeClient.IClientUtils.SetSpew((ESpewGroup)i, 9, 9);
            }
        }

        this.NativeClient.IClientEngine.SetWarningMessageHook(warningMessageHook);

        // Sets this process as the UI process
        // Doing this with an existing client causes the windows to disappear, and never reappear
        if (this.NativeClient.ConnectedWith == ConnectionType.NewClient) {
            RunServiceHack();
            this.NativeClient.IClientUtils.SetLauncherType(ELauncherType.Clientui);
            this.NativeClient.IClientUtils.SetCurrentUIMode(EUIMode.Normal);
            this.NativeClient.IClientUtils.SetAppIDForCurrentPipe(7);
            this.NativeClient.IClientUtils.SetClientUIProcess();
        }

        this.ClientApps = new ClientApps(this);
        this.ClientConfigStore = new ClientConfigStore(this);
        this.ClientMessaging = new ClientMessaging(this);

        // Before this, most important callbacks should be registered
        this.CallbackManager.StartThread();
    }

    /// <summary>
    /// Does trickery to allow running an external steamservice on Linux. Unused on Windows, as it's the default configuration there.
    /// You'll still need to provide your own host for the steamservice (an example is available at OpenSteamClient/OpenSteamClient/Native/serviced/main.cpp)
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
            LinuxNative.setenv($"SteamClientService_{Environment.ProcessId}", "127.0.0.1:57344", 1);
        }
    }

    public void Shutdown() {
        // Shutdown ClientInterfaces first
        this.ClientMessaging.Shutdown();
        this.ClientConfigStore.Shutdown();

        this.CallbackManager.RequestStopAndWaitForExit();
        this.NativeClient.native_Steam_ReleaseUser(this.NativeClient.Pipe, this.NativeClient.User);
        this.NativeClient.native_Steam_BReleaseSteamPipe(this.NativeClient.Pipe);
        this.NativeClient.IClientEngine.BShutdownIfAllPipesClosed();
        this.NativeClient.Unload();
        instance = null;
    }

    public void LogClientState() {
        if (this.NativeClient == null) {
            GeneralLogger.Info("NativeClient Unloaded");
            return;
        }

        GeneralLogger.Info("ConnectionType: " + this.NativeClient.ConnectedWith);

        GeneralLogger.Info("Pipe: " + this.NativeClient.Pipe);

        GeneralLogger.Info("User: " + this.NativeClient.User);

        GeneralLogger.Info("Logged on: " + this.NativeClient.IClientUser.BConnected());

        string username;
        {
            StringBuilder sb = new("", 1024);
            this.NativeClient.IClientUser.GetAccountName(sb, sb.Capacity);
            username = sb.ToString();
        }

        GeneralLogger.Info("Username: " + username);
        GeneralLogger.Info("HasCachedCredentials: " + this.NativeClient.IClientUser.BHasCachedCredentials(username));

        string token;
        {
            StringBuilder sb = new("", 1024);
            this.NativeClient.IClientUser.GetCurrentWebAuthToken(sb, (uint)sb.Capacity);
            token = sb.ToString();
        }

        
        GeneralLogger.Info("CurrentWebAuthToken: " + token);
        GeneralLogger.Info("IsAnyGameOrServiceAppRunning: " + this.NativeClient.IClientUser.BIsAnyGameOrServiceAppRunning());
        GeneralLogger.Info("NumGamesRunning: " + this.NativeClient.IClientUser.NumGamesRunning());
        GeneralLogger.Info("InstallPath: " + this.NativeClient.IClientUtils.GetInstallPath());

        EUniverse universe = this.NativeClient.IClientUtils.GetConnectedUniverse();
        GeneralLogger.Info("Universe: " + ((int)universe));
        GeneralLogger.Info("Universe (name): " + this.NativeClient.IClientEngine.GetUniverseName(universe));

        GeneralLogger.Info("SecondsSinceComputerActive: " + this.NativeClient.IClientUtils.GetSecondsSinceComputerActive());
        GeneralLogger.Info("SecondsSinceAppActive: " + this.NativeClient.IClientUtils.GetSecondsSinceAppActive());
    }
}