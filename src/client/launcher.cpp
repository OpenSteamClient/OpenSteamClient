#include "globals.h"
#include "../common/commandline.h"
#include <string>
#include <opensteamworks/SteamTypes.h>
#include <opensteamworks/UserCommon.h>
#include <opensteamworks/version.h>

// fake bootstrapper executable exports

extern "C" ELauncherType GetClientLauncherType() { 
  return k_ELauncherTypeClientui; 
}

extern "C" ELauncherType GetClientActualLauncherType() { 
  return k_ELauncherTypeClientui; 
}

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
  return STEAM_MANIFEST_VERSION_NUM;
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

extern "C" bool CanSetClientBeta() {
  return false;
}