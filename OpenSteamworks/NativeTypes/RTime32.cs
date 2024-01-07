using System;
using System.Runtime.InteropServices;


[OpenSteamworks.Attributes.CustomValueType]
public struct RTime32 {
    internal RTime32(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator RTime32(UInt32 value) {
        return new RTime32(value);
    }

    public static implicit operator UInt32(RTime32 me) {
        return me._value;
    }

    public static explicit operator DateTime(RTime32 me) {
        return DateTimeOffset.FromUnixTimeSeconds(me._value).DateTime;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}