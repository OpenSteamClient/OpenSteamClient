#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <ostream>
#include <netbridge.h>

extern "C" const char* SteamBootstrapper_GetInstallDir() {
    return netbridge->pSteamBootstrapper_GetInstallDir();
}

extern "C" const char* SteamBootstrapper_GetLoggingDir() {
    return netbridge->pSteamBootstrapper_GetLoggingDir();
}

extern "C" bool StartCheckingForUpdates() {
  return netbridge->pStartCheckingForUpdates();
}

// First function called by steamclient
extern "C" unsigned int SteamBootstrapper_GetEUniverse() {
    return netbridge->pSteamBootstrapper_GetEUniverse();
}

extern "C" long long int GetBootstrapperVersion() {
    return netbridge->pGetBootstrapperVersion();
}

extern "C" const char* GetCurrentClientBeta() {
    return netbridge->pGetCurrentClientBeta();
}

extern "C" void ClientUpdateRunFrame() {
    netbridge->pClientUpdateRunFrame();
}

extern "C" bool IsClientUpdateAvailable() {
    return netbridge->pIsClientUpdateAvailable();
}

extern "C" bool CanSetClientBeta() {
    return netbridge->pCanSetClientBeta();
}

extern "C" const char* SteamBootstrapper_GetBaseUserDir() {
    return netbridge->pSteamBootstrapper_GetBaseUserDir();
}

extern "C" void PermitDownloadClientUpdates(bool permit) {
    netbridge->pPermitDownloadClientUpdates(permit);
}