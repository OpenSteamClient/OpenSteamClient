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

#ifndef ICLIENTUTILS_H
#define ICLIENTUTILS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "UtilsCommon.h"

typedef enum EBrowserType
{
	k_EBrowserTypeOffScreen = 0,
	k_EBrowserTypeOpenVROverlay = 1,
	k_EBrowserTypeOpenVROverlay_Dashboard = 2,
	k_EBrowserTypeDirectHWND = 3,
	k_EBrowserTypeDirectHWND_Borderless = 4,
	k_EBrowserTypeDirectHWND_Hidden = 5,
	k_EBrowserTypeChildHWNDNative = 6,
	k_EBrowserTypeTransparent_Toplevel = 7,
	k_EBrowserTypeOffScreen_SharedTexture = 8,
	k_EBrowserTypeOffScreen_GameOverlay = 9,
	k_EBrowserTypeOffScreen_GameOverlay_SharedTexture = 10,
	k_EBrowserTypeOffscreen_FriendsUI = 11,
	k_EBrowserTypeMAX = 12,
} EBrowserType;

typedef enum EClientUINotificationType
{
	k_EClientUINotificationTypeGroupChatMessage = 1,
	k_EClientUINotificationTypeFriendChatMessage = 2,
	k_EClientUINotificationTypeFriendPersonaState = 3,
} EClientUINotificationType;

typedef enum ETextFilteringContext
{
	k_ETextFilteringContextUnknown = 0,
	k_ETextFilteringContextGameContent = 1,
	k_ETextFilteringContextChat = 2,
	k_ETextFilteringContextName = 3,
} ETextFilteringContext;

