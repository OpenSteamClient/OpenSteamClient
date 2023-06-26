#include <string>
#include <vector>
#include <filesystem>
#include <curl/curl.h>
#include <map>
#include <unordered_set>
#include <sys/stat.h>
#include <opensteamworks/version.h>

#pragma once

namespace fs = std::filesystem;

// A struct that holds info about a part of memory and how long it is
struct MemoryStruct {
  char *data;
  size_t size;
};

// Automatically downloads a compatible version of Valve's Steam Client binaries
class Updater
{
private:
    std::string baseUrl = "https://client-update.akamai.steamstatic.com/";

    // List of files to download from the server
    // These can be found from a client manifest
    // Do not update these unless you know what you're doing!
    std::vector<std::string> filesToDownload 
    {
        // Contains the 32-bit steamservice among other things
        STEAM_BINS_UBUNTU12_NAME,
        // Contains the 64- and 32-bit steamclient.so:s
        STEAM_BINS_SDK_UBUNTU12_NAME,
        // Contains the various tools like disk-free, gldriverquery, steamclient.dll, steamservice.exe and iscriptevaluator.exe
        STEAM_BINS_MISC_UBUNTU12_NAME
    };
    std::unordered_set<std::string> foldersToCreate
    {
        "linux64",
        "linux32",
        "ubuntu12_32",
        "ubuntu12_64",
        "legacycompat",
        "bin"
    };

    // What files we actually want from the archives
    // We don't need all of the files, such as bpm shaders
    std::unordered_set<std::string> fileWhitelist
    {
        // Can't include a vector in a set's initializer
        // Copy filesToVerify here too
        "linux64/steamclient.so",
        "linux64/crashhandler.so",
        "linux32/steamclient.so",
        "linux32/crashhandler.so",
        "ubuntu12_32/steamclient.so",
        "ubuntu12_32/steamservice.so",

        "ubuntu12_64/fossilize_replay", // This can be compiled from source
        "ubuntu12_64/gameoverlayrenderer.so",
        "ubuntu12_32/gameoverlayrenderer.so",
        "ubuntu12_32/gameoverlayui.so",
        "ubuntu12_32/gameoverlayrenderer.so",
        "ubuntu12_64/gameoverlayrenderer.so",
        "ubuntu12_32/gameoverlayui",
        "ubuntu12_32/fossilize_replay", // This can be compiled from source
        "ubuntu12_64/streaming_client",
        "ubuntu12_64/libSDL3.so.0",
        "linux32/steamerrorreporter",
        "GameOverlayRenderer64.dll",
        "bin/d3ddriverquery64.exe",
        "legacycompat/Steam.dll",
        "legacycompat/SteamService.exe",
        "legacycompat/iscriptevaluator.exe",
        "steamclient.dll",
        "steamclient64.dll",
        "ubuntu12_64/vulkandriverquery",
        "ubuntu12_32/vulkandriverquery",
        "ubuntu12_64/gldriverquery",
        "ubuntu12_32/gldriverquery",
        "ubuntu12_64/disk-free" // We could probably reimplement this one
    };
    std::vector<std::tuple<std::string, std::string>> filesToCopy{
        {"reaper", "linux64/reaper"},
        {"steam-launch-wrapper", "linux64/steam-launch-wrapper"},
        {"steamserviced", "ubuntu12_32/steamserviced"},
        {"libmocksteamservice.so", "linux64/steamservice.so"},
        {"libdynamicwebview.so", "linux64/libdynamicwebview.so"}
    };

    fs::path installDir;
    CURL *curl;

    // By the time we reach this code, the bootstrap has already run so this should be safe
    void LocateInstallDir();

    static size_t write_data(void *contents, size_t size, size_t nmemb, void *userp);

public:
    // Verifies all known important binaries
    bool BVerifyBinaries();

    // Verifies a single file.
    // If the file has a checksum, it is checked to make sure it matches the installed file (not implemented)
    bool BVerifyFile(std::string path_s);
    
    // (Re)installs and downloads all important client files
    void Update();
    
    void InitCurl();

    void DownloadFromServer(std::string file, MemoryStruct& dataOut);

    void Verify(bool forceRedownload = false);

    void CopyOpensteamFiles();

    Updater();
    ~Updater();
};