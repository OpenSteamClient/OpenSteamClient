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

#ifndef ESTEAMSUBSCRIPTIONSTATUS_H
#define ESTEAMSUBSCRIPTIONSTATUS_H
#ifdef _WIN32
#pragma once
#endif


typedef enum ESteamSubscriptionStatus
{
	eSteamSubscriptionOK = 0,
	eSteamSubscriptionPending = 1,
	eSteamSubscriptionPreorder = 2,
	eSteamSubscriptionPrepurchaseTransferred = 3,
	eSteamSubscriptionPrepurchaseInvalid = 4,
	eSteamSubscriptionPrepurchaseRejected = 5,
	eSteamSubscriptionPrepurchaseRevoked = 6,
	eSteamSubscriptionPaymentCardDeclined = 7,
	eSteamSubscriptionCancelledByUser = 8,
	eSteamSubscriptionCancelledByVendor = 9,
	eSteamSubscriptionPaymentCardUseLimit = 10,
	eSteamSubscriptionPaymentCardAlert = 11,
	eSteamSubscriptionFailed = 12,
	eSteamSubscriptionPaymentCardAVSFailure = 13,
	eSteamSubscriptionPaymentCardInsufficientFunds = 14,
	eSteamSubscriptionRestrictedCountry = 15,
} ESteamSubscriptionStatus;


#endif // ESTEAMSUBSCRIPTIONSTATUS_H
