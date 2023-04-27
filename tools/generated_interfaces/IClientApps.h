#ifndef ICLIENTAPPS_H
#define ICLIENTAPPS_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "AppsCommon.h"

abstract_class UNSAFE_INTERFACE IClientApps
{
public:
     virtual void GetAppData() = 0; //args: 4, index: 0
     virtual void SetLocalAppConfig() = 0; //args: 3, index: 1
     virtual void GetInternalAppIDFromGameID() = 0; //args: 1, index: 2
     virtual void GetAllOwnedMultiplayerApps() = 0; //args: 2, index: 3
     virtual void GetAvailableLaunchOptions() = 0; //args: 3, index: 4
     virtual void GetAppDataSection() = 0; //args: 5, index: 5
     virtual void GetMultipleAppDataSections() = 0; //args: 7, index: 6
     virtual void RequestAppInfoUpdate() = 0; //args: 2, index: 7
     virtual void GetDLCCount() = 0; //args: 1, index: 8
     virtual void BGetDLCDataByIndex() = 0; //args: 6, index: 9
     virtual void GetAppType() = 0; //args: 1, index: 10
     virtual void GetStoreTagLocalization() = 0; //args: 5, index: 11
     virtual void TakeUpdateLock() = 0; //args: 0, index: 12
     virtual void GetAppKVRaw() = 0; //args: 3, index: 13
     virtual void ReleaseUpdateLock() = 0; //args: 0, index: 14
};
#endif // ICLIENTAPPS_H
