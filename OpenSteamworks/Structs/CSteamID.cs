using System;

namespace OpenSteamworks.Structs;

public struct CSteamID {
    public CSteamID( UInt64 id )
	{
        steamid = id;
	}

    public UInt64 steamid;
}