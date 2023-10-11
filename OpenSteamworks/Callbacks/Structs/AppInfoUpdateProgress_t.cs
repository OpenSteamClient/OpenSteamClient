using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct AppInfoUpdateProgress_t
{
	public AppId_t m_nAppID;
};