using System;
using System.Numerics;
using System.Runtime.InteropServices;


[OpenSteamworks.Attributes.CustomValueType]
public struct RemotePlaySessionID_t : System.Numerics.IComparisonOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool> {
    internal RemotePlaySessionID_t(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator RemotePlaySessionID_t(UInt32 value) {
        return new RemotePlaySessionID_t(value);
    }

    public static implicit operator UInt32(RemotePlaySessionID_t me) {
        return me._value;
    }
    static bool IComparisonOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool>.operator >(RemotePlaySessionID_t left, RemotePlaySessionID_t right)
    {
        return left._value > right._value;
    }

    static bool IComparisonOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool>.operator >=(RemotePlaySessionID_t left, RemotePlaySessionID_t right)
    {
        return left._value >= right._value;
    }

    static bool IComparisonOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool>.operator <(RemotePlaySessionID_t left, RemotePlaySessionID_t right)
    {
        return left._value < right._value;
    }

    static bool IComparisonOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool>.operator <=(RemotePlaySessionID_t left, RemotePlaySessionID_t right)
    {
        return left._value <= right._value;
    }

    static bool IEqualityOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool>.operator ==(RemotePlaySessionID_t left, RemotePlaySessionID_t right)
    {
        return left._value == right._value;
    }

    static bool IEqualityOperators<RemotePlaySessionID_t, RemotePlaySessionID_t, bool>.operator !=(RemotePlaySessionID_t left, RemotePlaySessionID_t right)
    {
        return left._value != right._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}