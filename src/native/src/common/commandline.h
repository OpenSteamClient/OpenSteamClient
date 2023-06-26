#include <string>
#include <map>

#pragma once

// A utility class that parses argv and argc
// Also invokes the protocol handler if a link is sent here
class CommandLine
{
private:
    std::map<std::string, std::string> launchargs;
public:
    int argc;
    char **argv;

    // This should only be called once and at the start of the application.
    void SetLaunchCommandLine(int argc, char *argv[]);
    // Call this function once we're fully initialized
    // This can read the -applaunch param for example
    void ProcessCommandLine(int argc, char *argv[], bool noProtocolHandler = false);
    std::string GetOption(std::string option);
    bool HasOption(std::string option);

    CommandLine();
    ~CommandLine();
};