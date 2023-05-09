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

#ifndef USERCOMMON_H
#define USERCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define CLIENTUSER_INTERFACE_VERSION "CLIENTUSER_INTERFACE_VERSION001"
#define CLIENTGAMESERVER_INTERFACE_VERSION "CLIENTGAMESERVER_INTERFACE_VERSION001"

#define STEAMUSER_INTERFACE_VERSION_004 "SteamUser004"
#define STEAMUSER_INTERFACE_VERSION_005 "SteamUser005"
#define STEAMUSER_INTERFACE_VERSION_006 "SteamUser006"
#define STEAMUSER_INTERFACE_VERSION_007 "SteamUser007"
#define STEAMUSER_INTERFACE_VERSION_008 "SteamUser008"
#define STEAMUSER_INTERFACE_VERSION_009 "SteamUser009"
#define STEAMUSER_INTERFACE_VERSION_010 "SteamUser010"
#define STEAMUSER_INTERFACE_VERSION_011 "SteamUser011"
#define STEAMUSER_INTERFACE_VERSION_012 "SteamUser012"
#define STEAMUSER_INTERFACE_VERSION_013 "SteamUser013"
#define STEAMUSER_INTERFACE_VERSION_014 "SteamUser014"
#define STEAMUSER_INTERFACE_VERSION_015 "SteamUser015"
#define STEAMUSER_INTERFACE_VERSION_016 "SteamUser016"
#define STEAMUSER_INTERFACE_VERSION_017 "SteamUser017"

// Callback values for callback ValidateAuthTicketResponse_t which is a response to BeginAuthSession
enum EAuthSessionResponse
{
	k_EAuthSessionResponseOK = 0,							// Steam has verified the user is online, the ticket is valid and ticket has not been reused.
	k_EAuthSessionResponseUserNotConnectedToSteam = 1,		// The user in question is not connected to steam
	k_EAuthSessionResponseNoLicenseOrExpired = 2,			// The license has expired.
	k_EAuthSessionResponseVACBanned = 3,					// The user is VAC banned for this game.
	k_EAuthSessionResponseLoggedInElseWhere = 4,			// The user account has logged in elsewhere and the session containing the game instance has been disconnected.
	k_EAuthSessionResponseVACCheckTimedOut = 5,				// VAC has been unable to perform anti-cheat checks on this user
	k_EAuthSessionResponseAuthTicketCanceled = 6,			// The ticket has been canceled by the issuer
	k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed = 7,	// This ticket has already been used, it is not valid.
	k_EAuthSessionResponseAuthTicketInvalid = 8,			// This ticket is not from a user instance currently connected to steam.
	k_EAuthSessionResponsePublisherIssuedBan = 9,			// The user is banned for this game. The ban came via the web api and not VAC
};

// results from BeginAuthSession
enum EBeginAuthSessionResult
{
	k_EBeginAuthSessionResultOK = 0,						// Ticket is valid for this game and this steamID.
	k_EBeginAuthSessionResultInvalidTicket = 1,				// Ticket is not valid.
	k_EBeginAuthSessionResultDuplicateRequest = 2,			// A ticket has already been submitted for this steamID
	k_EBeginAuthSessionResultInvalidVersion = 3,			// Ticket is from an incompatible interface version
	k_EBeginAuthSessionResultGameMismatch = 4,				// Ticket is not for this game
	k_EBeginAuthSessionResultExpiredTicket = 5,				// Ticket has expired
};

typedef enum EAppUsageEvent
{
	k_EAppUsageEventGameLaunch = 1,
	k_EAppUsageEventGameLaunchTrial = 2,
	k_EAppUsageEventMedia = 3,
	k_EAppUsageEventPreloadStart = 4,
	k_EAppUsageEventPreloadFinish = 5,
	k_EAppUsageEventMarketingMessageView = 6,	// deprecated, do not use
	k_EAppUsageEventInGameAdViewed = 7,
	k_EAppUsageEventGameLaunchFreeWeekend = 8,
} EAppUsageEvent;

