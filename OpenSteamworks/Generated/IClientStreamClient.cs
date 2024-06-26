//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Protobuf;

namespace OpenSteamworks.Generated;

public unsafe interface IClientStreamClient
{
    // WARNING: Arguments are unknown!
    public void Launched(ulong unk);  // argc: 1, index: 1, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public void FocusGained(ulong unk, bool unk2);  // argc: 2, index: 2, ipc args: [bytes8, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public void FocusLost(ulong unk);  // argc: 1, index: 3, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public void Finished(ulong unk, uint unk2);  // argc: 2, index: 4, ipc args: [bytes8, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool BGetStreamingClientConfig(CUtlBuffer* buf);  // argc: 1, index: 5, ipc args: [], ipc returns: [boolean, utlbuffer]
    // WARNING: Arguments are unknown!
    public bool BSaveStreamingClientConfig(CUtlBuffer* buf);  // argc: 1, index: 6, ipc args: [utlbuffer], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public void SetQualityOverride(uint val);  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetBitrateOverride(uint val);  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: []
    public void ShowOnScreenKeyboard();  // argc: 0, index: 9, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public SteamAPICall_t BQueueControllerConfigMessageForLocal([ProtobufPtrType(typeof(CControllerConfigMsg))] IntPtr protoptr);  // argc: 1, index: 10, ipc args: [protobuf], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public bool BGetControllerConfigMessageForRemote([ProtobufPtrType(typeof(CControllerConfigMsg))] IntPtr protoptr);  // argc: 1, index: 11, ipc args: [], ipc returns: [boolean, protobuf]
    public string GetSystemInfo();  // argc: 0, index: 12, ipc args: [], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public void StartStreamingSession(ulong unk);  // argc: 1, index: 13, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public void ReportStreamingSessionEvent(ulong unk, string unk2);  // argc: 2, index: 14, ipc args: [bytes8, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void FinishStreamingSession(ulong unk, string unk2, string unk3);  // argc: 3, index: 15, ipc args: [bytes8, string, string], ipc returns: []
}