using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct FriendRichPresenceUpdate_t
{
	public CSteamID steamid;
	public AppId_t appid;
};