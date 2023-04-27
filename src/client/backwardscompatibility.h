#pragma once

// Manages our "Backwards compatible mode" 
class BackwardsCompatibilityMgr
{
private:
    
public:
    //TODO: list of backwards compatible things

    //TODO: static for now
    static void AddBackwardCompatibility(std::string featureKey, std::string featureName, std::string featureDesc) {

    }
    static bool IsInBackwardCompatibleMode(std::string featureKey) {
        //TODO: logic for this
        return false;
    }
    static void SetBackwardCompatibilityMode(std::string featureKey, bool bIsBackwardCompatible) {
        //TODO: logic for this
    }
    BackwardsCompatibilityMgr() {}
    ~BackwardsCompatibilityMgr() {}
};