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

#ifndef TSTEAMGLOBALUSERID_H
#define TSTEAMGLOBALUSERID_H
#ifdef _WIN32
#pragma once
#endif

// Applications need to be able to authenticate Steam users from ANY instance.
// So a SteamIDTicket contains SteamGlobalUserID, which is a unique combination of 
// instance and user id.
typedef struct TSteamGlobalUserID
{
	SteamInstanceID_t m_SteamInstanceID;

	union m_SteamLocalUserID
	{
		SteamLocalUserID_t		As64bits;
		TSteamSplitLocalUserID	Split;
	} m_SteamLocalUserID;

} TSteamGlobalUserID;


#endif // TSTEAMGLOBALUSERID_H
