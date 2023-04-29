#include "updater.h"
#include <string>
#include <cstring>
#include <vector>
#include <stdio.h>
#include <filesystem>
#include <iostream>
#include <cstdint>
#include <curl/curl.h>
#include <map>
#include <archive.h>
#include <archive_entry.h>
#include <fstream>
#include <unordered_set>
#include <sys/stat.h>
#include "../temporary_logging_solution.h"

namespace fs = std::filesystem;


// By the time we reach this code, the bootstrap has already run so this should be safe
void Updater::LocateInstallDir() {
    auto home_env = std::string(getenv("HOME"));
    installDir = home_env.append("/.local/share/Steam");
}

// Verifies all known important binaries
bool Updater::BVerifyBinaries() {
    for (std::string file : filesToVerify) {
        if (!BVerifyFile(file)) {
            return false;
        }
    } 
    return true;
}

// Verifies a single file.
// If the file has a checksum, it is checked to make sure it matches the installed file (not implemented)
bool Updater::BVerifyFile(std::string path_s) {
    if (!fs::exists(installDir / path_s)) {
        std::cerr << "[Updater] File " << (installDir / path_s) << " failed verification: File does not exist." << std::endl;
        return false;
    }
    //TODO: verify sha of file
    return true;
}

// (Re)installs and downloads all important client files
void Updater::Update() {
    for (std::string foldername : foldersToCreate) {
        fs::create_directory(installDir / foldername);
    }

    // Set this just in case
    std::filesystem::current_path(installDir);
    
    InitCurl();
    for (std::string pkgname : filesToDownload) {
        MemoryStruct zipfile;
        zipfile.data = (char*)malloc(1);
        zipfile.size = 0; 
        try
        {
            DownloadFromServer(pkgname, zipfile);
        }
        catch(const std::exception& e)
        {
            std::cerr << "[Updater] Failed to download " << pkgname << "; " << e.what() << std::endl;
            continue;
        }
        auto archive = archive_read_new();
        archive_read_support_format_zip(archive);
        archive_read_support_filter_compress(archive);  
    
        archive_read_open_memory(archive, zipfile.data, zipfile.size);
        //archive_read_extract_set_progress_callback(arhive, )
        archive_entry *entry;
        while (archive_read_next_header(archive, &entry) == ARCHIVE_OK) {
            auto filename = archive_entry_pathname(entry);
            auto size = archive_entry_size(entry);
            mode_t perm = 0755;
            
            if (size > 0)
            {
                if (fileWhitelist.contains(filename)) {
                    auto fullpath = (installDir / filename);
                    std::cout << "[Updater] Installing " << filename << " with size " << size << std::endl;
                    char *buf;
                    buf = (char *)malloc(size);
                    archive_read_data(archive, buf, size);
                    std::ofstream file(fullpath);
                    file.write(reinterpret_cast<char*>(buf), size);
                    file.close();
                    chmod(fullpath.c_str(), perm);
                    free(buf);
                }
            }
        }
        free(zipfile.data);
        archive_free(archive);
    }
    curl_easy_cleanup(curl);
}

void Updater::InitCurl() {
    curl_global_init(CURL_GLOBAL_ALL);
    curl = curl_easy_init();
    curl_easy_setopt(curl, CURLOPT_USERAGENT, "opensteamclient-updater/1.0");
}

size_t Updater::write_data(void *contents, size_t size, size_t nmemb, void *userp)
{
    size_t realsize = size * nmemb;
    struct MemoryStruct *mem = (struct MemoryStruct *)userp;
    
    char *ptr = (char*)realloc(mem->data, mem->size + realsize + 1);
    if(!ptr) {
        /* out of memory! */
        printf("not enough memory (realloc returned NULL)\n");
        return 0;
    }
    
    mem->data = ptr;
    memcpy(&(mem->data[mem->size]), contents, realsize);
    mem->size += realsize;
    mem->data[mem->size] = 0;
    
    return realsize;
}

void Updater::DownloadFromServer(std::string file, MemoryStruct& dataOut) {
    std::cout << "[Updater] Downloading " << (baseUrl + file) << " started" << std::endl;
    curl_easy_setopt(curl, CURLOPT_URL, ( baseUrl + file).c_str());
    curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, write_data);
    curl_easy_setopt(curl, CURLOPT_WRITEDATA, &dataOut);
    auto result = curl_easy_perform(curl);

    /* check for errors */
    if(result != CURLE_OK) {
        std::cout << "[Updater] Curl error occurred: " << curl_easy_strerror(result) << std::endl;
    }
    else {
        std::cout << "[Updater] Downloaded successfully" << std::endl;
    }
}

void Updater::Verify(bool forceRedownload) {
    if (forceRedownload == true || !BVerifyBinaries()) {
        Update();
    }
}

void Updater::CopyOpensteamFiles() {

    for (std::tuple<std::string, std::string> file : filesToCopy) {
        auto sourceFile = fs::path(getenv("UPDATER_FILES_DIR")) / std::get<0>(file);
        auto destFile = installDir / std::get<1>(file);
        std::cout << "[Updater] Copying " << sourceFile << " to " << destFile << std::endl;
        try
        {
            fs::create_directory(destFile.parent_path());
            fs::copy_file(sourceFile, destFile, fs::copy_options::update_existing);
        }
        catch(const std::exception& e)
        {
            if (std::string(e.what()).contains("File exists")) {
                std::cout << destFile << " is up-to-date. " << std::endl;
            } else {
                std::cerr << "[Updater] Failed to copy " << destFile << " from " << sourceFile << ": " << e.what() << std::endl;
            }
        }
    }
}

Updater::Updater() {
    LocateInstallDir();

    DEBUG_MSG << "[Updater] InstallDir is " << installDir << std::endl;
    if (getenv("UPDATER_FILES_DIR") == NULL) {
        std::cerr << "[Updater] Updater files path is NULL" << std::endl;
    } else {
        DEBUG_MSG << "[Updater] Updater files path is " << getenv("UPDATER_FILES_DIR") << std::endl;
    }
    
}
Updater::~Updater() {}
