using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Structs;

/// <summary>
/// One remote play player is 9 arguments long
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct RemotePlayPlayer_t
{
    public CSteamID m_playerID;
    public UInt64 m_guestID1;
    public UInt64 m_guestID2;
    public UInt64 m_guestID3;
    public UInt32 m_uUnk1;
}