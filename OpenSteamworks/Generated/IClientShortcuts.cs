//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

/// <summary>
/// The main interface for non-steam app management.
/// </summary>
public unsafe interface IClientShortcuts
{
    public AppId_t GetUniqueLocalAppId();  // argc: 0, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret GetGameIDForAppID();  // argc: 2, index: 2
    // WARNING: Arguments are unknown!
    public AppId_t GetAppIDForGameID(CGameID gameid);  // argc: 1, index: 3
    // WARNING: Arguments are unknown!
    public AppId_t GetDevkitAppIDByDevkitGameID(CGameID gameid);  // argc: 1, index: 4
    public bool GetShortcutAppIds(IntPtr CMsgShortcutAppIds_nativeptr);  // argc: 1, index: 5
    public bool GetShortcutInfoByIndex(int index, IntPtr CMsgShortcutInfo_nativeptr);  // argc: 2, index: 6
    public bool GetShortcutInfoByAppID(AppId_t appid, IntPtr CMsgShortcutInfo_nativeptr);  // argc: 2, index: 7
    // WARNING: Arguments are unknown!
    public AppId_t AddShortcut(string name, string executable, string icon, string shortcutPath, string launchOptions);  // argc: 5, index: 8
    // WARNING: Arguments are unknown!
    public AppId_t AddTemporaryShortcut(string name, string exepath, string icon);  // argc: 3, index: 9
    // WARNING: Arguments are unknown!
    public AppId_t AddOpenVRShortcut(string name, string exepath, string icon);  // argc: 3, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutFromFullpath(AppId_t appid, string fullpath);  // argc: 2, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutAppName(AppId_t appid, string name);  // argc: 2, index: 12
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetShortcutExe(AppId_t appid, string maybepath, int unk);  // argc: 3, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutStartDir(AppId_t appid, string workingdir);  // argc: 2, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutIcon(AppId_t appid, string iconpath);  // argc: 2, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutCommandLine(AppId_t appid, string commandline);  // argc: 2, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret ClearShortcutUserTags();  // argc: 1, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret AddShortcutUserTag();  // argc: 2, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveShortcutUserTag();  // argc: 2, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret ClearAndSetShortcutUserTags();  // argc: 2, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutHidden();  // argc: 2, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowDesktopConfig(AppId_t appid, bool allow);  // argc: 2, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowOverlay(AppId_t appid, bool allow);  // argc: 2, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret SetOpenVRShortcut(AppId_t appid, bool isVR);  // argc: 2, index: 24
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetDevkitShortcut(AppId_t appid, bool isDevkit, CGameID gameid);  // argc: 3, index: 25
    public bool SetFlatpakAppID(AppId_t appid, string flatpakAppID);  // argc: 2, index: 26
    // WARNING: Arguments are unknown!
    public void RemoveShortcut(AppId_t appid);  // argc: 1, index: 27
    public void RemoveAllTemporaryShortcuts();  // argc: 0, index: 28
    // WARNING: Arguments are unknown!
    public ulong LaunchShortcut(AppId_t appid);  // argc: 2, index: 29
}