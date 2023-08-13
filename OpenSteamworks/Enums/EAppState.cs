using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum EAppState : UInt32
{
    k_EAppStateInvalid = 0,
    k_EAppStateUninstalled = 1,
    k_EAppStateUpdateRequired = 1 << 1,
    k_EAppStateFullyInstalled = 1 << 2,
    k_EAppStateUpdateQueued = 1 << 3,
    k_EAppStateUpdateOptional = 1 << 4,
    k_EAppStateFilesMissing = 1 << 5,
    k_EAppStateSharedOnly = 1 << 6,
    k_EAppStateFilesCorrupt = 1 << 7,
    k_EAppStateUpdateRunning = 1 << 8,
    k_EAppStateUpdatePaused = 1 << 9,
    k_EAppStateUpdateStarted = 1 << 10,
    k_EAppStateUninstalling = 1 << 11,
    k_EAppStateBackupRunning = 1 << 12,
    k_EAppStateAppRunning = 1 << 13,
    k_EAppStateComponentInUse = 1 << 14,
    k_EAppStateMovingFolder = 1 << 15,
    k_EAppStateTerminating = 1 << 16,
    k_EAppStatePrefetchingInfo = 1 << 17,
    k_EAppStatePeerServer = 1 << 18,
};