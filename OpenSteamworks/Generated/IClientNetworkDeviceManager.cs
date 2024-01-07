//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Protobuf;

namespace OpenSteamworks.Generated;

public unsafe interface IClientNetworkDeviceManager
{
    public unknown_ret IsInterfaceValid();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public unknown_ret RefreshDevices();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    /// <summary>
    /// Takes a pointer to CMsgNetworkDevicesData protobuf object
    /// </summary>
    /// <param name="protoptr"></param>
    /// <returns></returns>
    public unknown_ret GetNetworkDevicesData([ProtobufPtrType(typeof(CMsgNetworkDevicesData))] IntPtr nativeptr);  // argc: 1, index: 0, ipc args: [], ipc returns: [bytes1, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret ConnectToDevice();  // argc: 1, index: 1, ipc args: [protobuf], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DisconnectFromDevice();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetWifiEnabled();  // argc: 1, index: 3, ipc args: [bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetWifiScanningEnabled();  // argc: 1, index: 4, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ForgetWirelessEndpoint();  // argc: 2, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetWirelessEndpointAutoconnect();  // argc: 3, index: 6, ipc args: [bytes4, bytes4, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCustomIPSettings();  // argc: 6, index: 7, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetCustomIPSettings();  // argc: 6, index: 8, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetProxyInfo();  // argc: 4, index: 9, ipc args: [bytes4, string, bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetProxyInfo();  // argc: 5, index: 10, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public unknown_ret GetObviousConnectivityProblem();  // argc: 0, index: 11, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TEST_SetFakeLocalSystemStateSetting();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    public unknown_ret TEST_GetFakeLocalSystemStateSetting();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    public unknown_ret TEST_GetFakeLocalSystemEffectiveState();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TEST_SetEmulateSingleWirelessDevice();  // argc: 1, index: 0, ipc args: [bytes1], ipc returns: []
    public unknown_ret TEST_GetEmulateSingleWirelessDevice();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_EnumerateNetworkDevices();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceType();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsCurrentDevice();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsCurrentlyConnected();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceIP4();  // argc: 3, index: 4, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceBroadcastIP4();  // argc: 3, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceIPV6InterfaceIndex();  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceVendor();  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceProduct();  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetMacAddress();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetSubnetMaskBitCount();  // argc: 3, index: 10, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetRouterAddressIP4();  // argc: 3, index: 11, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDNSResolversIP4();  // argc: 3, index: 12, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDeviceState();  // argc: 1, index: 13, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetDevicePluggedState();  // argc: 1, index: 14, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_EnumerateWirelessEndpoints();  // argc: 3, index: 15, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetConnectedWirelessEndpointSSID();  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetWirelessSecurityCapabilities();  // argc: 1, index: 17, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetWirelessEndpointSSIDUserDisplayString();  // argc: 2, index: 18, ipc args: [bytes4, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetWirelessEndpointStrength();  // argc: 2, index: 19, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsSecurityRequired();  // argc: 2, index: 20, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_GetCachedWirelessCredentials();  // argc: 2, index: 21, ipc args: [bytes4, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsWirelessEndpointForgettable();  // argc: 2, index: 22, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LEGACY_IsUsingDHCP();  // argc: 1, index: 23, ipc args: [bytes4], ipc returns: [bytes1]
}