using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;


namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
// 28 long
public unsafe struct DownloadStats_s
{
	public UInt32 currentConnectionsCount;
	public UInt64 totalDownloaded;
	public UInt64 unk1;
    public UInt32 unk2;
    public UInt32 unk3;
    public readonly override string ToString()
    {
        return string.Format("currentConnectionsCount: {0}, totalDownloaded: {1}, unk1: {2}, unk2: {3}, unk3: {4}", currentConnectionsCount, totalDownloaded, unk1, unk2, unk3);
    }
}