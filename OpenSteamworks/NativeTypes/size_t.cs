using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct size_t {
    internal size_t(UIntPtr val) {
        this._value = val;
    }
    private UIntPtr _value;
    public static implicit operator size_t(UIntPtr value) {
        return new size_t(value);
    }

    public static implicit operator UIntPtr(size_t me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}