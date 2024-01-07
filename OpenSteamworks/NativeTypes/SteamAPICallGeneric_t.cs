
using System;
using System.Numerics;
using System.Runtime.InteropServices;

[OpenSteamworks.Attributes.CustomValueType]
public struct SteamAPICall_t<T> where T: struct {
    internal UIntPtr _value;
    public SteamAPICall_t(UIntPtr firstBits) {
        this._value = firstBits;
    }

    public static implicit operator SteamAPICall_t<T>(UIntPtr value) {
        return new SteamAPICall_t<T>(value);
    }

    public static implicit operator UIntPtr(SteamAPICall_t<T> me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}