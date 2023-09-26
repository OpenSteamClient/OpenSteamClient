using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct DownloadStats_s
{
	public UInt32 currentConnectionsCount;
	public UInt64 totalDownloaded;
	public UInt64 unk1;
    public UInt32 unk2;
    public UInt32 unk3;
    public UInt32 unk4;
    public UInt16 unk5;

    public override string ToString()
    {
        return string.Format("currentConnectionsCount: {0}, totalDownloaded: {1}, unk1: {2}, unk2: {3}, unk3: {4}, unk4: {5}, unk5: {6}", currentConnectionsCount, totalDownloaded, unk1, unk2, unk3, unk4, unk5);
    }
}