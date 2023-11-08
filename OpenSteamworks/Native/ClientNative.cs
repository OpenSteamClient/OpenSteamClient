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
    public ISteamClient020 ISteamClient;

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

    public ISteamHTMLSurface005 ISteamHTMLSurface;

    public ConCommands.ConsoleNative consoleNative;
    public IntPtr libraryStartAddress;
    public IntPtr libraryEndAddress;
    public IntPtr librarySize;

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
    [MemberNotNull(nameof(ISteamClient))]
    private void LoadEngine() {
        this.IClientEngine = this.CreateInterface<IClientEngine>("CLIENTENGINE_INTERFACE_VERSION005");
        this.ISteamClient = this.CreateInterface<ISteamClient020>("SteamClient020");
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
    [MemberNotNull(nameof(ISteamHTMLSurface))]
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
        
        this.ISteamHTMLSurface = this.ISteamClient.GetISteamHTMLSurface(this.User, this.Pipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
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

    private Dictionary<IntPtr, byte[]> hookedFunctions = new();

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

    public unsafe void HookFunction(IntPtr functionToHook, IntPtr jmpTargetFunction) {
        SteamClient.GeneralLogger.Debug("Hooking function " + functionToHook);
        WidenLibrarySecurity();
        if (!hookedFunctions.ContainsKey(functionToHook)) {
            byte[] saved_buffer = new byte[5];
            fixed (byte* p = saved_buffer)
            {
                Buffer.MemoryCopy((void*)functionToHook, p, 5, 5);
            }
            SteamClient.GeneralLogger.Debug("Copied old data");
            hookedFunctions[functionToHook] = saved_buffer;
        }

        UIntPtr src = (UIntPtr)functionToHook + 5; 
        UIntPtr dst = (UIntPtr)jmpTargetFunction;
        UIntPtr relative_offset = dst-src;

        // mov r10, addr
        var patch_mov = new byte[10] { 0x49, 0xBA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        // jmp r10
        var patch_jmp = new byte[3] { 0x41, 0xFF, 0xE2 };

        var finalPatch = new byte[13];

        fixed (byte* p = patch_mov)
        {
            Buffer.MemoryCopy(&jmpTargetFunction, p+2, 8, 8);
        }

        patch_mov.CopyTo(finalPatch, 0);
        patch_jmp.CopyTo(finalPatch, 10);

        fixed (byte* p = finalPatch)
        {
            Buffer.MemoryCopy(p, (void*)functionToHook, 13, 13);
        }

        Console.WriteLine(Convert.ToHexString(finalPatch));

        SteamClient.GeneralLogger.Debug("Overwrote function");
        RestoreLibrarySecurity();
    }

    /// <summary>
    /// Removes the hook from a previously hooked function.
    /// </summary>
    /// <param name="functionToUnhook"></param>
    public unsafe void UnhookFunction(IntPtr functionToUnhook) {
        if (!hookedFunctions.ContainsKey(functionToUnhook)) {
            throw new ArgumentException("Function has not been hooked", nameof(functionToUnhook));
        }

        WidenLibrarySecurity();

        var originalCode = hookedFunctions[functionToUnhook];
        fixed (byte* p = originalCode)
        {
            Buffer.MemoryCopy(p, (void*)functionToUnhook, 5, 5);
        }
        hookedFunctions.Remove(functionToUnhook);

        RestoreLibrarySecurity();
    }

    private static byte[] ParsePatternString(string pattern, string mask)
	{
		List<byte> patternbytes = new();

        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i] == '?') {
                patternbytes.Add(0x0);
            } else {
                patternbytes.Add((byte)pattern[i]);
            }
        }

		return patternbytes.ToArray();
	}

    private unsafe bool PatternCheck(int nOffset, byte[] arrPattern, byte* memory)
    {
        for (int i = 0; i < arrPattern.Length; i++)
        {
            if (arrPattern[i] == 0x0)
                continue;
 
            if (arrPattern[i] != memory[nOffset + i]) {
                return false;
            }
        }
 
        return true;
    }
    
    /// <summary>
    /// Finds a signature
    /// </summary>
    /// <param name="signature">The signature to find</param>
    /// <param name="signatureMask">The mask to use</param>
    /// <param name="customStart">A custom start position to use, should be in range of memoryStart</param>
    /// <returns>An absolute pointer to the start of the found signature or nullptr if not found</returns>
    public unsafe IntPtr FindSignature(string szPattern, string signatureMask, IntPtr? offset = null)
    {
        WidenLibrarySecurity();
        byte* memoryPtr;
        int memoryLen = 0;
        if(offset.HasValue)
        {
            memoryPtr = (byte*)(libraryStartAddress + offset);
        } else {
            memoryPtr = (byte*)libraryStartAddress;
        }
        memoryLen = (int)(libraryEndAddress - (IntPtr)memoryPtr);

        //Console.WriteLine("Search starts at " + (nint)memoryPtr + " and ends at " + memoryEnd + ", offset is " + offset + ", length of signature " + szPattern.Length + ", length of mask " + signatureMask.Length + ", length of memory region " + memoryLen);

        byte[] arrPattern = ParsePatternString(szPattern, signatureMask);
 
        for (int nModuleIndex = 0; nModuleIndex < memoryLen; nModuleIndex++)
        {
            if (memoryPtr[nModuleIndex] != arrPattern[0])
                continue;

            if (PatternCheck(nModuleIndex, arrPattern, memoryPtr))
            {
                RestoreLibrarySecurity();
                return (IntPtr)memoryPtr + nModuleIndex;
            }
        }

        RestoreLibrarySecurity();
        throw new Exception("Failed to find signature");
    }

    /// <summary>
    /// Finds a signature and casts it to a callable function
    /// </summary>
    /// <param name="signature">The signature to find</param>
    /// <param name="signatureMask">The mask to use</param>
    /// <param name="offset">An offset to apply to memoryStart before starting a search</param>
    /// <returns>A function to call</returns>
    public T FindSignature<T>(string signature, string signatureMask, IntPtr? offset = null) {
        var ptr = FindSignature(signature, signatureMask, offset);
        return Marshal.GetDelegateForFunctionPointer<T>(ptr);
    }

    private readonly RefCount widenedSecurityRefCount = new();
    private int previousSecurity = 0;

    /// <summary>
    /// Widens library security temporarily to allow for things that modify or read library code, like hooking functions and scanning for signatures.
    /// </summary>
    public void WidenLibrarySecurity() {
        if (widenedSecurityRefCount.Increment()) {
            if (OperatingSystem.IsLinux()) {
                previousSecurity = LinuxNative.PROT_READ + LinuxNative.PROT_EXEC + LinuxNative.PROT_WRITE;
                if (LinuxNative.mprotect(libraryStartAddress, (UIntPtr)librarySize, LinuxNative.PROT_READ + LinuxNative.PROT_WRITE + LinuxNative.PROT_EXEC) != 0) {
                    GeneralLogger.Warning("Failed to widen library security, errno: " + Marshal.GetLastWin32Error());
                }
            }
        }
    }

    /// <summary>
    /// Restores library security to what it was before a call to WidenLibrarySecurity.
    /// </summary>
    public void RestoreLibrarySecurity() {
        if (widenedSecurityRefCount.Decrement()) {
            if (OperatingSystem.IsLinux()) {
                if(LinuxNative.mprotect(libraryStartAddress, (UIntPtr)librarySize, previousSecurity) != 0) {
                    GeneralLogger.Warning("Failed to restore library security, errno: " + Marshal.GetLastWin32Error());
                }
            }
        }
    }

    public ClientNative(string clientPath, ConnectionType connectionType) {
        nativeLibHandle = NativeLibrary.Load(clientPath);
        var modules = Process.GetCurrentProcess().Modules;
        for (int i = 0; i < modules.Count; i++)
        {
            var module = modules[i];
            SteamClient.GeneralLogger.Debug("Have loaded module " + module.ModuleName + ", path: " + module.FileName);
            if (module.FileName == clientPath) {
                libraryStartAddress = module.BaseAddress;
                libraryEndAddress = libraryStartAddress + module.ModuleMemorySize;
                librarySize = module.ModuleMemorySize;
                SteamClient.GeneralLogger.Debug("Found steamclient, addresses " + libraryStartAddress + "-" + libraryEndAddress + ", len " + module.ModuleMemorySize);
            }
        }

        // DefaultSpewOutputFunc (no valid signatures for SpewOutputFunc setter)
        var func = FindSignature(SteamClient.platform.DefaultSpewOutputFuncSig, SteamClient.platform.DefaultSpewOutputFuncSigMask);
        unsafe {
            HookFunction(func, (IntPtr)(delegate* unmanaged[Cdecl]<SpewType_t, void*, SpewRetval_t>)&SpewOutputFuncHook);
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