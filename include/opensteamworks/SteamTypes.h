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

#ifndef STEAMTYPES_H
#define STEAMTYPES_H
#ifdef _WIN32
#pragma once
#endif

// Compiler checks

#if defined(_MSC_VER)

	#if _MSC_VER < 1400
		#error "OpenSteamworks requires MSVC 2005 or better"
	#endif
	
#elif defined(__GNUC__)

	#if defined(_WIN32)
		#if (__GNUC__ < 4 || (__GNUC__ == 4 && __GNUC_MINOR__ < 6)) && !defined(_S4N_)
			#error "OpenSteamworks requires GCC 4.6 or better on windows"
		#endif
	#elif defined(__linux__) || defined(__APPLE_CC__)
		#if __GNUC__ < 4
			#error "OpenSteamworks requires GCC 4.X (4.2 or 4.4 have been tested)"
		#endif
	#else
		#error "Unsupported OS: OpenSteamworks can only be used with Windows, Mac OS X or Linux"
	#endif
	
#else
	#error "Unsupported compiler: OpenSteamworks can only be used with MSVC, GCC or CLANG"
#endif



#if defined(_WIN32)
	#ifndef _S4N_
		#if defined(_MSC_VER) && _MSC_VER > 1400
			#include <sdkddkver.h>
		#else
			#if !defined(_WIN32_WINNT)
				#define _WIN32_WINNT 0x0502
			#endif
			#if !defined(WINVER)
				#define WINVER _WIN32_WINNT
			#endif
		#endif
			
		#include <windows.h>

		#undef SendMessage		// for ISteamGameCoordinator001 to work right..
		#undef CreateProcess	// for ISteam2Bridge
	#endif

	#if defined( STEAM_API_EXPORTS )
		#define S_API extern "C" __declspec( dllexport ) 
	#else
		#define S_API extern "C" __declspec( dllimport ) 
	#endif // STEAM_API_EXPORTS
#else
	#include <dlfcn.h> // dlopen,dlclose, et al
	#include <unistd.h>
	#include <arpa/inet.h>
	#include <string.h>

	#define S_API extern "C"
#endif


#ifdef __GNUC__
	typedef int errno_t;
	
	#ifdef _S4N_
		typedef unsigned int size_t;
		#define NULL 0
	#endif
#endif

#ifdef _WIN32
	#define STEAM_CALL __cdecl
#else
	#define STEAM_CALL
#endif

#if defined(__x86_64__) || defined(_WIN64)
	#define X64BITS
#endif

typedef unsigned char uint8;
typedef signed char int8;

#if _MSC_VER == 1200
// msvc6 only targets win32
typedef char int8;
typedef unsigned char uint8;

typedef short int16;
typedef unsigned short uint16;

typedef int int32;
typedef unsigned int uint32;

typedef __int64 int64;
typedef unsigned __int64 uint64;
#else
#include <stdint.h>
typedef int8_t int8;
typedef uint8_t uint8;

typedef int16_t int16;
typedef uint16_t uint16;

typedef int32_t int32;
typedef uint32_t uint32;

typedef int64_t int64;
typedef uint64_t uint64;
#endif


#ifndef abstract_class
	#ifdef _MSC_VER
		#define abstract_class class __declspec( novtable )
	#else
		#define abstract_class class
	#endif
#endif


#ifdef _MSC_VER
	#define STEAMWORKS_DEPRECATE( Message ) __declspec(deprecated(#Message))
#elif defined(__GNUC__)
	#if (__GNUC__ > 4 || (__GNUC__ == 4 && __GNUC_MINOR__ >= 5))
		#define STEAMWORKS_DEPRECATE( Message ) __attribute__((__deprecated__(#Message)))
	#else
		#define STEAMWORKS_DEPRECATE( Message ) __attribute__((__deprecated__))
	#endif
#else
	#define STEAMWORKS_DEPRECATE( Message )
#endif

#ifndef STEAMWORKS_OBSOLETE_INTERFACES
	#define OBSOLETE_INTERFACE STEAMWORKS_DEPRECATE("This interface is obsolete and is not available in the latest builds of Steam. #define STEAMWORKS_OBSOLETE_INTERFACES to suppress this warning.")
#else
	#define OBSOLETE_INTERFACE
#endif

