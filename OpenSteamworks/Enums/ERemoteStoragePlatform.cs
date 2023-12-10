using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum ERemoteStoragePlatform : UInt32
{
    PlatformNone = 0,
    PlatformWindows = (1 << 0),
    PlatformOSX = (1 << 1),
    PlatformPS3 = (1 << 2),
    PlatformLinux = (1 << 3),
    PlatformReserved = (1 << 4),
    PlatformAll	= UInt32.MaxValue
};