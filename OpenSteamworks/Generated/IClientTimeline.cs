//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//
//=============================================================================

using System;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

// Note: These simply post callbacks for the UI to use. steamclient.so does not include clipping functionality, it is up to the UI to do so.
public unsafe interface IClientTimeline
{
    // WARNING: Arguments are unknown!
    public void SetTimelineStateDescription(string description, uint unk1);  // argc: 2, index: 1, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public void ClearTimelineStateDescription(uint unk);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public void AddTimelineEvent(string unk, string unk1, string unk2, uint unk3, uint unk4, uint unk5, float duration);  // argc: 7, index: 3, ipc args: [string, string, string, bytes4, bytes4, bytes4, bytes4], ipc returns: []
    public void SetTimelineGameMode(ETimelineGameMode mode);  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public void AddUserMarkerForGame(in CGameID gameid);  // argc: 1, index: 5, ipc args: [bytes8], ipc returns: []
    public void ToggleVideoRecordingForGame(in CGameID gameid);  // argc: 1, index: 6, ipc args: [bytes8], ipc returns: []
}