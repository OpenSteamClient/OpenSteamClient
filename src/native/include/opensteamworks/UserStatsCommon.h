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

#ifndef USERSTATSCOMMON_H
#define USERSTATSCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define STEAMUSERSTATS_INTERFACE_VERSION_001 "STEAMUSERSTATS_INTERFACE_VERSION001"
#define STEAMUSERSTATS_INTERFACE_VERSION_002 "STEAMUSERSTATS_INTERFACE_VERSION002"
#define STEAMUSERSTATS_INTERFACE_VERSION_003 "STEAMUSERSTATS_INTERFACE_VERSION003"
#define STEAMUSERSTATS_INTERFACE_VERSION_004 "STEAMUSERSTATS_INTERFACE_VERSION004"
#define STEAMUSERSTATS_INTERFACE_VERSION_005 "STEAMUSERSTATS_INTERFACE_VERSION005"
#define STEAMUSERSTATS_INTERFACE_VERSION_006 "STEAMUSERSTATS_INTERFACE_VERSION006"
#define STEAMUSERSTATS_INTERFACE_VERSION_007 "STEAMUSERSTATS_INTERFACE_VERSION007"
#define STEAMUSERSTATS_INTERFACE_VERSION_008 "STEAMUSERSTATS_INTERFACE_VERSION008"
#define STEAMUSERSTATS_INTERFACE_VERSION_009 "STEAMUSERSTATS_INTERFACE_VERSION009"
#define STEAMUSERSTATS_INTERFACE_VERSION_010 "STEAMUSERSTATS_INTERFACE_VERSION010"
#define STEAMUSERSTATS_INTERFACE_VERSION_011 "STEAMUSERSTATS_INTERFACE_VERSION011"

#define CLIENTUSERSTATS_INTERFACE_VERSION "CLIENTUSERSTATS_INTERFACE_VERSION002"


// size limit on stat or achievement name (UTF-8 encoded)
enum { k_cchStatNameMax = 128 };

// maximum number of bytes for a leaderboard name (UTF-8 encoded)
enum { k_cchLeaderboardNameMax = 128 };

// maximum number of details int32's storable for a single leaderboard entry
enum { k_cLeaderboardDetailsMax = 64 };

// handle to a single leaderboard
typedef uint64 SteamLeaderboard_t;

// handle to a set of downloaded entries in a leaderboard
typedef uint64 SteamLeaderboardEntries_t;

//-----------------------------------------------------------------------------
// types of user game stats fields
// WARNING: DO NOT RENUMBER EXISTING VALUES - STORED IN DATABASE
//-----------------------------------------------------------------------------
enum ESteamUserStatType
{
	k_ESteamUserStatTypeINVALID = 0,
	k_ESteamUserStatTypeINT = 1,
	k_ESteamUserStatTypeFLOAT = 2,
	// Read as FLOAT, set with count / session length
	k_ESteamUserStatTypeAVGRATE = 3,
	k_ESteamUserStatTypeACHIEVEMENTS = 4,
	k_ESteamUserStatTypeGROUPACHIEVEMENTS = 5,
};

// type of data request, when downloading leaderboard entries
enum ELeaderboardDataRequest
{
	k_ELeaderboardDataRequestGlobal = 0,
	k_ELeaderboardDataRequestGlobalAroundUser = 1,
	k_ELeaderboardDataRequestFriends = 2,
	k_ELeaderboardDataRequestUsers = 3,
};

// the display type (used by the Steam Community web site) for a leaderboard
enum ELeaderboardDisplayType
{
	k_ELeaderboardDisplayTypeNone = 0, 
	k_ELeaderboardDisplayTypeNumeric = 1,			// simple numerical score
	k_ELeaderboardDisplayTypeTimeSeconds = 2,		// the score represents a time, in seconds
	k_ELeaderboardDisplayTypeTimeMilliSeconds = 3,	// the score represents a time, in milliseconds
};

enum ELeaderboardUploadScoreMethod
{
	k_ELeaderboardUploadScoreMethodNone = 0,
	k_ELeaderboardUploadScoreMethodKeepBest = 1,	// Leaderboard will keep user's best score
	k_ELeaderboardUploadScoreMethodForceUpdate = 2,	// Leaderboard will always replace score with specified
};

// the sort order of a leaderboard
enum ELeaderboardSortMethod
{
	k_ELeaderboardSortMethodNone = 0,
	k_ELeaderboardSortMethodAscending = 1,	// top-score is lowest number
	k_ELeaderboardSortMethodDescending = 2,	// top-score is highest number
};

enum EGetAchievementIcon
{
	k_EGetAchievementIconUser = 0,
	k_EGetAchievementIconAchieved = 1,
	k_EGetAchievementIconUnachieved = 2,
};

#pragma pack( push, 8 )

// a single entry in a leaderboard, as returned by GetDownloadedLeaderboardEntry()
struct LeaderboardEntry001_t
{
	CSteamID m_steamIDUser; // user with the entry - use SteamFriends()->GetFriendPersonaName() & SteamFriends()->GetFriendAvatar() to get more info
	int32 m_nGlobalRank;	// [1..N], where N is the number of users with an entry in the leaderboard
	int32 m_nScore;			// score as set in the leaderboard
	int32 m_cDetails;		// number of int32 details available for this entry
};

struct LeaderboardEntry002_t
{
	CSteamID m_steamIDUser; // user with the entry - use SteamFriends()->GetFriendPersonaName() & SteamFriends()->GetFriendAvatar() to get more info
	int32 m_nGlobalRank;	// [1..N], where N is the number of users with an entry in the leaderboard
	int32 m_nScore;			// score as set in the leaderboard
	int32 m_cDetails;		// number of int32 details available for this entry
	UGCHandle_t m_hUGC;		// handle for UGC attached to the entry
};

