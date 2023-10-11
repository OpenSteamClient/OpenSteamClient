using System;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct AppId_t : System.Numerics.IComparisonOperators<AppId_t, AppId_t, bool> {
    internal AppId_t(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator AppId_t(UInt32 value) {
        return new AppId_t(value);
    }

    public static implicit operator UInt32(AppId_t me) {
        return me._value;
    }
    static bool IComparisonOperators<AppId_t, AppId_t, bool>.operator >(AppId_t left, AppId_t right)
    {
        return left._value > right._value;
    }

    static bool IComparisonOperators<AppId_t, AppId_t, bool>.operator >=(AppId_t left, AppId_t right)
    {
        return left._value >= right._value;
    }

    static bool IComparisonOperators<AppId_t, AppId_t, bool>.operator <(AppId_t left, AppId_t right)
    {
        return left._value < right._value;
    }

    static bool IComparisonOperators<AppId_t, AppId_t, bool>.operator <=(AppId_t left, AppId_t right)
    {
        return left._value <= right._value;
    }

    static bool IEqualityOperators<AppId_t, AppId_t, bool>.operator ==(AppId_t left, AppId_t right)
    {
        return left._value == right._value;
    }

    static bool IEqualityOperators<AppId_t, AppId_t, bool>.operator !=(AppId_t left, AppId_t right)
    {
        return left._value != right._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}