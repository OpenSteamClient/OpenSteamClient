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

#ifndef ICLIENTSHORTCUTS_H
#define ICLIENTSHORTCUTS_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

#define CLIENTSHORTCUTS_INTERFACE_VERSION "CLIENTSHORTCUTS_INTERFACE_VERSION001"

struct SteamParamStringArray_t
{
	SteamParamStringArray_t()
	{
		m_ppStrings = NULL;
		m_nNumStrings = 0;
	}

	const char ** m_ppStrings;
	int32 m_nNumStrings;
};

typedef void ShortcutInfo;

abstract_class IClientShortcuts
{
public:
    virtual AppId_t GetUniqueLocalAppId() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual CGameID GetGameIDForAppID() = 0; //argc: 2, index 1
    virtual AppId_t GetAppIDForGameID(CGameID) = 0; //argc: 1, index 2
    virtual AppId_t GetDevkitAppIDByDevkitGameID(CGameID) = 0; //argc: 1, index 3
    virtual unknown_ret GetShortcutAppIds(CUtlVector<AppId_t>* out) = 0; //argc: 1, index 4
    virtual unknown_ret GetShortcutInfoByIndex(int index, ShortcutInfo*) = 0; //argc: 2, index 5
    virtual unknown_ret GetShortcutInfoByAppID(AppId_t, ShortcutInfo*) = 0; //argc: 2, index 6
    virtual AppId_t AddShortcut(const char *szShortcutName, const char *szShortcutExe, const char *szUnk1, const char *szUnk2, const char *szhUnk3) = 0; //argc: 5, index 7
    virtual AppId_t AddTemporaryShortcut(const char *, const char *, const char *) = 0; //argc: 3, index 8
    virtual AppId_t AddOpenVRShortcut(const char *, const char *, const char *) = 0; //argc: 3, index 9
    virtual void SetShortcutFromFullpath( AppId_t unAppID, const char * szPath ) = 0; //argc: 2, index 10
    virtual void SetShortcutAppName( AppId_t unAppID, const char * szAppName ) = 0; //argc: 2, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetShortcutExe( AppId_t unAppID, const char * szExePath ) = 0; //argc: 3, index 12
    virtual void SetShortcutStartDir( AppId_t unAppID, const char * szPath ) = 0; //argc: 2, index 13
    virtual void SetShortcutIcon( AppId_t unAppID, const char * szIconPath ) = 0; //argc: 2, index 14
    virtual void SetShortcutCommandLine( AppId_t unAppID, const char * szCommandLine ) = 0; //argc: 2, index 15
    virtual void ClearShortcutUserTags( AppId_t unAppID ) = 0; //argc: 1, index 16
    virtual void AddShortcutUserTag( AppId_t unAppID, const char * szTag) = 0; //argc: 2, index 17
    virtual void RemoveShortcutUserTag( AppId_t unAppID, const char * szTag) = 0; //argc: 2, index 18
    virtual void ClearAndSetShortcutUserTags( AppId_t unAppID, const SteamParamStringArray_t *) = 0; //argc: 2, index 19
    virtual void SetShortcutHidden( AppId_t unAppID, bool bHidden ) = 0; //argc: 2, index 20
    virtual void SetAllowDesktopConfig( uint32, bool ) = 0; //argc: 2, index 21
    virtual void SetAllowOverlay( AppId_t unAppID, bool ) = 0; //argc: 2, index 22
    virtual void SetOpenVRShortcut( AppId_t unAppID, bool bOpenVRShortcut ) = 0; //argc: 2, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetDevkitShortcut( AppId_t unAppID, const char * pchUnk ) = 0; //argc: 3, index 24
    virtual void SetFlatpakAppID( AppId_t unAppID, const char * pchUnk ) = 0; //argc: 2, index 25
    virtual void RemoveShortcut( AppId_t unAppID ) = 0; //argc: 1, index 26
    virtual void RemoveAllTemporaryShortcuts() = 0; //argc: 0, index 27
    virtual bool LaunchShortcut( AppId_t unAppID, uint32 uUnk ) = 0; //argc: 2, index 1
};

#endif // ICLIENTSHORTCUTS_H