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

#ifndef ICLIENTBILLING_H
#define ICLIENTBILLING_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "BillingCommon.h"


enum EPackageStatus
{
};


abstract_class IClientBilling
{
public:
	virtual bool PurchaseWithActivationCode( const char *pchActivationCode ) = 0;
	virtual bool HasActiveLicense( AppId_t ) = 0;
	virtual bool GetLicenseInfo( uint32 nLicenseIndex, RTime32* pRTime32Created, RTime32* pRTime32NextProcess, int32* pnMinuteLimit, int32 * pnMinutesUsed, EPaymentMethod* pePaymentMethod, uint32* punFlags, int32 * pnTerritoryCode, char * prgchPurchaseCountryCode /* Use a 3 bytes buffer */) = 0;
	virtual void EnableTestLicense( PackageId_t unPackageID ) = 0;
	virtual void DisableTestLicense( PackageId_t unPackageID ) = 0;
	virtual uint32 GetAppsInPackage( PackageId_t unPackageID, AppId_t puIds[], uint32 uMaxIds ) = 0;
	virtual SteamAPICall_t RequestFreeLicenseForApps(AppId_t puIds[], uint32 puIdsLength) = 0;
};

#endif // ICLIENTBILLING_H
