// needs these headers
#include <cstdio>
#include <iostream>

// This is now stored in CMakeLists.txt, since the build kept complaining otherwise
// #define STEAMWORKS_CLIENT_INTERFACES 1

// For convenience
#include "../globals.h"

#include <opensteamworks/IClientAppManager.h>
#include <opensteamworks/IClientEngine.h>
#include <opensteamworks/SteamTypes.h>