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

abstract_class UNSAFE_INTERFACE IClientShortcuts
{
public:
	virtual AppId_t GetUniqueLocalAppId() = 0;
    virtual unknown_ret GetGameIDForAppID(void*,void*) = 0;
    virtual unknown_ret GetAppIDForGameID(void*) = 0;
    virtual unknown_ret GetDevkitAppIDByDevkitGameID(void*) = 0;
    virtual uint32 GetShortcutAppIds(AppId_t *pvecAppID) = 0;
    virtual unknown_ret GetShortcutInfoByIndex(void*,void*) = 0;
    virtual unknown_ret GetShortcutInfoByAppID(AppId_t,void*) = 0;
    virtual unknown_ret AddShortcut(void*,void*,void*,void*,void*) = 0;
    virtual unknown_ret AddTemporaryShortcut(void*,void*,void*) = 0;
    virtual unknown_ret AddOpenVRShortcut(void*,void*,void*) = 0;
    virtual unknown_ret SetShortcutFromFullpath(void*,void*) = 0;
    virtual unknown_ret SetShortcutAppName(void*,void*) = 0;
    virtual unknown_ret SetShortcutExe(void*,void*,void*) = 0;
    virtual unknown_ret SetShortcutStartDir(void*,void*) = 0;
    virtual unknown_ret SetShortcutIcon(void*,void*) = 0;
    virtual unknown_ret SetShortcutCommandLine(void*,void*) = 0;
    virtual unknown_ret ClearShortcutUserTags(void*) = 0;
    virtual unknown_ret AddShortcutUserTag(void*,void*) = 0;
    virtual unknown_ret RemoveShortcutUserTag(void*,void*) = 0;
    virtual unknown_ret ClearAndSetShortcutUserTags(void*,void*) = 0;
    virtual unknown_ret SetShortcutHidden(void*,void*) = 0;
    virtual unknown_ret SetAllowDesktopConfig(void*,void*) = 0;
    virtual unknown_ret SetAllowOverlay(void*,void*) = 0;
    virtual unknown_ret SetOpenVRShortcut(void*,void*) = 0;
    virtual unknown_ret SetDevkitShortcut(void*,void*,void*) = 0;
    virtual unknown_ret SetFlatpakAppID(void*,void*) = 0;
    virtual unknown_ret RemoveShortcut(void*) = 0;
    virtual unknown_ret RemoveAllTemporaryShortcuts() = 0;
    virtual unknown_ret LaunchShortcut(void*,void*) = 0;
};

#endif // ICLIENTSHORTCUTS_H
