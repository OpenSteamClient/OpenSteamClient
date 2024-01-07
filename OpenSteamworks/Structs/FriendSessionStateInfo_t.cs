using System;
using System.Runtime.InteropServices;
using OpenSteamworks;
using OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct FriendSessionStateInfo_t
{
	public UInt32 m_uiOnlineSessionInstances;
	public UInt8 m_uiPublishedToFriendsSessionInstance;

    public override string ToString()
    {
        return $"m_uiOnlineSessionInstances: {m_uiOnlineSessionInstances}, m_uiPublishedToFriendsSessionInstance: {m_uiPublishedToFriendsSessionInstance}";
    }
};