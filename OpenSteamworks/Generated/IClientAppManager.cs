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
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientAppManager
{
    public EAppUpdateError InstallApp(AppId_t unAppID, LibraryFolder_t libraryFolder, bool bLegacy);  // argc: 3, index: 1
    public EAppUpdateError UninstallApp(AppId_t unAppID);  // argc: 1, index: 2
    public EAppUpdateError LaunchApp(in CGameID gameID, uint uLaunchOption, ELaunchSource eLaunchSource, unknown_ret unknown);  // argc: 4, index: 3
    public unknown_ret ShutdownApp(AppId_t appId, bool force);  // argc: 2, index: 4
    public EAppState GetAppInstallState(AppId_t appid);  // argc: 1, index: 5
    public unknown_ret GetAppInstallDir(AppId_t appid, StringBuilder path, uint pathMax);  // argc: 3, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppContentInfo();  // argc: 6, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppStagingInfo();  // argc: 3, index: 8
    public bool IsAppDlcInstalled(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 9
    public bool GetDlcDownloadProgress(AppId_t appid, ref UInt64 downloaded, ref UInt64 toDownload);  // argc: 4, index: 10
    public unknown_ret BIsDlcEnabled(AppId_t appid, AppId_t dlcid, ref bool unk);  // argc: 3, index: 11
    public unknown_ret SetDlcEnabled(AppId_t appid, AppId_t dlcid, bool enable);  // argc: 3, index: 12
    public bool SetDlcContext(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret GetDlcSizes();  // argc: 4, index: 14
    public UInt32 GetNumInstalledApps();  // argc: 0, index: 15
    public UInt32 GetInstalledApps(AppId_t[] appids, UInt32 arrayLength);  // argc: 2, index: 16
    public bool BIsWaitingForInstalledApps();  // argc: 0, index: 17
    public unknown_ret GetAppDependencies(AppId_t appid, AppId_t[] dependencies, UInt32 dependenciesMax);  // argc: 3, index: 18
    public unknown_ret GetDependentApps(AppId_t app, AppId_t[] dependantApps, int dependantAppsMax);  // argc: 3, index: 19
    public unknown_ret GetUpdateInfo(AppId_t app, AppUpdateInfo_s* updateInfo);  // argc: 2, index: 20
    public unknown_ret GetAppConfigValue(AppId_t app, string configKey, StringBuilder configValue, int maxConfigValue);  // argc: 4, index: 21
    public unknown_ret SetAppConfigValue(AppId_t app, string configKey, string configValue);  // argc: 3, index: 22
    public bool BIsAppUpToDate(AppId_t app);  // argc: 1, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret GetAvailableLanguages();  // argc: 4, index: 24
    public unknown_ret GetCurrentLanguage(AppId_t app, ref string outLang, uint unk1);  // argc: 3, index: 25
    public ELanguage GetCurrentLanguage(AppId_t app);  // argc: 1, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret GetFallbackLanguage();  // argc: 2, index: 27
    public unknown_ret SetCurrentLanguage(AppId_t app, ELanguage language);  // argc: 2, index: 28
    public unknown_ret StartValidatingApp(AppId_t app);  // argc: 1, index: 29
    public unknown_ret CancelValidation(AppId_t app);  // argc: 1, index: 30
    public unknown_ret MarkContentCorrupt(AppId_t app, bool corrupt);  // argc: 2, index: 31
    public unknown_ret GetInstalledDepots(AppId_t appid, uint[] depots, int depotsLen);  // argc: 3, index: 32
    public unknown_ret GetFileDetails(AppId_t appid, string file);  // argc: 2, index: 33
    public unknown_ret VerifySignedFiles(AppId_t appid);  // argc: 1, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret GetAvailableBetas();  // argc: 5, index: 35
    public unknown_ret CheckBetaPassword(AppId_t appid, string betaPassword);  // argc: 2, index: 36
    public unknown_ret BHasCachedBetaPassword(AppId_t appid, string betaName);  // argc: 2, index: 37
    public unknown_ret GetActiveBeta(AppId_t appid, StringBuilder betaOut, int betaOutLen);  // argc: 3, index: 38
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BGetActiveBetaForApps();  // argc: 2, index: 39
    public unknown_ret SetDownloadingEnabled(bool enabled);  // argc: 1, index: 40
    public bool BIsDownloadingEnabled();  // argc: 0, index: 41
    public unknown_ret GetDownloadStats(DownloadStats_s* stats);  // argc: 1, index: 42
    public AppId_t GetDownloadingAppID();  // argc: 0, index: 43
    public unknown_ret GetAutoUpdateTimeRestrictionEnabled();  // argc: 0, index: 44
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoUpdateTimeRestrictionEnabled();  // argc: 1, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret GetAutoUpdateTimeRestrictionHours();  // argc: 2, index: 46
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoUpdateTimeRestrictionStartHour();  // argc: 1, index: 47
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoUpdateTimeRestrictionEndHour();  // argc: 1, index: 48
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppAutoUpdateBehavior();  // argc: 1, index: 49
    // WARNING: Arguments are unknown!
    public unknown_ret SetAppAutoUpdateBehavior();  // argc: 2, index: 50
    // WARNING: Arguments are unknown!
    public unknown_ret SetAppAllowDownloadsWhileRunningBehavior();  // argc: 2, index: 51
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppAllowDownloadsWhileRunningBehavior();  // argc: 1, index: 52
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowDownloadsWhileAnyAppRunning();  // argc: 1, index: 53
    public unknown_ret BAllowDownloadsWhileAnyAppRunning();  // argc: 0, index: 54
    // WARNING: Arguments are unknown!
    public unknown_ret ChangeAppDownloadQueuePlacement();  // argc: 2, index: 55
    // WARNING: Arguments are unknown!
    public unknown_ret SetAppDownloadQueueIndex();  // argc: 2, index: 56
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppDownloadQueueIndex();  // argc: 1, index: 57
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppAutoUpdateDelayedUntilTime();  // argc: 1, index: 58
    public unknown_ret GetNumAppsInDownloadQueue();  // argc: 0, index: 59
    public unknown_ret BHasLocalContentServer();  // argc: 0, index: 60
    // WARNING: Arguments are unknown!
    public unknown_ret BuildBackup();  // argc: 4, index: 61
    // WARNING: Arguments are unknown!
    public unknown_ret BuildInstaller();  // argc: 4, index: 62
    public unknown_ret CancelBackup();  // argc: 0, index: 63
    // WARNING: Arguments are unknown!
    public unknown_ret RestoreAppFromBackup();  // argc: 2, index: 64
    // WARNING: Arguments are unknown!
    public unknown_ret RecoverAppFromFolder();  // argc: 2, index: 65
    public bool CanMoveApp(AppId_t appid, LibraryFolder_t folder);  // argc: 2, index: 66
    public EAppUpdateError MoveApp(AppId_t appid, LibraryFolder_t folder);  // argc: 2, index: 67
    // WARNING: Arguments are unknown!
    public unknown_ret GetMoveAppProgress();  // argc: 4, index: 68
    // WARNING: Arguments are unknown!
    public unknown_ret CancelMoveApp();  // argc: 1, index: 69
    // WARNING: Arguments are unknown!
    /// <summary>
    /// Called by ValveSteam 440 times.
    /// </summary>
    /// <returns></returns>
    public unknown_ret GetAppStateInfo();  // argc: 2, index: 70
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BGetAppStateInfoForApps();  // argc: 2, index: 71
    // WARNING: Arguments are unknown!
    public unknown_ret BCanRemotePlayTogether();  // argc: 1, index: 73
    // WARNING: Arguments are unknown!
    public unknown_ret BIsLocalMultiplayerApp();  // argc: 1, index: 74
    public unknown_ret GetNumLibraryFolders();  // argc: 0, index: 75
    public unknown_ret GetLibraryFolderPath(LibraryFolder_t folder, StringBuilder outPath, int outPathMaxLength);  // argc: 3, index: 76
    public unknown_ret AddLibraryFolder(string folderPath);  // argc: 1, index: 77
    public unknown_ret SetLibraryFolderLabel(LibraryFolder_t folder, string label);  // argc: 2, index: 78
    public unknown_ret GetLibraryFolderLabel(LibraryFolder_t folder, StringBuilder outLabel, int outLabelMaxLength);  // argc: 3, index: 79
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveLibraryFolder();  // argc: 3, index: 80
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BGetLibraryFolderInfo();  // argc: 4, index: 81
    public LibraryFolder_t GetAppLibraryFolder(AppId_t appid);  // argc: 1, index: 82
    public unknown_ret RefreshLibraryFolders();  // argc: 0, index: 83
    public UInt32 GetNumAppsInFolder(LibraryFolder_t folder);  // argc: 1, index: 84
    public UInt32 GetAppsInFolder(LibraryFolder_t folder, AppId_t[] apps, int appsLen);  // argc: 3, index: 85
    // WARNING: Arguments are unknown!
    public unknown_ret ForceInstallDirOverride(string directory);  // argc: 1, index: 86
    // WARNING: Arguments are unknown!
    public unknown_ret SetDownloadThrottleRateKbps();  // argc: 2, index: 87
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadThrottleRateKbps();  // argc: 1, index: 88
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendDownloadThrottling();  // argc: 1, index: 89
    // WARNING: Arguments are unknown!
    public unknown_ret SetThrottleDownloadsWhileStreaming();  // argc: 1, index: 90
    public unknown_ret BThrottleDownloadsWhileStreaming();  // argc: 0, index: 91
    // WARNING: Arguments are unknown!
    public unknown_ret GetLaunchQueryParam();  // argc: 2, index: 92
    // WARNING: Arguments are unknown!
    public unknown_ret BeginLaunchQueryParams();  // argc: 1, index: 93
    // WARNING: Arguments are unknown!
    public unknown_ret SetLaunchQueryParam();  // argc: 3, index: 94
    // WARNING: Arguments are unknown!
    public unknown_ret CommitLaunchQueryParams();  // argc: 2, index: 95
    // WARNING: Arguments are unknown!
    public unknown_ret GetLaunchCommandLine();  // argc: 3, index: 96
    public void AddContentLogLine(string msg);  // argc: 1, index: 97
    public void SetUseHTTPSForDownloads(bool val);  // argc: 1, index: 98
    public bool GetUseHTTPSForDownloads();  // argc: 0, index: 99
    // WARNING: Arguments are unknown!
    public unknown_ret SetPeerContentServerMode();  // argc: 1, index: 100
    // WARNING: Arguments are unknown!
    public unknown_ret SetPeerContentClientMode();  // argc: 1, index: 101
    public unknown_ret GetPeerContentServerMode();  // argc: 0, index: 102
    public unknown_ret GetPeerContentClientMode();  // argc: 0, index: 103
    // WARNING: Arguments are unknown!
    public unknown_ret GetPeerContentServerStats();  // argc: 1, index: 104
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendPeerContentClient();  // argc: 1, index: 105
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendPeerContentServer();  // argc: 1, index: 106
    // WARNING: Arguments are unknown!
    public unknown_ret GetPeerContentServerForApp();  // argc: 3, index: 107
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyDriveRemoved();  // argc: 1, index: 4
}