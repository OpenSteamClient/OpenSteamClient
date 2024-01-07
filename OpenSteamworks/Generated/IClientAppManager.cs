//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;
using OpenSteamworks.Attributes;
using OpenSteamworks.Enums;

using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientAppManager
{
    public EAppUpdateError InstallApp(AppId_t unAppID, LibraryFolder_t libraryFolder, bool bLegacy);  // argc: 3, index: 1, ipc args: [bytes4, bytes4, bytes1], ipc returns: [bytes4]
    public EAppUpdateError UninstallApp(AppId_t unAppID);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes4]
    public EAppUpdateError LaunchApp(in CGameID gameID, uint uLaunchOption, ELaunchSource eLaunchSource, unknown_ret unknown);  // argc: 4, index: 3, ipc args: [bytes8, bytes4, bytes4, string], ipc returns: [bytes4]
    public unknown_ret ShutdownApp(AppId_t appId, bool force);  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    public EAppState GetAppInstallState(AppId_t appid);  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes4]
    public int GetAppInstallDir(AppId_t appid, StringBuilder path, int pathMax);  // argc: 3, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppContentInfo();  // argc: 6, index: 7, ipc args: [bytes4, bytes1], ipc returns: [bytes4, bytes4, bytes4, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    public bool GetAppStagingInfo();  // argc: 3, index: 8, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes8]
    public bool IsAppDlcInstalled(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 9, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool GetDlcDownloadProgress(AppId_t appid, ref UInt64 downloaded, ref UInt64 toDownload);  // argc: 4, index: 10, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes8, bytes8]
    public bool BIsDlcEnabled(AppId_t appid, AppId_t dlcid, ref bool appManagesDLC);  // argc: 3, index: 11, ipc args: [bytes4, bytes4], ipc returns: [boolean, boolean]
    public unknown_ret SetDlcEnabled(AppId_t appid, AppId_t dlcid, bool enable);  // argc: 3, index: 12, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    public bool SetDlcContext(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 13, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDlcSizes(AppId_t appid, [IPCOut] uint[] dlcs, int dlccount, [IPCOut] long[] sizes);  // argc: 4, index: 14, ipc args: [bytes4, bytes4, bytes_length_from_reg], ipc returns: [bytes1, bytes_length_from_reg]
    public uint GetNumInstalledApps();  // argc: 0, index: 15, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetInstalledApps([IPCOut] uint[] appids, uint arrayLength);  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public bool BIsWaitingForInstalledApps();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetAppDependencies(AppId_t appid, [IPCOut] uint[] dependencies, int dependenciesMax);  // argc: 3, index: 0, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public unknown_ret GetDependentApps(AppId_t app, [IPCOut] uint[] dependantApps, int dependantAppsMax);  // argc: 3, index: 1, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public bool GetUpdateInfo(AppId_t app, out AppUpdateInfo_s updateInfo);  // argc: 2, index: 2, ipc args: [bytes4], ipc returns: [bytes1, bytes120]
    public bool BIsAppUpToDate(AppId_t app);  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public int GetAvailableLanguages(AppId_t appid, bool unk, StringBuilder langOut, int maxLangOut);  // argc: 4, index: 4, ipc args: [bytes4, bytes1, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public int GetCurrentLanguage(AppId_t app, StringBuilder langOut, int maxLangOut);  // argc: 3, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public ELanguage GetCurrentLanguage(AppId_t app);  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFallbackLanguage(AppId_t appid, string language);  // argc: 2, index: 7, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public unknown_ret SetCurrentLanguage(AppId_t app, ELanguage language);  // argc: 2, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public bool StartValidatingApp(AppId_t app);  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes1]
    public bool CancelValidation(AppId_t app);  // argc: 1, index: 10, ipc args: [bytes4], ipc returns: [bytes1]
    /// <summary>
    /// Marks an app as missing/corrupted.
    /// </summary>
    /// <param name="corrupt">True for corrupted, false for missing files</param>
    public bool MarkContentCorrupt(AppId_t app, bool corrupt);  // argc: 2, index: 11, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    public uint GetInstalledDepots(AppId_t appid, DepotId_t* depots, uint depotsLen);  // argc: 3, index: 12, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public SteamAPICall_t GetFileDetails(AppId_t appid, string file);  // argc: 2, index: 13, ipc args: [bytes4, string], ipc returns: [bytes8]
    public SteamAPICall_t VerifySignedFiles(AppId_t appid);  // argc: 1, index: 14, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public int GetAvailableBetas(AppId_t appid);  // argc: 5, index: 15, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes4, bytes_length_from_mem, bytes4]
    public bool CheckBetaPassword(AppId_t appid, string betaPassword);  // argc: 2, index: 16, ipc args: [bytes4, string], ipc returns: [bytes1]
    public SteamAPICall_t SetActiveBeta(AppId_t appid, string beta);  // argc: 2, index: 17, ipc args: [bytes4, string], ipc returns: [bytes1]
    public int GetActiveBeta(AppId_t appid, StringBuilder betaOut, int betaOutLen);  // argc: 3, index: 18, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    [BlacklistedInCrossProcessIPC]
    public bool BGetActiveBetaForApps(CUtlVector<AppId_t>* apps, CUtlStringList* betas);  // argc: 2, index: 19, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    public bool SetDownloadingEnabled(bool enabled);  // argc: 1, index: 20, ipc args: [bytes1], ipc returns: [bytes1]
    public bool BIsDownloadingEnabled();  // argc: 0, index: 21, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetDownloadStats(DownloadStats_s* stats);  // argc: 1, index: 0, ipc args: [], ipc returns: [bytes1, bytes28]
    public AppId_t GetDownloadingAppID();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    public bool GetAutoUpdateTimeRestrictionEnabled();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    public void SetAutoUpdateTimeRestrictionEnabled(bool val);  // argc: 1, index: 0, ipc args: [bytes1], ipc returns: []
    public bool GetAutoUpdateTimeRestrictionHours(out int startTime, out int endTime);  // argc: 2, index: 1, ipc args: [], ipc returns: [bytes1, bytes4, bytes4]
    public bool SetAutoUpdateTimeRestrictionStartHour(out int startTime);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes1]
    public bool SetAutoUpdateTimeRestrictionEndHour(out int endTime);  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes1]
    public EAppAutoUpdateBehavior GetAppAutoUpdateBehavior(AppId_t appid);  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: [bytes4]
    public bool SetAppAutoUpdateBehavior(AppId_t appid, EAppAutoUpdateBehavior val);  // argc: 2, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public bool SetAppAllowDownloadsWhileRunningBehavior(AppId_t appid, EAppAllowDownloadsWhileRunningBehavior val);  // argc: 2, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public EAppAllowDownloadsWhileRunningBehavior GetAppAllowDownloadsWhileRunningBehavior(AppId_t appid);  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: [bytes4]
    public void SetAllowDownloadsWhileAnyAppRunning(bool val);  // argc: 1, index: 8, ipc args: [bytes1], ipc returns: []
    public bool BAllowDownloadsWhileAnyAppRunning();  // argc: 0, index: 9, ipc args: [], ipc returns: [boolean]
    public bool ChangeAppDownloadQueuePlacement(AppId_t appid, EAppDownloadQueuePlacement placement);  // argc: 2, index: 0, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public void SetAppDownloadQueueIndex(AppId_t appid, int index);  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public int GetAppDownloadQueueIndex(AppId_t appid);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppAutoUpdateDelayedUntilTime();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes4]
    public int GetNumAppsInDownloadQueue();  // argc: 0, index: 4, ipc args: [], ipc returns: [bytes4]
    public bool BHasLocalContentServer();  // argc: 0, index: 0, ipc args: [], ipc returns: [boolean]
    /// <summary>
    /// Builds a backup at the specified path. Does not create a subdirectory.
    /// Creates various Disk_XXXX numbered folders, according to ullMaxFileSize, which dictates the maximum size of one disk.
    /// </summary>
    // WARNING: Arguments are unknown!
    public EAppUpdateError BuildBackup(AppId_t unAppID, UInt64 ullMaxFileSize, string cszBackupPath);  // argc: 4, index: 0, ipc args: [bytes4, bytes8, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BuildInstaller(string projectFile, string backupPath, string unk, string unk2);  // argc: 4, index: 1, ipc args: [string, string, string, string], ipc returns: [bytes4]
    public bool CancelBackup();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes1]
    public EAppUpdateError RestoreAppFromBackup(AppId_t appid, string pathToBackup);  // argc: 2, index: 0, ipc args: [bytes4, string], ipc returns: [bytes4]
    public EAppUpdateError RecoverAppFromFolder(AppId_t appid, string folder);  // argc: 2, index: 1, ipc args: [bytes4, string], ipc returns: [bytes4]
    public EAppUpdateError CanMoveApp(AppId_t appid, ref AppId_t dependentApp);  // argc: 2, index: 2, ipc args: [bytes4], ipc returns: [bytes4, bytes4]
    public EAppUpdateError MoveApp(AppId_t appid, LibraryFolder_t folder);  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMoveAppProgress(AppId_t appid, int unk1, int unk2, int unk3);  // argc: 4, index: 4, ipc args: [bytes4], ipc returns: [bytes1, bytes8, bytes8, bytes4]
    // WARNING: Arguments are unknown!
    public bool CancelMoveApp(AppId_t appid);  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    /// <summary>
    /// Called by ValveSteam 440 times.
    /// </summary>
    /// <returns></returns>
    public unknown_ret GetAppStateInfo(AppId_t appid, AppStateInfo_s* state);  // argc: 2, index: 6, ipc args: [bytes4], ipc returns: [bytes1, bytes44]
    [BlacklistedInCrossProcessIPC]
    public bool BGetAppStateInfoForApps(CUtlVector<AppId_t>* apps, CUtlVector<AppStateInfo_s>* states);  // argc: 2, index: 7, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    
    // There's a function between these two "bool unk(AppId_t appid, string str)"
    // Except not for our steamclient version...
    // Seems to be a platform compatibility testing function
    
    public bool BCanRemotePlayTogether(AppId_t appid);  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: [boolean]
    public bool BIsLocalMultiplayerApp(AppId_t appid);  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [boolean]
    public int GetNumLibraryFolders();  // argc: 0, index: 10, ipc args: [], ipc returns: [bytes4]
    public int GetLibraryFolderPath(LibraryFolder_t folder, StringBuilder outPath, int outPathMaxLength);  // argc: 3, index: 0, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public LibraryFolder_t AddLibraryFolder(string folderPath);  // argc: 1, index: 1, ipc args: [string], ipc returns: [bytes4]
    public void SetLibraryFolderLabel(LibraryFolder_t folder, string label);  // argc: 2, index: 2, ipc args: [bytes4, string], ipc returns: []
    public int GetLibraryFolderLabel(LibraryFolder_t folder, StringBuilder outLabel, int outLabelMaxLength);  // argc: 3, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    // Can return 7 on failure. Maybe some sort of enum?
    /// <summary>
    /// Removes a library folder.
    /// </summary>
    /// <param name="libraryFolder"></param>
    /// <param name="unk1">Use false.</param>
    /// <param name="unk2">Use false.</param>
    /// <returns></returns>
    public unknown_ret RemoveLibraryFolder(LibraryFolder_t libraryFolder, bool unk1 = false, bool unk2 = false);  // argc: 3, index: 4, ipc args: [bytes4, bytes1, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public bool BGetLibraryFolderInfo(LibraryFolder_t libraryFolder, ref bool unk, ref UInt64 usedDiskSpace, ref UInt64 freeDiskSpace);  // argc: 4, index: 5, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: [boolean]
    public LibraryFolder_t GetAppLibraryFolder(AppId_t appid);  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret RefreshLibraryFolders();  // argc: 0, index: 7, ipc args: [], ipc returns: []
    public UInt32 GetNumAppsInFolder(LibraryFolder_t folder);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    public UInt32 GetAppsInFolder(LibraryFolder_t folder, AppId_t* apps, int appsLen);  // argc: 3, index: 1, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    /// <summary>
    /// Forces all apps installed in the current session to be installed to a specific non-library folder directory.
    /// </summary>
    public unknown_ret ForceInstallDirOverride(string directory);  // argc: 1, index: 2, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetDownloadThrottleRateKbps();  // argc: 2, index: 3, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadThrottleRateKbps();  // argc: 1, index: 4, ipc args: [bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public void SuspendDownloadThrottling(bool val);  // argc: 1, index: 5, ipc args: [bytes1], ipc returns: []
    public void SetThrottleDownloadsWhileStreaming(bool val);  // argc: 1, index: 6, ipc args: [bytes1], ipc returns: []
    public bool BThrottleDownloadsWhileStreaming();  // argc: 0, index: 7, ipc args: [], ipc returns: [boolean]
    public string GetLaunchQueryParam(AppId_t appid, string key);  // argc: 2, index: 0, ipc args: [bytes4, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret BeginLaunchQueryParams(AppId_t appid);  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetLaunchQueryParam(AppId_t appid, string key, string value);  // argc: 3, index: 2, ipc args: [bytes4, string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CommitLaunchQueryParams(AppId_t appid, string unk);  // argc: 2, index: 3, ipc args: [bytes4, string], ipc returns: [bytes1]
    public int GetLaunchCommandLine(AppId_t appid, StringBuilder commandLine, int commandLineMax);  // argc: 3, index: 4, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    public void AddContentLogLine(string msg);  // argc: 1, index: 5, ipc args: [string], ipc returns: []
    public void SetUseHTTPSForDownloads(bool val);  // argc: 1, index: 6, ipc args: [bytes1], ipc returns: []
    public bool GetUseHTTPSForDownloads();  // argc: 0, index: 7, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPeerContentServerMode(EPeerContentMode mode);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPeerContentClientMode(EPeerContentMode mode);  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    public EPeerContentMode GetPeerContentServerMode();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes4]
    public EPeerContentMode GetPeerContentClientMode();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPeerContentServerStats(UInt64 unk);  // argc: 1, index: 0, ipc args: [], ipc returns: [bytes1, bytes40]
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendPeerContentClient(UInt64 unk);  // argc: 1, index: 1, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendPeerContentServer(UInt64 unk);  // argc: 1, index: 2, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public UInt64 GetPeerContentServerForApp(AppId_t appid, int unk, int unk2);  // argc: 3, index: 3, ipc args: [bytes4], ipc returns: [string, bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyDriveAdded();  // argc: 1, index: 4, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public SteamAPICall_t NotifyDriveRemoved(string path);  // argc: 1, index: 5, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetAudioDownloadQuality();  // argc: 1, index: 6, ipc args: [bytes1], ipc returns: []
}