//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientNetworkingUtilsSerialized
{
    // WARNING: Arguments are unknown!
    public unknown_ret GetNetworkConfigJSON_DEPRECATED();  // argc: 3, index: 1, ipc args: [bytes4, string], ipc returns: [bytes4, bytes_length_from_mem]
    public unknown_ret GetLauncherType();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes4]
    public unknown_ret TEST_ClearCachedNetworkConfig();  // argc: 0, index: 3, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret PostConnectionStateMsg();  // argc: 2, index: 4, ipc args: [bytes4, bytes_length_from_mem], ipc returns: []
    public unknown_ret PostConnectionStateUpdatesForAllConnections();  // argc: 0, index: 5, ipc args: [], ipc returns: []
    public unknown_ret PostAppSummaryUpdates();  // argc: 0, index: 6, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GotLocationString(string str);  // argc: 1, index: 7, ipc args: [string], ipc returns: []
}