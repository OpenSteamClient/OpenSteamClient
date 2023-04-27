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

#ifndef TSTEAMAPPLAUNCHOPTION_H
#define TSTEAMAPPLAUNCHOPTION_H
#ifdef _WIN32
#pragma once
#endif

typedef struct TSteamAppLaunchOption
{
	char *szDesc;
	unsigned int uMaxDescChars;
	char *szCmdLine;
	unsigned int uMaxCmdLineChars;
	unsigned int uIndex;
	unsigned int uIconIndex;
	int bNoDesktopShortcut;
	int bNoStartMenuShortcut;
	int bIsLongRunningUnattended;

} TSteamAppLaunchOption;

#endif // TSTEAMAPPLAUNCHOPTION_H
