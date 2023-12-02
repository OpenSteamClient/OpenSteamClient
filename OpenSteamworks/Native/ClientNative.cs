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
    public NativeLibraryEx SteamClientLib;

    /// <summary>
    /// Always defined on Windows, null on other platforms
    /// </summary>
    [SupportedOSPlatform("windows")]
    public NativeLibraryEx? Tier0Lib;
    /// <summary>
    /// Always defined on Windows, null on other platforms
    /// </summary>

    [SupportedOSPlatform("windows")]
    public NativeLibraryEx? VStdLib;
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
        this.IClientAudio = this.IClientEngine.GetIClientAudio(this.User, this.Pipe);
        this.IClientAppDisableUpdate = this.IClientEngine.GetIClientAppDisableUpdate(this.User, this.Pipe);
        this.IClientApps = this.IClientEngine.GetIClientApps(this.User, this.Pipe);
        this.IClientAppManager = this.IClientEngine.GetIClientAppManager(this.User, this.Pipe);
        this.IClientBilling = this.IClientEngine.GetIClientBilling(this.User, this.Pipe);
        this.IClientCompat = this.IClientEngine.GetIClientCompat(this.User, this.Pipe);
        this.IClientConfigStore = this.IClientEngine.GetIClientConfigStore(this.User, this.Pipe);
        this.IClientDeviceAuth = this.IClientEngine.GetIClientDeviceAuth(this.User, this.Pipe);
        this.IClientFriends = this.IClientEngine.GetIClientFriends(this.User, this.Pipe);
        this.IClientGameStats = this.IClientEngine.GetIClientGameStats(this.User, this.Pipe);
        this.IClientHTMLSurface = this.IClientEngine.GetIClientHTMLSurface(this.User, this.Pipe);
        this.IClientNetworking = this.IClientEngine.GetIClientNetworking(this.User, this.Pipe);
        this.IClientMatchmaking = this.IClientEngine.GetIClientMatchmaking(this.User, this.Pipe);
        this.IClientMusic = this.IClientEngine.GetIClientMusic(this.User, this.Pipe);
        this.IClientRemoteStorage = this.IClientEngine.GetIClientRemoteStorage(this.User, this.Pipe);
        this.IClientScreenshots = this.IClientEngine.GetIClientScreenshots(this.User, this.Pipe);
        this.IClientShader = this.IClientEngine.GetIClientShader(this.User, this.Pipe);
        this.IClientSharedConnection = this.IClientEngine.GetIClientSharedConnection(this.User, this.Pipe);
        this.IClientShortcuts = this.IClientEngine.GetIClientShortcuts(this.User, this.Pipe);
        this.IClientUnifiedMessages = this.IClientEngine.GetIClientUnifiedMessages(this.User, this.Pipe);
        this.IClientUGC = this.IClientEngine.GetIClientUGC(this.User, this.Pipe);
        this.IClientUser = this.IClientEngine.GetIClientUser(this.User, this.Pipe);
        this.IClientUserStats = this.IClientEngine.GetIClientUserStats(this.User, this.Pipe);
        this.IClientUtils = this.IClientEngine.GetIClientUtils(this.Pipe);
        this.IClientVR = this.IClientEngine.GetIClientVR(this.Pipe);
    }

    private bool TryConnectToGlobalUser() {
        HSteamUser user = this.IClientEngine.ConnectToGlobalUser(this._pipe);
        SteamClient.GeneralLogger.Debug("ConnectToGlobalUser returned " + user);
        if (user == 0) {
            return false;
        }

        this.User = user;
        return true;
    }

    private void CreateGlobalUser() {
        var oldPipe = this._pipe;
        this.User = this.IClientEngine.CreateGlobalUser(ref this._pipe);
        SteamClient.GeneralLogger.Debug("CreateGlobalUser returned " + User + " with new pipe " + this._pipe + ", old pipe was: " + oldPipe);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe SpewRetval_t SpewOutputFuncHook(SpewType_t pSeverity, void* str) {
        string? message = Marshal.PtrToStringUTF8((IntPtr)str);
        message ??= "";
        var lines = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var line in lines)
        {
            var toPrint = Regex.Replace(line, "\\[....-..-.. ..\\:..\\:..\\] +", "");
            switch (pSeverity)
            {
                case SpewType_t.SPEW_WARNING:
                    NativeClientLogger.Warning(toPrint);
                    break;
                case SpewType_t.SPEW_ERROR:
                    NativeClientLogger.Error(toPrint);
                    break;
                case SpewType_t.SPEW_ASSERT:
                    NativeClientLogger.Warning(toPrint);
                    break;
                case SpewType_t.SPEW_MESSAGE:
                case SpewType_t.SPEW_LOG:
                default:
                    NativeClientLogger.Info(toPrint);
                    break;
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
            SteamClient.GeneralLogger.Debug("Have loaded module " + module.ModuleName + ", path: " + module.FileName);
        }

        if (OperatingSystem.IsWindows()) {
            // Hook DefaultSpewOutputFunc instead of using SpewOutputFunc setter, since steam replaces it internally in some unknown conditions
            var func = Tier0Lib!.FindSignature(SteamClient.platform.DefaultSpewOutputFuncSig, SteamClient.platform.DefaultSpewOutputFuncSigMask);
            unsafe {
                Tier0Lib!.HookFunction(func, (IntPtr)(delegate* unmanaged[Cdecl]<SpewType_t, void*, SpewRetval_t>)&SpewOutputFuncHook);
            }
        } else {
            // Hook DefaultSpewOutputFunc (no valid signatures for SpewOutputFunc setter)
            var func = SteamClientLib.FindSignature(SteamClient.platform.DefaultSpewOutputFuncSig, SteamClient.platform.DefaultSpewOutputFuncSigMask);
            unsafe {
                SteamClientLib.HookFunction(func, (IntPtr)(delegate* unmanaged[Cdecl]<SpewType_t, void*, SpewRetval_t>)&SpewOutputFuncHook);
            }
        }

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
}