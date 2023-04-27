//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code, comments, and all related files, projects, resources,
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo,
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef FRIENDSCOMMON_H
#define FRIENDSCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define CLIENTFRIENDS_INTERFACE_VERSION "CLIENTFRIENDS_INTERFACE_VERSION001"

#define STEAMFRIENDS_INTERFACE_VERSION_001 "SteamFriends001"
#define STEAMFRIENDS_INTERFACE_VERSION_002 "SteamFriends002"
#define STEAMFRIENDS_INTERFACE_VERSION_003 "SteamFriends003"
#define STEAMFRIENDS_INTERFACE_VERSION_004 "SteamFriends004"
#define STEAMFRIENDS_INTERFACE_VERSION_005 "SteamFriends005"
#define STEAMFRIENDS_INTERFACE_VERSION_006 "SteamFriends006"
#define STEAMFRIENDS_INTERFACE_VERSION_007 "SteamFriends007"
#define STEAMFRIENDS_INTERFACE_VERSION_008 "SteamFriends008"
#define STEAMFRIENDS_INTERFACE_VERSION_009 "SteamFriends009"
#define STEAMFRIENDS_INTERFACE_VERSION_010 "SteamFriends010"
#define STEAMFRIENDS_INTERFACE_VERSION_011 "SteamFriends011"
#define STEAMFRIENDS_INTERFACE_VERSION_012 "SteamFriends012"
#define STEAMFRIENDS_INTERFACE_VERSION_013 "SteamFriends013"
#define STEAMFRIENDS_INTERFACE_VERSION_014 "SteamFriends014"
#define STEAMFRIENDS_INTERFACE_VERSION_015 "SteamFriends015"

//-----------------------------------------------------------------------------
// Purpose: avatar sizes, used in ISteamFriends::GetFriendAvatar()
//-----------------------------------------------------------------------------
enum EAvatarSize
{
	k_EAvatarSize32x32 = 0,
	k_EAvatarSize64x64 = 1,
	k_EAvatarSize184x184 = 2,
	k_EAvatarSizeMAX = 3,
};

enum ECallState
{
	k_ECallStateUnknown = 0,
	k_ECallStateWaiting = 1,
	k_ECallStateDialing = 2,
	k_ECallStateRinging = 3,
	k_ECallStateInCall = 4,
};

//-----------------------------------------------------------------------------
// Purpose: Chat Entry Types (previously was only friend-to-friend message types)
//-----------------------------------------------------------------------------
enum EChatEntryType
{
	k_EChatEntryTypeInvalid = 0, 
	k_EChatEntryTypeChatMsg = 1,		// Normal text message from another user
	k_EChatEntryTypeTyping = 2,			// Another user is typing (not used in multi-user chat)
	k_EChatEntryTypeInviteGame = 3,		// Invite from other user into that users current game
	k_EChatEntryTypeEmote = 4,			// text emote message (deprecated, should be treated as ChatMsg)
	//k_EChatEntryTypeLobbyGameStart = 5,	// lobby game is starting (dead - listen for LobbyGameCreated_t callback instead)
	k_EChatEntryTypeLeftConversation = 6, // user has left the conversation ( closed chat window )
	// Above are previous FriendMsgType entries, now merged into more generic chat entry types
	k_EChatEntryTypeEntered = 7,		// user has entered the conversation (used in multi-user chat and group chat)
	k_EChatEntryTypeWasKicked = 8,		// user was kicked (data: 64-bit steamid of actor performing the kick)
	k_EChatEntryTypeWasBanned = 9,		// user was banned (data: 64-bit steamid of actor performing the ban)
	k_EChatEntryTypeDisconnected = 10,	// user disconnected
	k_EChatEntryTypeHistoricalChat = 11,	// a chat message from user's chat history or offilne message
	//k_EChatEntryTypeReserved1 = 12, // No longer used
	//k_EChatEntryTypeReserved2 = 13, // No longer used
	k_EChatEntryTypeLinkBlocked = 14, // a link was removed by the chat filter.
};


