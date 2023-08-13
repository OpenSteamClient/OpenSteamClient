using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AppUpdateStateChanged_t
{
	public AppId_t m_nAppID;
	public EAppUpdateState m_eOldState;
	public EAppUpdateState m_eNewState;
};