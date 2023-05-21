#include "globals.h"
#include "../common/commandline.h"
#include <string>
#include <opensteamworks/SteamTypes.h>
#include <opensteamworks/UserCommon.h>

// functions necessary to be exported so steamclient doesn't crash

extern "C" uint8_t GetClientLauncherType() { return k_ELauncherTypeClientui; }
extern "C" uint8_t GetClientActualLauncherType() { return k_ELauncherTypeClientui; }

extern "C" bool StartCheckingForUpdates() {
  return true;
}

extern "C" const char* SteamBootstrapper_GetInstallDir() {
  auto home_env = std::string(getenv("HOME"));
  auto newstr = home_env.append("/.local/share/Steam");
  return newstr.c_str();
}

extern "C" EUniverse SteamBootstrapper_GetEUniverse() {
  return k_EUniversePublic;
}

extern "C" long long int GetBootstrapperVersion() {
  return 0;
}

extern "C" const char* GetCurrentClientBeta() {
  return "";
}

extern "C" char* SteamBootstrapper_GetForwardedCommandLine() {
  return (*Global_CommandLine->argv);
}

extern "C" void ClientUpdateRunFrame() {
  return; 
}

extern "C" bool IsClientUpdateAvailable() {
  return false;
}

// updating is unsupported for now
extern "C" bool CanSetClientBeta() {
  return false;
}