//-----------------------------------------------------------------------------
// Purpose: set of relationships to other users
//-----------------------------------------------------------------------------
enum EFriendRelationship
{
	k_EFriendRelationshipNone = 0,
	k_EFriendRelationshipBlocked = 1,
	k_EFriendRelationshipRequestRecipient = 2,
	k_EFriendRelationshipFriend = 3,
	k_EFriendRelationshipRequestInitiator = 4,
	k_EFriendRelationshipIgnored = 5,
	k_EFriendRelationshipIgnoredFriend = 6,
	k_EFriendRelationshipSuggested = 7,

	// keep this updated
	k_EFriendRelationshipMax = 8,
};

enum EChatRoomType
{
	k_EChatRoomTypeFriend = 1,
	k_EChatRoomTypeMUC = 2,
	k_EChatRoomTypeLobby = 3,
};

enum EChatRoomVoiceStatus
{
	eChatRoomVoiceStatus_Invalid = -1,
	eChatRoomVoiceStatus_Bad = 0,
	eChatRoomVoiceStatus_UnknownRoom = 1,
	eChatRoomVoiceStatus_UnknownUser = 2,
	eChatRoomVoiceStatus_NotSpeaking = 3,
	eChatRoomVoiceStatus_Connected_Speaking = 4,
	eChatRoomVoiceStatus_Connected_SpeakingData = 5,
	eChatRoomVoiceStatus_NotConnected_Speaking = 6,
	eChatRoomVoiceStatus_Connecting = 7,
	eChatRoomVoiceStatus_Unreachable = 8,
	eChatRoomVoiceStatus_Disconnected = 9,
	eChatRoomVoiceStatus_Count = 10,
};

enum EClanRank
{
	k_EClanRankNone = 0,
	k_EClanRankOwner = 1,
	k_EClanRankOfficer = 2,
	k_EClanRankMember = 3,
};

enum EClanRelationship
{
	eClanRelationshipNone = 0,
	eClanRelationshipBlocked = 1,
	eClanRelationshipInvited = 2,
	eClanRelationshipMember = 3,
	eClanRelationshipKicked = 4,
};

//-----------------------------------------------------------------------------
// Purpose: flags for enumerating friends list, or quickly checking a the relationship between users
//-----------------------------------------------------------------------------
enum EFriendFlags
{
	k_EFriendFlagNone			= 0x00,
	k_EFriendFlagBlocked		= 0x01,
	k_EFriendFlagFriendshipRequested	= 0x02,
	k_EFriendFlagImmediate		= 0x04,			// "regular" friend
	k_EFriendFlagClanMember		= 0x08,
	k_EFriendFlagOnGameServer	= 0x10,	
	// k_EFriendFlagHasPlayedWith	= 0x20,	// not currently used
	// k_EFriendFlagFriendOfFriend	= 0x40, // not currently used
	k_EFriendFlagRequestingFriendship = 0x80,
	k_EFriendFlagRequestingInfo = 0x100,
	k_EFriendFlagIgnored		= 0x200,
	k_EFriendFlagIgnoredFriend	= 0x400,
	k_EFriendFlagSuggested		= 0x800,
	k_EFriendFlagAll			= 0xFFFF,
};

// for backwards compat
typedef EFriendFlags k_EFriendFlags;


//-----------------------------------------------------------------------------
// Purpose: list of states a friend can be in
//-----------------------------------------------------------------------------
enum EPersonaState
{
	k_EPersonaStateOffline = 0,			// friend is not currently logged on
	k_EPersonaStateOnline = 1,			// friend is logged on
	k_EPersonaStateBusy = 2,			// user is on, but busy
	k_EPersonaStateAway = 3,			// auto-away feature
	k_EPersonaStateSnooze = 4,			// auto-away for a long time
	k_EPersonaStateLookingToTrade = 5,	// Online, trading
	k_EPersonaStateLookingToPlay = 6,	// Online, wanting to play
	k_EPersonaStateMax,
};

// used in PersonaStateChange_t::m_nChangeFlags to describe what's changed about a user
// these flags describe what the client has learned has changed recently, so on startup you'll see a name, avatar & relationship change for every friend
enum EPersonaChange
{
	k_EPersonaChangeName		= 0x001,
	k_EPersonaChangeStatus		= 0x002,
	k_EPersonaChangeComeOnline	= 0x004,
	k_EPersonaChangeGoneOffline	= 0x008,
	k_EPersonaChangeGamePlayed	= 0x010,
	k_EPersonaChangeGameServer	= 0x020,
	k_EPersonaChangeAvatar		= 0x040,
	k_EPersonaChangeJoinedSource= 0x080,
	k_EPersonaChangeLeftSource	= 0x100,
	k_EPersonaChangeRelationshipChanged = 0x200,
	k_EPersonaChangeNameFirstSet = 0x400,
	k_EPersonaChangeFacebookInfo = 0x800,
};

