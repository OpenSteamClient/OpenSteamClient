#include <dlfcn.h>
#include <ostream>
#include <iostream>
#include "../interop/includesteamworks.h"
#include "../globals.h"

#define CLIENTENGINE_VERSION "CLIENTENGINE_INTERFACE_VERSION005"

#pragma once

class SteamClientMgr
{
private:
    void* TryLoadFunc(void* dlHandle, const char* funName);
public:
    HSteamUser user;
    HSteamPipe pipe;
    IClientEngine *ClientEngine;
    IClientUser *ClientUser;
    IClientFriends *ClientFriends;
    IClientApps *ClientApps;
    IClientAppManager *ClientAppManager;
    IClientUnifiedMessages *ClientUnifiedMessages;
    IClientSharedConnection *ClientSharedConnection;
    IClientUtils *ClientUtils;
    IClientCompat *ClientCompat;
    IClientConfigStore *ClientConfigStore;

    void *dl_handle;
    // Valid interfaces for steamclient.so:
    // SteamClientXXX (006-020, used by steam_api and games, this is what we would need to reimplement for a fully open source client) 
    // IVALIDATEXXX (001, used internally for testing)
    // CLIENTENGINE_INTERFACE_VERSIONXXX (005, used by the client)
    void* (*CreateInterface)(const char *pName, int *pReturnCode);
    HSteamPipe (*Steam_CreateSteamPipe)();
    HSteamUser (*Steam_ConnectToGlobalUser)(HSteamPipe *phSteamPipe);
    HSteamUser (*Steam_CreateGlobalUser)(HSteamPipe *phSteamPipe);
    HSteamUser (*Steam_CreateLocalUser)(HSteamPipe *phSteamPipe, EAccountType eAccountType);
    void (*Steam_ReleaseUser)(HSteamPipe hSteamPipe, HSteamUser hUser);
    bool (*Steam_BReleaseSteamPipe)(HSteamPipe hSteamPipe);
    bool (*Steam_BGetCallback)( HSteamPipe hSteamPipe, CallbackMsg_t *pCallbackMsg );
    void (*Steam_FreeLastCallback)( HSteamPipe hSteamPipe );
    bool failedLoad;

    SteamClientMgr();
    bool BFailedLoad();
    ~SteamClientMgr();
    void CreateClientEngine();
    void InitHSteamPipeAndHSteamUser();

    // Interfaces

    // Interfaces that don't need a login in order to work
    void CreateUserlessInterfaces();

    // Interfaces that need a login to work
    void CreateUserfulInterfaces();

    // Shuts down steamclient.so
    void Shutdown();
};