#pragma once
#include <Windows.h>
#include <string>
#include <iostream>

DWORD pidOfSteam;
LPSTR steamExecutablePath;

DWORD __stdcall GetCurrentProcessIdHook() {
    if (pidOfSteam == 0) {
        char *envvar = getenv("OPENSTEAM_PID");
        if (envvar == nullptr) {
            std::cerr << "[windowshooks.h] OPENSTEAM_PID not set" << std::endl;
            return 0;
        }
        pidOfSteam = std::stoi(envvar);
    }

    return pidOfSteam;
}

char *GetSteamExecutablePath() {
    char* opensteamExePath = getenv("OPENSTEAM_EXE_PATH");
    if (opensteamExePath == nullptr) {
        std::cerr << "[htmlhost_windowshooks] OPENSTEAM_EXE_PATH not set" << std::endl;
        return nullptr;
    }

    return opensteamExePath;
}

bool __cdecl Plat_GetExecutablePathHook(LPSTR path, DWORD length) {
    std::cout << "Plat_GetExecutablePath called: " << length << std::endl;
    if (steamExecutablePath == nullptr)
    {
        steamExecutablePath = GetSteamExecutablePath();
        if (steamExecutablePath != nullptr) {
            std::cout << "[htmlhost_windowshooks] ExecutablePath faked with " << steamExecutablePath << std::endl;
        } else {
            *path = NULL;
            return false;
        }
    }

    size_t actualLength = strlen(steamExecutablePath)+1;
    memcpy(path, steamExecutablePath, actualLength);
    return true;
}