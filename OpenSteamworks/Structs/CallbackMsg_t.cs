using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct NativeCallbackMsg_t {
    public HSteamUser m_hSteamUser;
	public int m_iCallback;
	public void* m_pubParam;
	public int m_cubParam;
}

public struct CallbackMsg_t {
    public HSteamUser steamUser;
	public int callbackID;
    public byte[] callbackData;

    public static readonly CallbackMsg_t Empty = new() { steamUser = 0, callbackID = 0, callbackData = Array.Empty<byte>() };
}