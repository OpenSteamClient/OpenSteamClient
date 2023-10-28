#pragma once
#include <Windows.h>
#include <string>
#include <iostream>
#include <memoryapi.h>

DWORD pidOfSteam;
LPSTR steamExecutablePath;

DWORD __cdecl GetCurrentProcessIdHook() {
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
            *path = 0;
            return false;
        }
    }

    size_t actualLength = strlen(steamExecutablePath)+1;
    memcpy(path, steamExecutablePath, actualLength);
    return true;
}

void WindowsHookFunc(HMODULE libPtr, const char* funcName, void* hookFunc) {
    auto origFuncPtr = GetProcAddress(libPtr, funcName);
    char patch[5]= {0};
    char saved_buffer[5];
    ReadProcessMemory(GetCurrentProcess(), origFuncPtr, saved_buffer, 5, NULL);

    DWORD src = (DWORD)origFuncPtr + 5; 
    DWORD dst = (DWORD)hookFunc;
    DWORD *relative_offset = (DWORD *)(dst-src); 

    memcpy(patch, "\xE9", 1);
    memcpy(patch + 1, &relative_offset, 4);

    WriteProcessMemory(GetCurrentProcess(), (LPVOID)origFuncPtr, patch, 5, NULL);
}

void WindowsHookFunc(const char* libName, const char* funcName, void* hookFunc) {
    auto lib = LoadLibraryA(TEXT(libName));
    WindowsHookFunc(lib, funcName, hookFunc);
}