#pragma once
#include <types.h>

typedef UInt32 AppId_t;
typedef UInt64 CGameID;

class IClientShortcuts
{
public:
    HSteamUser user;
    HSteamPipe pipe;
    virtual AppId_t GetUniqueLocalAppId() = 0;
    virtual CGameID *GetGameIDForAppID(void*, AppId_t shortcutAppID) = 0; 
    virtual AppId_t GetAppIDForGameID(CGameID* gameid) = 0;
};