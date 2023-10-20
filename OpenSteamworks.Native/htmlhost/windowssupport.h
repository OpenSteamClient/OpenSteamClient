#pragma once
#include <windows.h>
#include <libloaderapi.h>

#define RTLD_NOW 0

void *dlopen(const char* libName, int unusedOnWindows) {
    std::cout << "[windowssupport] Loading " << libName << std::endl;
    auto handle = LoadLibraryA(libName);
    if (handle == nullptr){
        DWORD error = ::GetLastError();
        std::string message = std::system_category().message(error);
        std::cout << "[windowssupport] LoadLibraryA failed: " << message << std::endl;
    }

    return (void*)handle;
}

void *dlsym(void *handle, const char* funcName) {
    return (void*)GetProcAddress((HMODULE)handle, funcName);
}