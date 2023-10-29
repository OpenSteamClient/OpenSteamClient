#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>


// DEBUGGER
#include <signal.h>
#include <csignal> // or C++ style alternative

#if __linux__
#define STEAMSERVICE_LIB "ubuntu12_32/steamservice.so"
#include <link.h>
#include <sys/prctl.h>
#endif 

#if _WIN32
#define STEAMSERVICE_LIB "steamservice.dll"
#include "../windowssupport.h"
#endif


bool done = false;

void handle_sigint(int sig)
{
    done = true;
}

// Find this by looking at SteamService_StartThread with Ghidra and seeing what function it calls
//TODO: use signature scanning to avoid having to manually decompile every time
int (*SteamServiceInternal_StartThread)(void* ipcServerPtr, const char *pszIPCName, bool bCrossProcess, bool bCrossSession, bool unknown);

// Unused on Windows, but could theoretically be used to allow usermode steamservice
int (*SteamService_StartThread)(const char* ipcName);

// Used on Windows to loop and ensure the actual installed service is running
void (*SteamService_RunMainLoop)();

// Shuts down the service
void (*SteamService_Shutdown)();

// The main function used to get most service related classes, kind of like IClientEngine
void *(*SteamService_GetIPCServer)();

void WaitForControlSignalToContinue() {
  std::cout << "Kill with CTRL+C" << std::endl;
  signal(SIGINT, handle_sigint);
  while (!done) {
    std::this_thread::sleep_for(std::chrono::milliseconds(1000));
#if _WIN32
    // if (SteamService_RunMainLoop != nullptr) {
    //     SteamService_RunMainLoop();
    // }
#endif
    //TODO: Test if parent process is still alive (on non-linux OS:s, handled with PR_SET_PDEATHSIG on linux)
  }
  done = false;
  signal(SIGINT, SIG_DFL);
}

void *servicePtr = nullptr;

//TODO: This is really fishy and could potentially be VAC bannable (we don't have .valvesig section or a Windows signature)
// An evil hack that allows us to:
// - Split the steam service into a 32-bit process while keeping our main process 64-bit
// - Bypass SteamService as a Windows service restrictions (scrapped for now, just use SteamService_StartThread if you want to test) (also might have the side effect of being able to run fully in usermode)
int main(int argc, char *argv[]) {
    // Kill process when parent dies
#if __linux__
    prctl(PR_SET_PDEATHSIG, SIGKILL);
#endif 

    auto dl_handle = dlopen(STEAMSERVICE_LIB, RTLD_NOW);
    if (dl_handle == nullptr) {
        std::cerr << "dl_handle == nullptr!!!" << std::endl;
        return 1;
    }

    // IPC names are defined in BSetIpPortFromName (and hardcoded in steamservice.dll)
    std::string ipcName = "SteamClientService";

#if __linux__
    EvilLinuxHack();
    *(void**)(&SteamService_GetIPCServer) = dlsym(dl_handle, "SteamService_GetIPCServer");
    *(void**)(&SteamServiceInternal_StartThread) = (void *)internalStartPtr;
    if (SteamService_GetIPCServer == nullptr) {
        std::cerr << "SteamService_GetIPCServer == nullptr!!!" << std::endl;
        return 1;
    }

    if (SteamServiceInternal_StartThread == nullptr) {
        std::cerr << "SteamServiceInternal_StartThread == nullptr (wtf?)" << std::endl;
        return 1;
    }

    servicePtr = SteamService_GetIPCServer();
    std::cout << "Service ptr " << servicePtr << std::endl;
#endif

#if _WIN32
    *(void**)(&SteamService_RunMainLoop) = dlsym(dl_handle, "SteamService_RunMainLoop");
    *(void**)(&SteamService_StartThread) = dlsym(dl_handle, "SteamService_StartThread");

    if (SteamService_RunMainLoop == nullptr) {
        std::cerr << "SteamService_RunMainLoop == nullptr!!!" << std::endl;
        return 1;
    }

    if (SteamService_StartThread == nullptr) {
        std::cerr << "SteamService_StartThread == nullptr!!!" << std::endl;
        return 1;
    }

    //ipcName = ipcName.append("_" + std::string(getenv("OPENSTEAM_PID")));
#endif

    *(void**)(&SteamService_Shutdown) = dlsym(dl_handle, "SteamService_Shutdown");

    // true, false, true is used on windows, on linux it is false, false, true by default
    // true, true, true is also valid, but what does it do?
    std::cout << "Starting SteamService using ipcName " << ipcName << std::endl;

#if __linux__
    int returnCode = SteamServiceInternal_StartThread(servicePtr, ipcName.c_str(), true, false, true);
    std::cout << "SteamService_ptr is " << servicePtr << std::endl;
    std::cout << "SteamServiceInternal_StartThread returned " << returnCode << std::endl;
#endif

// #if false
#if _WIN32
    int returnCode = SteamService_StartThread(ipcName.c_str());
    std::cout << "SteamService_StartThread returned " << returnCode << std::endl;
#endif


    WaitForControlSignalToContinue();
    SteamService_Shutdown();
}

#if __linux__
uintptr_t EvilLinuxHack() {
    Dl_info info;

    // Init is (always?) the first function exported in an ELF (libraries and executables).
    void *initPtr = dlsym(dl_handle, "_init");

    // 0047e20
    // 0x47e20 (addr of SteamServiceInternal_StartThread) - 0x10000(base of steamservice.so)
    // 0x5ee50
    uintptr_t offset = 0x47e20 - 0x10000;
    if (initPtr == nullptr) {
        std::cerr << "InitPtr == nullptr!!!" << std::endl;
        return 1;
    }

    std::cout << "SteamService _init is at " << (void *)(initPtr) << std::endl;
    // get info (including base address) of steamservice loaded in memory
    dladdr((void *)initPtr, &info);

    // calculate the location of SteamServiceInternal_StartThread
    uintptr_t internalStartPtr = ((uintptr_t)info.dli_fbase + offset);

    // here to aid in debugging (if info is 0 the lookup failed)
    std::cout << "dladdr returned  " << (void *)(&info) << std::endl;

    std::cout << "SteamService fbase is at " << (void *)(info.dli_fbase) << std::endl;
    std::cout << "SteamServiceInternal_StartThread is at " << (void *)internalStartPtr << std::endl;

    return internalStartPtr;
}
#endif