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

#ifndef MATCHMAKINGCOMMON_H
#define MATCHMAKINGCOMMON_H
#ifdef _WIN32
#pragma once
#endif

#include "FriendsCommon.h"



#define CLIENTMATCHMAKING_INTERFACE_VERSION "CLIENTMATCHMAKING_INTERFACE_VERSION001"

#define STEAMMATCHMAKING_INTERFACE_VERSION_001 "SteamMatchMaking001"
#define STEAMMATCHMAKING_INTERFACE_VERSION_002 "SteamMatchMaking002"
#define STEAMMATCHMAKING_INTERFACE_VERSION_003 "SteamMatchMaking003"
#define STEAMMATCHMAKING_INTERFACE_VERSION_004 "SteamMatchMaking004"
#define STEAMMATCHMAKING_INTERFACE_VERSION_005 "SteamMatchMaking005"
#define STEAMMATCHMAKING_INTERFACE_VERSION_006 "SteamMatchMaking006"
#define STEAMMATCHMAKING_INTERFACE_VERSION_007 "SteamMatchMaking007"
#define STEAMMATCHMAKING_INTERFACE_VERSION_008 "SteamMatchMaking008"
#define STEAMMATCHMAKING_INTERFACE_VERSION_009 "SteamMatchMaking009"



// lobby search filter tools
enum ELobbyComparison
{
	k_ELobbyComparisonEqualToOrLessThan = -2,
	k_ELobbyComparisonLessThan = -1,
	k_ELobbyComparisonEqual = 0,
	k_ELobbyComparisonGreaterThan = 1,
	k_ELobbyComparisonEqualToOrGreaterThan = 2,
	k_ELobbyComparisonNotEqual = 3,
};


// lobby search distance
enum ELobbyDistanceFilter
{
	k_ELobbyDistanceFilterClose,		// only lobbies in the same immediate region will be returned
	k_ELobbyDistanceFilterDefault,		// only lobbies in the same region or close, but looking further if the current region has infrequent lobby activity (the default)
	k_ELobbyDistanceFilterFar,			// for games that don't have many latency requirements, will return lobbies about half-way around the globe
	k_ELobbyDistanceFilterWorldwide,	// no filtering, will match lobbies as far as India to NY (not recommended, expect multiple seconds of latency between the clients)
};

// maximum number of characters a lobby metadata key can be
#define k_nMaxLobbyKeyLength 255

typedef int HServerQuery;
const int HSERVERQUERY_INVALID = 0xffffffff;

// game server flags
const uint32 k_unFavoriteFlagNone			= 0x00;
const uint32 k_unFavoriteFlagFavorite		= 0x01; // this game favorite entry is for the favorites list
const uint32 k_unFavoriteFlagHistory		= 0x02; // this game favorite entry is for the history list


#pragma pack( push, 8 )

//-----------------------------------------------------------------------------
// Purpose: a server was added/removed from the favorites list, you should refresh now
//-----------------------------------------------------------------------------
struct FavoritesListChangedOld_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 1 };
};

//-----------------------------------------------------------------------------
// Purpose: a server was added/removed from the favorites list, you should refresh now
//-----------------------------------------------------------------------------
struct FavoritesListChanged_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 2 };

	uint32 m_nIP; // an IP of 0 means reload the whole list, any other value means just one server
	uint32 m_nQueryPort;
	uint32 m_nConnPort;
	AppId_t m_nAppID;
	uint32 m_nFlags;
	bool m_bAdd; // true if this is adding the entry, otherwise it is a remove
};

//-----------------------------------------------------------------------------
// Purpose: Someone has invited you to join a Lobby
//			normally you don't need to do anything with this, since
//			the Steam UI will also display a '<user> has invited you to the lobby, join?' dialog
//
//			if the user outside a game chooses to join, your game will be launched with the parameter "+connect_lobby <64-bit lobby id>",
//			or with the callback GameLobbyJoinRequested_t if they're already in-game
//-----------------------------------------------------------------------------
struct LobbyInvite_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 3 };

	CSteamID m_ulSteamIDUser;	// Steam ID of the person making the invite
	CSteamID m_ulSteamIDLobby;	// Steam ID of the Lobby
	CGameID m_ulGameID;			// GameID of the Lobby
};


//-----------------------------------------------------------------------------
// Purpose: Sent on entering a lobby, or on failing to enter
//			m_EChatRoomEnterResponse will be set to k_EChatRoomEnterResponseSuccess on success,
//			or a higher value on failure (see enum EChatRoomEnterResponse)
//-----------------------------------------------------------------------------
struct LobbyEnter_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 4 };

	CSteamID m_ulSteamIDLobby;							// SteamID of the Lobby you have entered
	EChatPermission m_rgfChatPermissions;				// Permissions of the current user
	bool m_bLocked;										// If true, then only invited users may join
	EChatRoomEnterResponse m_EChatRoomEnterResponse;	// EChatRoomEnterResponse
};


//-----------------------------------------------------------------------------
// Purpose: The lobby metadata has changed
//			if m_ulSteamIDMember is the steamID of a lobby member, use GetLobbyMemberData() to access per-user details
//			if m_ulSteamIDMember == m_ulSteamIDLobby, use GetLobbyData() to access lobby metadata
//-----------------------------------------------------------------------------
struct LobbyDataUpdate_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 5 };

	CSteamID m_ulSteamIDLobby;		// steamID of the Lobby
	CSteamID m_ulSteamIDMember;		// steamID of the member whose data changed, or the room itself
	uint8 m_bSuccess;
};