enum EChatPermission
{
	k_EChatPermissionClose = 1,
	k_EChatPermissionInvite = 2,
	k_EChatPermissionTalk = 8,
	k_EChatPermissionKick = 16,
	k_EChatPermissionMute = 32,
	k_EChatPermissionSetMetadata = 64,
	k_EChatPermissionChangePermissions = 128,
	k_EChatPermissionBan = 256,
	k_EChatPermissionChangeAccess = 512,
	k_EChatPermissionEveryoneNotInClanDefault = 8,
	k_EChatPermissionEveryoneDefault = 10,
	k_EChatPermissionMemberDefault = 282,
	k_EChatPermissionOfficerDefault = 282,
	k_EChatPermissionOwnerDefault = 891,
	k_EChatPermissionMask = 1019,
};

//-----------------------------------------------------------------------------
// Purpose: Chat Room Enter Responses
//-----------------------------------------------------------------------------
enum EChatRoomEnterResponse
{
	k_EChatRoomEnterResponseSuccess = 1,		// Success
	k_EChatRoomEnterResponseDoesntExist = 2,	// Chat doesn't exist (probably closed)
	k_EChatRoomEnterResponseNotAllowed = 3,		// General Denied - You don't have the permissions needed to join the chat
	k_EChatRoomEnterResponseFull = 4,			// Chat room has reached its maximum size
	k_EChatRoomEnterResponseError = 5,			// Unexpected Error
	k_EChatRoomEnterResponseBanned = 6,			// You are banned from this chat room and may not join
	k_EChatRoomEnterResponseLimited = 7,		// Joining this chat is not allowed because you are a limited user (no value on account)
	k_EChatRoomEnterResponseClanDisabled = 8,	// Attempt to join a clan chat when the clan is locked or disabled
	k_EChatRoomEnterResponseCommunityBan = 9,	// Attempt to join a chat when the user has a community lock on their account
	k_EChatRoomEnterResponseMemberBlockedYou = 10, // Join failed - some member in the chat has blocked you from joining
	k_EChatRoomEnterResponseYouBlockedMember = 11, // Join failed - you have blocked some member already in the chat
	// k_EChatRoomEnterResponseNoRankingDataLobby = 12,  // No longer used
	// k_EChatRoomEnterResponseNoRankingDataUser = 13,  //  No longer used
	// k_EChatRoomEnterResponseRankOutOfRange = 14, //  No longer used
	k_EChatRoomEnterResponseRatelimitExceeded = 15, // Join failed - to many join attempts in a very short period of time
};

enum EChatAction
{
	k_EChatActionInviteChat = 1,
	k_EChatActionKick = 2,
	k_EChatActionBan = 3,
	k_EChatActionUnBan = 4,
	k_EChatActionStartVoiceSpeak = 5,
	k_EChatActionEndVoiceSpeak = 6,
	k_EChatActionLockChat = 7,
	k_EChatActionUnlockChat = 8,
	k_EChatActionCloseChat = 9,
	k_EChatActionSetJoinable = 10,
	k_EChatActionSetUnjoinable = 11,
	k_EChatActionSetOwner = 12,
	k_EChatActionSetInvisibleToFriends = 13,
	k_EChatActionSetVisibleToFriends = 14,
	k_EChatActionSetModerated = 15,
	k_EChatActionSetUnmoderated = 16,
};

enum EChatActionResult
{
	k_EChatActionResultSuccess = 1,
	k_EChatActionResultError = 2,
	k_EChatActionResultNotPermitted = 3,
	k_EChatActionResultNotAllowedOnClanMember = 4,
	k_EChatActionResultNotAllowedOnBannedUser = 5,
	k_EChatActionResultNotAllowedOnChatOwner = 6,
	k_EChatActionResultNotAllowedOnSelf = 7,
	k_EChatActionResultChatDoesntExist = 8,
	k_EChatActionResultChatFull = 9,
	k_EChatActionResultVoiceSlotsFull = 10,
};

