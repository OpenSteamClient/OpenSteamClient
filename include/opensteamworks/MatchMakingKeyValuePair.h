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

#ifndef MATCHMAKINGKEYVALUEPAIR_H
#define MATCHMAKINGKEYVALUEPAIR_H
#ifdef _WIN32
#pragma once
#endif

#ifdef _S4N_
	#define strncpy(...)
#else
	#include <stdio.h>
	#include <string.h>
#endif

struct MatchMakingKeyValuePair_t
{
	MatchMakingKeyValuePair_t() { m_szKey[0] = m_szValue[0] = 0; }
	

	#ifdef _MSC_VER
		#pragma warning(push) 
		#pragma warning(disable: 4996) 
	#endif

	MatchMakingKeyValuePair_t( const char *pchKey, const char *pchValue )
	{
		strncpy( m_szKey, pchKey, sizeof(m_szKey) ); // this is a public header, use basic c library string funcs only!
		m_szKey[ sizeof( m_szKey ) - 1 ] = '\0';
		strncpy( m_szValue, pchValue, sizeof(m_szValue) );
		m_szValue[ sizeof( m_szValue ) - 1 ] = '\0';
	}

	#ifdef _MSC_VER
		#pragma warning(pop) 
	#endif

	char m_szKey[ 256 ];
	char m_szValue[ 256 ];
};



#endif // MATCHMAKINGKEYVALUEPAIR_H
