//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Enums;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

/// <summary>
/// The main interface for non-steam app management.
/// </summary>
public unsafe interface IClientShortcuts
{
    public AppId_t GetUniqueLocalAppId();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public CGameID GetGameIDForAppID(AppId_t shortcutAppID);  // argc: 2, index: 2, ipc args: [bytes4], ipc returns: [bytes8]
    public AppId_t GetAppIDForGameID(CGameID gameid);  // argc: 1, index: 3, ipc args: [bytes8], ipc returns: [bytes4]
    public AppId_t GetDevkitAppIDByDevkitGameID(string devkitGameID);  // argc: 1, index: 4, ipc args: [string], ipc returns: [bytes4]
    public bool GetShortcutAppIds([ProtobufPtrType(typeof(CMsgShortcutAppIds))] IntPtr nativeptr);  // argc: 1, index: 5, ipc args: [], ipc returns: [bytes1, unknown]
    public bool GetShortcutInfoByIndex(int index, [ProtobufPtrType(typeof(CMsgShortcutInfo))] IntPtr nativeptr);  // argc: 2, index: 6, ipc args: [bytes4], ipc returns: [bytes1, unknown]
    public bool GetShortcutInfoByAppID(AppId_t appid, [ProtobufPtrType(typeof(CMsgShortcutInfo))] IntPtr nativeptr);  // argc: 2, index: 7, ipc args: [bytes4], ipc returns: [bytes1, unknown]
    // WARNING: Arguments are unknown!
    public AppId_t AddShortcut(string name, string executable, string icon, string shortcutPath, string launchOptions);  // argc: 5, index: 8, ipc args: [string, string, string, string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public AppId_t AddTemporaryShortcut(string name, string exepath, string icon);  // argc: 3, index: 9, ipc args: [string, string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public AppId_t AddOpenVRShortcut(string name, string exepath, string icon);  // argc: 3, index: 10, ipc args: [string, string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public void SetShortcutFromFullpath(AppId_t appid, string fullpath);  // argc: 2, index: 11, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetShortcutAppName(AppId_t appid, string name);  // argc: 2, index: 12, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public bool SetShortcutExe(AppId_t appid, string maybepath, int unk);  // argc: 3, index: 13, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public void SetShortcutStartDir(AppId_t appid, string workingdir);  // argc: 2, index: 14, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetShortcutIcon(AppId_t appid, string iconpath);  // argc: 2, index: 15, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetShortcutCommandLine(AppId_t appid, string commandline);  // argc: 2, index: 16, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void ClearShortcutUserTags();  // argc: 1, index: 17, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public void AddShortcutUserTag();  // argc: 2, index: 18, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void RemoveShortcutUserTag();  // argc: 2, index: 19, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public void ClearAndSetShortcutUserTags();  // argc: 2, index: 20, ipc args: [bytes4, utlvector], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetShortcutHidden();  // argc: 2, index: 21, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetAllowDesktopConfig(AppId_t appid, bool allow);  // argc: 2, index: 22, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetAllowOverlay(AppId_t appid, bool allow);  // argc: 2, index: 23, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public void SetOpenVRShortcut(AppId_t appid, bool isVR);  // argc: 2, index: 24, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public bool SetDevkitShortcut(AppId_t appid, string unk, uint unk2);  // argc: 3, index: 25, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1]
    public bool SetFlatpakAppID(AppId_t appid, string flatpakAppID);  // argc: 2, index: 26, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public void RemoveShortcut(AppId_t appid);  // argc: 1, index: 27, ipc args: [bytes4], ipc returns: []
    public void RemoveAllTemporaryShortcuts();  // argc: 0, index: 28, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public EAppError LaunchShortcut(AppId_t appid, int maybeLaunchOption);  // argc: 2, index: 29, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
}