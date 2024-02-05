//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientRemoteClientManager
{
    // WARNING: Arguments are unknown!
    public unknown_ret SetUIReadyForStream();  // argc: 1, index: 1, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StreamingAudioPreparationComplete();  // argc: 1, index: 2, ipc args: [bytes1], ipc returns: []
    public unknown_ret StreamingAudioFinished();  // argc: 0, index: 3, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamAvailable();  // argc: 2, index: 4, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamShutdown();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateStreamClientResolution();  // argc: 3, index: 6, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamClientConnected();  // argc: 11, index: 7, ipc args: [bytes4, bytes_length_from_reg, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetStreamClientPlayer();  // argc: 2, index: 8, ipc args: [bytes4], ipc returns: [bytes36]
    // WARNING: Arguments are unknown!
    public unknown_ret GetStreamClientFormFactor();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateStreamClientNetworkUtilization();  // argc: 3, index: 10, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessStreamClientDisconnected();  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool BGetStreamTransportSignal(uint unk, CUtlBuffer* data);  // argc: 2, index: 12, ipc args: [bytes4], ipc returns: [boolean, utlbuffer]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SendStreamTransportSignal();  // argc: 2, index: 13, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ConnectToRemote();  // argc: 2, index: 14, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ConnectToRemoteAddress();  // argc: 1, index: 15, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RefreshRemoteClients();  // argc: 1, index: 16, ipc args: [bytes1], ipc returns: []
    public unknown_ret GetClientPlatformTypes();  // argc: 0, index: 17, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetRemoteClientCount();  // argc: 0, index: 18, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientIDByIndex();  // argc: 1, index: 19, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientNameByIndex();  // argc: 1, index: 20, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientConnectStateByIndex();  // argc: 1, index: 21, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingSupportedByIndex();  // argc: 1, index: 22, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingEnabledByIndex();  // argc: 1, index: 23, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientAppStateByIndex();  // argc: 2, index: 24, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public unknown_ret GetRemoteClientConnectedCount();  // argc: 0, index: 25, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetRemoteClientStreamingEnabledCount();  // argc: 0, index: 26, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientName();  // argc: 2, index: 27, ipc args: [bytes8], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientStreaming();  // argc: 2, index: 28, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientStreamingSession();  // argc: 2, index: 29, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientFormFactor();  // argc: 2, index: 30, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientConnectState();  // argc: 2, index: 31, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingSupported();  // argc: 2, index: 32, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteClientHasStreamingEnabled();  // argc: 2, index: 33, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientAppAvailability();  // argc: 3, index: 34, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientAppState();  // argc: 3, index: 35, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    public unknown_ret GetRemoteDeviceCount();  // argc: 0, index: 36, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceIDByIndex();  // argc: 1, index: 37, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceNameByIndex();  // argc: 1, index: 38, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceName();  // argc: 2, index: 39, ipc args: [bytes8], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret BRemoteDeviceStreaming();  // argc: 2, index: 40, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceStreamingSession();  // argc: 2, index: 41, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteDeviceFormFactor();  // argc: 2, index: 42, ipc args: [bytes8], ipc returns: [bytes4]
    public unknown_ret UnpairRemoteDevices();  // argc: 0, index: 43, ipc args: [], ipc returns: []
    public unknown_ret BIsStreamingSupported();  // argc: 0, index: 44, ipc args: [], ipc returns: [boolean]
    public unknown_ret BIsStreamingEnabled();  // argc: 0, index: 45, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingEnabled();  // argc: 1, index: 46, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartStream();  // argc: 7, index: 47, ipc args: [bytes8, bytes4, bytes4, bytes4, bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsRemoteLaunch();  // argc: 1, index: 48, ipc args: [bytes8], ipc returns: [boolean]
    public unknown_ret BIsBigPictureActiveForStreaming();  // argc: 0, index: 49, ipc args: [], ipc returns: [boolean]
    public unknown_ret BIsStreamingSessionActive();  // argc: 0, index: 50, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamingSessionActiveForGame();  // argc: 1, index: 51, ipc args: [bytes8], ipc returns: [boolean]
    public unknown_ret BIsStreamingClientConnected();  // argc: 0, index: 52, ipc args: [], ipc returns: [boolean]
    public unknown_ret BStreamingClientWantsRecentGames();  // argc: 0, index: 53, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret StopStreamingSession();  // argc: 1, index: 54, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchAppProgress();  // argc: 5, index: 55, ipc args: [bytes4, string, string, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchAppResult();  // argc: 2, index: 56, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamStartInProgress();  // argc: 3, index: 57, ipc args: [bytes8, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchAppResultRequestLaunchOption();  // argc: 3, index: 58, ipc args: [bytes4, bytes4, bytes_length_from_reg], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AcceptEULA();  // argc: 5, index: 59, ipc args: [bytes8, bytes4, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteClientPlatformName();  // argc: 3, index: 60, ipc args: [bytes8], ipc returns: [string, bytes1]
    public unknown_ret BIsStreamClientRunning();  // argc: 0, index: 61, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamClientRunningConnectedToClient();  // argc: 3, index: 62, ipc args: [bytes8, bytes8], ipc returns: [boolean]
    public unknown_ret BIsStreamClientRemotePlayTogether();  // argc: 0, index: 63, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetStreamClientRemoteSteamVersion();  // argc: 0, index: 64, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public bool BGetStreamingClientConfig(CUtlBuffer* data);  // argc: 1, index: 65, ipc args: [], ipc returns: [boolean, utlbuffer]
    // WARNING: Arguments are unknown!
    public unknown_ret BSetStreamingClientConfig();  // argc: 1, index: 66, ipc args: [unknown], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BQueueControllerConfigMessageForRemote();  // argc: 1, index: 67, ipc args: [protobuf], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetControllerConfigMessageForLocal();  // argc: 1, index: 68, ipc args: [], ipc returns: [boolean, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestControllerConfig();  // argc: 4, index: 69, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret PostControllerConfig();  // argc: 4, index: 70, ipc args: [bytes8, bytes4, bytes_length_from_mem], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerConfig();  // argc: 4, index: 71, ipc args: [bytes8, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRemoteDeviceAuthorized();  // argc: 2, index: 72, ipc args: [bytes1, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingDriversInstalled();  // argc: 1, index: 73, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingPIN();  // argc: 1, index: 74, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetStreamingPINSize();  // argc: 1, index: 75, ipc args: [bytes4], ipc returns: []
    public unknown_ret UsedVideoX264();  // argc: 0, index: 76, ipc args: [], ipc returns: []
    public unknown_ret UsedVideoH264();  // argc: 0, index: 77, ipc args: [], ipc returns: []
    public unknown_ret UsedVideoHEVC();  // argc: 0, index: 78, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetRemotePlayTogetherQualityOverride();  // argc: 1, index: 79, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetRemotePlayTogetherBitrateOverride();  // argc: 1, index: 80, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BHasRemotePlayInviteAndSession(in RemotePlayPlayer_t player);  // argc: 9, index: 81, ipc args: [bytes_length_from_reg], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BCreateRemotePlayGroup();  // argc: 1, index: 82, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BCreateRemotePlayInviteAndSession(in RemotePlayPlayer_t player, AppId_t appid);  // argc: 10, index: 83, ipc args: [bytes_length_from_reg, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret CancelRemotePlayInviteAndSession(in RemotePlayPlayer_t player);  // argc: 9, index: 84, ipc args: [bytes_length_from_reg], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret JoinRemotePlaySession();  // argc: 3, index: 85, ipc args: [uint64, string], ipc returns: []
    public bool BStreamingDesktopToRemotePlayTogetherEnabled();  // argc: 0, index: 86, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetStreamingDesktopToRemotePlayTogetherEnabled(bool enabled);  // argc: 1, index: 87, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetStreamingSessionForRemotePlayer(in RemotePlayPlayer_t player);  // argc: 9, index: 88, ipc args: [bytes_length_from_reg], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPerUserKeyboardInputEnabled(in RemotePlayPlayer_t player, bool enabled);  // argc: 10, index: 89, ipc args: [bytes_length_from_reg, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPerUserMouseInputEnabled(in RemotePlayPlayer_t player, bool enabled);  // argc: 10, index: 90, ipc args: [bytes_length_from_reg, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPerUserControllerInputEnabled(in RemotePlayPlayer_t player, bool enabled);  // argc: 10, index: 91, ipc args: [bytes_length_from_reg, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetPerUserInputSettings();  // argc: 10, index: 92, ipc args: [bytes_length_from_reg, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetClientInputSettings();  // argc: 10, index: 93, ipc args: [bytes_length_from_reg, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret OnClientUsedInput();  // argc: 10, index: 94, ipc args: [bytes_length_from_reg, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OnPlaceholderStateChanged();  // argc: 1, index: 95, ipc args: [bytes1], ipc returns: []
    public unknown_ret OnRemoteClientRemotePlayClearControllers();  // argc: 0, index: 96, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OnRemoteClientRemotePlayControllerIndexSet();  // argc: 11, index: 97, ipc args: [bytes_length_from_reg, bytes4, bytes4], ipc returns: []
    public unknown_ret UpdateRemotePlayTogetherGroup();  // argc: 0, index: 98, ipc args: [], ipc returns: []
    public unknown_ret DisbandRemotePlayTogetherGroup();  // argc: 0, index: 99, ipc args: [], ipc returns: []
    public unknown_ret OnRemotePlayUIMovedController();  // argc: 0, index: 100, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OnSendRemotePlayTogetherInvite();  // argc: 3, index: 101, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCloudGameTimeRemaining();  // argc: 3, index: 102, ipc args: [bytes8, bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ShutdownStreamClients();  // argc: 1, index: 103, ipc args: [bytes1], ipc returns: []
}