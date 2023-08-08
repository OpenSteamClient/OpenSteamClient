using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CGameID {
    public CGameID( AppId_t appid )
	{
		gameid = appid;
	}

	public CGameID( string appidAsStr )
	{
		gameid = AppId_t.Parse(appidAsStr);
	}

	public override readonly string ToString() {
		return gameid.ToString();
	}

    public UInt64 gameid;
}