#ifndef STEAMWORKS_OBSOLETE_FUNCTIONS
	#define OBSOLETE_FUNCTION STEAMWORKS_DEPRECATE("This function is obsolete and is not available in the latest builds of Steam. #define STEAMWORKS_OBSOLETE_FUNCTIONS to suppress this warning.")
#else
	#define OBSOLETE_FUNCTION
#endif

#ifndef STEAMWORKS_OBSOLETE_CALLBACKS
	#define OBSOLETE_CALLBACK STEAMWORKS_DEPRECATE("This callback is obsolete and will not be triggered in the latest builds of Steam. #define STEAMWORKS_OBSOLETE_CALLBACKS to suppress this warning.")
#else
	#define OBSOLETE_CALLBACK
#endif

#ifndef STEAMWORKS_CLIENT_INTERFACES
	#define UNSAFE_INTERFACE STEAMWORKS_DEPRECATE("IClient interfaces are unversioned and potentially unsafe. Class definition can change between steamclient releases. #define STEAMWORKS_CLIENT_INTERFACES to suppress this warning.")
#else
	#define UNSAFE_INTERFACE
#endif

#ifndef STEAM_API_NON_VERSIONED_INTERFACES
	#ifdef _MSC_VER
		#define S_API_UNSAFE extern "C" __declspec( dllexport ) STEAMWORKS_DEPRECATE("Steam*() interface accessing functions are unversioned and potentially unsafe. These are versioned to assume you are using the latest version of the steam_api loader, if this is not the case your code is likely to crash, read the comment above the functions to learn about the version safe accessing method that will account for newer steam_api versions, older versions are always unsupported. #define STEAM_API_NON_VERSIONED_INTERFACES to suppress this warning.")
	#elif defined(__GNUC__)
		#if defined(__linux__) || defined(__APPLE_CC__)
			#define S_API_UNSAFE extern "C" STEAMWORKS_DEPRECATE("Steam*() interface accessing functions are unversioned and potentially unsafe. These are versioned to assume you are using the latest version of the steam_api loader, if this is not the case your code is likely to crash, read the comment above the functions to learn about the version safe accessing method that will account for newer steam_api versions, older versions are always unsupported. #define STEAM_API_NON_VERSIONED_INTERFACES to suppress this warning.")
		#else
			#define S_API_UNSAFE extern "C" __declspec( dllexport ) STEAMWORKS_DEPRECATE("Steam*() interface accessing functions are unversioned and potentially unsafe. These are versioned to assume you are using the latest version of the steam_api loader, if this is not the case your code is likely to crash, read the comment above the functions to learn about the version safe accessing method that will account for newer steam_api versions, older versions are always unsupported. #define STEAM_API_NON_VERSIONED_INTERFACES to suppress this warning.")
		#endif
	#else
		#define S_API_UNSAFE extern "C"
	#endif
#else
	#ifdef _WIN32
		#define S_API_UNSAFE extern "C" __declspec( dllexport )
	#else
		#define S_API_UNSAFE extern "C"
	#endif
#endif

#if defined(_WIN32) && defined(__GNUC__) && !defined(_S4N_)
	#define STEAMWORKS_STRUCT_RETURN_0(returnType, functionName)	\
		virtual void functionName( returnType& ret ) = 0;			\
		inline returnType functionName()							\
		{															\
			returnType ret;											\
			this->functionName(ret);								\
			return ret;												\
		}
	#define STEAMWORKS_STRUCT_RETURN_1(returnType, functionName, arg1Type, arg1Name)	\
		virtual void functionName( returnType& ret, arg1Type arg1Name ) = 0;			\
		inline returnType functionName( arg1Type arg1Name )								\
		{																				\
			returnType ret;																\
			this->functionName(ret, arg1Name);											\
			return ret;																	\
		}
	#define STEAMWORKS_STRUCT_RETURN_2(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name)	\
		virtual void functionName( returnType& ret, arg1Type arg1Name, arg2Type arg2Name ) = 0;				\
		inline returnType functionName( arg1Type arg1Name, arg2Type arg2Name )								\
		{																									\
			returnType ret;																					\
			this->functionName(ret, arg1Name, arg2Name);													\
			return ret;																						\
		}
	#define STEAMWORKS_STRUCT_RETURN_3(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name, arg3Type, arg3Name)	\
		virtual void functionName( returnType& ret, arg1Type arg1Name, arg2Type arg2Name, arg3Type arg3Name ) = 0;				\
		inline returnType functionName( arg1Type arg1Name, arg2Type arg2Name, arg3Type arg3Name )								\
		{																														\
			returnType ret;																										\
			this->functionName(ret, arg1Name, arg2Name, arg3Name);																\
			return ret;																											\
		}
