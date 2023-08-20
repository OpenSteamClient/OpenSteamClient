using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct SteamServerConnectFailure_t
{
	public EResult m_EResult;
    public bool m_bStillRetrying;
};