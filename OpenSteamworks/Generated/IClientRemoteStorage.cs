//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Generated;

public unsafe interface IClientRemoteStorage
{
    // WARNING: Arguments are unknown!
    public unknown_ret FileWrite();  // argc: 5, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret GetFileSize();  // argc: 3, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteAsync();  // argc: 4, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret FileReadAsync();  // argc: 5, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret FileReadAsyncComplete();  // argc: 5, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret FileRead();  // argc: 5, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret FileForget();  // argc: 3, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret FileDelete();  // argc: 3, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret FileShare();  // argc: 3, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret FileExists();  // argc: 3, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret FilePersisted();  // argc: 3, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetFileTimestamp();  // argc: 3, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret SetSyncPlatforms();  // argc: 4, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret GetSyncPlatforms();  // argc: 3, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteStreamOpen();  // argc: 3, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteStreamClose();  // argc: 2, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteStreamCancel();  // argc: 2, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret FileWriteStreamWriteChunk();  // argc: 4, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret GetFileCount();  // argc: 2, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret GetFileNameAndSize();  // argc: 5, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret GetQuota();  // argc: 3, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret GetUGCQuotaUsage();  // argc: 5, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret InitializeUGCQuotaUsage();  // argc: 1, index: 23
    public bool IsCloudEnabledForAccount();  // argc: 0, index: 24
    public bool IsCloudEnabledForApp(AppId_t appid);  // argc: 1, index: 25
    public unknown_ret SetCloudEnabledForApp(AppId_t appid, bool enable);  // argc: 2, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret IsCloudSyncOnSuspendAvailableForApp();  // argc: 1, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret IsCloudSyncOnSuspendEnabledForApp();  // argc: 1, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret SetCloudSyncOnSuspendEnabledForApp();  // argc: 2, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret UGCDownload();  // argc: 4, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret UGCDownloadToLocation();  // argc: 4, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret GetUGCDownloadProgress();  // argc: 4, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret GetUGCDetails();  // argc: 6, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret UGCRead();  // argc: 6, index: 34
    public unknown_ret GetCachedUGCCount();  // argc: 0, index: 35
    // WARNING: Arguments are unknown!
    public unknown_ret GetCachedUGCHandle();  // argc: 1, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret PublishFile();  // argc: 10, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret PublishVideo();  // argc: 11, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret PublishVideoFromURL();  // argc: 9, index: 39
    // WARNING: Arguments are unknown!
    public unknown_ret CreatePublishedFileUpdateRequest();  // argc: 3, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileFile();  // argc: 3, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFilePreviewFile();  // argc: 3, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileTitle();  // argc: 3, index: 43
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileDescription();  // argc: 3, index: 44
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileSetChangeDescription();  // argc: 3, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileVisibility();  // argc: 3, index: 46
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileTags();  // argc: 3, index: 47
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePublishedFileURL();  // argc: 3, index: 48
    // WARNING: Arguments are unknown!
    public unknown_ret CommitPublishedFileUpdate();  // argc: 4, index: 49
    // WARNING: Arguments are unknown!
    public unknown_ret GetPublishedFileDetails();  // argc: 4, index: 50
    // WARNING: Arguments are unknown!
    public unknown_ret DeletePublishedFile();  // argc: 2, index: 51
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserPublishedFiles();  // argc: 3, index: 52
    // WARNING: Arguments are unknown!
    public unknown_ret SubscribePublishedFile();  // argc: 3, index: 53
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserSubscribedFiles();  // argc: 4, index: 54
    // WARNING: Arguments are unknown!
    public unknown_ret UnsubscribePublishedFile();  // argc: 3, index: 55
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserPublishedFileAction();  // argc: 4, index: 56
    // WARNING: Arguments are unknown!
    public unknown_ret EnumeratePublishedFilesByUserAction();  // argc: 3, index: 57
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserSubscribedFilesWithUpdates();  // argc: 3, index: 58
    // WARNING: Arguments are unknown!
    public unknown_ret GetCREItemVoteSummary();  // argc: 2, index: 59
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateUserPublishedItemVote();  // argc: 3, index: 60
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserPublishedItemVoteDetails();  // argc: 2, index: 61
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateUserSharedWorkshopFiles();  // argc: 6, index: 62
    // WARNING: Arguments are unknown!
    public unknown_ret EnumeratePublishedWorkshopFiles();  // argc: 8, index: 63
    // WARNING: Arguments are unknown!
    public unknown_ret EGetFileSyncState();  // argc: 3, index: 64
    // WARNING: Arguments are unknown!
    public unknown_ret BIsFileSyncing();  // argc: 3, index: 65
    // WARNING: Arguments are unknown!
    public unknown_ret FilePersist();  // argc: 3, index: 66
    // WARNING: Arguments are unknown!
    public unknown_ret FileFetch();  // argc: 3, index: 67
    // WARNING: Arguments are unknown!
    public unknown_ret ResolvePath();  // argc: 5, index: 68
    // WARNING: Arguments are unknown!
    public unknown_ret FileTouch();  // argc: 4, index: 69
    // WARNING: Arguments are unknown!
    public unknown_ret SetCloudEnabledForAccount();  // argc: 1, index: 70
    // WARNING: Arguments are unknown!
    public unknown_ret LoadLocalFileInfoCache();  // argc: 1, index: 71
    // WARNING: Arguments are unknown!
    public unknown_ret EvaluateRemoteStorageSyncState();  // argc: 2, index: 72
    // WARNING: Arguments are unknown!
    public unknown_ret GetLastKnownSyncState();  // argc: 1, index: 73
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemoteStorageSyncState();  // argc: 1, index: 74
    // WARNING: Arguments are unknown!
    public unknown_ret HaveLatestFilesLocally();  // argc: 1, index: 75
    // WARNING: Arguments are unknown!
    public unknown_ret GetConflictingFileTimestamps();  // argc: 3, index: 76
    // WARNING: Arguments are unknown!
    public unknown_ret GetPendingRemoteOperationInfo();  // argc: 2, index: 77
    // WARNING: Arguments are unknown!
    public unknown_ret ResolveSyncConflict(AppId_t nAppId, bool bAcceptLocalFiles);  // argc: 2, index: 78
    // WARNING: Arguments are unknown!
    public unknown_ret SynchronizeApp(AppId_t nAppId, bool bSyncClient, bool bSyncServer);  // argc: 4, index: 79
    public unknown_ret IsAppSyncInProgress(AppId_t appid);  // argc: 1, index: 80
    public unknown_ret RunAutoCloudOnAppLaunch(AppId_t appid);  // argc: 1, index: 81
    public unknown_ret RunAutoCloudOnAppExit(AppId_t appid);  // argc: 1, index: 82
    // WARNING: Arguments are unknown!
    public unknown_ret ResetFileRequestState(AppId_t appid);  // argc: 1, index: 83
    // WARNING: Arguments are unknown!
    public unknown_ret ClearPublishFileUpdateRequests();  // argc: 1, index: 84
    public unknown_ret GetSubscribedFileDownloadCount();  // argc: 0, index: 85
    // WARNING: Arguments are unknown!
    public unknown_ret BGetSubscribedFileDownloadInfo(bool unk1);  // argc: 5, index: 86
    // WARNING: Arguments are unknown!
    public unknown_ret BGetSubscribedFileDownloadInfo(double unk1, bool unk2);  // argc: 5, index: 87
    public unknown_ret PauseSubscribedFileDownloadsForApp(AppId_t appid);  // argc: 1, index: 88
    public unknown_ret ResumeSubscribedFileDownloadsForApp(AppId_t appid);  // argc: 1, index: 89
    public unknown_ret PauseAllSubscribedFileDownloads();  // argc: 0, index: 90
    public unknown_ret ResumeAllSubscribedFileDownloads();  // argc: 0, index: 91
    public unknown_ret CancelCurrentAndPendingOperations();  // argc: 0, index: 92
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalFileChangeCount();  // argc: 1, index: 93
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalFileChange();  // argc: 4, index: 94
    // WARNING: Arguments are unknown!
    public unknown_ret BeginFileWriteBatch();  // argc: 1, index: 95
    // WARNING: Arguments are unknown!
    public unknown_ret EndFileWriteBatch();  // argc: 1, index: 96
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetCloudEnabledForAppMap(CUtlMap<AppId_t, bool>* map);  // argc: 1, index: 97
    /// <summary>
    /// This could be an enum.
    /// </summary>
    [BlacklistedInCrossProcessIPC]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLastKnownSyncStateMap(CUtlMap<AppId_t, uint>* map);  // argc: 2, index: 98
}