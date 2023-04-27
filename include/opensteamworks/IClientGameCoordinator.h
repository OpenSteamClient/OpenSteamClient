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

#ifndef ICLIENTGAMECOORDINATOR_H
#define ICLIENTGAMECOORDINATOR_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "GameCoordinatorCommon.h"


abstract_class IClientGameCoordinator
{
public:
	virtual EGCResults SendMessage( AppId_t unAppID, uint32 unMsgType, const void *pubData, uint32 cubData ) = 0;

	virtual bool IsMessageAvailable( AppId_t unAppID, uint32 *pcubMsgSize ) = 0;

	virtual EGCResults RetrieveMessage( AppId_t unAppID, uint32 *punMsgType, void *pubDest, uint32 cubDest, uint32 *pcubMsgSize ) = 0;
};

#endif
