using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CGameID {
    public CGameID( AppId_t nAppID )
	{
		gameid = nAppID;
	}

    public UInt64 gameid;
}
