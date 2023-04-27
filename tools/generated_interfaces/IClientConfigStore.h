#ifndef ICLIENTCONFIGSTORE_H
#define ICLIENTCONFIGSTORE_H
#ifndef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class UNSAFE_INTERFACE IClientConfigStore
{
public:
     virtual void IsSet() = 0; //args: 2, index: 0
     virtual void GetBool() = 0; //args: 3, index: 1
     virtual void GetInt() = 0; //args: 3, index: 2
     virtual void GetUint64() = 0; //args: 4, index: 3
     virtual void GetFloat() = 0; //args: 3, index: 4
     virtual void GetString() = 0; //args: 3, index: 5
     virtual void GetBinary() = 0; //args: 4, index: 6
     virtual void GetBinary() = 0; //args: 3, index: 7
     virtual void GetBinaryWatermarked() = 0; //args: 4, index: 8
     virtual void SetBool() = 0; //args: 3, index: 9
     virtual void SetInt() = 0; //args: 3, index: 10
     virtual void SetUint64() = 0; //args: 4, index: 11
     virtual void SetFloat() = 0; //args: 3, index: 12
     virtual void SetString() = 0; //args: 3, index: 13
     virtual void SetBinary() = 0; //args: 4, index: 14
     virtual void SetBinaryWatermarked() = 0; //args: 4, index: 15
     virtual void RemoveKey() = 0; //args: 2, index: 16
     virtual void GetKeySerialized() = 0; //args: 4, index: 17
     virtual void FlushToDisk() = 0; //args: 1, index: 18
};
#endif // ICLIENTCONFIGSTORE_H
