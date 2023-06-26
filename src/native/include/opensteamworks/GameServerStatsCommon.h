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

#ifndef GAMESERVERSTATSCOMMON_H
#define GAMESERVERSTATSCOMMON_H
#ifdef _WIN32
#pragma once
#endif

#define CLIENTGAMESERVERSTATS_INTERFACE_VERSION "CLIENTGAMESERVERSTATS_INTERFACE_VERSION001"

#define STEAMGAMESERVERSTATS_INTERFACE_VERSION_001 "SteamGameServerStats001"

#pragma pack( push, 8 )
//-----------------------------------------------------------------------------
// Purpose: called when the latests stats and achievements have been received
//			from the server
//-----------------------------------------------------------------------------
struct GSStatsReceived_t
{
	enum { k_iCallback = k_iSteamGameServerStatsCallbacks };

	EResult		m_eResult;		// Success / error fetching the stats
	CSteamID	m_steamIDUser;	// The user for whom the stats are retrieved for
};

//-----------------------------------------------------------------------------
// Purpose: result of a request to store the user stats for a game
//-----------------------------------------------------------------------------
struct GSStatsStored_t
{
	enum { k_iCallback = k_iSteamGameServerStatsCallbacks + 1 };

	EResult		m_eResult;		// success / error
	CSteamID	m_steamIDUser;	// The user for whom the stats were stored
};

//-----------------------------------------------------------------------------
// Purpose: Callback indicating that a user's stats have been unloaded.
//  Call RequestUserStats again to access stats for this user
//-----------------------------------------------------------------------------
struct GSStatsUnloaded_t
{
	enum { k_iCallback = k_iSteamUserStatsCallbacks + 8 };

	CSteamID	m_steamIDUser;	// User whose stats have been unloaded
};

#pragma pack( pop )


#endif // GAMESERVERSTATSCOMMON_H
