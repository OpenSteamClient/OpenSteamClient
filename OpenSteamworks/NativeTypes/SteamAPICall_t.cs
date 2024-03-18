
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OpenSteamworks;
using OpenSteamworks.Callbacks;

[OpenSteamworks.Attributes.CustomValueType]
public struct SteamAPICall_t {
    internal UIntPtr _value;
    public SteamAPICall_t(UIntPtr firstBits) {
        this._value = firstBits;
    }

    public static implicit operator SteamAPICall_t(UIntPtr value) {
        return new SteamAPICall_t(value);
    }

    public static implicit operator UIntPtr(SteamAPICall_t me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}

[OpenSteamworks.Attributes.CustomValueType]
public struct SteamAPICall<T> where T: struct {
    internal UIntPtr _value;
    public SteamAPICall(UIntPtr firstBits) {
        this._value = firstBits;
    }

    public static implicit operator SteamAPICall<T>(UIntPtr value) {
        return new SteamAPICall<T>(value);
    }

    public static implicit operator UIntPtr(SteamAPICall<T> me) {
        return me._value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public readonly Task<CallResult<T>> Wait(CancellationToken cancellationToken = default)
    {
        return SteamClient.GetCallbackManager().WaitForAPICallResultAsync<T>(this._value, true, cancellationToken);
    }
}