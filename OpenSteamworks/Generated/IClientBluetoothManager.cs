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
    public unknown_ret IsInterfaceValid();  // argc: 0, index: 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetBluetoothDevicesData();  // argc: 1, index: 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetEnabled();  // argc: 1, index: 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetDiscovering();  // argc: 2, index: 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret Pair();  // argc: 2, index: 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret CancelPairing();  // argc: 2, index: 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UnPair();  // argc: 2, index: 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret Connect();  // argc: 2, index: 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret Disconnect();  // argc: 2, index: 9
}