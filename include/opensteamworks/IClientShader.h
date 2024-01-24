#ifndef ICLIENTSHADER_H
#define ICLIENTSHADER_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientShader
{
public:
    virtual bool BIsShaderManagementEnabled() = 0; //argc: 0, index 1
    virtual bool BIsShaderBackgroundProcessingEnabled() = 0; //argc: 0, index 0
    virtual void EnableShaderManagement(bool) = 0; //argc: 1, index 0
    virtual void EnableShaderBackgroundProcessing(bool) = 0; //argc: 1, index 1
    virtual void GetShaderDepotsTotalDiskUsage() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetShaderCacheDiskSize() = 0; //argc: 3, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void StartShaderScan() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void StartPipelineBuild() = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void StartShaderConversion() = 0; //argc: 4, index 3
    virtual void StartShaderPruning() = 0; //argc: 0, index 4
    virtual void ProcessShaderCache(AppId_t) = 0; //argc: 1, index 0
    virtual void GetShaderCacheProcessingCompletion() = 0; //argc: 0, index 1
    virtual AppId_t GetShaderCacheProcessingAppID() = 0; //argc: 0, index 0
    virtual void SkipShaderProcessing(AppId_t) = 0; //argc: 1, index 0
    virtual bool BAppHasPendingShaderContentDownload() = 0; //argc: 1, index 1
    virtual void GetAppPendingShaderDownloadSize() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetBucketManifest() = 0; //argc: 3, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetStaleBucket() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ReportExternalBuild() = 0; //argc: 9, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void PrepopulatePrecompiledCache() = 0; //argc: 7, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void WritePrecompiledCache() = 0; //argc: 4, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void CompileShaders() = 0; //argc: 4, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetShaderBucketForGraphicsAPI() = 0; //argc: 2, index 9
    virtual void EnableShaderManagementSystem() = 0; //argc: 1, index 10
};
#endif // ICLIENTSHADER_H