//-----------------------------------------------------------------------------
// Purpose: user restriction flags
//-----------------------------------------------------------------------------
enum EUserRestriction
{
	k_nUserRestrictionNone			= 0,	// no known chat/content restriction
	k_nUserRestrictionUnknown		= 1,	// we don't know yet (user offline)
	k_nUserRestrictionAnyChat		= 2,	// user is not allowed to (or can't) send/recv any chat
	k_nUserRestrictionVoiceChat		= 4,	// user is not allowed to (or can't) send/recv voice chat
	k_nUserRestrictionGroupChat		= 8,	// user is not allowed to (or can't) send/recv group chat
	k_nUserRestrictionRating		= 16,	// user is too young according to rating in current region
	k_nUserRestrictionGameInvites	= 32,	// user cannot send or recv game invites (e.g. mobile)
	k_nUserRestrictionTrading		= 64,	// user cannot participate in trading (console, mobile)
};

enum EOverlayToStoreFlag
{
	k_EOverlayToStoreFlagNone		= 0,
	k_EOverlayToStoreFlagAddToCart	= 1,
};


// size limits on Rich Presence data
enum { k_cchMaxRichPresenceKeys = 20 };
enum { k_cchMaxRichPresenceKeyLength = 64 };
enum { k_cchMaxRichPresenceValueLength = 256 };

// maximum length of friend group name (not including terminating nul!)
const int k_cchMaxFriendGroupName = 64;

// maximum number of groups a single user is allowed
const int k_cFriendGroupLimit = 100;

const int k_nMaxFriends = 250;
const int k_nMaxPendingInvitations = 48;

// maximum length of friend group name (not including terminating nul!)
const int k_cchMaxFriendsGroupName = 64;

// maximum number of groups a single user is allowed
const int k_cFriendsGroupLimit = 100;

const int k_cEnumerateFollowersMax = 50;

// max size on chat messages
const uint32 k_cchFriendChatMsgMax = 0x3000;

// maximum number of characters in a user's name. Two flavors; one for UTF-8 and one for UTF-16.
// The UTF-8 version has to be very generous to accomodate characters that get large when encoded
// in UTF-8.
enum
{
	k_cchPersonaNameMax = 128,
	k_cwchPersonaNameMax = 32,
};

// size limit on chat room or member metadata
const uint32 k_cubChatMetadataMax = 8192;

const int k_cchSystemIMTextMax = 4096;	// upper bound of length of system IM text


#pragma pack( push, 8 )

//-----------------------------------------------------------------------------
// Purpose: information about user sessions
//-----------------------------------------------------------------------------
struct FriendSessionStateInfo_t
{
	uint32 m_uiOnlineSessionInstances;
	uint8 m_uiPublishedToFriendsSessionInstance;
};




// k_iSteamFriendsCallbacks callbacks


//-----------------------------------------------------------------------------
// Purpose: called internally when our persona state changes 
//-----------------------------------------------------------------------------
struct SetPersonaStateInternal_t 
{
	enum { k_iCallback = 1040027 };

	EPersonaState m_eState;
};
//-----------------------------------------------------------------------------
// Purpose: called when a friends' status changes
//-----------------------------------------------------------------------------
struct PersonaStateChange_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 4 };
	
	CSteamID m_ulSteamID;			// steamID of the friend who changed
	EPersonaChange m_nChangeFlags;	// what's changed
};


//-----------------------------------------------------------------------------
// Purpose: posted when game overlay activates or deactivates
//			the game can use this to be pause or resume single player games
//-----------------------------------------------------------------------------
struct GameOverlayActivated_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 31 };

	uint8 m_bActive;	// true if it's just been activated, false otherwise
};


//-----------------------------------------------------------------------------
// Purpose: called when the user tries to join a different game server from their friends list
//			game client should attempt to connect to specified server when this is received
//-----------------------------------------------------------------------------
struct GameServerChangeRequested_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 32 };

	char m_rgchServer[64];		// server address ("127.0.0.1:27015", "tf2.valvesoftware.com")
	char m_rgchPassword[64];	// server password, if any
};


