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

#ifndef GAMESTATSCOMMON_H
#define GAMESTATSCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define STEAMGAMESTATS_INTERFACE_VERSION_001 "SteamGameStats001"

#define CLIENTGAMESTATS_INTERFACE_VERSION "CLIENTGAMESTATS_INTERFACE_VERSION001"



//-----------------------------------------------------------------------------
// Purpose: nAccountType for GetNewSession
//-----------------------------------------------------------------------------
enum EGameStatsAccountType
{
	k_EGameStatsAccountType_Steam = 1,				// ullAccountID is a 64-bit SteamID for a player
	k_EGameStatsAccountType_Xbox = 2,				// ullAccountID is a 64-bit XUID
	k_EGameStatsAccountType_SteamGameServer = 3,	// ullAccountID is a 64-bit SteamID for a game server
};



#pragma pack( push, 8 )
//-----------------------------------------------------------------------------
// Purpose: callback for GetNewSession() method
//-----------------------------------------------------------------------------
struct GameStatsSessionIssued_t
{
	enum { k_iCallback = k_iSteamGameStatsCallbacks + 1 };

	uint64	m_ulSessionID;
	EResult	m_eResult;
	bool	m_bCollectingAny;
	bool	m_bCollectingDetails;
};


//-----------------------------------------------------------------------------
// Purpose: callback for EndSession() method
//-----------------------------------------------------------------------------
struct GameStatsSessionClosed_t
{
	enum { k_iCallback = k_iSteamGameStatsCallbacks + 2 };

	uint64	m_ulSessionID;
	EResult	m_eResult;
};
#pragma pack( pop )



#endif // GAMESTATSCOMMON_H
