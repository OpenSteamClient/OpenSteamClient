#include <opensteamworks/EAppState.h>
#include <string>
#include <vector>

#pragma once

class AppState {
private:
    bool CheckStateAndAppend(EAppState stateToCheck);
    std::vector<EAppState> states;

public:
    EAppState state;

    AppState(EAppState state = k_EAppStateInvalid);
    ~AppState();

    
    bool HasState(EAppState);
    std::string AsString();
    std::string DisplayString();

    bool BIsPlayable();

public:
    bool Uninstalled;
    bool UpdateRequired;
    bool FullyInstalled;
    bool DataEncrypted;
    bool SharedOnly;
    bool DataLocked;
    bool FilesMissing;
    bool FilesCorrupt;
    bool AppRunning;
    bool UpdateRunning;
    bool UpdateStopping;
    bool UpdatePaused;
    bool UpdateStarted;
    bool Reconfiguring;
    bool AddingFiles;
    bool Downloading;
    bool Staging;
    bool Committing;
    bool Uninstalling;
    bool Preallocating;
    bool Validating;
};