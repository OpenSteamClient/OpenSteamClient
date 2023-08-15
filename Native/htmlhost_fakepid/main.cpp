#include <stdio.h>
#include <iostream>
#include <thread>
#include <signal.h>
#include <link.h>
#include <unistd.h>
#include <iostream>
#include <fstream>

pid_t pidOfSteam = 0;

pid_t ReadPidFromPidFile() {
    char ch;
    
    char* homeEnv = getenv("HOME");
    if (homeEnv == nullptr) {
        std::cerr << "[htmlhost_fakepid] HOME not set" << std::endl;
        return 0;
    }

    std::string pidfilePath = std::string(homeEnv);
    pidfilePath.append("/.steam/steam.pid");

    std::ifstream pidfile(pidfilePath, std::ios::in);
 
    if (!pidfile.is_open()) {
        std::cout << "[htmlhost_fakepid] couldn't open " << pidfilePath << " for reading" << std::endl;
    }
    
    std::string pidStr;
    getline(pidfile, pidStr);

    return std::stoi(pidStr.c_str());
}

extern "C" pid_t getpid() {
    if (pidOfSteam == 0) {
        pidOfSteam = ReadPidFromPidFile();
    }

    std::cout << "[htmlhost_fakepid] getpid faked with " << pidOfSteam << std::endl;
    return pidOfSteam;
}