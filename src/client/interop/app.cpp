#include "app.h"
#include "../ext/steamclient.h"
#include "../utils/binarykv.h"
#include "appmanager.h"
#include <opensteamworks/IClientApps.h>
#include <opensteamworks/IClientAppManager.h>
#include <opensteamworks/IClientCompat.h>
#include <opensteamworks/IClientConfigStore.h>

App::App(AppManager *mgr, AppId_t i) {
    this->mgr = mgr;
    this->appid = i;
    // TODO: check if this breaks sourcemods/other mods
    this->gameid = CGameID(this->appid, 0);
    this->state = new AppState(k_EAppStateInvalid);
    this->debugPrefix = std::string("[App ").append(std::to_string(this->appid)).append("] ");

    char *nameCStr = new char[1024];
    Global_SteamClientMgr->ClientApps->GetAppData(i, "common/name", nameCStr, 1024);
    this->name = std::string(nameCStr);
    delete[] nameCStr;

    UpdateUpdateInfo();
    UpdateCompatInfo();
    UpdateAppState();
}

App::~App() {
    delete this->state;
}

AppStateInfo_t App::GetStateInfo() {
    AppStateInfo_t stateInfo;
    Global_SteamClientMgr->ClientAppManager->GetAppStateInfo(this->appid, &stateInfo);
    return stateInfo;
}

CompatData *App::GetCompatData() {
    return this->compatData;
}

void App::UpdateCompatInfo() {

    // This function is a mess... 

    if (this->compatData != nullptr) {
        delete this->compatData;
    }

    this->compatData = new CompatData();

    if (!Global_SteamClientMgr->ClientCompat->BIsCompatLayerEnabled()) {
        this->compatData->isCompatEnabled = false;
        this->compatData->currentCompatTool = nullptr;
        this->compatData->validCompatTools.clear();
        return;
    }

    this->compatData->isCompatEnabled = Global_SteamClientMgr->ClientCompat->BIsCompatibilityToolEnabled(this->appid);
    this->compatData->validCompatTools.clear();
    
    CUtlVector<CUtlString> *vec = new CUtlVector<CUtlString>(1, 1000000);
    Global_SteamClientMgr->ClientCompat->GetAvailableCompatToolsForApp(vec, this->appid);

    const char *currentCompatToolNameCStr = Global_SteamClientMgr->ClientCompat->GetCompatToolName(this->appid);

    std::string currentCompatToolName;
    if (currentCompatToolNameCStr != nullptr)
    {
       currentCompatToolName = std::string(currentCompatToolNameCStr);
    }

    for (size_t i = 0; i < vec->Count(); i++)
    {
        std::string name = std::string(vec->Element(i).str);
        for (auto &&i2 : mgr->compatTools)
        {
            if (i2->name == name) {
                this->compatData->validCompatTools.push_back(i2);
            }

            if (currentCompatToolName == name) {
                this->compatData->currentCompatTool = i2;
            }
        }
    }
   
    delete vec;

    emit AppDataChanged();
}

void App::TryRunUpdate() {
    Global_SteamClientMgr->ClientAppManager->ChangeAppDownloadQueuePlacement(this->appid, k_EAppDownloadQueuePlacementPriorityUserInitiated);
}

void App::SetCompatTool(std::string toolName) {
    Global_SteamClientMgr->ClientCompat->SpecifyCompatTool(appid, toolName.c_str(), toolName.c_str(), 250);
    UpdateCompatInfo();
}

void App::ClearCompatTool() {
    Global_SteamClientMgr->ClientCompat->SpecifyCompatTool(appid, "", "", 0);
    UpdateCompatInfo();
}

void App::UpdateAppState() {
    delete this->state;
    this->state = new AppState(Global_SteamClientMgr->ClientAppManager->GetAppInstallState(this->appid));
    emit AppDataChanged();
    emit StateChanged();
}

