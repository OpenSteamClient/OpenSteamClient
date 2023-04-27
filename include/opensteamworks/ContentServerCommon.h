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

#ifndef CONTENTSERVERCOMMON_H
#define CONTENTSERVERCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define CLIENTCONTENTSERVER_INTERFACE_VERSION "CLIENTCONTENTSERVER_INTERFACE_VERSION001"

#define STEAMCONTENTSERVER_INTERFACE_VERSION_001 "SteamContentServer001"
#define STEAMCONTENTSERVER_INTERFACE_VERSION_002 "SteamContentServer002"



typedef enum EConnectionPriority
{
	k_EConnectionPriorityLow = 0,
	k_EConnectionPriorityMedium = 1,
	k_EConnectionPriorityHigh = 2,
} EConnectionPriority;


#pragma pack( push, 8 )
struct CSClientApprove_t
{
	enum { k_iCallback = k_iSteamContentServerCallbacks + 1 };
	
	CSteamID m_SteamID;
	uint32 m_uContentID;
};

struct CSClientDeny_t
{
	enum { k_iCallback = k_iSteamContentServerCallbacks + 2 };

	CSteamID m_SteamID;
	uint32 m_uContentID;
	EDenyReason m_eDenyReason;
};
#pragma pack( pop )

#endif // CONTENTSERVERCOMMON_H
