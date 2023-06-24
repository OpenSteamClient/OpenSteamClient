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

#ifndef ICLIENTUGC_H
#define ICLIENTUGC_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "UGCCommon.h"
#include "RemoteStorageCommon.h"

#define CLIENTUGC_INTERFACE_VERSION "CLIENTUGC_INTERFACE_VERSION001"

abstract_class IClientUGC
{
public:
    virtual UGCQueryHandle_t CreateQueryUserUGCRequest( AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint32 unPage ) = 0; //argc: 7, index 1
    virtual UGCQueryHandle_t CreateQueryAllUGCRequest( EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint32 unPage ) = 0; //argc: 5, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CreateQueryAllUGCRequest() = 0; //argc: 5, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CreateQueryUGCDetailsRequest() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t SendQueryUGCRequest( UGCQueryHandle_t handle ) = 0; //argc: 2, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool GetQueryUGCResult( UGCQueryHandle_t handle, uint32 index, SteamUGCDetails_t *pDetails ) = 0; //argc: 4, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCNumTags() = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCTag() = 0; //argc: 6, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCTagDisplayName() = 0; //argc: 6, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCPreviewURL() = 0; //argc: 5, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCImageURL() = 0; //argc: 7, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCMetadata() = 0; //argc: 5, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCChildren() = 0; //argc: 5, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCStatistic() = 0; //argc: 5, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCNumAdditionalPreviews() = 0; //argc: 3, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCAdditionalPreview() = 0; //argc: 9, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCNumKeyValueTags() = 0; //argc: 3, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCKeyValueTag() = 0; //argc: 8, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCKeyValueTag() = 0; //argc: 6, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCContentDescriptors() = 0; //argc: 5, index 20
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueryUGCIsDepotBuild() = 0; //argc: 4, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool ReleaseQueryUGCRequest( UGCQueryHandle_t handle ) = 0; //argc: 2, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool AddRequiredTag( UGCQueryHandle_t handle, const char *pTagName ) = 0; //argc: 3, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddRequiredTagGroup() = 0; //argc: 3, index 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool AddExcludedTag( UGCQueryHandle_t handle, const char *pTagName ) = 0; //argc: 3, index 25
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetReturnOnlyIDs() = 0; //argc: 3, index 26
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetReturnKeyValueTags() = 0; //argc: 3, index 27
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetReturnLongDescription( UGCQueryHandle_t handle, bool bReturnLongDescription ) = 0; //argc: 3, index 28
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetReturnMetadata() = 0; //argc: 3, index 29
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetReturnChildren() = 0; //argc: 3, index 30
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetReturnAdditionalPreviews() = 0; //argc: 3, index 31
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetReturnTotalOnly( UGCQueryHandle_t handle, bool bReturnTotalOnly ) = 0; //argc: 3, index 32
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetReturnPlaytimeStats() = 0; //argc: 3, index 33
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetLanguage() = 0; //argc: 3, index 34
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetAllowCachedResponse( UGCQueryHandle_t handle, uint32 ) = 0; //argc: 3, index 35
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetCloudFileNameFilter( UGCQueryHandle_t handle, const char *pMatchCloudFileName ) = 0; //argc: 3, index 36
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetMatchAnyTag( UGCQueryHandle_t handle, bool bMatchAnyTag ) = 0; //argc: 3, index 37
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetSearchText( UGCQueryHandle_t handle, const char *pSearchText ) = 0; //argc: 3, index 38
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetRankedByTrendDays( UGCQueryHandle_t handle, uint32 unDays ) = 0; //argc: 3, index 39
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetTimeCreatedDateRange() = 0; //argc: 4, index 40
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetTimeUpdatedDateRange() = 0; //argc: 4, index 41
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddRequiredKeyValueTag() = 0; //argc: 4, index 42
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual SteamAPICall_t RequestUGCDetails( PublishedFileId_t nPublishedFileID ) = 0; //argc: 3, index 43
    virtual uint64 CreateItem( uint32, EWorkshopFileType ) = 0; //argc: 2, index 44
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual uint64 StartItemUpdate( uint32, uint64 ) = 0; //argc: 3, index 45
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetItemTitle( uint64, const char * ) = 0; //argc: 3, index 46
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetItemDescription( uint64, const char * ) = 0; //argc: 3, index 47
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetItemUpdateLanguage() = 0; //argc: 3, index 48
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetItemMetadata() = 0; //argc: 3, index 49
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetItemVisibility( uint64, ERemoteStoragePublishedFileVisibility ) = 0; //argc: 3, index 50
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetItemTags( uint64, SteamParamStringArray_t  const* ) = 0; //argc: 3, index 51
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetItemContent( uint64, uint64 ) = 0; //argc: 3, index 52
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetItemPreview( uint64, uint64 ) = 0; //argc: 3, index 53
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetAllowLegacyUpload() = 0; //argc: 3, index 54
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveAllItemKeyValueTags() = 0; //argc: 2, index 55
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveItemKeyValueTags() = 0; //argc: 3, index 56
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddItemKeyValueTag() = 0; //argc: 4, index 57
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddItemPreviewFile() = 0; //argc: 4, index 58
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddItemPreviewVideo() = 0; //argc: 3, index 59
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateItemPreviewFile() = 0; //argc: 4, index 60
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateItemPreviewVideo() = 0; //argc: 4, index 61
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveItemPreview() = 0; //argc: 3, index 62
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddContentDescriptor() = 0; //argc: 3, index 63
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveContentDescriptor() = 0; //argc: 3, index 64
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SubmitItemUpdate() = 0; //argc: 3, index 65
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemUpdateProgress() = 0; //argc: 4, index 66
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetUserItemVote() = 0; //argc: 3, index 67
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetUserItemVote() = 0; //argc: 2, index 68
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddItemToFavorites() = 0; //argc: 3, index 69
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveItemFromFavorites() = 0; //argc: 3, index 70
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual uint64 SubscribeItem( uint32, uint64 ) = 0; //argc: 4, index 71
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual uint64 UnsubscribeItem( uint32, uint64 ) = 0; //argc: 3, index 72
    virtual uint32 GetNumSubscribedItems( uint32 ) = 0; //argc: 1, index 73
    virtual uint32 GetSubscribedItems( uint32, uint64 *, uint32 ) = 0; //argc: 3, index 74
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemState() = 0; //argc: 3, index 75
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemInstallInfo() = 0; //argc: 7, index 76
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemDownloadInfo() = 0; //argc: 5, index 77
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DownloadItem() = 0; //argc: 4, index 78
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAppItemsStatus() = 0; //argc: 3, index 79
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BInitWorkshopForGameServer() = 0; //argc: 3, index 80
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SuspendDownloads() = 0; //argc: 2, index 81
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAllItemsSizeOnDisk() = 0; //argc: 1, index 82
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StartPlaytimeTracking() = 0; //argc: 3, index 83
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StopPlaytimeTracking() = 0; //argc: 3, index 84
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StopPlaytimeTrackingForAllItems() = 0; //argc: 1, index 85
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddDependency() = 0; //argc: 4, index 86
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveDependency() = 0; //argc: 4, index 87
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AddAppDependency() = 0; //argc: 3, index 88
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveAppDependency() = 0; //argc: 3, index 89
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAppDependencies() = 0; //argc: 2, index 90
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DeleteItem() = 0; //argc: 2, index 91
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ShowWorkshopEULA() = 0; //argc: 0, index 92
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetWorkshopEULAStatus() = 0; //argc: 0, index 93
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetNumDownloadedItems() = 0; //argc: 1, index 94
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetDownloadedItems() = 0; //argc: 3, index 95
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetFullQueryUGCResponse() = 0; //argc: 3, index 96
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetSerializedQueryUGCResponse() = 0; //argc: 3, index 97
};

#endif // ICLIENTUGC_H