#else
	#define STEAMWORKS_STRUCT_RETURN_0(returnType, functionName) virtual returnType functionName() = 0;
	#define STEAMWORKS_STRUCT_RETURN_1(returnType, functionName, arg1Type, arg1Name) virtual returnType functionName( arg1Type arg1Name ) = 0;
	#define STEAMWORKS_STRUCT_RETURN_2(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name) virtual returnType functionName( arg1Type arg1Name, arg2Type arg2Name ) = 0;
	#define STEAMWORKS_STRUCT_RETURN_3(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name, arg3Type, arg3Name) virtual returnType functionName( arg1Type arg1Name, arg2Type arg2Name, arg3Type arg3Name ) = 0;
#endif

// steamclient/api

#include "EResult.h"
#include "CUtl.h"


// lobby type description
enum ELobbyType
{
	k_ELobbyTypePrivate = 0,		// only way to join the lobby is to invite to someone else
	k_ELobbyTypeFriendsOnly = 1,	// shows for friends or invitees, but not in lobby list
	k_ELobbyTypePublic = 2,			// visible for friends and in lobby list
	k_ELobbyTypeInvisible = 3,		// returned by search, but not visible to other friends 
	//    useful if you want a user in two lobbies, for example matching groups together
	//	  a user can be in only one regular lobby, and up to two invisible lobbies
};

//-----------------------------------------------------------------------------
// Purpose: Possible positions to tell the overlay to show notifications in
//-----------------------------------------------------------------------------
enum ENotificationPosition
{
	k_EPositionTopLeft = 0,
	k_EPositionTopRight = 1,
	k_EPositionBottomLeft = 2,
	k_EPositionBottomRight = 3,
};

//-----------------------------------------------------------------------------
// Purpose: Used in ChatInfo messages - fields specific to a chat member - must fit in a uint32
//-----------------------------------------------------------------------------
enum EChatMemberStateChange
{
	// Specific to joining / leaving the chatroom
	k_EChatMemberStateChangeEntered			= 0x0001,		// This user has joined or is joining the chat room
	k_EChatMemberStateChangeLeft			= 0x0002,		// This user has left or is leaving the chat room
	k_EChatMemberStateChangeDisconnected	= 0x0004,		// User disconnected without leaving the chat first
	k_EChatMemberStateChangeKicked			= 0x0008,		// User kicked
	k_EChatMemberStateChangeBanned			= 0x0010,		// User kicked and banned
};


enum EServerMode
{
	eServerModeInvalid = 0, // DO NOT USE		
	eServerModeNoAuthentication = 1, // Don't authenticate user logins and don't list on the server list
	eServerModeAuthentication = 2, // Authenticate users, list on the server list, don't run VAC on clients that connect
	eServerModeAuthenticationAndSecure = 3, // Authenticate users, list on the server list and VAC protect clients
};

// Steam universes.  Each universe is a self-contained Steam instance.
enum EUniverse
{
	k_EUniverseInvalid = 0,
	k_EUniversePublic = 1,
	k_EUniverseBeta = 2,
	k_EUniverseInternal = 3,
	k_EUniverseDev = 4,
//	k_EUniverseRC = 5, // Removed

	k_EUniverseMax
};



// these is outside NO_STEAM because external things use it
#include "ESteamError.h"
#include "ESteamNotify.h"

#ifndef NO_STEAM
// steam
#include "ESteamSeekMethod.h"
#include "ESteamBufferMethod.h"
#include "ESteamFindFilter.h"
#include "ESteamSubscriptionBillingInfoType.h"
#include "ESteamPaymentCardType.h"
#include "ESteamAppUpdateStatsQueryType.h"
#include "ESteamSubscriptionStatus.h"
#include "ESteamServerType.h"
#endif // NO_STEAM


typedef void* (*CreateInterfaceFn)( const char *pName, int *pReturnCode );
typedef void* (*FactoryFn)( const char *pName );
typedef void* (*InstantiateInterfaceFn)( void );

typedef void  (*SteamAPIWarningMessageHook_t)(int hpipe, const char *message);
typedef void (*KeyValueIteratorCallback_t)(const char* key, const char* value, void* kv);

