#include "bootstrapper.h"
#include <string>
#include <stdio.h>
#include <filesystem>
#include <iostream>
#include <fstream>
#include <unistd.h>
#include "../../common/ldpath.h"
#include "updater.h"
#include "../globals.h"
#include "../../common/commandline.h"

namespace fs = std::filesystem;

void Bootstrapper::LocateInstallDir() {
    auto home_env = std::string(getenv("HOME"));
    targetInstallDir = home_env + "/.local/share/Steam";
    OSInstallDir = home_env + "/.local/share/OpenSteam";
    VSInstallDir = home_env + "/.local/share/ValveSteam";
    DEBUG_MSG << "[Bootstrapper] TargetInstallDir is at " << targetInstallDir << std::endl;
    DEBUG_MSG << "[Bootstrapper] OSInstallDir is at " << OSInstallDir << std::endl;
    DEBUG_MSG << "[Bootstrapper] VSInstallDir is at " << VSInstallDir << std::endl;
}
void Bootstrapper::LocateDatalinkDir() {
    auto home_env = std::string(getenv("HOME"));
    targetDatalinkDir = home_env + "/.steam";
    OSDatalinkDir = home_env + "/.opensteam";
    VSDatalinkDir = home_env + "/.valvesteam";
    DEBUG_MSG << "[Bootstrapper] TargetDatalink is at " << targetDatalinkDir << std::endl;
    DEBUG_MSG << "[Bootstrapper] OSDatalinkDir is at " << OSDatalinkDir << std::endl;
    DEBUG_MSG << "[Bootstrapper] VSDatalinkDir is at " << VSDatalinkDir << std::endl;
}
// Checks if split install is done
// Also returns false if it's configured wrong.
bool Bootstrapper::BIsSplitInstall() {
    if (!fs::exists(targetDatalinkDir)) {
        return false;
    }
    if (fs::is_symlink(targetDatalinkDir)) {
        auto datalinkLinkedTo = fs::read_symlink(targetDatalinkDir);
        DEBUG_MSG << "[Bootstrapper] Datalink linked to " << datalinkLinkedTo << std::endl;
        DEBUG_MSG << "[Bootstrapper] Datalink filename is " << datalinkLinkedTo.filename().string() << std::endl;
        if (datalinkLinkedTo.filename().string() == ".opensteam" || datalinkLinkedTo.filename().string() == ".valvesteam") {
        } else {
            std::cerr << "[Bootstrapper] Unknown datalink target " << datalinkLinkedTo.filename().string() << std::endl;
            return false;
        }
    } else {
        std::cerr << "[Bootstrapper] Datalink is not a symlink. " << std::endl;
        return false;
    }

    if (fs::is_symlink(targetInstallDir)) {
        auto installLinkedTo = fs::read_symlink(targetInstallDir);
        DEBUG_MSG << "[Bootstrapper] Installdir linked to " << installLinkedTo << std::endl;
        DEBUG_MSG << "[Bootstrapper] Installdir filename is " << installLinkedTo.filename().string() << std::endl;
        if (installLinkedTo.filename().string() == "OpenSteam" || installLinkedTo.filename().string() == "ValveSteam") {
            // If everything goes well, this is the result that should be returned
            return true;
        } else {
            std::cerr << "[Bootstrapper] Unknown installdir target " << installLinkedTo.filename().string() << std::endl;
            return false;
        }
    } else {
        std::cerr << "[Bootstrapper] Installdir is not a symlink. " << std::endl;
        return false;
    }
    std::cerr << "[Bootstrapper] WARNING! Could not determine if this is a split install! \nStrange issues may arise!" << std::endl;
    return false;
}
void Bootstrapper::CreateSplitInstall() {
    // Only ValveSteam is ever going to be a non-symlink (unless the user mucks with the files)

    if (fs::exists(targetDatalinkDir)) {
        if (fs::is_symlink(targetDatalinkDir)) {
            std::cerr << "[Bootstrapper] Not splitting installs; DatalinkDir is a symlink." << std::endl;
            return;
        } 
        if (fs::exists(targetDatalinkDir / "opensteam_marker")) {
            std::cerr << "[Bootstrapper] Not splitting installs; DatalinkDir contains opensteam_marker. \nIs " << targetDatalinkDir << " an OpenSteam installation?" << std::endl;
            return;
        }
        if (dryRun) {
            std::cout << "[Bootstrapper] Would have renamed " << targetDatalinkDir << " to " << VSDatalinkDir << std::endl;
            return;
        }
        fs::rename(targetDatalinkDir, VSDatalinkDir); 
    }
    if (fs::exists(targetInstallDir)) {
        if (fs::is_symlink(targetInstallDir)) {
            std::cerr << "[Bootstrapper] Not splitting installs; InstallDir is a symlink." << std::endl;
            return;
        } 
        if (fs::exists(targetInstallDir / "opensteam_marker")) {
            std::cerr << "[Bootstrapper] Not splitting installs; InstallDir contains opensteam_marker. \nIs " << targetInstallDir << " an OpenSteam installation?" << std::endl;
            return;
        }
        if (dryRun) {
            std::cout << "[Bootstrapper] Would have renamed " << targetInstallDir << " to " << VSInstallDir << std::endl;
            return;
        }
        fs::rename(targetInstallDir, VSInstallDir); 
    }

}

