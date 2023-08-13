using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum ERemoteStoragePlatform : UInt32
{
    k_ERemoteStoragePlatformNone = 0,
    k_ERemoteStoragePlatformWindows = (1 << 0),
    k_ERemoteStoragePlatformOSX = (1 << 1),
    k_ERemoteStoragePlatformPS3 = (1 << 2),
    k_ERemoteStoragePlatformLinux = (1 << 3),
    k_ERemoteStoragePlatformReserved = (1 << 4),
    k_ERemoteStoragePlatformAll	= UInt32.MaxValue
};