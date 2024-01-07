using System;
using System.Numerics;
using System.Runtime.InteropServices;


[OpenSteamworks.Attributes.CustomValueType]
public struct DepotId_t : System.Numerics.IComparisonOperators<DepotId_t, DepotId_t, bool> {
    internal DepotId_t(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator DepotId_t(UInt32 value) {
        return new DepotId_t(value);
    }

    public static implicit operator UInt32(DepotId_t me) {
        return me._value;
    }
    static bool IComparisonOperators<DepotId_t, DepotId_t, bool>.operator >(DepotId_t left, DepotId_t right)
    {
        return left._value > right._value;
    }

    static bool IComparisonOperators<DepotId_t, DepotId_t, bool>.operator >=(DepotId_t left, DepotId_t right)
    {
        return left._value >= right._value;
    }

    static bool IComparisonOperators<DepotId_t, DepotId_t, bool>.operator <(DepotId_t left, DepotId_t right)
    {
        return left._value < right._value;
    }

    static bool IComparisonOperators<DepotId_t, DepotId_t, bool>.operator <=(DepotId_t left, DepotId_t right)
    {
        return left._value <= right._value;
    }

    static bool IEqualityOperators<DepotId_t, DepotId_t, bool>.operator ==(DepotId_t left, DepotId_t right)
    {
        return left._value == right._value;
    }

    static bool IEqualityOperators<DepotId_t, DepotId_t, bool>.operator !=(DepotId_t left, DepotId_t right)
    {
        return left._value != right._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}