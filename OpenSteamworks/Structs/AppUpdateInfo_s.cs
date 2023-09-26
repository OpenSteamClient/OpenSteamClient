using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppUpdateInfo_s {
    /// <summary>
    /// Unix timestamp when the download will auto start
    /// </summary>
	public RTime32 m_timeUpdateStart;
	/// <summary>
    /// Update state flags
    /// </summary>
	public EAppUpdateState m_eAppUpdateState;
	public UInt64 m_unBytesToDownload;
	public UInt64 m_unBytesDownloaded;
	public UInt64 m_unBytesToProcess;
	public UInt64 m_unBytesProcessed;
	// What is this? 
	public UInt64 m_unBytesToProcess2;
	// What is this?
	public UInt64 m_unBytesProcessed2;
	public UInt64 m_uUnk3;
	public UInt64 m_uUnk4;
	public UInt64 m_uUnk5;
	public EResult m_someError; // Some sort of flag or error var (value is 0 most of the time)
	public UInt32 m_uUnk6;
	public UInt64 m_uUnk7; // value is 4294967295 most of the time
	public UInt64 m_uUn8; // is possibly UInt32 (value is 16777216 most of the time)
	public UInt64 m_targetBuildID; // Installing buildid
	public UInt64 m_uUnk10;
	public UInt64 m_uUnk11;
	public UInt64 m_uUnk12;
	public UInt64 m_uUnk13;
	public UInt64 m_uUnk14;
	public UInt64 m_uUnk15;
	public UInt64 m_uUnk16;
}