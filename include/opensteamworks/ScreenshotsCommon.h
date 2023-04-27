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

#ifndef SCREENSHOTSCOMMON_H
#define SCREENSHOTSCOMMON_H
#ifdef _WIN32
#pragma once
#endif


#include "RemoteStorageCommon.h"


// versions
#define CLIENTSCREENSHOTS_INTERFACE_VERSION "CLIENTSCREENSHOTS_INTERFACE_VERSION001"
#define STEAMSCREENSHOTS_INTERFACE_VERSION_001 "STEAMSCREENSHOTS_INTERFACE_VERSION001"
#define STEAMSCREENSHOTS_INTERFACE_VERSION_002 "STEAMSCREENSHOTS_INTERFACE_VERSION002"

// Handle is valid for the lifetime of your process and no longer
typedef uint32 ScreenshotHandle;
#define INVALID_SCREENSHOT_HANDLE 0

const uint32 k_nScreenshotMaxTaggedUsers = 32;
const int k_cubUFSTagTypeMax = 255;
const int k_cubUFSTagValueMax = 255;

// Required with of a thumbnail provided to AddScreenshotToLibrary.  If you do not provide a thumbnail
// one will be generated.
const int k_ScreenshotThumbWidth = 200;


// callbacks
#pragma pack( push, 8 )

//-----------------------------------------------------------------------------
// Purpose: Screenshot successfully written or otherwise added to the library
// and can now be tagged
//-----------------------------------------------------------------------------
struct ScreenshotReady_t
{
	enum { k_iCallback = k_iSteamScreenshotsCallbacks + 1 };

	ScreenshotHandle m_hLocal;
	EResult m_eResult;
};

//-----------------------------------------------------------------------------
// Purpose: Screenshot has been requested by the user.  Only sent if
// HookScreenshots() has been called, in which case Steam will not take
// the screenshot itself.
//-----------------------------------------------------------------------------
struct ScreenshotRequested_t
{
	enum { k_iCallback = k_iSteamScreenshotsCallbacks + 2 };
};



// k_iClientScreenshotsCallbacks callbacks



struct ScreenshotUploadProgress_t
{
	enum { k_iCallback = k_iClientScreenshotsCallbacks + 1 };

	double m_dPercentScreenshot;
	double m_dPercentBatch;
	int32 m_nFailed;
};

struct ScreenshotWritten_t
{
	enum { k_iCallback = k_iClientScreenshotsCallbacks + 2 };

	ScreenshotHandle m_hLocal;
	CGameID m_gameID;
	RTime32 m_timeCreated;
	uint32 m_nWidth;
	uint32 m_nHeight;
};

struct ScreenshotUploaded_t
{
	enum { k_iCallback = k_iClientScreenshotsCallbacks + 3 };

	ScreenshotHandle m_hLocal;
	CGameID m_gameID;
	UGCHandle_t m_hFile;
	RTime32 m_timeCreated;
	uint32 m_nWidth;
	uint32 m_nHeight;
	EUCMFilePrivacyState m_ePrivacy;
	char m_pchCaption[540];
	uint8 pubUnknownData[28];
};

struct ScreenshotBatchComplete_t
{
	enum { k_iCallback = k_iClientScreenshotsCallbacks + 4 };

	int m_nAttempted;
	int m_nResultsOK;
};

struct ScreenshotDeleted_t
{
	enum { k_iCallback = k_iClientScreenshotsCallbacks + 5 };

	EResult m_eResult;
	ScreenshotHandle m_hLocal;
	CGameID m_gameID;
};

struct ScreenshotTriggered_t
{
	enum { k_iCallback = k_iClientScreenshotsCallbacks + 6 };

	CGameID m_gameID;
};


#pragma pack( pop )


#endif // SCREENSHOTSCOMMON_H
