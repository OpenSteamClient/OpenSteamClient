using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Native.JIT;
using OpenSteamworks.Generated;
using static OpenSteamworks.SteamClient;
using System.IO;
using System.Diagnostics;
using OpenSteamworks.Protobuf;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using OpenSteamworks.Utils;
using System.Text.RegularExpressions;
using System.Runtime.Versioning;

namespace OpenSteamworks.Native;

struct NativeFuncs {
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr CreateInterface(string name, IntPtr error);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HSteamPipe Steam_CreateSteamPipe();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HSteamUser Steam_ConnectToGlobalUser(HSteamPipe steamPipe);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HSteamUser Steam_CreateGlobalUser(HSteamPipe steamPipe);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HSteamUser Steam_CreateLocalUser(HSteamPipe steamPipe, EAccountType eAccountType);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Steam_ReleaseUser(HSteamPipe steamPipe, HSteamUser steamUser);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool Steam_BReleaseSteamPipe(HSteamPipe steamPipe);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool Steam_BGetCallback(HSteamPipe steamPipe, IntPtr pCallbackMsg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Steam_FreeLastCallback(HSteamPipe steamPipe);
}

public partial class ClientNative {
    public NativeLibraryEx SteamClientLib;

    /// <summary>
    /// Always defined on Windows, null on other platforms
    /// </summary>
    [SupportedOSPlatform("windows")]
    public readonly NativeLibraryEx? Tier0Lib;
    /// <summary>
    /// Always defined on Windows, null on other platforms
    /// </summary>

    [SupportedOSPlatform("windows")]
    public readonly NativeLibraryEx? VStdLib;
    internal NativeFuncs.CreateInterface native_CreateInterface;
    internal NativeFuncs.Steam_CreateSteamPipe native_Steam_CreateSteamPipe;
    internal NativeFuncs.Steam_ConnectToGlobalUser native_Steam_ConnectToGlobalUser;
    internal NativeFuncs.Steam_CreateGlobalUser native_Steam_CreateGlobalUser;
    internal NativeFuncs.Steam_CreateLocalUser native_Steam_CreateLocalUser;
    internal NativeFuncs.Steam_ReleaseUser native_Steam_ReleaseUser;
    internal NativeFuncs.Steam_BReleaseSteamPipe native_Steam_BReleaseSteamPipe;
    internal NativeFuncs.Steam_BGetCallback native_Steam_BGetCallback;
    internal NativeFuncs.Steam_FreeLastCallback native_Steam_FreeLastCallback;

    private HSteamPipe _pipe;
    public HSteamPipe Pipe { 
        get {
            return _pipe;
        }
        private set {
            _pipe = value;
        }
    }

    private HSteamUser _user;
    public HSteamUser User { 
        get {
            return _user;
        } 
        private set {
            _user = value;
        }
    }

    public ConnectionType ConnectedWith { get; private set; }

