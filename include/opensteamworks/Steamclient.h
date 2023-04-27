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


#ifndef STEAMCLIENT_H
#define STEAMCLIENT_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

#if defined(__GNUC__) && defined(_WIN32)
	// This ugly hack allows us to provide GCC compatibility on windows without much effort
	#pragma push_macro("virtual")
	#undef virtual
	#define virtual virtual __thiscall
#endif

#ifndef NO_ICLIENT
// client interfaces
#include "IClientEngine.h"

#include "IClientAppManager.h"
#include "IClientApps.h"
#include "IClientAudio.h"
#include "IClientBilling.h"
#include "IClientContentServer.h"
#include "IClientCompat.h"
#include "IClientDepotBuilder.h"
#include "IClientFriends.h"
#include "IClientGameCoordinator.h"
#include "IClientGameServer.h"
#include "IClientGameServerStats.h"
#include "IClientGameStats.h"
#include "IClientInventory.h"
#include "IClientMasterServerUpdater.h"
#include "IClientMatchmaking.h"
#include "IClientMatchmakingServers.h"
#include "IClientNetworking.h"
#include "IClientRemoteStorage.h"
#include "IClientScreenshots.h"
#include "IClientSharedConnection.h"
#include "IClientUser.h"
#include "IClientUserStats.h"
#include "IClientUtils.h"
#include "IClientHTTP.h"
#include "IClientConfigStore.h"
#include "IClientUnifiedMessages.h"
#include "IClientShader.h"
#include "IClientStreamLauncher.h"
#include "IClientNetworkDeviceManager.h"
#include "IClientDeviceAuth.h"
#include "IClientMusic.h"
#include "IClientProductBuilder.h"
#include "IClientShortcuts.h"
#include "IClientStreamClient.h"
#include "IClientUGC.h"
#endif // NO_ICLIENT

// callback
#include "CCallback.h"

// steam_api
#ifdef VERSION_SAFE_STEAM_API_INTERFACES
	#include "CSteamAPIContext.h"
#endif // VERSION_SAFE_STEAM_API_INTERFACES

#if defined(__GNUC__) && defined(_WIN32)
	#pragma pop_macro("virtual")
#endif

// Breakpad
S_API errno_t STEAM_CALL Breakpad_SetSteamID( uint64 ulSteamID );
S_API errno_t STEAM_CALL Breakpad_SteamSetSteamID( uint64 ulSteamID );
S_API void STEAM_CALL Breakpad_SteamMiniDumpInit( uint32 a, const char *b, const char *c );
S_API errno_t STEAM_CALL Breakpad_SteamWriteMiniDumpSetComment( const char *pchMsg );
S_API void STEAM_CALL Breakpad_SteamWriteMiniDumpUsingExceptionInfoWithBuildId( int a, int b );


// Steam user
S_API bool STEAM_CALL Steam_BConnected( HSteamUser hUser, HSteamPipe hSteamPipe );
S_API bool STEAM_CALL Steam_BLoggedOn( HSteamUser hUser, HSteamPipe hSteamPipe );
S_API void STEAM_CALL Steam_LogOn( HSteamUser hUser, HSteamPipe hSteamPipe, uint64 ulSteamID );
S_API void STEAM_CALL Steam_LogOff( HSteamUser hUser, HSteamPipe hSteamPipe );
S_API int STEAM_CALL Steam_InitiateGameConnection( HSteamUser hUser, HSteamPipe hSteamPipe, void *pBlob, int cbMaxBlob, uint64 steamID, int nGameAppID, uint32 unIPServer, uint16 usPortServer, bool bSecure );
S_API void STEAM_CALL Steam_TerminateGameConnection( HSteamUser hUser, HSteamPipe hSteamPipe, uint32 unIPServer, uint16 usPortServer );

// Steam callbacks
S_API bool STEAM_CALL Steam_BGetCallback( HSteamPipe hSteamPipe, CallbackMsg_t *pCallbackMsg );
S_API void STEAM_CALL Steam_FreeLastCallback( HSteamPipe hSteamPipe );
S_API bool STEAM_CALL Steam_GetAPICallResult( HSteamPipe hSteamPipe, SteamAPICall_t hSteamAPICall, void* pCallback, int cubCallback, int iCallbackExpected, bool* pbFailed );

