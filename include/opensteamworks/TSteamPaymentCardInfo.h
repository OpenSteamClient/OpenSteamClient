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

#ifndef TSTEAMPAYMENTCARDINFO_H
#define TSTEAMPAYMENTCARDINFO_H
#ifdef _WIN32
#pragma once
#endif

typedef struct TSteamPaymentCardInfo
{
	ESteamPaymentCardType eCardType;
	char szCardNumber[ STEAM_CARD_NUMBER_SIZE +1 ];
	char szCardHolderName[ STEAM_CARD_HOLDERNAME_SIZE + 1];
	char szCardExpYear[ STEAM_CARD_EXPYEAR_SIZE + 1 ];
	char szCardExpMonth[ STEAM_CARD_EXPMONTH_SIZE+ 1 ];
	char szCardCVV2[ STEAM_CARD_CVV2_SIZE + 1 ];
	char szBillingAddress1[ STEAM_BILLING_ADDRESS1_SIZE + 1 ];
	char szBillingAddress2[ STEAM_BILLING_ADDRESS2_SIZE + 1 ];
	char szBillingCity[ STEAM_BILLING_CITY_SIZE + 1 ];
	char szBillingZip[ STEAM_BILLING_ZIP_SIZE + 1 ];
	char szBillingState[ STEAM_BILLING_STATE_SIZE + 1 ];
	char szBillingCountry[ STEAM_BILLING_COUNTRY_SIZE + 1 ];
	char szBillingPhone[ STEAM_BILLING_PHONE_SIZE + 1 ];
	char szBillingEmailAddress[ STEAM_BILLING_EMAIL_SIZE + 1 ];
	unsigned int uExpectedCostInCents;
	unsigned int uExpectedTaxInCents;
	char szShippingName[ STEAM_CARD_HOLDERNAME_SIZE + 1 ];
	char szShippingAddress1[ STEAM_BILLING_ADDRESS1_SIZE + 1 ];
	char szShippingAddress2[ STEAM_BILLING_ADDRESS2_SIZE + 1 ];
	char szShippingCity[ STEAM_BILLING_CITY_SIZE + 1 ];
	char szShippingZip[ STEAM_BILLING_ZIP_SIZE + 1 ];
	char szShippingState[ STEAM_BILLING_STATE_SIZE + 1 ];
	char szShippingCountry[ STEAM_BILLING_COUNTRY_SIZE + 1 ];
	char szShippingPhone[ STEAM_BILLING_PHONE_SIZE + 1];
	unsigned int uExpectedShippingCostInCents;
} TSteamPaymentCardInfo;


#endif // TSTEAMPAYMENTCARDINFO_H