    public IClientEngine IClientEngine { get; private set; }
    public IClientAppDisableUpdate IClientAppDisableUpdate { get; private set; }
    public IClientAppManager IClientAppManager { get; private set; }
    public IClientApps IClientApps { get; private set; }
    public IClientAudio IClientAudio { get; private set; }
    public IClientBilling IClientBilling { get; private set; }
    public IClientBluetoothManager IClientBluetoothManager { get; private set; }
    public IClientCompat IClientCompat { get; private set; }
    public IClientConfigStore IClientConfigStore { get; private set; }
    public IClientController IClientController { get; private set; }
    public IClientControllerSerialized IClientControllerSerialized { get; private set; }
    public IClientDepotBuilder IClientDepotBuilder { get; private set; }
    public IClientDeviceAuth IClientDeviceAuth { get; private set; }
    public IClientFriends IClientFriends { get; private set; }
    public IClientGameCoordinator IClientGameCoordinator { get; private set; }
    public IClientGameNotifications IClientGameNotifications { get; private set; }
    public IClientGameSearch IClientGameSearch { get; private set; }
    //public IClientGameServerInternal IClientGameServerInternal { get; private set; }
    //public IClientGameServerPacketHandler IClientGameServerPacketHandler { get; private set; }
    //public IClientGameServerStats IClientGameServerStats { get; private set; }
    public IClientGameStats IClientGameStats { get; private set; }
    public IClientHTMLSurface IClientHTMLSurface { get; private set; }
    public IClientHTTP IClientHTTP { get; private set; }
    //public IClientInstallUtils IClientInstallUtils { get; private set; }
    public IClientInventory IClientInventory { get; private set; }
    public IClientMatchmaking IClientMatchmaking { get; private set; }
    public IClientMatchmakingServers IClientMatchmakingServers { get; private set; }
    //public IClientModuleManager IClientModuleManager { get; private set; }
    public IClientMusic IClientMusic { get; private set; }
    public IClientNetworkDeviceManager IClientNetworkDeviceManager { get; private set; }
    public IClientNetworking IClientNetworking { get; private set; }
    //public IClientNetworkingMessages IClientNetworkingMessages { get; private set; }
    public IClientNetworkingSockets IClientNetworkingSockets { get; private set; }
    public IClientNetworkingSocketsSerialized IClientNetworkingSocketsSerialized { get; private set; }
    public IClientNetworkingUtils IClientNetworkingUtils { get; private set; }
    public IClientNetworkingUtilsSerialized IClientNetworkingUtilsSerialized { get; private set; }
    public IClientParentalSettings IClientParentalSettings { get; private set; }
    public IClientParties IClientParties { get; private set; }
    //public IClientProcessMonitor IClientProcessMonitor { get; private set; }
    public IClientProductBuilder IClientProductBuilder { get; private set; }
    public IClientRemoteClientManager IClientRemoteClientManager { get; private set; }
    public IClientRemotePlay IClientRemotePlay { get; private set; }
    public IClientRemoteStorage IClientRemoteStorage { get; private set; }
    public IClientScreenshots IClientScreenshots { get; private set; }
    //public IClientSecureDesktop IClientSecureDesktop { get; private set; }
    public IClientShader IClientShader { get; private set; }
    public IClientSharedConnection IClientSharedConnection { get; private set; }
    public IClientShortcuts IClientShortcuts { get; private set; }
    public IClientStreamClient IClientStreamClient { get; private set; }
    public IClientStreamLauncher IClientStreamLauncher { get; private set; }
    public IClientSystemAudioManager IClientSystemAudioManager { get; private set; }
    public IClientSystemDisplayManager IClientSystemDisplayManager { get; private set; }
    public IClientSystemDockManager IClientSystemDockManager { get; private set; }
    public IClientSystemManager IClientSystemManager { get; private set; }
    public IClientSystemPerfManager IClientSystemPerfManager { get; private set; }
    public IClientUGC IClientUGC { get; private set; }
    public IClientUnifiedMessages IClientUnifiedMessages { get; private set; }
    public IClientUser IClientUser { get; private set; }
    public IClientUserStats IClientUserStats { get; private set; }
    public IClientUtils IClientUtils { get; private set; }
    public IClientVideo IClientVideo { get; private set; }
    public IClientVR IClientVR { get; private set; }
    public IClientTimeline IClientTimeline { get; private set; }

    public ConCommands.ConsoleNative consoleNative;
    private static readonly string[] separator = new string[] { "\n" };


    /// <summary>
    /// Loads a native func. Throws if fails.
    /// </summary>
    /// <param name="name">Name of func to load</param>
    /// <param name="handle">Handle out to the loaded func</param>
    private void tryLoadNativeFunc<TDelegate>(string name, out TDelegate deleg) where TDelegate : Delegate {
        deleg = SteamClientLib.GetExport<TDelegate>(name);
    }

    [MemberNotNull(nameof(native_CreateInterface))]
    [MemberNotNull(nameof(native_Steam_CreateSteamPipe))]
    [MemberNotNull(nameof(native_Steam_ConnectToGlobalUser))]
    [MemberNotNull(nameof(native_Steam_CreateGlobalUser))]
    [MemberNotNull(nameof(native_Steam_CreateLocalUser))]
    [MemberNotNull(nameof(native_Steam_ReleaseUser))]
    [MemberNotNull(nameof(native_Steam_BReleaseSteamPipe))]
    [MemberNotNull(nameof(native_Steam_BGetCallback))]
    [MemberNotNull(nameof(native_Steam_FreeLastCallback))]
    private void loadNativeFunctions() {
        tryLoadNativeFunc("CreateInterface", out native_CreateInterface);
        tryLoadNativeFunc("Steam_CreateSteamPipe", out native_Steam_CreateSteamPipe);
        tryLoadNativeFunc("Steam_ConnectToGlobalUser", out native_Steam_ConnectToGlobalUser);
        tryLoadNativeFunc("Steam_CreateGlobalUser", out native_Steam_CreateGlobalUser);
        tryLoadNativeFunc("Steam_CreateLocalUser", out native_Steam_CreateLocalUser);
        tryLoadNativeFunc("Steam_ReleaseUser", out native_Steam_ReleaseUser);
        tryLoadNativeFunc("Steam_BReleaseSteamPipe", out native_Steam_BReleaseSteamPipe);
        tryLoadNativeFunc("Steam_BGetCallback", out native_Steam_BGetCallback);
        tryLoadNativeFunc("Steam_FreeLastCallback", out native_Steam_FreeLastCallback);
    }

