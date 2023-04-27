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

#ifndef ICLIENTPRODUCTBUILDER_H
#define ICLIENTPRODUCTBUILDER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"


// Not a typo, it seem that IClientProductBuilder's version string is really the same as IClientDepotBuilder's.
// Valid as of Steamclient beta 22nd March 2014 (1395164792)
#define CLIENTPRODUCTBUILDER_INTERFACE_VERSION "CLIENTDEPOTBUILDER_INTERFACE_VERSION001"


abstract_class IClientProductBuilder
{
public:
	virtual uint64 SignInstallScript( uint32, const char *, const char * ) = 0;
	virtual uint64 DRMWrap( uint32, const char *, const char *, const char *, uint32 ) = 0;
	virtual uint64 CEGWrap( uint32, const char *, const char *, const char * ) = 0;
};

#endif // ICLIENTPRODUCTBUILDER_H
