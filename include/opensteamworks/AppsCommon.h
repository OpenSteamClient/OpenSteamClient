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

#ifndef APPSCOMMON_H
#define APPSCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define CLIENTAPPS_INTERFACE_VERSION "CLIENTAPPS_INTERFACE_VERSION001"
#define CLIENTAPPMANAGER_INTERFACE_VERSION "CLIENTAPPMANAGER_INTERFACE_VERSION001"

#define STEAMAPPS_INTERFACE_VERSION_001 "STEAMAPPS_INTERFACE_VERSION001"
#define STEAMAPPS_INTERFACE_VERSION_002 "STEAMAPPS_INTERFACE_VERSION002"
#define STEAMAPPS_INTERFACE_VERSION_003 "STEAMAPPS_INTERFACE_VERSION003"
#define STEAMAPPS_INTERFACE_VERSION_004 "STEAMAPPS_INTERFACE_VERSION004"
#define STEAMAPPS_INTERFACE_VERSION_005 "STEAMAPPS_INTERFACE_VERSION005"
#define STEAMAPPS_INTERFACE_VERSION_006 "STEAMAPPS_INTERFACE_VERSION006"

#define STEAMAPPLIST_INTERFACE_VERSION_001 "STEAMAPPLIST_INTERFACE_VERSION001"

#include "EAppState.h"

enum EAppEvent
{
	k_EAppEventDownloadComplete = 2,
};

enum EAppInfoSection
{
	k_EAppInfoSectionUnknown = 0,
	k_EAppInfoSectionAll,
	k_EAppInfoSectionCommon,
	k_EAppInfoSectionExtended,
	k_EAppInfoSectionConfig,
	k_EAppInfoSectionStats,
	k_EAppInfoSectionInstall,
	k_EAppInfoSectionDepots,
	k_EAppInfoSectionVac,
	k_EAppInfoSectionDrm,
	k_EAppInfoSectionUfs,
	k_EAppInfoSectionOgg,
	k_EAppInfoSectionItems,
	k_EAppInfoSectionPolicies,
	k_EAppInfoSectionSysreqs,
	k_EAppInfoSectionCommunity
};

	// message AppUpdateInfo {
	// 	optional fixed32 time_update_start = 1;
	// 	optional uint64 bytes_to_download = 2;
	// 	optional uint64 bytes_downloaded = 3;
	// 	optional uint64 bytes_to_process = 4;
	// 	optional uint64 bytes_processed = 5;
	// 	optional int32 estimated_seconds_remaining = 6 [default = -1];
	// 	optional int32 update_result = 7;
	// 	optional uint32 update_state = 8;
	// }

enum EAppUpdateState
{
    k_EAppUpdateStateNone = 0,
    k_EAppUpdateStateRunningUpdate = 1,
    k_EAppUpdateStateReconfiguring = 2,
    k_EAppUpdateStateValidating = 4,
    k_EAppUpdateStatePreallocating = 16,
    k_EAppUpdateStateDownloading = 32,
    k_EAppUpdateStateStaging = 64,
    k_EAppUpdateStateVerifying = 128,
    k_EAppUpdateStateCommitting = 256,
    k_EAppUpdateStateRunningScript = 512,
    k_EAppUpdateStateStopping = 1024,
};


struct AppUpdateInfo_s
{
	// Unix timestamp when the download will auto start
	RTime32 m_timeUpdateStart;
	// Update state flags
	EAppUpdateState m_eAppUpdateState;
	uint64 m_unBytesToDownload;
	uint64 m_unBytesDownloaded;
	uint64 m_unBytesToProcess;
	uint64 m_unBytesProcessed;
	// What is this? 
	uint64 m_unBytesToProcess2;
	// What is this?
	uint64 m_unBytesProcessed2;
	uint64 m_uUnk3;
	uint64 m_uUnk4;
	uint64 m_uUnk5;
	EResult m_someError; // Some sort of flag or error var (value is 0 most of the time)
	uint32 m_uUnk6;
	uint64 m_uUnk7; // value is 4294967295 most of the time
	uint64 m_uUn8; // is possibly uint32 (value is 16777216 most of the time)
	uint64 m_targetBuildID; // Installing buildid
	uint64 m_uUnk10;
	uint64 m_uUnk11;
	uint64 m_uUnk12;
	uint64 m_uUnk13;
	uint64 m_uUnk14;
	uint64 m_uUnk15;
	uint64 m_uUnk16;
};

