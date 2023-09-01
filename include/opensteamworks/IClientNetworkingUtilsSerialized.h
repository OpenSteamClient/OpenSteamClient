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

#ifndef ICLIENTNETWORKINGUTILSSERIALIZED_H
#define ICLIENTNETWORKINGUTILSSERIALIZED_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientNetworkingUtilsSerialized
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetNetworkConfigJSON_DEPRECATED() = 0; //argc: 3, index 1
    virtual unknown_ret GetLauncherType() = 0; //argc: 0, index 2
    virtual unknown_ret TEST_ClearCachedNetworkConfig() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PostConnectionStateMsg() = 0; //argc: 2, index 0
    virtual unknown_ret PostConnectionStateUpdatesForAllConnections() = 0; //argc: 0, index 1
    virtual unknown_ret PostAppSummaryUpdates() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GotLocationString() = 0; //argc: 1, index 0
};

#endif // ICLIENTNETWORKINGUTILSSERIALIZED_H