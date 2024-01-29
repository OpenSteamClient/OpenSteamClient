using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum ERemoteStoragePlatform : uint
{
    PlatformNone = 0,
    PlatformWindows = 1 << 0,
    PlatformOSX = 1 << 1,
    PlatformPS3 = 1 << 2,
    PlatformLinux = 1 << 3,
    PlatformReserved = 1 << 4,
    PlatformAll	= uint.MaxValue
};