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

#ifndef ICLIENTGAMESERVER_H
#define ICLIENTGAMESERVER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "GameServerCommon.h"
#include "UserCommon.h"


typedef enum EGameConnectSteamResponse
{
	k_EGameConnectSteamResponse_WaitingForResponse = 0,
	k_EGameConnectSteamResponse_AuthorizedToPlay = 1,
	k_EGameConnectSteamResponse_Denied = 2,
	k_EGameConnectSteamResponse_ExceededReasonableTime_StillWaiting = 3,
} EGameConnectSteamResponse;

struct ConnectedUserInfo_t
{
	int32 m_cubConnectedUserInfo;
	int32 m_nCountOfGuestUsers;
	CSteamID m_SteamID;
	uint32 m_unIPPublic;
	uint32 m_nFrags;
	double m_flConnectTime;
	EGameConnectSteamResponse m_eGameConnectSteamResponse;
	EDenyReason m_eDenyReason;
};

abstract_class IClientGameServer
{
public:
	// returns the HSteamUser this interface represents
	virtual HSteamUser GetHSteamUser() = 0;

	virtual bool InitGameServer( uint32 unGameIP, uint16 unGamePort, uint16 usQueryPort, uint32 unServerFlags, AppId_t nAppID, const char *pchVersion ) = 0;
	virtual void SetProduct( const char *pchProductName ) = 0;
	virtual void SetGameDescription( const char *pchGameDescription ) = 0;
	virtual void SetModDir( const char *pchModDir ) = 0;
	virtual void SetDedicatedServer( bool bDedicatedServer ) = 0;
	virtual void LogOn( const char *pchToken ) = 0;
	virtual void LogOnAnonymous() = 0;
	virtual void LogOff() = 0;

	virtual CSteamID GetSteamID() = 0;

	virtual bool BLoggedOn() = 0;

	virtual bool BSecure() = 0;

	// Returns true if the master server has requested a restart.
	// Only returns true once per request.
	virtual bool WasRestartRequested() = 0;

	virtual void SetMaxPlayerCount( int32 cPlayersMax ) = 0;
	virtual void SetBotPlayerCount( int32 cBotPlayers ) = 0;
	virtual void SetServerName( const char *pchServerName ) = 0;
	virtual void SetMapName( const char *pchMapName ) = 0;
	virtual void SetPasswordProtected( bool bPasswordProtected ) = 0;

	// This can be called if spectator goes away or comes back (passing 0 means there is no spectator server now).
	virtual void SetSpectatorPort( uint16 unSpectatorPort ) = 0;

	virtual void SetSpectatorServerName( const char *pchSpectatorServerName ) = 0;

	// Call this to clear the whole list of key/values that are sent in rules queries.
	virtual void ClearAllKeyValues() = 0;

	// Call this to add/update a key/value pair.
	virtual void SetKeyValue( const char *pKey, const char *pValue ) = 0;

	// Sets a string defining the "gametags" for this server, this is optional, but if it is set
	// it allows users to filter in the matchmaking/server-browser interfaces based on the value
	virtual void SetGameTags( const char *pchGameTags ) = 0; 

	// Sets a string defining the "gamedata" for this server, this is optional, but if it is set
	// it allows users to filter in the matchmaking/server-browser interfaces based on the value
	// don't set this unless it actually changes, its only uploaded to the master once (when
	// acknowledged)
	virtual void SetGameData( const char *pchGameData ) = 0; 

	virtual void SetRegion( const char *pchRegionName ) = 0;

	// Handles receiving a new connection from a Steam user.  This call will ask the Steam
	// servers to validate the users identity, app ownership, and VAC status.  If the Steam servers 
	// are off-line, then it will validate the cached ticket itself which will validate app ownership 
	// and identity.  The AuthBlob here should be acquired on the game client using SteamUser()->InitiateGameConnection()
	// and must then be sent up to the game server for authentication.
	//
	// Return Value: returns true if the users ticket passes basic checks. pSteamIDUser will contain the Steam ID of this user. pSteamIDUser must NOT be NULL
	// If the call succeeds then you should expect a GSClientApprove_t or GSClientDeny_t callback which will tell you whether authentication
	// for the user has succeeded or failed (the steamid in the callback will match the one returned by this call)
	virtual EUserConnect SendUserConnectAndAuthenticate( uint32 unIPClient, const void *pvAuthBlob, uint32 cubAuthBlobSize, CSteamID *pSteamIDUser ) = 0;

	// Creates a fake user (ie, a bot) which will be listed as playing on the server, but skips validation.  
	// 
	// Return Value: Returns a SteamID for the user to be tracked with, you should call HandleUserDisconnect()
	// when this user leaves the server just like you would for a real user.
	virtual CSteamID CreateUnauthenticatedUserConnection() = 0;

	// Should be called whenever a user leaves our game server, this lets Steam internally
	// track which users are currently on which servers for the purposes of preventing a single
	// account being logged into multiple servers, showing who is currently on a server, etc.
	virtual void SendUserDisconnect( CSteamID steamIDUser ) = 0;

	// Update the data to be displayed in the server browser and matchmaking interfaces for a user
	// currently connected to the server.  For regular users you must call this after you receive a
	// GSUserValidationSuccess callback.
	// 
	// Return Value: true if successful, false if failure (ie, steamIDUser wasn't for an active player)
	virtual bool BUpdateUserData( CSteamID steamIDUser, const char *pchPlayerName, uint32 uScore ) = 0;

	// New auth system APIs - do not mix with the old auth system APIs.
	// ----------------------------------------------------------------

	// Retrieve ticket to be sent to the entity who wishes to authenticate you ( using BeginAuthSession API ). 
	// pcbTicket retrieves the length of the actual ticket.
	virtual HAuthTicket GetAuthSessionTicket( void *pTicket, int32 cbMaxTicket, uint32 *pcbTicket ) = 0;

	// Authenticate ticket ( from GetAuthSessionTicket ) from entity steamID to be sure it is valid and isnt reused
	// Registers for callbacks if the entity goes offline or cancels the ticket ( see ValidateAuthTicketResponse_t callback and EAuthSessionResponse )
	virtual EBeginAuthSessionResult BeginAuthSession( const void *pAuthTicket, int32 cbAuthTicket, CSteamID steamID ) = 0;

	// Stop tracking started by BeginAuthSession - called when no longer playing game with this entity
	virtual void EndAuthSession( CSteamID steamID ) = 0;

	// Cancel auth ticket from GetAuthSessionTicket, called when no longer playing game with the entity you gave the ticket to
	virtual void CancelAuthTicket( HAuthTicket hAuthTicket ) = 0;

	// After receiving a user's authentication data, and passing it to SendUserConnectAndAuthenticate, use this function
	// to determine if the user owns downloadable content specified by the provided AppID.
	virtual EUserHasLicenseForAppResult IsUserSubscribedAppInTicket( CSteamID steamID, AppId_t appID ) = 0;

	// Ask if a user in in the specified group, results returns async by GSUserGroupStatus_t
	// returns false if we're not connected to the steam servers and thus cannot ask
	virtual bool RequestUserGroupStatus( CSteamID steamIDUser, CSteamID steamIDGroup ) = 0;

	// Ask for the gameplay stats for the server. Results returned in a callback
	virtual void GetGameplayStats( ) = 0;

	// Gets the reputation score for the game server. This API also checks if the server or some
	// other server on the same IP is banned from the Steam master servers.
	virtual SteamAPICall_t GetServerReputation( ) = 0;

	// Returns the public IP of the server according to Steam, useful when the server is 
	// behind NAT and you want to advertise its IP in a lobby for other clients to directly
	// connect to
	virtual uint32 GetPublicIP() = 0;
	
	// These are in GameSocketShare mode, where instead of ISteamGameServer creating its own
	// socket to talk to the master server on, it lets the game use its socket to forward messages
	// back and forth. This prevents us from requiring server ops to open up yet another port
	// in their firewalls.
	//
	// the IP address and port should be in host order, i.e 127.0.0.1 == 0x7f000001

	// These are used when you've elected to multiplex the game server's UDP socket
	// rather than having the master server updater use its own sockets.
	// 
	// Source games use this to simplify the job of the server admins, so they 
	// don't have to open up more ports on their firewalls.

	// Call this when a packet that starts with 0xFFFFFFFF comes in. That means
	// it's for us.
	virtual bool HandleIncomingPacket( const void *pData, int32 cbData, uint32 srcIP, uint16 srcPort ) = 0;

	// AFTER calling HandleIncomingPacket for any packets that came in that frame, call this.
	// This gets a packet that the master server updater needs to send out on UDP.
	// It returns the length of the packet it wants to send, or 0 if there are no more packets to send.
	// Call this each frame until it returns 0.
	virtual int32 GetNextOutgoingPacket( void *pOut, int32 cbMaxOut, uint32 *pNetAdr, uint16 *pPort ) = 0;

	virtual void EnableHeartbeats( bool bEnabled ) = 0;
	virtual void SetHeartbeatInterval( int32 iInterval ) = 0;

	// Force it to request a heartbeat from the master servers.
	virtual void ForceHeartbeat() = 0;


	virtual ELogonState GetLogonState() = 0;
	virtual bool BConnected() = 0;

	virtual int32 RaiseConnectionPriority( EConnectionPriority eConnectionPriority ) = 0;
	virtual void ResetConnectionPriority( int32 hRaiseConnectionPriorityPrev ) = 0;

	virtual void SetCellID( CellID_t cellID ) = 0;

	virtual void TrackSteamUsageEvent( ESteamUsageEvent eSteamUsageEvent, const uint8 *pubKV, uint32 cubKV ) = 0;

	virtual void SetCountOfSimultaneousGuestUsersPerSteamAccount( int32 nCount ) = 0;

	virtual bool EnumerateConnectedUsers( int32 iterator, ConnectedUserInfo_t *pConnectedUserInfo ) = 0;

	virtual SteamAPICall_t AssociateWithClan( CSteamID clanID ) = 0;
	virtual SteamAPICall_t ComputeNewPlayerCompatibility( CSteamID steamID ) = 0;

	// Ask if a user has a specific achievement for this game, will get a callback on reply
	virtual bool _BGetUserAchievementStatus( CSteamID steamID, const char *pchAchievementName ) = 0;

	virtual void _GSSetSpawnCount( uint32 ucSpawn ) = 0;
	virtual bool _GSGetSteam2GetEncryptionKeyToSendToNewClient( void *pvEncryptionKey, uint32 *pcbEncryptionKey, uint32 cbMaxEncryptionKey ) = 0;

	virtual bool _GSSendSteam2UserConnect( uint32 unUserID, const void *pvRawKey, uint32 unKeyLen, uint32 unIPPublic, uint16 usPort, const void *pvCookie, uint32 cubCookie ) = 0;
	virtual bool _GSSendSteam3UserConnect( CSteamID steamID, uint32 unIPPublic, const void *pvCookie, uint32 cubCookie ) = 0;

	virtual bool _GSSendUserConnect( uint32 unUserID, uint32 unIPPublic, uint16 usPort, const void *pvCookie, uint32 cubCookie ) = 0;
	virtual bool _GSRemoveUserConnect( uint32 unUserID ) = 0;

	// Updates server status values which shows up in the server browser and matchmaking APIs
	virtual bool _GSUpdateStatus( int32 cPlayers, int32 cPlayersMax, int32 cBotPlayers, const char *pchServerName, const char *pSpectatorServerName, const char *pchMapName ) = 0;

	virtual bool _GSCreateUnauthenticatedUser( CSteamID *pSteamID ) = 0;

	virtual bool _GSSetServerType( int32 iAppID, uint32 unServerFlags, uint32 unGameIP, uint16 unGamePort, uint16 unSpectatorPort, uint16 usQueryPort, const char *pchGameDir, const char *pchVersion, bool bLANMode ) = 0;
	virtual void _SetBasicServerData( unsigned short nProtocolVersion, bool bDedicatedServer, const char *pRegionName, const char *pProductName, unsigned short nMaxReportedClients, bool bPasswordProtected, const char *pGameDescription ) = 0;

	virtual bool _GSSendUserDisconnect( CSteamID, uint32 unUserID ) = 0;
};


#endif // ICLIENTGAMESERVER_H