    [MemberNotNull(nameof(IClientEngine))]
    private void LoadEngine() {
        this.IClientEngine = this.CreateInterface<IClientEngine>("CLIENTENGINE_INTERFACE_VERSION005");
        this.Pipe = this.IClientEngine.CreateSteamPipe();
    }

    
    [MemberNotNull(nameof(IClientAppDisableUpdate))]
    [MemberNotNull(nameof(IClientAppManager))]
    [MemberNotNull(nameof(IClientApps))]
    [MemberNotNull(nameof(IClientAudio))]
    [MemberNotNull(nameof(IClientBilling))]
    [MemberNotNull(nameof(IClientBluetoothManager))]
    [MemberNotNull(nameof(IClientCompat))]
    [MemberNotNull(nameof(IClientConfigStore))]
    [MemberNotNull(nameof(IClientController))]
    [MemberNotNull(nameof(IClientControllerSerialized))]
    [MemberNotNull(nameof(IClientDepotBuilder))]
    [MemberNotNull(nameof(IClientDeviceAuth))]
    [MemberNotNull(nameof(IClientFriends))]
    [MemberNotNull(nameof(IClientGameCoordinator))]
    [MemberNotNull(nameof(IClientGameNotifications))]
    [MemberNotNull(nameof(IClientGameSearch))]
    //[MemberNotNull(nameof(IClientGameServerInternal))]
    //[MemberNotNull(nameof(IClientGameServerPacketHandler))]
    //[MemberNotNull(nameof(IClientGameServerStats))]
    [MemberNotNull(nameof(IClientGameStats))]
    [MemberNotNull(nameof(IClientHTMLSurface))]
    [MemberNotNull(nameof(IClientHTTP))]
    //[MemberNotNull(nameof(IClientInstallUtils))]
    [MemberNotNull(nameof(IClientInventory))]
    [MemberNotNull(nameof(IClientMatchmaking))]
    [MemberNotNull(nameof(IClientMatchmakingServers))]
    //[MemberNotNull(nameof(IClientModuleManager))]
    [MemberNotNull(nameof(IClientMusic))]
    [MemberNotNull(nameof(IClientNetworkDeviceManager))]
    [MemberNotNull(nameof(IClientNetworking))]
    //[MemberNotNull(nameof(IClientNetworkingMessages))]
    [MemberNotNull(nameof(IClientNetworkingSockets))]
    [MemberNotNull(nameof(IClientNetworkingSocketsSerialized))]
    [MemberNotNull(nameof(IClientNetworkingUtils))]
    [MemberNotNull(nameof(IClientNetworkingUtilsSerialized))]
    [MemberNotNull(nameof(IClientParentalSettings))]
    [MemberNotNull(nameof(IClientParties))]
    //[MemberNotNull(nameof(IClientProcessMonitor))]
    [MemberNotNull(nameof(IClientProductBuilder))]
    [MemberNotNull(nameof(IClientRemoteClientManager))]
    [MemberNotNull(nameof(IClientRemotePlay))]
    [MemberNotNull(nameof(IClientRemoteStorage))]
    [MemberNotNull(nameof(IClientScreenshots))]
    //[MemberNotNull(nameof(IClientSecureDesktop))]
    [MemberNotNull(nameof(IClientShader))]
    [MemberNotNull(nameof(IClientSharedConnection))]
    [MemberNotNull(nameof(IClientShortcuts))]
    [MemberNotNull(nameof(IClientStreamClient))]
    [MemberNotNull(nameof(IClientStreamLauncher))]
    [MemberNotNull(nameof(IClientSystemAudioManager))]
    [MemberNotNull(nameof(IClientSystemDisplayManager))]
    [MemberNotNull(nameof(IClientSystemDockManager))]
    [MemberNotNull(nameof(IClientSystemManager))]
    [MemberNotNull(nameof(IClientSystemPerfManager))]
    [MemberNotNull(nameof(IClientUGC))]
    [MemberNotNull(nameof(IClientUnifiedMessages))]
    [MemberNotNull(nameof(IClientUser))]
    [MemberNotNull(nameof(IClientUserStats))]
    [MemberNotNull(nameof(IClientUtils))]
    [MemberNotNull(nameof(IClientVideo))]
    [MemberNotNull(nameof(IClientVR))]
    [MemberNotNull(nameof(IClientTimeline))]
    private void LoadInterfaces() {
        this.IClientAppDisableUpdate = this.IClientEngine.GetIClientAppDisableUpdate(this.User, this.Pipe);
        this.IClientAppManager = this.IClientEngine.GetIClientAppManager(this.User, this.Pipe);
        this.IClientApps = this.IClientEngine.GetIClientApps(this.User, this.Pipe);
        this.IClientAudio = this.IClientEngine.GetIClientAudio(this.User, this.Pipe);
        this.IClientBilling = this.IClientEngine.GetIClientBilling(this.User, this.Pipe);
        this.IClientBluetoothManager = this.IClientEngine.GetIClientBluetoothManager(this.Pipe);
        this.IClientCompat = this.IClientEngine.GetIClientCompat(this.User, this.Pipe);
        this.IClientConfigStore = this.IClientEngine.GetIClientConfigStore(this.User, this.Pipe);
        this.IClientController = this.IClientEngine.GetIClientController(this.Pipe);
        this.IClientControllerSerialized = this.IClientEngine.GetIClientControllerSerialized(this.Pipe);
        this.IClientDepotBuilder = this.IClientEngine.GetIClientDepotBuilder(this.User, this.Pipe);
        this.IClientDeviceAuth = this.IClientEngine.GetIClientDeviceAuth(this.User, this.Pipe);
        this.IClientFriends = this.IClientEngine.GetIClientFriends(this.User, this.Pipe);
        this.IClientGameCoordinator = this.IClientEngine.GetIClientGameCoordinator(this.User, this.Pipe);
        this.IClientGameNotifications = this.IClientEngine.GetIClientGameNotifications(this.User, this.Pipe);
        this.IClientGameSearch = this.IClientEngine.GetIClientGameSearch(this.User, this.Pipe);
        //this.IClientGameServerInternal = this.IClientEngine.GetIClientGameServerInternal(this.User, this.Pipe);
        //this.IClientGameServerPacketHandler = this.IClientEngine.GetIClientGameServerPacketHandler(this.User, this.Pipe);
        //this.IClientGameServerStats = this.IClientEngine.GetIClientGameServerStats(this.User, this.Pipe);
        this.IClientGameStats = this.IClientEngine.GetIClientGameStats(this.User, this.Pipe);
        this.IClientHTMLSurface = this.IClientEngine.GetIClientHTMLSurface(this.User, this.Pipe);
        this.IClientHTTP = this.IClientEngine.GetIClientHTTP(this.User, this.Pipe);
        //this.IClientInstallUtils = this.IClientEngine.GetIClientInstallUtils(this.Pipe);
        this.IClientInventory = this.IClientEngine.GetIClientInventory(this.User, this.Pipe);
        this.IClientMatchmaking = this.IClientEngine.GetIClientMatchmaking(this.User, this.Pipe);
        this.IClientMatchmakingServers = this.IClientEngine.GetIClientMatchmakingServers(this.User, this.Pipe);
        //this.IClientModuleManager = this.IClientEngine.GetIClientModuleManager(this.Pipe);
        this.IClientMusic = this.IClientEngine.GetIClientMusic(this.User, this.Pipe);
        this.IClientNetworkDeviceManager = this.IClientEngine.GetIClientNetworkDeviceManager(this.Pipe);
        this.IClientNetworking = this.IClientEngine.GetIClientNetworking(this.User, this.Pipe);
        //this.IClientNetworkingMessages = this.IClientEngine.GetIClientNetworkingMessages(this.User, this.Pipe);
        this.IClientNetworkingSockets = this.IClientEngine.GetIClientNetworkingSockets(this.User, this.Pipe);
        this.IClientNetworkingSocketsSerialized = this.IClientEngine.GetIClientNetworkingSocketsSerialized(this.User, this.Pipe);
        this.IClientNetworkingUtils = this.IClientEngine.GetIClientNetworkingUtils(this.Pipe);
        this.IClientNetworkingUtilsSerialized = this.IClientEngine.GetIClientNetworkingUtilsSerialized(this.Pipe);
        this.IClientParentalSettings = this.IClientEngine.GetIClientParentalSettings(this.User, this.Pipe);
        this.IClientParties = this.IClientEngine.GetIClientParties(this.User, this.Pipe);
        //this.IClientProcessMonitor = this.IClientEngine.GetIClientProcessMonitor(this.User, this.Pipe);
        this.IClientProductBuilder = this.IClientEngine.GetIClientProductBuilder(this.User, this.Pipe);
        this.IClientRemoteClientManager = this.IClientEngine.GetIClientRemoteClientManager(this.Pipe);
        this.IClientRemotePlay = this.IClientEngine.GetIClientRemotePlay(this.User, this.Pipe);
        this.IClientRemoteStorage = this.IClientEngine.GetIClientRemoteStorage(this.User, this.Pipe);
        this.IClientScreenshots = this.IClientEngine.GetIClientScreenshots(this.User, this.Pipe);
        //this.IClientSecureDesktop = this.IClientEngine.GetIClientSecureDesktop(this.Pipe);
        this.IClientShader = this.IClientEngine.GetIClientShader(this.User, this.Pipe);
        this.IClientSharedConnection = this.IClientEngine.GetIClientSharedConnection(this.User, this.Pipe);
        this.IClientShortcuts = this.IClientEngine.GetIClientShortcuts(this.User, this.Pipe);
        this.IClientStreamClient = this.IClientEngine.GetIClientStreamClient(this.User, this.Pipe);
        this.IClientStreamLauncher = this.IClientEngine.GetIClientStreamLauncher(this.User, this.Pipe);
        this.IClientSystemAudioManager = this.IClientEngine.GetIClientSystemAudioManager(this.Pipe);
        this.IClientSystemDisplayManager = this.IClientEngine.GetIClientSystemDisplayManager(this.Pipe);
        this.IClientSystemDockManager = this.IClientEngine.GetIClientSystemDockManager(this.Pipe);
        this.IClientSystemManager = this.IClientEngine.GetIClientSystemManager(this.Pipe);
        this.IClientSystemPerfManager = this.IClientEngine.GetIClientSystemPerfManager(this.Pipe);
        this.IClientUGC = this.IClientEngine.GetIClientUGC(this.User, this.Pipe);
        this.IClientUnifiedMessages = this.IClientEngine.GetIClientUnifiedMessages(this.User, this.Pipe);
        this.IClientUser = this.IClientEngine.GetIClientUser(this.User, this.Pipe);
        this.IClientUserStats = this.IClientEngine.GetIClientUserStats(this.User, this.Pipe);
        this.IClientUtils = this.IClientEngine.GetIClientUtils(this.Pipe);
        this.IClientVideo = this.IClientEngine.GetIClientVideo(this.User, this.Pipe);
        this.IClientVR = this.IClientEngine.GetIClientVR(this.Pipe);
        this.IClientTimeline = this.IClientEngine.GetIClientTimeline(this.User, this.Pipe);
    }

