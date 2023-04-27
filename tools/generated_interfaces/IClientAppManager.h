#ifndef ICLIENTAPPMANAGER_H
#define ICLIENTAPPMANAGER_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class UNSAFE_INTERFACE IClientAppManager
{
public:
     virtual void InstallApp() = 0; //args: 3, index: 0
     virtual void UninstallApp() = 0; //args: 1, index: 1
     virtual void LaunchApp() = 0; //args: 4, index: 2
     virtual void ShutdownApp() = 0; //args: 2, index: 3
     virtual void GetAppInstallState() = 0; //args: 1, index: 4
     virtual void GetAppInstallDir() = 0; //args: 3, index: 5
     virtual void GetAppContentInfo() = 0; //args: 6, index: 6
     virtual void GetAppStagingInfo() = 0; //args: 3, index: 7
     virtual void IsAppDlcInstalled() = 0; //args: 2, index: 8
     virtual void GetDlcDownloadProgress() = 0; //args: 4, index: 9
     virtual void BIsDlcEnabled() = 0; //args: 3, index: 10
     virtual void SetDlcEnabled() = 0; //args: 3, index: 11
     virtual void SetDlcContext() = 0; //args: 2, index: 12
     virtual void GetDlcSizes() = 0; //args: 4, index: 13
     virtual void GetNumInstalledApps() = 0; //args: 0, index: 14
     virtual void GetInstalledApps() = 0; //args: 2, index: 15
     virtual void BIsWaitingForInstalledApps() = 0; //args: 0, index: 16
     virtual void GetAppDependencies() = 0; //args: 3, index: 17
     virtual void GetDependentApps() = 0; //args: 3, index: 18
     virtual void GetUpdateInfo() = 0; //args: 2, index: 19
     virtual void GetAppConfigValue() = 0; //args: 4, index: 20
     virtual void SetAppConfigValue() = 0; //args: 3, index: 21
     virtual void BIsAppUpToDate() = 0; //args: 1, index: 22
     virtual void GetAvailableLanguages() = 0; //args: 4, index: 23
     virtual void GetCurrentLanguage() = 0; //args: 3, index: 24
     virtual void GetCurrentLanguage() = 0; //args: 1, index: 25
     virtual void GetFallbackLanguage() = 0; //args: 2, index: 26
     virtual void SetCurrentLanguage() = 0; //args: 2, index: 27
     virtual void StartValidatingApp() = 0; //args: 1, index: 28
     virtual void CancelValidation() = 0; //args: 1, index: 29
     virtual void MarkContentCorrupt() = 0; //args: 2, index: 30
     virtual void GetInstalledDepots() = 0; //args: 3, index: 31
     virtual void GetFileDetails() = 0; //args: 2, index: 32
     virtual void VerifySignedFiles() = 0; //args: 1, index: 33
     virtual void GetAvailableBetas() = 0; //args: 5, index: 34
     virtual void CheckBetaPassword() = 0; //args: 2, index: 35
     virtual void BHasCachedBetaPassword() = 0; //args: 2, index: 36
     virtual void GetActiveBeta() = 0; //args: 3, index: 37
     virtual void BGetActiveBetaForApps() = 0; //args: 2, index: 38
     virtual void SetDownloadingEnabled() = 0; //args: 1, index: 39
     virtual void BIsDownloadingEnabled() = 0; //args: 0, index: 40
     virtual void GetDownloadStats() = 0; //args: 1, index: 41
     virtual void GetDownloadingAppID() = 0; //args: 0, index: 42
     virtual void GetAutoUpdateTimeRestrictionEnabled() = 0; //args: 0, index: 43
     virtual void SetAutoUpdateTimeRestrictionEnabled() = 0; //args: 1, index: 44
     virtual void GetAutoUpdateTimeRestrictionHours() = 0; //args: 2, index: 45
     virtual void SetAutoUpdateTimeRestrictionStartHour() = 0; //args: 1, index: 46
     virtual void SetAutoUpdateTimeRestrictionEndHour() = 0; //args: 1, index: 47
     virtual void GetAppAutoUpdateBehavior() = 0; //args: 1, index: 48
     virtual void SetAppAutoUpdateBehavior() = 0; //args: 2, index: 49
     virtual void SetAppAllowDownloadsWhileRunningBehavior() = 0; //args: 2, index: 50
     virtual void GetAppAllowDownloadsWhileRunningBehavior() = 0; //args: 1, index: 51
     virtual void SetAllowDownloadsWhileAnyAppRunning() = 0; //args: 1, index: 52
     virtual void BAllowDownloadsWhileAnyAppRunning() = 0; //args: 0, index: 53
     virtual void ChangeAppDownloadQueuePlacement() = 0; //args: 2, index: 54
     virtual void SetAppDownloadQueueIndex() = 0; //args: 2, index: 55
     virtual void GetAppDownloadQueueIndex() = 0; //args: 1, index: 56
     virtual void GetAppAutoUpdateDelayedUntilTime() = 0; //args: 1, index: 57
     virtual void GetNumAppsInDownloadQueue() = 0; //args: 0, index: 58
     virtual void BHasLocalContentServer() = 0; //args: 0, index: 59
     virtual void BuildBackup() = 0; //args: 4, index: 60
     virtual void BuildInstaller() = 0; //args: 4, index: 61
     virtual void CancelBackup() = 0; //args: 0, index: 62
     virtual void RestoreAppFromBackup() = 0; //args: 2, index: 63
     virtual void RecoverAppFromFolder() = 0; //args: 2, index: 64
     virtual void CanMoveApp() = 0; //args: 2, index: 65
     virtual void MoveApp() = 0; //args: 2, index: 66
     virtual void GetMoveAppProgress() = 0; //args: 4, index: 67
     virtual void CancelMoveApp() = 0; //args: 1, index: 68
     virtual void GetAppStateInfo() = 0; //args: 2, index: 69
     virtual void BGetAppStateInfoForApps() = 0; //args: 2, index: 70
     virtual void BIsAvailableOnPlatform() = 0; //args: 2, index: 71
     virtual void BCanRemotePlayTogether() = 0; //args: 1, index: 72
     virtual void BIsLocalMultiplayerApp() = 0; //args: 1, index: 73
     virtual void GetNumLibraryFolders() = 0; //args: 0, index: 74
     virtual void GetLibraryFolderPath() = 0; //args: 3, index: 75
     virtual void AddLibraryFolder() = 0; //args: 1, index: 76
     virtual void SetLibraryFolderLabel() = 0; //args: 2, index: 77
     virtual void GetLibraryFolderLabel() = 0; //args: 3, index: 78
     virtual void RemoveLibraryFolder() = 0; //args: 3, index: 79
     virtual void BGetLibraryFolderInfo() = 0; //args: 4, index: 80
     virtual void GetAppLibraryFolder() = 0; //args: 1, index: 81
     virtual void RefreshLibraryFolders() = 0; //args: 0, index: 82
     virtual void GetNumAppsInFolder() = 0; //args: 1, index: 83
     virtual void GetAppsInFolder() = 0; //args: 3, index: 84
     virtual void ForceInstallDirOverride() = 0; //args: 1, index: 85
     virtual void SetDownloadThrottleRateKbps() = 0; //args: 2, index: 86
     virtual void GetDownloadThrottleRateKbps() = 0; //args: 1, index: 87
     virtual void SuspendDownloadThrottling() = 0; //args: 1, index: 88
     virtual void SetThrottleDownloadsWhileStreaming() = 0; //args: 1, index: 89
     virtual void BThrottleDownloadsWhileStreaming() = 0; //args: 0, index: 90
     virtual void GetLaunchQueryParam() = 0; //args: 2, index: 91
     virtual void BeginLaunchQueryParams() = 0; //args: 1, index: 92
     virtual void SetLaunchQueryParam() = 0; //args: 3, index: 93
     virtual void CommitLaunchQueryParams() = 0; //args: 2, index: 94
     virtual void GetLaunchCommandLine() = 0; //args: 3, index: 95
     virtual void AddContentLogLine() = 0; //args: 1, index: 96
     virtual void GetSystemIconFile() = 0; //args: 4, index: 97
     virtual void SetUseHTTPSForDownloads() = 0; //args: 1, index: 98
     virtual void GetUseHTTPSForDownloads() = 0; //args: 0, index: 99
     virtual void SetPeerContentServerMode() = 0; //args: 1, index: 100
     virtual void SetPeerContentClientMode() = 0; //args: 1, index: 101
     virtual void GetPeerContentServerMode() = 0; //args: 0, index: 102
     virtual void GetPeerContentClientMode() = 0; //args: 0, index: 103
     virtual void GetPeerContentServerStats() = 0; //args: 1, index: 104
     virtual void SuspendPeerContentClient() = 0; //args: 1, index: 105
     virtual void SuspendPeerContentServer() = 0; //args: 1, index: 106
};
#endif // ICLIENTAPPMANAGER_H
