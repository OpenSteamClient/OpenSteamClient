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

#ifndef ICLIENTMASTERSERVERUPDATER_H
#define ICLIENTMASTERSERVERUPDATER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "MasterServerUpdaterCommon.h"

//-----------------------------------------------------------------------------
// Purpose: Game engines use this to tell the Steam master servers
// about their games so their games can show up in the server browser.
//-----------------------------------------------------------------------------
abstract_class OBSOLETE_INTERFACE IClientMasterServerUpdater
{
public:

	// Call this as often as you like to tell the master server updater whether or not
	// you want it to be active (default: off).
	virtual void SetActive( bool bActive ) = 0;

	// You usually don't need to modify this.
	// Pass -1 to use the default value for iHeartbeatInterval.
	// Some mods change this.
	virtual void SetHeartbeatInterval( int iHeartbeatInterval ) = 0;


	// These are in GameSocketShare mode, where instead of ISteamMasterServerUpdater creating its own
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
	virtual bool HandleIncomingPacket( const void *pData, int cbData, uint32 srcIP, uint16 srcPort ) = 0;

	// AFTER calling HandleIncomingPacket for any packets that came in that frame, call this.
	// This gets a packet that the master server updater needs to send out on UDP.
	// It returns the length of the packet it wants to send, or 0 if there are no more packets to send.
	// Call this each frame until it returns 0.
	virtual int GetNextOutgoingPacket( void *pOut, int cbMaxOut, uint32 *pNetAdr, uint16 *pPort ) = 0;


	// Functions to set various fields that are used to respond to queries.

	// Call this to set basic data that is passed to the server browser.
	virtual void SetBasicServerData( unsigned short nProtocolVersion, bool bDedicatedServer, const char *pRegionName, const char *pProductName, unsigned short nMaxReportedClients, bool bPasswordProtected, const char *pGameDescription ) = 0;

	// Call this to clear the whole list of key/values that are sent in rules queries.
	virtual void ClearAllKeyValues() = 0;

	// Call this to add/update a key/value pair.
	virtual void SetKeyValue( const char *pKey, const char *pValue ) = 0;


	// You can call this upon shutdown to clear out data stored for this game server and
	// to tell the master servers that this server is going away.
	virtual void NotifyShutdown() = 0;

	// Returns true if the master server has requested a restart.
	// Only returns true once per request.
	virtual bool WasRestartRequested() = 0;

	// Force it to request a heartbeat from the master servers.
	virtual void ForceHeartbeat() = 0;

	// Manually edit and query the master server list.
	// It will provide name resolution and use the default master server port if none is provided.
	virtual bool AddMasterServer( const char *pServerAddress ) = 0;
	virtual bool RemoveMasterServer( const char *pServerAddress ) = 0;

	virtual int GetNumMasterServers() = 0;

	// Returns the # of bytes written to pOut.
	virtual int GetMasterServerAddress( int iServer, char *pOut, int outBufferSize ) = 0;
};

#endif // ICLIENTMASTERSERVERUPDATER_H
