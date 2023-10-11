using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct AppEventStateChange_t
{
	public AppId_t m_nAppID;
	public EAppState m_eOldState;
	public EAppState m_eNewState;
	public EAppUpdateError m_eAppError;
};