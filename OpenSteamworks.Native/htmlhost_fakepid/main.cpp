#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>
#include <link.h>
#include <unistd.h>
#include <iostream>
#include <fstream>
#include <cstring>

pid_t pidOfSteam = 0;

char *steamExecutablePath;

pid_t ReadPidFromPidFile() {
    char ch;
    
    char* homeEnv = getenv("HOME");
    if (homeEnv == nullptr) {
        std::cerr << "[htmlhost_fakepid] HOME not set" << std::endl;
        return 0;
    }

    std::string pidfilePath = std::string(homeEnv);
    pidfilePath.append("/.steam/steam.pid");

    std::ifstream pidfile(pidfilePath, std::ios::in);
 
    if (!pidfile.is_open()) {
        std::cerr << "[htmlhost_fakepid] couldn't open " << pidfilePath << " for reading" << std::endl;
    }
    
    std::string pidStr;
    getline(pidfile, pidStr);

    return std::stoi(pidStr.c_str());
}

char *GetSteamExecutablePath() {
    char* opensteamExePath = getenv("OPENSTEAM_EXE_PATH");
    if (opensteamExePath == nullptr) {
        std::cerr << "[htmlhost_fakepid] OPENSTEAM_EXE_PATH not set" << std::endl;
        return nullptr;
    }

    return opensteamExePath;
}

extern "C" pid_t getpid() {
    if (pidOfSteam == 0) {
        pidOfSteam = ReadPidFromPidFile();
        std::cout << "[htmlhost_fakepid] getpid faked with " << pidOfSteam << std::endl;
    }

    return pidOfSteam;
}

extern "C" unsigned int Plat_GetExecutablePath(char* path, int length) {
    std::cout << "Plat_GetExecutablePath called: " << length << std::endl;
    if (steamExecutablePath == nullptr)
    {
        steamExecutablePath = GetSteamExecutablePath();
        if (steamExecutablePath != nullptr) {
            std::cout << "[htmlhost_fakepid] ExecutablePath faked with " << steamExecutablePath << std::endl;
        } else {
            *path = NULL;
            return false;
        }
    }

    size_t actualLength = strlen(steamExecutablePath)+1;
    // path = (char *)malloc(actualLength);
    memcpy(path, steamExecutablePath, actualLength);
    return true;


    // original
    // ssize_t readLength = readlink("/proc/self/exe",path,length - 1);
    // if (readLength != -1) {
    //     path[readLength] = '\0';
    //     return 1;
    // }
    // *path = '\0';
    // return 0;
}