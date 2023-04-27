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

abstract_class UNSAFE_INTERFACE IClientUGC
{
public:
	virtual UGCQueryHandle_t CreateQueryUserUGCRequest( AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint32 unPage ) = 0;
	virtual UGCQueryHandle_t CreateQueryAllUGCRequest( EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint32 unPage ) = 0;
	virtual SteamAPICall_t SendQueryUGCRequest( UGCQueryHandle_t handle ) = 0;
	virtual bool GetQueryUGCResult( UGCQueryHandle_t handle, uint32 index, SteamUGCDetails_t *pDetails ) = 0;
	virtual bool ReleaseQueryUGCRequest( UGCQueryHandle_t handle ) = 0;
	virtual bool AddRequiredTag( UGCQueryHandle_t handle, const char *pTagName ) = 0;
	virtual bool AddExcludedTag( UGCQueryHandle_t handle, const char *pTagName ) = 0;
	virtual bool SetReturnLongDescription( UGCQueryHandle_t handle, bool bReturnLongDescription ) = 0;
	virtual bool SetReturnTotalOnly( UGCQueryHandle_t handle, bool bReturnTotalOnly ) = 0;
	virtual bool SetAllowCachedResponse( UGCQueryHandle_t handle, uint32 ) = 0;
	virtual bool SetCloudFileNameFilter( UGCQueryHandle_t handle, const char *pMatchCloudFileName ) = 0;
	virtual bool SetMatchAnyTag( UGCQueryHandle_t handle, bool bMatchAnyTag ) = 0;
	virtual bool SetSearchText( UGCQueryHandle_t handle, const char *pSearchText ) = 0;
	virtual bool SetRankedByTrendDays( UGCQueryHandle_t handle, uint32 unDays ) = 0;
	virtual SteamAPICall_t RequestUGCDetails( PublishedFileId_t nPublishedFileID ) = 0;
	virtual uint64 CreateItem( uint32, EWorkshopFileType ) = 0;
	virtual uint64 UploadItemContent( uint64, const char * ) = 0;
	virtual uint64 UploadItemPreview( uint64, const char * ) = 0;
	virtual uint64 StartItemUpdate( uint32, uint64 ) = 0;
	virtual bool SetItemTitle( uint64, const char * ) = 0;
	virtual bool SetItemChangeDescription( uint64, const char * ) = 0;
	virtual bool SetItemDescription( uint64, const char * ) = 0;
	virtual bool SetItemVisibility( uint64, ERemoteStoragePublishedFileVisibility ) = 0;
	virtual bool SetItemTags( uint64, SteamParamStringArray_t  const* ) = 0;
	virtual bool SetItemContent( uint64, uint64 ) = 0;
	virtual bool SetItemPreview( uint64, uint64 ) = 0;
	virtual uint64 CommitItemUpdate( uint64 ) = 0;
	virtual uint64 SubscribeItem( uint32, uint64 ) = 0;
	virtual uint64 UnsubscribeItem( uint32, uint64 ) = 0;
	virtual uint32 GetNumSubscribedItems( uint32 ) = 0;
	virtual uint32 GetSubscribedItems( uint32, uint64 *, uint32 ) = 0;
	virtual bool GetSubscribedItemInfo( uint32, uint64, char *, int32, char *, int32, EWorkshopFileType *, bool *, char *, int32, uint64 * ) = 0;
	};

#endif // ICLIENTUGC_H
