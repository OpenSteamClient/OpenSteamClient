#include <QObject>
#include <string>
#include <vector>
#include <map>
#include <opensteamworks/EAppType.h>
#include "includesteamworks.h"
#include "appstate.h"
#include "launchoption.h"
#include <QImage>

#pragma once

struct LibraryAssets {
    bool hasLogo;
    std::string hero_url;
    std::string logoOverlay_url;
    std::string iconHash;
    std::string iconCachedFilename;
    QImage icon;
};

struct CompatTool 
{
    std::string name;
    std::string humanName;

    // If the tool allows you to play Windows games on Linux (Proton, Wine)
    bool windowsOnLinuxTool;
};

struct CompatData
{
    bool isCompatEnabled;
    CompatTool *currentCompatTool;
    std::vector<CompatTool*> validCompatTools;
};

struct Beta 
{
    std::string name;
    std::string description;
    bool pwdrequired;
    unsigned int buildid;
    unsigned int timeupdated;

    // Does this beta need a Local Content Server
    bool requirelcs;

    // Does the user have access to the beta through a password if it needs one
    bool hasAccess;
};

struct DLC
{
    std::string name;
    AppId_t dlcid;
};

struct LibraryFolder
{
    LibraryFolder_t folderIndex;
    std::string path;
    std::string label;
    std::string freeSpaceHuman;
    bool canFitGame;
};

class AppManager;

class App : public QObject
{
    Q_OBJECT
private:
    CompatData *compatData = nullptr;

    // This is a hack...
    AppManager *mgr;

public:

    // Initialization
    App(AppManager *mgr, AppId_t appid);
    ~App();
    void TypeFromString(const char* str);

    // We should work towards removing these

    void UpdateUpdateInfo();
    void UpdateAppState();
    void UpdateCompatInfo();

    // App Info

    AppId_t appid;
    CGameID gameid;
    std::string name;
    EAppType type;

    // Library data

    std::vector<std::string> inCategories;
    LibraryAssets libraryAssets;

    // Install data

    AppState *state;

    // Downloads info

    AppUpdateInfo_s updateInfo;

    // Debug stuff

    std::string debugPrefix;

// Data
public:

    // Actions

    void Kill(bool force);
    void Launch(LaunchOption launchOption);
    void Install(LibraryFolder libraryFolder);
    void Update();
    void Uninstall();
    void Move(LibraryFolder newLibraryFolder);
    void FetchLibraryAssets();
    void TryRunUpdate();

    // Set values

    void SetLibraryAssetsAvailable();
    void SetCompatTool(std::string toolName);
    void SetLaunchCommandLine(std::string newCommandLine);
    void SetCurrentBeta(std::string beta);


    // Clear values to defaults
    
    void ClearCompatTool();


    // Get values

    std::vector<LaunchOption> GetLaunchOptions();
    std::vector<LaunchOption> GetFilteredLaunchOptions();
    std::string GetLaunchCommandLine();
    std::vector<Beta> GetAllBetas();
    std::string GetCurrentBeta();
    AppStateInfo_t GetStateInfo();
    CompatData *GetCompatData();


    // UI 

    bool BShouldHighlightInGamesList();


signals:
    void LibraryAssetsAvailable();
    void UpdateInfoChanged();
    void UpdateFinished();
    void StateChanged();

    // Emitted when any of this App's fields change 
    // This is not called by updateinfo changes and other high-intensity codepaths.
    void AppDataChanged();

    // Errors
    void AppLaunchOrUpdateError(EAppUpdateError err);
};

Q_DECLARE_METATYPE(App)
Q_DECLARE_METATYPE(App*)