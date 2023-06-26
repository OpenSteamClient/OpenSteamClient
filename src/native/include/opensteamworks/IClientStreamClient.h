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

#ifndef ICLIENTSTREAMCLIENT_H
#define ICLIENTSTREAMCLIENT_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

#define CLIENTSTREAMCLIENT_INTERFACE_VERSION "CLIENTSTREAMCLIENT_INTERFACE_VERSION001"

abstract_class IClientStreamClient
{
public:
    virtual void Launched( CGameID gameID ) = 0; //argc: 1, index 1
    virtual void FocusGained( CGameID gameID, bool ) = 0; //argc: 2, index 2
    virtual void FocusLost( CGameID gameID ) = 0; //argc: 1, index 3
    virtual void Finished( CGameID gameID, EResult eResult ) = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetStreamingClientConfig() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BSaveStreamingClientConfig() = 0; //argc: 1, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetQualityOverride() = 0; //argc: 1, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetBitrateOverride() = 0; //argc: 1, index 8
    virtual unknown_ret ShowOnScreenKeyboard() = 0; //argc: 0, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BQueueControllerConfigMessageForLocal() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetControllerConfigMessageForRemote() = 0; //argc: 1, index 2
    virtual const char * GetSystemInfo() = 0; //argc: 0, index 3
    virtual void StartStreamingSession( CGameID gameID ) = 0; //argc: 1, index 1
    virtual void ReportStreamingSessionEvent( CGameID gameID, const char * ) = 0; //argc: 2, index 2
    virtual void FinishStreamingSession( CGameID gameID, const char *, const char * ) = 0; //argc: 3, index 3
};

#endif // ICLIENTSTREAMCLIENT_H