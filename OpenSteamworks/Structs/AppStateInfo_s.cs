using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;


namespace OpenSteamworks.Structs;

// 44 long
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppStateInfo_s {
    public uint unk1;
    public uint unk2;
    public uint unk3;
    public uint unk4;
    public uint unk5;
    public uint unk6;
    public uint unk7;
    public uint unk8;
    public uint unk9;
    public uint unk10;
    public uint unk11;

    //TODO: reverse
    // This enum contains a lot of useful stuff, like app ownership data, install data, current betas, oslists and possibly more, but we don't know it's layout (yet)
}