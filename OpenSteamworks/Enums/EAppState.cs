using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum EAppState : UInt32
{
    Invalid = 0,
    Uninstalled = 1,
    UpdateRequired = 1 << 1,
    FullyInstalled = 1 << 2,
    UpdateQueued = 1 << 3,
    UpdateOptional = 1 << 4,
    FilesMissing = 1 << 5,
    SharedOnly = 1 << 6,
    FilesCorrupt = 1 << 7,
    UpdateRunning = 1 << 8,
    UpdatePaused = 1 << 9,
    UpdateStarted = 1 << 10,
    Uninstalling = 1 << 11,
    BackupRunning = 1 << 12,
    AppRunning = 1 << 13,
    ComponentInUse = 1 << 14,
    MovingFolder = 1 << 15,
    Terminating = 1 << 16,
    PrefetchingInfo = 1 << 17,
    PeerServer = 1 << 18,
};