typedef enum ERegistrySubTree
{
	k_ERegistrySubTreeNews = 0,
	k_ERegistrySubTreeApps = 1,
	k_ERegistrySubTreeSubscriptions = 2,
	k_ERegistrySubTreeGameServers = 3,
	k_ERegistrySubTreeFriends = 4,
	k_ERegistrySubTreeSystem = 5,
	k_ERegistrySubTreeAppOwnershipTickets = 6,
	k_ERegistrySubTreeLegacyCDKeys = 7,
} ERegistrySubTree;

typedef enum ELogonState
{
    k_ELogonStateLoggedOff = 0,
    k_ELogonStateConnecting = 1,
    k_ELogonStateConnected = 2,
    k_ELogonStateLoggingOn = 3,
    k_ELogonStateLoggedOn = 4,
    k_ELogonStateLoggingOff = 5,
} ELogonState;


typedef enum ELauncherType
{
	k_ELauncherTypeDefault = 0,
	k_ELauncherTypePerfectWorld = 1,
	k_ELauncherTypeNexon = 2,
	k_ELauncherTypeCmdLine = 3,
	k_ELauncherTypeCSGO = 4,
	k_ELauncherTypeClientUI = 5,
	k_ELauncherTypeHeadless = 6,
	k_ELauncherTypeSteamChina = 7,
	k_ELauncherTypeSingleApp = 8,
} ELauncherType;



typedef enum EPlatformType
{
	k_EPlatformTypeUnknown = 0,
	k_EPlatformTypeWin32 = 1,
	k_EPlatformTypeWin64 = 2,
	// k_EPlatformTypeLinux = 3, 		// removed, "split to Linux64 and Linux32"
	k_EPlatformTypeLinux64 = 3,
	k_EPlatformTypeOSX = 4,
	k_EPlatformTypePS3 = 5,
	k_EPlatformTypeLinux32 = 6,
}
EPlatformType;

//-----------------------------------------------------------------------------
// Purpose: types of VAC bans
//-----------------------------------------------------------------------------
typedef enum EVACBan
{
	k_EVACBanGoldsrc,
	k_EVACBanSource,
	k_EVACBanDayOfDefeatSource,
} EVACBan;

typedef enum EUserHasLicenseForAppResult
{
	k_EUserHasLicenseResultHasLicense = 0,					// User has a license for specified app
	k_EUserHasLicenseResultDoesNotHaveLicense = 1,			// User does not have a license for the specified app
	k_EUserHasLicenseResultNoAuth = 2,						// User has not been authenticated
} EUserHasLicenseForAppResult;

// Enum for the types of news push items you can get
typedef enum ENewsUpdateType
{
	k_EAppNews = 0,	 // news about a particular app
	k_ESteamAds = 1, // Marketing messages
	k_ESteamNews = 2, // EJ's corner and the like
	k_ECDDBUpdate = 3, // backend has a new CDDB for you to load
	k_EClientUpdate = 4,	// new version of the steam client is available
} ENewsUpdateType;

typedef enum ESteamUsageEvent
{
	k_ESteamUsageEventMarketingMessageView = 1,
	k_ESteamUsageEventHardwareSurvey = 2,
	k_ESteamUsageEventDownloadStarted = 3,
	k_ESteamUsageEventLocalizedAudioChange = 4,
	k_ESteamUsageEventClientGUIUsage = 5,
	k_ESteamUsageEventCharityChoice = 6,
} ESteamUsageEvent;

typedef enum EClientStat
{
	k_EClientStatP2PConnectionsUDP = 0,
	k_EClientStatP2PConnectionsRelay = 1,
	k_EClientStatP2PGameConnections = 2,
	k_EClientStatP2PVoiceConnections = 3,
	k_EClientStatBytesDownloaded = 4,
	k_EClientStatMax = 5,
} EClientStat;

//-----------------------------------------------------------------------------
// Purpose: Marketing message flags that change how a client should handle them
//-----------------------------------------------------------------------------
typedef enum EMarketingMessageFlags
{
	k_EMarketingMessageFlagsNone = 0,
	k_EMarketingMessageFlagsHighPriority = 1 << 0,
	k_EMarketingMessageFlagsPlatformWindows = 1 << 1,
	k_EMarketingMessageFlagsPlatformMac = 1 << 2,

	//aggregate flags
	k_EMarketingMessageFlagsPlatformRestrictions = 
	k_EMarketingMessageFlagsPlatformWindows | k_EMarketingMessageFlagsPlatformMac,
} EMarketingMessageFlags;

