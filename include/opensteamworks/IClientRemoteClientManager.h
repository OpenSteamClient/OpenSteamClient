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

#ifndef ICLIENTREMOTECLIENTMANAGER_H
#define ICLIENTREMOTECLIENTMANAGER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientRemoteClientManager
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetUIReadyForStream() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StreamingAudioPreparationComplete() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StreamingAudioFinished() = 0; //argc: 0, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ProcessStreamAvailable() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ProcessStreamShutdown() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateStreamClientResolution() = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ProcessStreamClientConnected() = 0; //argc: 11, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStreamClientPlayer() = 0; //argc: 2, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStreamClientFormFactor() = 0; //argc: 1, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateStreamClientNetworkUtilization() = 0; //argc: 3, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ProcessStreamClientDisconnected() = 0; //argc: 1, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetStreamTransportSignal() = 0; //argc: 2, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SendStreamTransportSignal() = 0; //argc: 2, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ConnectToRemote() = 0; //argc: 2, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ConnectToRemoteAddress() = 0; //argc: 1, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RefreshRemoteClients() = 0; //argc: 1, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetClientPlatformTypes() = 0; //argc: 0, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientCount() = 0; //argc: 0, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientIDByIndex() = 0; //argc: 1, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientNameByIndex() = 0; //argc: 1, index 20
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientConnectStateByIndex() = 0; //argc: 1, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BRemoteClientHasStreamingSupportedByIndex() = 0; //argc: 1, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BRemoteClientHasStreamingEnabledByIndex() = 0; //argc: 1, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientAppStateByIndex() = 0; //argc: 2, index 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientConnectedCount() = 0; //argc: 0, index 25
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientStreamingEnabledCount() = 0; //argc: 0, index 26
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientName() = 0; //argc: 2, index 27
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BRemoteClientStreaming() = 0; //argc: 2, index 28
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientStreamingSession() = 0; //argc: 2, index 29
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientFormFactor() = 0; //argc: 2, index 30
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientConnectState() = 0; //argc: 2, index 31
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BRemoteClientHasStreamingSupported() = 0; //argc: 2, index 32
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BRemoteClientHasStreamingEnabled() = 0; //argc: 2, index 33
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientAppAvailability() = 0; //argc: 3, index 34
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientAppState() = 0; //argc: 3, index 35
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteDeviceCount() = 0; //argc: 0, index 36
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteDeviceIDByIndex() = 0; //argc: 1, index 37
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteDeviceNameByIndex() = 0; //argc: 1, index 38
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteDeviceName() = 0; //argc: 2, index 39
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BRemoteDeviceStreaming() = 0; //argc: 2, index 40
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteDeviceStreamingSession() = 0; //argc: 2, index 41
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteDeviceFormFactor() = 0; //argc: 2, index 42
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UnpairRemoteDevices() = 0; //argc: 0, index 43
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamingSupported() = 0; //argc: 0, index 44
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamingEnabled() = 0; //argc: 0, index 45
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetStreamingEnabled() = 0; //argc: 1, index 46
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StartStream() = 0; //argc: 7, index 47
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsRemoteLaunch() = 0; //argc: 1, index 48
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsBigPictureActiveForStreaming() = 0; //argc: 0, index 49
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamingSessionActive() = 0; //argc: 0, index 50
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamingSessionActiveForGame() = 0; //argc: 1, index 51
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamingClientConnected() = 0; //argc: 0, index 52
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BStreamingClientWantsRecentGames() = 0; //argc: 0, index 53
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StopStreamingSession() = 0; //argc: 1, index 54
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LaunchAppProgress() = 0; //argc: 5, index 55
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LaunchAppResult() = 0; //argc: 2, index 56
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamStartInProgress() = 0; //argc: 3, index 57
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LaunchAppResultRequestLaunchOption() = 0; //argc: 3, index 58
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AcceptEULA() = 0; //argc: 5, index 59
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemoteClientPlatformName() = 0; //argc: 3, index 60
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamClientRunning() = 0; //argc: 0, index 61
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamClientRunningConnectedToClient() = 0; //argc: 3, index 62
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsStreamClientRemotePlayTogether() = 0; //argc: 0, index 63
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStreamClientRemoteSteamVersion() = 0; //argc: 0, index 64
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetStreamingClientConfig() = 0; //argc: 1, index 65
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BSetStreamingClientConfig() = 0; //argc: 1, index 66
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BQueueControllerConfigMessageForRemote() = 0; //argc: 1, index 67
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetControllerConfigMessageForLocal() = 0; //argc: 1, index 68
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RequestControllerConfig() = 0; //argc: 4, index 69
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PostControllerConfig() = 0; //argc: 4, index 70
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetControllerConfig() = 0; //argc: 4, index 71
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetRemoteDeviceAuthorized() = 0; //argc: 2, index 72
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetStreamingDriversInstalled() = 0; //argc: 1, index 73
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetStreamingPIN() = 0; //argc: 1, index 74
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStreamingPINSize() = 0; //argc: 1, index 75
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UsedVideoX264() = 0; //argc: 0, index 76
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UsedVideoH264() = 0; //argc: 0, index 77
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UsedVideoHEVC() = 0; //argc: 0, index 78
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetRemotePlayTogetherQualityOverride() = 0; //argc: 1, index 79
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetRemotePlayTogetherBitrateOverride() = 0; //argc: 1, index 80
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BHasRemotePlayInviteAndSession() = 0; //argc: 9, index 81
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BCreateRemotePlayGroup() = 0; //argc: 1, index 82
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BCreateRemotePlayInviteAndSession() = 0; //argc: 10, index 83
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CancelRemotePlayInviteAndSession() = 0; //argc: 9, index 84
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret JoinRemotePlaySession() = 0; //argc: 3, index 85
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BStreamingDesktopToRemotePlayTogetherEnabled() = 0; //argc: 0, index 86
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetStreamingDesktopToRemotePlayTogetherEnabled() = 0; //argc: 1, index 87
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStreamingSessionForRemotePlayer() = 0; //argc: 9, index 88
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetPerUserKeyboardInputEnabled() = 0; //argc: 10, index 89
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetPerUserMouseInputEnabled() = 0; //argc: 10, index 90
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetPerUserControllerInputEnabled() = 0; //argc: 10, index 91
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPerUserInputSettings() = 0; //argc: 10, index 92
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetClientInputSettings() = 0; //argc: 10, index 93
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret OnClientUsedInput() = 0; //argc: 10, index 94
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret OnPlaceholderStateChanged() = 0; //argc: 1, index 95
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret OnRemoteClientRemotePlayClearControllers() = 0; //argc: 0, index 96
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret OnRemoteClientRemotePlayControllerIndexSet() = 0; //argc: 11, index 97
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateRemotePlayTogetherGroup() = 0; //argc: 0, index 98
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DisbandRemotePlayTogetherGroup() = 0; //argc: 0, index 99
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret OnRemotePlayUIMovedController() = 0; //argc: 0, index 100
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret OnSendRemotePlayTogetherInvite() = 0; //argc: 3, index 101
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetCloudGameTimeRemaining() = 0; //argc: 3, index 102
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ShutdownStreamClients() = 0; //argc: 1, index 103
};

#endif // ICLIENTREMOTECLIENTMANAGER_H