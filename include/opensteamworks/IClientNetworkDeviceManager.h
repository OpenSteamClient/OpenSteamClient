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

abstract_class IClientNetworkDeviceManager
{
public:
    virtual bool IsInterfaceValid() = 0; //argc: 0, index 1
    virtual void RefreshDevices() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetNetworkDevicesData() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ConnectToDevice() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DisconnectFromDevice() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetWifiEnabled() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetWifiScanningEnabled() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ForgetWirelessEndpoint() = 0; //argc: 2, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetWirelessEndpointAutoconnect() = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetCustomIPSettings() = 0; //argc: 6, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetCustomIPSettings() = 0; //argc: 6, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetProxyInfo() = 0; //argc: 4, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetProxyInfo() = 0; //argc: 5, index 10
    virtual unknown_ret GetObviousConnectivityProblem() = 0; //argc: 0, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TEST_SetFakeLocalSystemStateSetting() = 0; //argc: 1, index 0
    virtual unknown_ret TEST_GetFakeLocalSystemStateSetting() = 0; //argc: 0, index 1
    virtual unknown_ret TEST_GetFakeLocalSystemEffectiveState() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TEST_SetEmulateSingleWirelessDevice() = 0; //argc: 1, index 0
    virtual unknown_ret TEST_GetEmulateSingleWirelessDevice() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_EnumerateNetworkDevices() = 0; //argc: 2, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceType() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_IsCurrentDevice() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_IsCurrentlyConnected() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceIP4() = 0; //argc: 3, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceBroadcastIP4() = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceIPV6InterfaceIndex() = 0; //argc: 1, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceVendor() = 0; //argc: 1, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceProduct() = 0; //argc: 1, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetMacAddress() = 0; //argc: 1, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetSubnetMaskBitCount() = 0; //argc: 3, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetRouterAddressIP4() = 0; //argc: 3, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDNSResolversIP4() = 0; //argc: 3, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDeviceState() = 0; //argc: 1, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetDevicePluggedState() = 0; //argc: 1, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_EnumerateWirelessEndpoints() = 0; //argc: 3, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetConnectedWirelessEndpointSSID() = 0; //argc: 1, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetWirelessSecurityCapabilities() = 0; //argc: 1, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetWirelessEndpointSSIDUserDisplayString() = 0; //argc: 2, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetWirelessEndpointStrength() = 0; //argc: 2, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_IsSecurityRequired() = 0; //argc: 2, index 20
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_GetCachedWirelessCredentials() = 0; //argc: 2, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_IsWirelessEndpointForgettable() = 0; //argc: 2, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LEGACY_IsUsingDHCP() = 0; //argc: 1, index 23
};

#endif // ICLIENTNETWORKDEVICEMANAGER_H