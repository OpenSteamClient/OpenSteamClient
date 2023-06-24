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

abstract_class IClientGameStats
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t GetNewSession( int8 nAccountType, uint64 ullAccountID, AppId_t nAppID, RTime32 rtTimeStarted ) = 0; //argc: 5, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t EndSession( uint64 ulSessionID, RTime32 rtTimeEnded, int16 nReasonCode ) = 0; //argc: 4, index 2
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddSessionAttributeInt( uint64 ulSessionID, const char *pstrName, int32 nData ) = 0; //argc: 4, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddSessionAttributeString( uint64 ulSessionID, const char *pstrName, const char *pstrData ) = 0; //argc: 4, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddSessionAttributeFloat( uint64 ulSessionID, const char *pstrName, float fData ) = 0; //argc: 4, index 5
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddNewRow( uint64 *pulRowID, uint64 ulSessionID, const char *pstrTableName ) = 0; //argc: 4, index 6
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult CommitRow( uint64 ulRowID ) = 0; //argc: 2, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult CommitOutstandingRows( uint64 ulSessionID ) = 0; //argc: 2, index 8
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddRowAttributeInt( uint64 ulRowID, const char *pstrName, int32 iData ) = 0; //argc: 4, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddRowAttributeString() = 0; //argc: 4, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddRowAttributeFloat( uint64 ulRowID, const char *pstrName, float fData ) = 0; //argc: 4, index 11
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddSessionAttributeInt64( uint64 ulSessionID, const char *pstrName, int64 llData ) = 0; //argc: 5, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult AddRowAttributeInt64( uint64 ulRowID, const char *pstrName, int64 llData ) = 0; //argc: 5, index 13
};


#endif // ICLIENTGAMESTATS_H