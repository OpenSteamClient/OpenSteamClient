//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code, comments, and all related files, projects, resources,
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo,
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef CLIENTCOMMON_H
#define CLIENTCOMMON_H
#ifdef _WIN32
#pragma once
#endif

// interface predec
class ISteamClient;
class ISteamUser;
class ISteamGameServer;
class ISteamFriends;
class ISteamUtils;
class ISteamMatchmaking;
class ISteamContentServer;
class ISteamMasterServerUpdater;
class ISteamMatchmakingServers;
class ISteam2Bridge;
class ISteamUserStats;
class ISteamApps;
class ISteamBilling;
class IVAC;
class ISteamNetworking;
class ISteamRemoteStorage;
class ISteamGameServerItems;
class ISteamGameServerStats;
class ISteamHTTP;
class ISteamScreenshots;
class ISteamUnifiedMessages;
class ISteamController;
class ISteamUGC;
class ISteamAppList;
class ISteamMusic;
class ISteamMusicRemote;
class ISteamHTMLSurface;
class ISteamInventory;
class ISteamVideo;


#define CLIENTENGINE_INTERFACE_VERSION "CLIENTENGINE_INTERFACE_VERSION004"


#define STEAMCLIENT_INTERFACE_VERSION_006		"SteamClient006"
#define STEAMCLIENT_INTERFACE_VERSION_007		"SteamClient007"
#define STEAMCLIENT_INTERFACE_VERSION_008		"SteamClient008"
#define STEAMCLIENT_INTERFACE_VERSION_009		"SteamClient009"
#define STEAMCLIENT_INTERFACE_VERSION_010		"SteamClient010"
#define STEAMCLIENT_INTERFACE_VERSION_011		"SteamClient011"
#define STEAMCLIENT_INTERFACE_VERSION_012		"SteamClient012"
#define STEAMCLIENT_INTERFACE_VERSION_017		"SteamClient017"

#endif
