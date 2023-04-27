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

#ifndef TSTEAMPREPURCHASEINFO_H
#define TSTEAMPREPURCHASEINFO_H
#ifdef _WIN32
#pragma once
#endif

typedef struct TSteamPrepurchaseInfo
{
	char					szTypeOfProofOfPurchase[ STEAM_TYPE_OF_PROOF_OF_PURCHASE_SIZE + 1 ];

	// A ProofOfPurchase token is not necessarily a nul-terminated string; it may be binary data
	// (perhaps encrypted). Hence we need a length and an array of bytes.
	unsigned int			uLengthOfBinaryProofOfPurchaseToken;	
	char					cBinaryProofOfPurchaseToken[ STEAM_PROOF_OF_PURCHASE_TOKEN_SIZE + 1 ];
} TSteamPrepurchaseInfo;

#endif // TSTEAMPREPURCHASEINFO_H