void App::TypeFromString(const char* str) {
    size_t length = strlen(str);

    if (strncmp(str, "Game", length) == 0) {
        this->type = k_EAppTypeGame;
        return;
    }

    // wtf, game is specified in uppercase and lowercase
    if (strncmp(str, "game", length) == 0) {
        this->type = k_EAppTypeGame;
        return;
    }

    if (strncmp(str, "Demo", length) == 0) {
        this->type = k_EAppTypeDemo;
        return;
    }

    if (strncmp(str, "Demo", length) == 0) {
        type = k_EAppTypeDemo;
        return;
    } 

    if (strncmp(str, "Beta", length) == 0) {
        type = k_EAppTypeBeta;
        return;
    } 

    if (strncmp(str, "Tool", length) == 0) {
        type = k_EAppTypeTool;
        return;
    } 

    if (strncmp(str, "Application", length) == 0) {
        type = k_EAppTypeApplication;
        return;
    }

    if (strncmp(str, "Music", length) == 0) {
        type = k_EAppTypeMusic;
        return;
    }

    if (strncmp(str, "Config", length) == 0) {
        type = k_EAppTypeConfig;
        return;
    }

    if (strncmp(str, "DLC", length) == 0) {
        type = k_EAppTypeDlc;
        return;
    }

    if (strncmp(str, "media", length) == 0) {
        type = k_EAppTypeMedia;
        return;
    }

    if (strncmp(str, "Video", length) == 0) {
        type = k_EAppTypeVideo;
        return;
    }

    type = k_EAppTypeInvalid;
    DEBUG_MSG << debugPrefix << "App::TypeFromString unhandled type " << str << std::endl;
}

void App::UpdateUpdateInfo() {
    Global_SteamClientMgr->ClientAppManager->GetUpdateInfo(this->appid, &this->updateInfo);

    // Strange quirk: If this isn't done the auto download dates aren't set
    if (this->state->UpdateRequired) {
        Global_SteamClientMgr->ClientAppManager->ChangeAppDownloadQueuePlacement(this->appid, k_EAppDownloadQueuePlacementPriorityPaused);
    }
}

std::vector<LaunchOption> App::GetLaunchOptions() {
    size_t bufLen = 1000000;
    std::vector<uint8_t> buf(bufLen);

    int32 returnedLength = Global_SteamClientMgr->ClientApps->GetAppDataSection(this->appid, k_EAppInfoSectionConfig, buf.data(), bufLen, false);

    std::vector<LaunchOption> launchOptions;

    if (returnedLength != 0) {
        DEBUG_MSG << debugPrefix << "Retval: " << returnedLength << std::endl;
        DEBUG_MSG << debugPrefix << "Buf size: " << buf.size() << std::endl;

        BinaryKV *bkv = new BinaryKV(buf);
        DEBUG_MSG << bkv->outputJSON << std::endl;

        nlohmann::json launchOptsJSON = bkv->outputJSON["config"]["launch"];
        for (auto& [index, value] : launchOptsJSON.items()) {
            LaunchOption opt;
            
            opt.index = (int)atol(index.c_str());
            opt.positionInList = launchOptions.size();
            opt.isDefault = false;

            opt.name = "Unnamed option " + index;

            if (value.contains("description")) {
                opt.name = value["description"];
            }

            if (value.contains("type")) {
                std::string type = value["type"];
                if (type == "default") {
                    opt.positionInList = 0;
                    opt.isDefault = true;

                    if (opt.name.empty())
                    {
                        opt.name = "Play " + this->name;
                    }
                    else
                    {
                        opt.name = "Play " + this->name + " (" + opt.name + ")";
                    }
                } else if (type == "vr") {
                    opt.name = opt.name = "Play " + this->name + " in VR";
                } else if (type == "option1") {
                    opt.positionInList = 1;
                } else if (type == "option2") {
                    opt.positionInList = 2;
                } else if (type == "option3") {
                    opt.positionInList = 3;
                } else {
                    opt.name += " (" + type + ")";
                }
            }

            if (value.contains("config")) {
                auto config = value["config"];
                if (config.contains("oslist"))
                {
                    opt.oslist = config["oslist"];
                }
                if (config.contains("realm")) {
                    opt.realm = config["realm"];
                }
                if (config.contains("ownsdlc")) {
                    opt.ownsdlc = config["ownsdlc"];
                } else {
                    opt.ownsdlc = -1;
                }
                if (config.contains("BetaKey")) {
                    opt.BetaKey = config["BetaKey"];
                }
            } else {
                opt.oslist = "";
                opt.realm = "";
                opt.ownsdlc = -1;
                opt.BetaKey = "";
            }

            launchOptions.push_back(opt);
        }
        delete bkv;
    }

    return launchOptions;
}

