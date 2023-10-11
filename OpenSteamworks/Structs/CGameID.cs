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
		gameid = UInt64.Parse(appidAsStr);
	}

	public AppId_t GetAppId() {
		//TODO: this isn't the right way, but doing this should work as long as the user doesn't have sourcemods or shortcuts (which we don't support yet anyway)
        return (AppId_t)gameid;
    }

	public override readonly string ToString() {
		return gameid.ToString();
	}

    public UInt64 gameid;
}
