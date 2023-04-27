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

#ifndef GAMESERVERITEM_H
#define GAMESERVERITEM_H
#ifdef _WIN32
#pragma once
#endif

//-----------------------------------------------------------------------------
// Purpose: Data describing a single server
//-----------------------------------------------------------------------------
class gameserveritem_t
{
public:
	gameserveritem_t();

	const char* GetName() const;
	void SetName( const char *pName );

public:
	servernetadr_t m_NetAdr;		// IP/Query Port/Connection Port for this server
	int m_nPing;					// current ping time in milliseconds
	bool m_bHadSuccessfulResponse;	// server has responded successfully in the past
	bool m_bDoNotRefresh;			// server is marked as not responding and should no longer be refreshed
	char m_szGameDir[32];			// current game directory
	char m_szMap[32];				// current map
	char m_szGameDescription[64];	// game description
	uint32 m_nAppID;				// Steam App ID of this server
	int m_nPlayers;					// current number of players on the server
	int m_nMaxPlayers;				// Maximum players that can join this server
	int m_nBotPlayers;				// Number of bots (i.e simulated players) on this server
	bool m_bPassword;				// true if this server needs a password to join
	bool m_bSecure;					// Is this server protected by VAC
	uint32 m_ulTimeLastPlayed;		// time (in unix time) when this server was last played on (for favorite/history servers)
	int	m_nServerVersion;			// server version as reported to Steam

private:
	char m_szServerName[64];		//  Game server name

	// For data added after SteamMatchMaking001 add it here
public:
	char m_szGameTags[128];			// the tags this server exposes
	CSteamID m_steamID;				// steamID of the game server - invalid if it's doesn't have one (old server, or not connected to Steam)
};


inline gameserveritem_t::gameserveritem_t()
{
	m_szGameDir[0] = m_szMap[0] = m_szGameDescription[0] = m_szServerName[0] = 0;
	m_bHadSuccessfulResponse = m_bDoNotRefresh = m_bPassword = m_bSecure = false;
	m_nPing = m_nAppID = m_nPlayers = m_nMaxPlayers = m_nBotPlayers = m_ulTimeLastPlayed = m_nServerVersion = 0;
	m_szGameTags[0] = 0;
}

inline const char* gameserveritem_t::GetName() const
{
	// Use the IP address as the name if nothing is set yet.
	if ( m_szServerName[0] == 0 )
		return m_NetAdr.GetConnectionAddressString();
	else
		return m_szServerName;
}

#ifdef _S4N_
	#define strncpy(...)
#elif defined(_MSC_VER)
	#pragma warning(push) 
	#pragma warning(disable: 4996) 
#endif

inline void gameserveritem_t::SetName( const char *pName )
{
	strncpy( m_szServerName, pName, sizeof( m_szServerName ) );
	m_szServerName[ sizeof( m_szServerName ) - 1 ] = '\0';
}

#ifdef _MSC_VER
	#pragma warning(pop) 
#endif

#endif // GAMESERVERITEM_H
