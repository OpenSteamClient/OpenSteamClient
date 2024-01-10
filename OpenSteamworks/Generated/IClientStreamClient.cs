//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientStreamClient
{
    // WARNING: Arguments are unknown!
    public unknown_ret Launched();  // argc: 1, index: 1, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FocusGained();  // argc: 2, index: 2, ipc args: [bytes8, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FocusLost();  // argc: 1, index: 3, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret Finished();  // argc: 2, index: 4, ipc args: [bytes8, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BGetStreamingClientConfig();  // argc: 1, index: 5, ipc args: [], ipc returns: [boolean, utlbuffer]
    // WARNING: Arguments are unknown!
    public unknown_ret BSaveStreamingClientConfig();  // argc: 1, index: 6, ipc args: [unknown], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetQualityOverride();  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetBitrateOverride();  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: []
    public unknown_ret ShowOnScreenKeyboard();  // argc: 0, index: 9, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BQueueControllerConfigMessageForLocal();  // argc: 1, index: 10, ipc args: [protobuf], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetControllerConfigMessageForRemote();  // argc: 1, index: 11, ipc args: [], ipc returns: [boolean, protobuf]
    public unknown_ret GetSystemInfo();  // argc: 0, index: 12, ipc args: [], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret StartStreamingSession();  // argc: 1, index: 13, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ReportStreamingSessionEvent();  // argc: 2, index: 14, ipc args: [bytes8, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FinishStreamingSession();  // argc: 3, index: 15, ipc args: [bytes8, string, string], ipc returns: []
}