// Steam client
S_API HSteamPipe STEAM_CALL Steam_CreateSteamPipe();
S_API bool STEAM_CALL Steam_BReleaseSteamPipe( HSteamPipe hSteamPipe );
S_API HSteamUser STEAM_CALL Steam_CreateLocalUser( HSteamPipe *phSteamPipe, EAccountType eAccountType );
S_API HSteamUser STEAM_CALL Steam_CreateGlobalUser( HSteamPipe *phSteamPipe );
S_API HSteamUser STEAM_CALL Steam_ConnectToGlobalUser( HSteamPipe hSteamPipe );
S_API void STEAM_CALL Steam_ReleaseUser( HSteamPipe hSteamPipe, HSteamUser hUser );
S_API void STEAM_CALL Steam_SetLocalIPBinding( uint32 unIP, uint16 usLocalPort );

// Steam game server
S_API int STEAM_CALL Steam_GSGetSteamGameConnectToken( HSteamUser hUser, HSteamPipe hSteamPipe, void *pBlob, int cbBlobMax ); // does this exist anymore?
S_API void* STEAM_CALL Steam_GetGSHandle( HSteamUser hUser, HSteamPipe hSteamPipe );
S_API bool STEAM_CALL Steam_GSSendSteam2UserConnect( void *phSteamHandle, uint32 unUserID, const void *pvRawKey, uint32 unKeyLen, uint32 unIPPublic, uint16 usPort, const void *pvCookie, uint32 cubCookie );
S_API bool STEAM_CALL Steam_GSSendUserDisconnect( void *phSteamHandle, uint64 ulSteamID, uint32 unUserID );
S_API OBSOLETE_FUNCTION bool STEAM_CALL Steam_GSSendUserStatusResponse( void *phSteamHandle, uint64 ulSteamID, int nSecondsConnected, int nSecondsSinceLast );
S_API bool STEAM_CALL Steam_GSUpdateStatus( void *phSteamHandle, int cPlayers, int cPlayersMax, int cBotPlayers, const char *pchServerName, const char *pchMapName );
S_API bool STEAM_CALL Steam_GSRemoveUserConnect( void *phSteamHandle, uint32 unUserID );
S_API void STEAM_CALL Steam_GSSetSpawnCount( void *phSteamHandle, uint32 ucSpawn );
S_API bool STEAM_CALL Steam_GSGetSteam2GetEncryptionKeyToSendToNewClient( void *phSteamHandle, void *pvEncryptionKey, uint32 *pcbEncryptionKey, uint32 cbMaxEncryptionKey );
S_API void STEAM_CALL Steam_GSLogOn( void *phSteamHandle );
S_API void STEAM_CALL Steam_GSLogOff( void *phSteamHandle );
S_API bool STEAM_CALL Steam_GSBLoggedOn( void *phSteamHandle );
S_API bool STEAM_CALL Steam_GSSetServerType( void *phSteamHandle, int32 nAppIdServed, uint32 unServerFlags, uint32 unGameIP, uint32 unGamePort, const char *pchGameDir, const char *pchVersion );
S_API bool STEAM_CALL Steam_GSBSecure( void *phSteamHandle);


//----------------------------------------------------------------------------------------------------------------------------------------------------------//
//	steamclient.dll private wrapper functions
//
//	The following functions are part of abstracting API access to the steamclient.dll, but should only be used in very specific cases
//----------------------------------------------------------------------------------------------------------------------------------------------------------//
S_API void STEAM_CALL Steam_RunCallbacks( HSteamPipe hSteamPipe, bool bGameServerCallbacks );
S_API void STEAM_CALL Steam_RegisterInterfaceFuncs( void *hModule );

S_API HSteamUser STEAM_CALL Steam_GetHSteamUserCurrent();

S_API const char* STEAM_CALL SteamAPI_GetSteamInstallPath();

// used in version safe api
S_API HSteamPipe STEAM_CALL GetHSteamPipe();
S_API HSteamUser STEAM_CALL GetHSteamUser();

#endif // STEAMCLIENT_H
