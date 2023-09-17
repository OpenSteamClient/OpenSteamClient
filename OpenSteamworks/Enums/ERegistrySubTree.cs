using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

/// <summary>
/// I couldn't find any references to this enum in ghidra. Is this enum actually used anymore?
/// </summary>

public enum ERegistrySubTree : int
{
    k_ERegistrySubTreeNews = 0,
    k_ERegistrySubTreeApps = 1,
    k_ERegistrySubTreeSubscriptions = 2,
    k_ERegistrySubTreeGameServers = 3,
    k_ERegistrySubTreeFriends = 4,
    k_ERegistrySubTreeSystem = 5,
    k_ERegistrySubTreeAppOwnershipTickets = 6,
    k_ERegistrySubTreeLegacyCDKeys = 7,
};