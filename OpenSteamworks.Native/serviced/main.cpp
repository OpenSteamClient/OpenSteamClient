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
#include <sys/mman.h>
#include <string.h>
#include <sys/stat.h>
#include <fcntl.h>
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

#if __linux__
link_map* getLinkMap(void* handle)
{
    link_map* map = nullptr;
    dlinfo(handle, RTLD_DI_LINKMAP, &map);
    return map;
}

Elf32_Shdr getSection(void* handle, const char* sectionName, void** base, Elf32_Ehdr* ehdrOut) {
    std::size_t size = 0;
    Elf32_Shdr sect;

    const auto linkMap = getLinkMap(handle);
    if (!linkMap) {
        throw std::runtime_error("Failed to get link map");
    }

    if (const auto fd = open(linkMap->l_name, O_RDONLY); fd >= 0) {
        struct stat st;
        if (fstat(fd, &st) == 0) {
            if (const auto map = mmap(nullptr, st.st_size, PROT_READ, MAP_PRIVATE, fd, 0); map != MAP_FAILED) {
                const auto ehdr = (Elf32_Ehdr*)map;
                const auto shdrs = (Elf32_Shdr*)(uintptr_t(ehdr) + ehdr->e_shoff);
                const auto strTab = (const char*)(uintptr_t(ehdr) + shdrs[ehdr->e_shstrndx].sh_offset);
                
                for (auto i = 0; i < ehdr->e_shnum; ++i) {
                    const auto shdr = (Elf32_Shdr*)(uintptr_t(shdrs) + i * ehdr->e_shentsize);

                    if (strcmp(strTab + shdr->sh_name, sectionName) != 0)
                        continue;

                    *base = (void*)(linkMap->l_addr + shdr->sh_addr);
                    size = shdr->sh_size;
                    sect = *shdr;
                    *ehdrOut = *ehdr;

                    munmap(map, st.st_size);
                    close(fd);
                    break;
                }
                munmap(map, st.st_size);
            } else {
                throw std::runtime_error("Failed to mmap");
            }
        }
        close(fd);
    }

    return sect;
}

uintptr_t FindSignature(void* handle, const char* t_sign, const char* t_mask)
{
    auto map = getLinkMap(handle);
    void *base = nullptr;
    Elf32_Ehdr ehdr;
    Elf32_Shdr sect = getSection(handle, ".text", &base, &ehdr);
    auto size = sect.sh_size;
    size_t signLen = strlen(t_mask);
    if(signLen == 0)
    {
        throw std::runtime_error("empty signature");
    }

    const char *baseAddress = (char *)base;
    const char *searchBase = baseAddress;
    const char* searchEnd = (char*)(searchBase + size - signLen);

    while (searchBase < searchEnd)
    {
        int i;

        for(i = 0; i < signLen; ++i)
        {
            if(t_mask[i] != '?' && t_sign[i] != searchBase[i])
            {
                break;
            }
        }

        if(i == signLen)
        {
            return (uintptr_t)searchBase;
        }

        ++searchBase;
    }

    throw std::runtime_error("failed to find signature");
}

uintptr_t EvilLinuxHack(void* dl_handle) {
    uintptr_t internalStartPtr = FindSignature(dl_handle, "\x55\x89\xE5\x57\x56\x53\xE8\x00\x00\x00\x00\x81\xC3\x00\x00\x00\x00\x83\xEC\x00\x8B\x00\x00\x6A\x00\x8B", "xxxxxxx????xx????xx?x??x?x");
    std::cout << "SteamServiceInternal_StartThread is at " << (void *)internalStartPtr << std::endl;

    return internalStartPtr;
}

#endif

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
    uintptr_t internalStartPtr = EvilLinuxHack(dl_handle);
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