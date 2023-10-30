// Windows doesn't have fancy things like LD_PRELOAD, so it gets this header we include in every exe instead
#include <string>
#include <cstring>
#include <iostream> 
#include <vector>

#include <shlobj.h>
#include <stdio.h>
#include <stdlib.h>

#define DLLEXPORT extern "C" __declspec(dllexport)

static int n = 0;

static char *install_dir = nullptr;
static char *logging_dir = nullptr;
static char *local_share_path = nullptr;

static void set_localappdata_path() {
    if (local_share_path != nullptr) {
        return;
    }
    
    // Microsoft what the fuck is this????
    wchar_t* localAppData = 0;
    SHGetKnownFolderPath(FOLDERID_LocalAppData, 0, NULL, (PWSTR*)&localAppData);
    char cstr[512];
    wcstombs(cstr, localAppData, 512);
    std::string str = std::string(cstr);

    local_share_path = new char[str.size() + 1];
    strcpy(local_share_path, str.c_str());
    return;
}

DLLEXPORT char* __cdecl SteamBootstrapper_GetInstallDir() {
    if (install_dir != nullptr) {
        return install_dir;
    }

    set_localappdata_path();

    auto home_env = std::string(local_share_path);
    home_env.append("/OpenSteam");

    install_dir = new char[home_env.size() + 1];
    strcpy(install_dir, home_env.c_str());
    return install_dir;
}

DLLEXPORT char* __cdecl SteamBootstrapper_GetLoggingDir() {
    if (logging_dir != nullptr) {
        return logging_dir;
    }

    SteamBootstrapper_GetInstallDir();

    auto logdir = std::string(install_dir);
    logdir.append("/logs");

    logging_dir = new char[logdir.size() + 1];
    strcpy(logging_dir, logdir.c_str());
    return logging_dir;
}

DLLEXPORT bool __cdecl StartCheckingForUpdates() {
  return true;
}

// First function called by steamclient
DLLEXPORT unsigned int __cdecl SteamBootstrapper_GetEUniverse() {
    std::cout << "SteamBootstrapper_GetEUniverse: 1" << std::endl;
    return 1;
}

DLLEXPORT long long int __cdecl GetBootstrapperVersion() {
    std::cout << "GetBootstrapperVersion: 0" << std::endl;
    return 0;
}

DLLEXPORT const char* __cdecl GetCurrentClientBeta() {
    std::cout << "GetCurrentClientBeta: opensteamclient" << std::endl;
    return "opensteamclient";
}

DLLEXPORT void __cdecl ClientUpdateRunFrame() {
    return; 
}

DLLEXPORT bool __cdecl IsClientUpdateAvailable() {
    return false;
}

DLLEXPORT bool __cdecl CanSetClientBeta() {
    return false;
}