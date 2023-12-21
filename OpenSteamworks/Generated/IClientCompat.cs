//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientCompat
{
    public bool BIsCompatLayerEnabled();  // argc: 0, index: 1
    public void EnableCompat(bool enable);  // argc: 1, index: 2
    [BlacklistedInCrossProcessIPC]
    public void GetAvailableCompatTools(CUtlStringList* compatTools);  // argc: 1, index: 3
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAvailableCompatToolsFiltered(CUtlStringList* compatTools, ERemoteStoragePlatform platform);  // argc: 2, index: 4
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAvailableCompatToolsForApp(CUtlStringList* compatTools, AppId_t appid);  // argc: 2, index: 5
    public unknown_ret SpecifyCompatTool(AppId_t appid, string toolName, string unk, int priority);  // argc: 4, index: 6
    public bool BIsCompatibilityToolEnabled(AppId_t appid);  // argc: 1, index: 7
    public string GetCompatToolName(AppId_t app);  // argc: 1, index: 8
    public int GetCompatToolMappingPriority(AppId_t appid);  // argc: 1, index: 9
    public string GetCompatToolDisplayName(string name);  // argc: 1, index: 10'
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetWhitelistedGameList(CUtlVector<AppWhitelistSetting_t>* compatTools);  // argc: 1, index: 11
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerConfigOverrides();  // argc: 1, index: 12
    public UInt64 StartSession(AppId_t appid);  // argc: 1, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseSession(AppId_t appid, UInt64 sessionid);  // argc: 3, index: 14
    public bool BIsLauncherServiceEnabled(AppId_t appid);  // argc: 1, index: 15
    public unknown_ret DeleteCompatData(AppId_t appid);  // argc: 1, index: 16
    public unknown_ret GetCompatibilityDataDiskSize(AppId_t appid);  // argc: 1, index: 17
    public bool BNeedsUnlockH264(AppId_t appid);  // argc: 1, index: 18
}