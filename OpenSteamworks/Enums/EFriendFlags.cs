using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum EFriendFlags
{
	None			= 0x00,
	Blocked		= 0x01,
	FriendshipRequested	= 0x02,
    /// <summary>
    /// "regular" friend
    /// </summary>
	Immediate		= 0x04,	
	ClanMember		= 0x08,
	OnGameServer	= 0x10,	
	HasPlayedWith	= 0x20,	
	FriendOfFriend	= 0x40,
	RequestingFriendship = 0x80,
	RequestingInfo = 0x100,
	Ignored		= 0x200,
	IgnoredFriend	= 0x400,
    /// <summary>
    /// old facebook linking system
    /// </summary>
	Suggested		= 0x800,
	ChatMember		= 0x1000,
	All			= 0xFFFF,
}