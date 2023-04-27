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

#ifndef ICLIENTGAMESERVERSTATS_H
#define ICLIENTGAMESERVERSTATS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "GameServerStatsCommon.h"

abstract_class UNSAFE_INTERFACE IClientGameServerStats
{
public:
	virtual SteamAPICall_t RequestUserStats( CSteamID steamIDUser, CGameID gameID ) = 0;
	
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, int32 *pData ) = 0;
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, float *pData ) = 0;
#else
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, float *pData ) = 0;
	virtual bool GetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, int32 *pData ) = 0;
#endif

	virtual bool GetUserAchievement( CSteamID steamIDUser, CGameID gameID, const char *pchName, bool *pbAchieved, RTime32 *prtTime ) = 0;
	
#if !(defined(_WIN32) && defined(__GNUC__))
	virtual bool SetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, int32 nData ) = 0;
	virtual bool SetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, float fData ) = 0;
#else
	virtual bool SetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, float fData ) = 0;
	virtual bool SetUserStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, int32 nData ) = 0;
#endif

	virtual bool UpdateUserAvgRateStat( CSteamID steamIDUser, CGameID gameID, const char *pchName, float flCountThisSession, double dSessionLength ) = 0;
	virtual bool SetUserAchievement( CSteamID steamIDUser, CGameID gameID, const char *pchName ) = 0;
	virtual bool ClearUserAchievement( CSteamID steamIDUser, CGameID gameID, const char *pchName ) = 0;
	virtual SteamAPICall_t StoreUserStats( CSteamID steamIDUser, CGameID gameID ) = 0;
	virtual void SetMaxStatsLoaded( uint32 uMax ) = 0;
};

#endif // ICLIENTGAMESERVERSTATS_H
