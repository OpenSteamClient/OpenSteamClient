using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.JIT;

namespace OpenSteamworks.Native;

public struct HSteamPipe {
    int value;
}
public struct HSteamUser {
    int value;
}

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
    private NativeFuncs.CreateInterface native_CreateInterface;
    private NativeFuncs.Steam_CreateSteamPipe native_Steam_CreateSteamPipe;
    private NativeFuncs.Steam_ConnectToGlobalUser native_Steam_ConnectToGlobalUser;
    private NativeFuncs.Steam_CreateGlobalUser native_Steam_CreateGlobalUser;
    private NativeFuncs.Steam_CreateLocalUser native_Steam_CreateLocalUser;
    private NativeFuncs.Steam_ReleaseUser native_Steam_ReleaseUser;
    private NativeFuncs.Steam_BReleaseSteamPipe native_Steam_BReleaseSteamPipe;
    private NativeFuncs.Steam_BGetCallback native_Steam_BGetCallback;
    private NativeFuncs.Steam_FreeLastCallback native_Steam_FreeLastCallback;

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

    public ClientNative(string clientPath) {
        if (!NativeLibrary.TryLoad(clientPath, out nativeLibHandle)) {
            throw new Exception("Failed to load steamclient library.");
        }
        loadNativeFunctions();
    }

    public void Unload() {
        NativeLibrary.Free(nativeLibHandle);
    }

    // CreateInterface is common code. We should split it in the future.
    public IFaceT? CreateInterface<IFaceT>(string name, out int returnCode) where IFaceT : class {
        IntPtr returned = IntPtr.Zero;
        unsafe {
            int _returnCode = -1;

            // Why is this cast necessary...
            returned = native_CreateInterface(name, (IntPtr)(&_returnCode));

            returnCode = _returnCode;
        }

        if (returned == IntPtr.Zero) {
            return null;
        }

        try {
            return JITEngine.GenerateClass<IFaceT>(returned);
        } catch (Exception e) {
            Console.WriteLine(e);
            return null;
        }
    }
}