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
    virtual void GetInstallPath() = 0; //argc: 0, index 1
    virtual void GetUserBaseFolderInstallImage() = 0; //argc: 0, index 0
    virtual unknown_ret GetUserBaseFolderPersistentStorage() = 0; //argc: 0, index 0
    virtual void GetManagedContentRoot() = 0; //argc: 0, index 0
    virtual void GetSecondsSinceAppActive() = 0; //argc: 0, index 0
    virtual void GetSecondsSinceComputerActive() = 0; //argc: 0, index 0
    virtual void SetComputerActive() = 0; //argc: 0, index 0
    virtual void GetConnectedUniverse() = 0; //argc: 0, index 0
    virtual void GetSteamRealm() = 0; //argc: 0, index 0
    virtual void GetServerRealTime() = 0; //argc: 0, index 0
    virtual void GetIPCountry() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetImageSize() = 0; //argc: 3, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetImageRGBA() = 0; //argc: 3, index 1
    virtual void GetNumRunningApps() = 0; //argc: 0, index 2
    virtual void GetCurrentBatteryPower() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetBatteryInformation() = 0; //argc: 2, index 0
    virtual void SetOfflineMode(bool) = 0; //argc: 1, index 1
    virtual bool GetOfflineMode() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetAppIDForCurrentPipe(AppId_t) = 0; //argc: 2, index 0
    virtual AppId_t GetAppID() = 0; //argc: 0, index 1
    virtual void SetAPIDebuggingActive(bool, bool) = 0; //argc: 2, index 0
    virtual void AllocPendingAPICallHandle() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void IsAPICallCompleted() = 0; //argc: 3, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAPICallFailureReason() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAPICallResult() = 0; //argc: 6, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetAPICallResultWithoutPostingCallback() = 0; //argc: 5, index 3
    virtual void SignalAppsToShutDown() = 0; //argc: 0, index 4
    virtual void SignalServiceAppsToDisconnect() = 0; //argc: 0, index 0
    virtual void TerminateAllApps() = 0; //argc: 1, index 0
    virtual uint32 GetCellID() = 0; //argc: 0, index 1
    virtual bool BIsGlobalInstance() = 0; //argc: 0, index 0
    virtual void CheckFileSignature() = 0; //argc: 1, index 0
    virtual uint32 GetBuildID() = 0; //argc: 0, index 1
    virtual void SetCurrentUIMode(EUIMode) = 0; //argc: 1, index 0
    virtual EUIMode GetCurrentUIMode() = 0; //argc: 0, index 1
    virtual void BIsWebBasedUIMode() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetDisableOverlayScaling() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ShutdownLauncher() = 0; //argc: 2, index 1
    virtual void SetLauncherType(ELauncherType) = 0; //argc: 1, index 2
    virtual ELauncherType GetLauncherType() = 0; //argc: 0, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ShowGamepadTextInput() = 0; //argc: 5, index 0
    virtual void GetEnteredGamepadTextLength() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEnteredGamepadTextInput() = 0; //argc: 2, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GamepadTextInputClosed() = 0; //argc: 3, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ShowControllerLayoutPreview() = 0; //argc: 3, index 2
    virtual void SetSpew(int, int, int) = 0; //argc: 3, index 3
    virtual bool BDownloadsDisabled() = 0; //argc: 0, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetFocusedWindow() = 0; //argc: 4, index 0
    virtual void GetSteamUILanguage() = 0; //argc: 0, index 1
    virtual void CheckSteamReachable() = 0; //argc: 0, index 0
    virtual void SetLastGameLaunchMethod() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetVideoAdapterInfo() = 0; //argc: 7, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetOverlayWindowFocusForPipe() = 0; //argc: 3, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGameOverlayUIInstanceFocusGameID() = 0; //argc: 3, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetFocusedGameWindow() = 0; //argc: 3, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetControllerConfigFileForAppID() = 0; //argc: 2, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetControllerConfigFileForAppID() = 0; //argc: 3, index 6
    virtual void IsSteamRunningInVR() = 0; //argc: 0, index 7
    virtual void StartVRDashboard() = 0; //argc: 0, index 0
    virtual void IsVRHeadsetStreamingEnabled() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetVRHeadsetStreamingEnabled() = 0; //argc: 2, index 1
    virtual void GenerateSupportSystemReport() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetSupportSystemReport() = 0; //argc: 4, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppIdForPid() = 0; //argc: 2, index 1
    virtual void SetClientUIProcess() = 0; //argc: 0, index 2
    virtual void BIsClientUIInForeground() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AllowSetForegroundThroughWebhelper() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetOverlayBrowserInfo() = 0; //argc: 8, index 1
    virtual void ClearOverlayBrowserInfo() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetOverlayBrowserInfo() = 0; //argc: 3, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetOverlayNotificationPosition() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetOverlayNotificationInset() = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DispatchClientUINotification() = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RespondToClientUINotification() = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DispatchClientUICommand() = 0; //argc: 2, index 8
    virtual void DispatchComputerActiveStateChange() = 0; //argc: 0, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DispatchOpenURLInClient() = 0; //argc: 3, index 0
    virtual void UpdateWideVineCDM() = 0; //argc: 1, index 1
    virtual void DispatchClearAllBrowsingData() = 0; //argc: 0, index 2
    virtual void DispatchClientSettingsChanged() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DispatchClientPostMessage() = 0; //argc: 3, index 0
    virtual void IsSteamChina() = 0; //argc: 0, index 1
    virtual void NeedsSteamChinaWorkshop() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void InitFilterText() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void FilterText() = 0; //argc: 7, index 2
    virtual void GetIPv6ConnectivityState() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ScheduleConnectivityTest() = 0; //argc: 2, index 4
    virtual void GetConnectivityTestState() = 0; //argc: 1, index 5
    virtual void GetCaptivePortalURL() = 0; //argc: 0, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RecordSteamInterfaceCreation() = 0; //argc: 2, index 0
    virtual void GetCloudGamingPlatform() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetMacAddresses() = 0; //argc: 3, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetDiskSerialNumber() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetSteamEnvironmentForApp() = 0; //argc: 3, index 2
    virtual void TestHTTP() = 0; //argc: 1, index 3
    virtual void DumpJobs() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ShowFloatingGamepadTextInput() = 0; //argc: 6, index 5
    virtual void DismissFloatingGamepadTextInput() = 0; //argc: 1, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DismissGamepadTextInput() = 0; //argc: 1, index 7
    virtual void FloatingGamepadTextInputDismissed() = 0; //argc: 0, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetGameLauncherMode() = 0; //argc: 2, index 0
    virtual void ClearAllHTTPCaches() = 0; //argc: 0, index 1
    virtual void GetFocusedGameID() = 0; //argc: 1, index 0
    virtual void GetFocusedWindowPID() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetWebUITransportWebhelperPID() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetWebUITransportInfo() = 0; //argc: 1, index 1
    virtual void RecordFakeReactRouteMetric() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SteamRuntimeSystemInfo() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DumpHTTPClients() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetMachineID() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret NotifyMissingInterface() = 0; //argc: 1, index 6
    virtual unknown_ret IsSteamInTournamentMode() = 0; //argc: 0, index 7
};

#endif // ICLIENTUTILS_H