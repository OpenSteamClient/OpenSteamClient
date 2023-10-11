using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct DownloadScheduleChanged_t
{
	public bool m_bDownloadEnabled;
	public UInt32 unk1;
	public UInt32 unk2;
	public UInt32 m_nTotalAppsScheduled;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public AppId_t[] m_rgunAppSchedule;
};