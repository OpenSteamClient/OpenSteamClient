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


abstract_class UNSAFE_INTERFACE IClientBilling
{
public:
	virtual unknown_ret PurchaseWithActivationCode(char const*) = 0;
    virtual unknown_ret HasActiveLicense(unsigned int) = 0;
    virtual unknown_ret GetLicenseInfo(unsigned int, unsigned int*, unsigned int*, int*, int*, EPaymentMethod*, unsigned int*, int*, char*) = 0;
    virtual unknown_ret EnableTestLicense(unsigned int) = 0;
    virtual unknown_ret DisableTestLicense(unsigned int) = 0;
    virtual unknown_ret GetAppsInPackage(unsigned int, unsigned int*, unsigned int) = 0;
    virtual unknown_ret RequestFreeLicenseForApps(unsigned int const*, unsigned int) = 0;
};

#endif // ICLIENTBILLING_H
