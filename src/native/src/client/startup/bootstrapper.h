#include <string>
#include <filesystem>
#include <vector>

#pragma once

namespace fs = std::filesystem;

enum EActiveInstall {
    k_EActiveInstallValveSteam = 1,
    k_EActiveInstallOpenSteam = 2,
    // In this case, user doesn't have split install enabled.
    k_EActiveInstallNone = 3,
    // We failed to detect the install type
    k_EActiveInstallInvalid = 4,
};

class Bootstrapper
{
private:
    // The install dir is located in .local/share/Steam.
    fs::path targetInstallDir;

    // The Steam data link is the .steam folder in a user's home folder. It contains symlinks to the steam install. 
    // This could be used to install steam into another folder, but this is currently untested.
    fs::path targetDatalinkDir;
    
    // Real paths of client installs if split install is enabled

    fs::path VSInstallDir;
    fs::path VSDatalinkDir;

    fs::path OSInstallDir;
    fs::path OSDatalinkDir;

    bool dryRun = false;

    void LocateInstallDir();
    void LocateDatalinkDir();
    // Checks if split install is done
    // Also returns false if it's configured wrong.
    bool BIsSplitInstall();
    void CreateSplitInstall();

    // Sets up OpenSteam for the first time
    void SetupOpenSteam();
    void CreateDatalinkForOpenSteam();
    void SetOpenSteamAsActiveInstall();
    void SetValveSteamAsActiveInstall();
    void DryRunDelete(const fs::path in);
    void CreateSymlinkIfInvalid(const fs::path in, const fs::path target);
    void AssertTargetDataLinkExists();
    void AssertTargetInstallDirExists();
    bool BLaunchedCorrectly();
    void CreateDirectory(fs::path);
    
public:
    EActiveInstall GetActiveInstall();
    // Sets ValveSteam as the active steam client. 
    // Close the app soon after calling this, otherwise we could mess with files
    void SetValveSteamActive();

    // When dryRun is true, no files will be altered.
    // Use this to determine if the bootstrapper will work before running it on your machine
    void RunBootstrap();
    std::string GetInstallDir();

    // Restarts the client
    void Restart(bool bNoSecondVerify);

    // Only call this when you need to relaunch with custom arguments, otherwise use Restart
    void Relaunch(bool bUseSelf = false, std::vector<std::string> customArgs = std::vector<std::string>());
    void CopyOpensteamMainBin();
    Bootstrapper();
    ~Bootstrapper();
};