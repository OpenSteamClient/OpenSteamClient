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

#ifndef CONTROLLERCOMMON_H
#define CONTROLLERCOMMON_H
#ifdef _WIN32
#pragma once
#endif

#define STEAMCONTROLLER_INTERFACE_VERSION_001 "STEAMCONTROLLER_INTERFACE_VERSION"

#define STEAM_RIGHT_TRIGGER_MASK            0x0000000000000001l
#define STEAM_LEFT_TRIGGER_MASK             0x0000000000000002l
#define STEAM_RIGHT_BUMPER_MASK             0x0000000000000004l
#define STEAM_LEFT_BUMPER_MASK              0x0000000000000008l
#define STEAM_BUTTON_0_MASK                 0x0000000000000010l
#define STEAM_BUTTON_1_MASK                 0x0000000000000020l
#define STEAM_BUTTON_2_MASK                 0x0000000000000040l
#define STEAM_BUTTON_3_MASK                 0x0000000000000080l
#define STEAM_TOUCH_0_MASK                  0x0000000000000100l
#define STEAM_TOUCH_1_MASK                  0x0000000000000200l
#define STEAM_TOUCH_2_MASK                  0x0000000000000400l
#define STEAM_TOUCH_3_MASK                  0x0000000000000800l
#define STEAM_BUTTON_MENU_MASK              0x0000000000001000l
#define STEAM_BUTTON_STEAM_MASK             0x0000000000002000l
#define STEAM_BUTTON_ESCAPE_MASK            0x0000000000004000l
#define STEAM_BUTTON_BACK_LEFT_MASK         0x0000000000008000l
#define STEAM_BUTTON_BACK_RIGHT_MASK        0x0000000000010000l
#define STEAM_BUTTON_LEFTPAD_CLICKED_MASK   0x0000000000020000l
#define STEAM_BUTTON_RIGHTPAD_CLICKED_MASK  0x0000000000040000l
#define STEAM_LEFTPAD_FINGERDOWN_MASK       0x0000000000080000l
#define STEAM_RIGHTPAD_FINGERDOWN_MASK      0x0000000000100000l

struct SteamControllerState_t
{
	// If packet num matches that on your prior call, then the controller state hasn't been changed since 
	// your last call and there is no need to process it
	uint32 unPacketNum;

	// bit flags for each of the buttons
	uint64 ulButtons;

	// Left pad coordinates
	short sLeftPadX;
	short sLeftPadY;

	// Right pad coordinates
	short sRightPadX;
	short sRightPadY;

};

enum ESteamControllerPad
{
	k_ESteamControllerPad_Left,
	k_ESteamControllerPad_Right
};

#endif