// Sets up OpenSteam for the first time
void Bootstrapper::SetupOpenSteam() {
    CreateDirectory(OSInstallDir);

    //TODO: move this to a logger class
    CreateDirectory(OSInstallDir / "logs");

    CopyOpensteamMainBin();
    Global_Updater->CopyOpensteamFiles();

    CreateDatalinkForOpenSteam();
    SetOpenSteamAsActiveInstall();

    Restart(false);
}

void Bootstrapper::CreateDirectory(fs::path path) {
    if (dryRun) {
        std::cout << "[Bootstrapper] Would have created directory " << path << std::endl;
    } else {
        fs::create_directory(path);
    }
}

std::vector<char*> AppendArgsAndArgv(std::vector<std::string> arguments) {
    std::vector<char*> args_full;
    for (const auto& arg : arguments) {
        size_t strSize = arg.size() + sizeof(NULL);
        void *alloced = malloc(strSize);
        memcpy(alloced, arg.data(), strSize);
        args_full.push_back((char *)alloced);
    }

    for (size_t i = 0; i < Global_CommandLine->argc-1; i++)
    {
        if (Global_CommandLine->argv[i] != NULL) {
            DEBUG_MSG << "[Bootstrapper] Appending " << Global_CommandLine->argv[i] << std::endl;
        } else {
            DEBUG_MSG << "[Bootstrapper] Appending NULL" << std::endl;
        }
        args_full.push_back(Global_CommandLine->argv[i]);
    }

    args_full.push_back(NULL);
    return args_full;
}

