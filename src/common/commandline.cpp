#include "commandline.h"
#include <string>
#include <map>

// This should only be called once and at the start of the application.
void CommandLine::SetLaunchCommandLine(int argc, char *argv[]) {
    this->argc = argc;
    this->argv = argv;
    for (size_t i = 0; i < argc; i++)
    {
        auto option = std::string(argv[i]);
        if (option.starts_with("--")) {
            std::string val = "";

            // Checks if the option has a value
            if ( (i+1) <= argc && argv[i+1] != NULL && !std::string(argv[i+1]).starts_with("--")) {
                val = std::string(argv[i+1]);
            }
            launchargs.insert({option, val});
        }
    }
}
// Call this function once we're fully initialized
// This can read the -applaunch param for example
void CommandLine::ProcessCommandLine(int argc, char *argv[], bool noProtocolHandler) {

}
std::string CommandLine::GetOption(std::string option) {
    if (!HasOption(option)) {
        return "";
    }
    return launchargs[option];
}
bool CommandLine::HasOption(std::string option) {
    return launchargs.contains(option);
}

CommandLine::CommandLine() {}
CommandLine::~CommandLine() {}
