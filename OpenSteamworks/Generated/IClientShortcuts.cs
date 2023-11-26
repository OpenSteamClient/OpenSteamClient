//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
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
    public unknown_ret GetAppIDForGameID();  // argc: 1, index: 3
    // WARNING: Arguments are unknown!
    public AppId_t GetDevkitAppIDByDevkitGameID(CGameID gameid);  // argc: 1, index: 4
    /// <summary>
    /// 
    /// </summary>
    /// <param name="arr">Array out</param>
    /// <returns>The length of the array</returns>
    public UInt32 GetShortcutAppIds(out AppId_t *arr);  // argc: 1, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret GetShortcutInfoByIndex();  // argc: 2, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret GetShortcutInfoByAppID();  // argc: 2, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret AddShortcut();  // argc: 5, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret AddTemporaryShortcut();  // argc: 3, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret AddOpenVRShortcut();  // argc: 3, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutFromFullpath();  // argc: 2, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutAppName();  // argc: 2, index: 12
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetShortcutExe();  // argc: 3, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutStartDir();  // argc: 2, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutIcon();  // argc: 2, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutCommandLine();  // argc: 2, index: 16
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
    public unknown_ret SetAllowDesktopConfig();  // argc: 2, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowOverlay();  // argc: 2, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret SetOpenVRShortcut();  // argc: 2, index: 24
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetDevkitShortcut();  // argc: 3, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret SetFlatpakAppID();  // argc: 2, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveShortcut();  // argc: 1, index: 27
    public unknown_ret RemoveAllTemporaryShortcuts();  // argc: 0, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchShortcut();  // argc: 2, index: 29
}