typedef void (*SteamNotificationCallback_t)(ESteamNotify eEvent, unsigned int nData);

typedef bool (*SteamBGetCallbackFn)( int hpipe, void *pCallbackMsg );
typedef void (*SteamFreeLastCallbackFn)( int hpipe );
typedef bool (*SteamGetAPICallResultFn)( int hpipe, uint64 hSteamAPICall, void* pCallback, int cubCallback, int iCallbackExpected, bool* pbFailed );

//-----------------------------------------------------------------------------
// Purpose: Passed as argument to SteamAPI_UseBreakpadCrashHandler to enable optional callback
//  just before minidump file is captured after a crash has occurred.  (Allows app to append additional comment data to the dump, etc.)
//-----------------------------------------------------------------------------
typedef void (*PFNPreMinidumpCallback)(void *context);

//-----------------------------------------------------------------------------
// Purpose: Used by ICrashHandler interfaces to reference particular installed crash handlers
//-----------------------------------------------------------------------------
typedef void *BREAKPAD_HANDLE;
#define BREAKPAD_INVALID_HANDLE (BREAKPAD_HANDLE)0 

const int k_cubDigestSize = 20;							// CryptoPP::SHA::DIGESTSIZE
const int k_cubSaltSize   = 8;

const int k_cchGameExtraInfoMax = 64;

// Max number of credit cards stored for one account
const int k_nMaxNumCardsPerAccount = 1;

// game server flags
const uint32 k_unServerFlagNone			= 0x00;
const uint32 k_unServerFlagActive		= 0x01;		// server has users playing
const uint32 k_unServerFlagSecure		= 0x02;		// server wants to be secure
const uint32 k_unServerFlagDedicated	= 0x04;		// server is dedicated
const uint32 k_unServerFlagLinux		= 0x08;		// linux build
const uint32 k_unServerFlagPassworded	= 0x10;		// password protected
const uint32 k_unServerFlagPrivate		= 0x20;		// server shouldn't list on master server and
													// won't enforce authentication of users that connect to the server.
													// Useful when you run a server where the clients may not
													// be connected to the internet but you want them to play (i.e LANs)

//-----------------------------------------------------------------------------
// Constants used for query ports.
//-----------------------------------------------------------------------------
#define QUERY_PORT_NOT_INITIALIZED		0xFFFF	// We haven't asked the GS for this query port's actual value yet.
#define QUERY_PORT_ERROR				0xFFFE	// We were unable to get the query port for this server.


typedef	uint8 SHADigest_t[ k_cubDigestSize ];
typedef	uint8 Salt_t[ k_cubSaltSize ];

//-----------------------------------------------------------------------------
// GID (GlobalID) stuff
// This is a globally unique identifier.  It's guaranteed to be unique across all
// racks and servers for as long as a given universe persists.
//-----------------------------------------------------------------------------
// NOTE: for GID parsing/rendering and other utils, see gid.h
typedef uint64 GID_t;

const GID_t k_GIDNil = 0xffffffffffffffffull;

// For convenience, we define a number of types that are just new names for GIDs
typedef GID_t JobID_t;			// Each Job has a unique ID
typedef GID_t TxnID_t;			// Each financial transaction has a unique ID

const GID_t k_TxnIDNil = k_GIDNil;
const GID_t k_TxnIDUnknown = 0;


// this is baked into client messages and interfaces as an int, 
// make sure we never break this.
typedef uint32 PackageId_t;
const PackageId_t k_uPackageIdFreeSub = 0x0;
const PackageId_t k_uPackageIdInvalid = 0xFFFFFFFF;
const PackageId_t k_uPackageIdWallet = -2;
const PackageId_t k_uPackageIdMicroTxn = -3;

// this is baked into client messages and interfaces as an int, 
// make sure we never break this.
typedef uint32 AppId_t;

// A depot id is just an appid, but let's define it as a separate type so the API is easier to understand
typedef uint32 DepotId_t;