//-----------------------------------------------------------------------------
// Purpose: called when the user tries to join a lobby from their friends list
//			game client should attempt to connect to specified lobby when this is received
//-----------------------------------------------------------------------------
struct GameLobbyJoinRequested_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 33 };
	CSteamID m_steamIDLobby;

	// The friend they did the join via (will be invalid if not directly via a friend)
	//
	// On PS3, the friend will be invalid if this was triggered by a PSN invite via the XMB, but
	// the account type will be console user so you can tell at least that this was from a PSN friend
	// rather than a Steam friend.
	CSteamID m_steamIDFriend;		
};


//-----------------------------------------------------------------------------
// Purpose: called when an avatar is loaded in from a previous GetLargeFriendAvatar() call
//			if the image wasn't already available
//-----------------------------------------------------------------------------
struct AvatarImageLoaded_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 34 };

	CSteamID m_steamID; // steamid the avatar has been loaded for
	int m_iImage; // the image index of the now loaded image
	int m_iWide; // width of the loaded image
	int m_iTall; // height of the loaded image
};


//-----------------------------------------------------------------------------
// Purpose: marks the return of a request officer list call
//-----------------------------------------------------------------------------
struct ClanOfficerListResponse_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 35 };

	CSteamID m_steamIDClan;
	int m_cOfficers;
	uint8 m_bSuccess;
};

//-----------------------------------------------------------------------------
// Purpose: callback indicating updated data about friends rich presence information
//-----------------------------------------------------------------------------
struct FriendRichPresenceUpdate_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 36 };

	CSteamID m_steamIDFriend;	// friend who's rich presence has changed
	AppId_t m_nAppID;			// the appID of the game (should always be the current game)
};

//-----------------------------------------------------------------------------
// Purpose: called when the user tries to join a game from their friends list
//			rich presence will have been set with the "connect" key which is set here
//-----------------------------------------------------------------------------
struct GameRichPresenceJoinRequested_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 37 };

	CSteamID m_steamIDFriend;		// the friend they did the join via (will be invalid if not directly via a friend)
	char m_rgchConnect[k_cchMaxRichPresenceValueLength];
};

//-----------------------------------------------------------------------------
// Purpose: a chat message has been received for a clan chat the game has joined
//-----------------------------------------------------------------------------
struct GameConnectedClanChatMsg_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 38 };

	CSteamID m_steamIDClanChat;
	CSteamID m_steamIDUser;
	int m_iMessageID;
};

//-----------------------------------------------------------------------------
// Purpose: a user has joined a clan chat
//-----------------------------------------------------------------------------
struct GameConnectedChatJoin_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 39 };

	CSteamID m_steamIDClanChat;
	CSteamID m_steamIDUser;
};

//-----------------------------------------------------------------------------
// Purpose: a user has left the chat we're in
//-----------------------------------------------------------------------------
struct GameConnectedChatLeave_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 40 };

	CSteamID m_steamIDClanChat;
	CSteamID m_steamIDUser;
	bool m_bKicked;		// true if admin kicked
	bool m_bDropped;	// true if Steam connection dropped
};

//-----------------------------------------------------------------------------
// Purpose: a DownloadClanActivityCounts() call has finished
//-----------------------------------------------------------------------------
struct DownloadClanActivityCountsResult_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 41 };

	bool m_bSuccess;
};

//-----------------------------------------------------------------------------
// Purpose: a JoinClanChatRoom() call has finished
//-----------------------------------------------------------------------------
struct JoinClanChatRoomCompletionResult_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 42 };

	CSteamID m_steamIDClanChat;
	EChatRoomEnterResponse m_eChatRoomEnterResponse;
};

//-----------------------------------------------------------------------------
// Purpose: a chat message has been received from a user
//-----------------------------------------------------------------------------
struct GameConnectedFriendChatMsg_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 43 };

	CSteamID m_steamIDUser;
	int m_iMessageID;
};

struct FriendsGetFollowerCount_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 44 };

	EResult m_eResult;
	CSteamID m_steamID;
	int32 m_cCount;
};

struct FriendsIsFollowing_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 45 };

	EResult m_eResult;
	CSteamID m_steamID;
	bool m_bIsFollowing;
};

struct FriendsEnumerateFollowingList_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 46 };

	EResult m_eResult;
	CSteamID m_steamIDs[ k_cEnumerateFollowersMax ];
	int32 m_cSteamIDs;
	int32 m_cTotalResults;
};

