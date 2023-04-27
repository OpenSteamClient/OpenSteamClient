#include "appstate.h"

AppState::AppState(EAppState state) {
    Uninstalled = (state & k_EAppStateUninstalled) == k_EAppStateUninstalled;
    UpdateRequired = (state & k_EAppStateUpdateRequired) == k_EAppStateUpdateRequired;
    FullyInstalled = (state & k_EAppStateFullyInstalled) == k_EAppStateFullyInstalled;
    DataEncrypted = (state & k_EAppStateDataEncrypted) == k_EAppStateDataEncrypted;
    SharedOnly = (state & k_EAppStateSharedOnly) == k_EAppStateSharedOnly;
    DataLocked = (state & k_EAppStateDataLocked) == k_EAppStateDataLocked;
    FilesMissing = (state & k_EAppStateFilesMissing) == k_EAppStateFilesMissing;
    FilesCorrupt = (state & k_EAppStateFilesCorrupt) == k_EAppStateFilesCorrupt;
    AppRunning = (state & k_EAppStateAppRunning) == k_EAppStateAppRunning;
    UpdateRunning = (state & k_EAppStateUpdateRunning) == k_EAppStateUpdateRunning;
    UpdateStopping = (state & k_EAppStateUpdateStopping) == k_EAppStateUpdateStopping;
    UpdatePaused = (state & k_EAppStateUpdatePaused) == k_EAppStateUpdatePaused;
    UpdateStarted = (state & k_EAppStateUpdateStarted) == k_EAppStateUpdateStarted;
    Reconfiguring = (state & k_EAppStateReconfiguring) == k_EAppStateReconfiguring;
    AddingFiles = (state & k_EAppStateAddingFiles) == k_EAppStateAddingFiles;
    Downloading = (state & k_EAppStateDownloading) == k_EAppStateDownloading;
    Staging = (state & k_EAppStateStaging) == k_EAppStateStaging;
    Committing = (state & k_EAppStateCommitting) == k_EAppStateCommitting;
    Uninstalling = (state & k_EAppStateUninstalling) == k_EAppStateUninstalling;
    Preallocating = (state & k_EAppStatePreallocating) == k_EAppStatePreallocating;
    Validating = (state & k_EAppStateValidating) == k_EAppStateValidating;
}
AppState::~AppState() {
    
}