#pragma once
#include <types.h>

typedef UInt32 AppId_t;

class IClientShortcuts
{
public:
    virtual AppId_t GetUniqueLocalAppId() = 0;
    virtual CGameID* GetGameIDForAppID(AppId_t shortcutAppID) = 0; 
    virtual AppId_t GetAppIDForGameID(CGameID* gameid) = 0;
};