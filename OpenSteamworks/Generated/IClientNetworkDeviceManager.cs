//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientNetworkDeviceManager
{
    public unknown_ret IsInterfaceValid();  // argc: 0, index: 1
    public unknown_ret RefreshDevices();  // argc: 0, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret GetNetworkDevicesData();  // argc: 1, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret ConnectToDevice();  // argc: 1, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret DisconnectFromDevice();  // argc: 1, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret SetWifiEnabled();  // argc: 1, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret SetWifiScanningEnabled();  // argc: 1, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret ForgetWirelessEndpoint();  // argc: 2, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret SetWirelessEndpointAutoconnect();  // argc: 3, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret SetCustomIPSettings();  // argc: 6, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret GetCustomIPSettings();  // argc: 6, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret SetProxyInfo();  // argc: 3, index: 12
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetProxyInfo();  // argc: 4, index: 13
    public unknown_ret GetObviousConnectivityProblem();  // argc: 0, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret TEST_SetFakeLocalSystemStateSetting();  // argc: 1, index: 15
    public unknown_ret TEST_GetFakeLocalSystemStateSetting();  // argc: 0, index: 16
    public unknown_ret TEST_GetFakeLocalSystemEffectiveState();  // argc: 0, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret TEST_SetEmulateSingleWirelessDevice();  // argc: 1, index: 18
    public unknown_ret TEST_GetEmulateSingleWirelessDevice();  // argc: 0, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_EnumerateNetworkDevices();  // argc: 2, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceType();  // argc: 1, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsCurrentDevice();  // argc: 1, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsCurrentlyConnected();  // argc: 1, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceIP4();  // argc: 3, index: 24
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceBroadcastIP4();  // argc: 3, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceIPV6InterfaceIndex();  // argc: 1, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceVendor();  // argc: 1, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceProduct();  // argc: 1, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetMacAddress();  // argc: 1, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetSubnetMaskBitCount();  // argc: 3, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetRouterAddressIP4();  // argc: 3, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDNSResolversIP4();  // argc: 3, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceState();  // argc: 1, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDevicePluggedState();  // argc: 1, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_EnumerateWirelessEndpoints();  // argc: 3, index: 35
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetConnectedWirelessEndpointSSID();  // argc: 1, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetWirelessSecurityCapabilities();  // argc: 1, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetWirelessEndpointSSIDUserDisplayString();  // argc: 2, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetWirelessEndpointStrength();  // argc: 2, index: 39
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsSecurityRequired();  // argc: 2, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetCachedWirelessCredentials();  // argc: 2, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsWirelessEndpointForgettable();  // argc: 2, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsUsingDHCP();  // argc: 1, index: 43
}