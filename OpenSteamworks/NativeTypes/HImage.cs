using System;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenSteamworks.Attributes;


[CustomValueType]
public struct HImage : System.Numerics.IComparisonOperators<HImage, HImage, bool> {
    internal HImage(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator HImage(UInt32 value) {
        return new HImage(value);
    }

    public static implicit operator UInt32(HImage me) {
        return me._value;
    }
    static bool IComparisonOperators<HImage, HImage, bool>.operator >(HImage left, HImage right)
    {
        return left._value > right._value;
    }

    static bool IComparisonOperators<HImage, HImage, bool>.operator >=(HImage left, HImage right)
    {
        return left._value >= right._value;
    }

    static bool IComparisonOperators<HImage, HImage, bool>.operator <(HImage left, HImage right)
    {
        return left._value < right._value;
    }

    static bool IComparisonOperators<HImage, HImage, bool>.operator <=(HImage left, HImage right)
    {
        return left._value <= right._value;
    }

    static bool IEqualityOperators<HImage, HImage, bool>.operator ==(HImage left, HImage right)
    {
        return left._value == right._value;
    }

    static bool IEqualityOperators<HImage, HImage, bool>.operator !=(HImage left, HImage right)
    {
        return left._value != right._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}