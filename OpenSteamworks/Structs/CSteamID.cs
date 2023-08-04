using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CSteamID {
    public CSteamID( UInt64 id )
	{
        steamid = id;
	}

    public CSteamID( string appidAsStr )
	{
		steamid = UInt64.Parse(appidAsStr);
	}

    public UInt64 steamid;
}