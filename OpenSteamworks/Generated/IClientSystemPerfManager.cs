//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientSystemPerfManager
{
    public unknown_ret IsInterfaceValid();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDiagnosticInfo();  // argc: 1, index: 2, ipc args: [], ipc returns: [bytes1, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret GetState();  // argc: 1, index: 3, ipc args: [], ipc returns: [bytes1, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateSettings();  // argc: 1, index: 4, ipc args: [protobuf], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRefreshRateExternallyManaged();  // argc: 1, index: 5, ipc args: [bytes1], ipc returns: []
}