typedef enum ENatDiscoveryTypes
{
	eNatTypeUntested = 0,
	eNatTypeTestFailed = 1,
	eNatTypeNoUDP = 2,
	eNatTypeOpenInternet = 3,
	eNatTypeFullCone = 4,
	eNatTypeRestrictedCone = 5,
	eNatTypePortRestrictedCone = 6,
	eNatTypeUnspecified = 7,
	eNatTypeSymmetric = 8,
	eNatTypeSymmetricFirewall = 9,
	eNatTypeCount = 10,
} ENatType;

typedef enum EPhysicalSocketConnectionResult
{
	PhysicalSocket_Unknown = 0,
	PhysicalSocket_IsRemoteSide = 1,
	PhysicalSocket_Connected = 2,
	PhysicalSocket_Failed = 3,
	PhysicalSocket_SignalingFailed = 4,
	PhysicalSocket_ResultCount = 5,
} EPhysicalSocketConnectionResult;

class CNatTraversalStat
{
public:
	EPhysicalSocketConnectionResult m_eResult;
	ENatDiscoveryTypes m_eLocalNatType;
	ENatDiscoveryTypes m_eRemoteNatType;
	bool m_bMultiUserChat : 1;
	bool m_bRelay : 1;
};

class CAmount
{
public:
	int m_nAmount;
	ECurrencyCode m_eCurrencyCode;
};

enum EMicroTxnAuthResponse
{
	k_EMicroTxnAuthResponseInvalid = 0,
	k_EMicroTxnAuthResponseAuthorize = 1,
	k_EMicroTxnAuthResponseDeny = 2,
	k_EMicroTxnAuthResponseAutoDeny = 3,
};

enum EMicroTxnAuthResult
{
	k_EMicroTxnAuthResultInvalid = 0,
	k_EMicroTxnAuthResultOK = 1,
	k_EMicroTxnAuthResultFail = 2,
	k_EMicroTxnAuthResultInsufficientFunds = 3,
};

enum ERequestAccountDataAction
{
	k_ERequestAccountDataActionFindAccountsByEmailAddress = 1,
	k_ERequestAccountDataActionFindAccountsByCdKey = 2,
	k_ERequestAccountDataActionGetNumAccountsWithEmailAddress = 3,
	//k_ERequestAccountDataActionIsAccountNameInUse = 4, // Only used internally
};

enum ESteamGuardProvider
{
	// TODO: Reverse this enum
};

enum EUserConnect
{
	// TODO: Reverse this enum
};

#pragma pack( push, 8 )
//-----------------------------------------------------------------------------
// Purpose: called when a connections to the Steam back-end has been established
//			this means the Steam client now has a working connection to the Steam servers
//			usually this will have occurred before the game has launched, and should
//			only be seen if the user has dropped connection due to a networking issue
//			or a Steam server update
//-----------------------------------------------------------------------------
struct SteamServersConnected_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 1 };
};

struct PostLogonState_t 
{
	enum { k_iCallback = 1020087 };
	uint16_t unk1;         // 2 bytes
	uint16_t unk2;         // 2 bytes
	uint16_t unk3;         // 2 bytes
	bool logonComplete;         // 2 bytes
	uint16_t unk5;		   // 2 bytes
};

struct CheckAppBetaPasswordResponse_t
{
	enum { k_iCallback = 1280020 };
	AppId_t appid;
	EResult eResult;
	// These exist, however we don't really need these
	// FUN_00a75220(&local_328,8,1,"m_szBetaName","char",64);
    // FUN_00a75220(&local_328,72,1,"m_szBetaDesc","char",128);
};

#pragma endregion 
//-----------------------------------------------------------------------------
// Purpose: called when a connection attempt has failed
//			this will occur periodically if the Steam client is not connected, 
//			and has failed in it's retry to establish a connection
//-----------------------------------------------------------------------------
struct SteamServerConnectFailure_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 2 };

	EResult m_eResult;
	bool m_bStillRetrying;
};

