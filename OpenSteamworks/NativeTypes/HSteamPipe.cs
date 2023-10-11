using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct HSteamPipe {
    public HSteamPipe(Int32 val) {
        this._value = val;
    }
    private Int32 _value;
    public static implicit operator HSteamPipe(Int32 value) {
        return new HSteamPipe(value);
    }

    public static implicit operator Int32(HSteamPipe me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}