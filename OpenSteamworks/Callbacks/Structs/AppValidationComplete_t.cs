using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public unsafe struct AppValidationComplete_t
{
	public AppId_t m_nAppID;
	public bool m_bFinished;

	public UInt64 m_TotalBytesValidated;
	public UInt64 m_TotalBytesFailed;
	public UInt32 m_TotalFilesValidated;
	public UInt32 m_TotalFilesFailed;
};