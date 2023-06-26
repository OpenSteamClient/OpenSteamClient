#include "pidutils.h"
#include <string>
#include <stdio.h>
#include <unistd.h>
#include <fstream>

std::string PIDUtils::GetPid() {
    return std::to_string(getpid());
}
// THIS FUNCTION IS INCOMPLETE
std::string PIDUtils::GetPidOfProcess(std::string procName) {
    char line[sizeof(pid_t)];
    FILE *cmd = popen(std::string("pidof -s ").append(procName).c_str(), "r");

    fgets(line, sizeof(pid_t), cmd);
    pid_t pid = strtoul(line, NULL, 10);
    
    pclose(cmd);
    return std::to_string(pid);
}