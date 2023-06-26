#include <dlfcn.h>
#include <iostream>
#include <signal.h>
#include <fcntl.h>
#include "steamservice.h"
#include "../../common/pidutils.h"
#include <fstream>

int SteamServiceMgr::StartRemoteService() {
    pid_t forkresult = fork();

    switch (forkresult)
    {
        case 0:
        {
            // redirect steamserviced output to this file
            int fd = open("logs/serviced_log.txt", O_RDWR | O_CREAT, S_IRUSR | S_IWUSR);

            // stdout
            dup2(fd, 1);  

            // stderr
            dup2(fd, 2);  

            unsetenv("LD_LIBRARY_PATH");

            const char *args[] = {"./ubuntu12_32/steamserviced", NULL };
            execvp(args[0], (char**)args);
            break;
        }
        case -1:
        {
            std::cerr << "[SteamServiceMgr] An error occurred forking" << std::endl;
            return 1;
            break;
        }
    }

    std::cout << "[SteamServiceMgr] Steam client service's PID is " << forkresult << std::endl;
    externalServicePid = forkresult;

    //TODO: find out a better way (maybe in IClientNetworkingUtils) to set bIsServiceLocal to true instead of doing this
    // Currently, we create a mock steamservice.so(src/service/fakeservice.cpp) that contains all functions needed to get the steam client to init steamservice far enough.

    // 1. When the IPC server is first initializing, it calls BSetIpPortFromName. This function hard-codes Steam3Master and SteamClientService.
    // 2. If the name passed to it doesn't match either, it uses getenv to try and find it.
    // If it returns an IP:PORT that is in use, it will connect to it instead of trying to start it's own IPCServer

    // 1. When the service in-process is initializing, it tries to find SteamClientService_<thispid> with BSetIpPortFromName
    // 2. This normally fails, and makes steamclient initialize steamservice.
    // We abuse the getenv call to make it go to a locally running service instead.

    // This call sets SteamClientService_<thispid> envvar to point to 127.0.0.1:57344 (default for SteamClientService in the shared steam codebase between all steam bins),
    // thus it finds out that a service is already running and it doesn't try to init further, which would fail as we don't have a full steam service impl.
    // Is this VAC bannable?
    setenv(std::string("SteamClientService_").append(PIDUtils::GetPid()).c_str(), "127.0.0.1:57344", 1);

    return 0;
}
void SteamServiceMgr::KillRemoteService() {
    // the steamservice doesn't handle any important state, so it's probably safe to kill without corrupting state
    if (externalServicePid != 0) {
        kill(externalServicePid, SIGKILL);
    }
}

SteamServiceMgr::SteamServiceMgr() 
{
#ifdef STEAMSERVICED
    dl_handle = dlopen("steamservice.so", RTLD_NOW | RTLD_LOCAL);
    *(void**)(&SteamService_StartThread) = dlsym(dl_handle, "SteamService_StartThread");
    *(void**)(&SteamService_GetIPCServer) = dlsym(dl_handle, "SteamService_GetIPCServer");
    *(void**)(&SteamService_Shutdown) = dlsym(dl_handle, "SteamService_Shutdown");
    *(void**)(&CreateInterface) = dlsym(dl_handle, "CreateInterface");
#endif
}
SteamServiceMgr::~SteamServiceMgr() 
{
#ifdef STEAMSERVICED
    dlclose(dl_handle);
#endif
}