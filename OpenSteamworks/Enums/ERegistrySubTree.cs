using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

/// <summary>
/// I couldn't find any references to this enum in ghidra. Is this enum actually used anymore?
/// </summary>

public enum ERegistrySubTree : int
{
    News = 0,
    Apps = 1,
    Subscriptions = 2,
    GameServers = 3,
    Friends = 4,
    System = 5,
    AppOwnershipTickets = 6,
    LegacyCDKeys = 7,
};