abstract_class IClientUtils
{
public:
	virtual void GetInstallPath() = 0; //args: 0, index: 0
    virtual void GetUserBaseFolderInstallImage() = 0; //args: 0, index: 1
    virtual void GetManagedContentRoot() = 0; //args: 0, index: 2
    virtual void GetSecondsSinceAppActive() = 0; //args: 0, index: 3
    virtual void GetSecondsSinceComputerActive() = 0; //args: 0, index: 4
    virtual void SetComputerActive() = 0; //args: 0, index: 5
    virtual void GetConnectedUniverse() = 0; //args: 0, index: 6
    virtual void GetSteamRealm() = 0; //args: 0, index: 7
    virtual void GetServerRealTime() = 0; //args: 0, index: 8
    virtual void GetIPCountry() = 0; //args: 0, index: 9
    virtual void GetImageSize() = 0; //args: 3, index: 10
    virtual void GetImageRGBA() = 0; //args: 3, index: 11
    virtual void GetNumRunningApps() = 0; //args: 0, index: 12
    virtual void GetCurrentBatteryPower() = 0; //args: 0, index: 13
    virtual void GetBatteryInformation() = 0; //args: 2, index: 14
    virtual void SetOfflineMode(bool) = 0; //args: 1, index: 15
    virtual bool GetOfflineMode() = 0; //args: 0, index: 16
    virtual void SetAppIDForCurrentPipe(AppId_t) = 0; //args: 2, index: 17
    virtual AppId_t GetAppID() = 0; //args: 0, index: 18
    virtual void SetAPIDebuggingActive(bool, bool) = 0; //args: 2, index: 19
    virtual void AllocPendingAPICallHandle() = 0; //args: 0, index: 20
    virtual void IsAPICallCompleted() = 0; //args: 3, index: 21
    virtual void GetAPICallFailureReason() = 0; //args: 2, index: 22
    virtual void GetAPICallResult() = 0; //args: 6, index: 23
    virtual void SetAPICallResultWithoutPostingCallback() = 0; //args: 5, index: 24
    virtual void SignalAppsToShutDown() = 0; //args: 0, index: 25
    virtual void SignalServiceAppsToDisconnect() = 0; //args: 0, index: 26
    virtual void TerminateAllApps() = 0; //args: 1, index: 27
    virtual uint32 GetCellID() = 0; //args: 0, index: 28
    virtual bool BIsGlobalInstance() = 0; //args: 0, index: 29
    virtual void CheckFileSignature() = 0; //args: 1, index: 30
    virtual uint32 GetBuildID() = 0; //args: 0, index: 31
    virtual void SetCurrentUIMode(EUIMode) = 0; //args: 1, index: 32
    virtual EUIMode GetCurrentUIMode() = 0; //args: 0, index: 33
    virtual void BIsWebBasedUIMode() = 0; //args: 0, index: 34
    virtual void ShutdownLauncher() = 0; //args: 2, index: 35
    virtual void SetLauncherType(ELauncherType) = 0; //args: 1, index: 36
    virtual ELauncherType GetLauncherType() = 0; //args: 0, index: 37
    virtual void ShowGamepadTextInput() = 0; //args: 5, index: 38
    virtual void GetEnteredGamepadTextLength() = 0; //args: 0, index: 39
    virtual void GetEnteredGamepadTextInput() = 0; //args: 2, index: 40
    virtual void GamepadTextInputClosed() = 0; //args: 3, index: 41
    virtual void ShowControllerLayoutPreview() = 0; //args: 3, index: 42
    virtual void SetSpew(int, int, int) = 0; //args: 3, index: 43
    virtual bool BDownloadsDisabled() = 0; //args: 0, index: 44
    virtual void SetFocusedWindow() = 0; //args: 4, index: 45
    virtual void GetSteamUILanguage() = 0; //args: 0, index: 46
    virtual void CheckSteamReachable() = 0; //args: 0, index: 47
    virtual void SetLastGameLaunchMethod() = 0; //args: 1, index: 48
    virtual void SetVideoAdapterInfo() = 0; //args: 7, index: 49
    virtual void SetOverlayWindowFocusForPipe() = 0; //args: 3, index: 50
    virtual void GetGameOverlayUIInstanceFocusGameID() = 0; //args: 2, index: 51
    virtual void SetControllerConfigFileForAppID() = 0; //args: 2, index: 52
    virtual void GetControllerConfigFileForAppID() = 0; //args: 3, index: 53
    virtual void IsSteamRunningInVR() = 0; //args: 0, index: 54
    virtual void BIsRunningOnAlienwareAlpha() = 0; //args: 0, index: 55
    virtual void StartVRDashboard() = 0; //args: 0, index: 56
    virtual void IsVRHeadsetStreamingEnabled() = 0; //args: 1, index: 57
    virtual void SetVRHeadsetStreamingEnabled() = 0; //args: 2, index: 58
    virtual void GenerateSupportSystemReport() = 0; //args: 0, index: 59
    virtual void GetSupportSystemReport() = 0; //args: 4, index: 60
    virtual void GetAppIdForPid() = 0; //args: 2, index: 61
    virtual void SetClientUIProcess() = 0; //args: 0, index: 62
    virtual void BIsClientUIInForeground() = 0; //args: 0, index: 63
    virtual void SetOverlayChatBrowserInfo() = 0; //args: 5, index: 64
    virtual void ClearOverlayChatBrowserInfo() = 0; //args: 1, index: 65
    virtual void GetOverlayChatBrowserInfo() = 0; //args: 3, index: 66
    virtual void SetOverlayBrowserInfo() = 0; //args: 7, index: 67
    virtual void ClearOverlayBrowserInfo() = 0; //args: 1, index: 68
    virtual void GetOverlayBrowserInfo() = 0; //args: 3, index: 69
    virtual void SetOverlayNotificationPosition() = 0; //args: 2, index: 70
    virtual void SetOverlayNotificationInset() = 0; //args: 3, index: 71
    virtual void DispatchClientUINotification() = 0; //args: 3, index: 72
    virtual void RespondToClientUINotification() = 0; //args: 3, index: 73
    virtual void DispatchClientUICommand() = 0; //args: 2, index: 74
    virtual void DispatchComputerActiveStateChange() = 0; //args: 0, index: 75
    virtual void DispatchOpenURLInClient() = 0; //args: 3, index: 76
    virtual void UpdateWideVineCDM() = 0; //args: 1, index: 77
    virtual void DispatchClearAllBrowsingData() = 0; //args: 0, index: 78
    virtual void DispatchClientSettingsChanged() = 0; //args: 0, index: 79
    virtual void DispatchClientPostMessage() = 0; //args: 3, index: 80
    virtual void IsSteamChina() = 0; //args: 0, index: 81
    virtual void NeedsSteamChinaWorkshop() = 0; //args: 1, index: 82
    virtual void InitFilterText() = 0; //args: 2, index: 83
    virtual void FilterText() = 0; //args: 7, index: 84
    virtual void GetIPv6ConnectivityState() = 0; //args: 1, index: 85
    virtual void ScheduleConnectivityTest() = 0; //args: 2, index: 86
    virtual void GetConnectivityTestState() = 0; //args: 1, index: 87
    virtual void GetCaptivePortalURL() = 0; //args: 0, index: 88
    virtual void RecordSteamInterfaceCreation() = 0; //args: 2, index: 89
    virtual void StartRuntimeInformationGathering() = 0; //args: 0, index: 90
    virtual void GetRuntimeInformation() = 0; //args: 0, index: 91
    virtual void GetCloudGamingPlatform() = 0; //args: 0, index: 92
    virtual void BGetMacAddresses() = 0; //args: 3, index: 93
    virtual void BGetDiskSerialNumber() = 0; //args: 2, index: 94
    virtual void GetSteamEnvironmentForApp() = 0; //args: 3, index: 95
    virtual void TestHTTP() = 0; //args: 1, index: 96
    virtual void DumpJobs() = 0; //args: 1, index: 97
    virtual void ShowFloatingGamepadTextInput() = 0; //args: 6, index: 98
    virtual void DismissFloatingGamepadTextInput() = 0; //args: 1, index: 99
    virtual void FloatingGamepadTextInputDismissed() = 0; //args: 0, index: 100
    virtual void SetGameLauncherMode() = 0; //args: 2, index: 101
    virtual void ClearAllHTTPCaches() = 0; //args: 0, index: 102
    virtual void GetFocusedGameID() = 0; //args: 1, index: 103
    virtual void GetFocusedWindowPID() = 0; //args: 0, index: 104
    virtual void RecordFakeReactRouteMetric() = 0; //args: 1, index: 105
};

#endif // ICLIENTUTILS_H
