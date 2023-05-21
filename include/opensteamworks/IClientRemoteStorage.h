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

	virtual EResult FileWrite( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, const void *pvData, int32 cubData ) = 0;
	virtual int32 GetFileSize( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile  ) = 0;

	virtual int32 FileRead( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, void *pvData, int32 cubDataToRead ) = 0;
	
	virtual bool FileForget( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;
	virtual bool FileDelete( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;
	virtual SteamAPICall_t FileShare( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual bool FileExists( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual bool FilePersisted( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;
	virtual int64 GetFileTimestamp( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual bool SetSyncPlatforms( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, ERemoteStoragePlatform eRemoteStoragePlatform ) = 0;
	virtual ERemoteStoragePlatform GetSyncPlatforms( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual GID_t FileWriteStreamOpen( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;
	virtual EResult FileWriteStreamClose( GID_t hStream ) = 0;
	virtual EResult FileWriteStreamCancel( GID_t hStream ) = 0;
	virtual EResult FileWriteStreamWriteChunk( GID_t hStream, const void *pvData, int32 cubData ) = 0;

	virtual int32 GetFileCount( AppId_t nAppId, bool bFromExternalAPI ) = 0;
	virtual const char *GetFileNameAndSize( AppId_t nAppId, int32 iFile, ERemoteStorageFileRoot *peRemoteStorageFileRoot, int32 *pnFileSizeInBytes, bool bFromExternalAPI ) = 0;

	virtual bool GetQuota( AppId_t nAppId, int32 *pnTotalBytes, int32 *pnAvailableBytes ) = 0;
	
	virtual bool IsCloudEnabledForAccount();
	virtual bool IsCloudEnabledForApp( AppId_t nAppId );
	virtual void SetCloudEnabledForApp( AppId_t nAppId, bool bEnable );

	virtual SteamAPICall_t UGCDownload( UGCHandle_t hContent, bool bUseNewCallback, uint32 uUnk ) = 0; // Old callback id = 1308, new callback id = 1317
	virtual SteamAPICall_t UGCDownloadToLocation( UGCHandle_t hContent, const char *cszLocation, uint32 uUnk ) = 0;

	virtual bool GetUGCDownloadProgress( UGCHandle_t hContent, uint32 *puDownloadedBytes, uint32 *puTotalBytes );
	virtual bool GetUGCDetails( UGCHandle_t hContent, AppId_t *pnAppID, char **ppchName, int32 *pnFileSizeInBytes, CSteamID *pSteamIDOwner ) = 0;
	virtual int32 UGCRead( UGCHandle_t hContent, void *pvData, int32 cubDataToRead, uint32 uOffset, EUGCReadAction eAction ) = 0;
	virtual int32 GetCachedUGCCount() = 0;
	virtual UGCHandle_t GetCachedUGCHandle( int32 iCachedContent ) = 0;

	virtual SteamAPICall_t PublishFile( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *cszFileName, const char *cszPreviewFileName, AppId_t nConsumerAppId , const char *cszTitle, const char *cszDescription, ERemoteStoragePublishedFileVisibility eVisibility, SteamParamStringArray_t const *pTags, EWorkshopFileType eType ) = 0;
	virtual SteamAPICall_t PublishVideo( AppId_t nAppId, EWorkshopVideoProvider eVideoProvider, const char *cszVideoAccountName, const char *cszVideoIdentifier, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *cszFileName, AppId_t nConsumerAppId, const char *cszTitle, const char *cszDescription, ERemoteStoragePublishedFileVisibility eVisibility, SteamParamStringArray_t const *pTags ) = 0;
	virtual SteamAPICall_t PublishVideoFromURL( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *cszVideoURL, const char *cszFileName, AppId_t nConsumerAppId, const char *cszTitle, const char *cszDescription, ERemoteStoragePublishedFileVisibility eVisibility, SteamParamStringArray_t const *pTags ) = 0;

	virtual JobID_t CreatePublishedFileUpdateRequest( AppId_t nAppId, PublishedFileId_t unPublishedFileId ) = 0;
	virtual bool UpdatePublishedFileFile( JobID_t hUpdateRequest, const char *cszFile ) = 0;
	virtual bool UpdatePublishedFilePreviewFile( JobID_t hUpdateRequest, const char *cszPreviewFile ) = 0;
	virtual bool UpdatePublishedFileTitle( JobID_t hUpdateRequest, const char *cszTitle ) = 0;
	virtual bool UpdatePublishedFileDescription( JobID_t hUpdateRequest, const char *cszDescription ) = 0;
	virtual bool UpdatePublishedFileSetChangeDescription( JobID_t hUpdateRequest, const char *cszDescription ) = 0;
	virtual bool UpdatePublishedFileVisibility( JobID_t hUpdateRequest, ERemoteStoragePublishedFileVisibility eVisibility ) = 0;
	virtual bool UpdatePublishedFileTags( JobID_t hUpdateRequest, SteamParamStringArray_t const *pTags ) = 0;
	virtual bool UpdatePublishedFileURL( JobID_t hUpdateRequest, const char *cszURL ) = 0;
	
	virtual SteamAPICall_t CommitPublishedFileUpdate( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, JobID_t hUpdateRequest ) = 0;
	
	virtual SteamAPICall_t GetPublishedFileDetails( PublishedFileId_t unPublishedFileId, bool bUseNewCallback, uint32 ) = 0; // Old callback id = 1310, new callback id = 1318
	virtual SteamAPICall_t DeletePublishedFile( PublishedFileId_t unPublishedFileId ) = 0;
	virtual SteamAPICall_t EnumerateUserPublishedFiles( AppId_t nAppId, uint32 uStartIndex, ERemoteStoragePublishedFileSortOrder eOrder ) = 0;
	virtual SteamAPICall_t SubscribePublishedFile( AppId_t nAppId, PublishedFileId_t unPublishedFileId ) = 0;
	virtual SteamAPICall_t EnumerateUserSubscribedFiles( AppId_t nAppId, uint32 uStartIndex, uint8 uListType, EPublishedFileInfoMatchingFileType eMatchingFileType ) = 0;
	virtual SteamAPICall_t UnsubscribePublishedFile( AppId_t nAppId, PublishedFileId_t unPublishedFileId ) = 0;
	
	virtual SteamAPICall_t SetUserPublishedFileAction( AppId_t nAppId, PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction ) = 0;
	virtual SteamAPICall_t EnumeratePublishedFilesByUserAction( AppId_t nAppId, EWorkshopFileAction eAction, uint32 uStartIndex ) = 0;
	virtual SteamAPICall_t EnumerateUserSubscribedFilesWithUpdates( AppId_t nAppId, uint32 uStartIndex, RTime32 uStartTime ) = 0;
	virtual SteamAPICall_t GetCREItemVoteSummary( PublishedFileId_t unPublishedFileId ) = 0;
	virtual SteamAPICall_t UpdateUserPublishedItemVote( PublishedFileId_t unPublishedFileId, bool bVoteUp ) = 0;
	virtual SteamAPICall_t GetUserPublishedItemVoteDetails( PublishedFileId_t unPublishedFileId ) = 0;
	virtual SteamAPICall_t EnumerateUserSharedWorkshopFiles( AppId_t nAppId, CSteamID creatorSteamID, uint32 uStartIndex, SteamParamStringArray_t const *pRequiredTags, SteamParamStringArray_t const *pExcludedTags ) = 0;
	virtual SteamAPICall_t EnumeratePublishedWorkshopFiles( AppId_t nAppId, EWorkshopEnumerationType eType, EPublishedFileInfoMatchingFileType eFileType, uint32 uStartIndex, uint32 cDays, uint32 cCount, SteamParamStringArray_t const *pTags, SteamParamStringArray_t const *pUserTags ) = 0;
	
	virtual EFileRemoteStorageSyncState EGetFileSyncState( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;
	virtual bool BIsFileSyncing( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual EResult FilePersist( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual bool FileFetch( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile ) = 0;

	virtual bool ResolvePath( AppId_t nAppID, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchRelPath, char *pchDest, uint32 cchDest ) = 0;

	virtual EResult FileTouch( AppId_t nAppId, ERemoteStorageFileRoot eRemoteStorageFileRoot, const char *pchFile, bool ) = 0;

	virtual void SetCloudEnabledForAccount( bool bEnabled );

	virtual void LoadLocalFileInfoCache( AppId_t nAppId ) = 0;

	virtual void EvaluateRemoteStorageSyncState( AppId_t nAppId, bool bUnk) = 0;
	virtual ERemoteStorageSyncState GetRemoteStorageSyncState( AppId_t nAppId ) = 0;

	virtual bool HaveLatestFilesLocally( AppId_t nAppId ) = 0;

	virtual bool GetConflictingFileTimestamps( AppId_t nAppId, RTime32* pnTimestampLocal, RTime32* pnTimestampRemote ) = 0;
	virtual bool ResolveSyncConflict( AppId_t nAppId, bool bAcceptLocalFiles ) = 0;

	virtual bool SynchronizeApp( AppId_t nAppId, bool bSyncClient, bool bSyncServer ) = 0;
	virtual bool IsAppSyncInProgress( AppId_t nAppId ) = 0;
	
	virtual void RunAutoCloudOnAppLaunch( AppId_t nAppId ) = 0;
	virtual void RunAutoCloudOnAppExit( AppId_t nAppId ) = 0;
	
	virtual bool ResetFileRequestState( AppId_t nAppId ) = 0;

	virtual void ClearPublishFileUpdateRequests( AppId_t nAppId ) = 0;
	
	virtual int32 GetSubscribedFileDownloadCount() = 0;
	virtual bool BGetSubscribedFileDownloadInfo( int32 iFile, PublishedFileId_t* punPublishedFileId, uint32 *puBytesDownloaded, uint32 *puBytesExpected, AppId_t* pnAppId ) = 0;
	virtual bool BGetSubscribedFileDownloadInfo( PublishedFileId_t unPublishedFileId, uint32 *puBytesDownloaded, uint32 *puBytesExpected, AppId_t* pnAppId ) = 0;
	virtual void PauseSubscribedFileDownloadsForApp( AppId_t nAppId ) = 0;
	virtual void ResumeSubscribedFileDownloadsForApp( AppId_t nAppId ) = 0;
	virtual void PauseAllSubscribedFileDownloads() = 0;
	virtual void ResumeAllSubscribedFileDownloads() = 0;
	virtual void OnAppLifetime( AppId_t nAppId, bool bUnk ) = 0;
};

#endif // ICLIENTREMOTESTORAGE_H
