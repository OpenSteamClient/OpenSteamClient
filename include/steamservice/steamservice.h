#include "../steamtypes.h"

extern "C" void *CreateInterface(const char *pName, int *pReturnCode);

extern "C" void SteamService_Stop();
extern "C" void SteamService_Shutdown();
extern "C" void *SteamService_GetIPCServer();
extern "C" void *SteamService_StartThread(char *pIPCName);