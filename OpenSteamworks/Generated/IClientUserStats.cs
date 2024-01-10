//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUserStats
{
    public UInt32 GetNumStats(in CGameID nGameID);  // argc: 1, index: 1, ipc args: [bytes8], ipc returns: [bytes4]
    public string GetStatName(in CGameID nGameID, UInt32 iStat);  // argc: 2, index: 2, ipc args: [bytes8, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetStatType();  // argc: 2, index: 3, ipc args: [bytes8, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumAchievements();  // argc: 1, index: 4, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementName();  // argc: 2, index: 5, ipc args: [bytes8, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestCurrentStats();  // argc: 1, index: 6, ipc args: [bytes8], ipc returns: [bytes1]
    public unknown_ret GetStat(in CGameID nGameID, string pchName, ref Int32 pData);  // argc: 3, index: 7, ipc args: [bytes8, string], ipc returns: [bytes1, bytes4]
    public unknown_ret GetStat(in CGameID nGameID, string pchName, ref float pData);  // argc: 3, index: 8, ipc args: [bytes8, string], ipc returns: [bytes1, bytes4]
    public unknown_ret SetStat(in CGameID nGameID, string pchName, Int32 nData);  // argc: 3, index: 9, ipc args: [bytes8, string, bytes4], ipc returns: [bytes1]
    public unknown_ret SetStat(in CGameID nGameID, string pchName, float nData);  // argc: 3, index: 10, ipc args: [bytes8, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateAvgRateStat();  // argc: 4, index: 11, ipc args: [bytes8, string, bytes4, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievement();  // argc: 4, index: 12, ipc args: [bytes8, string], ipc returns: [bytes1, bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAchievement();  // argc: 2, index: 13, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearAchievement();  // argc: 2, index: 14, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementProgress();  // argc: 5, index: 15, ipc args: [bytes8, string], ipc returns: [bytes1, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret StoreStats();  // argc: 1, index: 16, ipc args: [bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementIcon();  // argc: 3, index: 17, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetAchievementIconURL();  // argc: 5, index: 18, ipc args: [bytes8, string, bytes4, bytes4], ipc returns: [boolean, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementDisplayAttribute();  // argc: 4, index: 19, ipc args: [bytes8, string, string, bytes1], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret IndicateAchievementProgress();  // argc: 4, index: 20, ipc args: [bytes8, string, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetMaxStatsLoaded();  // argc: 1, index: 21, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RequestUserStats();  // argc: 3, index: 22, ipc args: [uint64, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserStat(in CSteamID steamIDUser, in CGameID nGameID, string pchName, ref Int32 pData);  // argc: 5, index: 23, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserStat(in CSteamID steamIDUser, in CGameID nGameID, string pchName, ref float pData);  // argc: 5, index: 24, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserAchievement();  // argc: 6, index: 25, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserAchievementProgress();  // argc: 7, index: 26, ipc args: [uint64, bytes8, string], ipc returns: [bytes1, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ResetAllStats();  // argc: 2, index: 27, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret FindOrCreateLeaderboard();  // argc: 3, index: 28, ipc args: [string, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret FindLeaderboard();  // argc: 1, index: 29, ipc args: [string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardName();  // argc: 2, index: 30, ipc args: [bytes8], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardEntryCount();  // argc: 2, index: 31, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardSortMethod();  // argc: 2, index: 32, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardDisplayType();  // argc: 2, index: 33, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadLeaderboardEntries();  // argc: 5, index: 34, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadLeaderboardEntriesForUsers();  // argc: 4, index: 35, ipc args: [bytes8, bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadedLeaderboardEntry();  // argc: 6, index: 36, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes28, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret AttachLeaderboardUGC();  // argc: 4, index: 37, ipc args: [bytes8, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UploadLeaderboardScore();  // argc: 6, index: 38, ipc args: [bytes8, bytes4, bytes4, bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    public unknown_ret GetNumberOfCurrentPlayers();  // argc: 0, index: 39, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumAchievedAchievements();  // argc: 1, index: 40, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLastAchievementUnlocked();  // argc: 1, index: 41, ipc args: [bytes8], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMostRecentAchievementUnlocked();  // argc: 2, index: 42, ipc args: [bytes8, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestGlobalAchievementPercentages();  // argc: 1, index: 43, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMostAchievedAchievementInfo();  // argc: 5, index: 44, ipc args: [bytes8, bytes4], ipc returns: [bytes4, bytes_length_from_mem, bytes4, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNextMostAchievedAchievementInfo();  // argc: 6, index: 45, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem, bytes4, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementAchievedPercent();  // argc: 3, index: 46, ipc args: [bytes8, string], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestGlobalStats();  // argc: 2, index: 47, ipc args: [bytes8, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStat(in CGameID nGameID, string pchName, ref Int64 pData);  // argc: 3, index: 48, ipc args: [bytes8, string], ipc returns: [bytes1, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStat(in CGameID nGameID, string pchName, ref double pData);  // argc: 3, index: 49, ipc args: [bytes8, string], ipc returns: [bytes1, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStatHistory(in CGameID nGameID, string pchName, ref Int64 pData, UInt32 cubData);  // argc: 4, index: 50, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStatHistory(in CGameID nGameID, string pchName, ref double pData, UInt32 cubData);  // argc: 4, index: 51, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementProgressLimits(in CGameID nGameID, string pchName, ref Int64 pData, UInt32 cubData);  // argc: 4, index: 52, ipc args: [bytes8, string], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementProgressLimits(in CGameID nGameID, string pchName, ref double pData, UInt32 cubData);  // argc: 4, index: 53, ipc args: [bytes8, string], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BAchievementIconLoaded();  // argc: 3, index: 54, ipc args: [bytes8, string, bytes1], ipc returns: [boolean]
}