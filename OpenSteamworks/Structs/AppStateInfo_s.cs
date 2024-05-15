using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;


namespace OpenSteamworks.Structs;

// 44 long
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppStateInfo_s {
    // Purpose unknown.
    // If we have appinfo, it's 4, otherwise 0.
    public uint unk1;
    public EAppOwnershipFlags appOwnershipFlags;
    public EAppState appState;
    public uint ownerAccountID;
    public UInt64 unk6;
    public uint lastChangeNumber;
    public uint unk8;
    public uint unk9;
    public uint unk10;
    public uint unk11;

    public override readonly string ToString()
    {
        return $"{unk1}, ownershipflags: {appOwnershipFlags}, appstate: {appState}, ownerid: {ownerAccountID}, {unk6}, change: {lastChangeNumber}, {unk8}, {unk9}, {unk10}, {unk11}";
    }
}