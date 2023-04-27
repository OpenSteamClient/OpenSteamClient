#ifndef ICLIENTCOMPAT_H
#define ICLIENTCOMPAT_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class UNSAFE_INTERFACE IClientCompat
{
public:
     virtual void BIsCompatLayerEnabled() = 0; //args: 0, index: 0
     virtual void EnableCompat() = 0; //args: 1, index: 1
     virtual void GetAvailableCompatTools() = 0; //args: 1, index: 2
     virtual void GetAvailableCompatToolsFiltered() = 0; //args: 2, index: 3
     virtual void GetAvailableCompatToolsForApp() = 0; //args: 2, index: 4
     virtual void SpecifyCompatTool() = 0; //args: 4, index: 5
     virtual void BIsCompatibilityToolEnabled() = 0; //args: 1, index: 6
     virtual void GetCompatToolName() = 0; //args: 1, index: 7
     virtual void GetCompatToolMappingPriority() = 0; //args: 1, index: 8
     virtual void GetCompatToolDisplayName() = 0; //args: 1, index: 9
     virtual void GetWhitelistedGameList() = 0; //args: 1, index: 10
     virtual void GetControllerConfigOverrides() = 0; //args: 1, index: 11
     virtual void StartSession() = 0; //args: 1, index: 12
     virtual void ReleaseSession() = 0; //args: 3, index: 13
     virtual void BIsLauncherServiceEnabled() = 0; //args: 1, index: 14
     virtual void DeleteCompatData() = 0; //args: 1, index: 15
     virtual void GetCompatibilityDataDiskSize() = 0; //args: 1, index: 16
     virtual void BNeedsUnlockH264() = 0; //args: 1, index: 17
};
#endif // ICLIENTCOMPAT_H
