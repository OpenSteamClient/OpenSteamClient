using System;
using System.Numerics;
using System.Runtime.InteropServices;


[OpenSteamworks.Attributes.CustomValueType]
public struct AccountID_t : System.Numerics.IComparisonOperators<AccountID_t, AccountID_t, bool> {
    internal AccountID_t(UInt32 val) {
        this._value = val;
    }
    private UInt32 _value;
    public static implicit operator AccountID_t(UInt32 value) {
        return new AccountID_t(value);
    }

    public static implicit operator UInt32(AccountID_t me) {
        return me._value;
    }
    static bool IComparisonOperators<AccountID_t, AccountID_t, bool>.operator >(AccountID_t left, AccountID_t right)
    {
        return left._value > right._value;
    }

    static bool IComparisonOperators<AccountID_t, AccountID_t, bool>.operator >=(AccountID_t left, AccountID_t right)
    {
        return left._value >= right._value;
    }

    static bool IComparisonOperators<AccountID_t, AccountID_t, bool>.operator <(AccountID_t left, AccountID_t right)
    {
        return left._value < right._value;
    }

    static bool IComparisonOperators<AccountID_t, AccountID_t, bool>.operator <=(AccountID_t left, AccountID_t right)
    {
        return left._value <= right._value;
    }

    static bool IEqualityOperators<AccountID_t, AccountID_t, bool>.operator ==(AccountID_t left, AccountID_t right)
    {
        return left._value == right._value;
    }

    static bool IEqualityOperators<AccountID_t, AccountID_t, bool>.operator !=(AccountID_t left, AccountID_t right)
    {
        return left._value != right._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}