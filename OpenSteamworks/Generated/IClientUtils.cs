//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;
using OpenSteamworks.Enums;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUtils
{
    public string GetInstallPath();  // argc: 0, index: 1
    public string GetUserBaseFolderInstallImage();  // argc: 0, index: 2
    public string GetUserBaseFolderPersistentStorage();  // argc: 0, index: 0
    public string GetManagedContentRoot();  // argc: 0, index: 3
    public RTime32 GetSecondsSinceAppActive();  // argc: 0, index: 4
    public RTime32 GetSecondsSinceComputerActive();  // argc: 0, index: 5
    public void SetComputerActive();  // argc: 0, index: 6
    public EUniverse GetConnectedUniverse();  // argc: 0, index: 7
    public ESteamRealm GetSteamRealm();  // argc: 0, index: 8
    public RTime32 GetServerRealTime();  // argc: 0, index: 9
    public unknown_ret GetIPCountry();  // argc: 0, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret GetImageSize();  // argc: 3, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetImageRGBA();  // argc: 3, index: 12
    public unknown_ret GetNumRunningApps();  // argc: 0, index: 13
    public unknown_ret GetCurrentBatteryPower();  // argc: 0, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret GetBatteryInformation();  // argc: 2, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret SetOfflineMode(bool offline);  // argc: 1, index: 16
    public bool GetOfflineMode();  // argc: 0, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret SetAppIDForCurrentPipe(AppId_t appid);  // argc: 2, index: 18
    public AppId_t GetAppID();  // argc: 0, index: 19
    public void SetAPIDebuggingActive(bool active, bool verbose);  // argc: 2, index: 20
    public SteamAPICall_t AllocPendingAPICallHandle();  // argc: 0, index: 21
    public bool IsAPICallCompleted(SteamAPICall_t handle, ref bool failed);  // argc: 3, index: 22
    public ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t handle);  // argc: 2, index: 23
    /// <summary>
    /// Gets a result for an api call.
    /// </summary>
    public bool GetAPICallResult(SteamAPICall_t handle, void* callbackData, int callbackDataMax, int expectedCallbackID, out bool failed);  // argc: 6, index: 24
    public unknown_ret SetAPICallResultWithoutPostingCallback(SteamAPICall_t handle, byte[] responseData, int responseDataLen, int responseCallbackID);  // argc: 5, index: 25
    public unknown_ret SignalAppsToShutDown();  // argc: 0, index: 26
    public unknown_ret SignalServiceAppsToDisconnect();  // argc: 0, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret TerminateAllApps();  // argc: 1, index: 28
    public uint GetCellID();  // argc: 0, index: 29
    public bool BIsGlobalInstance();  // argc: 0, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret CheckFileSignature();  // argc: 1, index: 31
    public unknown_ret GetBuildID();  // argc: 0, index: 32
    public void SetCurrentUIMode(EUIMode mode);  // argc: 1, index: 33
    public EUIMode GetCurrentUIMode();  // argc: 0, index: 34
    public bool BIsWebBasedUIMode();  // argc: 0, index: 35
    public void SetDisableOverlayScaling(bool val);  // argc: 1, index: 0
    // WARNING: Arguments are unknown!
    public unknown_ret ShutdownLauncher();  // argc: 2, index: 36
    // WARNING: Arguments are unknown!
    public void SetLauncherType(ELauncherType type);  // argc: 1, index: 37
    public ELauncherType GetLauncherType();  // argc: 0, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret ShowGamepadTextInput();  // argc: 5, index: 39
    public unknown_ret GetEnteredGamepadTextLength();  // argc: 0, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret GetEnteredGamepadTextInput();  // argc: 2, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret GamepadTextInputClosed();  // argc: 3, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret ShowControllerLayoutPreview();  // argc: 3, index: 43
    // WARNING: Arguments are unknown!
    public void SetSpew(ESpewGroup group, int spewlevel, int loglevel);  // argc: 3, index: 44
    public bool BDownloadsDisabled();  // argc: 0, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret SetFocusedWindow();  // argc: 4, index: 46
    public unknown_ret GetSteamUILanguage();  // argc: 0, index: 47
    public SteamAPICall_t CheckSteamReachable();  // argc: 0, index: 48
    /// <summary>
    /// TODO: What does this do?
    /// </summary>
    /// <param name="launchmethod"></param>
    public void SetLastGameLaunchMethod(int launchmethod);  // argc: 1, index: 49
    // WARNING: Arguments are unknown!
    public unknown_ret SetVideoAdapterInfo();  // argc: 7, index: 50
    // WARNING: Arguments are unknown!
    public unknown_ret SetOverlayWindowFocusForPipe();  // argc: 3, index: 51
    // WARNING: Arguments are unknown!
    public unknown_ret GetGameOverlayUIInstanceFocusGameID();  // argc: 2, index: 52
    // WARNING: Arguments are unknown!
    public unknown_ret GetFocusedGameWindow();  // argc: 2, index: 53
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerConfigFileForAppID();  // argc: 2, index: 54
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerConfigFileForAppID();  // argc: 3, index: 55
    public bool IsSteamRunningInVR();  // argc: 0, index: 56
    public unknown_ret StartVRDashboard();  // argc: 0, index: 58
    // WARNING: Arguments are unknown!
    public unknown_ret IsVRHeadsetStreamingEnabled();  // argc: 1, index: 59
    // WARNING: Arguments are unknown!
    public unknown_ret SetVRHeadsetStreamingEnabled();  // argc: 2, index: 60
    public unknown_ret GenerateSupportSystemReport();  // argc: 0, index: 61
    // WARNING: Arguments are unknown!
    public unknown_ret GetSupportSystemReport();  // argc: 4, index: 62
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppIdForPid(AppId_t appid, bool unk);  // argc: 2, index: 63
    public unknown_ret SetClientUIProcess();  // argc: 0, index: 64
    public unknown_ret BIsClientUIInForeground();  // argc: 0, index: 65
    // WARNING: Arguments are unknown!
    public unknown_ret AllowSetForegroundThroughWebhelper(bool val);  // argc: 1, index: 66
    // WARNING: Arguments are unknown!
    public unknown_ret SetOverlayBrowserInfo();  // argc: 8, index: 67
    // WARNING: Arguments are unknown!
    public unknown_ret ClearOverlayBrowserInfo();  // argc: 1, index: 68
    // WARNING: Arguments are unknown!
    public unknown_ret GetOverlayBrowserInfo();  // argc: 3, index: 69
    // WARNING: Arguments are unknown!
    public unknown_ret SetOverlayNotificationPosition();  // argc: 2, index: 70
    // WARNING: Arguments are unknown!
    public unknown_ret SetOverlayNotificationInset();  // argc: 3, index: 71
    // WARNING: Arguments are unknown!
    public unknown_ret DispatchClientUINotification();  // argc: 3, index: 72
    // WARNING: Arguments are unknown!
    public unknown_ret RespondToClientUINotification();  // argc: 3, index: 73
    // WARNING: Arguments are unknown!
    public unknown_ret DispatchClientUICommand();  // argc: 2, index: 74
    public unknown_ret DispatchComputerActiveStateChange();  // argc: 0, index: 75
    // WARNING: Arguments are unknown!
    public unknown_ret DispatchOpenURLInClient();  // argc: 3, index: 76
    // WARNING: Arguments are unknown!
    public SteamAPICall_t UpdateWideVineCDM(string maybePath);  // argc: 1, index: 77
    public unknown_ret DispatchClearAllBrowsingData();  // argc: 0, index: 78
    public unknown_ret DispatchClientSettingsChanged();  // argc: 0, index: 79
    // WARNING: Arguments are unknown!
    public unknown_ret DispatchClientPostMessage();  // argc: 3, index: 80
    public unknown_ret IsSteamChina();  // argc: 0, index: 81
    // WARNING: Arguments are unknown!
    public bool NeedsSteamChinaWorkshop(AppId_t app);  // argc: 1, index: 82
    public bool InitFilterText(AppId_t appid, uint filterOptions = 0);  // argc: 2, index: 83
    public int FilterText(AppId_t appid, ETextFilteringContext context, CSteamID senderSteamID, string msg, StringBuilder msgOut, int maxMsgOut);  // argc: 7, index: 84
    // WARNING: Arguments are unknown!
    public unknown_ret GetIPv6ConnectivityState(ESteamIPv6ConnectivityProtocol protocol);  // argc: 1, index: 85
    // WARNING: Arguments are unknown!
    public unknown_ret ScheduleConnectivityTest();  // argc: 2, index: 86
    // WARNING: Arguments are unknown!
    public unknown_ret GetConnectivityTestState();  // argc: 1, index: 87
    public string GetCaptivePortalURL();  // argc: 0, index: 88
    // WARNING: Arguments are unknown!
    public unknown_ret RecordSteamInterfaceCreation(string unk, string unk1);  // argc: 2, index: 89
    public ECloudGamingPlatform GetCloudGamingPlatform();  // argc: 0, index: 90
    // WARNING: Arguments are unknown!
    public unknown_ret BGetMacAddresses();  // argc: 3, index: 91
    // WARNING: Arguments are unknown!
    public unknown_ret BGetDiskSerialNumber(StringBuilder builder, int maxOut);  // argc: 2, index: 92
    // WARNING: Arguments are unknown!
    public unknown_ret GetSteamEnvironmentForApp(AppId_t appid, StringBuilder buf, int bufMax);  // argc: 3, index: 93
    // WARNING: Arguments are unknown!
    public unknown_ret TestHTTP(string unk);  // argc: 1, index: 94
    // WARNING: Arguments are unknown!
    public unknown_ret DumpJobs(string unk);  // argc: 1, index: 95
    // WARNING: Arguments are unknown!
    public unknown_ret ShowFloatingGamepadTextInput();  // argc: 6, index: 96
    // WARNING: Arguments are unknown!
    public unknown_ret DismissFloatingGamepadTextInput();  // argc: 1, index: 97
    public unknown_ret FloatingGamepadTextInputDismissed();  // argc: 0, index: 98
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameLauncherMode();  // argc: 2, index: 99
    public unknown_ret ClearAllHTTPCaches();  // argc: 0, index: 100
    // WARNING: Arguments are unknown!
    public unknown_ret GetFocusedGameID();  // argc: 1, index: 101
    public uint GetFocusedWindowPID();  // argc: 0, index: 102
    // WARNING: Arguments are unknown!
    public unknown_ret SetWebUITransportWebhelperPID(uint pid);  // argc: 1, index: 103
    // WARNING: Arguments are unknown!
    public unknown_ret GetWebUITransportInfo();  // argc: 1, index: 104
    // WARNING: Arguments are unknown!
    public unknown_ret RecordFakeReactRouteMetric();  // argc: 1, index: 105
    // WARNING: Arguments are unknown!
    public unknown_ret SteamRuntimeSystemInfo();  // argc: 1, index: 106
    // WARNING: Arguments are unknown!
    public unknown_ret DumpHTTPClients();  // argc: 1, index: 107
    // WARNING: Arguments are unknown!
    public unknown_ret BGetMachineID();  // argc: 1, index: 108
    public unknown_ret NotifyMissingInterface(string interfaceName);  // argc: 1, index: 6
}