using System;
using OpenSteamworks.Converters;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[JsonConverter(typeof(CSteamIDJsonConverter))]
public struct CSteamID {
    public CSteamID( UInt64 id )
	{
        steamid = id;
	}

    public CSteamID( string steamidAsStr )
	{
		steamid = UInt64.Parse(steamidAsStr);
	}

	public override readonly string ToString() {
		return steamid.ToString();
	}

    public UInt64 steamid;

    public UInt32 GetAccountId() {
        return (uint)(steamid - 76561197960265728);
    }

    public static implicit operator CSteamID(UInt64 v)
    {
        return new CSteamID(v);
    }
	public static implicit operator UInt64(CSteamID v)
    {
        return v.steamid;
    }
}