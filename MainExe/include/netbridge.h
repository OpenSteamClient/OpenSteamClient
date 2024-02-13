#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <cstring>
#include <iostream> 
#include <vector>
#include <stdint.h>
#include <assert.h>
#include <chrono>
#include <thread>

#include <sharedlib.h>

// Coreclr imports
#include <netcore/hostfxr.h>
#include <netcore/nethost.h>
#include <netcore/coreclr_delegates.h>

#ifdef _WIN32
#include <windows.h>

#define STR(s) L ## s
#define CH(c) L ## c
#define DIR_SEPARATOR L'\\'

#define string_compare wcscmp

#else
#include <dlfcn.h>
#include <limits.h>

#define STR(s) s
#define CH(c) c
#define DIR_SEPARATOR '/'
#define MAX_PATH PATH_MAX

#define string_compare strcmp

#endif

using string_t = std::basic_string<char_t>;

class CNetBridge;

static CNetBridge *netbridge = nullptr;

// Bootstrapper funcs
typedef const char* (CORECLR_DELEGATE_CALLTYPE *pSteamBootstrapper_GetInstallDir_fn)();
typedef const char* (CORECLR_DELEGATE_CALLTYPE *pSteamBootstrapper_GetLoggingDir_fn)();
typedef bool (CORECLR_DELEGATE_CALLTYPE *pStartCheckingForUpdates_fn)();
typedef unsigned int (CORECLR_DELEGATE_CALLTYPE *pSteamBootstrapper_GetEUniverse_fn)();
typedef long long int (CORECLR_DELEGATE_CALLTYPE *pGetBootstrapperVersion_fn)();
typedef const char* (CORECLR_DELEGATE_CALLTYPE *pGetCurrentClientBeta_fn)();
typedef void (CORECLR_DELEGATE_CALLTYPE *pClientUpdateRunFrame_fn)();
typedef bool (CORECLR_DELEGATE_CALLTYPE *pIsClientUpdateAvailable_fn)();
typedef bool (CORECLR_DELEGATE_CALLTYPE *pCanSetClientBeta_fn)();
typedef const char* (CORECLR_DELEGATE_CALLTYPE *pSteamBootstrapper_GetBaseUserDir_fn)();




/*
    Utility class for interfacing with managed code
*/
class CNetBridge
{
private:
    int argc;
    char_t **argv;

    // hostfxr exports
    hostfxr_initialize_for_dotnet_command_line_fn init_for_cmd_line_fptr;
    hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
    hostfxr_get_runtime_delegate_fn get_delegate_fptr;
    hostfxr_run_app_fn run_app_fptr;
    hostfxr_close_fn close_fptr;
    load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer = nullptr;
    string_t managed_path;

    bool load_hostfxr(const char_t *app);
    load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t *assembly);
    int run_component(const string_t &root_path);
public:
    void *GetFunction(
        const string_t &className /* Assembly qualified type name */, 
        const string_t &funcName /* Public static method name compatible with delegateType */,
        char_t *delegate_type_name /* Assembly qualified delegate type name or null
                                        or UNMANAGEDCALLERSONLY_METHOD if the method is marked with
                                        the UnmanagedCallersOnlyAttribute. */ = nullptr);
    int Run();
    CNetBridge(int argc, char_t *argv[]);
    ~CNetBridge();

    pSteamBootstrapper_GetInstallDir_fn pSteamBootstrapper_GetInstallDir;
    pSteamBootstrapper_GetLoggingDir_fn pSteamBootstrapper_GetLoggingDir;
    pStartCheckingForUpdates_fn pStartCheckingForUpdates;
    pSteamBootstrapper_GetEUniverse_fn pSteamBootstrapper_GetEUniverse;
    pGetBootstrapperVersion_fn pGetBootstrapperVersion;
    pGetCurrentClientBeta_fn pGetCurrentClientBeta;
    pClientUpdateRunFrame_fn pClientUpdateRunFrame;
    pIsClientUpdateAvailable_fn pIsClientUpdateAvailable;
    pCanSetClientBeta_fn pCanSetClientBeta;
    pSteamBootstrapper_GetBaseUserDir_fn pSteamBootstrapper_GetBaseUserDir;
};