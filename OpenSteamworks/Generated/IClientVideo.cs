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
    public unknown_ret UnlockH264();  // argc: 2, index: 2
    public unknown_ret EGetBroadcastReady();  // argc: 0, index: 3
    public unknown_ret BeginBroadcastSession();  // argc: 0, index: 4
    public unknown_ret EndBroadcastSession();  // argc: 0, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret IsBroadcasting();  // argc: 1, index: 6
    public unknown_ret BIsUploadingThumbnails();  // argc: 0, index: 7
    public unknown_ret GetBroadcastSessionID();  // argc: 0, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret ReceiveBroadcastChat();  // argc: 5, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret PostBroadcastChat();  // argc: 3, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret MuteBroadcastChatUser();  // argc: 4, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret InitBroadcastVideo();  // argc: 8, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret InitBroadcastAudio();  // argc: 7, index: 13
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_12_DONTUSE();  // argc: -1, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret UploadBroadcastThumbnail();  // argc: 4, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret DroppedVideoFrames();  // argc: 1, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret SetCurrentVideoEncodingRate();  // argc: 1, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret SetMicrophoneState();  // argc: 2, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret SetVideoSource();  // argc: 1, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret BroadcastRecorderError();  // argc: 1, index: 20
    public unknown_ret LoadBroadcastSettings();  // argc: 0, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret SetBroadcastPermissions();  // argc: 1, index: 22
    public unknown_ret GetBroadcastPermissions();  // argc: 0, index: 23
    public unknown_ret GetBroadcastMaxKbps();  // argc: 0, index: 24
    public unknown_ret GetBroadcastDelaySeconds();  // argc: 0, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret BGetBroadcastDimensions();  // argc: 2, index: 26
    public unknown_ret GetBroadcastIncludeDesktop();  // argc: 0, index: 27
    public unknown_ret GetBroadcastRecordSystemAudio();  // argc: 0, index: 28
    public unknown_ret GetBroadcastRecordMic();  // argc: 0, index: 29
    public unknown_ret GetBroadcastShowChatCorner();  // argc: 0, index: 30
    public unknown_ret GetBroadcastShowDebugInfo();  // argc: 0, index: 31
    public unknown_ret GetBroadcastShowReminderBanner();  // argc: 0, index: 32
    public unknown_ret GetBroadcastEncoderSetting();  // argc: 0, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret InviteToBroadcast();  // argc: 4, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret IgnoreApprovalRequest();  // argc: 3, index: 35
    public unknown_ret BroadcastFirstTimeComplete();  // argc: 0, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret SetInHomeStreamState();  // argc: 1, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret WatchBroadcast();  // argc: 2, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret GetWatchBroadcastMPD();  // argc: 2, index: 39
    public unknown_ret GetApprovalRequestCount();  // argc: 0, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret GetApprovalRequests();  // argc: 2, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret AddBroadcastGameData();  // argc: 2, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveBroadcastGameData();  // argc: 1, index: 43
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineMarker();  // argc: 5, index: 44
    public unknown_ret RemoveTimelineMarker();  // argc: 0, index: 45
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret AddRegion();  // argc: 4, index: 46
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveRegion();  // argc: 1, index: 47
    // WARNING: Arguments are unknown!
    public unknown_ret GetVideoURL();  // argc: 1, index: 48
    // WARNING: Arguments are unknown!
    public unknown_ret GetOPFSettings();  // argc: 1, index: 49
    // WARNING: Arguments are unknown!
    public unknown_ret GetOPFStringForApp();  // argc: 4, index: 50
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret WebRTCGetTURNAddress();  // argc: 1, index: 51
    // WARNING: Arguments are unknown!
    public unknown_ret WebRTCStartResult();  // argc: 4, index: 52
    // WARNING: Arguments are unknown!
    public unknown_ret WebRTCAddCandidate();  // argc: 5, index: 53
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret WebRTCGetAnswer();  // argc: 3, index: 54
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineHighlightMarker();  // argc: 4, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineTimestamp();  // argc: 1, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineRangeStart();  // argc: 2, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret AddTimelineRangeEnd();  // argc: 1, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimelineGameMode();  // argc: 1, index: 13
}