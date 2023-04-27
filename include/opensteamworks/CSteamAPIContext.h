#ifndef CSTEAMAPICONTEXT_H
#define CSTEAMAPICONTEXT_H
#ifdef _WIN32
#pragma once
#endif 

#include "SteamTypes.h"
#include "Steamclient.h"

//----------------------------------------------------------------------------------------------------------------------------------------------------------//
// VERSION_SAFE_STEAM_API_INTERFACES uses CSteamAPIContext to provide interfaces to each module in a way that 
// lets them each specify the interface versions they are compiled with.
//
// It's important that these stay inlined in the header so the calling module specifies the interface versions
// for whatever Steam API version it has.
//----------------------------------------------------------------------------------------------------------------------------------------------------------//

class CSteamAPIContext
{
public:
	CSteamAPIContext();
	void Clear();

	bool Init();

	ISteamUser013*         SteamUser()                         { return m_pSteamUser; }
	ISteamFriends005*      SteamFriends()                      { return m_pSteamFriends; }
	ISteamUtils005*        SteamUtils()                        { return m_pSteamUtils; }
	ISteamMatchmaking008*  SteamMatchmaking()                  { return m_pSteamMatchmaking; }
	ISteamUserStats007*    SteamUserStats()                    { return m_pSteamUserStats; }
	ISteamApps003*         SteamApps()                         { return m_pSteamApps; }
	ISteamMatchmakingServers002*   SteamMatchmakingServers()   { return m_pSteamMatchmakingServers; }
	ISteamNetworking003*   SteamNetworking()                   { return m_pSteamNetworking; }
	ISteamRemoteStorage002* SteamRemoteStorage()               { return m_pSteamRemoteStorage; }

private:
	ISteamUser013      *m_pSteamUser;
	ISteamFriends005   *m_pSteamFriends;
	ISteamUtils005     *m_pSteamUtils;
	ISteamMatchmaking008   *m_pSteamMatchmaking;
	ISteamUserStats007     *m_pSteamUserStats;
	ISteamApps003          *m_pSteamApps;
	ISteamMatchmakingServers002    *m_pSteamMatchmakingServers;
	ISteamNetworking003    *m_pSteamNetworking;
	ISteamRemoteStorage002 *m_pSteamRemoteStorage;
};

inline CSteamAPIContext::CSteamAPIContext()
{
	Clear();
}

inline void CSteamAPIContext::Clear()
{
	m_pSteamUser = NULL;
	m_pSteamFriends = NULL;
	m_pSteamUtils = NULL;
	m_pSteamMatchmaking = NULL;
	m_pSteamUserStats = NULL;
	m_pSteamApps = NULL;
	m_pSteamMatchmakingServers = NULL;
	m_pSteamNetworking = NULL;
	m_pSteamRemoteStorage = NULL;
}

// This function must be inlined so the module using steam_api.dll gets the version names they want.
inline bool CSteamAPIContext::Init()
{
	if ( !SteamClient() )
		return false;

	HSteamUser hSteamUser = SteamAPI_GetHSteamUser();
	HSteamPipe hSteamPipe = SteamAPI_GetHSteamPipe();

	m_pSteamUser = (ISteamUser013 *)SteamClient()->GetISteamUser( hSteamUser, hSteamPipe, STEAMUSER_INTERFACE_VERSION_013 );
	if ( !m_pSteamUser )
		return false;

	m_pSteamFriends = (ISteamFriends005 *)SteamClient()->GetISteamFriends( hSteamUser, hSteamPipe, STEAMFRIENDS_INTERFACE_VERSION_005 );
	if ( !m_pSteamFriends )
		return false;

	m_pSteamUtils = (ISteamUtils005 *)SteamClient()->GetISteamUtils( hSteamPipe, STEAMUTILS_INTERFACE_VERSION_005 );
	if ( !m_pSteamUtils )
		return false;

	m_pSteamMatchmaking = (ISteamMatchmaking008 *)SteamClient()->GetISteamMatchmaking( hSteamUser, hSteamPipe, STEAMMATCHMAKING_INTERFACE_VERSION_008 );
	if ( !m_pSteamMatchmaking )
		return false;

	m_pSteamMatchmakingServers = (ISteamMatchmakingServers002 *)SteamClient()->GetISteamMatchmakingServers( hSteamUser, hSteamPipe, STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION_002 );
	if ( !m_pSteamMatchmakingServers )
		return false;

	m_pSteamUserStats = (ISteamUserStats007 *)SteamClient()->GetISteamUserStats( hSteamUser, hSteamPipe, STEAMUSERSTATS_INTERFACE_VERSION_007 );
	if ( !m_pSteamUserStats )
		return false;

	m_pSteamApps = (ISteamApps003 *)SteamClient()->GetISteamApps( hSteamUser, hSteamPipe, STEAMAPPS_INTERFACE_VERSION_003 );
	if ( !m_pSteamApps )
		return false;

	m_pSteamNetworking = (ISteamNetworking003 *)SteamClient()->GetISteamNetworking( hSteamUser, hSteamPipe, STEAMNETWORKING_INTERFACE_VERSION_003 );
	if ( !m_pSteamNetworking )
		return false;

	m_pSteamRemoteStorage = (ISteamRemoteStorage002 *)SteamClient()->GetISteamRemoteStorage( hSteamUser, hSteamPipe, STEAMREMOTESTORAGE_INTERFACE_VERSION_002 );
	if ( !m_pSteamRemoteStorage )
		return false;

	return true;
}

