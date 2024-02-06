using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;


namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
// 28 long
public unsafe struct DownloadStats_s
{
    // "Detected write gap", "rate was"
	public UInt32 currentConnectionsCount;
	public UInt64 totalDownloaded;
    // Everytime we get a "detected write gap" this goes up by one, seemingly to a max of 2. State flags?
    // Verifying puts this at 3, where it doesn't return from 
    public uint totalAppsDownloadedThisSession;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    public byte[] unk;
    public readonly override string ToString()
    {
        return string.Format("currentConnectionsCount: {0}, totalDownloaded: {1}", currentConnectionsCount, totalDownloaded);
    }
}