//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code, comments, and all related files, projects, resources, 
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo, 
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef ICLIENTVIDEO_H
#define ICLIENTVIDEO_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientVideo
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetUserVideoFolder() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UnlockH264() = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EGetBroadcastReady() = 0; //argc: 0, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BeginBroadcastSession() = 0; //argc: 0, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EndBroadcastSession() = 0; //argc: 0, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IsBroadcasting() = 0; //argc: 1, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsUploadingThumbnails() = 0; //argc: 0, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastSessionID() = 0; //argc: 0, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ReceiveBroadcastChat() = 0; //argc: 5, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PostBroadcastChat() = 0; //argc: 3, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret MuteBroadcastChatUser() = 0; //argc: 4, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InitBroadcastVideo() = 0; //argc: 8, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InitBroadcastAudio() = 0; //argc: 7, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_13_DONTUSE() = 0; //argc: -1, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UploadBroadcastThumbnail() = 0; //argc: 4, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DroppedVideoFrames() = 0; //argc: 1, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetCurrentVideoEncodingRate() = 0; //argc: 1, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetMicrophoneState() = 0; //argc: 2, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetVideoSource() = 0; //argc: 1, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BroadcastRecorderError() = 0; //argc: 1, index 20
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LoadBroadcastSettings() = 0; //argc: 0, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetBroadcastPermissions() = 0; //argc: 1, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastPermissions() = 0; //argc: 0, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastMaxKbps() = 0; //argc: 0, index 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastDelaySeconds() = 0; //argc: 0, index 25
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetBroadcastDimensions() = 0; //argc: 2, index 26
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastIncludeDesktop() = 0; //argc: 0, index 27
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastRecordSystemAudio() = 0; //argc: 0, index 28
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastRecordMic() = 0; //argc: 0, index 29
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastShowChatCorner() = 0; //argc: 0, index 30
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastShowDebugInfo() = 0; //argc: 0, index 31
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastShowReminderBanner() = 0; //argc: 0, index 32
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetBroadcastEncoderSetting() = 0; //argc: 0, index 33
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InviteToBroadcast() = 0; //argc: 4, index 34
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IgnoreApprovalRequest() = 0; //argc: 3, index 35
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BroadcastFirstTimeComplete() = 0; //argc: 0, index 36
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetInHomeStreamState() = 0; //argc: 1, index 37
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret WatchBroadcast() = 0; //argc: 2, index 38
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetWatchBroadcastMPD() = 0; //argc: 2, index 39
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetApprovalRequestCount() = 0; //argc: 0, index 40
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetApprovalRequests() = 0; //argc: 2, index 41
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddBroadcastGameData() = 0; //argc: 2, index 42
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveBroadcastGameData() = 0; //argc: 1, index 43
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddTimelineMarker() = 0; //argc: 5, index 44
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveTimelineMarker() = 0; //argc: 0, index 45
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddRegion() = 0; //argc: 4, index 46
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveRegion() = 0; //argc: 1, index 47
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetVideoURL() = 0; //argc: 1, index 48
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetOPFSettings() = 0; //argc: 1, index 49
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetOPFStringForApp() = 0; //argc: 4, index 50
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret WebRTCGetTURNAddress() = 0; //argc: 1, index 51
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret WebRTCStartResult() = 0; //argc: 4, index 52
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret WebRTCAddCandidate() = 0; //argc: 5, index 53
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret WebRTCGetAnswer() = 0; //argc: 3, index 54
};

#endif // ICLIENTVIDEO_H