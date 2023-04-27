#ifndef ICLIENTSHADER_H
#define ICLIENTSHADER_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class UNSAFE_INTERFACE IClientShader
{
public:
     virtual bool BIsShaderManagementEnabled() = 0; //args: 0, index: 0
     virtual bool BIsShaderBackgroundProcessingEnabled() = 0; //args: 0, index: 1
     virtual void EnableShaderManagement(bool) = 0; //args: 1, index: 2
     virtual void EnableShaderBackgroundProcessing(bool) = 0; //args: 1, index: 3
     virtual void GetShaderDepotsTotalDiskUsage() = 0; //args: 0, index: 4
     virtual void GetShaderCacheDiskSize() = 0; //args: 1, index: 5
     virtual void StartShaderScan() = 0; //args: 2, index: 6
     virtual void StartPipelineBuild() = 0; //args: 2, index: 7
     virtual void StartShaderConversion() = 0; //args: 4, index: 8
     virtual void StartShaderPruning() = 0; //args: 0, index: 9
     virtual void ProcessShaderCache(AppId_t) = 0; //args: 1, index: 10
     virtual void GetShaderCacheProcessingCompletion() = 0; //args: 0, index: 11
     virtual void GetShaderCacheProcessingAppID() = 0; //args: 0, index: 12
     virtual void SkipShaderProcessing(AppId_t) = 0; //args: 1, index: 13
     virtual bool BAppHasPendingShaderContentDownload(AppId_t) = 0; //args: 1, index: 14
     virtual void GetAppPendingShaderDownloadSize(AppId_t) = 0; //args: 1, index: 15
     virtual void GetBucketManifest() = 0; //args: 3, index: 16
     virtual void GetStaleBucket() = 0; //args: 2, index: 17
     virtual void ReportExternalBuild() = 0; //args: 9, index: 18
     virtual void PrepopulatePrecompiledCache() = 0; //args: 7, index: 19
     virtual void WritePrecompiledCache() = 0; //args: 4, index: 20
     virtual void CompileShaders() = 0; //args: 4, index: 21
     virtual void GetShaderBucketForGraphicsAPI() = 0; //args: 2, index: 22
     virtual void EnableShaderManagementSystem() = 0; //args: 1, index: 23
};
#endif // ICLIENTSHADER_H
