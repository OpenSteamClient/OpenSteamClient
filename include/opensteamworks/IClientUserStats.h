//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code, comments, and all related files, projects, resources, 
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo, 
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef ICLIENTUSERSTATS_H
#define ICLIENTUSERSTATS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "UserStatsCommon.h"


abstract_class UNSAFE_INTERFACE IClientUserStats
{
public:
	// The "schema" of a Game's UserData is really defined elsewhere, and
	// the game should know it before accessing this interface. These top
	// three functions are mostly provided for iteration / testing purposes.
	// Get the number of stats fields for nGameID
	virtual uint32 GetNumStats( CGameID nGameID ) = 0;
	// Get stat name iStat in [0, GetNumStats)
	virtual const char * GetStatName( CGameID nGameID, uint32 iStat ) = 0;
	// Get type of this field
	virtual ESteamUserStatType GetStatType( CGameID nGameID, const char *pchName ) = 0;
	// Get the number of achievements for nGameID
	virtual uint32 GetNumAchievements( CGameID nGameID ) = 0;
	// Get achievement name iAchievement in [0, GetNumAchievements)
	virtual const char * GetAchievementName( CGameID nGameID, uint32 iAchievement ) = 0;

	// Ask the server to send down this user's data and achievements for nGameID
	virtual bool RequestCurrentStats( CGameID nGameID ) = 0;

	// Data accessors
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual bool GetStat( CGameID nGameID, const char *pchName, int32 *pData ) = 0;
	virtual bool GetStat( CGameID nGameID, const char *pchName, float *pData ) = 0;
#else
	virtual bool GetStat( CGameID nGameID, const char *pchName, float *pData ) = 0;
	virtual bool GetStat( CGameID nGameID, const char *pchName, int32 *pData ) = 0;
#endif

	// Set / update data
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual bool SetStat( CGameID nGameID, const char *pchName, int32 nData ) = 0;
	virtual bool SetStat( CGameID nGameID, const char *pchName, float fData ) = 0;
#else
	virtual bool SetStat( CGameID nGameID, const char *pchName, float fData ) = 0;
	virtual bool SetStat( CGameID nGameID, const char *pchName, int32 nData ) = 0;
#endif
	virtual bool UpdateAvgRateStat( CGameID nGameID, const char *pchName, float flCountThisSession, double dSessionLength ) = 0;

	// Achievement flag accessors
	virtual bool GetAchievement( CGameID nGameID, const char *pchName, bool *pbAchieved, uint32 *punUnlockTime ) = 0;
	virtual bool SetAchievement( CGameID nGameID, const char *pchName ) = 0;
	virtual bool ClearAchievement( CGameID nGameID, const char *pchName ) = 0;

	// Store the current data on the server, will get a callback when set
	// And one callback for every new achievement
	virtual bool StoreStats( CGameID nGameID ) = 0;

	// Gets the icon of the achievement, which is a handle to be used in IClientUtils::GetImageRGBA( ), or 0 if none set
	virtual int32 GetAchievementIcon( CGameID nGameID, const char *pchName, EGetAchievementIcon ) = 0;
	// Get general attributes ( display name / text, etc) for an Achievement
	virtual const char * GetAchievementDisplayAttribute( CGameID nGameID, const char *pchName, const char *pchKey, bool bUseGameLanguage ) = 0;

	// Achievement progress - triggers an AchievementProgress callback, that is all.
	// Calling this w/ N out of N progress will NOT set the achievement, the game must still do that.
	virtual bool IndicateAchievementProgress( CGameID nGameID, const char *pchName, uint32 nCurProgress, uint32 nMaxProgress ) = 0;
	
	virtual void SetMaxStatsLoaded( uint32 uMax ) = 0;

	// downloads stats for the user
	// returns a UserStatsReceived_t received when completed
	// if the other user has no stats, UserStatsReceived_t.m_eResult will be set to k_EResultFail
	// these stats won't be auto-updated; you'll need to call RequestUserStats( ) again to refresh any data
	virtual SteamAPICall_t RequestUserStats( CSteamID steamIDUser, CGameID nGameID ) = 0;

	// requests stat information for a user, usable after a successful call to RequestUserStats( )
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID nGameID, const char *pchName, int32 *pData ) = 0;
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID nGameID, const char *pchName, float *pData ) = 0;
#else
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID nGameID, const char *pchName, float *pData ) = 0;
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID nGameID, const char *pchName, int32 *pData ) = 0;
#endif
	virtual bool GetUserAchievement( CSteamID steamIDUser, CGameID nGameID, const char  *pchName, bool *pbAchieved, uint32 *punUnlockTime ) = 0;

	// Reset stats
	virtual bool ResetAllStats( CGameID nGameID, bool bAchievementsToo ) = 0;

	// asks the Steam back-end for a leaderboard by name, and will create it if it's not yet
	// This call is asynchronous, with the result returned in LeaderboardFindResult_t
	virtual SteamAPICall_t FindOrCreateLeaderboard( const char *pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType ) = 0;

	// as above, but won't create the leaderboard if it's not found
	// This call is asynchronous, with the result returned in LeaderboardFindResult_t
	virtual SteamAPICall_t FindLeaderboard( const char *pchLeaderboardName ) = 0;

	// returns the name of a leaderboard
	virtual const char * GetLeaderboardName( SteamLeaderboard_t hSteamLeaderboard ) = 0;

	// returns the total number of entries in a leaderboard, as of the last request
	virtual int32 GetLeaderboardEntryCount( SteamLeaderboard_t hSteamLeaderboard ) = 0;

	// returns the sort method of the leaderboard
	virtual ELeaderboardSortMethod GetLeaderboardSortMethod( SteamLeaderboard_t hSteamLeaderboard ) = 0;

	// returns the display type of the leaderboard
	virtual ELeaderboardDisplayType GetLeaderboardDisplayType( SteamLeaderboard_t hSteamLeaderboard ) = 0;

	// Asks the Steam back-end for a set of rows in the leaderboard.
	// This call is asynchronous, with the result returned in LeaderboardScoresDownloaded_t
	// LeaderboardScoresDownloaded_t will contain a handle to pull the results from GetDownloadedLeaderboardEntries( ) ( below)
	// You can ask for more entries than exist, and it will return as many as do exist.
	// k_ELeaderboardDataRequestGlobal requests rows in the leaderboard from the full table, with nRangeStart & nRangeEnd in the range [1, TotalEntries]
	// k_ELeaderboardDataRequestGlobalAroundUser requests rows around the current user, nRangeStart being negate
	//   e.g. DownloadLeaderboardEntries( hLeaderboard, k_ELeaderboardDataRequestGlobalAroundUser, -3, 3 ) will return 7 rows, 3 before the user, 3 after
	// k_ELeaderboardDataRequestFriends requests all the rows for friends of the current user 
	virtual SteamAPICall_t DownloadLeaderboardEntries( SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int32 nRangeStart, int32 nRangeEnd ) = 0;

	// as above, but downloads leaderboard entries for an arbitrary set of users - ELeaderboardDataRequest is k_ELeaderboardDataRequestUsers
	// if a user doesn't have a leaderboard entry, they won't be included in the result
	// a max of 100 users can be downloaded at a time, with only one outstanding call at a time
	virtual SteamAPICall_t DownloadLeaderboardEntriesForUsers( SteamLeaderboard_t hSteamLeaderboard, CSteamID *prgUsers, int32 cUsers ) = 0;

	// Returns data about a single leaderboard entry
	// use a for loop from 0 to LeaderboardScoresDownloaded_t::m_cEntryCount to get all the downloaded entries
	// e.g.
	//		void OnLeaderboardScoresDownloaded( LeaderboardScoresDownloaded_t *pLeaderboardScoresDownloaded )
	//		{
	//			for ( int32 index = 0; index < pLeaderboardScoresDownloaded->m_cEntryCount; index++ )
	//			{
	//				LeaderboardEntry_t leaderboardEntry;
	//				int32 details[3];		// we know this is how many we've stored previously
	//				GetDownloadedLeaderboardEntry( pLeaderboardScoresDownloaded->m_hSteamLeaderboardEntries, index, &leaderboardEntry, details, 3 );
	//				assert( leaderboardEntry.m_cDetails == 3 );
	//				...
	//			}
	// once you've accessed all the entries, the data will be free'd, and the SteamLeaderboardEntries_t handle will become invalid
	virtual bool GetDownloadedLeaderboardEntry( SteamLeaderboardEntries_t hSteamLeaderboardEntries, int32 index, LeaderboardEntry_t *pLeaderboardEntry, int32 pDetails[], int32 cDetailsMax ) = 0;

	// Attaches a piece of user generated content the user's entry on a leaderboard.
	// hContent is a handle to a piece of user generated content that was shared using ISteamUserRemoteStorage::FileShare().
	// This call is asynchronous, with the result returned in LeaderboardUGCSet_t.
	virtual SteamAPICall_t AttachLeaderboardUGC( SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC ) = 0;

	// Uploads a user score to the Steam back-end.
	// This call is asynchronous, with the result returned in LeaderboardScoreUploaded_t
	// If the score passed in is no better than the existing score this user has in the leaderboard, then the leaderboard will not be updated.
	// Details are extra game-defined information regarding how the user got that score
	// pScoreDetails points to an array of int32's, cScoreDetailsCount is the number of int32's in the list
	virtual SteamAPICall_t UploadLeaderboardScore( SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int32 nScore, const int32 *pScoreDetails, int32 cScoreDetailsCount ) = 0;

	// Retrieves the number of players currently playing your game ( online + offline)
	// This call is asynchronous, with the result returned in NumberOfCurrentPlayers_t
	virtual SteamAPICall_t GetNumberOfCurrentPlayers() = 0;

	virtual uint32 GetNumAchievedAchievements( CGameID nGameID ) = 0;
	virtual const char * GetLastAchievementUnlocked( CGameID nGameID ) = 0;

	// Requests that Steam fetch data on the percentage of players who have received each achievement
	// for the game globally.
	// This call is asynchronous, with the result returned in GlobalAchievementPercentagesReady_t.
	virtual SteamAPICall_t RequestGlobalAchievementPercentages( CGameID nGameID ) = 0;

	// Get the info on the most achieved achievement for the game, returns an iterator index you can use to fetch
	// the next most achieved afterwards.  Will return -1 if there is no data on achievement 
	// percentages (ie, you haven't called RequestGlobalAchievementPercentages and waited on the callback).
	virtual int32 GetMostAchievedAchievementInfo( CGameID nGameID, char *pchName, uint32 unNameBufLen, float *pflPercent, bool *pbAchieved ) = 0;

	// Get the info on the next most achieved achievement for the game. Call this after GetMostAchievedAchievementInfo or another
	// GetNextMostAchievedAchievementInfo call passing the iterator from the previous call. Returns -1 after the last
	// achievement has been iterated.
	virtual int32 GetNextMostAchievedAchievementInfo( CGameID nGameID, int32 iIteratorPrevious, char *pchName, uint32 unNameBufLen, float *pflPercent, bool *pbAchieved ) = 0;
	
	// Returns the percentage of users who have achieved the specified achievement.
	virtual bool GetAchievementAchievedPercent( CGameID nGameID, const char *pchName, float *pflPercent ) = 0;

	// Requests global stats data, which is available for stats marked as "aggregated".
	// This call is asynchronous, with the results returned in GlobalStatsReceived_t.
	// nHistoryDays specifies how many days of day-by-day history to retrieve in addition
	// to the overall totals. The limit is 60.
	virtual SteamAPICall_t RequestGlobalStats( CGameID nGameID, int32 nHistoryDays ) = 0;
	
	// Gets the lifetime totals for an aggregated stat
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual bool GetGlobalStat( CGameID nGameID, char const* pchName, int64 *pData ) = 0;
	virtual bool GetGlobalStat( CGameID nGameID, char const* pchName, double *pData ) = 0;
#else
	virtual bool GetGlobalStat( CGameID nGameID, char const* pchName, double *pData ) = 0;
	virtual bool GetGlobalStat( CGameID nGameID, char const* pchName, int64 *pData ) = 0;
#endif

	// Gets history for an aggregated stat. pData will be filled with daily values, starting with today.
	// So when called, pData[0] will be today, pData[1] will be yesterday, and pData[2] will be two days ago, 
	// etc. cubData is the size in bytes of the pubData buffer. Returns the number of 
	// elements actually set.
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual int32 GetGlobalStatHistory( CGameID nGameID, char const* pchName, int64 *pData, uint32 cubData ) = 0;
	virtual int32 GetGlobalStatHistory( CGameID nGameID, char const* pchName, double *pData, uint32 cubData ) = 0;
#else
	virtual int32 GetGlobalStatHistory( CGameID nGameID, char const* pchName, double *pData, uint32 cubData ) = 0;
	virtual int32 GetGlobalStatHistory( CGameID nGameID, char const* pchName, int64 *pData, uint32 cubData ) = 0;
#endif
};

#endif // ICLIENTUSERSTATS_H
