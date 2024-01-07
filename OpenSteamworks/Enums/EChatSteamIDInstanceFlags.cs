using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Enums;

[Flags]
public enum EChatSteamIDInstanceFlags {
    k_EChatAccountInstanceMask = 0x00000FFF, // top 8 bits are flags
    k_EChatInstanceFlagClan = ( CSteamID.k_unSteamAccountInstanceMask + 1 ) >> 1,	// top bit
    k_EChatInstanceFlagLobby = ( CSteamID.k_unSteamAccountInstanceMask + 1 ) >> 2,	// next one down, etc
    k_EChatInstanceFlagMMSLobby = ( CSteamID.k_unSteamAccountInstanceMask + 1 ) >> 3,	// next one down, etc

    // Max of 8 flags
}