//-----------------------------------------------------------------------------
// Purpose: called if the client has lost connection to the Steam servers
//			real-time services will be disabled until a matching SteamServersConnected_t has been posted
//-----------------------------------------------------------------------------
struct SteamServersDisconnected_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 3 };

	EResult m_eResult;
};

//-----------------------------------------------------------------------------
// Purpose: called when the client is trying to retry logon after being unintentionally logged off
//-----------------------------------------------------------------------------
struct OBSOLETE_CALLBACK BeginLogonRetry_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 4 };
};

//-----------------------------------------------------------------------------
// Purpose: Sent by the Steam server to the client telling it to disconnect from the specified game server,
//			which it may be in the process of or already connected to.
//			The game client should immediately disconnect upon receiving this message.
//			This can usually occur if the user doesn't have rights to play on the game server.
//-----------------------------------------------------------------------------
struct ClientGameServerDeny_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 13 };

	AppId_t m_uAppID;
	uint32 m_unGameServerIP;
	uint16 m_usGameServerPort;
	uint16 m_bSecure;
	uint32 m_uReason;
};

//-----------------------------------------------------------------------------
// Purpose: notifies the user that they are now the primary access point for chat messages
//-----------------------------------------------------------------------------
struct OBSOLETE_CALLBACK PrimaryChatDestinationSetOld_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 14 };
	uint8 m_bIsPrimary;
};

// See GSPolicyResponse_t in GameServerCommon.h for callback 115

//-----------------------------------------------------------------------------
// Purpose: called when the callback system for this client is in an error state (and has flushed pending callbacks)
//			When getting this message the client should disconnect from Steam, reset any stored Steam state and reconnect.
//			This usually occurs in the rare event the Steam client has some kind of fatal error.
//-----------------------------------------------------------------------------
struct IPCFailure_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 17 };
	enum EFailureType 
	{ 
		k_EFailureFlushedCallbackQueue, 
		k_EFailurePipeFail,
	};
	uint8 m_eFailureType;
};

struct LicensesUpdated_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 25 };
};

struct AppLifetimeNotice_t
{
	enum { k_iCallback = 1020030 }; 

	AppId_t m_nAppID;
	int32 m_nInstanceID;
	bool m_bExiting;
};

struct OBSOLETE_CALLBACK DRMSDKFileTransferResult_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 41 };

	EResult m_EResult;
};

//-----------------------------------------------------------------------------
// callback for BeginAuthSession
//-----------------------------------------------------------------------------
struct ValidateAuthTicketResponse_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 43 };

	CSteamID m_SteamID;
	EAuthSessionResponse m_eAuthSessionResponse;
};

//-----------------------------------------------------------------------------
// Purpose: called when a user has responded to a microtransaction authorization request
//-----------------------------------------------------------------------------
struct MicroTxnAuthorizationResponse_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 52 };

	uint32 m_unAppID;			// AppID for this microtransaction
	uint64 m_ulOrderID;			// OrderID provided for the microtransaction
	uint8 m_bAuthorized;		// if user authorized transaction
};

//-----------------------------------------------------------------------------
// Purpose: Result from RequestEncryptedAppTicket
//-----------------------------------------------------------------------------
struct EncryptedAppTicketResponse_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 54 };

	EResult m_eResult;
};

//-----------------------------------------------------------------------------
// callback for GetAuthSessionTicket
//-----------------------------------------------------------------------------
struct GetAuthSessionTicketResponse_t
{
	enum { k_iCallback = k_iSteamUserCallbacks + 63 };

	HAuthTicket m_hAuthTicket;
	EResult m_eResult;
};



// k_iClientUserCallbacks callbacks



struct SystemIM_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 1 };

	uint32 m_ESystemIMType;
	char m_rgchMsgBody[4096];
};

struct GuestPassGiftTarget_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 2 };

	uint32 m_unPackageID;
	uint64 m_ulSteamIDFriend;
	int32 m_iPotentialGiftTarget;
	int32 m_cPotentialGiftTargetsTotal;
	uint8 m_bValidGiftTarget;
};

struct PrimaryChatDestinationSet_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 3 };

	uint8 m_bIsPrimary;
	uint8 m_bWasPrimary;
};

// TODO: Add callback 904

struct LicenseChanged_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 5 };

	PackageId_t m_nPackageID;
};

