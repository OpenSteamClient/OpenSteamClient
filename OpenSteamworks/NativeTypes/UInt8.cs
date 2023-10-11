using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct UInt8 {
    internal UInt8(Byte val) {
        this._value = val;
    }
    private Byte _value;
    public static implicit operator UInt8(Byte value) {
        return new UInt8(value);
    }

    public static implicit operator Byte(UInt8 me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}
