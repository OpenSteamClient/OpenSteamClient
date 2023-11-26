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
    public UInt32 GetNumStats(in CGameID nGameID);  // argc: 1, index: 1
    public string GetStatName(in CGameID nGameID, UInt32 iStat);  // argc: 2, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret GetStatType();  // argc: 2, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumAchievements();  // argc: 1, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementName();  // argc: 2, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret RequestCurrentStats();  // argc: 1, index: 6
    public unknown_ret GetStat(in CGameID nGameID, string pchName, ref Int32 pData);  // argc: 3, index: 7
    public unknown_ret GetStat(in CGameID nGameID, string pchName, ref float pData);  // argc: 3, index: 8
    public unknown_ret SetStat(in CGameID nGameID, string pchName, Int32 nData);  // argc: 3, index: 9
    public unknown_ret SetStat(in CGameID nGameID, string pchName, float nData);  // argc: 3, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateAvgRateStat();  // argc: 4, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievement();  // argc: 4, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret SetAchievement();  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret ClearAchievement();  // argc: 2, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementProgress();  // argc: 5, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret StoreStats();  // argc: 1, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementIcon();  // argc: 3, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret BGetAchievementIconURL();  // argc: 5, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementDisplayAttribute();  // argc: 4, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret IndicateAchievementProgress();  // argc: 4, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret SetMaxStatsLoaded();  // argc: 1, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret RequestUserStats();  // argc: 3, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserStat(in CSteamID steamIDUser, in CGameID nGameID, string pchName, ref Int32 pData);  // argc: 5, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserStat(in CSteamID steamIDUser, in CGameID nGameID, string pchName, ref float pData);  // argc: 5, index: 24
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserAchievement();  // argc: 6, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserAchievementProgress();  // argc: 7, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret ResetAllStats();  // argc: 2, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret FindOrCreateLeaderboard();  // argc: 3, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret FindLeaderboard();  // argc: 1, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardName();  // argc: 2, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardEntryCount();  // argc: 2, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardSortMethod();  // argc: 2, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret GetLeaderboardDisplayType();  // argc: 2, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadLeaderboardEntries();  // argc: 5, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadLeaderboardEntriesForUsers();  // argc: 4, index: 35
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadedLeaderboardEntry();  // argc: 6, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret AttachLeaderboardUGC();  // argc: 4, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret UploadLeaderboardScore();  // argc: 6, index: 38
    public unknown_ret GetNumberOfCurrentPlayers();  // argc: 0, index: 39
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumAchievedAchievements();  // argc: 1, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret GetLastAchievementUnlocked();  // argc: 1, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret GetMostRecentAchievementUnlocked();  // argc: 2, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret RequestGlobalAchievementPercentages();  // argc: 1, index: 43
    // WARNING: Arguments are unknown!
    public unknown_ret GetMostAchievedAchievementInfo();  // argc: 5, index: 44
    // WARNING: Arguments are unknown!
    public unknown_ret GetNextMostAchievedAchievementInfo();  // argc: 6, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementAchievedPercent();  // argc: 3, index: 46
    // WARNING: Arguments are unknown!
    public unknown_ret RequestGlobalStats();  // argc: 2, index: 47
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStat(in CGameID nGameID, string pchName, ref Int64 pData);  // argc: 3, index: 48
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStat(in CGameID nGameID, string pchName, ref double pData);  // argc: 3, index: 49
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStatHistory(in CGameID nGameID, string pchName, ref Int64 pData, UInt32 cubData);  // argc: 4, index: 50
    // WARNING: Arguments are unknown!
    public unknown_ret GetGlobalStatHistory(in CGameID nGameID, string pchName, ref double pData, UInt32 cubData);  // argc: 4, index: 51
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementProgressLimits(in CGameID nGameID, string pchName, ref Int64 pData, UInt32 cubData);  // argc: 4, index: 52
    // WARNING: Arguments are unknown!
    public unknown_ret GetAchievementProgressLimits(in CGameID nGameID, string pchName, ref double pData, UInt32 cubData);  // argc: 4, index: 53
    // WARNING: Arguments are unknown!
    public unknown_ret BAchievementIconLoaded();  // argc: 3, index: 14
}