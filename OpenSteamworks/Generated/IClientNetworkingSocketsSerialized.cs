//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientNetworkingSocketsSerialized
{
    // WARNING: Arguments are unknown!
    public void SendP2PRendezvous();  // argc: 5, index: 1, ipc args: [uint64, bytes4, bytes4, bytes_length_from_mem], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SendP2PConnectionFailureLegacy();  // argc: 5, index: 2, ipc args: [uint64, bytes4, bytes4, string], ipc returns: []
    public unknown_ret GetCertAsync();  // argc: 0, index: 3, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CacheRelayTicket();  // argc: 2, index: 4, ipc args: [bytes4, bytes_length_from_mem], ipc returns: []
    public unknown_ret GetCachedRelayTicketCount();  // argc: 0, index: 5, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCachedRelayTicket();  // argc: 3, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSTUNServer();  // argc: 3, index: 7, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret AllowDirectConnectToPeerString(string peerString);  // argc: 1, index: 8, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BeginAsyncRequestFakeIP();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret AllowDirectConnectToPeerString(string peerString, out uint unk);  // argc: 1, index: 10, ipc args: [string], ipc returns: [unknown, bytes4]
    // WARNING: Arguments are unknown!
    public void SetAllowShareIPUserSetting();  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetAllowShareIPUserSetting();  // argc: 0, index: 12, ipc args: [], ipc returns: [bytes4]
    public void TEST_ClearInMemoryCachedCredentials();  // argc: 0, index: 13, ipc args: [], ipc returns: []
}