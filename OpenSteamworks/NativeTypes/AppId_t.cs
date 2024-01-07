using System;
using System.Numerics;
using System.Runtime.InteropServices;

[OpenSteamworks.Attributes.CustomValueType]
public struct AppId_t : System.Numerics.IComparisonOperators<AppId_t, AppId_t, bool>, System.IEquatable<AppId_t>, System.IComparable<AppId_t> {
    public static readonly AppId_t Invalid = new(0x0);
    private uint _value;
    
    public AppId_t(uint value) {
        _value = value;
    }

    public readonly override string ToString() {
        return _value.ToString();
    }

    public readonly override bool Equals(object? other) {
        return other is not null && other is AppId_t t && this == t;
    }

    public readonly override int GetHashCode() {
        return _value.GetHashCode();
    }

    public static bool operator ==(AppId_t x, AppId_t y) {
        return x._value == y._value;
    }

    public static bool operator !=(AppId_t x, AppId_t y) {
        return !(x == y);
    }

    public static bool operator >(AppId_t left, AppId_t right)
    {
        return left._value > right._value;
    }

    public static bool operator >=(AppId_t left, AppId_t right)
    {
        return left._value >= right._value;
    }

    public static bool operator <(AppId_t left, AppId_t right)
    {
        return left._value < right._value;
    }

    public static bool operator <=(AppId_t left, AppId_t right)
    {
        return left._value <= right._value;
    }

    public static implicit operator AppId_t(uint value) {
        return new AppId_t(value);
    }

    public static implicit operator uint(AppId_t that) {
        return that._value;
    }

    public readonly bool Equals(AppId_t other) {
        return _value == other._value;
    }

    public readonly int CompareTo(AppId_t other) {
        return _value.CompareTo(other._value);
    }
}