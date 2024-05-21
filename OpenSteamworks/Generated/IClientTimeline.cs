//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientTimeline
{
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimelineStateDescription();  // argc: 2, index: 1, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ClearTimelineStateDescription();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineEvent();  // argc: 7, index: 3, ipc args: [string, string, string, bytes4, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimelineGameMode();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddUserMarkerForGame();  // argc: 1, index: 5, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ToggleVideoRecordingForGame();  // argc: 1, index: 6, ipc args: [bytes8], ipc returns: []
}