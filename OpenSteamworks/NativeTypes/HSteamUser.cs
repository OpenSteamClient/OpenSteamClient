using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct HSteamUser {
    public HSteamUser(Int32 val) {
        this._value = val;
    }
    private Int32 _value;
    public static implicit operator HSteamUser(Int32 value) {
        return new HSteamUser(value);
    }

    public static implicit operator Int32(HSteamUser me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}