//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientShader
{
    public bool BIsShaderManagementEnabled();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public bool BIsShaderBackgroundProcessingEnabled();  // argc: 0, index: 2, ipc args: [], ipc returns: [boolean]
    public void EnableShaderManagement(bool enable);  // argc: 1, index: 3, ipc args: [bytes1], ipc returns: []
    public void EnableShaderBackgroundProcessing(bool enable);  // argc: 1, index: 4, ipc args: [bytes1], ipc returns: []
    public UInt64 GetShaderDepotsTotalDiskUsage();  // argc: 0, index: 5, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public UInt64 GetShaderCacheDiskSize(AppId_t appid);  // argc: 3, index: 6, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret StartShaderScan(AppId_t appid, string unk);  // argc: 2, index: 7, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartPipelineBuild(AppId_t appid, int unk);  // argc: 2, index: 8, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartShaderConversion(AppId_t appid, UInt64 unk, string unk2);  // argc: 4, index: 9, ipc args: [bytes4, bytes8, string], ipc returns: [bytes8]
    public void StartShaderPruning();  // argc: 0, index: 10, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool ProcessShaderCache(AppId_t appid);  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: [bytes1]
    public UInt32 GetShaderCacheProcessingCompletion();  // argc: 0, index: 12, ipc args: [], ipc returns: [bytes4]
    public AppId_t GetShaderCacheProcessingAppID();  // argc: 0, index: 13, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public void SkipShaderProcessing(AppId_t appid);  // argc: 1, index: 14, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool BAppHasPendingShaderContentDownload(AppId_t appid);  // argc: 1, index: 15, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppPendingShaderDownloadSize(AppId_t appid);  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CheckDepotManifestID();  // argc: 1, index: 17, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBucketManifest();  // argc: 3, index: 18, ipc args: [bytes4, string, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetStaleBucket();  // argc: 2, index: 19, ipc args: [string, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ReportExternalBuild();  // argc: 9, index: 20, ipc args: [bytes4, string, string, bytes8, string, string, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret PrepopulatePrecompiledCache();  // argc: 7, index: 21, ipc args: [bytes4, bytes4, string, string, string, string, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret WritePrecompiledCache();  // argc: 4, index: 22, ipc args: [bytes4, string, string, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CompileShaders();  // argc: 4, index: 23, ipc args: [bytes4, bytes4, string, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetShaderBucketForGraphicsAPI();  // argc: 2, index: 24, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    public void EnableShaderManagementSystem(bool enable);  // argc: 1, index: 25, ipc args: [bytes1], ipc returns: []
}