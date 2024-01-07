using System;

namespace OpenSteamworks;

[Flags]
public enum ConnectionType
{
    ExistingClient = 1 << 1,
    NewClient = 1 << 2,
    ExperimentalIPCClient = 1 << 3,
}