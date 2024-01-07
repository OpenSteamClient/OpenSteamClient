//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Enums;

using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientCompat
{
    public bool BIsCompatLayerEnabled();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public void EnableCompat(bool enable);  // argc: 1, index: 0, ipc args: [bytes1], ipc returns: []
    [BlacklistedInCrossProcessIPC]
    public void GetAvailableCompatTools(CUtlStringList* compatTools);  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAvailableCompatToolsFiltered(CUtlStringList* compatTools, ERemoteStoragePlatform platform);  // argc: 2, index: 2, ipc args: [bytes4, bytes4], ipc returns: []
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAvailableCompatToolsForApp(CUtlStringList* compatTools, AppId_t appid);  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: []
    public unknown_ret SpecifyCompatTool(AppId_t appid, string toolName, string unk, int priority);  // argc: 4, index: 4, ipc args: [bytes4, string, string, bytes4], ipc returns: []
    public bool BIsCompatibilityToolEnabled(AppId_t appid);  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [boolean]
    public string GetCompatToolName(AppId_t app);  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [string]
    public int GetCompatToolMappingPriority(AppId_t appid);  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: [bytes4]
    public string GetCompatToolDisplayName(string name);  // argc: 1, index: 8, ipc args: [string], ipc returns: [string]
    [BlacklistedInCrossProcessIPC]
    public bool GetWhitelistedGameList(CUtlVector<AppWhitelistSetting_t>* compatTools);  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerConfigOverrides();  // argc: 1, index: 10, ipc args: [bytes4], ipc returns: []
    public UInt64 StartSession(AppId_t appid);  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseSession(AppId_t appid, UInt64 sessionid);  // argc: 3, index: 12, ipc args: [bytes4, bytes8], ipc returns: []
    public bool BIsLauncherServiceEnabled(AppId_t appid);  // argc: 1, index: 13, ipc args: [bytes4], ipc returns: [boolean]
    public unknown_ret DeleteCompatData(AppId_t appid);  // argc: 1, index: 14, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetCompatibilityDataDiskSize(AppId_t appid);  // argc: 1, index: 15, ipc args: [bytes4], ipc returns: [bytes8]
    public bool BNeedsUnlockH264(AppId_t appid);  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: [boolean]
}