//-----------------------------------------------------------------------------
// Purpose: a SetPersonaName / SetPersonaNameEx call has finished
//-----------------------------------------------------------------------------
struct SetPersonaNameResponse_t
{
	enum { k_iCallback = k_iSteamFriendsCallbacks + 47 };

	bool m_bUnk1;
	bool m_bUnk2;
	
	EResult m_eResult;
};


// k_iClientFriendsCallbacks callbacks



struct GameOverlayActivateRequested_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 1 };

	char m_rgchDialog[256];
	CSteamID m_steamIDTarget;
	AppId_t m_nAppID;
	bool m_bWebPage;
	EOverlayToStoreFlag m_eFlag;
};

struct ClanEventReceived_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 2 };

	uint64 m_gidEvent;
	uint64 m_ulSteamIDClan;
	char m_rgchTitle[256];
	uint32 m_nStartTime;
	CGameID m_GameID;
};

//-----------------------------------------------------------------------------
// Purpose: called after a friend has been successfully added
//-----------------------------------------------------------------------------
struct FriendAdded_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 3 };

	EResult m_eResult;
	uint64 m_ulSteamID;	// steamID of the friend who was just added
};


//-----------------------------------------------------------------------------
// Purpose: called when a user is requesting friendship
//			the persona details of this user are guaranteed to be available locally
//			at the point this callback occurs
//-----------------------------------------------------------------------------
struct UserRequestingFriendship_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 4 };

	CSteamID m_ulSteamID;		// steamID of the friend who just added us
};


//-----------------------------------------------------------------------------
// Purpose: called when this client has received a chat/invite/etc. message from a friend
//-----------------------------------------------------------------------------
struct FriendChatMsg_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 5 };
	
	CSteamID m_ulFriendID;			// other participant in the msg
	CSteamID m_ulSenderID;			// steamID of the friend who has sent this message
	uint8 m_eChatEntryType;
	uint8 m_bLimitedAccount;
	uint32 m_iChatID;				// chat id
};

struct FriendInvited_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 6 };

	EResult m_eResult;
};

struct ChatRoomInvite_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 7 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDPatron;
	CSteamID m_ulSteamIDFriendChat;

	EChatRoomType m_EChatRoomType;

	char m_rgchChatRoomName[ 128 ];
};

struct ChatRoomEnter_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 8 };

	CSteamID m_ulSteamIDChat;

	EChatRoomType m_EChatRoomType;

	uint64 m_ulSteamIDOwner; // Cannot use CSteamID here due to padding issues.
	CSteamID m_ulSteamIDClan;
	CSteamID m_ulSteamIDFriendChat;

	bool m_bLocked;
	uint32 m_rgfChatPermissions;
	EChatRoomEnterResponse m_EChatRoomEnterResponse;

	char m_rgchChatRoomName[ 128 ];
};

struct ChatMemberStateChange_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 9 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDUserChanged;
	EChatMemberStateChange m_rgfChatMemberStateChange;
	uint64 m_ulSteamIDMakingChange; // Cannot use CSteamID here due to padding issues.
};

struct ChatRoomMsg_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 10 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDUser;
	uint8 m_eChatEntryType;
	uint32 m_iChatID;
};

struct ChatRoomDlgClose_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 11 };

	CSteamID m_ulSteamIDChat;
};

struct ChatRoomClosing_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 12 };

	CSteamID m_ulSteamIDChat;
};

struct ChatRoomKicking_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 13 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDAdmin;
};

struct ChatRoomBanning_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 14 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDAdmin;
};

struct ChatRoomCreate_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 15 };

	EResult m_eResult;

	uint64 m_ulSteamIDChat; // Cannot use CSteamID here due to padding issues.
	CSteamID m_ulSteamIDFriendChat;
};

struct OpenChatDialog_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 16 };

	CSteamID m_ulSteamID;
};

struct ChatRoomActionResult_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 17 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDUserActedOn;
	EChatAction m_EChatAction;
	EChatActionResult m_EChatActionResult;
};

struct ChatRoomDlgSerialized_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 18 };

	CSteamID m_ulSteamID;
};

struct ClanInfoChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 19 };

	CSteamID m_ulSteamID;

	bool m_bNameChanged;
	bool m_bAvatarChanged;
	bool m_bAccountInfoChanged;
};

