#include <stdio.h>
#include <fstream>
#include <iostream>
#include <filesystem>
#include <signal.h>
#include <fcntl.h>

#pragma once

class SingleInstanceMgr
{
private:
    std::string thispid_s;
    pid_t thispid;
    std::string instancepid_s;
    pid_t instancepid;
    std::string pidfile_path;
    std::string pipefile_path;
    void LocatePidFile();
    void LocatePipeFile();

public:
    SingleInstanceMgr(std::string thispid);
    bool BCheckForInstance();

    // Works by:
    // echo "steam -applaunch 730" > ~/.steam/steam.pipe
    void SendArgvToInstance(int argc, char *argv[]);

    std::string GetInstancePid();

    //NOTE: doesn't start the pipe, this happens later
    void SetThisProcessAsInstance();

    ~SingleInstanceMgr();
};