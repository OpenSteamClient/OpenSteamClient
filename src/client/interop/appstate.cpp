#include "appstate.h"
#include <map>

const std::map<EAppState, const char *> mapOfUpdateStates = {
    {k_EAppStateAddingFiles, "Adding Files"},
    {k_EAppStateAppRunning, "Running"},
    {k_EAppStateBackupRunning, "Backup Running"},
    {k_EAppStateCommitting, "Committing"},
    {k_EAppStateDataEncrypted, "Encrypted"},
    {k_EAppStateDataLocked, "Locked"},
    {k_EAppStateDownloading, "Downloading"},
    {k_EAppStateFilesCorrupt, "Corrupt"}, 
    {k_EAppStateFilesMissing, "Files Missing"}, 
    {k_EAppStateFullyInstalled, "Installed"}, 
    {k_EAppStateInvalid, "Invalid"},
    {k_EAppStatePreallocating, "Preallocating"},
    {k_EAppStateReconfiguring, "Reconfiguring"},
    {k_EAppStateSharedOnly, "Shared Only"},
    {k_EAppStateStaging, "Staging"},
    {k_EAppStateUninstalled, "Uninstalled"},
    {k_EAppStateUninstalling, "Uninstalling"},
    {k_EAppStateUpdatePaused, "Update Paused"},
    {k_EAppStateUpdateRequired, "Update Required"},
    {k_EAppStateUpdateRunning, "Updating"}, 
    {k_EAppStateUpdateStopping, "Stopping"},
    {k_EAppStateValidating, "Validating"}
};

bool AppState::CheckStateAndAppend(EAppState stateToCheck) 
{
    bool hasState = (state & stateToCheck) == stateToCheck;
    if (hasState)
    {
        states.push_back(stateToCheck);
    }
    return hasState;
}

bool AppState::HasState(EAppState stateToCheck) 
{
    return (state & stateToCheck) == stateToCheck;
}

AppState::AppState(EAppState state)
{
    this->state = state;
    Uninstalled = CheckStateAndAppend(k_EAppStateUninstalled);
    UpdateRequired = CheckStateAndAppend(k_EAppStateUpdateRequired);
    FullyInstalled = CheckStateAndAppend(k_EAppStateFullyInstalled);
    DataEncrypted = CheckStateAndAppend(k_EAppStateDataEncrypted);
    SharedOnly = CheckStateAndAppend(k_EAppStateSharedOnly);
    DataLocked = CheckStateAndAppend(k_EAppStateDataLocked);
    FilesMissing = CheckStateAndAppend(k_EAppStateFilesMissing);
    FilesCorrupt = CheckStateAndAppend(k_EAppStateFilesCorrupt);
    AppRunning = CheckStateAndAppend(k_EAppStateAppRunning);
    UpdateRunning = CheckStateAndAppend(k_EAppStateUpdateRunning);
    UpdateStopping = CheckStateAndAppend(k_EAppStateUpdateStopping);
    UpdatePaused = CheckStateAndAppend(k_EAppStateUpdatePaused);
    UpdateStarted = CheckStateAndAppend(k_EAppStateUpdateStarted);
    Reconfiguring = CheckStateAndAppend(k_EAppStateReconfiguring);
    AddingFiles = CheckStateAndAppend(k_EAppStateAddingFiles);
    Downloading = CheckStateAndAppend(k_EAppStateDownloading);
    Staging = CheckStateAndAppend(k_EAppStateStaging);
    Committing = CheckStateAndAppend(k_EAppStateCommitting);
    Uninstalling = CheckStateAndAppend(k_EAppStateUninstalling);
    Preallocating = CheckStateAndAppend(k_EAppStatePreallocating);
    Validating = CheckStateAndAppend(k_EAppStateValidating);
}
AppState::~AppState() {
    
}

bool AppState::BIsPlayable() 
{
    if (UpdateRequired || DataEncrypted || DataLocked || FilesMissing || FilesCorrupt || Downloading || Staging || Committing || Uninstalling || Preallocating || Validating) {
        return false;
    }
    return true;
}
std::string AppState::AsString() 
{
    std::string finalString;
    for (size_t i = 0; i < states.size(); i++)
    {
        finalString += mapOfUpdateStates.at(states[i]);
        if (i < states.size()-1) {
            finalString += " ";
        }
    }

    return finalString;
}

std::string AppState::DisplayString()  
{
    if (UpdatePaused) {
        return "Update Paused";
    }
    if (UpdateRequired) {
        return "Update Required";
    }
    if (AppRunning) {
        return "Running";
    }
    if (UpdateRunning) {
        return "Updating";
    }
    if (UpdateStopping) {
        return "Stopping";
    }
    if (Uninstalling) {
        return "Uninstalling";
    }

    return "";
}