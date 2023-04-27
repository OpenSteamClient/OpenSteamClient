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

#ifndef MATCHMAKINGSERVERSCOMMON_H
#define MATCHMAKINGSERVERSCOMMON_H
#ifdef _WIN32
#pragma once
#endif


#define CLIENTMATCHMAKINGSERVERS_INTERFACE_VERSION "CLIENTMATCHMAKINGSERVERS_INTERFACE_VERSION001"

#define STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION_001 "SteamMatchMakingServers001"
#define STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION_002 "SteamMatchMakingServers002"


typedef enum EMatchMakingServerResponse
{
	eServerResponded = 0,
	eServerFailedToRespond,
	eNoServersListedOnMasterServer // for the Internet query type, returned in response callback if no servers of this type match
} EMatchMakingServerResponse;

typedef enum EMatchMakingType
{
	eInternetServer = 0,
	eLANServer,
	eFriendsServer,
	eFavoritesServer,
	eHistoryServer,
	eSpectatorServer,
	eInvalidServer 
} EMatchMakingType;




//-----------------------------------------------------------------------------
// Callback interfaces for server list functions (see ISteamMatchmakingServers below)
//
// The idea here is that your game code implements objects that implement these
// interfaces to receive callback notifications after calling asynchronous functions
// inside the ISteamMatchmakingServers() interface below.
//
// This is different than normal Steam callback handling due to the potentially
// large size of server lists.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Purpose: Callback interface for receiving responses after a server list refresh
// or an individual server update.
//
// Since you get these callbacks after requesting full list refreshes you will
// usually implement this interface inside an object like CServerBrowser.  If that
// object is getting destructed you should use ISteamMatchMakingServers()->CancelQuery()
// to cancel any in-progress queries so you don't get a callback into the destructed
// object and crash.
//-----------------------------------------------------------------------------
abstract_class ISteamMatchmakingServerListResponse001
{
public:
	// Server has responded ok with updated data
	virtual void ServerResponded( int iServer ) = 0; 

	// Server has failed to respond
	virtual void ServerFailedToRespond( int iServer ) = 0; 

	// A list refresh you had initiated is now 100% completed
	virtual void RefreshComplete( EMatchMakingServerResponse response ) = 0; 
};

abstract_class ISteamMatchmakingServerListResponse002
{
public:
	// Server has responded ok with updated data
	virtual void ServerResponded( HServerListRequest hRequest, int iServer ) = 0; 

	// Server has failed to respond
	virtual void ServerFailedToRespond( HServerListRequest hRequest, int iServer ) = 0; 

	// A list refresh you had initiated is now 100% completed
	virtual void RefreshComplete( HServerListRequest hRequest, EMatchMakingServerResponse response ) = 0; 
};

//Typedef to the lastest version of the interface
typedef ISteamMatchmakingServerListResponse002 ISteamMatchmakingServerListResponse;

//-----------------------------------------------------------------------------
// Purpose: Callback interface for receiving responses after pinging an individual server 
//
// These callbacks all occur in response to querying an individual server
// via the ISteamMatchmakingServers()->PingServer() call below.  If you are 
// destructing an object that implements this interface then you should call 
// ISteamMatchmakingServers()->CancelServerQuery() passing in the handle to the query
// which is in progress.  Failure to cancel in progress queries when destructing
// a callback handler may result in a crash when a callback later occurs.
//-----------------------------------------------------------------------------
abstract_class ISteamMatchmakingPingResponse
{
public:
	// Server has responded successfully and has updated data
	virtual void ServerResponded( gameserveritem_t &server ) = 0;

	// Server failed to respond to the ping request
	virtual void ServerFailedToRespond() = 0;
};


//-----------------------------------------------------------------------------
// Purpose: Callback interface for receiving responses after requesting details on
// who is playing on a particular server.
//
// These callbacks all occur in response to querying an individual server
// via the ISteamMatchmakingServers()->PlayerDetails() call below.  If you are 
// destructing an object that implements this interface then you should call 
// ISteamMatchmakingServers()->CancelServerQuery() passing in the handle to the query
// which is in progress.  Failure to cancel in progress queries when destructing
// a callback handler may result in a crash when a callback later occurs.
//-----------------------------------------------------------------------------
abstract_class ISteamMatchmakingPlayersResponse
{
public:
	// Got data on a new player on the server -- you'll get this callback once per player
	// on the server which you have requested player data on.
	virtual void AddPlayerToList( const char *pchName, int nScore, float flTimePlayed ) = 0;

	// The server failed to respond to the request for player details
	virtual void PlayersFailedToRespond() = 0;

	// The server has finished responding to the player details request 
	// (ie, you won't get anymore AddPlayerToList callbacks)
	virtual void PlayersRefreshComplete() = 0;
};


//-----------------------------------------------------------------------------
// Purpose: Callback interface for receiving responses after requesting rules
// details on a particular server.
//
// These callbacks all occur in response to querying an individual server
// via the ISteamMatchmakingServers()->ServerRules() call below.  If you are 
// destructing an object that implements this interface then you should call 
// ISteamMatchmakingServers()->CancelServerQuery() passing in the handle to the query
// which is in progress.  Failure to cancel in progress queries when destructing
// a callback handler may result in a crash when a callback later occurs.
//-----------------------------------------------------------------------------
abstract_class ISteamMatchmakingRulesResponse
{
public:
	// Got data on a rule on the server -- you'll get one of these per rule defined on
	// the server you are querying
	virtual void RulesResponded( const char *pchRule, const char *pchValue ) = 0;

	// The server failed to respond to the request for rule details
	virtual void RulesFailedToRespond() = 0;

	// The server has finished responding to the rule details request 
	// (ie, you won't get anymore RulesResponded callbacks)
	virtual void RulesRefreshComplete() = 0;
};

#endif // MATCHMAKINGSERVERSCOMMON_H
