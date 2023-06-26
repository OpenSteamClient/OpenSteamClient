#include "jslikebuffer.h"

// TODO: this doesn't belong here (maybe a separate buffer class?)
// bufLen: length of the buffer
// pos: current cursor position, advances by the length of an uint32 if the read is possible
uint32_t BufferHelpers::ReadUint32(const char* buffer, size_t bufLen, size_t *pos) {
    if (pos == nullptr) {
        std::cerr << "[BufferHelpers] Current cursor position is nullptr" << std::endl;
        return -1;
    }

    if (*pos >= bufLen) {
        std::cerr << "[BufferHelpers] Current cursor position is at or over the end of the buffer" << std::endl;
        return -1;
    }

    if ((*pos + sizeof(uint32_t)) > bufLen) {
        std::cerr << "[BufferHelpers] Read would have exceeded bufLen" << std::endl;
        return -1;
    }

    uint32_t value;
    memcpy(&value, buffer + *pos, sizeof(uint32_t));
    *pos += sizeof(uint32_t);
    return value;
}

// TODO: this doesn't belong here (maybe a separate buffer class?)
// bufLen: length of the buffer
// pos: current cursor position, advances by the length of an uint32 if the read is possible
uint8_t BufferHelpers::ReadUint8(const char* buffer, size_t bufLen, size_t *pos) {
    if (pos == nullptr) {
        std::cerr << "[BufferHelpers] Current cursor position is nullptr" << std::endl;
        return -1;
    }

    if (*pos >= bufLen) {
        std::cerr << "[BufferHelpers] Current cursor position is at or over the end of the buffer" << std::endl;
        return -1;
    }

    if ((*pos + sizeof(uint8_t)) > bufLen) {
        std::cerr << "[BufferHelpers] Read would have exceeded bufLen" << std::endl;
        return -1;
    }

    uint8_t value;
    memcpy(&value, buffer + *pos, sizeof(uint8_t));
    *pos += sizeof(uint8_t);
    return value;
}

// TODO: this doesn't belong here (maybe a separate buffer class?)
// bufLen: length of the buffer
// pos: current cursor position, advances by the length of an uint32 if the read is possible
// length: how much data to copy
// Returns: char* with the data
char* BufferHelpers::ReadLength(const char* buffer, size_t bufLen, size_t *pos, size_t length) {
    if (pos == nullptr) {
        std::cerr << "[BufferHelpers] Current cursor position is nullptr" << std::endl;
        return nullptr;
    }

    if (*pos >= bufLen) {
        std::cerr << "[BufferHelpers] Current cursor position is at or over the end of the buffer" << std::endl;
        return nullptr;
    }

    if ((*pos + length) > bufLen)
    {
        std::cerr << "[BufferHelpers] Read would have exceeded bufLen" << std::endl;
        return nullptr;
    }

    char *value = (char*)malloc(length);
    memcpy(value, buffer + *pos, length);
    *pos += length;
    return value;
}