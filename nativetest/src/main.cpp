#include <dlfcn.h>
#include <clientshortcuts.h>
#include <clientengine.h>
#include <stdio.h>
#include <stdlib.h>
#include <thread>
#include <unistd.h>

typedef void *(*CreateInterfaceFn)(const char *name);
typedef CGameID *(*GetGameIDForAppIDFn)(CGameID *tls, IClientShortcuts *, AppId_t appid);

pthread_key_t key;

int main(int argc, char const *argv[])
{
    auto steamclient = dlopen("/home/onni/.local/share/OpenSteam/linux64/steamclient.so", RTLD_GLOBAL | RTLD_NOW);
    printf("steamclient = %p\n", steamclient);
    CreateInterfaceFn createinterface = (CreateInterfaceFn)dlsym(steamclient, "CreateInterface");
    printf("CreateInterface = %p\n", createinterface);
    IClientEngine *engine = (IClientEngine*)createinterface("CLIENTENGINE_INTERFACE_VERSION005");
    printf("Engine = %p\n", engine);

    HSteamPipe pipe = engine->CreateSteamPipe();
    HSteamUser user = engine->ConnectToGlobalUser(pipe);
    printf("Pipe = %u, User = %u\n", pipe, user);

    IClientShortcuts *shortcuts = engine->GetIClientShortcuts(user, pipe);
    printf("IClientShortcuts = %p\n", shortcuts);

    CGameID gameid = 0;
    auto getAppIDForGameIDRet = shortcuts->GetAppIDForGameID(&gameid);
    printf("getAppIDForGameIDRet = %u\n", getAppIDForGameIDRet);

    AppId_t appid = 0;
    
    printf("pipe: %d, user: %d\n", shortcuts->pipe, shortcuts->user);

    CGameID gameid2 = 1;
    // pthread_key_create(&key, NULL);
    // CGameID *tl = (CGameID *)malloc(4096);
    // *tl = 1;
    // pthread_setspecific(key, tl);

    GetGameIDForAppIDFn GetGameIDForAppID = reinterpret_cast<GetGameIDForAppIDFn>(reinterpret_cast<uintptr_t>(shortcuts) + 8);
    auto getGameIDForAppID = GetGameIDForAppID(&gameid2, shortcuts, appid);
    printf("%p, %p, %p\n", getGameIDForAppID, &gameid2, shortcuts);
    printf("getGameIDForAppID = %d\n", gameid2);
    
    return 0;
}
