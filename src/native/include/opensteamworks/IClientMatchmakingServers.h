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

#ifndef ICLIENTMATCHMAKINGSERVERS_H
#define ICLIENTMATCHMAKINGSERVERS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "MatchmakingServersCommon.h"


abstract_class IClientMatchmakingServers
{
public:
	virtual HServerListRequest  RequestInternetServerList( AppId_t iApp, MatchMakingKeyValuePair_t **ppchFilters, uint32 nFilters, ISteamMatchmakingServerListResponse *pRequestServersResponse ) = 0;
	virtual HServerListRequest  RequestLANServerList( AppId_t iApp, ISteamMatchmakingServerListResponse *pRequestServersResponse ) = 0;
	virtual HServerListRequest  RequestFriendsServerList( AppId_t iApp, MatchMakingKeyValuePair_t **ppchFilters, uint32 nFilters, ISteamMatchmakingServerListResponse *pRequestServersResponse ) = 0;
	virtual HServerListRequest  RequestFavoritesServerList( AppId_t iApp, MatchMakingKeyValuePair_t **ppchFilters, uint32 nFilters, ISteamMatchmakingServerListResponse *pRequestServersResponse ) = 0;
	virtual HServerListRequest  RequestHistoryServerList( AppId_t iApp, MatchMakingKeyValuePair_t **ppchFilters, uint32 nFilters, ISteamMatchmakingServerListResponse *pRequestServersResponse ) = 0;
	virtual HServerListRequest  RequestSpectatorServerList( AppId_t iApp, MatchMakingKeyValuePair_t **ppchFilters, uint32 nFilters, ISteamMatchmakingServerListResponse *pRequestServersResponse ) = 0;
	virtual void ReleaseRequest( HServerListRequest hServerListRequest ) = 0;
	virtual gameserveritem_t *GetServerDetails( HServerListRequest hServerListRequest, int iServer ) = 0;
	virtual void CancelQuery( HServerListRequest hServerListRequest ) = 0;
	virtual void RefreshQuery( HServerListRequest hServerListRequest ) = 0;
	virtual bool IsRefreshing( HServerListRequest hServerListRequest ) = 0;
	virtual int GetServerCount( HServerListRequest hServerListRequest ) = 0;
	virtual void RefreshServer( HServerListRequest hServerListRequest, int iServer ) = 0;
	virtual HServerQuery PingServer( uint32 unIP, uint16 usPort, ISteamMatchmakingPingResponse *pRequestServersResponse ) = 0;
	virtual HServerQuery PlayerDetails( uint32 unIP, uint16 usPort, ISteamMatchmakingPlayersResponse *pRequestServersResponse ) = 0;
	virtual HServerQuery ServerRules( uint32 unIP, uint16 usPort, ISteamMatchmakingRulesResponse *pRequestServersResponse ) = 0;
	virtual void CancelServerQuery( HServerQuery hServerQuery ) = 0;

	virtual void _RequestXxxServerList_v001( EMatchMakingType eType, AppId_t iApp, MatchMakingKeyValuePair_t **ppchFilters, uint32 nFilters, ISteamMatchmakingServerListResponse001 *pRequestServersResponse ) = 0;
	virtual gameserveritem_t *_GetServerDetails_v001( EMatchMakingType eType, int iServer ) = 0;
	virtual void _CancelQuery_v001( EMatchMakingType eType ) = 0;
	virtual void _RefreshQuery_v001 (EMatchMakingType eType ) = 0;
	virtual bool _IsRefreshing_v001( EMatchMakingType eType ) = 0;
	virtual int _GetServerCount_v001( EMatchMakingType eType ) = 0;
	virtual void _RefreshServer_v001( EMatchMakingType eType, int iServer ) = 0;
};


#endif // ICLIENTMATCHMAKINGSERVERS_H
