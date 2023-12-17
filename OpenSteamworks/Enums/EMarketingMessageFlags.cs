using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum EMarketingMessageFlags
{
    None = 0,
    HighPriority = 1 << 0,
    PlatformWindows = 1 << 1,
    PlatformMac = 1 << 2,
    PlatformLinux = 1 << 3,
    PlatformRestrictions =
    PlatformWindows |
    PlatformMac |
    PlatformLinux,
};