struct ChatMemberInfoChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 20 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDUser;
	uint32  m_rgfChatMemberPermissions;
};

struct ChatRoomInfoChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 21 };

	CSteamID m_ulSteamIDChat;
	uint32 m_rgfChatRoomDetails;
	uint64 m_ulSteamIDMakingChange; // Cannot use CSteamID here due to padding issues.
};

struct OBSOLETE_CALLBACK SteamRackBouncing_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 22 };

	// TODO : Reverse this callback
};

struct ChatRoomSpeakChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 23 };

	CSteamID m_ulSteamIDChat;
	CSteamID m_ulSteamIDUser;
	bool m_bSpeaking;
};

struct NotifyIncomingCall_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 24 };

	HVoiceCall m_Handle;

	uint64 m_ulSteamID; // Cannot use CSteamID here due to padding issues.
	CSteamID m_ulSteamIDChat;

	bool m_bIncoming;
};

struct NotifyHangup_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 25 };

	HVoiceCall m_Handle;
	EVoiceResult m_eVoiceResult;
};

struct NotifyRequestResume_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 26 };

	HVoiceCall m_Handle;
};

struct NotifyChatRoomVoiceStateChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 27 };

	CSteamID m_steamChatRoom;
	CSteamID m_steamUser;
};

struct ChatRoomDlgUIChange_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 28 };

	CSteamID m_SteamIDChat;
	bool m_bShowAvatars;
	bool m_bBeepOnNewMsg;
	bool m_bShowSteamIDs;
	bool m_bShowTimestampOnNewMsg;
};

struct VoiceCallInitiated_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 29 };

	uint64 m_ulSteamIDUser;
	uint64 m_ulSteamIDFriend;
	HVoiceCall m_hVoiceCall;
};

struct FriendIgnored_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 30 };

	EResult m_eResult;
	uint64 m_ulSteamID; // Cannot use CSteamID here due to padding issues.
	CSteamID m_ulSteamIDFriend;
	bool m_bIgnored;
};

struct VoiceInputDeviceChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 31 };
};

struct VoiceEnabledStateChanged_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 32 };

	bool m_bVoiceEnabled;
};

struct FriendsWhoPlayGameUpdate_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 33 };

	CGameID m_gameID;
};

struct FriendProfileInfoResponse_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 34 };

	CSteamID m_steamIDFriend;
	EResult m_eResult;
};

struct RichInviteReceived_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 35 };

	CSteamID m_steamIDFriend;
	AppId_t m_nAppID;
	char m_rgchConnect[256];
};

struct FriendsMenuChange_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 36 };

	bool m_bShowAvatars;
	bool m_bSortByName;
	bool m_bShowOnlineFriendsOnly;
	bool m_bShowUntaggedFriends;
};

struct TradeInviteReceived_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 37 };

	CSteamID m_steamIDPartner;
	uint32 m_unTradeRequestID;
};

struct TradeInviteResponse_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 38 };

	CSteamID m_steamIDPartner;
	uint32 m_eResponse;
};

struct TradeStartSession_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 39 };

	CSteamID m_steamIDPartner;
};

struct TradeInviteCanceled_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 40 };

	// TODO : Reverse this callback
};

struct GameUsingVoice_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 41 };

	// TODO : Reverse this callback
};

struct FriendsGroupCreated_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 42 };

	// TODO : Reverse this callback
};

struct FriendsGroupDeleted_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 43 };

	// TODO : Reverse this callback
};

struct FriendsGroupRenamed_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 44 };

	// TODO : Reverse this callback
};

struct FriendsGroupMemberAdded_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 45 };

	// TODO : Reverse this callback
};

struct FriendsGroupMemberRemoved_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 46 };

	// TODO : Reverse this callback
};

//-----------------------------------------------------------------------------
// Purpose: called when the user requests the history of player names on a given account
//-----------------------------------------------------------------------------
struct NameHistoryResponse_t
{
	enum { k_iCallback = k_iClientFriendsCallbacks + 47 };
	int m_cSuccessfulLookups;		// number of lookups that were successful
	int m_cFailedLookups;			// number of lookups that failed for one reason or another
};

#pragma pack( pop )



#endif // FRIENDSCOMMON_H
