using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks.Structs;

/// <summary>
/// Seems to be some sort of job state callback for tracking CJob:s.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct UnknownCallback4288_1040033
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4288)]
    public string unk;
};