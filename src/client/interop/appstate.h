#include <opensteamworks/EAppState.h>

#pragma once

class AppState {
public:
    AppState(EAppState state);
    ~AppState();
    EAppState state;
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