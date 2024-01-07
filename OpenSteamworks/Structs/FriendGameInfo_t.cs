using System;
using System.Runtime.InteropServices;
using OpenSteamworks;
using OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct FriendGameInfo_t
{
	public CGameID m_gameID;
	public UInt32 m_unGameIP;
	public UInt16 m_usGamePort;
	public UInt16 m_usQueryPort;
	public CSteamID m_steamIDLobby;

    public override string ToString()
    {
        return $"GameID: {m_gameID}, IP: {m_unGameIP}, port: {m_usGamePort}, queryport: {m_usQueryPort}, lobby: {m_steamIDLobby}";
    }
};