struct DownloadStats_s
{
	uint32 currentConnectionsCount;
	uint64 totalDownloaded;
	uint64 estimatedDownloadSpeed;
	char test1[16];
	char test2[96];
	uint64 test3;
};

enum EAppDownloadPriority
{
	k_EAppDownloadPriorityNone = 0,
	k_EAppDownloadPriorityFirst = 1,
	k_EAppDownloadPriorityUp = 2,
	k_EAppDownloadPriorityDown = 3,
	k_EAppDownloadPriorityLast = 4,
	k_EAppDownloadPriorityPaused = 5,
};

enum EAppUpdateError
{
	k_EAppErrorNone = 0,
	k_EAppErrorUnspecified = 1,
	k_EAppErrorPaused = 2,
	k_EAppErrorCanceled = 3,
	k_EAppErrorSuspended = 4,
	k_EAppErrorNoSubscription = 5,
	k_EAppErrorNoConnection = 6,
	k_EAppErrorTimeout = 7,
	k_EAppErrorMissingKey = 8,
	k_EAppErrorMissingConfig = 9,
	k_EAppErrorDiskReadFailure = 10,
	k_EAppErrorDiskWriteFailure = 11,
	k_EAppErrorCorruptContent = 13,
	k_EAppErrorWaitingForDisk = 14,
	k_EAppErrorInvalidInstallPath = 15,
	k_EAppErrorApplicationRunning = 16,
	k_EAppErrorDependencyFailure = 17,
	k_EAppErrorNotInstalled = 18,
	k_EAppErrorUpdateRequired = 19,
	k_EAppErrorStillBusy = 20,
	k_EAppErrorNoConnectionToContentServers = 21,
	k_EAppErrorInvalidApplicationConfiguration = 22,
	k_EAppErrorInvalidContentConfiguration = 23,
	k_EAppErrorMissingManifest = 24,
	k_EAppErrorNotReleased = 25,
	k_EAppErrorRegionRestricted = 26,
	k_EAppErrorCorruptDepotCache = 27,
	k_EAppErrorMissingExecutable = 28,
	k_EAppErrorInvalidPlatform = 29,
	k_EAppErrorInvalidFileSystem = 30,
	k_EAppErrorCorruptUpdateFiles = 31,
	k_EAppUpdateErrorDownloadCorrupt = 32,
	k_EAppUpdateErrorDownloadDisabled = 33,
	k_EAppUpdateErrorSharedLibraryLocked = 34,
	k_EAppUpdateErrorPurchasePending = 35,
	k_EAppUpdateErrorOtherSessionPlaying = 36,
};

//-----------------------------------------------------------------------------
// Purpose: possible results when registering an activation code
//-----------------------------------------------------------------------------
enum ERegisterActivactionCodeResult
{
	k_ERegisterActivactionCodeResultOK = 0,
	k_ERegisterActivactionCodeResultFail = 1,
	k_ERegisterActivactionCodeResultAlreadyRegistered = 2,
	k_ERegisterActivactionCodeResultTimeout = 3,
	k_ERegisterActivactionCodeAlreadyOwned = 4
};

enum EAppOwnershipFlags
{
	k_EAppOwnershipFlagsNone =				0,
	k_EAppOwnershipFlagsOwnsLicense =		1 << 0,
	k_EAppOwnershipFlagsFreeLicense =		1 << 1,
	k_EAppOwnershipFlagsRegionRestricted =	1 << 2,
	k_EAppOwnershipFlagsLowViolence =		1 << 3,
	k_EAppOwnershipFlagsInvalidPlatform =	1 << 4,
	k_EAppOwnershipFlagsSharedLicense =		1 << 5,
	k_EAppOwnershipFlagsFreeWeekend =		1 << 6,
	k_EAppOwnershipFlagsLockedLicense =		1 << 7,
	k_EAppOwnershipFlagsPending	=			1 << 8,
	k_EAppOwnershipFlagsExpired	=			1 << 9,
	k_EAppOwnershipFlagsPermanent	=		1 << 10,
	k_EAppOwnershipFlagsRecurring	=		1 << 11,
};

enum EAppReleaseState
{
	k_EAppReleaseStateUnknown = 0,
	k_EAppReleaseStateUnavailable,
	k_EAppReleaseStatePrerelease,
	k_EAppReleaseStatePreloadonly,
	k_EAppReleaseStateReleased,
};

enum EAppAutoUpdateBehavior
{
	// TODO: Reverse this enum
};

enum EAppAllowDownloadsWhileRunningBehavior
{
	// TODO: Reverse this enum
};

