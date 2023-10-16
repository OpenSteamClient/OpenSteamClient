using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct HHTMLBrowser {
    public HHTMLBrowser(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator HHTMLBrowser(UInt32 value) {
        return new HHTMLBrowser(value);
    }

    public static implicit operator UInt32(HHTMLBrowser me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}