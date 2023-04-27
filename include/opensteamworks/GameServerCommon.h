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

#ifndef GAMESERVERCOMMON_H
#define GAMESERVERCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define STEAMGAMESERVER_INTERFACE_VERSION_002 "SteamGameServer002"
#define STEAMGAMESERVER_INTERFACE_VERSION_003 "SteamGameServer003"
#define STEAMGAMESERVER_INTERFACE_VERSION_004 "SteamGameServer004"
#define STEAMGAMESERVER_INTERFACE_VERSION_005 "SteamGameServer005"
#define STEAMGAMESERVER_INTERFACE_VERSION_006 "SteamGameServer006"
#define STEAMGAMESERVER_INTERFACE_VERSION_007 "SteamGameServer007"
#define STEAMGAMESERVER_INTERFACE_VERSION_008 "SteamGameServer008"
#define STEAMGAMESERVER_INTERFACE_VERSION_009 "SteamGameServer009"
#define STEAMGAMESERVER_INTERFACE_VERSION_010 "SteamGameServer010"
#define STEAMGAMESERVER_INTERFACE_VERSION_011 "SteamGameServer011"
#define STEAMGAMESERVER_INTERFACE_VERSION_012 "SteamGameServer012"


// Result codes to GSHandleClientDeny/Kick
enum EDenyReason
{
	k_EDenyInvalid = 0,
	k_EDenyInvalidVersion = 1,
	k_EDenyGeneric = 2,
	k_EDenyNotLoggedOn = 3,
	k_EDenyNoLicense = 4,
	k_EDenyCheater = 5,
	k_EDenyLoggedInElseWhere = 6,
	k_EDenyUnknownText = 7,
	k_EDenyIncompatibleAnticheat = 8,
	k_EDenyMemoryCorruption = 9,
	k_EDenyIncompatibleSoftware = 10,
	k_EDenySteamConnectionLost = 11,
	k_EDenySteamConnectionError = 12,
	k_EDenySteamResponseTimedOut = 13,
	k_EDenySteamValidationStalled = 14,
	k_EDenySteamOwnerLeftGuestUser = 15,
};


#pragma pack( push, 8 )
// client has been approved to connect to this game server
struct GSClientApprove_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 1 };

	CSteamID m_SteamID;
};


// client has been denied to connection to this game server
struct GSClientDeny_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 2 };

	CSteamID m_SteamID;

	EDenyReason m_eDenyReason;
	char m_pchOptionalText[ 128 ];
};


// request the game server should kick the user
struct GSClientKick_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 3 };

	CSteamID m_SteamID;
	EDenyReason m_eDenyReason;
};

// client has been denied to connect to this game server because of a Steam2 auth failure
struct GSClientSteam2Deny_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 4 };

	uint32 m_UserID;
	ESteamError m_eSteamError;
};

// client has been accepted by Steam2 to connect to this game server
struct GSClientSteam2Accept_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 5 };

	uint32 m_UserID;
	uint64 m_SteamID;
};

// client achievement info
struct GSClientAchievementStatus_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 6 };

	CSteamID m_SteamID;

	char m_pchAchievement[ 128 ];
	bool m_bUnlocked;
};

// GS gameplay stats info
struct GSGameplayStats_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 7 };

	EResult m_eResult;					// Result of the call

	int32	m_nRank;					// Overall rank of the server (0-based)
	uint32	m_unTotalConnects;			// Total number of clients who have ever connected to the server
	uint32	m_unTotalMinutesPlayed;		// Total number of minutes ever played on the server
};


// send as a reply to RequestUserGroupStatus()
struct GSClientGroupStatus_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 8 };

	CSteamID m_SteamIDUser;
	CSteamID m_SteamIDGroup;

	bool m_bMember;
	bool m_bOfficer;
};

// Sent as a reply to GetServerReputation()
struct GSReputation_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 9 };

	EResult	m_eResult;				// Result of the call;
	uint32	m_unReputationScore;	// The reputation score for the game server
	bool	m_bBanned;				// True if the server is banned from the Steam
	// master servers

	// The following members are only filled out if m_bBanned is true. They will all 
	// be set to zero otherwise. Master server bans are by IP so it is possible to be
	// banned even when the score is good high if there is a bad server on another port.
	// This information can be used to determine which server is bad.

	uint32	m_unBannedIP;		// The IP of the banned server
	uint16	m_usBannedPort;		// The port of the banned server
	uint64	m_ulBannedGameID;	// The game ID the banned server is serving
	uint32	m_unBanExpires;		// Time the ban expires, expressed in the Unix epoch (seconds since 1/1/1970)
};

// Sent as a reply to AssociateWithClan()
struct AssociateWithClanResult_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 10 };

	EResult	m_eResult;				// Result of the call;
};

// Sent as a reply to ComputeNewPlayerCompatibility()
struct ComputeNewPlayerCompatibilityResult_t
{
	enum { k_iCallback = k_iSteamGameServerCallbacks + 11 };

	EResult	m_eResult;				// Result of the call;
	int m_cPlayersThatDontLikeCandidate;
	int m_cPlayersThatCandidateDoesntLike;
	int m_cClanPlayersThatDontLikeCandidate;
	CSteamID m_SteamIDCandidate;
};



// received when the game server requests to be displayed as secure (VAC protected)
// m_bSecure is true if the game server should display itself as secure to users, false otherwise
struct GSPolicyResponse_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 15 };

	uint8 m_bSecure;
};
#pragma pack( pop )



#endif // GAMESERVERCOMMON_H