const AppId_t k_uAppIdInvalid = 0x0;
const AppId_t k_nGameIDNotepad = 65535;
const AppId_t k_nGameIDCSSTestApp = 65534;
const AppId_t k_nGameIDDRMTestApp_Static = 6710;
const AppId_t k_nGameIDDRMTestApp_Blob = 6711;
const AppId_t k_nGameIDDRMTestApp_Dynamic = 6712;
const AppId_t k_nGameIDDRMTestApp_SDK = 6713;
const AppId_t k_nGameIDWinUI = 7;
const AppId_t k_nGameIDWinUI2 = 8;
const AppId_t k_nGameIDCS = 10;
const AppId_t k_nGameIDTFC = 20;
const AppId_t k_nGameIDDOD = 30;
const AppId_t k_nGameIDDMC = 40;
const AppId_t k_nGameIDOpFor = 50;
const AppId_t k_nGameIDRicochet = 60;
const AppId_t k_nGameIDHL1 = 70;
const AppId_t k_nGameIDCZero = 80;
const AppId_t k_nGameIDCSBeta = 150;
const AppId_t k_nGameIDMacVAC = 160;
const AppId_t k_nGameIDWinVAC = 202;
const AppId_t k_nGameIDScreenshots = 760;
const AppId_t k_nGameDRMTest = 199;
const AppId_t k_nGameIDBaseSourceSDK = 215;
const AppId_t k_nGameIDHL2 = 220;
const AppId_t k_nDepotHL2Buka = 235;
const AppId_t k_nGameIDCSS = 240;
const AppId_t k_nGameIDCSSBeta = 260;
const AppId_t k_nGameHL1SRC = 280;
const AppId_t k_nGameIDDODSRC = 300;
const AppId_t k_nGameIDHL2DM = 320;
const AppId_t k_nGameIDPortal = 400;
const AppId_t k_nGameIDHL2EP2 = 420;
const AppId_t k_nGameIDTF2 = 440;
const AppId_t k_nGameIDL4D = 500;
const AppId_t k_nGameIDL4DDemo = 530;
const AppId_t k_nGameIDL4D2 = 550;
const AppId_t k_nGameIDASW = 630;
const AppId_t k_nGameIDTF2Staging = 810;
const AppId_t k_nGameIDPortal2Main = 852;
const AppId_t k_nGameIDPortal2 = 620;
const AppId_t k_nGameIDASWMain = 877;
const AppId_t k_nGameIDDOTA = 882;
const AppId_t k_nGameIDASWStaging = 886;
const AppId_t k_nGameIDRedOrchestra = 1200;
const AppId_t k_nGameIDRedOrchestraBeta = 1210;
const AppId_t k_nGameIDKillingFloor = 1250;
const AppId_t k_nGameIDSin1 = 1309;
const AppId_t k_nGameIDEarth2160 = 1900;
const AppId_t k_nGameIDTheShip = 2400;
const AppId_t k_nGameIDTheShipBeta = 2410;
const AppId_t k_nGameIDDarkMessiahSP = 2100;
const AppId_t k_nGameIDDarkMessiahMPBeta = 2110;
const AppId_t k_nGameIDDarkMessiahMP = 2115;
const AppId_t k_nGameIDDarkMessiahSPDemo = 2120;
const AppId_t k_nGameIDDarkMessiahFix = 2130;
const AppId_t k_nGameRaceWTCC = 4230;
const AppId_t k_nGameIDLostPlanetOld = 6500;
const AppId_t k_nGameIDLostPlanet = 6510;
const AppId_t k_nGameIDNBA2K9 = 7740;
const AppId_t k_nGameIDCallofDuty4 = 7940;
const AppId_t k_nMLBFrontOfficeManager = 7780;
const AppId_t k_nGameIDMW2SP = 10180;
const AppId_t k_nGameIDMW2MP = 10190;
const AppId_t k_nGameIDIW5SP = 42680;
const AppId_t k_nGameIDIW5MP = 42690;
const AppId_t k_nGameIDCODBLOPSSP = 42700;
const AppId_t k_nGameIDCODBLOPSMP = 42710;
const AppId_t k_nGameIDEmpireTotalWar = 10500;
const AppId_t k_nGameCSSOnline = 11600;
const AppId_t k_nGameIDFirstSource = 200;
const AppId_t k_nGameIDLastSource = 999;
const AppId_t k_nGameIDFirstGoldSource = 10;
const AppId_t k_nGameIDLastGoldSource = 199;
const AppId_t k_nGameIDFirstNonSource = 1000;
const AppId_t k_nGameIDMax = 2147483647;
const AppId_t k_nGameIDStress = 30020;
const AppId_t k_nGameIDGCTest = 30100;
const AppId_t k_nAppATIDriver_Vista7_32 = 61800;
const AppId_t k_nAppATIDriver_Vista7_64 = 61810;
const AppId_t k_nAppATIDriver_XP_32 = 61820;
const AppId_t k_nAppATIDriver_XP_64 = 61830;

