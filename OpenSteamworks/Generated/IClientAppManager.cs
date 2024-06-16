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
    public EAppError InstallApp(AppId_t unAppID, LibraryFolder_t libraryFolder, bool bLegacy);  // argc: 3, index: 1, ipc args: [bytes4, bytes4, bytes1], ipc returns: [bytes4]
    public EAppError UninstallApp(AppId_t unAppID);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes4]
    public EAppError LaunchApp(in CGameID gameID, uint uLaunchOption, ELaunchSource eLaunchSource, string unkStr);  // argc: 4, index: 3, ipc args: [bytes8, bytes4, bytes4, string], ipc returns: [bytes4]
    public bool ShutdownApp(AppId_t appId, bool force);  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    public EAppState GetAppInstallState(AppId_t appid);  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes4]
    public int GetAppInstallDir(AppId_t appid, StringBuilder path, int pathMax);  // argc: 3, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppContentInfo(AppId_t appid, bool bUnk, out uint installedBuildID, out RTime32 installedBuildTimestamp, out ulong installedSize, out ulong installedDLCSize);  // argc: 6, index: 7, ipc args: [bytes4, bytes1], ipc returns: [bytes4, bytes4, bytes4, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    public bool GetAppStagingInfo(AppId_t appid, out uint unkOut, out ulong unkOut2);  // argc: 3, index: 8, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes8]
    public bool IsAppDlcInstalled(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 9, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    public bool GetDlcDownloadProgress(AppId_t appid, AppId_t dlcid, out UInt64 downloaded, out UInt64 toDownload);  // argc: 4, index: 10, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes8, bytes8]
    public bool BIsDlcEnabled(AppId_t appid, AppId_t dlcid, out bool appManagesDLC);  // argc: 3, index: 11, ipc args: [bytes4, bytes4], ipc returns: [boolean, boolean]
    public void SetDlcEnabled(AppId_t appid, AppId_t dlcid, bool enable);  // argc: 3, index: 12, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    public bool SetDlcContext(AppId_t appid, AppId_t dlcid);  // argc: 2, index: 13, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool GetDlcSizes(AppId_t appid, uint[] dlcs, int dlccount, long[] sizes);  // argc: 4, index: 14, ipc args: [bytes4, bytes4, bytes_length_from_reg], ipc returns: [bytes1, bytes_length_from_reg]
    public uint GetNumInstalledApps();  // argc: 0, index: 15, ipc args: [], ipc returns: [bytes4]
    public uint GetInstalledApps(uint[] appids, uint arrayLength);  // argc: 2, index: 16, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public bool BIsWaitingForInstalledApps();  // argc: 0, index: 17, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetAppDependencies(AppId_t appid, uint[] dependencies, int dependenciesMax);  // argc: 3, index: 18, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public unknown_ret GetDependentApps(AppId_t app, uint[] dependantApps, int dependantAppsMax);  // argc: 3, index: 19, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public bool GetUpdateInfo(AppId_t app, out AppUpdateInfo_s updateInfo);  // argc: 2, index: 20, ipc args: [bytes4], ipc returns: [bytes1, bytes120]
    public bool BIsAppUpToDate(AppId_t app);  // argc: 1, index: 21, ipc args: [bytes4], ipc returns: [boolean]
    /// <summary>
    /// Gets all the available languages for the app.
    /// </summary>
    /// <param name="langOut">Comma separated list of supported languages</param>
    /// <returns>How much data was returned. Trimmed if buffer is too small.</returns>
    // WARNING: Arguments are unknown!
    public int GetAvailableLanguages(AppId_t appid, bool unk, StringBuilder langOut, int maxLangOut);  // argc: 4, index: 22, ipc args: [bytes4, bytes1, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    /// <summary>
    /// Gets the current language of the app.
    /// </summary>
    /// <returns>How much data was returned. Trimmed if buffer is too small.</returns>
    public int GetCurrentLanguage(AppId_t app, StringBuilder langOut, int maxLangOut);  // argc: 3, index: 23, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public ELanguage GetCurrentLanguage(AppId_t app);  // argc: 1, index: 24, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public ELanguage GetFallbackLanguage(AppId_t appid, ELanguage fallback);  // argc: 2, index: 25, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public unknown_ret SetCurrentLanguage(AppId_t app, ELanguage language);  // argc: 2, index: 26, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public bool StartValidatingApp(AppId_t app);  // argc: 1, index: 27, ipc args: [bytes4], ipc returns: [bytes1]
    public bool CancelValidation(AppId_t app);  // argc: 1, index: 28, ipc args: [bytes4], ipc returns: [bytes1]
    /// <summary>
    /// Marks an app as missing/corrupted.
    /// </summary>
    /// <param name="corrupt">True for corrupted, false for missing files</param>
    public bool MarkContentCorrupt(AppId_t app, bool corrupt);  // argc: 2, index: 29, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    public uint GetInstalledDepots(AppId_t appid, uint[] depots, uint depotsLen);  // argc: 3, index: 30, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public SteamAPICall_t GetFileDetails(AppId_t appid, string file);  // argc: 2, index: 31, ipc args: [bytes4, string], ipc returns: [bytes8]
    public SteamAPICall_t VerifySignedFiles(AppId_t appid);  // argc: 1, index: 32, ipc args: [bytes4], ipc returns: [bytes8]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unk"></param>
    /// <param name="unk3"></param>
    /// <param name="length"></param>
    /// <param name="unk2"></param>
    /// <returns>The number of betas available</returns>
    public int GetAvailableBetas(AppId_t appid, out uint unk, StringBuilder unk3, int length, out uint unk2);  // argc: 5, index: 33, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes4, bytes_length_from_mem, bytes4]
    public bool CheckBetaPassword(AppId_t appid, string betaPassword);  // argc: 2, index: 34, ipc args: [bytes4, string], ipc returns: [bytes1]
    public bool SetActiveBeta(AppId_t appid, string beta);  // argc: 2, index: 35, ipc args: [bytes4, string], ipc returns: [bytes1]
    public int GetActiveBeta(AppId_t appid, StringBuilder betaOut, int betaOutLen);  // argc: 3, index: 36, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    [BlacklistedInCrossProcessIPC]
    public bool BGetActiveBetaForApps(CUtlVector<AppId_t>* apps, CUtlStringList* betas);  // argc: 2, index: 37, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    public bool SetDownloadingEnabled(bool enabled);  // argc: 1, index: 38, ipc args: [bytes1], ipc returns: [bytes1]
    public bool BIsDownloadingEnabled();  // argc: 0, index: 39, ipc args: [], ipc returns: [boolean]
    public bool GetDownloadStats(out DownloadStats_s stats);  // argc: 1, index: 40, ipc args: [], ipc returns: [bytes1, bytes28]
    public AppId_t GetDownloadingAppID();  // argc: 0, index: 41, ipc args: [], ipc returns: [bytes4]
    public bool GetAutoUpdateTimeRestrictionEnabled();  // argc: 0, index: 42, ipc args: [], ipc returns: [bytes1]
    public void SetAutoUpdateTimeRestrictionEnabled(bool val);  // argc: 1, index: 43, ipc args: [bytes1], ipc returns: []
    public bool GetAutoUpdateTimeRestrictionHours(out int startTime, out int endTime);  // argc: 2, index: 44, ipc args: [], ipc returns: [bytes1, bytes4, bytes4]
    public bool SetAutoUpdateTimeRestrictionStartHour(out int startTime);  // argc: 1, index: 45, ipc args: [bytes4], ipc returns: [bytes1]
    public bool SetAutoUpdateTimeRestrictionEndHour(out int endTime);  // argc: 1, index: 46, ipc args: [bytes4], ipc returns: [bytes1]
    public EAppAutoUpdateBehavior GetAppAutoUpdateBehavior(AppId_t appid);  // argc: 1, index: 47, ipc args: [bytes4], ipc returns: [bytes4]
    public bool SetAppAutoUpdateBehavior(AppId_t appid, EAppAutoUpdateBehavior val);  // argc: 2, index: 48, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public bool SetAppAllowDownloadsWhileRunningBehavior(AppId_t appid, EAppAllowDownloadsWhileRunningBehavior val);  // argc: 2, index: 49, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public EAppAllowDownloadsWhileRunningBehavior GetAppAllowDownloadsWhileRunningBehavior(AppId_t appid);  // argc: 1, index: 50, ipc args: [bytes4], ipc returns: [bytes4]
    public void SetAllowDownloadsWhileAnyAppRunning(bool val);  // argc: 1, index: 51, ipc args: [bytes1], ipc returns: []
    public bool BAllowDownloadsWhileAnyAppRunning();  // argc: 0, index: 52, ipc args: [], ipc returns: [boolean]
    public bool ChangeAppDownloadQueuePlacement(AppId_t appid, EAppDownloadQueuePlacement placement);  // argc: 2, index: 53, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public bool SetAppDownloadQueueIndex(AppId_t appid, int index);  // argc: 2, index: 54, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public int GetAppDownloadQueueIndex(AppId_t appid);  // argc: 1, index: 55, ipc args: [bytes4], ipc returns: [bytes4]
    public RTime32 GetAppAutoUpdateDelayedUntilTime(AppId_t appid);  // argc: 1, index: 56, ipc args: [bytes4], ipc returns: [bytes4]
    public int GetNumAppsInDownloadQueue();  // argc: 0, index: 57, ipc args: [], ipc returns: [bytes4]
    public bool BHasLocalContentServer();  // argc: 0, index: 58, ipc args: [], ipc returns: [boolean]
    /// <summary>
    /// Builds a backup at the specified path. Does not create a subdirectory.
    /// Creates various Disk_XXXX numbered folders, according to ullMaxFileSize, which dictates the maximum size of one disk.
    /// </summary>
    // WARNING: Arguments are unknown!
    public EAppError BuildBackup(AppId_t unAppID, UInt64 ullMaxFileSize, string cszBackupPath);  // argc: 4, index: 59, ipc args: [bytes4, bytes8, string], ipc returns: [bytes4]
    /// <summary>
    /// This function is meant for people publishing their Steam games as CD installers.
    /// </summary> 
    // WARNING: Arguments are unknown!
    public EAppError BuildInstaller(string projectFile, string backupPath, string unk, string unk2);  // argc: 4, index: 60, ipc args: [string, string, string, string], ipc returns: [bytes4]
    public bool CancelBackup();  // argc: 0, index: 61, ipc args: [], ipc returns: [bytes1]
    public EAppError RestoreAppFromBackup(AppId_t appid, string pathToBackup);  // argc: 2, index: 62, ipc args: [bytes4, string], ipc returns: [bytes4]
    public EAppError RecoverAppFromFolder(AppId_t appid, string folder);  // argc: 2, index: 63, ipc args: [bytes4, string], ipc returns: [bytes4]
    public EAppError CanMoveApp(AppId_t appid, out AppId_t dependentApp);  // argc: 2, index: 64, ipc args: [bytes4], ipc returns: [bytes4, bytes4]
    public EAppError MoveApp(AppId_t appid, LibraryFolder_t folder);  // argc: 2, index: 65, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool GetMoveAppProgress(AppId_t appid, out UInt64 unk1, out UInt64 unk2, out uint unk3);  // argc: 4, index: 66, ipc args: [bytes4], ipc returns: [bytes1, bytes8, bytes8, bytes4]
    // WARNING: Arguments are unknown!
    public bool CancelMoveApp(AppId_t appid);  // argc: 1, index: 67, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    /// <summary>
    /// Called by ValveSteam 440 times.
    /// </summary>
    /// <returns></returns>
    public bool GetAppStateInfo(AppId_t appid, out AppStateInfo_s state);  // argc: 2, index: 68, ipc args: [bytes4], ipc returns: [bytes1, bytes36]
    [BlacklistedInCrossProcessIPC]
    public bool BGetAppStateInfoForApps(CUtlVector<AppId_t>* apps, CUtlVector<AppStateInfo_s>* states);  // argc: 2, index: 69, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    
    // There's a function between these two "bool unk(AppId_t appid, string str)"
    // Except not for our steamclient version...
    // Seems to be a platform compatibility testing function
    
    public bool BCanRemotePlayTogether(AppId_t appid);  // argc: 1, index: 70, ipc args: [bytes4], ipc returns: [boolean]
    public bool BIsLocalMultiplayerApp(AppId_t appid);  // argc: 1, index: 71, ipc args: [bytes4], ipc returns: [boolean]
    public int GetNumLibraryFolders();  // argc: 0, index: 72, ipc args: [], ipc returns: [bytes4]
    public int GetLibraryFolderPath(LibraryFolder_t folder, StringBuilder outPath, int outPathMaxLength);  // argc: 3, index: 73, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public LibraryFolder_t AddLibraryFolder(string folderPath);  // argc: 1, index: 74, ipc args: [string], ipc returns: [bytes4]
    public void SetLibraryFolderLabel(LibraryFolder_t folder, string label);  // argc: 2, index: 75, ipc args: [bytes4, string], ipc returns: []
    public int GetLibraryFolderLabel(LibraryFolder_t folder, StringBuilder outLabel, int outLabelMaxLength);  // argc: 3, index: 76, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    /// <summary>
    /// Removes a library folder.
    /// </summary>
    /// <param name="libraryFolder"></param>
    /// <param name="unk1">Use false.</param>
    /// <param name="unk2">Use false.</param>
    /// <returns>An appid that is blocking the library folder from being removed</returns>
    public uint RemoveLibraryFolder(LibraryFolder_t libraryFolder, bool unk1 = false, bool unk2 = false);  // argc: 3, index: 77, ipc args: [bytes4, bytes1, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public bool BGetLibraryFolderInfo(LibraryFolder_t libraryFolder, out bool mounted, out UInt64 usedDiskSpace, out UInt64 freeDiskSpace);  // argc: 4, index: 78, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: [boolean]
    public LibraryFolder_t GetAppLibraryFolder(AppId_t appid);  // argc: 1, index: 79, ipc args: [bytes4], ipc returns: [bytes4]
    public void RefreshLibraryFolders();  // argc: 0, index: 80, ipc args: [], ipc returns: []
    public int GetNumAppsInFolder(LibraryFolder_t folder);  // argc: 1, index: 81, ipc args: [bytes4], ipc returns: [bytes4]
    public int GetAppsInFolder(LibraryFolder_t folder, uint[] apps, int appsLen);  // argc: 3, index: 82, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    /// <summary>
    /// Forces all apps installed in the current session to be installed to a specific non-library folder directory.
    /// </summary>
    public void ForceInstallDirOverride(string directory);  // argc: 1, index: 83, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool SetDownloadThrottleRateKbps(int rate, bool unk);  // argc: 2, index: 84, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public int GetDownloadThrottleRateKbps(bool unk);  // argc: 1, index: 85, ipc args: [bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public void SuspendDownloadThrottling(bool val);  // argc: 1, index: 86, ipc args: [bytes1], ipc returns: []
    public void SetThrottleDownloadsWhileStreaming(bool val);  // argc: 1, index: 87, ipc args: [bytes1], ipc returns: []
    public bool BThrottleDownloadsWhileStreaming();  // argc: 0, index: 88, ipc args: [], ipc returns: [boolean]
    public string GetLaunchQueryParam(AppId_t appid, string key);  // argc: 2, index: 89, ipc args: [bytes4, string], ipc returns: [string]
    public void BeginLaunchQueryParams(AppId_t appid);  // argc: 1, index: 90, ipc args: [bytes4], ipc returns: []
    public void SetLaunchQueryParam(AppId_t appid, string key, string value);  // argc: 3, index: 91, ipc args: [bytes4, string, string], ipc returns: []
    public bool CommitLaunchQueryParams(AppId_t appid, string unk);  // argc: 2, index: 92, ipc args: [bytes4, string], ipc returns: [bytes1]
    public int GetLaunchCommandLine(AppId_t appid, StringBuilder commandLine, int commandLineMax);  // argc: 3, index: 93, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    public void AddContentLogLine(string msg);  // argc: 1, index: 94, ipc args: [string], ipc returns: []
    public void SetUseHTTPSForDownloads(bool val);  // argc: 1, index: 95, ipc args: [bytes1], ipc returns: []
    public bool GetUseHTTPSForDownloads();  // argc: 0, index: 96, ipc args: [], ipc returns: [bytes1]
    public void SetPeerContentServerMode(EPeerContentMode mode);  // argc: 1, index: 97, ipc args: [bytes4], ipc returns: []
    public void SetPeerContentClientMode(EPeerContentMode mode);  // argc: 1, index: 98, ipc args: [bytes4], ipc returns: []
    public EPeerContentMode GetPeerContentServerMode();  // argc: 0, index: 99, ipc args: [], ipc returns: [bytes4]
    public EPeerContentMode GetPeerContentClientMode();  // argc: 0, index: 100, ipc args: [], ipc returns: [bytes4]
    public bool GetPeerContentServerStats(out PeerContentServerStats_s stats);  // argc: 1, index: 101, ipc args: [], ipc returns: [bytes1, bytes40]
    // WARNING: Arguments are unknown!
    public void SuspendPeerContentClient(bool unk);  // argc: 1, index: 102, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SuspendPeerContentServer(bool unk);  // argc: 1, index: 103, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public string GetPeerContentServerForApp(AppId_t appid, out bool unk1, out bool unk2);  // argc: 3, index: 104, ipc args: [bytes4], ipc returns: [string, bytes1, bytes1]
    public void NotifyDriveAdded(string drivePath);  // argc: 1, index: 105, ipc args: [string], ipc returns: []
    public void NotifyDriveRemoved(string drivePath);  // argc: 1, index: 106, ipc args: [string], ipc returns: []
    public void SetAudioDownloadQuality(bool bHighQuality);  // argc: 1, index: 107, ipc args: [bytes1], ipc returns: []
}