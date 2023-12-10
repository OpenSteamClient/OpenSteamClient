using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct WindowFocusChanged_t
{
    public AppId_t appid;
    public UInt32 unk1;
    public UInt32 pidOfProgram;
    public UInt8 unk3;
    public UInt8 unk4;
    public UInt8 unk5;
    public UInt8 unk6;
};