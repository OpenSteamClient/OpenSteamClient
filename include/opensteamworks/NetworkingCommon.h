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

#ifndef NETWORKINGCOMMON_H
#define NETWORKINGCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define STEAMNETWORKING_INTERFACE_VERSION_001 "SteamNetworking001"
#define STEAMNETWORKING_INTERFACE_VERSION_002 "SteamNetworking002"
#define STEAMNETWORKING_INTERFACE_VERSION_003 "SteamNetworking003"
#define STEAMNETWORKING_INTERFACE_VERSION_004 "SteamNetworking004"
#define STEAMNETWORKING_INTERFACE_VERSION_005 "SteamNetworking005"

#define CLIENTNETWORKING_INTERFACE_VERSION "ClientNetworking001"

// SendP2PPacket() send types
// Typically k_EP2PSendUnreliable is what you want for UDP-like packets, k_EP2PSendReliable for TCP-like packets
enum EP2PSend
{
	// Basic UDP send. Packets can't be bigger than 1200 bytes (your typical MTU size). Can be lost, or arrive out of order (rare).
	// The sending API does have some knowledge of the underlying connection, so if there is no NAT-traversal accomplished or
	// there is a recognized adjustment happening on the connection, the packet will be batched until the connection is open again.
	k_EP2PSendUnreliable = 0,

	// As above, but if the underlying p2p connection isn't yet established the packet will just be thrown away. Using this on the first
	// packet sent to a remote host almost guarantees the packet will be dropped.
	// This is only really useful for kinds of data that should never buffer up, i.e. voice payload packets
	k_EP2PSendUnreliableNoDelay = 1,

	// Reliable message send. Can send up to 1MB of data in a single message. 
	// Does fragmentation/re-assembly of messages under the hood, as well as a sliding window for efficient sends of large chunks of data. 
	k_EP2PSendReliable = 2,

	// As above, but applies the Nagle algorithm to the send - sends will accumulate 
	// until the current MTU size (typically ~1200 bytes, but can change) or ~200ms has passed (Nagle algorithm). 
	// Useful if you want to send a set of smaller messages but have the coalesced into a single packet
	// Since the reliable stream is all ordered, you can do several small message sends with k_EP2PSendReliableWithBuffering and then
	// do a normal k_EP2PSendReliable to force all the buffered data to be sent.
	k_EP2PSendReliableWithBuffering = 3,

};

// list of possible errors returned by SendP2PPacket() API
// these will be posted in the P2PSessionConnectFail_t callback
enum EP2PSessionError
{
	k_EP2PSessionErrorNone = 0,
	k_EP2PSessionErrorNotRunningApp = 1,			// target is not running the same game
	k_EP2PSessionErrorNoRightsToApp = 2,			// local user doesn't own the app that is running
	k_EP2PSessionErrorDestinationNotLoggedIn = 3,	// target user isn't connected to Steam
	k_EP2PSessionErrorTimeout = 4,					// target isn't responding, perhaps not calling AcceptP2PSessionWithUser()
	// corporate firewalls can also block this (NAT traversal is not firewall traversal)
	// make sure that UDP ports 3478, 4379, and 4380 are open in an outbound direction
	k_EP2PSessionErrorMax = 5
};

// describes how the socket is currently connected
enum ESNetSocketConnectionType
{
	k_ESNetSocketConnectionTypeNotConnected = 0,
	k_ESNetSocketConnectionTypeUDP = 1,
	k_ESNetSocketConnectionTypeUDPRelay = 2,
};

// connection progress indicators
enum ESNetSocketState
{
	k_ESNetSocketStateInvalid = 0,						

	// communication is valid
	k_ESNetSocketStateConnected = 1,				

	// states while establishing a connection
	k_ESNetSocketStateInitiated = 10,				// the connection state machine has started

	// p2p connections
	k_ESNetSocketStateLocalCandidatesFound = 11,	// we've found our local IP info
	k_ESNetSocketStateReceivedRemoteCandidates = 12,// we've received information from the remote machine, via the Steam back-end, about their IP info

	// direct connections
	k_ESNetSocketStateChallengeHandshake = 15,		// we've received a challenge packet from the server

	// failure states
	k_ESNetSocketStateDisconnecting = 21,			// the API shut it down, and we're in the process of telling the other end	
	k_ESNetSocketStateLocalDisconnect = 22,			// the API shut it down, and we've completed shutdown
	k_ESNetSocketStateTimeoutDuringConnect = 23,	// we timed out while trying to creating the connection
	k_ESNetSocketStateRemoteEndDisconnected = 24,	// the remote end has disconnected from us
	k_ESNetSocketStateConnectionBroken = 25,		// connection has been broken; either the other end has disappeared or our local network connection has broke

};


// handle to a socket
typedef uint32 SNetSocket_t;		// CreateP2PConnectionSocket()
typedef uint32 SNetListenSocket_t;	// CreateListenSocket()


#pragma pack( push, 8 )

// connection state to a specified user, returned by GetP2PSessionState()
// this is under-the-hood info about what's going on with a SendP2PPacket(), shouldn't be needed except for debuggin
struct P2PSessionState_t
{
	uint8 m_bConnectionActive;		// true if we've got an active open connection
	uint8 m_bConnecting;			// true if we're currently trying to establish a connection
	uint8 m_eP2PSessionError;		// last error recorded (see enum above)
	uint8 m_bUsingRelay;			// true if it's going through a relay server (TURN)
	int32 m_nBytesQueuedForSend;
	int32 m_nPacketsQueuedForSend;
	uint32 m_nRemoteIP;				// potential IP:Port of remote host. Could be TURN server. 
	uint16 m_nRemotePort;			// Only exists for compatibility with older authentication api's
};


// callback notification - status of a socket has changed
// used as part of the CreateListenSocket() / CreateP2PConnectionSocket() 
struct SocketStatusCallback_t
{ 
	enum { k_iCallback = k_iSteamNetworkingCallbacks + 1 };

	SNetSocket_t m_hSocket;				// the socket used to send/receive data to the remote host
	SNetListenSocket_t m_hListenSocket;	// this is the server socket that we were listening on; NULL if this was an outgoing connection
	CSteamID m_steamIDRemote;			// remote steamID we have connected to, if it has one
	int m_eSNetSocketState;				// socket state, ESNetSocketState
};


// callback notification - a user wants to talk to us over the P2P channel via the SendP2PPacket() API
// in response, a call to AcceptP2PPacketsFromUser() needs to be made, if you want to talk with them
struct P2PSessionRequest_t
{ 
	enum { k_iCallback = k_iSteamNetworkingCallbacks + 2 };

	CSteamID m_steamIDRemote;			// user who wants to talk to us
};


// callback notification - packets can't get through to the specified user via the SendP2PPacket() API
// all packets queued packets unsent at this point will be dropped
// further attempts to send will retry making the connection (but will be dropped if we fail again)
struct P2PSessionConnectFail_t
{ 
	enum { k_iCallback = k_iSteamNetworkingCallbacks + 3 };

	CSteamID m_steamIDRemote;			// user we were sending packets to
	uint8 m_eP2PSessionError;			// EP2PSessionError indicating why we're having trouble
};
#pragma pack( pop )

#endif // NETWORKINGCOMMON_H
