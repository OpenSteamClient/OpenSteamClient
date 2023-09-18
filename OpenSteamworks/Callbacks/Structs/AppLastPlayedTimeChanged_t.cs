using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct AppLastPlayedTimeChanged_t
{
	public AppId_t m_nAppID;
    public RTime32 m_lastPlayed;
};