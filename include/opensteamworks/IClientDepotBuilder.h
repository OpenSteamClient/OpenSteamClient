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

#ifndef ICLIENTDEPOTBUILDER_H
#define ICLIENTDEPOTBUILDER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"



#define CLIENTDEPOTBUILDER_INTERFACE_VERSION "CLIENTDEPOTBUILDER_INTERFACE_VERSION001"



typedef enum EDepotBuildStatus
{
	k_EDepotBuildStatusInvalid = 0,
	k_EDepotBuildStatusProcessingConfig = 1,
	k_EDepotBuildStatusBuildingFileList = 2,
	k_EDepotBuildStatusProcessingData = 3,
	k_EDepotBuildStatusUploadingData = 4,
	k_EDepotBuildStatusCompleted = 5,
	k_EDepotBuildStatusFailed = 6,
}  EDepotBuildStatus;

//-----------------------------------------------------------------------------
// Purpose: Status of a given depot version, these are stored in the DB, don't renumber
//-----------------------------------------------------------------------------
enum EStatusDepotVersion
{
	k_EStatusDepotVersionInvalid = 0,			
	k_EStatusDepotVersionDisabled = 1,			// version was disabled, no manifest & content available
	k_EStatusDepotVersionAvailable = 2,			// manifest & content is available, but not current
	k_EStatusDepotVersionCurrent = 3,			// current depot version. The can be multiple, one for public and one for each beta key
};


typedef uint32 HDEPOTBUILD;


abstract_class IClientDepotBuilder
{
public:
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool BGetDepotBuildStatus(HDEPOTBUILD hDepotBuild, EDepotBuildStatus*, unsigned long long *pPercentDone, unsigned long long *pPercentMax) = 0; //argc: 5, index 1
    virtual unknown_ret VerifyChunkStore(AppId_t, DepotId_t, const char*) = 0; //argc: 3, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_2_DONTUSE() = 0; //argc: -1, index 3
    virtual unknown_ret DownloadChunk(AppId_t, DepotId_t, unsigned char const (*) [20]) = 0; //argc: 3, index 4
    virtual unknown_ret StartDepotBuild(AppId_t, DepotId_t, uint32, char const*) = 0; //argc: 4, index 5
    virtual unknown_ret CommitAppBuild(uint32 buildId, AppId_t appid, unsigned int *unkOut, unsigned long long *unkOut2, char const *betaKey, char const *unk) = 0; //argc: 6, index 6
};

#endif // ICLIENTDEPOTBUILDER_H