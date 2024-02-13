extern "C" const char* SteamBootstrapper_GetInstallDir() {
    return "";
}

extern "C" const char* SteamBootstrapper_GetLoggingDir() {
    return "";
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