//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientVideo
{
    // WARNING: Arguments are unknown!
    public unknown_ret UnlockH264();  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: []
    public unknown_ret EGetBroadcastReady();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes4]
    public unknown_ret BeginBroadcastSession();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    public unknown_ret EndBroadcastSession();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsBroadcasting();  // argc: 1, index: 0, ipc args: [], ipc returns: [boolean, bytes4]
    public unknown_ret BIsUploadingThumbnails();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetBroadcastSessionID();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ReceiveBroadcastChat();  // argc: 5, index: 0, ipc args: [uint64, bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret PostBroadcastChat();  // argc: 3, index: 1, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret MuteBroadcastChatUser();  // argc: 4, index: 2, ipc args: [bytes8, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret InitBroadcastVideo();  // argc: 8, index: 3, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes_length_from_mem, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret InitBroadcastAudio();  // argc: 7, index: 4, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_12_DONTUSE();  // argc: -1, index: 5, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UploadBroadcastThumbnail();  // argc: 4, index: 6, ipc args: [bytes4, bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret DroppedVideoFrames();  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetCurrentVideoEncodingRate();  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetMicrophoneState();  // argc: 2, index: 9, ipc args: [bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVideoSource();  // argc: 1, index: 10, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BroadcastRecorderError();  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: []
    public unknown_ret LoadBroadcastSettings();  // argc: 0, index: 12, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetBroadcastPermissions();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetBroadcastPermissions();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetBroadcastMaxKbps();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetBroadcastDelaySeconds();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetBroadcastDimensions();  // argc: 2, index: 0, ipc args: [], ipc returns: [boolean, bytes4, bytes4]
    public unknown_ret GetBroadcastIncludeDesktop();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastRecordSystemAudio();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastRecordMic();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastShowChatCorner();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetBroadcastShowDebugInfo();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastShowReminderBanner();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastEncoderSetting();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InviteToBroadcast();  // argc: 4, index: 0, ipc args: [uint64, bytes1, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IgnoreApprovalRequest();  // argc: 3, index: 1, ipc args: [uint64, bytes4], ipc returns: []
    public unknown_ret BroadcastFirstTimeComplete();  // argc: 0, index: 2, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetInHomeStreamState();  // argc: 1, index: 0, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WatchBroadcast();  // argc: 2, index: 1, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetWatchBroadcastMPD();  // argc: 2, index: 2, ipc args: [uint64], ipc returns: [string]
    public unknown_ret GetApprovalRequestCount();  // argc: 0, index: 3, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetApprovalRequests();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret AddBroadcastGameData();  // argc: 2, index: 1, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveBroadcastGameData();  // argc: 1, index: 2, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineMarker();  // argc: 5, index: 3, ipc args: [string, bytes1, bytes1, bytes1, bytes1], ipc returns: []
    public unknown_ret RemoveTimelineMarker();  // argc: 0, index: 4, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret AddRegion();  // argc: 4, index: 0, ipc args: [string, string, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveRegion();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetVideoURL();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetOPFSettings();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetOPFStringForApp();  // argc: 4, index: 4, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret WebRTCGetTURNAddress();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WebRTCStartResult();  // argc: 4, index: 6, ipc args: [bytes8, bytes1, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WebRTCAddCandidate();  // argc: 5, index: 7, ipc args: [bytes8, string, bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret WebRTCGetAnswer();  // argc: 3, index: 8, ipc args: [bytes8, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineHighlightMarker();  // argc: 5, index: 9, ipc args: [string, string, string, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineTimestamp();  // argc: 2, index: 10, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineRange();  // argc: 7, index: 11, ipc args: [string, string, string, bytes4, bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimelineGameMode();  // argc: 1, index: 12, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddUserMarkerForGame();  // argc: 1, index: 13, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ToggleVideoRecordingForGame();  // argc: 1, index: 14, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CreateVideoManagerClip();  // argc: 23, index: 15, ipc args: [bytes_length_from_reg, bytes28, bytes20], ipc returns: [bytes8]
}