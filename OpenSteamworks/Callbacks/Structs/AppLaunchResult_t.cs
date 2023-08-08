using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AppLaunchResult_t
{
	public CGameID m_GameID;
	public EAppUpdateError m_eAppError;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
	public string m_szErrorDetail;
};
