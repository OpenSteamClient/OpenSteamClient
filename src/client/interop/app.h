#include <QObject>
#include <string>
#include <vector>
#include <map>
#include <opensteamworks/EAppType.h>
#include "includesteamworks.h"
#include "appstate.h"
#include "launchoption.h"
#include <QIcon>

#pragma once

struct LibraryAssets {
    bool hasLogo;
    std::string hero_url;
    std::string logoOverlay_url;
    std::string iconHash;
    std::string iconCachedFilename;
};

struct CompatData
{
    bool isCompatEnabled;

    // If the tool allows you to play Windows games on Linux (Proton, Wine)
    bool isWindowsOnLinuxTool;
    std::string currentCompatTool;
    std::vector<std::string> whitelistedCompatTools;
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

class App : public QObject
{
    Q_OBJECT
public:

    // Initialization
    App(AppId_t appid);
    ~App();
    void TypeFromString(const char* str);
    void UpdateUpdateInfo();

    std::vector<LaunchOption> GetLaunchOptions();
    std::vector<LaunchOption> GetFilteredLaunchOptions();

    // App Info
    AppId_t appid;
    CGameID gameid;
    std::string name;
    EAppType type;
    std::vector<Beta> allBetas;
    std::vector<DLC> allDLC;

    // Library data
    std::vector<std::string> inCategories;
    LibraryAssets libraryAssets;

    // Install data
    Beta currentBeta;
    std::vector<DLC> enabledDLC;
    AppState *state;
    LibraryFolder installFolder;
    CompatData compatData;

    // Downloads info
    AppUpdateInfo_s updateInfo;
    bool hasUpdate;

public slots:
    // Network requests
    void FetchLibraryAssets();

    // Lifecycle management
    void Kill(bool force);
    void Launch(LaunchOption launchOption);
    void Install(LibraryFolder libraryFolder);
    void Update();
    void Uninstall();
    void Move(LibraryFolder newLibraryFolder);

    // Change fields
    void UpdateAppState();
    void UpdateCompatInfo();
    void SetLaunchCommandLine(std::string newCommandLine);
    void SetLibraryAssetsAvailable();

signals:
    void LibraryAssetsAvailable();
    void UpdateInfoChanged();
    void UpdateFinished();

    // Emitted when any of this App's fields change 
    // This is not called by updateinfo changes and other high-intensity codepaths.
    void AppDataChanged();

    // Errors
    void AppLaunchOrUpdateError(EAppUpdateError err);
};

Q_DECLARE_METATYPE(App)
Q_DECLARE_METATYPE(App*)