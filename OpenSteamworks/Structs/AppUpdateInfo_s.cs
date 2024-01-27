using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;


namespace OpenSteamworks.Structs;

// This struct is 120 long
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppUpdateInfo_s {
    [Flags]
    public enum UnknownStatusEnum : uint {
        UNK1 = 1 << 1,
        ShadersOrWorkshopItem = 1 << 2, // Playable during update?
    }

    /// <summary>
    /// Unix timestamp when the download was started
    /// </summary>
	public RTime32 m_timeUpdateStart;
	/// <summary>
    /// Update state flags
    /// </summary>
	public EAppUpdateState m_eAppUpdateState;
	public ulong m_unBytesToDownload;
	public ulong m_unBytesDownloaded;
	public ulong m_unBytesToProcess;
	public ulong m_unBytesProcessed;
	public ulong m_unBytesToVerify;
	public ulong m_unBytesVerified;
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public uint[] unkArr1;
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public uint[] unkArr2;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public uint[] unkArr3;

    /// <summary>
    /// Buildid that is currently installed
    /// </summary>
    public uint m_currentBuildID;
    /// <summary>
    /// Buildid that will be installed
    /// </summary>
    public uint m_targetBuildID;
    /// <summary>
    /// If downloading a workshop item, this is the item ID of that item
    /// </summary>
    public uint downloadingWorkshopItemID;
    public byte unkBool;
    public byte someFlags;
    public byte undefined2_1; // Padding?
    public byte undefined2_2; // Padding?
    public UnknownStatusEnum m_uUnk7; // This becomes 4 when downloading workshop items, flags?
	public UInt32 m_uUnk8;
	public UInt32 m_uUnk9; 
	public UInt32 m_uUnk10;
	public UInt32 m_uUnk11;

    public override readonly string ToString()
    {
        return $"m_timeUpdateStart: {m_timeUpdateStart}, " +
               $"m_eAppUpdateState: {m_eAppUpdateState}, " +
               $"m_unBytesToDownload: {m_unBytesToDownload}, " +
               $"m_unBytesDownloaded: {m_unBytesDownloaded}, " +
               $"m_unBytesToProcess: {m_unBytesToProcess}, " +
               $"m_unBytesProcessed: {m_unBytesProcessed}, " +
               $"m_unBytesToVerify: {m_unBytesToVerify}, " +
               $"m_unBytesVerified: {m_unBytesVerified}, " +
               $"unkArr1: {string.Join(',', unkArr1)}, " +
               $"unkArr2: {string.Join(',', unkArr2)}, " +
               $"unkArr3: {string.Join(',', unkArr3)}, " +
               $"m_currentBuildID: {m_currentBuildID}, " +
               $"m_targetBuildID: {m_targetBuildID}, " +
               $"downloadingWorkshopItemID: {downloadingWorkshopItemID}, " +
               $"unkBool: {unkBool}, " +
               $"someFlags: {someFlags}, " +
               $"m_uUnk7: {(uint)m_uUnk7} ({m_uUnk7}), " +
               $"m_uUnk8: {m_uUnk8}, " +
               $"m_uUnk9: {m_uUnk8}, " +
               $"m_uUnk10: {m_uUnk10}, " +
               $"m_uUnk11: {m_uUnk11}"; 

    }
}