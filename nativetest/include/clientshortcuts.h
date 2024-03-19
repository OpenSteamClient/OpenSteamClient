#pragma once
#include <types.h>

typedef UInt32 AppId_t;

class IClientShortcuts
{
public:
    HSteamUser user;
    HSteamPipe pipe;
    virtual AppId_t GetUniqueLocalAppId() = 0;
    virtual __attribute__((ms_abi)) __attribute__((stdcall)) CGameID GetGameIDForAppID(AppId_t shortcutAppID) = 0; 
    virtual AppId_t GetAppIDForGameID(CGameID* gameid) = 0;
};