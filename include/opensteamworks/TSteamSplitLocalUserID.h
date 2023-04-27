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

#ifndef TSTEAMSPLITLOCALUSERID_H
#define TSTEAMSPLITLOCALUSERID_H
#ifdef _WIN32
#pragma once
#endif


// Applications need to be able to authenticate Steam users from ANY instance.
// So a SteamIDTicket contains SteamGlobalUserID, which is a unique combination of 
// instance and user id.

// SteamLocalUserID is an unsigned 64-bit integer.
// For platforms without 64-bit int support, we provide access via a union that splits it into 
// high and low unsigned 32-bit ints.  Such platforms will only need to compare LocalUserIDs 
// for equivalence anyway - not perform arithmetic with them.
typedef struct TSteamSplitLocalUserID
{
	unsigned int	Low32bits;
	unsigned int	High32bits;
} TSteamSplitLocalUserID;


#endif // TSTEAMSPLITLOCALUSERID_H
