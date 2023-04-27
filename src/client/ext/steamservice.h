#include <dlfcn.h>
#include <iostream>
#include "../../common/pidutils.h"

class SteamServiceMgr
{
private:
public:
    // The pid of the external service (if external)
    pid_t externalServicePid = 0;

    // The dlopen handle of the service (if not external)
    void *dl_handle;

    // Unused
    void* (*CreateInterface)(const char *pName, int *pReturnCode);

    // Starts the steamservice
    void* (*SteamService_StartThread)(const char *pszIpcName);

    // Shuts down the service if internal
    void (*SteamService_Shutdown)();

    // The main function used to get most service related classes, kind of like IClientEngine
    void *(*SteamService_GetIPCServer)();

    int StartRemoteService();
    void KillRemoteService();

    SteamServiceMgr();
    ~SteamServiceMgr();
};