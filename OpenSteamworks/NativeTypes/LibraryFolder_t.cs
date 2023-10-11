using System;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct LibraryFolder_t {
    internal LibraryFolder_t(Int32 val) {
        this._value = val;
    }
    private Int32 _value;
    public static implicit operator LibraryFolder_t(Int32 value) {
        return new LibraryFolder_t(value);
    }

    public static implicit operator Int32(LibraryFolder_t me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}