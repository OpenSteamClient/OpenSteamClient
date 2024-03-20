//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientVideo
{
    // WARNING: Arguments are unknown!
    public unknown_ret UnlockH264();  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: []
    public unknown_ret EGetBroadcastReady();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes4]
    public unknown_ret BeginBroadcastSession();  // argc: 0, index: 3, ipc args: [], ipc returns: []
    public unknown_ret EndBroadcastSession();  // argc: 0, index: 4, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsBroadcasting();  // argc: 1, index: 5, ipc args: [], ipc returns: [boolean, bytes4]
    public unknown_ret BIsUploadingThumbnails();  // argc: 0, index: 6, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetBroadcastSessionID();  // argc: 0, index: 7, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ReceiveBroadcastChat();  // argc: 5, index: 8, ipc args: [uint64, bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret PostBroadcastChat();  // argc: 3, index: 9, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret MuteBroadcastChatUser();  // argc: 4, index: 10, ipc args: [bytes8, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret InitBroadcastVideo();  // argc: 8, index: 11, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes_length_from_mem, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret InitBroadcastAudio();  // argc: 7, index: 12, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_12_DONTUSE();  // argc: -1, index: 13, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UploadBroadcastThumbnail();  // argc: 4, index: 14, ipc args: [bytes4, bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret DroppedVideoFrames();  // argc: 1, index: 15, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetCurrentVideoEncodingRate();  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetMicrophoneState();  // argc: 2, index: 17, ipc args: [bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVideoSource();  // argc: 1, index: 18, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BroadcastRecorderError();  // argc: 1, index: 19, ipc args: [bytes4], ipc returns: []
    public unknown_ret LoadBroadcastSettings();  // argc: 0, index: 20, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetBroadcastPermissions();  // argc: 1, index: 21, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetBroadcastPermissions();  // argc: 0, index: 22, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetBroadcastMaxKbps();  // argc: 0, index: 23, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetBroadcastDelaySeconds();  // argc: 0, index: 24, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetBroadcastDimensions();  // argc: 2, index: 25, ipc args: [], ipc returns: [boolean, bytes4, bytes4]
    public unknown_ret GetBroadcastIncludeDesktop();  // argc: 0, index: 26, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastRecordSystemAudio();  // argc: 0, index: 27, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastRecordMic();  // argc: 0, index: 28, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastShowChatCorner();  // argc: 0, index: 29, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetBroadcastShowDebugInfo();  // argc: 0, index: 30, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastShowReminderBanner();  // argc: 0, index: 31, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetBroadcastEncoderSetting();  // argc: 0, index: 32, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InviteToBroadcast();  // argc: 4, index: 33, ipc args: [uint64, bytes1, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IgnoreApprovalRequest();  // argc: 3, index: 34, ipc args: [uint64, bytes4], ipc returns: []
    public unknown_ret BroadcastFirstTimeComplete();  // argc: 0, index: 35, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetInHomeStreamState();  // argc: 1, index: 36, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WatchBroadcast();  // argc: 2, index: 37, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetWatchBroadcastMPD();  // argc: 2, index: 38, ipc args: [uint64], ipc returns: [string]
    public unknown_ret GetApprovalRequestCount();  // argc: 0, index: 39, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetApprovalRequests();  // argc: 2, index: 40, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret AddBroadcastGameData();  // argc: 2, index: 41, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveBroadcastGameData();  // argc: 1, index: 42, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineMarker();  // argc: 5, index: 43, ipc args: [string, bytes1, bytes1, bytes1, bytes1], ipc returns: []
    public unknown_ret RemoveTimelineMarker();  // argc: 0, index: 44, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret AddRegion();  // argc: 4, index: 45, ipc args: [string, string, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveRegion();  // argc: 1, index: 46, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetVideoURL();  // argc: 1, index: 47, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetOPFSettings();  // argc: 1, index: 48, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetOPFStringForApp();  // argc: 4, index: 49, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret WebRTCGetTURNAddress();  // argc: 1, index: 50, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WebRTCStartResult();  // argc: 4, index: 51, ipc args: [bytes8, bytes1, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WebRTCAddCandidate();  // argc: 5, index: 52, ipc args: [bytes8, string, bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret WebRTCGetAnswer();  // argc: 3, index: 53, ipc args: [bytes8, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimelineStateDescription();  // argc: 2, index: 54, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ClearTimelineStateDescription();  // argc: 1, index: 55, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineEvent();  // argc: 7, index: 56, ipc args: [string, string, string, bytes4, bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimelineGameMode();  // argc: 1, index: 57, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddUserMarkerForGame();  // argc: 1, index: 58, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ToggleVideoRecordingForGame(CGameID gameid);  // argc: 1, index: 59, ipc args: [bytes8], ipc returns: []
}