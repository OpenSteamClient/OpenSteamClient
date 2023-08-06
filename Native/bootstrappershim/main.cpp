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

typedef char* (*getenv_t)(const char *);
getenv_t original_getenv_func = NULL;

struct ElfInfoStruct {
    ElfW(Addr) start, end;
};

static int n = 0;
static std::vector<ElfInfoStruct*> segments;

static char *fake_home_path = nullptr;
static char *install_dir = nullptr;
static char *local_share_path = nullptr;

// Taken partially from https://github.com/joaomlneto/procmap
// Mostly just inlined some things and removed the things we don't need
static void readMemoryMap() {
    char *line = NULL;
    size_t line_size = 0;

    // open maps file
    FILE *maps = fopen("/proc/self/maps", "r");

    // parse the maps file
    while (getline(&line, &line_size, maps) > 0) {
        ElfInfoStruct *info = (ElfInfoStruct*)malloc(sizeof(ElfInfoStruct));
        unsigned long _offset;
        unsigned int  _deviceMajor;
        unsigned int  _deviceMinor;
        ino_t         _inode;
        std::string   name;
        int name_start = 0, name_end = 0;
        char perms_str[8];

        // parse string
        sscanf(line, "%lx-%lx %7s %lx %u:%u %lu %n%*[^\n]%n",
                            &info->start, &info->end, perms_str, &_offset,
                            &_deviceMajor, &_deviceMinor, &_inode,
                            &name_start, &name_end) < 7;

        // copy name
        if (name_end > name_start) {
            line[name_end] = '\0';
            name.assign(&line[name_start]);
        }
        
        // check if name is steamclient
        if (name.find("steamclient.so") == std::string::npos) {
            free(info);
            continue;
        }

        n++;
        segments.push_back(info);
    }

    // cleanup
    free(line);
}

static void set_local_share_path() {
    if (local_share_path != nullptr) {
        return;
    }

    local_share_path = original_getenv_func("XDG_DATA_HOME");
    if (local_share_path == nullptr) {
        char *home = original_getenv_func("HOME");
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
    home_env.append("OpenSteam");

    install_dir = new char[home_env.size() + 1];
    strcpy(install_dir, home_env.c_str());
    return install_dir;
}

static void set_fake_home_path() {
    if (fake_home_path != nullptr) {
        return;
    }

    std::string shareDir;
    shareDir = std::string(SteamBootstrapper_GetInstallDir());
    shareDir.append("/fakehome");

    fake_home_path = new char[shareDir.size() + 1];
    strcpy(fake_home_path, shareDir.c_str());
}



__attribute__((constructor))
static void setup(void) {
    original_getenv_func = (getenv_t)dlsym(RTLD_NEXT, "getenv");
    set_local_share_path();
    set_fake_home_path();
}

__attribute__((destructor))
static void teardown(void) {
    for (auto &&i : segments)
    {
        free(i);
    }
    free(fake_home_path);
    free(install_dir);
    free(local_share_path);
}

extern "C" bool StartCheckingForUpdates() {
  return true;
}

// First function resolved by steamclient
extern "C" unsigned int SteamBootstrapper_GetEUniverse() {
    if (n == 0) {
        // Store current shared objects's memory segments, and later on fake the HOME variable only for them
        readMemoryMap();
    }
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

extern "C" char* getenv(const char *name) {

    char* result = original_getenv_func(name);

    if (n != 0) {
        ElfW(Addr) addr = (ElfW(Addr))__builtin_extract_return_addr(__builtin_return_address(0));
        for (int i = 0; i < n; i++)
        {
            if(addr >= segments[i]->start && addr < segments[i]->end) {
                if (std::string(name) == "HOME") {
                    return fake_home_path;
                }
            }
        }
    }

    return result;
}