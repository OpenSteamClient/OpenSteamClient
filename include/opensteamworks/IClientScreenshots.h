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


abstract_class IClientScreenshots
{
public:
    virtual const char *GetShortcutDisplayName( CGameID gameID ) = 0; //argc: 1, index 1
    virtual void SetShortcutDisplayName( CGameID, const char *cszName ) = 0; //argc: 2, index 2
    
    virtual ScreenshotHandle WriteScreenshot( CGameID gameID, const uint8 *pubRGBData, uint32 uRGBDataSize, int32 iWidth, int32 iHeight ) = 0; //argc: 5, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual ScreenshotHandle AddScreenshotToLibrary( CGameID gameID, const char *cszScreenshotPath, const char *cszThumbnailPath, int32 iWidth, int32 iHeight ) = 0; //argc: 7, index 4
    
    virtual void TriggerScreenshot( CGameID gameID ) = 0; //argc: 1, index 5
    virtual void RequestScreenshotFromGame( AppId_t nAppId ) = 0; //argc: 1, index 6
    
    virtual bool SetLocation( CGameID gameID, ScreenshotHandle hScreenshot, const char *cszLocation ) = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool TagUser( CGameID gameID, ScreenshotHandle hScreenshot, CSteamID userID ) = 0; //argc: 4, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool TagPublishedFile( CGameID gameID, ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileId ) = 0; //argc: 4, index 9
    
    virtual bool ResolvePath( CGameID gameID, ScreenshotHandle hScreenshot, bool bUnk, char *szResolvedPath, uint32 cubResolvedPath ) = 0; //argc: 5, index 10
    virtual uint32 GetSizeOnDisk( CGameID gameID, ScreenshotHandle hScreenshot ) = 0; //argc: 2, index 11
    virtual uint32 GetSizeInCloud( CGameID gameID, ScreenshotHandle hScreenshot ) = 0; //argc: 2, index 12
    virtual bool IsPersisted( CGameID gameID, ScreenshotHandle hScreenshot ) = 0; //argc: 2, index 13
    
    virtual int32 GetNumGamesWithLocalScreenshots() = 0; //argc: 0, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual CGameID GetGameWithLocalScreenshots(int32 iGameIndex) = 0; //argc: 2, index 2
    
    virtual int32 GetLocalScreenshotCount( CGameID gameID ) = 0; //argc: 1, index 3
    virtual bool GetLocalScreenshot( CGameID gameID, int32 iScreenshotIndex, ScreenshotHandle* phScreenshot, int32 *piWidth, int32 *piHeight, uint32 *puTimestamp, EUCMFilePrivacyState *pePrivacy, uint64* pullFileID, char *pchCaption, uint32 cubCaption, bool *pbSpoiler ) = 0; //argc: 11, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalScreenshotByHandle() = 0; //argc: 10, index 5
    virtual bool SetLocalScreenshotCaption( CGameID gameID, ScreenshotHandle hScreenshot, const char *cszCaption ) = 0; //argc: 3, index 6
    virtual bool SetLocalScreenshotPrivacy( CGameID gameID, ScreenshotHandle hScreenshot, EUCMFilePrivacyState ePrivacy ) = 0; //argc: 3, index 7
    virtual bool SetLocalScreenshotSpoiler( CGameID, ScreenshotHandle hScreenshot, bool bSpoiler ) = 0; //argc: 3, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalLastScreenshot() = 0; //argc: 2, index 9
    
    virtual bool StartBatch( CGameID gameID ) = 0; //argc: 1, index 10
    virtual bool AddToBatch( ScreenshotHandle hScreenshot ) = 0; //argc: 1, index 11
    virtual SteamAPICall_t UploadBatch( EUCMFilePrivacyState ePrivacy ) = 0; //argc: 1, index 12
    virtual SteamAPICall_t DeleteBatch( bool bDeleteFromCloud ) = 0; //argc: 1, index 13
    virtual bool CancelBatch() = 0; //argc: 0, index 14
    
    virtual void RecoverOldScreenshots() = 0; //argc: 0, index 1
    virtual uint32 GetTaggedUserCount( CGameID gameID, ScreenshotHandle hScreenshot ) = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual CSteamID GetTaggedUser( CGameID gameID, ScreenshotHandle hScreenshot, int32 iUserIndex ) = 0; //argc: 4, index 3
    virtual bool GetLocation( CGameID gameID, ScreenshotHandle hScreenshot, char *pchLocation, uint32 cubLocation ) = 0; //argc: 4, index 4
    
    virtual uint32 GetTaggedPublishedFileCount( CGameID gameID, ScreenshotHandle hScreenshot ) = 0; //argc: 2, index 5
    virtual PublishedFileId_t GetTaggedPublishedFile( CGameID gameID, ScreenshotHandle hScreenshot, int32 iPublishedFileIndex ) = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetScreenshotVRType() = 0; //argc: 2, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BGetUserScreenshotDirectory() = 0; //argc: 2, index 8
};

#endif // ICLIENTSCREENSHOTS_H