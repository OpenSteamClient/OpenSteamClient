using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AppEventStateChange_t
{
	public AppId_t m_nAppID;
	public UInt32 m_eOldState;
	public UInt32 m_eNewState;
	public EAppUpdateError m_eAppError;
};