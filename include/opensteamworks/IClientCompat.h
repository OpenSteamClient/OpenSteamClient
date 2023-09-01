#ifndef ICLIENTCOMPAT_H
#define ICLIENTCOMPAT_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

// Used for ERemoteStoragePlatform
#include "RemoteStorageCommon.h"

struct AppWhitelistSetting_t;
struct AppControllerConfigOverride_t;

abstract_class IClientCompat
{
public:
    virtual bool BIsCompatLayerEnabled() = 0; //argc: 0, index 1
    virtual void EnableCompat(bool) = 0; //argc: 1, index 0
    virtual void GetAvailableCompatTools(CUtlVector<CUtlString>*) = 0; //argc: 1, index 1
    virtual void GetAvailableCompatToolsFiltered(CUtlVector<CUtlString>*, ERemoteStoragePlatform) = 0; //argc: 2, index 2
    virtual void GetAvailableCompatToolsForApp(CUtlVector<CUtlString>*, AppId_t) = 0; //argc: 2, index 3
    virtual void SpecifyCompatTool(AppId_t, const char*, const char*, int priority) = 0; //argc: 4, index 4
    virtual bool BIsCompatibilityToolEnabled(AppId_t) = 0; //argc: 1, index 5
    virtual char *GetCompatToolName(AppId_t) = 0; //argc: 1, index 6
    virtual void GetCompatToolMappingPriority(AppId_t) = 0; //argc: 1, index 7
    virtual char* GetCompatToolDisplayName(const char*) = 0; //argc: 1, index 8
    virtual void GetWhitelistedGameList(CUtlVector<AppWhitelistSetting_t>*) = 0; //argc: 1, index 9
    virtual void GetControllerConfigOverrides(CUtlVector<AppControllerConfigOverride_t>*) = 0; //argc: 1, index 10
    virtual void StartSession(AppId_t) = 0; //argc: 1, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ReleaseSession() = 0; //argc: 3, index 12
    virtual bool BIsLauncherServiceEnabled(AppId_t) = 0; //argc: 1, index 13
    virtual void DeleteCompatData(AppId_t) = 0; //argc: 1, index 14
    virtual void GetCompatibilityDataDiskSize() = 0; //argc: 1, index 15
    virtual bool BNeedsUnlockH264() = 0; //argc: 1, index 16
};

#endif // ICLIENTCOMPAT_H