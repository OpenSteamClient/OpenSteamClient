using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Native.JIT;
using OpenSteamworks.Generated;
using static OpenSteamworks.SteamClient;

namespace OpenSteamworks.Native;

struct NativeFuncs {
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate IntPtr CreateInterface(string name, IntPtr error);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate HSteamPipe Steam_CreateSteamPipe();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate HSteamUser Steam_ConnectToGlobalUser(HSteamPipe steamPipe);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate HSteamUser Steam_CreateGlobalUser(HSteamPipe steamPipe);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate HSteamUser Steam_CreateLocalUser(HSteamPipe steamPipe, EAccountType eAccountType);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Steam_ReleaseUser(HSteamPipe steamPipe, HSteamUser steamUser);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool Steam_BReleaseSteamPipe(HSteamPipe steamPipe);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool Steam_BGetCallback(HSteamPipe steamPipe, IntPtr pCallbackMsg);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void Steam_FreeLastCallback(HSteamPipe steamPipe);
}

public class ClientNative {
    private IntPtr nativeLibHandle = IntPtr.Zero;
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
    public HSteamPipe pipe { 
        get {
            return _pipe;
        }
        private set {
            _pipe = value;
        }
    }

    private HSteamUser _user;
    public HSteamUser user { 
        get {
            return _user;
        } 
        private set {
            _user = value;
        }
    }

    public ConnectionType ConnectedWith { get; private set; }

    public IClientEngine IClientEngine;

    public IClientAudio IClientAudio;
    public IClientAppDisableUpdate IClientAppDisableUpdate;
    public IClientApps IClientApps;
    public IClientAppManager IClientAppManager;
    public IClientBilling IClientBilling;
    public IClientCompat IClientCompat;
    public IClientConfigStore IClientConfigStore;
    public IClientDeviceAuth IClientDeviceAuth;
    public IClientFriends IClientFriends;
    public IClientGameStats IClientGameStats;
    public IClientHTMLSurface IClientHTMLSurface;
    public IClientNetworking IClientNetworking;
    public IClientMatchmaking IClientMatchmaking;
    public IClientMusic IClientMusic;
    public IClientRemoteStorage IClientRemoteStorage;
    public IClientScreenshots IClientScreenshots;
    public IClientShader IClientShader;
    public IClientSharedConnection IClientSharedConnection;
    public IClientShortcuts IClientShortcuts;
    public IClientUnifiedMessages IClientUnifiedMessages;
    public IClientUGC IClientUGC;
    public IClientUser IClientUser;
    public IClientUserStats IClientUserStats;
    public IClientUtils IClientUtils;
    public IClientVR IClientVR;

    public ConCommands.ConsoleNative consoleNative;

    /// <summary>
    /// Loads a native func. Throws if fails.
    /// </summary>
    /// <param name="name">Name of func to load</param>
    /// <param name="handle">Handle out to the loaded func</param>
    private void tryLoadNativeFunc<TDelegate>(string name, out TDelegate deleg) where TDelegate : Delegate {
        if (!NativeLibrary.TryGetExport(nativeLibHandle, name, out IntPtr handle)) {
            throw new Exception($"Failed to get {name}");
        }

        deleg = Marshal.GetDelegateForFunctionPointer<TDelegate>(handle);
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
        this.pipe = this.IClientEngine.CreateSteamPipe();
    }