std::string App::GetLaunchCommandLine() {
    std::string path = std::string("Software/Valve/Steam/Apps/").append(std::to_string(appid)).append("/LaunchOptions");

    const char *launchOpts = Global_SteamClientMgr->ClientConfigStore->GetString(k_EConfigStoreUserLocal, path.c_str(), "");
    return std::string(launchOpts);
}

void App::SetLaunchCommandLine(std::string commandLine) {
    std::string path = std::string("Software/Valve/Steam/Apps/").append(std::to_string(this->appid)).append("/LaunchOptions");

    Global_SteamClientMgr->ClientConfigStore->SetString(k_EConfigStoreUserLocal, path.c_str(), commandLine.c_str());
}

bool App::BShouldHighlightInGamesList() {
    if (this->state->UpdateRequired) {
        return true;
    }
    if (this->state->AppRunning) {
        return true;
    }
    if (this->state->Downloading)  {
        return true;
    }
    if (this->state->Uninstalling) {
        return true;
    }
    return false;
}

std::vector<Beta> App::GetAllBetas() {
    std::vector<Beta> betas;

    size_t bufLen = 1000000;
    std::vector<uint8_t> buf(bufLen);

    int returnedLength = Global_SteamClientMgr->ClientApps->GetAppDataSection(appid, k_EAppInfoSectionDepots, buf.data(), bufLen, false);

    if (returnedLength <= 0) {
        return std::vector<Beta>();
    }

    BinaryKV *bkv = new BinaryKV(buf);
    DEBUG_MSG << bkv->outputJSON << std::endl;

    for (auto &&i : bkv->outputJSON["depots"]["branches"].items())
    {
        Beta beta;

        beta.name = i.key();
        if (beta.name == "public") {
            continue;
        }

        beta.buildid = -1;
        beta.pwdrequired = false;
        beta.timeupdated = -1;

        if (i.value().contains("description")) {
            beta.description = i.value()["description"];
        } else {
            beta.description = "Missing Description";
        }

        if (i.value().contains("pwdrequired")) {
            beta.pwdrequired = (bool)(int)i.value()["pwdrequired"];
            beta.hasAccess = Global_SteamClientMgr->ClientAppManager->BHasCachedBetaPassword(appid, beta.name.c_str());
        }
        else
        {
            beta.hasAccess = true;
        }

        if (i.value().contains("timeupdated")) {
            beta.timeupdated = i.value()["timeupdated"];
        }
        
        betas.push_back(beta);
    }

    delete bkv;
    return betas;
}

std::string App::GetCurrentBeta() {
    size_t bufLen = 256;
    char *currentBetaCStr = new char[256];
    Global_SteamClientMgr->ClientAppManager->GetActiveBeta(appid, currentBetaCStr, bufLen);
    std::string currentBeta = std::string(currentBetaCStr);
    delete[] currentBetaCStr;
    return currentBeta;
}

void App::SetCurrentBeta(std::string beta) {
    Global_SteamClientMgr->ClientAppManager->SetAppConfigValue(appid, "betakey", beta.c_str());
    if (GetCurrentBeta() != beta) {
        std::cerr << debugPrefix << "Setting beta failed." << std::endl;
    }
}

void App::SetLibraryAssetsAvailable() {
    emit LibraryAssetsAvailable();
}

