using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FocusedAndFocusableWindowsUpdate_t
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Unknown_t {
        public UInt32 unk1;
        public UInt32 unk2;
        public UInt32 unk3;
        public override string ToString()
        {
            return "unk1: " + unk1 + ", unk2: " + unk2 + ", unk3: " + unk3;
        }
    }

    public AppId_t currentlyFocusedAppID;
    public UInt32 unkLen1;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public UInt32[] unk1;

    public UInt32 unkLen2;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public UInt32[] unk2;

    public UInt32 unkLen3;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public UInt32[] unk3;
};