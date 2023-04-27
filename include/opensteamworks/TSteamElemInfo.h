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

#ifndef TSTEAMELEMINFO_H
#define TSTEAMELEMINFO_H
#ifdef _WIN32
#pragma once
#endif

typedef struct TSteamElemInfo
{
	int bIsDir;						/* If non-zero, element is a directory; if zero, element is a file */
	unsigned int uSizeOrCount;		/* If element is a file, this contains size of file in bytes */
	int bIsLocal;					/* If non-zero, reported item is a standalone element on local filesystem */
	char cszName[STEAM_MAX_PATH];	/* Base element name (no path) */
	long lLastAccessTime;			/* Seconds since 1/1/1970 (like time_t) when element was last accessed */
	long lLastModificationTime;		/* Seconds since 1/1/1970 (like time_t) when element was last modified */
	long lCreationTime;				/* Seconds since 1/1/1970 (like time_t) when element was created */
} TSteamElemInfo;

typedef struct TSteamElemInfo64
{
	int bIsDir;							/* If non-zero, element is a directory; if zero, element is a file */
	unsigned long long ullSizeOrCount;	/* If element is a file, this contains size of file in bytes */
	int bIsLocal;						/* If non-zero, reported item is a standalone element on local filesystem */
	char cszName[STEAM_MAX_PATH];		/* Base element name (no path) */
	long long llLastAccessTime;			/* Seconds since 1/1/1970 (like time_t) when element was last accessed */
	long long llLastModificationTime;	/* Seconds since 1/1/1970 (like time_t) when element was last modified */
	long long llCreationTime;			/* Seconds since 1/1/1970 (like time_t) when element was created */
} TSteamElemInfo64;

#endif // TSTEAMELEMINFO_H
