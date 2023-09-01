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


abstract_class IClientMatchmaking
{
public:
    virtual int32 GetFavoriteGameCount() = 0; //argc: 0, index 1
    virtual bool GetFavoriteGame( int32 iGame, AppId_t *pnAppID, uint32 *pnIP, uint16 *pnConnPort, uint16 *pnQueryPort, uint32 *punFlags, uint32 *pRTime32LastPlayedOnServer ) = 0; //argc: 7, index 0
    virtual int32 AddFavoriteGame( AppId_t nAppID, uint32 nIP, uint16 nConnPort, uint16 nQueryPort, uint32 unFlags, uint32 rTime32LastPlayedOnServer ) = 0; //argc: 6, index 1
    virtual bool RemoveFavoriteGame( AppId_t nAppID, uint32 nIP, uint16 nConnPort, uint16 nQueryPort, uint32 unFlags ) = 0; //argc: 5, index 2
    
    virtual SteamAPICall_t RequestLobbyList() = 0; //argc: 0, index 3
    
    virtual void AddRequestLobbyListStringFilter( const char *pchKeyToMatch, const char *pchValueToMatch, ELobbyComparison eComparisonType ) = 0; //argc: 3, index 0
    virtual void AddRequestLobbyListNumericalFilter( const char *pchKeyToMatch, int32 nValueToMatch, ELobbyComparison eComparisonType ) = 0; //argc: 3, index 1
    virtual void AddRequestLobbyListNearValueFilter( const char *pchKeyToMatch, int32 nValueToBeCloseTo ) = 0; //argc: 2, index 2
    virtual void AddRequestLobbyListFilterSlotsAvailable( int32 nSlotsAvailable ) = 0; //argc: 1, index 3
    virtual void AddRequestLobbyListDistanceFilter( ELobbyDistanceFilter filter ) = 0; //argc: 1, index 4
    virtual void AddRequestLobbyListResultCountFilter( int32 cMaxResults ) = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AddRequestLobbyListCompatibleMembersFilter( CSteamID steamID ) = 0; //argc: 2, index 6
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual CSteamID GetLobbyByIndex( int32 iLobby ) = 0; //argc: 2, index 7
    
    virtual SteamAPICall_t CreateLobby( ELobbyType eLobbyType, int32 cMaxMembers ) = 0; //argc: 2, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t JoinLobby( CSteamID steamIDLobby ) = 0; //argc: 2, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void LeaveLobby( CSteamID steamIDLobby ) = 0; //argc: 2, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool InviteUserToLobby( CSteamID steamIDLobby, CSteamID steamIDInvitee ) = 0; //argc: 4, index 11
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual int32 GetNumLobbyMembers( CSteamID steamIDLobby ) = 0; //argc: 2, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual CSteamID GetLobbyMemberByIndex( CSteamID steamIDLobby, int32 iMember ) = 0; //argc: 4, index 13
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual const char *GetLobbyData( CSteamID steamIDLobby, const char *pchKey ) = 0; //argc: 3, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetLobbyData( CSteamID steamIDLobby, const char *pchKey, const char *pchValue ) = 0; //argc: 4, index 15
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual int32 GetLobbyDataCount( CSteamID steamIDLobby ) = 0; //argc: 2, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool GetLobbyDataByIndex( CSteamID steamIDLobby, int32 iLobbyData, char *pchKey, int32 cchKeyBufferSize, char *pchValue, int32 cchValueBufferSize ) = 0; //argc: 7, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool DeleteLobbyData( CSteamID steamIDLobby, const char *pchKey ) = 0; //argc: 3, index 18
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual const char *GetLobbyMemberData( CSteamID steamIDLobby, CSteamID steamIDUser, const char *pchKey ) = 0; //argc: 5, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetLobbyMemberData( CSteamID steamIDLobby, const char *pchKey, const char *pchValue ) = 0; //argc: 4, index 20
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SendLobbyChatMsg( CSteamID steamIDLobby, const void *pvMsgBody, int32 cubMsgBody ) = 0; //argc: 4, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual int32 GetLobbyChatEntry( CSteamID steamIDLobby, int32 iChatID, CSteamID *pSteamIDUser, void *pvData, int32 cubData, EChatEntryType *peChatEntryType ) = 0; //argc: 7, index 22
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool RequestLobbyData( CSteamID steamIDLobby ) = 0; //argc: 2, index 23
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetLobbyGameServer( CSteamID steamIDLobby, uint32 unGameServerIP, uint16 unGameServerPort, CSteamID steamIDGameServer ) = 0; //argc: 6, index 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool GetLobbyGameServer( CSteamID steamIDLobby, uint32 *punGameServerIP, uint16 *punGameServerPort, CSteamID *psteamIDGameServer ) = 0; //argc: 5, index 25
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetLobbyMemberLimit( CSteamID steamIDLobby, int32 cMaxMembers ) = 0; //argc: 3, index 26
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual int32 GetLobbyMemberLimit( CSteamID steamIDLobby ) = 0; //argc: 2, index 27
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetLobbyVoiceEnabled( CSteamID steamIDLobby, bool bVoiceEnabled ) = 0; //argc: 3, index 28
    virtual bool RequestFriendsLobbies() = 0; //argc: 0, index 29
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetLobbyType( CSteamID steamIDLobby, ELobbyType eLobbyType ) = 0; //argc: 3, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetLobbyJoinable( CSteamID steamIDLobby, bool bLobbyJoinable ) = 0; //argc: 3, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual CSteamID GetLobbyOwner( CSteamID steamIDLobby ) = 0; //argc: 3, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetLobbyOwner( CSteamID steamIDLobby, CSteamID steamIDNewOwner ) = 0; //argc: 4, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetLinkedLobby( CSteamID steamIDLobby, CSteamID steamIDLobby2 ) = 0; //argc: 4, index 4
    
    virtual uint64 BeginGMSQuery( AppId_t nAppId, int32 iRegionCode, const char* szFilterText ) = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual int32 PollGMSQuery( uint64 ullGMSQuery ) = 0; //argc: 2, index 6
    virtual int32 GetGMSQueryResults( uint64 ullGMSQuery, GMSQueryResult_t *pGMSQueryResults, int32 nResultBufSizeInBytes ) = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ReleaseGMSQuery( uint64 ullGMSQuery ) = 0; //argc: 2, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret QueryServerByFakeIP() = 0; //argc: 4, index 9
    
    virtual uint64 EnsureFavoriteGameAccountsUpdated( bool bUnk ) = 0; //argc: 1, index 10
};


#endif // ICLIENTMATCHMAKING_H