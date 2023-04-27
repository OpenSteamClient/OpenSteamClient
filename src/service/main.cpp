
#include "../common/pidutils.h"
#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>
#include <link.h>
#include <filesystem>
namespace fs = std::filesystem;

#include "../common/ldpath.h"
#include "../client/ext/steamservice.cpp"

// DEBUGGER
#include <signal.h>
#include <csignal> // or C++ style alternative

bool done = false;
void handle_sigint(int sig)
{
    done = true;
}

void WaitForControlSignalToContinue() {
  std::cout << "Kill with CTRL+C" << std::endl;
  signal(SIGINT, handle_sigint);
  while (!done) {
    std::this_thread::sleep_for(std::chrono::milliseconds(1000));
  }
  done = false;
  signal(SIGINT, SIG_DFL);
}

// Find this by looking at SteamService_StartThread with Ghidra and seeing what function it calls
int (*SteamServiceInternal_StartThread)(void* ipcServerPtr, const char *pszIPCName, bool bCrossProcess, bool bCrossSession, bool unknown);

//TODO: This is really fishy and could potentially be VAC bannable (we don't have .valvesig)
int main(int argc, char *argv[]) {

  // thispath should be in ubuntu12_32
  auto thispath = fs::path(fs::read_symlink("/proc/self/exe")).parent_path();

  if (!fs::exists(thispath / "steamservice.so")) {
    std::cerr << "Failed to start steamserviced: Launched in wrong directory" << std::endl;
    exit(EXIT_FAILURE);
  }

  // Set our LD_LIBRARY_PATH so dlopen can find steamservice.so
  SetLdLibraryPath(fs::absolute(thispath / ".."));

  auto steamservicemgr = new SteamServiceMgr();

  // IPC names are defined in BSetIpPortFromName
  std::string ipcName = "SteamClientService";

  // Change this only for testing
  std::string launchType = "Internal";

  Dl_info info;
  link_map map;

  // Init is (always?) the first function exported in an ELF (libraries and executables).
  void *initPtr = dlsym(steamservicemgr->dl_handle, "_init");

  // 0x45f70 (addr of SteamServiceInternal_StartThread) - 0x10000(base of steamservice.so)
  uintptr_t offset = 0x35f70;
  if (initPtr == nullptr) {
    std::cout << "Re-execing" << std::endl;
    // Re-exec, LdLibraryPath was set 
    auto failcode = execvp(argv[0], argv);
    std::cout << argv[0] << std::endl;
  }
  std::cout << "SteamService _init is at " << (void *)(initPtr) << std::endl;
  // get info (including base address) of steamservice loaded in memory
  dladdr((void *)initPtr, &info);

  // calculate the location of SteamServiceInternal_StartThread
  uintptr_t internalStartPtr = ((uintptr_t)info.dli_fbase + offset);

  // here to aid in debugging (if info is 0 the lookup failed)
  std::cout << "dladdr returned  " << (void *)(&info) << std::endl;

  std::cout << "SteamService fbase is at " << (void *)(info.dli_fbase) << std::endl;
  std::cout << "(calculated) SteamServiceInternal_StartThread is at " << (void *)internalStartPtr << std::endl;

  std::cout << "Starting SteamService using LaunchMethod: " << launchType << " with ipcName " << ipcName << std::endl;
  if (launchType == "Internal")
  {
    void *servicePtr = steamservicemgr->SteamService_GetIPCServer();
    *(void **)(&SteamServiceInternal_StartThread) = (void *)internalStartPtr;

    // true, false, true is the one used on windows, on linux it is false, false, true by default
    int returnCode = SteamServiceInternal_StartThread(servicePtr, ipcName.c_str(), true, false, true);

    std::cout << "SteamService_ptr is " << servicePtr << std::endl;
    std::cout << "SteamServiceInternal_StartThread returned " << returnCode << std::endl;
    }
    else if (launchType == "Exported")
    {
      std::cout << "SteamService_StartThread is at " << (void*)steamservicemgr->SteamService_StartThread << std::endl;
      auto steamservicePtr = steamservicemgr->SteamService_StartThread(ipcName.c_str());
      std::cout << "SteamService_ptr is " << steamservicePtr << std::endl;
    }
    else
    {
      std::cout << "Not launching, compiled with invalid launch type.";
    }
    //DEBUGGER
    //raise(SIGSTOP);

    WaitForControlSignalToContinue();
    steamservicemgr->SteamService_Shutdown();
}
