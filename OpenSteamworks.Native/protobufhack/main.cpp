#ifdef _WIN32
#define EXPORT extern "C" __declspec(dllexport)
#endif

#if __linux__
#define EXPORT extern "C"
#endif

#include "generated/steammessages_client_objects.pb.h"

//TODO: auto generate allllll of these
EXPORT CMsgCellList *CMsgCellList_Construct() {
    return new CMsgCellList();
}

EXPORT size_t Protobuf_ByteSizeLong(google::protobuf::Message* msg) {
    return msg->ByteSizeLong();
}

EXPORT bool Protobuf_SerializeToArray(google::protobuf::Message* msg, void* buffer, size_t maxLen) {
    return msg->SerializeToArray(buffer, maxLen);
}

EXPORT CMsgCellList *CMsgCellList_Deserialize(void* buffer, int len) {
    CMsgCellList *msg = new CMsgCellList();
    if (!msg->ParseFromArray(buffer, len)) {
        return nullptr;
    }

    return msg;
}

EXPORT void CMsgCellList_Delete(CMsgCellList* ptr) {
    delete ptr;
}