    private bool TryConnectToGlobalUser() {
        HSteamUser user = this.IClientEngine.ConnectToGlobalUser(this._pipe);
        Logging.GeneralLogger.Debug("ConnectToGlobalUser returned " + user);
        if (user == 0) {
            return false;
        }

        this.User = user;
        return true;
    }

    private void CreateGlobalUser() {
        var oldPipe = this._pipe;
        this.User = this.IClientEngine.CreateGlobalUser(ref this._pipe);
        Logging.GeneralLogger.Debug("CreateGlobalUser returned " + User + " with new pipe " + this._pipe + ", old pipe was: " + oldPipe);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe SpewRetval_t SpewOutputFuncHook(SpewType_t pSeverity, void* str) {
        string? message = Marshal.PtrToStringUTF8((IntPtr)str);
        message ??= string.Empty;
        if (!message.Contains('\n')) {
            Logging.NativeClientLogger.Write(message);
        } else {
            var lines = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                // Replace the time and date if one exists
                //var toPrint = TimeDateRegex().Replace(line, string.Empty);
                switch (pSeverity)
                {
                    case SpewType_t.SPEW_WARNING:
                        Logging.NativeClientLogger.Warning(line);
                        break;
                    case SpewType_t.SPEW_ERROR:
                        Logging.NativeClientLogger.Error(line);
                        break;
                    case SpewType_t.SPEW_ASSERT:
                        Logging.NativeClientLogger.Warning(line);
                        break;
                    case SpewType_t.SPEW_MESSAGE:
                    case SpewType_t.SPEW_LOG:
                    default:
                        Logging.NativeClientLogger.Info(line);
                        break;
                }
            }
        }
        
        if (pSeverity == SpewType_t.SPEW_ASSERT) {
            return SpewRetval_t.SPEW_DEBUGGER;
        }

        return SpewRetval_t.SPEW_CONTINUE;
    }

