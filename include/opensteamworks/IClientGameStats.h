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

#ifndef ICLIENTGAMESTATS_H
#define ICLIENTGAMESTATS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "GameStatsCommon.h"

abstract_class UNSAFE_INTERFACE IClientGameStats
{
public:
	virtual SteamAPICall_t GetNewSession( int8 nAccountType, uint64 ullAccountID, AppId_t nAppID, RTime32 rtTimeStarted ) = 0;
	virtual SteamAPICall_t EndSession( uint64 ulSessionID, RTime32 rtTimeEnded, int16 nReasonCode ) = 0;

	virtual EResult AddSessionAttributeInt( uint64 ulSessionID, const char *pstrName, int32 nData ) = 0;
	virtual EResult AddSessionAttributeString( uint64 ulSessionID, const char *pstrName, const char *pstrData ) = 0;
	virtual EResult AddSessionAttributeFloat( uint64 ulSessionID, const char *pstrName, float fData ) = 0;

	virtual EResult AddNewRow( uint64 *pulRowID, uint64 ulSessionID, const char *pstrTableName ) = 0;

	virtual EResult CommitRow( uint64 ulRowID ) = 0;
	virtual EResult CommitOutstandingRows( uint64 ulSessionID ) = 0;

	virtual EResult AddRowAttributeInt( uint64 ulRowID, const char *pstrName, int32 iData ) = 0;
	virtual EResult AddRowAtributeString( uint64 ulRowID, const char *pstrName, const char *pstrData ) = 0;
	virtual EResult AddRowAttributeFloat( uint64 ulRowID, const char *pstrName, float fData ) = 0;

	virtual EResult AddSessionAttributeInt64( uint64 ulSessionID, const char *pstrName, int64 llData ) = 0;
	virtual EResult AddRowAttributeInt64( uint64 ulRowID, const char *pstrName, int64 llData ) = 0;

	virtual EResult ReportString( uint64 ulSessionID, int32 iSeverity, const char *cszFormat, ... ) = 0;
	virtual EResult _ReportString( uint64 ulSessionID, int32, char const*, int32, unsigned char const* ) = 0;
	virtual EResult ReportStringAccumulated( uint64 ulSessionID, int32 iSeverity, const char *cszFormat, ... ) = 0;
	virtual EResult _ReportStringAccumulated( uint64 ulSessionID, int32, char const*, int32, unsigned char const* ) = 0;
	virtual EResult ReportBugScreenshot( uint64 ulSessionID, char const* szBugText, int32 cubScreenshot, const uint8* pubScreenshot ) = 0;
};


#endif // ICLIENTGAMESTATS_H