struct RequestClientAppListInfo_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 6 };

	bool m_bMedia;
	bool m_bTools;
	bool m_bGames;
	bool m_bInstalled;
};

struct SetClientAppUpdateState_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 7 };

	uint64 m_ulJobIDToPostResultTo;
	AppId_t m_nAppID;
	bool m_bUpdate;
};

struct InstallClientApp_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 8 };

	uint64 m_ulJobIDToPostResultTo;
	AppId_t m_nAppID;
};

struct UninstallClientApp_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 9 };

	uint64 m_ulJobIDToPostResultTo;
	AppId_t m_nAppID;
};

//-----------------------------------------------------------------------------
// Purpose: called when the steam2 ticket has been set
//-----------------------------------------------------------------------------
struct OBSOLETE_CALLBACK Steam2TicketChanged_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 10 };
};

//-----------------------------------------------------------------------------
// Purpose: called when app news update is recieved
//-----------------------------------------------------------------------------
struct ClientAppNewsItemUpdate_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 11 };

	uint8 m_eNewsUpdateType;	// one of ENewsUpdateType
	uint32 m_uNewsID;			// unique news post ID
	uint32 m_uAppID;			// app ID this update applies to if it is of type k_EAppNews
};

//-----------------------------------------------------------------------------
// Purpose: steam news update
//-----------------------------------------------------------------------------
struct ClientSteamNewsItemUpdate_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 12 };

	uint8 m_eNewsUpdateType;	// one of ENewsUpdateType

	uint32 m_uNewsID;			// unique news post ID
	uint32 m_uHaveSubID;		// conditions to control if we display this update for type k_ESteamNews
	uint32 m_uNotHaveSubID;
	uint32 m_uHaveAppID;
	uint32 m_uNotHaveAppID;
	uint32 m_uHaveAppIDInstalled;
	uint32 m_uHavePlayedAppID;
};

//-----------------------------------------------------------------------------
// Purpose: steam cddb/bootstrapper update
//-----------------------------------------------------------------------------
struct ClientSteamNewsClientUpdate_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 13 };

	uint8 m_eNewsUpdateType;	// one of ENewsUpdateType
	uint8 m_bReloadCDDB;		// if true there is a new CDDB available
	uint32 m_unCurrentBootstrapperVersion;
	uint32 m_unCurrentClientVersion;
};

struct LegacyCDKeyRegistered_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 14 };

	EResult m_eResult;
	AppId_t m_iAppID;
	char m_rgchCDKey[ 64 ];
};

struct AccountInformationUpdated_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 15 };

	bool m_bEmailValidationAction;
};

struct GuestPassSent_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 16 };

	EResult m_eResult;
};

struct GuestPassAcked_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 17 };

	EResult m_eResult;

	PackageId_t m_unPackageID;

	GID_t m_gidGuestPassID;
	uint64 m_ulGuestPassKey;
};

struct GuestPassRedeemed_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 18 };

	EResult m_eResult;
	uint32 m_unPackageID;
};

struct UpdateGuestPasses_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 19 };

	EResult m_eResult;

	uint32 m_cGuestPassesToGive;
	uint32 m_cGuestPassesToRedeem;
};

struct LogOnCredentialsChanged_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 20 };
};

struct CheckPasswordResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 22 };

	EResult m_EResult;
};

struct ResetPasswordResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 23 };

	EResult m_EResult;
};

struct DRMDataRequest_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 24 };

	uint32 m_EResult;
	uint32 m_unAppID;
	bool m_bRestartApp;
};

struct DRMDataResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 25 };

	// TODO : Reverse this callback
};

struct DRMFailureResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 26 };

	// TODO : Reverse this callback
};

struct AppOwnershipTicketReceived_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 28 };

	AppId_t m_nAppID;
};

struct PasswordChangeResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 29 };

	EResult m_EResult;
};

struct EmailChangeResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 30 };

	EResult m_EResult;
	bool m_bFinal;
};

struct SecretQAChangeResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 31 };

	EResult m_EResult;
};

struct CreateAccountResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 32 };

	EResult m_EResult;
};

struct SendForgottonPasswordEmailResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 33 };

	EResult m_EResult;
};

