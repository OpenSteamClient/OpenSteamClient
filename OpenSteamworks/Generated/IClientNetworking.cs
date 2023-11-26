//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientNetworking
{
    // WARNING: Arguments are unknown!
    public unknown_ret SendP2PPacket();  // argc: 6, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret IsP2PPacketAvailable();  // argc: 2, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret ReadP2PPacket();  // argc: 5, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret AcceptP2PSessionWithUser();  // argc: 2, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret CloseP2PSessionWithUser();  // argc: 2, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret CloseP2PChannelWithUser();  // argc: 3, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret GetP2PSessionState();  // argc: 3, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret AllowP2PPacketRelay();  // argc: 1, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret CreateListenSocket();  // argc: 8, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret CreateP2PConnectionSocket();  // argc: 5, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret CreateConnectionSocket();  // argc: 7, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret DestroySocket();  // argc: 2, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret DestroyListenSocket();  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret SendDataOnSocket();  // argc: 4, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret IsDataAvailableOnSocket();  // argc: 2, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret RetrieveDataFromSocket();  // argc: 4, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret IsDataAvailable();  // argc: 3, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret RetrieveData();  // argc: 5, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret GetSocketInfo();  // argc: 5, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret GetListenSocketInfo();  // argc: 3, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret GetSocketConnectionType();  // argc: 1, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret GetMaxPacketSize();  // argc: 1, index: 22
}