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
    public unknown_ret BIsShaderManagementEnabled();  // argc: 0, index: 1
    public unknown_ret BIsShaderBackgroundProcessingEnabled();  // argc: 0, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret EnableShaderManagement();  // argc: 1, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret EnableShaderBackgroundProcessing();  // argc: 1, index: 4
    public unknown_ret GetShaderDepotsTotalDiskUsage();  // argc: 0, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret GetShaderCacheDiskSize();  // argc: 1, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret StartShaderScan();  // argc: 2, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret StartPipelineBuild();  // argc: 2, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret StartShaderConversion();  // argc: 4, index: 9
    public unknown_ret StartShaderPruning();  // argc: 0, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessShaderCache();  // argc: 1, index: 11
    public unknown_ret GetShaderCacheProcessingCompletion();  // argc: 0, index: 12
    public unknown_ret GetShaderCacheProcessingAppID();  // argc: 0, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret SkipShaderProcessing();  // argc: 1, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret BAppHasPendingShaderContentDownload();  // argc: 1, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppPendingShaderDownloadSize();  // argc: 1, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret GetBucketManifest();  // argc: 3, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret GetStaleBucket();  // argc: 2, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret ReportExternalBuild();  // argc: 9, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret PrepopulatePrecompiledCache();  // argc: 7, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret WritePrecompiledCache();  // argc: 4, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret CompileShaders();  // argc: 4, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret GetShaderBucketForGraphicsAPI();  // argc: 2, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret EnableShaderManagementSystem();  // argc: 1, index: 24
}