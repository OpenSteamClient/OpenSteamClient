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

#ifndef ICLIENTSCREENSHOTS_H
#define ICLIENTSCREENSHOTS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "ScreenshotsCommon.h"


abstract_class UNSAFE_INTERFACE IClientScreenshots
{
public:
	virtual const char *GetShortcutDisplayName( CGameID gameID ) = 0;
	virtual void SetShortcutDisplayName( CGameID, const char *cszName ) = 0;

	virtual ScreenshotHandle WriteScreenshot( CGameID gameID, const uint8 *pubRGBData, uint32 uRGBDataSize, int32 iWidth, int32 iHeight ) = 0;
	virtual ScreenshotHandle AddScreenshotToLibrary( CGameID gameID, const char *cszScreenshotPath, const char *cszThumbnailPath, int32 iWidth, int32 iHeight ) = 0;

	virtual void TriggerScreenshot( CGameID gameID ) = 0;
	virtual void RequestScreenshotFromGame( AppId_t nAppId ) = 0;

	virtual bool SetLocation( CGameID gameID, ScreenshotHandle hScreenshot, const char *cszLocation ) = 0;
	virtual bool TagUser( CGameID gameID, ScreenshotHandle hScreenshot, CSteamID userID ) = 0;
	virtual bool TagPublishedFile( CGameID gameID, ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileId ) = 0;
	
	virtual bool ResolvePath( CGameID gameID, ScreenshotHandle hScreenshot, bool bUnk, char *szResolvedPath, uint32 cubResolvedPath ) = 0;
	virtual uint32 GetSizeOnDisk( CGameID gameID, ScreenshotHandle hScreenshot ) = 0;
	virtual uint32 GetSizeInCloud( CGameID gameID, ScreenshotHandle hScreenshot ) = 0;
	virtual bool IsPersisted( CGameID gameID, ScreenshotHandle hScreenshot ) = 0;

	virtual int32 GetNumGamesWithLocalScreenshots() = 0;
	STEAMWORKS_STRUCT_RETURN_1(CGameID, GetGameWithLocalScreenshots, int32, iGameIndex) /*virtual CGameID GetGameWithLocalScreenshots( int32 iGameIndex ) = 0;*/

	virtual int32 GetLocalScreenshotCount( CGameID gameID ) = 0;
	virtual bool GetLocalScreenshot( CGameID gameID, int32 iScreenshotIndex, ScreenshotHandle* phScreenshot, int32 *piWidth, int32 *piHeight, uint32 *puTimestamp, EUCMFilePrivacyState *pePrivacy, uint64* pullFileID, char *pchCaption, uint32 cubCaption, bool *pbSpoiler ) = 0;
	virtual bool SetLocalScreenshotCaption( CGameID gameID, ScreenshotHandle hScreenshot, const char *cszCaption ) = 0;
	virtual bool SetLocalScreenshotPrivacy( CGameID gameID, ScreenshotHandle hScreenshot, EUCMFilePrivacyState ePrivacy ) = 0;
	virtual bool SetLocalScreenshotSpoiler( CGameID, ScreenshotHandle hScreenshot, bool bSpoiler ) = 0;

	virtual bool StartBatch( CGameID gameID ) = 0;
	virtual bool AddToBatch( ScreenshotHandle hScreenshot ) = 0;
	virtual SteamAPICall_t UploadBatch( EUCMFilePrivacyState ePrivacy ) = 0;
	virtual SteamAPICall_t DeleteBatch( bool bDeleteFromCloud ) = 0;
	virtual bool CancelBatch() = 0;

	virtual void RecoverOldScreenshots() = 0;
	virtual uint32 GetTaggedUserCount( CGameID gameID, ScreenshotHandle hScreenshot ) = 0;
	STEAMWORKS_STRUCT_RETURN_3(CSteamID, GetTaggedUser, CGameID, gameID, ScreenshotHandle, hScreenshot, int32, iUserIndex) /*virtual CSteamID GetTaggedUser( CGameID gameID, ScreenshotHandle hScreenshot, int32 iUserIndex ) = 0;*/
	virtual bool GetLocation( CGameID gameID, ScreenshotHandle hScreenshot, char *pchLocation, uint32 cubLocation ) = 0;
	
	virtual uint32 GetTaggedPublishedFileCount( CGameID gameID, ScreenshotHandle hScreenshot ) = 0;
	virtual PublishedFileId_t GetTaggedPublishedFile( CGameID gameID, ScreenshotHandle hScreenshot, int32 iPublishedFileIndex ) = 0;
};

#endif // ICLIENTSCREENSHOTS_H
