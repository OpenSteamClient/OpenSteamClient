using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public unsafe struct AppLicensesChanged_t
{
	public bool bReloadAll;
	public UInt32 m_unAppsUpdated;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public AppId_t[] m_rgAppsUpdated;
};