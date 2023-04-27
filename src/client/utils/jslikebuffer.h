#include <cstdint>
#include <stdlib.h>
#include <iostream>
#include <cstring>

namespace BufferHelpers {
    char *ReadLength(const char *buffer, size_t bufLen, size_t *pos, size_t length);
    uint32_t ReadUint32(const char *buffer, size_t bufLen, size_t *pos);
    uint8_t ReadUint8(const char *buffer, size_t bufLen, size_t *pos);

    template<typename IN, typename OUT>
    OUT ReadAbstract(const IN *buffer, size_t bufLen, size_t *pos) {
        if (pos == nullptr) {
            std::cerr << "Current cursor position is nullptr" << std::endl;
            return -1;
        }

        if (*pos >= bufLen) {
            std::cerr << "Current cursor position is at or over the end of the buffer" << std::endl;
            return -1;
        }

        if ((*pos + sizeof(OUT)) > bufLen) {
            std::cerr << "Read would have exceeded bufLen" << std::endl;
            return -1;
        }

        OUT value;
        memcpy(&value, buffer + *pos, sizeof(OUT));
        *pos += sizeof(OUT);
        return value;
    }
}