class CSteamGameServerAPIContext
{
public:
	CSteamGameServerAPIContext();
	void Clear();

	bool Init();

	ISteamGameServer010 *SteamGameServer() { return m_pSteamGameServer; }
	ISteamUtils005 *SteamGameServerUtils() { return m_pSteamGameServerUtils; }
	ISteamMasterServerUpdater001 *SteamMasterServerUpdater() { return m_pSteamMasterServerUpdater; }
	ISteamNetworking003 *SteamGameServerNetworking() { return m_pSteamGameServerNetworking; }
	ISteamGameServerStats001 *SteamGameServerStats() { return m_pSteamGameServerStats; }

private:
	ISteamGameServer010            *m_pSteamGameServer;
	ISteamUtils005                 *m_pSteamGameServerUtils;
	ISteamMasterServerUpdater001   *m_pSteamMasterServerUpdater;
	ISteamNetworking003            *m_pSteamGameServerNetworking;
	ISteamGameServerStats001       *m_pSteamGameServerStats;
};

inline CSteamGameServerAPIContext::CSteamGameServerAPIContext()
{
	Clear();
}

inline void CSteamGameServerAPIContext::Clear()
{
	m_pSteamGameServer = NULL;
	m_pSteamGameServerUtils = NULL;
	m_pSteamMasterServerUpdater = NULL;
	m_pSteamGameServerNetworking = NULL;
	m_pSteamGameServerStats = NULL;
}

S_API ISteamClient009 *g_pSteamClientGameServer;
// This function must be inlined so the module using steam_api.dll gets the version names they want.
inline bool CSteamGameServerAPIContext::Init()
{
	if ( !g_pSteamClientGameServer )
		return false;

	HSteamUser hSteamUser = SteamGameServer_GetHSteamUser();
	HSteamPipe hSteamPipe = SteamGameServer_GetHSteamPipe();

	m_pSteamGameServer = (ISteamGameServer010 *)g_pSteamClientGameServer->GetISteamGameServer( hSteamUser, hSteamPipe, STEAMGAMESERVER_INTERFACE_VERSION_010 );
	if ( !m_pSteamGameServer )
		return false;

	m_pSteamGameServerUtils = (ISteamUtils005 *)g_pSteamClientGameServer->GetISteamUtils( hSteamPipe, STEAMUTILS_INTERFACE_VERSION_005 );
	if ( !m_pSteamGameServerUtils )
		return false;

	m_pSteamMasterServerUpdater = (ISteamMasterServerUpdater001 *)g_pSteamClientGameServer->GetISteamMasterServerUpdater( hSteamUser, hSteamPipe, STEAMMASTERSERVERUPDATER_INTERFACE_VERSION_001 );
	if ( !m_pSteamMasterServerUpdater )
		return false;

	m_pSteamGameServerNetworking = (ISteamNetworking003 *)g_pSteamClientGameServer->GetISteamNetworking( hSteamUser, hSteamPipe, STEAMNETWORKING_INTERFACE_VERSION_003 );
	if ( !m_pSteamGameServerNetworking )
		return false;

	m_pSteamGameServerStats = (ISteamGameServerStats001 *)g_pSteamClientGameServer->GetISteamGameServerStats( hSteamUser, hSteamPipe, STEAMGAMESERVERSTATS_INTERFACE_VERSION_001 );
	if ( !m_pSteamGameServerStats )
		return false;

	return true;
}

#endif // CSTEAMAPICONTEXT_H
