//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientGameServerStats
{
    // WARNING: Arguments are unknown!
    public unknown_ret RequestUserStats();  // argc: 3, index: 1, ipc args: [uint64, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserStat(bool unk1);  // argc: 5, index: 2, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserStat(double unk1, bool unk2);  // argc: 5, index: 3, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserAchievement();  // argc: 6, index: 4, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserStat(bool unk1);  // argc: 5, index: 5, ipc args: [uint64, bytes8, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserStat(double unk1, bool unk2);  // argc: 5, index: 6, ipc args: [uint64, bytes8, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateUserAvgRateStat();  // argc: 6, index: 7, ipc args: [uint64, bytes8, string, bytes4, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserAchievement();  // argc: 4, index: 8, ipc args: [uint64, bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearUserAchievement();  // argc: 4, index: 9, ipc args: [uint64, bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret StoreUserStats();  // argc: 3, index: 10, ipc args: [uint64, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetMaxStatsLoaded();  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: []
}