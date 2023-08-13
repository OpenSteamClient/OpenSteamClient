using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum EAppUpdateState : UInt32
{
    k_EAppUpdateStateNone = 0,
    k_EAppUpdateStateRunningUpdate = 1 << 0,
    k_EAppUpdateStateReconfiguring = 1 << 1,
    k_EAppUpdateStateValidating = 1 << 2,
    k_EAppUpdateStatePreallocating = 1 << 3,
    k_EAppUpdateStateDownloading = 1 << 4,
    k_EAppUpdateStateStaging = 1 << 5,
    k_EAppUpdateStateVerifying = 1 << 6,
    k_EAppUpdateStateCommitting = 1 << 7,
    k_EAppUpdateStateRunningScript = 1 << 8,
    k_EAppUpdateStateStopping = 1 << 9,
};