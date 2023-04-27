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

#ifndef ICLIENTHTML_H
#define ICLIENTHTML_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class UNSAFE_INTERFACE IClientHTML
{
public:
    virtual unknown_ret Fun01() = 0;
    virtual unknown_ret Fun02() = 0;
    virtual unknown_ret Init() = 0;
    virtual unknown_ret Shutdown() = 0;
    virtual unknown_ret CreateBrowser(char const*, char const*) = 0;
    virtual unknown_ret RemoveBrowser(unsigned int) = 0;
    virtual unknown_ret AllowStartRequest(unsigned int, bool) = 0;
    virtual unknown_ret LoadURL(unsigned int, char const*, char const*) = 0;
    virtual unknown_ret SetSize(unsigned int, unsigned int, unsigned int) = 0;
    virtual unknown_ret StopLoad(unsigned int) = 0;
    virtual unknown_ret Reload(unsigned int) = 0;
    virtual unknown_ret GoBack(unsigned int) = 0;
    virtual unknown_ret GoForward(unsigned int) = 0;
    virtual unknown_ret AddHeader(unsigned int, char const*, char const*) = 0;
    virtual unknown_ret ExecuteJavascript(unsigned int, char const*) = 0;
    virtual unknown_ret MouseUp(unsigned int, int) = 0;
    virtual unknown_ret MouseDown(unsigned int, int) = 0;
    virtual unknown_ret MouseDoubleClick(unsigned int, int) = 0;
    virtual unknown_ret MouseMove(unsigned int, int, int) = 0;
    virtual unknown_ret MouseWheel(unsigned int, int) = 0;
    virtual unknown_ret KeyDown(unsigned int, unsigned int, int, bool) = 0;
    virtual unknown_ret KeyUp(unsigned int, unsigned int, int) = 0;
    virtual unknown_ret KeyChar(unsigned int, unsigned int, int) = 0;
    virtual unknown_ret SetHorizontalScroll(unsigned int, unsigned int) = 0;
    virtual unknown_ret SetVerticalScroll(unsigned int, unsigned int) = 0;
    virtual unknown_ret SetKeyFocus(unsigned int, bool) = 0;
    virtual unknown_ret ViewSource(unsigned int) = 0;
    virtual unknown_ret CopyToClipboard(unsigned int) = 0;
    virtual unknown_ret PasteFromClipboard(unsigned int) = 0;
    virtual unknown_ret Find(unsigned int, char const*, bool, bool) = 0;
    virtual unknown_ret StopFind(unsigned int) = 0;
    virtual unknown_ret GetLinkAtPosition(unsigned int, int, int) = 0;
    virtual unknown_ret JSDialogResponse(unsigned int, bool) = 0;
    virtual unknown_ret FileLoadDialogResponse(unsigned int, char const**) = 0;
    virtual unknown_ret SetCookie(char const*, char const*, char const*, char const*, unsigned int, bool, bool) = 0;
    virtual unknown_ret SetPageScaleFactor(unsigned int, float, int, int) = 0;
    virtual unknown_ret SetBackgroundMode(unsigned int, bool) = 0;
    virtual unknown_ret SetDPIScalingFactor(unsigned int, float) = 0;
    virtual unknown_ret OpenDeveloperTools(unsigned int) = 0;
    virtual unknown_ret Validate(void*, char const*) = 0;
};

#endif // ICLIENTHTML_H
