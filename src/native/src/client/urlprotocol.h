#include <string>
#include <vector>

#pragma once

enum URLProtocolCommand {
    k_URLProtocolCommandRun = 1
};

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
class URLProtocolHandler
{
private:
    
public:
    std::string protocolPrefix;
    void ProcessLink(std::string url);
    bool IsValidUrl(std::string url);
    // Splits a protocol string into parts.
    // DOES NOT PERFORM VALIDATION, validate before calling!
    std::vector<std::string> SplitToParts(std::string url);
    URLProtocolHandler();
    ~URLProtocolHandler();
};