std::vector<LaunchOption> App::GetFilteredLaunchOptions() {
    std::vector<LaunchOption> launchOptions = GetLaunchOptions();
    std::vector<LaunchOption> workingLaunchOptions;
    for (auto &&opt : launchOptions)
    {
        // Filter by platform
        if (!this->compatData->currentCompatTool->windowsOnLinuxTool) {
            if (!opt.oslist.empty() && !opt.oslist.contains("linux"))
            {
                std::cout << debugPrefix << opt.index << ": Didn't match linux filter and proton is disabled" << std::endl;
                continue;
            }
        }   
        


        // Filter by realm
        // Let's assume Valve isn't using this internally with their dev universes
        if (!opt.realm.empty() && !opt.realm.contains("steamglobal")) {
            continue;
        }
        
        // Filter by required DLC
        AppId_t dlcid = opt.ownsdlc;

        if (dlcid != -1) {
            if (!Global_SteamClientMgr->ClientAppManager->IsAppDlcInstalled(this->appid, dlcid)) {
                std::cout << debugPrefix << opt.index << ": Didn't match ownsdlc filter " << opt.ownsdlc << std::endl;
                continue;
            }
        }
        

        // Filter by required beta
        if (!opt.BetaKey.empty()) {
            std::string betaAsStr = "public";
            char *currentBeta = new char[512];
            Global_SteamClientMgr->ClientAppManager->GetActiveBeta(this->appid, currentBeta, 512);

            if (!(std::string(currentBeta).empty()))
            {
                betaAsStr = std::string(currentBeta);
            }

            delete[] currentBeta;

            if (!opt.BetaKey.contains(betaAsStr)) {
                std::cout << debugPrefix << opt.index << ": Didn't match BetaKey filter" << std::endl;
                continue;
            }

            
        }

        workingLaunchOptions.push_back(opt);
    }

    return workingLaunchOptions;
}

void App::FetchLibraryAssets() {

}

// Lifecycle management

void App::Kill(bool force) {
    Global_SteamClientMgr->ClientAppManager->ShutdownApp(this->appid, force);
    UpdateAppState();
}

void App::Launch(LaunchOption launchOption) {
    EAppUpdateError err = (EAppUpdateError)0;
    DEBUG_MSG << debugPrefix << "Launching " << this->name << " with launch opt " << launchOption.index << std::endl;
    
    std::string launchOptsPath = std::string("Software/Valve/Steam/Apps/").append(std::to_string(this->appid)).append("/LaunchOptions");
    const char *launchOptsCStr = Global_SteamClientMgr->ClientConfigStore->GetString(k_EConfigStoreUserLocal, launchOptsPath.c_str(), "");
    //TODO: global launch options set in settings

    err = Global_SteamClientMgr->ClientAppManager->LaunchApp(this->gameid, launchOption.index, 64, launchOptsCStr);

    emit AppLaunchOrUpdateError(err);
    UpdateAppState();
}

// Install and download management

void App::Install(LibraryFolder libraryFolder) {
    EAppUpdateError err = (EAppUpdateError)0;
    err = Global_SteamClientMgr->ClientAppManager->InstallApp(this->appid, libraryFolder.folderIndex, false);
    
    emit AppLaunchOrUpdateError(err);

    UpdateAppState();
}

// This function shouldn't be called automatically, only at user request
void App::Update() {
    Global_SteamClientMgr->ClientAppManager->ChangeAppDownloadQueuePlacement(this->appid, k_EAppDownloadQueuePlacementPriorityUserInitiated);
    UpdateAppState();
}

void App::Uninstall() {
    EAppUpdateError err = (EAppUpdateError)0;
    err = Global_SteamClientMgr->ClientAppManager->UninstallApp(this->appid);
    
    emit AppLaunchOrUpdateError(err);
    UpdateAppState();
}

void App::Move(LibraryFolder newLibraryFolder) {

    //TODO: should we check CanMoveApp?
    EAppUpdateError err = (EAppUpdateError)0;
    err = Global_SteamClientMgr->ClientAppManager->MoveApp(this->appid, newLibraryFolder.folderIndex);
    
    emit AppLaunchOrUpdateError(err);
    UpdateAppState();
}