void Bootstrapper::Relaunch(bool bUseSelf, std::vector<std::string> customArgs) {
    fs::path newbin;
    if (bUseSelf == true) {
        newbin = fs::read_symlink("/proc/self/exe");
    } else {
        newbin = (OSInstallDir / "linux64" / "steam");
    }

    if (dryRun) {
        std::cout << "[Bootstrapper] Would have execvp()d but we are running in dryRun mode. \nExecution cannot continue." << std::endl;
        exit(EXIT_FAILURE);
        return;
    }

    CreateDirectory(OSInstallDir);
    fs::current_path(OSInstallDir);

    // Use custom args if specified, else use normal launch
    if (customArgs.size() > 0) {
        std::cout << "[Bootstrapper] Relaunching with custom args" << std::endl;
        std::vector<std::string> fullArgs;
        for (auto &&i : customArgs)
        {
            fullArgs.push_back(i);
        }
        fullArgs.push_back(newbin.string().c_str());

        std::vector<char *> arguments = AppendArgsAndArgv(fullArgs);
        execvp(newbin.string().c_str(), arguments.data());
        std::cerr << "[Bootstrapper] Failed to relaunch: " << errno << std::endl;
        exit(EXIT_FAILURE);
    } else {
        std::cout << "[Bootstrapper] Relaunching" << std::endl;
        auto debugger = getenv("STEAM_DEBUGGER");
        if (debugger != NULL) {
            std::cout << "[Bootstrapper] Debugger enabled!" << std::endl;
            if (std::string(debugger).contains("gdb"))
            {
                std::cout << "[Bootstrapper] Debugger is gdb" << std::endl;

                
                std::vector<char*> arguments = AppendArgsAndArgv({"--args", newbin.string().c_str()});

                // Start with debugger
                std::cout << "[Bootstrapper] Starting with gdb" << std::endl;
                std::cout << "[Bootstrapper] NOTE: when using gdb, you should ignore the first exception from IPCServ, it is not fatal" << std::endl;
                execvp("gdb", arguments.data());
                if (errno == ENOENT) {
                    std::cout << "[Bootstrapper] Missing gdb or not in path" << std::endl;
                } else {
                    std::cout << "[Bootstrapper] Failed to relaunch: " << errno << std::endl;
                }
                exit(EXIT_FAILURE);
                return;
            }
        }
    }
    
    
    execvp(newbin.string().c_str(), Global_CommandLine->argv);
    std::cerr << "[Bootstrapper] Failed to relaunch: " << errno << std::endl;
    exit(EXIT_FAILURE);
}
void Bootstrapper::CreateDatalinkForOpenSteam() {
    
    CreateDirectory(OSDatalinkDir);

    CreateSymlinkIfInvalid(OSDatalinkDir / "bin32", OSInstallDir / "ubuntu12_32");
    CreateSymlinkIfInvalid(OSDatalinkDir / "bin64", OSInstallDir / "ubuntu12_64");
    CreateSymlinkIfInvalid(OSDatalinkDir / "root",  OSInstallDir);
    CreateSymlinkIfInvalid(OSDatalinkDir / "sdk32", OSInstallDir / "linux32");
    CreateSymlinkIfInvalid(OSDatalinkDir / "sdk64", OSInstallDir / "linux64");
    CreateSymlinkIfInvalid(OSDatalinkDir / "steam", OSInstallDir);
    
    if (dryRun) {
        std::cout << "[Bootstrapper] Would have created file " << (OSDatalinkDir / "opensteam_marker") << std::endl;
    } else {
        std::ofstream marker(OSDatalinkDir / "opensteam_marker");
        marker.close();
    }
    
    if (dryRun) {
        std::cout << "[Bootstrapper] Would have created file " << (OSInstallDir / "opensteam_marker") << std::endl;
    } else {
        std::ofstream marker(OSInstallDir / "opensteam_marker");
        marker.close();
    }
}
void Bootstrapper::SetOpenSteamAsActiveInstall() {
    std::cout << "[Bootstrapper] Setting OpenSteam as active install" << std::endl;

    if (fs::exists(targetDatalinkDir)) {
        DryRunDelete(targetDatalinkDir);
    }
    if (fs::exists(targetInstallDir)) {
        DryRunDelete(targetInstallDir);
    }

    CreateSymlinkIfInvalid(targetDatalinkDir, OSDatalinkDir);
    CreateSymlinkIfInvalid(targetInstallDir, OSInstallDir);
    
}
void Bootstrapper::SetValveSteamAsActiveInstall() {
    std::cout << "[Bootstrapper] Setting ValveSteam as active install" << std::endl;

    AssertTargetDataLinkExists();
    AssertTargetInstallDirExists();

    CreateSymlinkIfInvalid(targetDatalinkDir, VSDatalinkDir);
    CreateSymlinkIfInvalid(targetInstallDir, VSInstallDir);

}
void Bootstrapper::DryRunDelete(const fs::path in) {
    if (dryRun) {
        std::cout << "[Bootstrapper] Would have deleted " << in << std::endl;
    } else {
        fs::remove(targetDatalinkDir);
    }
}
void Bootstrapper::CreateSymlinkIfInvalid(const fs::path in, const fs::path target) {
    if (fs::is_symlink(in)) {
        if (fs::read_symlink(in) == target) {
            return;
        } else {
            fs::remove(in);
        }
    }
    if (dryRun) {
        std::cout << "[Bootstrapper] Would have created symlink at " << in << " pointing to " << target << std::endl;
        return;
    }
    fs::create_directory_symlink(target, in);
}
void Bootstrapper::AssertTargetDataLinkExists() {
    if (!fs::exists(targetDatalinkDir)) {
        throw std::logic_error("Target datalink dir does not exist.");
    }
    if (!fs::is_directory(targetDatalinkDir)) {
        throw std::logic_error("Target datalink dir is not a directory.");
    }
}
void Bootstrapper::AssertTargetInstallDirExists() {
    if (!fs::exists(targetInstallDir)) {
        throw std::logic_error("Target install dir does not exist.");
    }
    if (!fs::is_directory(targetInstallDir)) {
        throw std::logic_error("Target install dir is not a directory.");
    }
}
bool Bootstrapper::BLaunchedCorrectly() {
    if (fs::exists(fs::current_path() / "opensteam_marker")) {
        std::cout << "[Bootstrapper] Launched correctly, opensteam_marker is inside this directory" << std::endl;
        return true;
    }
    std::cout << "[Bootstrapper] Current folder " << fs::current_path() << " doesn't contain a marker" << std::endl;
    return false;
}
EActiveInstall Bootstrapper::GetActiveInstall() {
    if (!BIsSplitInstall()) {
        return k_EActiveInstallNone;
    }
    if (fs::is_regular_file(targetDatalinkDir / "opensteam_marker") && fs::is_regular_file(targetInstallDir / "opensteam_marker")) {
        return k_EActiveInstallOpenSteam;
    }
    // Let's hope Valve doesn't get rid of this file
    if (fs::is_regular_file(targetInstallDir / "bootstrap.tar.xz")) {
        return k_EActiveInstallValveSteam;
    }
    return k_EActiveInstallInvalid;
}
// Sets ValveSteam as the active steam client. 
// Close the app soon after calling this, otherwise we could mess with files
void Bootstrapper::SetValveSteamActive() {
    //TODO: Run some tests before we set this (does the user even have ValveSteam)
    SetValveSteamAsActiveInstall();
}

