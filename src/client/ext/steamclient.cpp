#include "steamclient.h"

SteamClientMgr::SteamClientMgr() 
{
    failedLoad = false;
    dl_handle = dlopen("steamclient.so", RTLD_NOW | RTLD_LOCAL);
    if (dl_handle == 0x0) {
        std::cout << "Failed to dlopen " << dlerror() << std::endl;
        failedLoad = true;
        return;
    }
    *(void**)(&CreateInterface) = TryLoadFunc(dl_handle, "CreateInterface");
    *(void**)(&Steam_CreateSteamPipe) = TryLoadFunc(dl_handle, "Steam_CreateSteamPipe");
    *(void**)(&Steam_ConnectToGlobalUser) = TryLoadFunc(dl_handle, "Steam_ConnectToGlobalUser");
    *(void**)(&Steam_CreateGlobalUser) = TryLoadFunc(dl_handle, "Steam_CreateGlobalUser");
    *(void**)(&Steam_CreateLocalUser) = TryLoadFunc(dl_handle, "Steam_CreateLocalUser");
    *(void**)(&Steam_ReleaseUser) = TryLoadFunc(dl_handle, "Steam_ReleaseUser");
    *(void**)(&Steam_BReleaseSteamPipe) = TryLoadFunc(dl_handle, "Steam_BReleaseSteamPipe");
    *(void**)(&Steam_BGetCallback) = TryLoadFunc(dl_handle, "Steam_BGetCallback");
    *(void**)(&Steam_FreeLastCallback) = TryLoadFunc(dl_handle, "Steam_FreeLastCallback");
    
}
void* SteamClientMgr::TryLoadFunc(void* dlHandle, const char* funName) {
    auto handle = dlsym(dlHandle, funName);
    if (handle == nullptr) {
        failedLoad = true;
        std::cerr << "Failed to dlsym " << funName << " from steamclient.so" << std::endl;
    }
    return handle;
}
bool SteamClientMgr::BFailedLoad() {
    return failedLoad;
}
SteamClientMgr::~SteamClientMgr() 
{
    dlclose(dl_handle);
}
void SteamClientMgr::CreateClientEngine() {
    int retcode = 0;
    auto enginePtr = CreateInterface(CLIENTENGINE_VERSION, &retcode);
    ClientEngine = static_cast<IClientEngine*>(enginePtr);
    if (ClientEngine != 0x0) {
        std::cout << "steamclient.so Found " << CLIENTENGINE_VERSION << " at " << ClientEngine << std::endl;
    } else {
        std::cerr << "steamclient_CreateInterface didn't understand " << CLIENTENGINE_VERSION << std::endl;
    }
}
void SteamClientMgr::InitHSteamPipeAndHSteamUser()
{
    pipe = ClientEngine->CreateSteamPipe();
    user = ClientEngine->ConnectToGlobalUser(pipe);
    if (user == 0x0)
    {
      std::cout << "Creating new GlobalUser" << std::endl;
      user = ClientEngine->CreateGlobalUser(&pipe);
    } else {
      std::cout << "Connecting to existing GlobalUser (warning: strange behaviour may occur)" << std::endl;
    }
}

void SteamClientMgr::CreateUserlessInterfaces() {
    ClientUser = ClientEngine->GetIClientUser(user, pipe);
    ClientApps = ClientEngine->GetIClientApps(user, pipe);
    ClientAppManager = ClientEngine->GetIClientAppManager(user, pipe);
    ClientUnifiedMessages = ClientEngine->GetIClientUnifiedMessages(user, pipe);
    ClientSharedConnection = ClientEngine->GetIClientSharedConnection(user, pipe);
    ClientUtils = ClientEngine->GetIClientUtils(pipe);
    ClientCompat = ClientEngine->GetIClientCompat(user, pipe);
    ClientConfigStore = ClientEngine->GetIClientConfigStore(user, pipe);
}

void SteamClientMgr::CreateUserfulInterfaces() {
    ClientFriends = ClientEngine->GetIClientFriends(user, pipe);
}

void SteamClientMgr::Shutdown() {
    ClientEngine->ReleaseUser(pipe, user);
    ClientEngine->BReleaseSteamPipe(pipe);
    ClientEngine->BShutdownIfAllPipesClosed();
}