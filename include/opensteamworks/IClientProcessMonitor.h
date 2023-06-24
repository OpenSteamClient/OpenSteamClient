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

#ifndef ICLIENTPROCESSMONITOR_H
#define ICLIENTPROCESSMONITOR_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientProcessMonitor
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RegisterProcess() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UnregisterProcess() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TerminateProcess() = 0; //argc: 2, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SuspendProcess() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ResumeProcess() = 0; //argc: 1, index 5
};

#endif // ICLIENTPROCESSMONITOR_H