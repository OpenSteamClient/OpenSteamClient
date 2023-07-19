using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal struct SteamAPICallCompleted_t {
    public SteamAPICall_t m_hAsyncCall;
    public int k_iCallback;
    public UInt32 m_cubParam;
}