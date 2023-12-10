using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum EAppUpdateState : UInt32
{
    None = 0,
    RunningUpdate = 1 << 0,
    Reconfiguring = 1 << 1,
    Validating = 1 << 2,
    Preallocating = 1 << 3,
    Downloading = 1 << 4,
    Staging = 1 << 5,
    Verifying = 1 << 6,
    Committing = 1 << 7,
    RunningScript = 1 << 8,
    Stopping = 1 << 9,
};