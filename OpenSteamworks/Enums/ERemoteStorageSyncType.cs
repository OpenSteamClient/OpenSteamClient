using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum ERemoteStorageSyncType : int {
    Down = 1,
    Up = 2
}