struct ResetForgottonPasswordResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 34 };

	EResult m_EResult;
};

struct CreateAccountInformSteam3Response_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 35 };

	uint32 m_EResult;
};

struct DownloadFromDFSResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 36 };

	EResult m_EResult;

	char m_rgchURL[ 128 ];
};

struct ClientMarketingMessageUpdate_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 37 };
};

struct ValidateEmailResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 38 };

	uint32 m_EResult;
};

struct RequestChangeEmailResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 39 };

	uint32 m_EResult;
};

struct VerifyPasswordResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 40 };

	uint32 m_EResult;
};

struct Steam2LoginResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 41 };

	bool m_bSuccessful;
	uint32 m_steam2Error;
};

struct WebAuthRequestCallback_t
{
	enum { k_iCallback = 1020042 };
	
	bool m_bSuccessful;
	char m_rgchToken[1024];
};

struct MicroTxnAuthRequestCallback_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 44 };

	GID_t m_gidTransID;
	AppId_t m_unAppID;
};

struct MicroTxnAuthResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 45 };

	EMicroTxnAuthResult m_eAuthResult;
};

struct AppMinutesPlayedDataNotice_t
{
	enum { k_iCallback = 1020046 };

	int32 m_nAppID;
};

struct MicroTxnInfoUpdated_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 47 };

	EResult m_eResult;
	GID_t m_gidTransID;
};

struct WalletBalanceUpdated_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 48 };
};

struct EnableMachineLockingResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 49 };

	uint32 m_EResult;
};

struct MachineLockProgressResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 50 };

	uint32 m_EResult;
};

struct Steam3ExtraLoginProgress_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 51 }; // Note sure

	uint32 m_EResult;
	int32 m_eState;
};

struct RequestAccountDataResult_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 52 };

	EResult m_EResult;
	uint32 m_cMatches;
	ERequestAccountDataAction m_eAction;
};

struct IsAccountNameInUseResult_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 53 };

	EResult m_EResult;
	char m_szAccountNameSuggestion1[64];
	char m_szAccountNameSuggestion2[64];
	char m_szAccountNameSuggestion3[64];
};

struct LoginInformationChanged_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 55 };

	// TODO : Reverse this callback
};

struct RequestSpecialSurveyResult_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 56 };

	int32 m_iSurveyID;
	EResult m_eResult;
	/*ESurveyState*/int32 m_eState; // 1 = denied, 4 = already complete
	char m_szName[256];
	char m_szCustomURL[512];
	bool m_bIncludeSoftware; 
	uint8 m_ubToken[16];
};

struct SendSpecialSurveyResponseResult_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 57 };

	int32 m_iSurveyID;
	EResult m_eResult;
	uint8 m_ubToken[16];
};

struct UpdateItemAnnouncement_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 58 };

	uint32 m_cNewItems;
};

struct ChangeSteamGuardOptionsResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 59 };

	EResult m_eResult;
};

struct UpdateCommentNotification_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 60 };

	uint32 m_cNewComments;
	uint32 m_cNewCommentsOwner;
	uint32 m_cNewCommentsSubscriptions;
};

struct FriendUserStatusPublished_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 61 };

	CSteamID m_steamIDFriend;
	AppId_t m_unAppID;
	char m_szStatus[512];
};

struct UpdateOfflineMessageNotification_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 62 };
	
	// TODO : Reverse this callback
};

struct FriendMessageHistoryChatLog_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 63 };
	
	// TODO : Reverse this callback
};

struct TestAvailablePasswordResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 64 };
	
	// TODO : Reverse this callback
};

// 65 ??

struct GetSteamGuardDetailsResponse_t
{
	enum { k_iCallback = k_iClientUserCallbacks + 66 };
	
	// TODO : Reverse this callback
};

struct AppLastPlayedTimeChanged_t
{
	enum { k_iCallback = 1020070 };
	AppId_t m_nAppID;
	RTime32 m_rtimeLastPlayed;
};

struct AppLicensesChanged_t
{
	enum { k_iCallback = 1020094 };
	bool bReloadAll;
	uint32 m_unAppsUpdated;
	AppId_t m_rgAppsUpdated[64];
};

#pragma pack( pop )


#endif // USERCOMMON_H
