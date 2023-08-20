// An LD_PRELOAD:able library to provide bootstrapper symbols on linux
// Windows doesn't have fancy things like LD_PRELOAD, so it doesn't get this
// MacOS has DYLD_INSERT_LIBRARIES
#include <string>
#include <cstring>
#include <iostream> 
#include <vector>

#include <dlfcn.h>
#include <link.h>
#include <stdio.h>
#include <stdlib.h>

static int n = 0;

static char *install_dir = nullptr;
static char *logging_dir = nullptr;
static char *local_share_path = nullptr;

static void set_local_share_path() {
    if (local_share_path != nullptr) {
        return;
    }

    local_share_path = getenv("XDG_DATA_HOME");
    if (local_share_path == nullptr) {
        char *home = getenv("HOME");
        if (home == nullptr) {
            throw "HOME not set";
        }

        std::string finalStr = std::string(home);
        finalStr.append("/.local/share/");
        local_share_path = new char[finalStr.size() + 1];
        strcpy(local_share_path, finalStr.c_str());
        return;
    }
}

extern "C" char* SteamBootstrapper_GetInstallDir() {
    if (install_dir != nullptr) {
        return install_dir;
    }

    auto home_env = std::string(local_share_path);
    home_env.append("/OpenSteam");

    install_dir = new char[home_env.size() + 1];
    strcpy(install_dir, home_env.c_str());
    return install_dir;
}

extern "C" char* SteamBootstrapper_GetLoggingDir() {
    if (logging_dir != nullptr) {
        return logging_dir;
    }

    auto logdir = std::string(install_dir);
    logdir.append("/logs");

    logging_dir = new char[logdir.size() + 1];
    strcpy(logging_dir, logdir.c_str());
    return logging_dir;
}

extern "C" bool StartCheckingForUpdates() {
  return true;
}

// First function called by steamclient
extern "C" unsigned int SteamBootstrapper_GetEUniverse() {
    std::cout << "SteamBootstrapper_GetEUniverse: 1" << std::endl;
    return 1;
}

extern "C" long long int GetBootstrapperVersion() {
    std::cout << "GetBootstrapperVersion: 0" << std::endl;
    return 0;
}

extern "C" const char* GetCurrentClientBeta() {
    std::cout << "GetCurrentClientBeta: opensteamclient" << std::endl;
    return "opensteamclient";
}

extern "C" void ClientUpdateRunFrame() {
    return; 
}

extern "C" bool IsClientUpdateAvailable() {
    return false;
}

extern "C" bool CanSetClientBeta() {
    return false;
}

__attribute__((constructor))
static void setup(void) {
    set_local_share_path();
    SteamBootstrapper_GetInstallDir();
    SteamBootstrapper_GetLoggingDir();
}

__attribute__((destructor))
static void teardown(void) {
    free(local_share_path);
    free(install_dir);
    free(logging_dir);
}