typedef enum ShareType_t
{
	SHARE_STOPIMMEDIATELY = 0,
	SHARE_RATIO = 1,
	SHARE_MANUAL = 2,
} ShareType_t;

typedef uint64 AssetClassId_t;
const AssetClassId_t k_ulAssetClassIdInvalid = 0x0;

typedef uint32 PhysicalItemId_t;
const PhysicalItemId_t k_uPhysicalItemIdInvalid = 0x0;

// this is baked into client messages and interfaces as an int, 
// make sure we never break this.  AppIds and DepotIDs also presently
// share the same namespace, but since we'd like to change that in the future
// I've defined it seperately here.
typedef uint32 DepotId_t;
const DepotId_t k_uDepotIdInvalid = 0x0;

typedef int HVoiceCall;


// RTime32
// We use this 32 bit time representing real world time.
// It offers 1 second resolution beginning on January 1, 1970 (Unix time)
typedef uint32 RTime32;
const RTime32 k_RTime32Nil = 0;
const RTime32 k_RTime32MinValid = 10;
const RTime32 k_RTime32Infinite = 2147483647;

typedef uint32 CellID_t;
const CellID_t k_uCellIDInvalid = 0xFFFFFFFF;

// handle to a Steam API call
typedef uint64 SteamAPICall_t;
const SteamAPICall_t k_uAPICallInvalid = 0x0;

typedef uint32 AccountID_t;

// handle to a communication pipe to the Steam client
typedef int32 HSteamPipe;
// handle to single instance of a steam user
typedef int32 HSteamUser;
// reference to a steam call, to filter results by
typedef int32 HSteamCall;

//-----------------------------------------------------------------------------
// Typedef for handle type you will receive when requesting server list.
//-----------------------------------------------------------------------------
typedef void* HServerListRequest;

// return type of GetAuthSessionTicket
typedef uint32 HAuthTicket;
const HAuthTicket k_HAuthTicketInvalid = 0;

typedef int HNewItemRequest;
typedef uint64 ItemID;

typedef uint32 HTTPRequestHandle;

typedef int unknown_ret; // unknown return value

// returns true of the flags indicate that a user has been removed from the chat
#define BChatMemberStateChangeRemoved( rgfChatMemberStateChangeFlags ) ( rgfChatMemberStateChangeFlags & ( k_EChatMemberStateChangeDisconnected | k_EChatMemberStateChangeLeft | k_EChatMemberStateChangeKicked | k_EChatMemberStateChangeBanned ) )

typedef void (*PFNLegacyKeyRegistration)( const char *pchCDKey, const char *pchInstallPath );
typedef bool (*PFNLegacyKeyInstalled)();

const unsigned int k_unSteamAccountIDMask = 0xFFFFFFFF;
const unsigned int k_unSteamAccountInstanceMask = 0x000FFFFF;
const unsigned int k_unSteamUserDefaultInstance	= 1; // fixed instance for all individual users

// we allow 3 simultaneous user account instances right now, 1= desktop, 2 = console, 4 = web, 0 = all
const unsigned int k_unSteamUserDesktopInstance = 1;	 
const unsigned int k_unSteamUserConsoleInstance = 2;
const unsigned int k_unSteamUserWebInstance		= 4;

// Special flags for Chat accounts - they go in the top 8 bits
// of the steam ID's "instance", leaving 12 for the actual instances
enum EChatSteamIDInstanceFlags
{
	k_EChatAccountInstanceMask = 0x00000FFF, // top 8 bits are flags

	k_EChatInstanceFlagClan = ( k_unSteamAccountInstanceMask + 1 ) >> 1,	// top bit
	k_EChatInstanceFlagLobby = ( k_unSteamAccountInstanceMask + 1 ) >> 2,	// next one down, etc
	k_EChatInstanceFlagMMSLobby = ( k_unSteamAccountInstanceMask + 1 ) >> 3,	// next one down, etc

	// Max of 8 flags
};

