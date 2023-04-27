#include "errmsgutils.h"
#include <map>

std::map<EAppUpdateError, std::string> mapOfEAppUpdateErrors 
{
    {k_EAppErrorApplicationRunning, "Application is running"}, 
    {k_EAppErrorNoSubscription, "No subscription"}, 
    {k_EAppErrorCorruptContent, "Corrupt content"}, 
    {k_EAppErrorCorruptDepotCache, "Corrupt depot cache"}, 
    {k_EAppErrorCorruptUpdateFiles, "Corrupt update files"}, 
    {k_EAppErrorDependencyFailure, "Dependency failure"}, 
    {k_EAppErrorDiskReadFailure, "Disk read failure"}, 
    {k_EAppErrorDiskWriteFailure, "Disk write failure"}, 
    {k_EAppErrorInvalidApplicationConfiguration, "Invalid application configuration (most likely launch option too high)"}, 
    {k_EAppErrorInvalidContentConfiguration, "Invalid content configuration"}, 
    {k_EAppErrorInvalidFileSystem, "Invalid filesystem (unused?)"}, 
    {k_EAppErrorInvalidInstallPath, "Invalid install path"}, 
    {k_EAppErrorInvalidPlatform, "Invalid platform"}, 
    {k_EAppErrorMissingConfig, "Missing config"}, 
    {k_EAppErrorMissingExecutable, "Missing executable (wrong platform or missing depots)"}, 
    {k_EAppErrorMissingKey, "Missing key"},
    {k_EAppErrorMissingManifest, "Missing manifest"}, 
    {k_EAppErrorNoConnection, "No connection"},
    {k_EAppErrorNoConnectionToContentServers, "No connection to content servers"}, 
    {k_EAppErrorNone, "Success"},  
    {k_EAppErrorNotInstalled, "Not installed"}, 
    {k_EAppErrorNotReleased, "Not released"}, 
    {k_EAppErrorRegionRestricted, "Region restricted"}, 
    {k_EAppErrorStillBusy, "StillBusy"}, 
    {k_EAppErrorTimeout, "Timed out"}, 
    {k_EAppErrorUpdateRequired, "Update required"}, 
    {k_EAppErrorWaitingForDisk, "Waiting for disk"}, 
    {k_EAppUpdateErrorDownloadCorrupt, "Download corrupt"}, 
    {k_EAppUpdateErrorDownloadDisabled, "Downloads are disabled"}, 
    {k_EAppUpdateErrorOtherSessionPlaying, "Other session is playing"}, 
    {k_EAppUpdateErrorPurchasePending, "Purchase pending"}, 
    {k_EAppUpdateErrorSharedLibraryLocked, "Shared library locked"}, 
};

std::string ErrMsgUtils::GetErrorMessageFromEAppUpdateError(EAppUpdateError error)
{
    auto errStr = mapOfEAppUpdateErrors[error];
    if (errStr.empty()) {
        return std::string("[EAppUpdateError ").append(std::to_string(error)).append("]");
    }
    return errStr;  
}