//-----------------------------------------------------------------------------
// Purpose: The lobby chat room state has changed
//			this is usually sent when a user has joined or left the lobby
//-----------------------------------------------------------------------------
struct LobbyChatUpdate_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 6 };

	CSteamID m_ulSteamIDLobby;			// Lobby ID
	CSteamID m_ulSteamIDUserChanged;		// user who's status in the lobby just changed - can be recipient
	CSteamID m_ulSteamIDMakingChange;		// Chat member who made the change (different from SteamIDUserChange if kicking, muting, etc.)
										// for example, if one user kicks another from the lobby, this will be set to the id of the user who initiated the kick
	EChatMemberStateChange m_rgfChatMemberStateChange;	// bitfield of EChatMemberStateChange values
};


//-----------------------------------------------------------------------------
// Purpose: A chat message for this lobby has been sent
//			use GetLobbyChatEntry( m_iChatID ) to retrieve the contents of this message
//-----------------------------------------------------------------------------
struct LobbyChatMsg_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 7 };

	uint64 m_ulSteamIDLobby;			// the lobby id this is in
	uint64 m_ulSteamIDUser;			// steamID of the user who has sent this message
	uint8 m_eChatEntryType;			// type of message
	uint32 m_iChatID;				// index of the chat entry to lookup
};

//-----------------------------------------------------------------------------
// Purpose: There's a change of Admin in this Lobby
//-----------------------------------------------------------------------------
struct OBSOLETE_CALLBACK LobbyAdminChange_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 8 };

	CSteamID m_ulSteamIDLobby;
	CSteamID m_ulSteamIDNewAdmin;
};

//-----------------------------------------------------------------------------
// Purpose: A game created a game for all the members of the lobby to join,
//			as triggered by a SetLobbyGameServer()
//			it's up to the individual clients to take action on this; the usual
//			game behavior is to leave the lobby and connect to the specified game server
//-----------------------------------------------------------------------------
struct LobbyGameCreated_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 9 };

	CSteamID m_ulSteamIDLobby;		// the lobby we were in
	CSteamID m_ulSteamIDGameServer;	// the new game server that has been created or found for the lobby members
	uint32 m_unIP;					// IP & Port of the game server (if any)
	uint16 m_usPort;
};

//-----------------------------------------------------------------------------
// Purpose: Number of matching lobbies found
//			iterate the returned lobbies with GetLobbyByIndex(), from values 0 to m_nLobbiesMatching-1
//-----------------------------------------------------------------------------
struct LobbyMatchList_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 10 };

	uint32 m_nLobbiesMatching;		// Number of lobbies that matched search criteria and we have SteamIDs for
};


//-----------------------------------------------------------------------------
// Purpose: Called when the lobby is being forcefully closed
//			lobby details functions will no longer be updated
//-----------------------------------------------------------------------------
struct LobbyClosing_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 11 };

	CSteamID m_ulSteamIDLobby;			// Lobby
};


//-----------------------------------------------------------------------------
// Purpose: posted if a user is forcefully removed from a lobby
//			can occur if a user loses connection to Steam
//-----------------------------------------------------------------------------
struct LobbyKicked_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 12 };

	uint64 m_ulSteamIDLobby;			// Lobby
	uint64 m_ulSteamIDAdmin;			// User who kicked you - possibly the ID of the lobby itself
	uint8 m_bKickedDueToDisconnect;		// true if you were kicked from the lobby due to the user losing connection to Steam (currently always true)
};



//-----------------------------------------------------------------------------
// Purpose: Result of our request to create a Lobby
//			m_eResult == k_EResultOK on success
//			at this point, the local user may not have finishing joining this lobby;
//			game code should wait until the subsequent LobbyEnter_t callback is received
//-----------------------------------------------------------------------------
struct LobbyCreated_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 13 };
	
	EResult m_eResult;		// k_EResultOK - the lobby was successfully created
							// k_EResultNoConnection - your Steam client doesn't have a connection to the back-end
							// k_EResultTimeout - you the message to the Steam servers, but it didn't respond
							// k_EResultFail - the server responded, but with an unknown internal error
							// k_EResultAccessDenied - your game isn't set to allow lobbies, or your client does haven't rights to play the game
							// k_EResultLimitExceeded - your game client has created too many lobbies

	uint64 m_ulSteamIDLobby;		// chat room, zero if failed
};

struct RequestFriendsLobbiesResponse_t
{
	enum { k_iCallback = k_iSteamMatchmakingCallbacks + 14 };

	uint64 m_ulSteamIDFriend;
	uint64 m_ulSteamIDLobby;
	int m_cResultIndex;
	int m_cResultsTotal;
};

struct GMSQueryResult_t
{
	uint32 uServerIP;
	uint32 uServerPort;
	int32 nAuthPlayers;
};

struct PingSample_t
{
	// TODO: Reverse this struct
	#ifdef _S4N_
	int m_iPadding;
	#endif
};

#pragma pack( pop )



#endif // MATCHMAKINGCOMMON_H
