//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;


namespace OpenSteamworks.Generated;

public unsafe interface IClientRemoteStorage
{
    // WARNING: Arguments are unknown!
    public bool FileWrite(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file, byte[] data, int dataLen);  // argc: 5, index: 1, ipc args: [bytes4, bytes4, string, bytes4, bytes_length_from_mem], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public int GetFileSize(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 2, ipc args: [bytes4, bytes4, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t FileWriteAsync(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string path, [IPCIn] CUtlBuffer* data);  // argc: 4, index: 3, ipc args: [bytes4, bytes4, string, unknown], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t FileReadAsync(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file, UInt32 nOffset, UInt32 cubToRead);  // argc: 5, index: 4, ipc args: [bytes4, bytes4, string, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public bool FileReadAsyncComplete(AppId_t nAppId, SteamAPICall_t hReadCall, byte[] data, int dataLen);  // argc: 5, index: 5, ipc args: [bytes4, bytes8, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public int FileRead(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file, byte[] data, int dataLen);  // argc: 5, index: 6, ipc args: [bytes4, bytes4, string, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool FileForget(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 7, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool FileDelete(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 8, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t FileShare(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 9, ipc args: [bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public bool FileExists(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 10, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool FilePersisted(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 11, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public RTime32 GetFileTimestamp(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 12, ipc args: [bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetSyncPlatforms(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file, ERemoteStoragePlatform platform);  // argc: 4, index: 13, ipc args: [bytes4, bytes4, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public ERemoteStoragePlatform GetSyncPlatforms(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 14, ipc args: [bytes4, bytes4, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public UGCFileWriteStreamHandle_t FileWriteStreamOpen(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, string file);  // argc: 3, index: 15, ipc args: [bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteStreamClose(UGCFileWriteStreamHandle_t handle);  // argc: 2, index: 16, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteStreamCancel(UGCFileWriteStreamHandle_t handle);  // argc: 2, index: 17, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t handle, byte[] data, int dataLen);  // argc: 4, index: 18, ipc args: [bytes8, bytes4, bytes_length_from_mem], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public int GetFileCount(AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot);  // argc: 2, index: 19, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public string GetFileNameAndSize(AppId_t nAppId, int index, out ERemoteStorageFileRoot eRemoteStorageFileRoot, out int fileSizeBytes, bool unk);  // argc: 5, index: 20, ipc args: [bytes4, bytes4, bytes1], ipc returns: [string, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQuota();  // argc: 3, index: 21, ipc args: [bytes4], ipc returns: [bytes1, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUGCQuotaUsage();  // argc: 5, index: 22, ipc args: [bytes4], ipc returns: [bytes1, bytes8, bytes4, bytes8, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InitializeUGCQuotaUsage();  // argc: 1, index: 23, ipc args: [bytes4], ipc returns: [bytes1]
    public bool IsCloudEnabledForAccount();  // argc: 0, index: 24, ipc args: [], ipc returns: [boolean]
    public bool IsCloudEnabledForApp(AppId_t appid);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [boolean]
    public unknown_ret SetCloudEnabledForApp(AppId_t appid, bool enable);  // argc: 2, index: 1, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsCloudSyncOnSuspendAvailableForApp();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret IsCloudSyncOnSuspendEnabledForApp();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCloudSyncOnSuspendEnabledForApp();  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UGCDownload();  // argc: 4, index: 5, ipc args: [bytes8, bytes1, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UGCDownloadToLocation();  // argc: 4, index: 6, ipc args: [bytes8, string, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUGCDownloadProgress();  // argc: 4, index: 7, ipc args: [bytes8], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUGCDetails();  // argc: 6, index: 8, ipc args: [bytes8], ipc returns: [bytes1, bytes4, unknown, bytes4, uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret UGCRead();  // argc: 6, index: 9, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    public unknown_ret GetCachedUGCCount();  // argc: 0, index: 10, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCachedUGCHandle();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret PublishFile();  // argc: 10, index: 1, ipc args: [bytes4, bytes4, string, string, bytes4, string, string, bytes4, utlvector, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret PublishVideo();  // argc: 11, index: 2, ipc args: [bytes4, bytes4, string, string, bytes4, string, bytes4, string, string, bytes4, utlvector], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret PublishVideoFromURL();  // argc: 9, index: 3, ipc args: [bytes4, bytes4, string, string, bytes4, string, string, bytes4, utlvector], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CreatePublishedFileUpdateRequest();  // argc: 3, index: 4, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileFile();  // argc: 3, index: 5, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFilePreviewFile();  // argc: 3, index: 6, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileTitle();  // argc: 3, index: 7, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileDescription();  // argc: 3, index: 8, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileSetChangeDescription();  // argc: 3, index: 9, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileVisibility();  // argc: 3, index: 10, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileTags();  // argc: 3, index: 11, ipc args: [bytes8, utlvector], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileURL();  // argc: 3, index: 12, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CommitPublishedFileUpdate();  // argc: 4, index: 13, ipc args: [bytes4, bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPublishedFileDetails();  // argc: 4, index: 14, ipc args: [bytes8, bytes1, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DeletePublishedFile();  // argc: 2, index: 15, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserPublishedFiles();  // argc: 3, index: 16, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SubscribePublishedFile();  // argc: 3, index: 17, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserSubscribedFiles();  // argc: 4, index: 18, ipc args: [bytes4, bytes4, bytes1, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UnsubscribePublishedFile();  // argc: 3, index: 19, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserPublishedFileAction();  // argc: 4, index: 20, ipc args: [bytes4, bytes8, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumeratePublishedFilesByUserAction();  // argc: 3, index: 21, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserSubscribedFilesWithUpdates();  // argc: 3, index: 22, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCREItemVoteSummary();  // argc: 2, index: 23, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateUserPublishedItemVote();  // argc: 3, index: 24, ipc args: [bytes8, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserPublishedItemVoteDetails();  // argc: 2, index: 25, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserSharedWorkshopFiles();  // argc: 6, index: 26, ipc args: [bytes4, uint64, bytes4, utlvector, utlvector], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumeratePublishedWorkshopFiles();  // argc: 8, index: 27, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes4, utlvector, utlvector], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EGetFileSyncState();  // argc: 3, index: 28, ipc args: [bytes4, bytes4, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsFileSyncing();  // argc: 3, index: 29, ipc args: [bytes4, bytes4, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret FilePersist();  // argc: 3, index: 30, ipc args: [bytes4, bytes4, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret FileFetch();  // argc: 3, index: 31, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ResolvePath();  // argc: 5, index: 32, ipc args: [bytes4, bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret FileTouch();  // argc: 4, index: 33, ipc args: [bytes4, bytes4, string, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCloudEnabledForAccount();  // argc: 1, index: 34, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret LoadLocalFileInfoCache();  // argc: 1, index: 35, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EvaluateRemoteStorageSyncState();  // argc: 2, index: 36, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public ESteamCloudSyncState GetLastKnownSyncState(AppId_t appid);  // argc: 1, index: 37, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteStorageSyncState();  // argc: 1, index: 38, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret HaveLatestFilesLocally();  // argc: 1, index: 39, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetConflictingFileTimestamps();  // argc: 3, index: 40, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPendingRemoteOperationInfo();  // argc: 2, index: 41, ipc args: [bytes4], ipc returns: [bytes1, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret ResolveSyncConflict(AppId_t nAppId, bool bAcceptLocalFiles);  // argc: 2, index: 42, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SynchronizeApp(AppId_t nAppId, bool bSyncClient, bool bSyncServer);  // argc: 4, index: 43, ipc args: [bytes4, bytes4, bytes8], ipc returns: [bytes1]
    public unknown_ret IsAppSyncInProgress(AppId_t appid);  // argc: 1, index: 44, ipc args: [bytes4], ipc returns: [boolean]
    public unknown_ret RunAutoCloudOnAppLaunch(AppId_t appid);  // argc: 1, index: 45, ipc args: [bytes4], ipc returns: []
    public unknown_ret RunAutoCloudOnAppExit(AppId_t appid);  // argc: 1, index: 46, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ResetFileRequestState(AppId_t appid);  // argc: 1, index: 47, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearPublishFileUpdateRequests();  // argc: 1, index: 48, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetSubscribedFileDownloadCount();  // argc: 0, index: 49, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetSubscribedFileDownloadInfo(bool unk1);  // argc: 5, index: 0, ipc args: [bytes4], ipc returns: [boolean, bytes8, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetSubscribedFileDownloadInfo(double unk1, bool unk2);  // argc: 5, index: 1, ipc args: [bytes8], ipc returns: [boolean, bytes4, bytes4, bytes4]
    public unknown_ret PauseSubscribedFileDownloadsForApp(AppId_t appid);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    public unknown_ret ResumeSubscribedFileDownloadsForApp(AppId_t appid);  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: []
    public unknown_ret PauseAllSubscribedFileDownloads();  // argc: 0, index: 4, ipc args: [], ipc returns: []
    public unknown_ret ResumeAllSubscribedFileDownloads();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    public unknown_ret CancelCurrentAndPendingOperations();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalFileChangeCount();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalFileChange();  // argc: 4, index: 1, ipc args: [bytes4, bytes4], ipc returns: [string, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BeginFileWriteBatch();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EndFileWriteBatch();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes1]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetCloudEnabledForAppMap(CUtlMap<AppId_t, bool>* map);  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    /// <summary>
    /// This could be an enum.
    /// </summary>
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetLastKnownSyncStateMap(CUtlMap<AppId_t, ESteamCloudSyncState>* map);  // argc: 2, index: 5, ipc args: [bytes4, bytes4], ipc returns: []
}