enum EAppDownloadQueuePlacement
{
	k_EAppDownloadQueuePlacementPriorityNone = 0,
    k_EAppDownloadQueuePlacementPriorityFirst = 1,
    k_EAppDownloadQueuePlacementPriorityUserInitiated = 2,
    k_EAppDownloadQueuePlacementPriorityUp = 3,
    k_EAppDownloadQueuePlacementPriorityDown = 4,
    k_EAppDownloadQueuePlacementPriorityAutoUpdate = 5,
    k_EAppDownloadQueuePlacementPriorityPaused = 6,
    k_EAppDownloadQueuePlacementPriorityManual = 7,
};

struct SHADigestWrapper_t
{
	uint32 A;
	uint32 B;
	uint32 C;
	uint32 D;
	uint32 E;
};

const int k_cubAppProofOfPurchaseKeyMax = 64;			// max bytes of a legacy cd key we support

#pragma pack( push, 8 )
//-----------------------------------------------------------------------------
// Purpose: called when new information about an app has arrived
//-----------------------------------------------------------------------------
struct AppDataChanged_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 1 };

	AppId_t m_nAppID;

	bool m_bBySteamUI;
	bool m_bCDDBUpdate;
};

struct RequestAppCallbacksComplete_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 2 };
};

struct AppInfoUpdateComplete_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 3 };

	EResult m_EResult;
	uint32 m_cAppsUpdated;
	bool m_bSteam2CDDBChanged;
};

struct AppEventTriggered_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 4 };

	AppId_t m_nAppID;
	EAppEvent m_eAppEvent;
};

//TODO: this is here until we reorganise OSW
struct SharedLibraryLockChanged_t 
{
	enum { k_iCallback = 1080004 };

	uint m_unLibraryOwner;
	uint m_unLibraryLockedBy;
	char m_szOwnerName[64];
};

//-----------------------------------------------------------------------------
// Purpose: posted after the user gains ownership of DLC & that DLC is installed
//-----------------------------------------------------------------------------
struct DlcInstalled_t
{
	enum
	{
		k_iCallback = k_iSteamAppsCallbacks + 5
	};

	AppId_t m_nAppID; // AppID of the DLC
};

struct AppEventStateChange_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 6 };

	AppId_t m_nAppID;
	uint32 m_eOldState;
	uint32 m_eNewState;
	EAppUpdateError m_eAppError;
};

struct AppValidationComplete_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 7 };

	AppId_t m_nAppID;
	bool m_bFinished;

	uint64 m_TotalBytesValidated;
	uint64 m_TotalBytesFailed;
	uint32 m_TotalFilesValidated;
	uint32 m_TotalFilesFailed;
	uint32 m_TotalFilesFailedCEGFiles;
};

//-----------------------------------------------------------------------------
// Purpose: response to RegisterActivationCode()
//-----------------------------------------------------------------------------
struct RegisterActivationCodeResponse_t
{
	enum { k_iCallback = k_iSteamAppsCallbacks + 8 };

	ERegisterActivactionCodeResult m_eResult;
	uint32 m_unPackageRegistered;						// package that was registered. Only set on success
};

struct DownloadScheduleChanged_t
{
	enum { k_iCallback = k_iClientAppsCallbacks + 9 };

	bool m_bDownloadEnabled;
	uint32 unk1;
	uint32 unk2;
	uint32 m_nTotalAppsScheduled;
	unsigned int m_rgunAppSchedule[32];
};

struct DlcInstallRequest_t
{
	enum { k_iCallback = k_iSteamAppsCallbacks + 10 };

	AppId_t m_nAppID;
	bool m_bInstall;
};

struct AppLaunchTenFootOverlay_t
{
	enum { k_iCallback = k_iSteamAppsCallbacks + 11 };

	CGameID m_GameID;
	uint64 m_nPid;
	bool m_bCanShareSurfaces;
};

struct AppBackupStatus_t
{
	enum { k_iCallback = k_iSteamAppsCallbacks + 12 };

	AppId_t m_nAppID;
	EResult m_eResult;
	
	uint64 m_unBytesToProcess;
	uint64 m_unBytesProcessed;
	uint64 m_unTotalBytesWritten;
	uint64 m_unBytesFailed;
};

struct RequestAppProofOfPurchaseKeyResponse_t
{
	enum { k_iCallback = k_iSteamAppsCallbacks + 13 };

	EResult m_eResult;
	AppId_t m_nAppID;
	char m_rgchKey[ k_cubAppProofOfPurchaseKeyMax ];	
};

#pragma pack( pop )

#endif // APPSCOMMON_H
