#include <string>
#include <cstdint>
#pragma once

struct LaunchOption
{
    int index;

    // Used for UI purposes
    int positionInList;
    bool isDefault;
    std::string name;

    // Requirements
    std::string oslist;
    std::string realm;
    uint32_t ownsdlc;
    std::string BetaKey;
};
