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

#ifndef ICLIENTNETWORKDEVICEMANAGER_H
#define ICLIENTNETWORKDEVICEMANAGER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "AppsCommon.h"

enum ENetworkDeviceState
{
	// TODO : Reverse this enum
};

enum EWirelessSecurityFlags
{
	// TODO : Reverse this enum
};

struct WirelessAccessPoint_t
{
	// TODO : Reverse this struct
#ifdef _S4N_
	int m_iPadding;
#endif
};

struct WirelessCredentials_t
{
	// TODO : Reverse this struct
#ifdef _S4N_
	int m_iPadding;
#endif
};

enum ENetworkDeviceManagerError
{
	// TODO: Reverse this enum
};

#define CLIENTNETWORKDEVICEMANAGER_INTERFACE_VERSION "CLIENTNETWORKDEVICEMANAGER_INTERFACE_VERSION001"

abstract_class UNSAFE_INTERFACE IClientNetworkDeviceManager
{
public:
	virtual bool IsInterfaceValid() = 0;
	virtual void RefreshDevices() = 0;
	virtual ENetworkDeviceState GetWirelessDeviceState() = 0;
	virtual int32 GetWiredDeviceCount() = 0;
	virtual ENetworkDeviceState GetWiredDeviceState( int32 iDevice ) = 0;
	virtual bool IsWiredDevicePluggedIn( int32 iDevice ) = 0;
	virtual bool GetActiveWirelessAccessPoint( WirelessAccessPoint_t * pAccessPoint ) = 0;
	virtual bool EnumerateWirelessAccessPoints( WirelessAccessPoint_t * pAccessPoint, uint32 uUnk, uint32 * puUnk ) = 0;
	virtual bool GetCachedCredentialsForSSID( const char * pchUnk, WirelessCredentials_t * pCredentials ) = 0;
	virtual EWirelessSecurityFlags GetPreferredSecurityMethod( uint32 uUnk ) = 0;

	virtual ENetworkDeviceManagerError ActivateWiredConnection( int32 iConnection ) = 0;
	virtual void DeactivateWiredConnection( int32 iConnection ) = 0;
	virtual ENetworkDeviceManagerError ConnectToAccessPoint( const char * pchUnk1, bool bUnk, EWirelessSecurityFlags eSecurityFlags, const char * pchUnk2 ) = 0;
	virtual void DisconnectFromAccessPoint() = 0;
};

#endif // ICLIENTNETWORKDEVICEMANAGER_H