    [MemberNotNull(nameof(IClientAudio))]
    [MemberNotNull(nameof(IClientAppDisableUpdate))]
    [MemberNotNull(nameof(IClientApps))]
    [MemberNotNull(nameof(IClientAppManager))]
    [MemberNotNull(nameof(IClientBilling))]
    [MemberNotNull(nameof(IClientCompat))]
    [MemberNotNull(nameof(IClientConfigStore))]
    [MemberNotNull(nameof(IClientDeviceAuth))]
    [MemberNotNull(nameof(IClientFriends))]
    [MemberNotNull(nameof(IClientGameStats))]
    [MemberNotNull(nameof(IClientHTMLSurface))]
    [MemberNotNull(nameof(IClientNetworking))]
    [MemberNotNull(nameof(IClientMatchmaking))]
    [MemberNotNull(nameof(IClientMusic))]
    [MemberNotNull(nameof(IClientRemoteStorage))]
    [MemberNotNull(nameof(IClientScreenshots))]
    [MemberNotNull(nameof(IClientShader))]
    [MemberNotNull(nameof(IClientSharedConnection))]
    [MemberNotNull(nameof(IClientShortcuts))]
    [MemberNotNull(nameof(IClientUnifiedMessages))]
    [MemberNotNull(nameof(IClientUGC))]
    [MemberNotNull(nameof(IClientUser))]
    [MemberNotNull(nameof(IClientUserStats))]
    [MemberNotNull(nameof(IClientUtils))]
    [MemberNotNull(nameof(IClientVR))]
    private void LoadInterfaces() {
        this.IClientAudio = this.IClientEngine.GetIClientAudio(this.user, this.pipe);
        this.IClientAppDisableUpdate = this.IClientEngine.GetIClientAppDisableUpdate(this.user, this.pipe);
        this.IClientApps = this.IClientEngine.GetIClientApps(this.user, this.pipe);
        this.IClientAppManager = this.IClientEngine.GetIClientAppManager(this.user, this.pipe);
        this.IClientBilling = this.IClientEngine.GetIClientBilling(this.user, this.pipe);
        this.IClientCompat = this.IClientEngine.GetIClientCompat(this.user, this.pipe);
        this.IClientConfigStore = this.IClientEngine.GetIClientConfigStore(this.user, this.pipe);
        this.IClientDeviceAuth = this.IClientEngine.GetIClientDeviceAuth(this.user, this.pipe);
        this.IClientFriends = this.IClientEngine.GetIClientFriends(this.user, this.pipe);
        this.IClientGameStats = this.IClientEngine.GetIClientGameStats(this.user, this.pipe);
        this.IClientHTMLSurface = this.IClientEngine.GetIClientHTMLSurface(this.user, this.pipe);
        this.IClientNetworking = this.IClientEngine.GetIClientNetworking(this.user, this.pipe);
        this.IClientMatchmaking = this.IClientEngine.GetIClientMatchmaking(this.user, this.pipe);
        this.IClientMusic = this.IClientEngine.GetIClientMusic(this.user, this.pipe);
        this.IClientRemoteStorage = this.IClientEngine.GetIClientRemoteStorage(this.user, this.pipe);
        this.IClientScreenshots = this.IClientEngine.GetIClientScreenshots(this.user, this.pipe);
        this.IClientShader = this.IClientEngine.GetIClientShader(this.user, this.pipe);
        this.IClientSharedConnection = this.IClientEngine.GetIClientSharedConnection(this.user, this.pipe);
        this.IClientShortcuts = this.IClientEngine.GetIClientShortcuts(this.user, this.pipe);
        this.IClientUnifiedMessages = this.IClientEngine.GetIClientUnifiedMessages(this.user, this.pipe);
        this.IClientUGC = this.IClientEngine.GetIClientUGC(this.user, this.pipe);
        this.IClientUser = this.IClientEngine.GetIClientUser(this.user, this.pipe);
        this.IClientUserStats = this.IClientEngine.GetIClientUserStats(this.user, this.pipe);
        this.IClientUtils = this.IClientEngine.GetIClientUtils(this.pipe);
        this.IClientVR = this.IClientEngine.GetIClientVR(this.pipe);
    }

    private bool TryConnectToGlobalUser() {
        HSteamUser user = this.IClientEngine.ConnectToGlobalUser(this._pipe);
        Console.WriteLine("ConnectToGlobalUser returned " + user);
        if (user == 0) {
            return false;
        }

        this.user = user;
        return true;
    }

    private void CreateGlobalUser() {
        this.user = this.IClientEngine.CreateGlobalUser(ref this._pipe);
    }

    public ClientNative(string clientPath, ConnectionType connectionType) {
        nativeLibHandle = NativeLibrary.Load(clientPath);

        loadNativeFunctions();
        LoadEngine();

        bool succeededConnecting = false;

        if (connectionType.HasFlag(ConnectionType.ExistingClient)) {
            succeededConnecting = TryConnectToGlobalUser();
            if (succeededConnecting) {
                ConnectedWith = ConnectionType.ExistingClient;
            }
        }

        if (!succeededConnecting && connectionType.HasFlag(ConnectionType.NewClient)) {
            CreateGlobalUser();
            if (this.user != 0) {
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
        NativeLibrary.Free(nativeLibHandle);
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
}