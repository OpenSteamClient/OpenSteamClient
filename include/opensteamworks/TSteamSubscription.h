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

#ifndef TSTEAMSUBSCRIPTION_H
#define TSTEAMSUBSCRIPTION_H
#ifdef _WIN32
#pragma once
#endif


typedef enum EBillingType
{
	eNoCost = 0,
	eBillOnceOnly = 1,
	eBillMonthly = 2,
	eProofOfPrepurchaseOnly = 3,
	eGuestPass = 4,
	eHardwarePromo = 5,
	eGift = 6,
	eAutoGrant = 7,
	OEMTicket = 8,
	eRecurringOption = 9,
	eNumBillingTypes = 10,
} EBillingType;


typedef struct TSteamSubscription
{
	char* szName;
	unsigned int uMaxNameChars;
	unsigned int* puAppIds;
	unsigned int uMaxAppIds;
	unsigned int uId;
	unsigned int uNumApps;
	EBillingType eBillingType;
	unsigned int uCostInCents;
	unsigned int uNumDiscounts;
	int bIsPreorder;
	int bRequiresShippingAddress;
	unsigned int uDomesticShippingCostInCents;
	unsigned int uInternationalShippingCostInCents;
	bool bIsCyberCafeSubscription;
	unsigned int uGameCode;
	char szGameCodeDesc[STEAM_MAX_PATH];
	bool bIsDisabled;
	bool bRequiresCD;
	unsigned int uTerritoryCode;
	bool bIsSteam3Subscription;
} TSteamSubscription;


#endif // TSTEAMSUBSCRIPTION_H
