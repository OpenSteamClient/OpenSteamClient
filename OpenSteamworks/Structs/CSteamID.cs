using System;
using OpenSteamworks.Converters;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[JsonConverter(typeof(CSteamIDJsonConverter))]
public struct CSteamID {
    public UInt64 SteamID64;
    /// <summary>
    /// The "magic" used to calculate to and from an accountid.
    /// Assumes default universe, default account instance and default account type
    /// </summary>
    public const UInt64 STEAMID_ACCOUNTID_BITS = 76561197960265728;
    public CSteamID( UInt64 id )
	{
        SteamID64 = id;
	}

    public CSteamID( string steamidAsStr )
	{
		SteamID64 = UInt64.Parse(steamidAsStr);
	}

    private static CSteamID InterfaceDebuggerSupport(string dbgStr) {
        return new CSteamID(UInt64.Parse(dbgStr));
    }

	public override readonly string ToString() {
		return SteamID64.ToString();
	}

    public UInt32 GetAccountId() {
        return (uint)(SteamID64 - STEAMID_ACCOUNTID_BITS);
    }

    public static CSteamID FromAccountID(UInt32 accountID) {
        return new CSteamID((UInt64)accountID + STEAMID_ACCOUNTID_BITS);
    }

    public static implicit operator CSteamID(UInt64 v)
    {
        return new CSteamID(v);
    }
	public static implicit operator UInt64(CSteamID v)
    {
        return v.SteamID64;
    }
}