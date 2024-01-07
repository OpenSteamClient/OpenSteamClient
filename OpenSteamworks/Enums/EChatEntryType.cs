using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

public enum EChatEntryType
{
	Invalid = 0, 
	ChatMsg = 1,		// Normal text message from another user
	Typing = 2,			// Another user is typing (not used in multi-user chat)
	InviteGame = 3,		// Invite from other user into that users current game
	Emote = 4,			// text emote message (deprecated, should be treated as ChatMsg)
	//LobbyGameStart = 5,	// lobby game is starting (dead - listen for LobbyGameCreated_t callback instead)
	LeftConversation = 6, // user has left the conversation ( closed chat window )
	// Above are previous FriendMsgType entries, now merged into more generic chat entry types
	Entered = 7,		// user has entered the conversation (used in multi-user chat and group chat)
	WasKicked = 8,		// user was kicked (data: 64-bit steamid of actor performing the kick)
	WasBanned = 9,		// user was banned (data: 64-bit steamid of actor performing the ban)
	Disconnected = 10,	// user disconnected
	HistoricalChat = 11,	// a chat message from user's chat history or offilne message
	//Reserved1 = 12, // No longer used
	//Reserved2 = 13, // No longer used
	LinkBlocked = 14, // a link was removed by the chat filter.
};
