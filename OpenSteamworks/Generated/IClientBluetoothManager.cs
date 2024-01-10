//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientBluetoothManager
{
    public unknown_ret IsInterfaceValid();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBluetoothDevicesData();  // argc: 1, index: 2, ipc args: [], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetEnabled();  // argc: 1, index: 3, ipc args: [bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetDiscovering();  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret Pair();  // argc: 2, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CancelPairing();  // argc: 2, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UnPair();  // argc: 2, index: 7, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret Connect();  // argc: 2, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret Disconnect();  // argc: 2, index: 9, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
}