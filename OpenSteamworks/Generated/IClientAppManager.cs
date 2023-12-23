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
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientAppManager
{
    public EAppUpdateError InstallApp(AppId_t unAppID, LibraryFolder_t libraryFolder, bool bLegacy);  // argc: 3, index: 1
    public EAppUpdateError UninstallApp(AppId_t unAppID);  // argc: 1, index: 2
    public EAppUpdateError LaunchApp(in CGameID gameID, uint uLaunchOption, ELaunchSource eLaunchSource, unknown_ret unknown);  // argc: 4, index: 3
    public unknown_ret ShutdownApp(AppId_t appId, bool force);  // argc: 2, index: 4
    public EAppState GetAppInstallState(AppId_t appid);  // argc: 1, index: 5
    public int GetAppInstallDir(AppId_t appid, StringBuilder path, int pathMax);  // argc: 3, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppContentInfo();  // argc: 6, index: 7
    // WARNING: Arguments are unknown!
    public bool GetAppStagingInfo();  // argc: 3, index: 8
    public bool IsAppDlcInstalled(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 9
    // WARNING: Arguments are unknown!
    public bool GetDlcDownloadProgress(AppId_t appid, ref UInt64 downloaded, ref UInt64 toDownload);  // argc: 4, index: 10
    public bool BIsDlcEnabled(AppId_t appid, AppId_t dlcid, ref bool appManagesDLC);  // argc: 3, index: 11
    public unknown_ret SetDlcEnabled(AppId_t appid, AppId_t dlcid, bool enable);  // argc: 3, index: 12
    public bool SetDlcContext(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret GetDlcSizes(AppId_t appid, AppId_t[] dlcs, int dlccount, long[] sizes);  // argc: 4, index: 14
    public UInt32 GetNumInstalledApps();  // argc: 0, index: 15
    public UInt32 GetInstalledApps(AppId_t[] appids, int arrayLength);  // argc: 2, index: 16
    public bool BIsWaitingForInstalledApps();  // argc: 0, index: 17
    public unknown_ret GetAppDependencies(AppId_t appid, AppId_t[] dependencies, int dependenciesMax);  // argc: 3, index: 18
    public unknown_ret GetDependentApps(AppId_t app, AppId_t[] dependantApps, int dependantAppsMax);  // argc: 3, index: 19
    public bool GetUpdateInfo(AppId_t app, AppUpdateInfo_s* updateInfo);  // argc: 2, index: 20
    public bool BIsAppUpToDate(AppId_t app);  // argc: 1, index: 23
    // WARNING: Arguments are unknown!
    public int GetAvailableLanguages(AppId_t appid, bool unk, StringBuilder langOut, int maxLangOut);  // argc: 4, index: 24
    public int GetCurrentLanguage(AppId_t app, StringBuilder langOut, int maxLangOut);  // argc: 3, index: 25
    public ELanguage GetCurrentLanguage(AppId_t app);  // argc: 1, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret GetFallbackLanguage(AppId_t appid, string language);  // argc: 2, index: 27
    public unknown_ret SetCurrentLanguage(AppId_t app, ELanguage language);  // argc: 2, index: 28
    public bool StartValidatingApp(AppId_t app);  // argc: 1, index: 29
    public bool CancelValidation(AppId_t app);  // argc: 1, index: 30
    /// <summary>
    /// Marks an app as missing/corrupted.
    /// </summary>
    /// <param name="corrupt">True for corrupted, false for missing files</param>
    public bool MarkContentCorrupt(AppId_t app, bool corrupt);  // argc: 2, index: 31
    public uint GetInstalledDepots(AppId_t appid, uint[] depots, uint depotsLen);  // argc: 3, index: 32
    public SteamAPICall_t GetFileDetails(AppId_t appid, string file);  // argc: 2, index: 33
    public SteamAPICall_t VerifySignedFiles(AppId_t appid);  // argc: 1, index: 34
    // WARNING: Arguments are unknown!
    public int GetAvailableBetas(AppId_t appid);  // argc: 5, index: 35
    public bool CheckBetaPassword(AppId_t appid, string betaPassword);  // argc: 2, index: 36
    public SteamAPICall_t SetActiveBeta(AppId_t appid, string beta);  // argc: 2, index: 17
    public int GetActiveBeta(AppId_t appid, StringBuilder betaOut, int betaOutLen);  // argc: 3, index: 38
    [BlacklistedInCrossProcessIPC]
    public bool BGetActiveBetaForApps(CUtlVector<AppId_t>* apps, CUtlStringList* betas);  // argc: 2, index: 39
    public bool SetDownloadingEnabled(bool enabled);  // argc: 1, index: 40
    public bool BIsDownloadingEnabled();  // argc: 0, index: 41
    public unknown_ret GetDownloadStats(DownloadStats_s* stats);  // argc: 1, index: 42
    public AppId_t GetDownloadingAppID();  // argc: 0, index: 43
    public bool GetAutoUpdateTimeRestrictionEnabled();  // argc: 0, index: 44
    public void SetAutoUpdateTimeRestrictionEnabled(bool val);  // argc: 1, index: 45
    // WARNING: Arguments are unknown!
    public bool GetAutoUpdateTimeRestrictionHours();  // argc: 2, index: 46
    // WARNING: Arguments are unknown!
    public bool SetAutoUpdateTimeRestrictionStartHour();  // argc: 1, index: 47
    // WARNING: Arguments are unknown!
    public bool SetAutoUpdateTimeRestrictionEndHour();  // argc: 1, index: 48
    public EAppAutoUpdateBehavior GetAppAutoUpdateBehavior(AppId_t appid);  // argc: 1, index: 49
    public bool SetAppAutoUpdateBehavior(AppId_t appid, EAppAutoUpdateBehavior val);  // argc: 2, index: 50
    public bool SetAppAllowDownloadsWhileRunningBehavior(AppId_t appid, EAppAllowDownloadsWhileRunningBehavior val);  // argc: 2, index: 51
    public EAppAllowDownloadsWhileRunningBehavior GetAppAllowDownloadsWhileRunningBehavior(AppId_t appid);  // argc: 1, index: 52
    public void SetAllowDownloadsWhileAnyAppRunning(bool val);  // argc: 1, index: 53
    public bool BAllowDownloadsWhileAnyAppRunning();  // argc: 0, index: 54
    public bool ChangeAppDownloadQueuePlacement(AppId_t appid, EAppDownloadQueuePlacement placement);  // argc: 2, index: 55
    public void SetAppDownloadQueueIndex(AppId_t appid, int index);  // argc: 2, index: 56
    public int GetAppDownloadQueueIndex(AppId_t appid);  // argc: 1, index: 57
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppAutoUpdateDelayedUntilTime();  // argc: 1, index: 58
    public int GetNumAppsInDownloadQueue();  // argc: 0, index: 59
    public bool BHasLocalContentServer();  // argc: 0, index: 60
    /// <summary>
    /// Builds a backup at the specified path. Does not create a subdirectory.
    /// Creates various Disk_XXXX numbered folders, according to ullMaxFileSize, which dictates the maximum size of one disk.
    /// </summary>
    public EAppUpdateError BuildBackup(AppId_t unAppID, UInt64 ullMaxFileSize, string cszBackupPath);  // argc: 4, index: 61
    // WARNING: Arguments are unknown!
    public unknown_ret BuildInstaller(string projectFile, string backupPath, string unk, string unk2);  // argc: 4, index: 62
    public bool CancelBackup();  // argc: 0, index: 63
    public EAppUpdateError RestoreAppFromBackup(AppId_t appid, string pathToBackup);  // argc: 2, index: 64
    public EAppUpdateError RecoverAppFromFolder(AppId_t appid, string folder);  // argc: 2, index: 65
    public EAppUpdateError CanMoveApp(AppId_t appid, ref AppId_t dependentApp);  // argc: 2, index: 66
    public EAppUpdateError MoveApp(AppId_t appid, LibraryFolder_t folder);  // argc: 2, index: 67
    // WARNING: Arguments are unknown!
    public unknown_ret GetMoveAppProgress(AppId_t appid, int unk1, int unk2, int unk3);  // argc: 4, index: 68
    // WARNING: Arguments are unknown!
    public bool CancelMoveApp(AppId_t appid);  // argc: 1, index: 69
    // WARNING: Arguments are unknown!
    /// <summary>
    /// Called by ValveSteam 440 times.
    /// </summary>
    /// <returns></returns>
    public unknown_ret GetAppStateInfo(AppId_t appid, AppStateInfo_s* state);  // argc: 2, index: 70
    [BlacklistedInCrossProcessIPC]
    public bool BGetAppStateInfoForApps(CUtlVector<AppId_t>* apps, CUtlVector<AppStateInfo_s>* states);  // argc: 2, index: 71
    
    // There's a function between these two "bool unk(AppId_t appid, string str)"
    // Except not for our steamclient version...
    // Seems to be a platform compatibility testing function
    
    public bool BCanRemotePlayTogether(AppId_t appid);  // argc: 1, index: 73
    public bool BIsLocalMultiplayerApp(AppId_t appid);  // argc: 1, index: 74
    public int GetNumLibraryFolders();  // argc: 0, index: 75
    public int GetLibraryFolderPath(LibraryFolder_t folder, StringBuilder outPath, int outPathMaxLength);  // argc: 3, index: 76
    public LibraryFolder_t AddLibraryFolder(string folderPath);  // argc: 1, index: 77
    public void SetLibraryFolderLabel(LibraryFolder_t folder, string label);  // argc: 2, index: 78
    public int GetLibraryFolderLabel(LibraryFolder_t folder, StringBuilder outLabel, int outLabelMaxLength);  // argc: 3, index: 79
    // WARNING: Arguments are unknown!
    // Can return 7 on failure. Maybe some sort of enum?
    /// <summary>
    /// Removes a library folder.
    /// </summary>
    /// <param name="libraryFolder"></param>
    /// <param name="unk1">Use false.</param>
    /// <param name="unk2">Use false.</param>
    /// <returns></returns>
    public unknown_ret RemoveLibraryFolder(LibraryFolder_t libraryFolder, bool unk1 = false, bool unk2 = false);  // argc: 3, index: 80
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public bool BGetLibraryFolderInfo(LibraryFolder_t libraryFolder, ref bool unk, ref UInt64 usedDiskSpace, ref UInt64 freeDiskSpace);  // argc: 4, index: 81
    public LibraryFolder_t GetAppLibraryFolder(AppId_t appid);  // argc: 1, index: 82
    public unknown_ret RefreshLibraryFolders();  // argc: 0, index: 83
    public UInt32 GetNumAppsInFolder(LibraryFolder_t folder);  // argc: 1, index: 84
    public UInt32 GetAppsInFolder(LibraryFolder_t folder, AppId_t[] apps, int appsLen);  // argc: 3, index: 85
    /// <summary>
    /// Forces all apps installed in the current session to be installed to a specific non-library folder directory.
    /// </summary>
    public unknown_ret ForceInstallDirOverride(string directory);  // argc: 1, index: 86
    // WARNING: Arguments are unknown!
    public unknown_ret SetDownloadThrottleRateKbps();  // argc: 2, index: 87
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadThrottleRateKbps();  // argc: 1, index: 88
    // WARNING: Arguments are unknown!
    public void SuspendDownloadThrottling(bool val);  // argc: 1, index: 89
    public void SetThrottleDownloadsWhileStreaming(bool val);  // argc: 1, index: 90
    public bool BThrottleDownloadsWhileStreaming();  // argc: 0, index: 91
    public string GetLaunchQueryParam(AppId_t appid, string key);  // argc: 2, index: 92
    // WARNING: Arguments are unknown!
    public unknown_ret BeginLaunchQueryParams(AppId_t appid);  // argc: 1, index: 93
    // WARNING: Arguments are unknown!
    public unknown_ret SetLaunchQueryParam(AppId_t appid, string key, string value);  // argc: 3, index: 94
    // WARNING: Arguments are unknown!
    public unknown_ret CommitLaunchQueryParams(AppId_t appid, string unk);  // argc: 2, index: 95
    public int GetLaunchCommandLine(AppId_t appid, StringBuilder commandLine, int commandLineMax);  // argc: 3, index: 96
    public void AddContentLogLine(string msg);  // argc: 1, index: 97
    public void SetUseHTTPSForDownloads(bool val);  // argc: 1, index: 98
    public bool GetUseHTTPSForDownloads();  // argc: 0, index: 99
    // WARNING: Arguments are unknown!
    public unknown_ret SetPeerContentServerMode(EPeerContentMode mode);  // argc: 1, index: 100
    // WARNING: Arguments are unknown!
    public unknown_ret SetPeerContentClientMode(EPeerContentMode mode);  // argc: 1, index: 101
    public EPeerContentMode GetPeerContentServerMode();  // argc: 0, index: 102
    public EPeerContentMode GetPeerContentClientMode();  // argc: 0, index: 103
    // WARNING: Arguments are unknown!
    public unknown_ret GetPeerContentServerStats(UInt64 unk);  // argc: 1, index: 104
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendPeerContentClient(UInt64 unk);  // argc: 1, index: 105
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendPeerContentServer(UInt64 unk);  // argc: 1, index: 106
    // WARNING: Arguments are unknown!
    public UInt64 GetPeerContentServerForApp(AppId_t appid, int unk, int unk2);  // argc: 3, index: 107
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyDriveAdded();  // argc: 1, index: 4
    // WARNING: Arguments are unknown!
    public SteamAPICall_t NotifyDriveRemoved(string path);  // argc: 1, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret SetAudioDownloadQuality();  // argc: 1, index: 6
}