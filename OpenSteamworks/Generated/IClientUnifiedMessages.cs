//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUnifiedMessages
{
    // WARNING: Arguments are unknown!
    public unknown_ret SendMethod();  // argc: 5, index: 1, ipc args: [string, bytes4, bytes_length_from_mem, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMethodResponseInfo();  // argc: 4, index: 2, ipc args: [bytes8], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMethodResponseData();  // argc: 5, index: 3, ipc args: [bytes8, bytes4, bytes1], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseMethod();  // argc: 2, index: 4, ipc args: [bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SendNotification();  // argc: 3, index: 5, ipc args: [string, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
}