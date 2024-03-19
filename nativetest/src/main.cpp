#include <dlfcn.h>
#include <clientshortcuts.h>
#include <clientengine.h>
#include <stdio.h>
#include <stdlib.h>
#include <thread>
#include <unistd.h>

#ifdef __GNUC__
#define BYSTACK __attribute__((ms_abi)) __attribute__((stdcall))
#endif

typedef void *(*CreateInterfaceFn)(const char *name);
typedef void *(BYSTACK *GetGameIDForAppIDFn)(void *, void *, AppId_t appid);

struct fakeinterface {
    HSteamUser user;
    HSteamPipe pipe;
};

Dl_info info;

void *calculateStaticPtr(void* ptr) {
    return (void*)((((uintptr_t)ptr) - (uintptr_t)info.dli_fbase) + 0x100000);
}

extern void *stdcall_func_GetAppIDForGameID(void* func, CGameID *buf, AppId_t appid);

int main(int argc, char const *argv[])
{
    auto steamclient = dlopen("/home/onni/.local/share/OpenSteam/linux64/steamclient.so", RTLD_GLOBAL | RTLD_NOW);
    printf("steamclient = %p\n", steamclient);

    void *initPtr = dlsym(steamclient, "_init");
    printf("initPtr: %p\n", initPtr);
    dladdr(initPtr, &info);

    // 0x1bc5e60
    // 0x1ac5e60
    CreateInterfaceFn createinterface = (CreateInterfaceFn)dlsym(steamclient, "CreateInterface");
    printf("CreateInterface = %p, static %p\n", createinterface, calculateStaticPtr((void*)createinterface));
    IClientEngine *engine = (IClientEngine*)createinterface("CLIENTENGINE_INTERFACE_VERSION005");
    printf("Engine = %p, static = %p\n", engine, calculateStaticPtr(engine));

    HSteamPipe pipe = engine->CreateSteamPipe();
    HSteamUser user = engine->ConnectToGlobalUser(pipe);
    printf("Pipe = %u, User = %u\n", pipe, user);

    IClientShortcuts *shortcuts = engine->GetIClientShortcuts(user, pipe);
    printf("IClientShortcuts = %p\n", shortcuts);

    CGameID gameid = CGameID(730, 2);
    auto getAppIDForGameIDRet = shortcuts->GetAppIDForGameID(&gameid);
    printf("getAppIDForGameIDRet = %u\n", getAppIDForGameIDRet);

    AppId_t appid = 0;
    
    printf("pipe: %d, user: %d\n", shortcuts->pipe, shortcuts->user);

    CGameID gameid2 = CGameID(0);

    auto vtable = (uintptr_t*)((uintptr_t*)shortcuts)[0];
    printf("vtable: %p, static: %p\n", vtable, calculateStaticPtr(vtable));

    GetGameIDForAppIDFn GetGameIDForAppID = reinterpret_cast<GetGameIDForAppIDFn>(vtable + 1);
    printf("GetGameIDForAppID: %p, GetGameIDForAppID: %p\n", (void*)GetGameIDForAppID, calculateStaticPtr((void*)GetGameIDForAppID));

    //auto getGameIDForAppID = GetGameIDForAppID(&gameid2, shortcuts, appid);
    auto getGameIDForAppID = stdcall_func_GetAppIDForGameID((void*)GetGameIDForAppID, &gameid2, appid);
    // auto getGameIDForAppID = shortcuts->GetGameIDForAppID(0);
    
    printf("%p, %p, %p\n", getGameIDForAppID, &gameid2, shortcuts);
    printf("getGameIDForAppID = %lu\n", gameid2);
    
    return 0;
}
