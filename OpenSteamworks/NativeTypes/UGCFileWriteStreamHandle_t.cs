
using System;
using System.Numerics;
using System.Runtime.InteropServices;

[OpenSteamworks.Attributes.CustomValueType]
public struct UGCFileWriteStreamHandle_t {
    internal UInt64 _value;
    public UGCFileWriteStreamHandle_t(UInt64 firstBits) {
        this._value = firstBits;
    }

    public static implicit operator UGCFileWriteStreamHandle_t(UInt64 value) {
        return new UGCFileWriteStreamHandle_t(value);
    }

    public static implicit operator UInt64(UGCFileWriteStreamHandle_t me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}