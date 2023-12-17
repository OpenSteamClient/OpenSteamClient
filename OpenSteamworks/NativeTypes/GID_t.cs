using System;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenSteamworks.NativeTypes;

[OpenSteamworks.Attributes.CustomValueType]
public struct GID_t : System.Numerics.IComparisonOperators<GID_t, GID_t, bool> {
    internal GID_t(UInt64 val) {
        this._value = val;
    }
    private UInt64 _value;
    public static implicit operator GID_t(UInt64 value) {
        return new GID_t(value);
    }

    public static implicit operator UInt64(GID_t me) {
        return me._value;
    }
    static bool IComparisonOperators<GID_t, GID_t, bool>.operator >(GID_t left, GID_t right)
    {
        return left._value > right._value;
    }

    static bool IComparisonOperators<GID_t, GID_t, bool>.operator >=(GID_t left, GID_t right)
    {
        return left._value >= right._value;
    }

    static bool IComparisonOperators<GID_t, GID_t, bool>.operator <(GID_t left, GID_t right)
    {
        return left._value < right._value;
    }

    static bool IComparisonOperators<GID_t, GID_t, bool>.operator <=(GID_t left, GID_t right)
    {
        return left._value <= right._value;
    }

    static bool IEqualityOperators<GID_t, GID_t, bool>.operator ==(GID_t left, GID_t right)
    {
        return left._value == right._value;
    }

    static bool IEqualityOperators<GID_t, GID_t, bool>.operator !=(GID_t left, GID_t right)
    {
        return left._value != right._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}