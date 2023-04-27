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

abstract_class UNSAFE_INTERFACE IClientCompat
{
public:
     virtual bool BIsCompatLayerEnabled() = 0; //args: 0, index: 0
     virtual void EnableCompat(bool) = 0; //args: 1, index: 1
     virtual void GetAvailableCompatTools(CUtlVector<CUtlString>*) = 0; //args: 1, index: 2
     virtual void GetAvailableCompatToolsFiltered(CUtlVector<CUtlString>*, ERemoteStoragePlatform) = 0; //args: 2, index: 3
     virtual void GetAvailableCompatToolsForApp(CUtlVector<CUtlString>*, AppId_t) = 0; //args: 2, index: 4
     virtual void SpecifyCompatTool(AppId_t, const char*, const char*, int priority) = 0; //args: 4, index: 5
     virtual bool BIsCompatibilityToolEnabled(AppId_t) = 0; //args: 1, index: 6
     virtual char *GetCompatToolName(AppId_t) = 0; //args: 1, index: 7
     virtual void GetCompatToolMappingPriority(AppId_t) = 0; //args: 1, index: 8
     virtual char* GetCompatToolDisplayName(const char*) = 0; //args: 1, index: 9
     virtual void GetWhitelistedGameList(CUtlVector<AppWhitelistSetting_t>*) = 0; //args: 1, index: 10
     virtual void GetControllerConfigOverrides(CUtlVector<AppControllerConfigOverride_t>*) = 0; //args: 1, index: 11
     virtual void StartSession(AppId_t) = 0; //args: 1, index: 12
     virtual void ReleaseSession() = 0; //args: 3, index: 13
     virtual bool BIsLauncherServiceEnabled(AppId_t) = 0; //args: 1, index: 14
     virtual void DeleteCompatData(AppId_t) = 0; //args: 1, index: 15
     virtual void GetCompatibilityDataDiskSize() = 0; //args: 1, index: 16
     virtual bool BNeedsUnlockH264() = 0; //args: 1, index: 17
};

#endif // ICLIENTCOMPAT_H
