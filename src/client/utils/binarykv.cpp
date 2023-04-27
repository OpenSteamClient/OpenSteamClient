#include "binarykv.h"
#include "jslikebuffer.h"
#include <map>
#include <iostream>

// Made from the below implementation of BinaryKVs, thanks DoctorMcKay!
// https://github.com/DoctorMcKay/node-binarykvparser/blob/master/index.js

enum BinaryKVType {
    None = 0,
    String = 1,
    Int32 = 2,
    Float32 = 3,
    Pointer = 4,
    WideString = 5,
    Color = 6,
    UInt64 = 7,
    SignedInt64 = 10,
    End = 8,
};

// This function is used exclusively for BinaryKV
static std::string ReadCString(const uint8_t *buffer, size_t bufLen, size_t *pos) {
    if (pos == nullptr) {
        std::cerr << "Current cursor position is nullptr" << std::endl;
        return "";
    }

    if (*pos >= bufLen) {
        std::cerr << "Current cursor position is at or over the end of the buffer" << std::endl;
        return "";
    }

    char* ptrToString = (char*)(buffer + *pos);
    size_t len = strlen(ptrToString);

    if ((*pos + len) > bufLen)
    {
        std::cerr << "Read would have exceeded bufLen" << std::endl;
        return "";
    }

    *pos += len+1;

    return std::string(ptrToString, len);
}

nlohmann::json ParseBinary(std::vector<uint8_t> data, size_t *offset) {
    nlohmann::json jsonObj;
    uint8_t* buf = data.data();
    size_t bufSize = data.size();

    while (true)
    {
        std::string name;
        uint8_t type = BufferHelpers::ReadAbstract<uint8_t, uint8_t>(buf, bufSize, offset);

        if (type == BinaryKVType::End) {
		    break;
        }

        name = ReadCString(buf, bufSize, offset);
		
		if (type == BinaryKVType::None && name.empty() && jsonObj.size() == 0) {
			// Root node
			name = ReadCString(buf, bufSize, offset);
		}

        if (name.empty()) {
            name = "UnnamedKey";
        }

        switch ((BinaryKVType)type)
        {

        case BinaryKVType::None:
        {
            nlohmann::json intermediate = ParseBinary(data, offset);
            bool hasNonNumberKeys = false;
            nlohmann::json array = nlohmann::json::array();

            bool isFirstItem = true;
            for (auto& val : intermediate.items()) {
                char* p;
                long converted = strtoul(val.key().c_str(), &p, 10);

                if (*p) {
                    // key is not a number
                    hasNonNumberKeys = true;
                    break;
                } else {
                    // key is a number

                    if (isFirstItem) {
                        // An array will always start with a zero. If it starts with any other number, it's not an array.
                        if (converted != 0) {
                            hasNonNumberKeys = true;
                            break;
                        }
                        isFirstItem = false;
                    }

                    array.push_back(val.value());
                }
            }

            if (hasNonNumberKeys) {
                jsonObj[name] = intermediate;
            } else {
                jsonObj[name] = array;
            }
            
            break;
        }
        case BinaryKVType::String:
            jsonObj[name] = ReadCString(buf, bufSize, offset);
            break;

        case BinaryKVType::Int32:
        case BinaryKVType::Color:
        case BinaryKVType::Pointer:
            jsonObj[name] = BufferHelpers::ReadAbstract<uint8_t, int32_t>(buf, bufSize, offset);
            break;

        case BinaryKVType::UInt64:
            jsonObj[name] = BufferHelpers::ReadAbstract<uint8_t, uint64_t>(buf, bufSize, offset);
            break;

        case BinaryKVType::SignedInt64:
            jsonObj[name] = BufferHelpers::ReadAbstract<uint8_t, int64_t>(buf, bufSize, offset);
            break;

        case BinaryKVType::Float32:
            // Ensure that the float is 32 bit
            static_assert(sizeof(float) * CHAR_BIT == 32, "require 32 bits floats");

            jsonObj[name] = BufferHelpers::ReadAbstract<uint8_t, float>(buf, bufSize, offset);
            break;

        default:
            std::cerr << "Unhandled BinaryKVType " << (BinaryKVType)type << std::endl;
            break;
        }
    }
    return jsonObj;
}

BinaryKV::BinaryKV(std::vector<uint8_t> data)
{
    size_t startSize = 0;
    outputJSON = ParseBinary(data, &startSize);
}

BinaryKV::~BinaryKV()
{
}