#define STEAM_USING_FILESYSTEM							(0x00000001)
#define STEAM_USING_LOGGING								(0x00000002)
#define STEAM_USING_USERID								(0x00000004)
#define STEAM_USING_ACCOUNT								(0x00000008)
#define STEAM_USING_ALL									(0x0000000f)
#define STEAM_MAX_PATH									(255)
#define STEAM_QUESTION_MAXLEN							(255)
#define STEAM_SALT_SIZE									(8)

#define STEAM_DATE_SIZE									(9)
#define STEAM_TIME_SIZE									(9)
#define STEAM_CARD_NUMBER_SIZE							(17)
#define STEAM_CONFIRMATION_CODE_SIZE					(22)
#define STEAM_CARD_HOLDERNAME_SIZE						(100)
#define STEAM_CARD_APPROVAL_CODE_SIZE					(100)
#define STEAM_CARD_EXPYEAR_SIZE							(4)
#define STEAM_CARD_LASTFOURDIGITS_SIZE					(4)
#define STEAM_CARD_EXPMONTH_SIZE						(2)
#define STEAM_CARD_CVV2_SIZE							(5)
#define STEAM_BILLING_ADDRESS1_SIZE						(128)
#define STEAM_BILLING_ADDRESS2_SIZE						(128)
#define STEAM_BILLING_CITY_SIZE							(50)
#define STEAM_BILLING_ZIP_SIZE							(16)
#define STEAM_BILLING_STATE_SIZE						(32)
#define STEAM_BILLING_COUNTRY_SIZE						(32)
#define STEAM_BILLING_PHONE_SIZE						(20)
#define STEAM_BILLING_EMAIL_SIZE						(100)
#define STEAM_TYPE_OF_PROOF_OF_PURCHASE_SIZE			(20)
#define STEAM_PROOF_OF_PURCHASE_TOKEN_SIZE				(200)
#define STEAM_EXTERNAL_ACCOUNTNAME_SIZE					(100)
#define STEAM_EXTERNAL_ACCOUNTPASSWORD_SIZE				(80)

#define IS_STEAM_ERROR(e) (e.eSteamError != eSteamErrorNone)

typedef unsigned int SteamHandle_t;

typedef void * SteamUserIDTicketValidationHandle_t;

typedef unsigned int SteamCallHandle_t;

#if defined(_MSC_VER)
	typedef __int64				SteamSigned64_t;
	typedef unsigned __int64	SteamUnsigned64_t;
#else
	typedef long long			SteamSigned64_t;
	typedef unsigned long long	SteamUnsigned64_t;
#endif


#ifdef __cplusplus

const SteamHandle_t										STEAM_INVALID_HANDLE = 0;
const SteamCallHandle_t									STEAM_INVALID_CALL_HANDLE = 0;
const SteamUserIDTicketValidationHandle_t				STEAM_INACTIVE_USERIDTICKET_VALIDATION_HANDLE = 0;
const unsigned int										STEAM_USE_LATEST_VERSION = 0xFFFFFFFF;

#else

#define STEAM_INVALID_HANDLE							((SteamHandle_t)(0))
#define STEAM_INVALID_CALL_HANDLE						((SteamCallHandle_t)(0))
#define	STEAM_INACTIVE_USERIDTICKET_VALIDATION_HANDLE	((SteamUserIDTicketValidationHandle_t)(0))
#define STEAM_USE_LATEST_VERSION						(0xFFFFFFFFu);

#endif


// Each Steam instance (licensed Steam Service Provider) has a unique SteamInstanceID_t.
//
// Each Steam instance as its own DB of users.
// Each user in the DB has a unique SteamLocalUserID_t (a serial number, with possible 
// rare gaps in the sequence).

typedef	unsigned short		SteamInstanceID_t;		// MUST be 16 bits

#if defined (_MSC_VER)
	typedef	unsigned __int64	SteamLocalUserID_t;	// MUST be 64 bits
#else
	typedef	unsigned long long	SteamLocalUserID_t;	// MUST be 64 bits
#endif


typedef char SteamPersonalQuestion_t[ STEAM_QUESTION_MAXLEN + 1 ];

