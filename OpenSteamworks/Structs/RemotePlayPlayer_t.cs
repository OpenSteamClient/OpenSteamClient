using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RemotePlayPlayer_t
{
    public CSteamID m_playerID;
    public UInt64 m_guestID;
    public UInt64 m_ullUnk1;
    public UInt64 m_ullUnk2;
    public UInt32 m_uUnk1;
}