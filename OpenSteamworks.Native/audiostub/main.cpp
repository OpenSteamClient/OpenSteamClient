#include <cstdlib>
#include <iostream>

enum EAudioDeviceType
{
    k_EAudioDeviceTypeGeneric = 0,
    k_EAudioDeviceTypeVoice = 1,
    k_EAudioDeviceTypeMusic = 2
};

extern "C" void* AudioSinkList() {
    std::cout << "[libaudiostub] Tried to get audio sink list " << std::endl;
    return nullptr;
}

extern "C" void* CreateInterface(const char* iface, bool* bSuccess) {
    if (iface != nullptr) {
        std::cout << "[libaudiostub] Tried to get interface " << iface << std::endl;
    } else {
        std::cout << "[libaudiostub] Tried to get NULL interface " << std::endl;
    }
    
    if (bSuccess != nullptr) {
        *bSuccess = false;
    }

    return nullptr;
}

extern "C" double* CreateMilesMusicAudioDevice(void* unk1, void* unk2, void* unk3) {
    std::cout << "[libaudiostub] Tried to create miles music device" << std::endl;
    return nullptr;
}

extern "C" double* CreateMilesSoundAudioDevice(void* unk1, void* unk2, void* unk3) {
    std::cout << "[libaudiostub] Tried to create miles sound device" << std::endl;
    return nullptr;
}

extern "C" double* CreateMilesVoiceAudioDevice(void* unk1, void* unk2, void* unk3) {
    std::cout << "[libaudiostub] Tried to create miles voice device" << std::endl;
    return nullptr;
}

extern "C" void FreeAudioDevice(void* unk1) {
    std::cout << "[libaudiostub] Tried to free sound device" << std::endl;
}

