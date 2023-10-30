#pragma once
#include <windows.h>
#include <string>
#include <iostream>
#include <memoryapi.h>
#include "ghidradefines.h"

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

uint __cdecl GetOSTypeHook() {
    return 10;
}

// meh, always assume true
bool __cdecl Is64BitOSHook() {
    return true;
}

LPWSTR LCSTRToLPCWSTR(LPCSTR param_1) {
    if (param_1 == nullptr) {
        return nullptr;
    }
    
    int wchars_num = MultiByteToWideChar(CP_UTF8, 0, param_1, -1, NULL, 0);
    wchar_t* wstr = new wchar_t[wchars_num];
    MultiByteToWideChar(CP_UTF8, 0, param_1, -1, wstr, wchars_num );
    return wstr;
}

BOOL __cdecl CreateProcessUTF8(LPCSTR param_1,LPCSTR param_2,LPSECURITY_ATTRIBUTES param_3,
                 LPSECURITY_ATTRIBUTES param_4,BOOL param_5,DWORD param_6,LPVOID param_7,
                 LPCSTR param_8,int param_9,LPPROCESS_INFORMATION param_10)
{
  LPWSTR lpCurrentDirectory = LCSTRToLPCWSTR(param_8);
  LPWSTR lpCommandLine = LCSTRToLPCWSTR(param_2);
  LPSTARTUPINFOW lpStartupInfo = new STARTUPINFOW();

  std::cout << "Calling CreateProcessW with " << param_2 << std::endl;
  bool result = CreateProcessW(NULL, lpCommandLine, param_3, param_4, param_5, param_6, param_7,
                               lpCurrentDirectory, lpStartupInfo, param_10);
  auto error = GetLastError();
  std::cout << "CreateProcessW returned " << result << " with error " << error << std::endl;
  free(lpCurrentDirectory);
  free(lpCommandLine);
  free(lpStartupInfo);
  SetLastError(error);
  return result;
}

undefined4 __cdecl CreateSimpleProcessHook(LPCSTR param_1, uint param_2) {
  _PROCESS_INFORMATION processInformation;
  processInformation.hProcess = (HANDLE)0x0;
  processInformation.hThread = (HANDLE)0x0;
  processInformation.dwProcessId = 0;
  processInformation.dwThreadId = 0;
  undefined4 local_58 = 0x44;
  BOOL BVar2 = CreateProcessUTF8((LPCSTR)0x0,param_1,(LPSECURITY_ATTRIBUTES)0x0,
                        (LPSECURITY_ATTRIBUTES)0x0,0,(param_2 & 1) << 0x1b,(LPVOID)0x0,
                        (LPCSTR)0x0,(int)&local_58,&processInformation);
  HANDLE pvVar1 = processInformation.hProcess;
  if (BVar2 == 0) {
    return 0xffffffff;
  }
  CloseHandle(processInformation.hThread);
  return (int)pvVar1;
}

void WindowsHookFunc(HMODULE libPtr, const char* funcName, void* hookFunc) {
    auto origFuncPtr = GetProcAddress(libPtr, funcName);
    char patch[5] = {0};
    char saved_buffer[5];
    ReadProcessMemory(GetCurrentProcess(), (void*)origFuncPtr, saved_buffer, 5, NULL);

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