    public ClientNative(string clientPath, ConnectionType connectionType) {
        if (OperatingSystem.IsWindows()) {
            // This is fine, since steamclient.dll also imports from the same directory as itself, so this should never fail
            Tier0Lib = NativeLibraryEx.Load(Path.Combine(Path.GetDirectoryName(clientPath)!, "tier0_s64.dll"));
            VStdLib = NativeLibraryEx.Load(Path.Combine(Path.GetDirectoryName(clientPath)!, "vstdlib_s64.dll"));
        }

        SteamClientLib = NativeLibraryEx.Load(clientPath);

        var modules = Process.GetCurrentProcess().Modules;
        for (int i = 0; i < modules.Count; i++)
        {
            var module = modules[i];
            Logging.GeneralLogger.Debug("Have loaded module " + module.ModuleName + ", path: " + module.FileName);
        }

        if (OperatingSystem.IsWindows()) {
            // Hook DefaultSpewOutputFunc instead of using SpewOutputFunc setter, since steam replaces it internally in some unknown conditions
            var func = Tier0Lib!.FindSignature(SteamClient.platform.DefaultSpewOutputFuncSig, SteamClient.platform.DefaultSpewOutputFuncSigMask);
            if (func == 0) {
                Logging.GeneralLogger.Warning("Failed to find DefaultSpewOutputFunc");
            } else {
                unsafe {
                    Tier0Lib!.HookFunction(func, (IntPtr)(delegate* unmanaged[Cdecl]<SpewType_t, void*, SpewRetval_t>)&SpewOutputFuncHook);
                }
            }
        } else {
            // Hook DefaultSpewOutputFunc (no valid signatures for SpewOutputFunc setter)
            var func = SteamClientLib.FindSignature(SteamClient.platform.DefaultSpewOutputFuncSig, SteamClient.platform.DefaultSpewOutputFuncSigMask);
            if (func == 0) {
                Logging.GeneralLogger.Warning("Failed to find DefaultSpewOutputFunc");
            } else {
                unsafe {
                    SteamClientLib.HookFunction(func, (IntPtr)(delegate* unmanaged[Cdecl]<SpewType_t, void*, SpewRetval_t>)&SpewOutputFuncHook);
                }
            }
        }

        loadNativeFunctions();
        LoadEngine();

        bool succeededConnecting = false;

        if (connectionType.HasFlag(ConnectionType.ExistingClient)) {
            succeededConnecting = TryConnectToGlobalUser();
            if (succeededConnecting) {
                ConnectedWith = ConnectionType.ExistingClient;
                // No way to determine this, but let's assume yes
                SteamClient.IsIPCCrossProcess = true;
            }
        }

        if (!succeededConnecting && connectionType.HasFlag(ConnectionType.NewClient)) {
            CreateGlobalUser();
            if (this.User != 0) {
                succeededConnecting = true;
                ConnectedWith = ConnectionType.NewClient;
            }
        }

        if (!succeededConnecting) {
            throw new Exception("Failed to get a GlobalUser. Specified flags: " + connectionType);
        }
        
        LoadInterfaces();

        consoleNative = new ConCommands.ConsoleNative(this);
    }

    public void Unload() {
        SteamClientLib.Unload();
    }

    // CreateInterface is common code. We should split it in the future.
    public IFaceT CreateInterface<IFaceT>(string name) where IFaceT : class {
        IntPtr returned = IntPtr.Zero;
        int returnCode = -1;

        unsafe {
            // Why is the IntPtr cast necessary...
            returned = native_CreateInterface(name, (IntPtr)(&returnCode));
        }

        if (returned == IntPtr.Zero) {
            throw new Exception($"Call to CreateInterface({name}) resulted in NULL. Return value {returnCode}");
        }

        return JITEngine.GenerateClass<IFaceT>(returned);
    }

    [GeneratedRegex("\\[....-..-.. ..\\:..\\:..\\] +")]
    private static partial Regex TimeDateRegex();
}