//-----------------------------------------------------------------------------
// Purpose: Base values for callback identifiers, each callback must
//			have a unique ID.
//-----------------------------------------------------------------------------
enum ECallbackType
{
	k_iSteamUserCallbacks = 100, // possibly 1020000 now
	k_iSteamGameServerCallbacks = 200,
	k_iSteamFriendsCallbacks = 300,
	k_iSteamBillingCallbacks = 400,
	k_iSteamMatchmakingCallbacks = 500,
	k_iSteamContentServerCallbacks = 600,
	k_iSteamUtilsCallbacks = 700, // possibly 1040000 now
	k_iClientFriendsCallbacks = 800, // possibly 1010000 now
	k_iClientUserCallbacks = 900,
	k_iSteamAppsCallbacks = 1000,
	k_iSteamUserStatsCallbacks = 1100,
	k_iSteamNetworkingCallbacks = 1200,
	k_iClientRemoteStorageCallbacks = 1300, // possibly 1260000 now
	k_iSteamUserItemsCallbacks = 1400,
	k_iSteamGameServerItemsCallbacks = 1500,
	k_iClientUtilsCallbacks = 1600,
	k_iSteamGameCoordinatorCallbacks = 1700,
	k_iSteamGameServerStatsCallbacks = 1800,
	k_iSteam2AsyncCallbacks = 1900,
	k_iSteamGameStatsCallbacks = 2000,
	k_iClientHTTPCallbacks = 2100,
	k_iClientScreenshotsCallbacks = 2200,
	k_iSteamScreenshotsCallbacks = 2300,
	k_iClientAudioCallbacks = 2400,
	k_iSteamUnifiedMessagesCallbacks = 2500,
	k_iClientUnifiedMessagesCallbacks = 2600,
	k_iClientControllerCallbacks = 2700,
	k_iSteamControllerCallbacks = 2800,
	k_iClientParentalSettingsCallbacks = 2900,
	k_iClientDeviceAuthCallbacks = 3000,
	k_iClientNetworkDeviceManagerCallbacks = 3100,
	k_iClientMusicCallbacks = 3200, // possibly 1100000 now
	k_iClientRemoteClientManagerCallbacks = 3300,
	k_iClientUGCCallbacks = 3400,
	k_iSteamStreamClientCallbacks = 3500,
	k_IClientProductBuilderCallbacks = 3600,
	k_iClientShortcutsCallbacks = 3700,
	k_iClientRemoteControlManagerCallbacks = 3800,
	k_iSteamAppListCallbacks = 3900,
	k_iSteamMusicCallbacks = 4000,
	k_iSteamMusicRemoteCallbacks = 4100,
	k_iClientVRCallbacks = 4200,
	k_iClientReservedCallbacks = 4300,
	k_iSteamReservedCallbacks = 4400,
	k_iSteamHTMLSurfaceCallbacks = 4500,
	k_iClientVideoCallbacks = 4600,
	k_iClientInventoryCallbacks = 4700,
	k_iClientSharedConnectionCallbacks = 4900,
	k_iClientAppsCallbacks = 1280000,
};


#ifndef NO_STEAM
// steam structs, etc
#include "TSteamElemInfo.h"
#include "TSteamError.h"
#include "TSteamProgress.h"
#include "TSteamAppStats.h"
#include "TSteamUpdateStats.h"
#include "TSteamPaymentCardInfo.h"
#include "TSteamPrepurchaseInfo.h"
#include "TSteamExternalBillingInfo.h"
#include "TSteamSubscriptionBillingInfo.h"
#include "TSteamSubscriptionStats.h"
#include "TSteamSubscription.h"
#include "TSteamApp.h"
#include "TSteamAppLaunchOption.h"
#include "TSteamAppVersion.h"
#include "TSteamSplitLocalUserID.h"
#include "TSteamGlobalUserID.h"
#include "TSteamAppDependencyInfo.h"
#include "TSteamOfflineStatus.h"
#include "TSteamPaymentCardReceiptInfo.h"
#include "TSteamPrepurchaseReceiptInfo.h"
#include "TSteamSubscriptionReceipt.h"
#include "TSteamSubscriptionDiscount.h"
#include "TSteamDiscountQualifier.h"
#include "SteamSalt.h"
#endif // NO_STEAM


// steamclient/api
#include "CSteamID.h"

#include "MatchMakingKeyValuePair.h"
#include "servernetadr.h"
#include "gameserveritem.h"
#include "FriendGameInfo.h"
#include "EVoiceResult.h"
#include "ECurrencyCode.h"

// structure that contains client callback data
struct CallbackMsg_t
{
	HSteamUser m_hSteamUser;
	int m_iCallback;
	uint8 *m_pubParam;
	int m_cubParam;
};



#endif // STEAMTYPES_H

