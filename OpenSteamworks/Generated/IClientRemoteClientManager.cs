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

public unsafe interface IClientRemoteClientManager
{
    // WARNING: Arguments are unknown!
    public unknown_ret SetUIReadyForStream();  // argc: 1, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret StreamingAudioPreparationComplete();  // argc: 1, index: 2
    public unknown_ret StreamingAudioFinished();  // argc: 0, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamAvailable();  // argc: 2, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamShutdown();  // argc: 1, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateStreamClientResolution();  // argc: 3, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamClientConnected();  // argc: 11, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret GetStreamClientPlayer();  // argc: 2, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret GetStreamClientFormFactor();  // argc: 1, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateStreamClientNetworkUtilization();  // argc: 3, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamClientDisconnected();  // argc: 1, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret BGetStreamTransportSignal();  // argc: 2, index: 12
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SendStreamTransportSignal();  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret ConnectToRemote();  // argc: 2, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret ConnectToRemoteAddress();  // argc: 1, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret RefreshRemoteClients();  // argc: 1, index: 16
    public unknown_ret GetClientPlatformTypes();  // argc: 0, index: 17
    public unknown_ret GetRemoteClientCount();  // argc: 0, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientIDByIndex();  // argc: 1, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientNameByIndex();  // argc: 1, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientConnectStateByIndex();  // argc: 1, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingSupportedByIndex();  // argc: 1, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingEnabledByIndex();  // argc: 1, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientAppStateByIndex();  // argc: 2, index: 24
    public unknown_ret GetRemoteClientConnectedCount();  // argc: 0, index: 25
    public unknown_ret GetRemoteClientStreamingEnabledCount();  // argc: 0, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientName();  // argc: 2, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientStreaming();  // argc: 2, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientStreamingSession();  // argc: 2, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientFormFactor();  // argc: 2, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientConnectState();  // argc: 2, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingSupported();  // argc: 2, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingEnabled();  // argc: 2, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientAppAvailability();  // argc: 3, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientAppState();  // argc: 3, index: 35
    public unknown_ret GetRemoteDeviceCount();  // argc: 0, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceIDByIndex();  // argc: 1, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceNameByIndex();  // argc: 1, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceName();  // argc: 2, index: 39
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteDeviceStreaming();  // argc: 2, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceStreamingSession();  // argc: 2, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceFormFactor();  // argc: 2, index: 42
    public unknown_ret UnpairRemoteDevices();  // argc: 0, index: 43
    public unknown_ret BIsStreamingSupported();  // argc: 0, index: 44
    public unknown_ret BIsStreamingEnabled();  // argc: 0, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingEnabled();  // argc: 1, index: 46
    // WARNING: Arguments are unknown!
    public unknown_ret StartStream();  // argc: 7, index: 47
    // WARNING: Arguments are unknown!
    public unknown_ret BIsRemoteLaunch();  // argc: 1, index: 48
    public unknown_ret BIsBigPictureActiveForStreaming();  // argc: 0, index: 49
    public unknown_ret BIsStreamingSessionActive();  // argc: 0, index: 50
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamingSessionActiveForGame();  // argc: 1, index: 51
    public unknown_ret BIsStreamingClientConnected();  // argc: 0, index: 52
    public unknown_ret BStreamingClientWantsRecentGames();  // argc: 0, index: 53
    // WARNING: Arguments are unknown!
    public unknown_ret StopStreamingSession();  // argc: 1, index: 54
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchAppProgress();  // argc: 5, index: 55
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchAppResult();  // argc: 2, index: 56
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamStartInProgress();  // argc: 3, index: 57
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchAppResultRequestLaunchOption();  // argc: 3, index: 58
    // WARNING: Arguments are unknown!
    public unknown_ret AcceptEULA();  // argc: 5, index: 59
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientPlatformName();  // argc: 3, index: 60
    public unknown_ret BIsStreamClientRunning();  // argc: 0, index: 61
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamClientRunningConnectedToClient();  // argc: 3, index: 62
    public unknown_ret BIsStreamClientRemotePlayTogether();  // argc: 0, index: 63
    public unknown_ret GetStreamClientRemoteSteamVersion();  // argc: 0, index: 64
    // WARNING: Arguments are unknown!
    public unknown_ret BGetStreamingClientConfig();  // argc: 1, index: 65
    // WARNING: Arguments are unknown!
    public unknown_ret BSetStreamingClientConfig();  // argc: 1, index: 66
    // WARNING: Arguments are unknown!
    public unknown_ret BQueueControllerConfigMessageForRemote();  // argc: 1, index: 67
    // WARNING: Arguments are unknown!
    public unknown_ret BGetControllerConfigMessageForLocal();  // argc: 1, index: 68
    // WARNING: Arguments are unknown!
    public unknown_ret RequestControllerConfig();  // argc: 4, index: 69
    // WARNING: Arguments are unknown!
    public unknown_ret PostControllerConfig();  // argc: 4, index: 70
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerConfig();  // argc: 4, index: 71
    // WARNING: Arguments are unknown!
    public unknown_ret SetRemoteDeviceAuthorized();  // argc: 2, index: 72
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingDriversInstalled();  // argc: 1, index: 73
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingPIN();  // argc: 1, index: 74
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetStreamingPINSize();  // argc: 1, index: 75
    public unknown_ret UsedVideoX264();  // argc: 0, index: 76
    public unknown_ret UsedVideoH264();  // argc: 0, index: 77
    public unknown_ret UsedVideoHEVC();  // argc: 0, index: 78
    // WARNING: Arguments are unknown!
    public unknown_ret SetRemotePlayTogetherQualityOverride();  // argc: 1, index: 79
    // WARNING: Arguments are unknown!
    public unknown_ret SetRemotePlayTogetherBitrateOverride();  // argc: 1, index: 80
    // WARNING: Arguments are unknown!
    public unknown_ret BHasRemotePlayInviteAndSession();  // argc: 9, index: 81
    // WARNING: Arguments are unknown!
    public unknown_ret BCreateRemotePlayGroup();  // argc: 1, index: 82
    // WARNING: Arguments are unknown!
    public bool BCreateRemotePlayInviteAndSession(in RemotePlayPlayer_t player, AppId_t appid);  // argc: 10, index: 83
    // WARNING: Arguments are unknown!
    public unknown_ret CancelRemotePlayInviteAndSession(in RemotePlayPlayer_t player);  // argc: 9, index: 84
    // WARNING: Arguments are unknown!
    public unknown_ret JoinRemotePlaySession();  // argc: 3, index: 85
    public unknown_ret BStreamingDesktopToRemotePlayTogetherEnabled();  // argc: 0, index: 86
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingDesktopToRemotePlayTogetherEnabled();  // argc: 1, index: 87
    // WARNING: Arguments are unknown!
    public unknown_ret GetStreamingSessionForRemotePlayer();  // argc: 9, index: 88
    // WARNING: Arguments are unknown!
    public unknown_ret SetPerUserKeyboardInputEnabled();  // argc: 10, index: 89
    // WARNING: Arguments are unknown!
    public unknown_ret SetPerUserMouseInputEnabled();  // argc: 10, index: 90
    // WARNING: Arguments are unknown!
    public unknown_ret SetPerUserControllerInputEnabled();  // argc: 10, index: 91
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetPerUserInputSettings();  // argc: 10, index: 92
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetClientInputSettings();  // argc: 10, index: 93
    // WARNING: Arguments are unknown!
    public unknown_ret OnClientUsedInput();  // argc: 10, index: 94
    // WARNING: Arguments are unknown!
    public unknown_ret OnPlaceholderStateChanged();  // argc: 1, index: 95
    public unknown_ret OnRemoteClientRemotePlayClearControllers();  // argc: 0, index: 96
    // WARNING: Arguments are unknown!
    public unknown_ret OnRemoteClientRemotePlayControllerIndexSet();  // argc: 11, index: 97
    public unknown_ret UpdateRemotePlayTogetherGroup();  // argc: 0, index: 98
    public unknown_ret DisbandRemotePlayTogetherGroup();  // argc: 0, index: 99
    public unknown_ret OnRemotePlayUIMovedController();  // argc: 0, index: 100
    // WARNING: Arguments are unknown!
    public unknown_ret OnSendRemotePlayTogetherInvite();  // argc: 3, index: 101
    // WARNING: Arguments are unknown!
    public unknown_ret GetCloudGameTimeRemaining();  // argc: 3, index: 102
    // WARNING: Arguments are unknown!
    public unknown_ret ShutdownStreamClients();  // argc: 1, index: 103
}