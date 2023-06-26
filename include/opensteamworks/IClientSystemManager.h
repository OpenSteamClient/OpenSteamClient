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

#ifndef ICLIENTSYSTEMMANAGER_H
#define ICLIENTSYSTEMMANAGER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientSystemManager
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetSettings() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateSettings() = 0; //argc: 1, index 2
    virtual unknown_ret ShutdownSystem() = 0; //argc: 0, index 3
    virtual unknown_ret SuspendSystem() = 0; //argc: 0, index 1
    virtual unknown_ret RestartSystem() = 0; //argc: 0, index 1
    virtual unknown_ret FactoryReset() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RebootToFactoryTestImage() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetDisplayBrightness() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetDisplayBrightness() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret FormatRemovableStorage() = 0; //argc: 1, index 4
    virtual unknown_ret GetOSBranchList() = 0; //argc: 0, index 5
    virtual unknown_ret GetCurrentOSBranch() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SelectOSBranch() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetUpdateState() = 0; //argc: 1, index 2
    virtual unknown_ret CheckForUpdate() = 0; //argc: 0, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ApplyUpdate() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetBackgroundUpdateCheckInterval() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ClearAudioDefaults() = 0; //argc: 1, index 3
    virtual unknown_ret RunDeckMicEnableHack() = 0; //argc: 0, index 4
    virtual unknown_ret RunDeckEchoCancellationHack() = 0; //argc: 0, index 1
};

#endif // ICLIENTSYSTEMMANAGER_H