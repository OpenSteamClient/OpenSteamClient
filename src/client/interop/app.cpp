#include "app.h"
#include "../ext/steamclient.h"
#include "../utils/binarykv.h"

App::App(AppId_t i) {
    this->appid = i;
    // TODO: check if this breaks sourcemods/other mods
    this->gameid = CGameID(this->appid, 0);
    this->state = new AppState(k_EAppStateInvalid);
    UpdateUpdateInfo();
    UpdateCompatInfo();
}
App::~App() {
    delete this->state;
}

void App::UpdateCompatInfo() {
    this->compatData.isCompatEnabled = Global_SteamClientMgr->ClientCompat->BIsCompatibilityToolEnabled(this->appid);
    this->compatData.whitelistedCompatTools = std::vector<std::string>();

    if (this->compatData.isCompatEnabled) {
        char* cname = Global_SteamClientMgr->ClientCompat->GetCompatToolName(this->appid);
        std::string name = std::string(cname);
        this->compatData.currentCompatTool = name;

        // Is this proton or a different kind of compat tool?
        // TODO: improve detection logic
        if (name.starts_with("proton_")) {
            this->compatData.isWindowsOnLinuxTool = true;
        } else {
            this->compatData.isWindowsOnLinuxTool = false;
        }
    } else {
        this->compatData.currentCompatTool = "";
        this->compatData.isWindowsOnLinuxTool = false;
    }

    emit AppDataChanged();
}

void App::UpdateAppState() {
    delete this->state;
    this->state = new AppState(Global_SteamClientMgr->ClientAppManager->GetAppInstallState(this->appid));
    emit AppDataChanged();
}

void App::TypeFromString(const char* str) {
    size_t length = strlen(str);

    if (strncmp(str, "Game", length) == 0) {
        this->type = k_EAppTypeGame;
        return;
    }

    // wtf
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

    type = k_EAppTypeInvalid;
    DEBUG_MSG << "App::TypeFromString unhandled type " << str << std::endl;
}

void App::UpdateUpdateInfo() {
    Global_SteamClientMgr->ClientAppManager->GetUpdateInfo(this->appid, &this->updateInfo);
    this->hasUpdate = (this->updateInfo.m_unBytesToDownload > 0);
}

std::vector<LaunchOption> App::GetLaunchOptions() {
    size_t bufLen = 1000000;
    std::vector<uint8_t> buf(bufLen);

    int32 returnedLength = Global_SteamClientMgr->ClientApps->GetAppDataSection(this->appid, k_EAppInfoSectionConfig, buf.data(), bufLen, false);

    std::vector<LaunchOption> launchOptions;

    if (returnedLength != 0) {
        DEBUG_MSG << "Retval: " << returnedLength << std::endl;
        DEBUG_MSG << "Buf size: " << buf.size() << std::endl;

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
    }

    return launchOptions;
}

void App::SetLaunchCommandLine(std::string commandLine) {
    std::string path = std::string("Software/Valve/Steam/Apps/").append(std::to_string(this->appid)).append("/LaunchOptions");

    Global_SteamClientMgr->ClientConfigStore->SetString(k_EConfigStoreUserLocal, path.c_str(), commandLine.c_str());
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
        if (!this->compatData.isWindowsOnLinuxTool) {
            if (!opt.oslist.empty() && !opt.oslist.contains("linux"))
            {
                std::cout << opt.index << ": Didn't match linux filter and proton is disabled" << std::endl;
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
                std::cout << opt.index << ": Didn't match ownsdlc filter " << opt.ownsdlc << std::endl;
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
                std::cout << opt.index << ": Didn't match BetaKey filter" << std::endl;
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
    DEBUG_MSG << "Launching " << this->name << " with launch opt " << launchOption.index << std::endl;
    
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

    if (err == 0) {
        this->installFolder = libraryFolder;
    }

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