// When dryRun is true, no files will be altered.
// Use this to determine if the bootstrapper will work before running it on your machine
void Bootstrapper::RunBootstrap() {
    this->dryRun = Global_CommandLine->HasOption("--bootstrapper-dry-run");

    if (getenv("LD_LIBRARY_PATH") == NULL)  {
        SetLdLibraryPath(GetInstallDir());
        Restart(false);
    }

    // If we're in development, we should re-copy the files every build
#ifdef DEV_BUILD
    std::cout << "[Bootstrapper] Running in development, re-copying files from build dir" << std::endl;
    CopyOpensteamMainBin();
    Global_Updater->CopyOpensteamFiles();
#endif

    auto activeInstall = GetActiveInstall();
    switch (activeInstall)
    {
        case k_EActiveInstallInvalid:
        case k_EActiveInstallNone:
            if (!BIsSplitInstall()) {
                CreateSplitInstall();
            }
            SetupOpenSteam();
            break;
        case k_EActiveInstallOpenSteam:
            break;
        case k_EActiveInstallValveSteam:
            SetOpenSteamAsActiveInstall();
            break;
        default:
            break;
    }
    DEBUG_MSG << activeInstall << std::endl;
    if (!BLaunchedCorrectly()) {
        SetupOpenSteam();
    }
    if (dryRun) {
        std::cout << "[Bootstrapper] Exiting OpenSteam as we cannot proceed with dryRun enabled." << std::endl;
        exit(EXIT_FAILURE);
    }
}

std::string Bootstrapper::GetInstallDir() {
    return OSInstallDir;
}

void Bootstrapper::Restart(bool bNoSecondVerify) {
    if (bNoSecondVerify == true) {
        setenv("UPDATER_RAN_ONCE", "1", 0);
    }
    Relaunch(true);
}

void Bootstrapper::CopyOpensteamMainBin() {
    auto thisexe = fs::read_symlink("/proc/self/exe");
    auto newbin = (OSInstallDir / "linux64" / "steam");
    CreateDirectory(OSInstallDir / "linux64");
    if (dryRun) {
        std::cout << "[Bootstrapper] Would have copied " << thisexe << " to " << newbin << std::endl;
    } else {
        try
        {
            fs::copy_file(thisexe, newbin, fs::copy_options::update_existing);
        }
        catch(const std::exception& e)
        {
            if (std::string(e.what()).contains("File exists")) {
                std::cout << "[Bootstrapper] " << newbin << " is up-to-date. " << std::endl;
            } else {
                std::cerr << "[Bootstrapper] Failed to copy " << newbin << ": " << e.what() << std::endl;
            }
        }
    }
}
Bootstrapper::Bootstrapper() {    
    LocateInstallDir();
    LocateDatalinkDir();
}
Bootstrapper::~Bootstrapper() {}