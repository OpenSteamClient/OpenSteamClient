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

#ifndef ICLIENTREMOTESTORAGE_H
#define ICLIENTREMOTESTORAGE_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "RemoteStorageCommon.h"

abstract_class IClientRemoteStorage
{
public:
    
    virtual EResult FileWrite( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, const void *pvData, int32 cubData ) = 0; //argc: 5, index 1
    virtual int32 GetFileSize( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile  ) = 0; //argc: 3, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret FileWriteAsync() = 0; //argc: 4, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret FileReadAsync() = 0; //argc: 5, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret FileReadAsyncComplete() = 0; //argc: 5, index 5
    
    virtual int32 FileRead( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, void *pvData, int32 cubDataToRead ) = 0; //argc: 5, index 6
    
    virtual bool FileForget( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 7
    virtual bool FileDelete( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 8
    virtual SteamAPICall_t FileShare( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 9
    
    virtual bool FileExists( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 10
    
    virtual bool FilePersisted( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 11
    virtual int64 GetFileTimestamp( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 12
    
    virtual bool SetSyncPlatforms( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, ERemoteStoragePlatform eRemoteStoragePlatform ) = 0; //argc: 4, index 13
    virtual ERemoteStoragePlatform GetSyncPlatforms( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 14
    
    virtual GID_t FileWriteStreamOpen( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult FileWriteStreamClose( GID_t hStream ) = 0; //argc: 2, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult FileWriteStreamCancel( GID_t hStream ) = 0; //argc: 2, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual EResult FileWriteStreamWriteChunk( GID_t hStream, const void *pvData, int32 cubData ) = 0; //argc: 4, index 18
    
    virtual int32 GetFileCount( AppId_t nAppId, bool bFromExternalAPI ) = 0; //argc: 2, index 19
    virtual const char *GetFileNameAndSize( AppId_t nAppId, int32 iFile, ERemoteStorageFileRoot *peRemoteStorageFileRoot, int32 *pnFileSizeInBytes, bool bFromExternalAPI ) = 0; //argc: 5, index 20
    
    virtual bool GetQuota( AppId_t nAppId, int32 *pnTotalBytes, int32 *pnAvailableBytes ) = 0; //argc: 3, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetUGCQuotaUsage() = 0; //argc: 5, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InitializeUGCQuotaUsage() = 0; //argc: 1, index 23
    
    virtual bool IsCloudEnabledForAccount() = 0; //argc: 0, index 24
    virtual bool IsCloudEnabledForApp( AppId_t nAppId ) = 0; //argc: 1, index 2
    virtual void SetCloudEnabledForApp( AppId_t nAppId, bool bEnable ) = 0; //argc: 2, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IsCloudSyncOnSuspendAvailableForApp() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IsCloudSyncOnSuspendEnabledForApp() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetCloudSyncOnSuspendEnabledForApp() = 0; //argc: 2, index 6
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t UGCDownload( UGCHandle_t hContent, bool bUseNewCallback, uint32 uUnk ) = 0; //argc: 4, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t UGCDownloadToLocation( UGCHandle_t hContent, const char *cszLocation, uint32 uUnk ) = 0; //argc: 4, index 8
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool GetUGCDownloadProgress( UGCHandle_t hContent, uint32 *puDownloadedBytes, uint32 *puTotalBytes ) = 0; //argc: 4, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool GetUGCDetails( UGCHandle_t hContent, AppId_t *pnAppID, char **ppchName, int32 *pnFileSizeInBytes, CSteamID *pSteamIDOwner ) = 0; //argc: 6, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual int32 UGCRead( UGCHandle_t hContent, void *pvData, int32 cubDataToRead, uint32 uOffset, EUGCReadAction eAction ) = 0; //argc: 6, index 11
    virtual int32 GetCachedUGCCount() = 0; //argc: 0, index 12
    virtual UGCHandle_t GetCachedUGCHandle( int32 iCachedContent ) = 0; //argc: 1, index 1
    
    virtual SteamAPICall_t PublishFile( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *cszFileName, const char *cszPreviewFileName, AppId_t nConsumerAppId , const char *cszTitle, const char *cszDescription, ERemoteStoragePublishedFileVisibility eVisibility, SteamParamStringArray_t const *pTags, EWorkshopFileType eType ) = 0; //argc: 10, index 2
    virtual SteamAPICall_t PublishVideo( AppId_t nAppId, EWorkshopVideoProvider eVideoProvider, const char *cszVideoAccountName, const char *cszVideoIdentifier, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *cszFileName, AppId_t nConsumerAppId, const char *cszTitle, const char *cszDescription, ERemoteStoragePublishedFileVisibility eVisibility, SteamParamStringArray_t const *pTags ) = 0; //argc: 11, index 3
    virtual SteamAPICall_t PublishVideoFromURL( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *cszVideoURL, const char *cszFileName, AppId_t nConsumerAppId, const char *cszTitle, const char *cszDescription, ERemoteStoragePublishedFileVisibility eVisibility, SteamParamStringArray_t const *pTags ) = 0; //argc: 9, index 4
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual JobID_t CreatePublishedFileUpdateRequest( AppId_t nAppId, PublishedFileId_t unPublishedFileId ) = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileFile( JobID_t hUpdateRequest, const char *cszFile ) = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFilePreviewFile( JobID_t hUpdateRequest, const char *cszPreviewFile ) = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileTitle( JobID_t hUpdateRequest, const char *cszTitle ) = 0; //argc: 3, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileDescription( JobID_t hUpdateRequest, const char *cszDescription ) = 0; //argc: 3, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileSetChangeDescription( JobID_t hUpdateRequest, const char *cszDescription ) = 0; //argc: 3, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileVisibility( JobID_t hUpdateRequest, ERemoteStoragePublishedFileVisibility eVisibility ) = 0; //argc: 3, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileTags( JobID_t hUpdateRequest, SteamParamStringArray_t const *pTags ) = 0; //argc: 3, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool UpdatePublishedFileURL( JobID_t hUpdateRequest, const char *cszURL ) = 0; //argc: 3, index 13
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t CommitPublishedFileUpdate( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, JobID_t hUpdateRequest ) = 0; //argc: 4, index 14
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t GetPublishedFileDetails( PublishedFileId_t unPublishedFileId, bool bUseNewCallback, uint32 ) = 0; //argc: 4, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t DeletePublishedFile( PublishedFileId_t unPublishedFileId ) = 0; //argc: 2, index 16
    virtual SteamAPICall_t EnumerateUserPublishedFiles( AppId_t nAppId, uint32 uStartIndex, ERemoteStoragePublishedFileSortOrder eOrder ) = 0; //argc: 3, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t SubscribePublishedFile( AppId_t nAppId, PublishedFileId_t unPublishedFileId ) = 0; //argc: 3, index 18
    virtual SteamAPICall_t EnumerateUserSubscribedFiles( AppId_t nAppId, uint32 uStartIndex, uint8 uListType, EPublishedFileInfoMatchingFileType eMatchingFileType ) = 0; //argc: 4, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t UnsubscribePublishedFile( AppId_t nAppId, PublishedFileId_t unPublishedFileId ) = 0; //argc: 3, index 20
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t SetUserPublishedFileAction( AppId_t nAppId, PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction ) = 0; //argc: 4, index 21
    virtual SteamAPICall_t EnumeratePublishedFilesByUserAction( AppId_t nAppId, EWorkshopFileAction eAction, uint32 uStartIndex ) = 0; //argc: 3, index 22
    virtual SteamAPICall_t EnumerateUserSubscribedFilesWithUpdates( AppId_t nAppId, uint32 uStartIndex, RTime32 uStartTime ) = 0; //argc: 3, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t GetCREItemVoteSummary( PublishedFileId_t unPublishedFileId ) = 0; //argc: 2, index 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t UpdateUserPublishedItemVote( PublishedFileId_t unPublishedFileId, bool bVoteUp ) = 0; //argc: 3, index 25
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t GetUserPublishedItemVoteDetails( PublishedFileId_t unPublishedFileId ) = 0; //argc: 2, index 26
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t EnumerateUserSharedWorkshopFiles( AppId_t nAppId, CSteamID creatorSteamID, uint32 uStartIndex, SteamParamStringArray_t const *pRequiredTags, SteamParamStringArray_t const *pExcludedTags ) = 0; //argc: 6, index 27
    virtual SteamAPICall_t EnumeratePublishedWorkshopFiles( AppId_t nAppId, EWorkshopEnumerationType eType, EPublishedFileInfoMatchingFileType eFileType, uint32 uStartIndex, uint32 cDays, uint32 cCount, SteamParamStringArray_t const *pTags, SteamParamStringArray_t const *pUserTags ) = 0; //argc: 8, index 28
    
    virtual EFileRemoteStorageSyncState EGetFileSyncState( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 29
    virtual bool BIsFileSyncing( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 30
    
    virtual EResult FilePersist( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 31
    
    virtual bool FileFetch( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0; //argc: 3, index 32
    
    virtual bool ResolvePath( AppId_t nAppID, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchRelPath, char *pchDest, uint32 cchDest ) = 0; //argc: 5, index 33
    
    virtual EResult FileTouch( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, bool ) = 0; //argc: 4, index 34
    
    virtual void SetCloudEnabledForAccount( bool bEnabled ) = 0; //argc: 1, index 35
    
    virtual void LoadLocalFileInfoCache( AppId_t nAppId ) = 0; //argc: 1, index 36
    
    virtual void EvaluateRemoteStorageSyncState( AppId_t nAppId, bool bUnk) = 0; //argc: 2, index 37
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLastKnownSyncState() = 0; //argc: 1, index 38
    virtual ERemoteStorageSyncState GetRemoteStorageSyncState( AppId_t nAppId ) = 0; //argc: 1, index 39
    
    virtual bool HaveLatestFilesLocally( AppId_t nAppId ) = 0; //argc: 1, index 40
    
    virtual bool GetConflictingFileTimestamps( AppId_t nAppId, RTime32* pnTimestampLocal, RTime32* pnTimestampRemote ) = 0; //argc: 3, index 41
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPendingRemoteOperationInfo() = 0; //argc: 2, index 42
    virtual bool ResolveSyncConflict( AppId_t nAppId, bool bAcceptLocalFiles ) = 0; //argc: 2, index 43
    
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SynchronizeApp( AppId_t nAppId, bool bSyncClient, bool bSyncServer ) = 0; //argc: 4, index 44
    virtual bool IsAppSyncInProgress( AppId_t nAppId ) = 0; //argc: 1, index 45
    
    virtual void RunAutoCloudOnAppLaunch( AppId_t nAppId ) = 0; //argc: 1, index 46
    virtual void RunAutoCloudOnAppExit( AppId_t nAppId ) = 0; //argc: 1, index 47
    
    virtual bool ResetFileRequestState( AppId_t nAppId ) = 0; //argc: 1, index 48
    
    virtual void ClearPublishFileUpdateRequests( AppId_t nAppId ) = 0; //argc: 1, index 49
    
    virtual int32 GetSubscribedFileDownloadCount() = 0; //argc: 0, index 50
    virtual bool BGetSubscribedFileDownloadInfo( int32 iFile, PublishedFileId_t* punPublishedFileId, uint32 *puBytesDownloaded, uint32 *puBytesExpected, AppId_t* pnAppId ) = 0; //argc: 5, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool BGetSubscribedFileDownloadInfo( PublishedFileId_t unPublishedFileId, uint32 *puBytesDownloaded, uint32 *puBytesExpected, AppId_t* pnAppId ) = 0; //argc: 5, index 3
    virtual void PauseSubscribedFileDownloadsForApp( AppId_t nAppId ) = 0; //argc: 1, index 4
    virtual void ResumeSubscribedFileDownloadsForApp( AppId_t nAppId ) = 0; //argc: 1, index 5
    virtual void PauseAllSubscribedFileDownloads() = 0; //argc: 0, index 6
    virtual void ResumeAllSubscribedFileDownloads() = 0; //argc: 0, index 1
    virtual unknown_ret CancelCurrentAndPendingOperations() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalFileChangeCount() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalFileChange() = 0; //argc: 4, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BeginFileWriteBatch() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EndFileWriteBatch() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetCloudEnabledForAppMap() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLastKnownSyncStateMap() = 0; //argc: 2, index 6
};

#endif // ICLIENTREMOTESTORAGE_H