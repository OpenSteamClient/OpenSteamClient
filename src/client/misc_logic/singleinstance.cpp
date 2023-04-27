#include "singleinstance.h"
#include "../globals.h"
#include <stdio.h>
#include <fstream>
#include <iostream>
#include <filesystem>
#include <signal.h>
#include <fcntl.h>

namespace fs = std::filesystem;

void SingleInstanceMgr::LocatePidFile() {
    auto home_env = std::string(getenv("HOME"));
    pidfile_path = home_env.append("/.steam/steam.pid");
}
void SingleInstanceMgr::LocatePipeFile() {
    auto home_env = std::string(getenv("HOME"));
    pipefile_path = home_env.append("/.steam/steam.pipe");
}

SingleInstanceMgr::SingleInstanceMgr(std::string thispid) {
    LocatePidFile();
    LocatePipeFile();
    thispid_s = thispid;
    this->thispid = stoi(thispid);
}
bool SingleInstanceMgr::BCheckForInstance() {
    if (!fs::exists(pidfile_path)) {
        DEBUG_MSG << "PID file " << pidfile_path << " does not exist!" << std::endl;
        return false;
    }

    try {
        //TODO: make this an absolute path (and check it exists)
        std::ifstream pidfile(pidfile_path);

        // The file should have only one line, and that is it's PID
        getline(pidfile, instancepid_s);
    } 
    catch(const std::exception& e)
    {
        std::cerr << "Failed to read PID file " << pidfile_path << "; " << e.what() << std::endl;
        return false;
    }

    try
    {
        instancepid = stoi(instancepid_s);
    }
    catch(const std::exception& e)
    {
        std::cerr << "Failed to convert PID " << instancepid_s << " to int; " << e.what() << std::endl;
        return false;
    }

    // This shouldn't be possible, but just in case
    if (instancepid != thispid) {
        // Checks if a process is running with the PID
        if (0 == kill(instancepid, 0))
        {
            fs::path otherProcessPath = fs::read_symlink(std::string("/proc/").append(std::to_string(instancepid)).append("/exe"));
            DEBUG_MSG << "Other process filename " << otherProcessPath.filename() << std::endl;
            if (otherProcessPath.filename() == "steam") {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    return false;
}

// Works by:
// echo "steam -applaunch 730" > ~/.steam/steam.pipe
void SingleInstanceMgr::SendArgvToInstance(int argc, char *argv[]) {
    std::string argsAsString;

    // Concat the argv to a single string for sending
    for (size_t i = 0; i < argc; i++)
    {
        argsAsString += argv[i];
        argsAsString += " ";
    }
    argsAsString += "\n";

    if (Global_debugCbLogging) {
        DEBUG_MSG << "full C string " << argsAsString << std::endl;
    }
    
    // Open the named pipe
    // When writing, open with O_WRONLY
    auto fd = open(pipefile_path.c_str(), O_WRONLY); 

    if (Global_debugCbLogging) {
        DEBUG_MSG << "fd for pipe is " << fd << std::endl;
    }

    if (fd == -1) {
        perror("open");
        exit(EXIT_FAILURE);
    }

    if (Global_debugCbLogging) {
        DEBUG_MSG << "Data to write: " << argsAsString.length() << std::endl;
    }
    
    // set this explicitly here just incase
    signal(SIGPIPE, SIG_IGN);

    // use data instead of c_str for pipes, as NULL ending up in a pipe breaks it silently (even this? guess C++ is a fragile language)
    auto wresult = write(fd, argsAsString.data(), argsAsString.size());
    
    if (wresult == -1)
    {
        perror("write");
        exit(EXIT_FAILURE);
    }

    if (Global_debugCbLogging) {
        DEBUG_MSG << "Result: " << wresult << std::endl;
    }
    
    // We should close it (why _exactly_?)
    if (close(fd) == -1) {
        perror("close");
        exit(EXIT_FAILURE);
    }
}

std::string SingleInstanceMgr::GetInstancePid() {
    return instancepid_s;
}

//NOTE: doesn't start the pipe, this happens later
void SingleInstanceMgr::SetThisProcessAsInstance() {
    std::ofstream pidfile;
    pidfile.open(pidfile_path, std::ios::trunc);
    pidfile << thispid_s;
    pidfile.close();
    instancepid_s = thispid_s;
    instancepid = thispid;
}

SingleInstanceMgr::~SingleInstanceMgr() {}