typedef LeaderboardEntry002_t LeaderboardEntry_t;

//-----------------------------------------------------------------------------
// Purpose: called when the latests stats and achievements have been received
//			from the server
//-----------------------------------------------------------------------------
struct UserStatsReceived_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 1 };

	uint64		m_nGameID;		// Game these stats are for
	EResult		m_eResult;		// Success / error fetching the stats
	CSteamID	m_steamIDUser;	// The user for whom the stats are retrieved for
};


//-----------------------------------------------------------------------------
// Purpose: result of a request to store the user stats for a game
//-----------------------------------------------------------------------------
struct UserStatsStored_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 2 };

	uint64		m_nGameID;		// Game these stats are for
	EResult		m_eResult;		// success / error
};


//-----------------------------------------------------------------------------
// Purpose: result of a request to store the achievements for a game, or an 
//			"indicate progress" call. If both m_nCurProgress and m_nMaxProgress
//			are zero, that means the achievement has been fully unlocked.
//-----------------------------------------------------------------------------
struct UserAchievementStored_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 3 };

	uint64		m_nGameID;				// Game this is for
	bool		m_bGroupAchievement;	// if this is a "group" achievement
	char		m_rgchAchievementName[k_cchStatNameMax];		// name of the achievement
	uint32		m_nCurProgress;			// current progress towards the achievement
	uint32		m_nMaxProgress;			// "out of" this many
};


//-----------------------------------------------------------------------------
// Purpose: call result for finding a leaderboard, returned as a result of FindOrCreateLeaderboard() or FindLeaderboard()
//			use CCallResult<> to map this async result to a member function
//-----------------------------------------------------------------------------
struct LeaderboardFindResult_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 4 };

	SteamLeaderboard_t m_hSteamLeaderboard;	// handle to the leaderboard serarched for, 0 if no leaderboard found
	uint8 m_bLeaderboardFound;				// 0 if no leaderboard found
};


//-----------------------------------------------------------------------------
// Purpose: call result indicating scores for a leaderboard have been downloaded and are ready to be retrieved, returned as a result of DownloadLeaderboardEntries()
//			use CCallResult<> to map this async result to a member function
//-----------------------------------------------------------------------------
struct LeaderboardScoresDownloaded_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 5 };

	SteamLeaderboard_t m_hSteamLeaderboard;
	SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;	// the handle to pass into GetDownloadedLeaderboardEntries()
	int m_cEntryCount; // the number of entries downloaded
};


//-----------------------------------------------------------------------------
// Purpose: call result indicating scores has been uploaded, returned as a result of UploadLeaderboardScore()
//			use CCallResult<> to map this async result to a member function
//-----------------------------------------------------------------------------
struct LeaderboardScoreUploaded_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 6 };

	uint8 m_bSuccess;			// 1 if the call was successful
	SteamLeaderboard_t m_hSteamLeaderboard;	// the leaderboard handle that was
	int32 m_nScore;				// the score that was attempted to set
	uint8 m_bScoreChanged;		// true if the score in the leaderboard change, false if the existing score was better
	int m_nGlobalRankNew;		// the new global rank of the user in this leaderboard
	int m_nGlobalRankPrevious;	// the previous global rank of the user in this leaderboard; 0 if the user had no existing entry in the leaderboard
};

struct NumberOfCurrentPlayers_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 7 };

	uint8 m_bSuccess;			// 1 if the call was successful
	int32 m_cPlayers;			// Number of players currently playing
};

//-----------------------------------------------------------------------------
// Purpose: Callback indicating that a user's stats have been unloaded.
//  Call RequestUserStats again to access stats for this user
//-----------------------------------------------------------------------------
struct UserStatsUnloaded_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 8 };

	CSteamID	m_steamIDUser;	// User whose stats have been unloaded
};

//-----------------------------------------------------------------------------
// Purpose: Callback indicating that an achievement icon has been fetched
//-----------------------------------------------------------------------------
struct UserAchievementIconFetched_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 9 };

	CGameID		m_nGameID;				// Game this is for
	char		m_rgchAchievementName[k_cchStatNameMax];		// name of the achievement
	bool		m_bAchieved;		// Is the icon for the achieved or not achieved version?
	int			m_nIconHandle;		// Handle to the image, which can be used in ClientUtils()->GetImageRGBA(), 0 means no image is set for the achievement
};

//-----------------------------------------------------------------------------
// Purpose: Callback indicating that global achievement percentages are fetched
//-----------------------------------------------------------------------------
struct GlobalAchievementPercentagesReady_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 10 };

	uint64		m_nGameID;				// Game this is for
	EResult		m_eResult;				// Result of the operation
};

//-----------------------------------------------------------------------------
// Purpose: call result indicating UGC has been uploaded, returned as a result of SetLeaderboardUGC()
//-----------------------------------------------------------------------------
struct LeaderboardUGCSet_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 11 };

	EResult m_eResult;				// The result of the operation
	SteamLeaderboard_t m_hSteamLeaderboard;	// the leaderboard handle that was
};

//-----------------------------------------------------------------------------
// Purpose: callback indicating global stats have been received.
//	Returned as a result of RequestGlobalStats()
//-----------------------------------------------------------------------------
struct GlobalStatsReceived_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 12 };

	uint64	m_nGameID;				// Game global stats were requested for
	EResult	m_eResult;				// The result of the request
};

#pragma pack( pop )




#endif // USERSTATSCOMMON_H
