EXPORT size_t Protobuf_ByteSizeLong(google::protobuf::Message* msg) {
    return msg->ByteSizeLong();
}

EXPORT bool Protobuf_SerializeToArray(google::protobuf::Message* msg, void* buffer, size_t maxLen) {
    return msg->SerializeToArray(buffer, maxLen);
}