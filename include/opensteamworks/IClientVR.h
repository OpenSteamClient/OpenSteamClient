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

#ifndef ICLIENTVR_H
#define ICLIENTVR_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientVR
{
public:
    virtual unknown_ret GetCurrentHmd() = 0; //argc: 0, index 1
    virtual unknown_ret GetCompositor() = 0; //argc: 0, index 1
    virtual unknown_ret GetOverlay() = 0; //argc: 0, index 1
    virtual unknown_ret GetChaperone() = 0; //argc: 0, index 1
    virtual unknown_ret GetSettings() = 0; //argc: 0, index 1
    virtual unknown_ret GetProperties() = 0; //argc: 0, index 1
    virtual unknown_ret GetPaths() = 0; //argc: 0, index 1
    virtual unknown_ret GetOverlayHandle() = 0; //argc: 0, index 1
    virtual unknown_ret IsHmdPresent() = 0; //argc: 0, index 1
    virtual unknown_ret UpdateHmdStatus() = 0; //argc: 0, index 1
    virtual unknown_ret IsVRModeActive() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InitVR() = 0; //argc: 3, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StartSteamVR() = 0; //argc: 1, index 2
    virtual unknown_ret CleanupVR() = 0; //argc: 0, index 3
    virtual unknown_ret QuitAllVR() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret QuitApplication() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStringForHmdError() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LaunchApplication() = 0; //argc: 1, index 3
    virtual unknown_ret GetSteamVRAppId() = 0; //argc: 0, index 4
    virtual unknown_ret GetSteamVRPid() = 0; //argc: 0, index 1
    virtual unknown_ret GetWebSecret() = 0; //argc: 0, index 1
    virtual unknown_ret BSteamCanMakeVROverlays() = 0; //argc: 0, index 1
    virtual unknown_ret BServeVRGamepadUIOverlay() = 0; //argc: 0, index 1
    virtual unknown_ret BServeTenfootOverlay() = 0; //argc: 0, index 1
    virtual unknown_ret BSuppressDesktopBPM() = 0; //argc: 0, index 1
};

#endif // ICLIENTVR_H