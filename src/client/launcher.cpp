#include "globals.h"
#include "../common/commandline.h"
#include <string>
#include <opensteamworks/SteamTypes.h>

// functions necessary to be exported so steamclient doesn't crash
// -singleapp launch mode is 8
// -steamchina launch mode is 7
// normal launch (no arguments) is 0
// returns a byte
extern "C" uint8_t GetClientLauncherType() { return 0; }
extern "C" uint8_t GetClientActualLauncherType() { return 0; }

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
extern "C" const char* SteamBootstrapper_GetBaseUserDir() {
  auto home_env = std::string(getenv("HOME"));
  auto newstr = home_env.append("/.local/share/Steam/test_dir");
  return newstr.c_str();
}
extern "C" const char* SteamBootstrapper_GetLoggingDir() {
  auto home_env = std::string(getenv("HOME"));
  auto newstr = home_env.append("/.local/share/Steam/logs2");
  return newstr.c_str();
}
extern "C" long long int GetBootstrapperVersion() {
  return 111111111111;
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