using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct AppLaunchResult_t
{
	public CGameID m_GameID;
	public EAppError m_eAppError;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
	public string m_szErrorDetail;
};
