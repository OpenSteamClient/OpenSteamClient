#include <string>
#include "globals.h"
#include "backwardscompatibility.h"
#include "interop/appmanager.h"
#include "urlprotocol.h"
#include <sstream>

// We aim to support the protocol fully eventually
// See https://developer.valvesoftware.com/wiki/Steam_browser_protocol
//TODO: important protocols we should get working
// steam://connect/12.123.456.789 (this might be difficult)
// steam://run/730
// steam://rungameid/730
// steam://openurl/
// steam://store/730
// steam://uninstall/730 (popup and ask for delete, add feature flag in the future if people want to uninstall without a popup)
// steam://open/
void URLProtocolHandler::ProcessLink(std::string url)
{
    URLProtocolCommand cmd;
    if (!IsValidUrl(url))
        return;

    auto commandv = SplitToParts(url);
    for (auto str : commandv)
    {
        DEBUG_MSG << str << std::endl;
    }
    if (commandv[0] == "run" || commandv[0] == "rungameid") {
        cmd = k_URLProtocolCommandRun;
    }
    switch (cmd)
    {
        case k_URLProtocolCommandRun:
            AppId_t appid;
            try
            {
                appid = stoi(commandv[1]);
            }
            catch(const std::exception& e)
            {
                std::cerr << "Failed to convert " << commandv[1] << " to an AppId_t; " << e.what() << std::endl;
                return;
            }
            if (appid) {
                
            } else {
                std::cerr << "appid was empty" << std::endl;
            }
            
        break;

        default:
            std::cerr << "unknown command " << url << std::endl;
        break;
    }
}
bool URLProtocolHandler::IsValidUrl(std::string url) {
    if (!url.starts_with(protocolPrefix))
        return false;

    //TODO: specific checks for different URLs
    return true;
}
// Splits a protocol string into parts.
// DOES NOT PERFORM VALIDATION, validate before calling!
std::vector<std::string> URLProtocolHandler::SplitToParts(std::string url)
{
    std::string segment;
    std::vector<std::string> parts;
    std::string urlWithoutPrefix = url.erase(0, protocolPrefix.length());

    // ugh, unnecessary stream
    std::stringstream sstream(urlWithoutPrefix);
    while (std::getline(sstream, segment, '/'))
    {
        parts.push_back(segment);
    }
    return parts;
}

URLProtocolHandler::URLProtocolHandler() {
    protocolPrefix = "steam://";
    BackwardsCompatibilityMgr::AddBackwardCompatibility("protocolhandler:allowexitsteam", "Allow steam://ExitSteam protocol", "If this feature is enabled, running steam://ExitSteam will quit the running instance of OpenSteam.");
}

URLProtocolHandler::~URLProtocolHandler() {

}