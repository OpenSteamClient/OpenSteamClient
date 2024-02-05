using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct DownloadScheduleChanged_t
{
	public bool m_bDownloadEnabled;
	public int m_nLastTotalAppsScheduled;
	public bool unk2;
	public int m_nTotalAppsScheduled;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public AppId_t[] m_rgunAppSchedule;
};