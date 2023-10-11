using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct Int8 {
    internal Int8(SByte val) {
        this._value = val;
    }
    private SByte _value;
    public static explicit operator Int8(SByte value) {
        return new Int8(value);
    }

    public static implicit operator SByte(Int8 me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}