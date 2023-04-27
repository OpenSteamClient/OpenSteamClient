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

#ifndef ICLIENTMATCHMAKING_H
#define ICLIENTMATCHMAKING_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "MatchmakingCommon.h"
#include "UserCommon.h"


abstract_class UNSAFE_INTERFACE IClientMatchmaking
{
public:
	virtual int32 GetFavoriteGameCount() = 0;
	virtual bool GetFavoriteGame( int32 iGame, AppId_t *pnAppID, uint32 *pnIP, uint16 *pnConnPort, uint16 *pnQueryPort, uint32 *punFlags, uint32 *pRTime32LastPlayedOnServer ) = 0;
	virtual int32 AddFavoriteGame( AppId_t nAppID, uint32 nIP, uint16 nConnPort, uint16 nQueryPort, uint32 unFlags, uint32 rTime32LastPlayedOnServer ) =0;
	virtual bool RemoveFavoriteGame( AppId_t nAppID, uint32 nIP, uint16 nConnPort, uint16 nQueryPort, uint32 unFlags ) = 0;

	virtual SteamAPICall_t RequestLobbyList() = 0;

	virtual void AddRequestLobbyListStringFilter( const char *pchKeyToMatch, const char *pchValueToMatch, ELobbyComparison eComparisonType ) = 0;
	virtual void AddRequestLobbyListNumericalFilter( const char *pchKeyToMatch, int32 nValueToMatch, ELobbyComparison eComparisonType ) = 0;
	virtual void AddRequestLobbyListNearValueFilter( const char *pchKeyToMatch, int32 nValueToBeCloseTo ) = 0;
	virtual void AddRequestLobbyListFilterSlotsAvailable( int32 nSlotsAvailable ) = 0;
	virtual void AddRequestLobbyListDistanceFilter( ELobbyDistanceFilter filter ) = 0;
	virtual void AddRequestLobbyListResultCountFilter( int32 cMaxResults ) = 0;
	virtual void AddRequestLobbyListCompatibleMembersFilter( CSteamID steamID ) = 0;

	STEAMWORKS_STRUCT_RETURN_1(CSteamID, GetLobbyByIndex, int32, iLobby) /*virtual CSteamID GetLobbyByIndex( int32 iLobby ) = 0;*/

	virtual SteamAPICall_t CreateLobby( ELobbyType eLobbyType, int32 cMaxMembers ) = 0;
	virtual SteamAPICall_t JoinLobby( CSteamID steamIDLobby ) = 0;
	virtual void LeaveLobby( CSteamID steamIDLobby ) = 0;
	virtual bool InviteUserToLobby( CSteamID steamIDLobby, CSteamID steamIDInvitee ) = 0;

	virtual int32 GetNumLobbyMembers( CSteamID steamIDLobby ) = 0;
	STEAMWORKS_STRUCT_RETURN_2(CSteamID, GetLobbyMemberByIndex, CSteamID, steamIDLobby, int32, iMember) /*virtual CSteamID GetLobbyMemberByIndex( CSteamID steamIDLobby, int32 iMember ) = 0;*/

	virtual const char *GetLobbyData( CSteamID steamIDLobby, const char *pchKey ) = 0;
	virtual bool SetLobbyData( CSteamID steamIDLobby, const char *pchKey, const char *pchValue ) = 0;

	virtual int32 GetLobbyDataCount( CSteamID steamIDLobby ) = 0;
	virtual bool GetLobbyDataByIndex( CSteamID steamIDLobby, int32 iLobbyData, char *pchKey, int32 cchKeyBufferSize, char *pchValue, int32 cchValueBufferSize ) = 0;
	virtual bool DeleteLobbyData( CSteamID steamIDLobby, const char *pchKey ) = 0;

	virtual const char *GetLobbyMemberData( CSteamID steamIDLobby, CSteamID steamIDUser, const char *pchKey ) = 0;
	virtual void SetLobbyMemberData( CSteamID steamIDLobby, const char *pchKey, const char *pchValue ) = 0;

	virtual bool SendLobbyChatMsg( CSteamID steamIDLobby, const void *pvMsgBody, int32 cubMsgBody ) = 0;
	virtual int32 GetLobbyChatEntry( CSteamID steamIDLobby, int32 iChatID, CSteamID *pSteamIDUser, void *pvData, int32 cubData, EChatEntryType *peChatEntryType ) = 0;

	virtual bool RequestLobbyData( CSteamID steamIDLobby ) = 0;

	virtual void SetLobbyGameServer( CSteamID steamIDLobby, uint32 unGameServerIP, uint16 unGameServerPort, CSteamID steamIDGameServer ) = 0;
	virtual bool GetLobbyGameServer( CSteamID steamIDLobby, uint32 *punGameServerIP, uint16 *punGameServerPort, CSteamID *psteamIDGameServer ) = 0;

	virtual bool SetLobbyMemberLimit( CSteamID steamIDLobby, int32 cMaxMembers ) = 0;
	virtual int32 GetLobbyMemberLimit( CSteamID steamIDLobby ) = 0;

	virtual void SetLobbyVoiceEnabled( CSteamID steamIDLobby, bool bVoiceEnabled ) = 0;
	virtual bool RequestFriendsLobbies() = 0;

	virtual bool SetLobbyType( CSteamID steamIDLobby, ELobbyType eLobbyType ) = 0;
	virtual bool SetLobbyJoinable( CSteamID steamIDLobby, bool bLobbyJoinable ) = 0;
	STEAMWORKS_STRUCT_RETURN_1(CSteamID, GetLobbyOwner, CSteamID, steamIDLobby) /*virtual CSteamID GetLobbyOwner( CSteamID steamIDLobby ) = 0;*/
	virtual bool SetLobbyOwner( CSteamID steamIDLobby, CSteamID steamIDNewOwner ) = 0;
	virtual bool SetLinkedLobby( CSteamID steamIDLobby, CSteamID steamIDLobby2 ) = 0;

	virtual uint64 BeginGMSQuery( AppId_t nAppId, int32 iRegionCode, const char* szFilterText ) = 0;
	virtual int32 PollGMSQuery( uint64 ullGMSQuery ) = 0;
	virtual int32 GetGMSQueryResults( uint64 ullGMSQuery, GMSQueryResult_t *pGMSQueryResults, int32 nResultBufSizeInBytes ) = 0;
	virtual void ReleaseGMSQuery( uint64 ullGMSQuery ) = 0;
	
	virtual void SendGameServerPingSample( AppId_t unAppID, int32 nSamples, const PingSample_t * pSamples ) = 0;

	virtual uint64 EnsureFavoriteGameAccountsUpdated( bool bUnk ) = 0;
};


#endif // ICLIENTMATCHMAKING_H
