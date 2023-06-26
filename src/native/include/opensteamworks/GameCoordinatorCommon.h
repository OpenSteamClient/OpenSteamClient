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

#ifndef ISTEAMGAMECOORDINATORCOMMON_H
#define ISTEAMGAMECOORDINATORCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define CLIENTGAMECOORDINATOR_INTERFACE_VERSION "CLIENTGAMECOORDINATOR_INTERFACE_VERSION001"

#define STEAMGAMECOORDINATOR_INTERFACE_VERSION_001 "SteamGameCoordinator001"


// list of possible return values from the ISteamGameCoordinator API
enum EGCResults
{
	k_EGCResultOK = 0,
	k_EGCResultNoMessage = 1,			// There is no message in the queue
	k_EGCResultBufferTooSmall = 2,		// The buffer is too small for the requested message
	k_EGCResultNotLoggedOn = 3,			// The client is not logged onto Steam
	k_EGCResultInvalidMessage = 4,		// Something was wrong with the message being sent with SendMessage
};

/**
 * Valve moved a lot of messages to the protobuf format.
 * This means that the structs below should no longer be trusted to be correct.
 * A protobuf message can be detected with:
 *  (uMsgType & 0x80000000) == 0x80000000
 */

typedef enum EGCMessages
{
	k_EGCMsgGenericReply = 10,

	k_ESOMsg_Create = 21,
	k_ESOMsg_Update,
	k_ESOMsg_Destroy,
	k_ESOMsg_CacheSubscribed,
	k_ESOMsg_CacheUnsubscribed,
	k_ESOMsg_UpdateMultiple,
	k_ESOMsg_CacheSubscriptionCheck,
	k_ESOMsg_CacheSubscriptionRefresh,

	k_EGCMsgAchievementAwarded = 51,
	k_EGCMsgConCommand,
	k_EGCMsgStartPlaying,
	k_EGCMsgStopPlaying,
	k_EGCMsgStartGameserver,
	k_EGCMsgStopGameserver,
	k_EGCMsgWGRequest,
	k_EGCMsgWGResponse,
	k_EGCMsgGetUserGameStatsSchema,
	k_EGCMsgGetUserGameStatsSchemaResponse,
	k_EGCMsgGetUserStatsDEPRECATED,
	k_EGCMsgGetUserStatsResponse,
	k_EGCMsgAppInfoUpdated,
	k_EGCMsgValidateSession,
	k_EGCMsgValidateSessionResponse,
	k_EGCMsgLookupAccountFromInput,
	k_EGCMsgSendHTTPRequest,
	k_EGCMsgSendHTTPRequestResponse,
	k_EGCMsgPreTestSetup,
	k_EGCMsgRecordSupportAction,
	k_EGCMsgGetAccountDetails,
	k_EGCMsgSendInterAppMessage,
	k_EGCMsgReceiveInterAppMessage,
	k_EGCMsgFindAccounts,
	k_EGCMsgPostAlert,
	k_EGCMsgGetLicenses,
	k_EGCMsgGetUserStats,
	k_EGCMsgGetCommands,
	k_EGCMsgGetCommandsResponse,
	k_EGCMsgAddFreeLicense,
	k_EGCMsgAddFreeLicenseResponse,

	k_EGCMsgWebAPIRegisterInterfaces = 101,
	k_EGCMsgWebAPIJobRequest,
	k_EGCMsgWebAPIRegistrationRequested,

	k_EGCMsgMemCachedGet = 200,
	k_EGCMsgMemCachedGetResponse,
	k_EGCMsgMemCachedSet,
	k_EGCMsgMemCachedDelete,

	k_EMsgGCSetItemPosition = 1001,
	k_EMsgGCCraft,
	k_EMsgGCCraftResponse,
	k_EMsgGCDelete,
	k_EMsgGCVerifyCacheSubscription,
	k_EMsgGCNameItem,
	k_EMsgGCDecodeItem,
	k_EMsgGCDecodeItemResponse,
	k_EMsgGCPaintItem,
	k_EMsgGCPaintItemResponse,
	k_EMsgGCGoldenWrenchBroadcast,
	k_EMsgGCMOTDRequest,
	k_EMsgGCMOTDRequestResponse,
	k_EMsgGCAddItemToSocket,
	k_EMsgGCAddItemToSocketResponse,
	k_EMsgGCAddSocketToBaseItem,
	k_EMsgGCAddSocketToItem,
	k_EMsgGCAddSocketToItemResponse,
	k_EMsgGCNameBaseItem,
	k_EMsgGCNameBaseItemResponse,
	k_EMsgGCRemoveSocketItem,
	k_EMsgGCRemoveSocketItemResponse,
	k_EMsgGCCustomizeItemTexture,
	k_EMsgGCCustomizeItemTextureResponse,
	k_EMsgGCUseItemRequest,
	k_EMsgGCUseItemResponse,
	k_EMsgGCGiftedItems,
	k_EMsgGCSpawnItem,
	k_EMsgGCRespawnPostLoadoutChange,
	k_EMsgGCRemoveItemName,
	k_EMsgGCRemoveItemPaint,
	k_EMsgGCGiftWrapItem,
	k_EMsgGCGiftWrapItemResponse,
	k_EMsgGCDeliverGift,
	k_EMsgGCDeliverGiftResponseGiver,
	k_EMsgGCDeliverGiftResponseReceiver,
	k_EMsgGCUnwrapGiftRequest,
	k_EMsgGCUnwrapGiftResponse,
	k_EMsgGCSetItemStyle,
	k_EMsgGCUsedClaimCodeItem,
	k_EMsgGCSortItems,
	k_EMsgGC_RevolvingLootList,
	k_EMsgGCLookupAccount,
	k_EMsgGCLookupAccountResponse,
	k_EMsgGCLookupAccountName,
	k_EMsgGCLookupAccountNameResponse,
	k_EMsgGCUpdateItemSchema = 1049,
	k_EMsgGCRequestInventoryRefresh,
	k_EMsgGCRemoveCustomTexture,
	k_EMsgGCRemoveCustomTextureResponse,
	k_EMsgGCRemoveMakersMark,
	k_EMsgGCRemoveMakersMarkResponse,
	k_EMsgGCRemoveUniqueCraftIndex,
	k_EMsgGCRemoveUniqueCraftIndexResponse,
	k_EMsgGCSaxxyBroadcast,
	k_EMsgGCBackpackSortFinished,
	k_EMsgGCRequestItemSchemaData = 1060,

	k_EMsgGCTrading_InitiateTradeRequest = 1501,
	k_EMsgGCTrading_InitiateTradeResponse,
	k_EMsgGCTrading_StartSession,
	k_EMsgGCTrading_SetItem,
	k_EMsgGCTrading_RemoveItem,
	k_EMsgGCTrading_UpdateTradeInfo,
	k_EMsgGCTrading_SetReadiness,
	k_EMsgGCTrading_ReadinessResponse,
	k_EMsgGCTrading_SessionClosed,
	k_EMsgGCTrading_CancelSession,
	k_EMsgGCTrading_TradeChatMsg,
	k_EMsgGCTrading_ConfirmOffer,
	k_EMsgGCTrading_TradeTypingChatMsg,

	k_EMsgGCServerBrowser_FavoriteServer = 1601,
	k_EMsgGCServerBrowser_BlacklistServer,

	k_EMsgGCDev_NewItemRequest = 2001,
	k_EMsgGCDev_NewItemRequestResponse,

	k_EMsgGCStoreGetUserData = 2500,
	k_EMsgGCStoreGetUserDataResponse,
	k_EMsgGCStorePurchaseInit,
	k_EMsgGCStorePurchaseInitResponse,
	k_EMsgGCStorePurchaseFinalize,
	k_EMsgGCStorePurchaseFinalizeResponse,
	k_EMsgGCStorePurchaseCancel,
	k_EMsgGCStorePurchaseCancelResponse,
	k_EMsgGCStorePurchaseQueryTxn,
	k_EMsgGCStorePurchaseQueryTxnResponse,

	//k_EMsgGCSystemMessage = 4001,
	k_EMsgGCReplicateConVars,
	k_EMsgGCConVarUpdated,

	k_EMsgGCReportWarKill = 5001,
	k_EMsgGCVoteKickBanPlayer = 5018,
	k_EMsgGCVoteKickBanPlayerResult,
	k_EMsgGCKickPlayer,
	k_EMsgGCStartedTraining,
	k_EMsgGCFreeTrial_ChooseMostHelpfulFriend,
	k_EMsgGCRequestTF2Friends,
	k_EMsgGCRequestTF2FriendsResponse,
	k_EMsgGCReplay_UploadedToYouTube,
	k_EMsgGCReplay_SubmitContestEntry,
	k_EMsgGCReplay_SubmitContestEntryResponse,

	k_EMsgGCCoaching_AddToCoaches = 5200,
	k_EMsgGCCoaching_AddToCoachesResponse,
	k_EMsgGCCoaching_RemoveFromCoaches,
	k_EMsgGCCoaching_RemoveFromCoachesResponse,
	k_EMsgGCCoaching_FindCoach,
	k_EMsgGCCoaching_FindCoachResponse,
	k_EMsgGCCoaching_AskCoach,
	k_EMsgGCCoaching_AskCoachResponse,
	k_EMsgGCCoaching_CoachJoinGame,
	k_EMsgGCCoaching_CoachJoining,
	k_EMsgGCCoaching_CoachJoined,
	k_EMsgGCCoaching_LikeCurrentCoach,
	k_EMsgGCCoaching_RemoveCurrentCoach,
	k_EMsgGCCoaching_AlreadyRatedCoach,

	k_EMsgGC_Duel_Request = 5500,
	k_EMsgGC_Duel_Response,
	k_EMsgGC_Duel_Results,
	k_EMsgGC_Duel_Status,

	k_EMsgGC_Halloween_ReservedItem = 5600,
	k_EMsgGC_Halloween_GrantItem,
	k_EMsgGC_Halloween_GrantItemResponse = 5604,
	k_EMsgGC_Halloween_Cheat_QueryResponse,
	k_EMsgGC_Halloween_ItemClaimed,

	k_EMsgGC_GameServer_LevelInfo = 5700,
	k_EMsgGC_GameServer_AuthChallenge,
	k_EMsgGC_GameServer_AuthChallengeResponse,
	k_EMsgGC_GameServer_CreateIdentity,
	k_EMsgGC_GameServer_CreateIdentityResponse,
	k_EMsgGC_GameServer_List,
	k_EMsgGC_GameServer_ListResponse,
	k_EMsgGC_GameServer_AuthResult,
	k_EMsgGC_GameServer_ResetIdentity,
	k_EMsgGC_GameServer_ResetIdentityResponse,

	k_EMsgGC_QP_ScoreServers = 5800,
	k_EMsgGC_QP_ScoreServersResponse,

	k_EMsgGC_PickupItemEligibility_Query = 6000,
	k_EMsgGCDev_GrantWarKill,

	k_EMsgGC_IncrementKillCountAttribute = 6100,
	k_EMsgGC_IncrementKillCountResponse
	
} EGCMessages;

typedef enum ETFInitTradeResult
{
	k_ETFInitTradeResultOk,
	k_ETFInitTradeResultDeclined, // The other player has declined the trade request.
	k_ETFInitTradeResultVACBanned, // You do not have trading privileges.
	k_ETFInitTradeResultOtherVACBanned, // The other player does not have trading privileges at this time.
	k_ETFInitTradeResultBusy, // The other player is currently busy trading with someone else.
	k_ETFInitTradeResultDisabled, // Trading items is currently disabled.
	k_ETFInitTradeResultNoLoggedIn, // The other player is not available for trading.
	k_ETFInitTradeResultCanceled,
	k_ETFInitTradeResultTooSoon // You must wait at least 30 seconds between trade requests.
	
} ETFInitTradeResult;

typedef enum ETFTradeResult
{
	k_ETFTradeResultOk,
	k_ETFTradeResultCanceled, // The trading session has been canceled.
	k_ETFTradeResultStaleInventory, // The trade was cancelled, because some items do not belong to you or the other player.
	k_ETFTradeResultUntradeable, // The trade was cancelled, because some items are not allowed in trading.
	k_ETFTradeResultNoItems, // The trade was cancelled, because there were no items to trade.
	k_ETFTradeResultDisabled // Trading items is currently disabled.
	
} ETFTradeResult;

#pragma pack( push, 8 )
// callback notification - A new message is available for reading from the message queue
struct GCMessageAvailable_t
{
	enum { k_iCallback = k_iSteamGameCoordinatorCallbacks + 1 };

	uint32 m_nMessageSize;
};

// callback notification - A message failed to make it to the GC. It may be down temporarily
struct GCMessageFailed_t
{
	enum { k_iCallback = k_iSteamGameCoordinatorCallbacks + 2 };
};
#pragma pack( pop )

#pragma pack(push, 1)

struct GCMsgHeader_t
{
	uint16 headerVersion;
	uint64 targetJobID;
	uint64 sourceJobID;
};

struct SOMsgCacheSubscribed_t
{
	enum { k_iMessage = k_ESOMsg_CacheSubscribed };
	GCMsgHeader_t header;
	
	CSteamID steamid;
	uint32 numberOfTypes; // Number of different 'sets' of information included, starts with items, goes on to recipes etc.
	// [SOMsgCacheSubscribed_*s_t] * numberOfTypes; SOMsgCacheSubscribed_Items_t is first, and the only one currently documented.
};

struct SOMsgCacheSubscribed_Items_t
{
	uint16 idOfType; // Check this is 1
	uint16 itemcount;
	// Variable length data:
	// [SOMsgCacheSubscribed_Item_t] * itemcount
};

struct SOMsgCacheSubscribed_Item_t
{
	uint64 itemid;
	uint32 accountid;
	uint16 itemdefindex;
	uint8 itemlevel;
	uint8 itemquality;
	uint32 position;
	uint32 quantity;
	uint16 namelength;
	// Variable length data:
	// char customname[namelength];
	// uint8 flags;
	// uint8 origin;
	// uint16 descriptionlength;
	// char customdescription[descriptionlength];
	// uint8 unk;
	// uint16 attribcount;
	// [SOMsgCacheSubscribed_Item_Attrib_t] * attribcount
	// uint64 containedItem; // If this is set, there is a whole other item after.
};

struct SOMsgCacheSubscribed_Item_Attrib_t
{
	uint16 attribindex;
	float value;
};

struct SOMsgCacheUnsubscribed_t
{
	enum { k_iMessage = k_ESOMsg_CacheUnsubscribed };
	GCMsgHeader_t header;
	
	CSteamID steamid;
};

struct SOMsgCreate_t
{
	enum { k_iMessage = k_ESOMsg_Create };
	GCMsgHeader_t header;
	
	CSteamID steamid;
	uint32 unknown;
	SOMsgCacheSubscribed_Item_t item;
};

/*
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 76f0da0200000000 0105 0f000080
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 21ccd90200000000 0105 10000080
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 d069ea0200000000 0105 20000080
*/
struct SOMsgUpdate_t
{
	enum { k_iMessage = k_ESOMsg_Update };
	GCMsgHeader_t header;
	
	CSteamID steamid;
	uint32 unk1;
	uint64 itemID;
	uint16 unk2;
	uint32 position;
};

/*
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 7f7e1b0200000000
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 5a77020200000000
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 bdbc1c0200000000
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 8885210200000000
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001 01000000 e582e30100000000
*/
struct SOMsgDeleted_t
{
	enum { k_iMessage = k_ESOMsg_Destroy };
	GCMsgHeader_t header;
	
	CSteamID steamid;
	uint32 unk1;
	uint64 itemid;
};

/*
0100 ffffffffffffffffffffffffffffffff 76f0da0200000000 0f000080 00000000
0100 ffffffffffffffffffffffffffffffff 21ccd90200000000 10000080 00000000
0100 ffffffffffffffffffffffffffffffff cff9ea0200000000 42000080 00000000
0100 ffffffffffffffffffffffffffffffff d069ea0200000000 20000080 00000000
*/
struct GCSetItemPosition_t
{
	enum { k_iMessage = k_EMsgGCSetItemPosition };
	GCMsgHeader_t header;
	
	uint64 itemID;
	uint32 position;
	uint32 unk1;
};


/*
This one is 4 natasha
0100 ffffffffffffffffffffffffffffffff 0700 0400 5a77020200000000 bdbc1c0200000000 8885210200000000 e582e30100000000
*/
struct GCCraft_t
{
	enum { k_iMessage = k_EMsgGCCraft };
	GCMsgHeader_t header;
	
	uint16 blueprint;//0xffff = unknown blueprint
	uint16 itemcount;
	// Variable length data:
	// [uint64 itemid] * itemcount
};


/*
0100 ffffffffffffffffffffffffffffffff 0700 0000000000000100 d069ea0200000000
*/
struct GCCraftResponse_t
{
	enum { k_iMessage = k_EMsgGCCraftResponse };
	GCMsgHeader_t header;
	
	uint16 blueprint;//0xffff = failed
	uint64 unk1;
	uint64 itemid;
};


/*
0100 ffffffffffffffffffffffffffffffff 7f7e1b0200000000
*/
struct GCDelete_t
{
	enum { k_iMessage = k_EMsgGCDelete };
	GCMsgHeader_t header;
	
	uint64 itemID;
};


/*
0100 ffffffffffffffffffffffffffffffff 86cf4e0001001001
*/
struct GCVerifyCacheSubscription_t
{
	enum { k_iMessage = k_EMsgGCVerifyCacheSubscription };
	GCMsgHeader_t header;
	
	CSteamID steamid;
};


/*
0100 ffffffffffffffffffffffffffffffff 28000000 4d61783637202846522900
0100 ffffffffffffffffffffffffffffffff 29000000 54726962697400
0100 ffffffffffffffffffffffffffffffff 2a000000 776973686d617374657200
0100 ffffffffffffffffffffffffffffffff 2b000000 416d616e6f6f00
0100 ffffffffffffffffffffffffffffffff 2c000000 7c4b47437c2047617920526f62696e00
0100 ffffffffffffffffffffffffffffffff 2d000000 416e6164757200
0100 ffffffffffffffffffffffffffffffff 2e000000 54686520436f726e62616c6c657200
0100 ffffffffffffffffffffffffffffffff 2f000000 69736c6100
*/
struct GCGoldenWrenchBroadcast_t
{
	enum { k_iMessage = k_EMsgGCGoldenWrenchBroadcast };
	GCMsgHeader_t header;
	
	uint16 WrenchNumber;
	uint16 State; // 0 = Found, 1 = Destroyed
	// Variable length data:
	// char OwnerName[];
};

/*
0100 ffffffffffffffffffffffffffffffff 00000000 02000000 
0100 ffffffffffffffffffffffffffffffff 329d2d4c 02000000 
0100 ffffffffffffffffffffffffffffffff e6c74e4c 02000000 
*/
struct GCMOTDRequest_t
{
	enum { k_iMessage = k_EMsgGCMOTDRequest };
	GCMsgHeader_t header;
	
	uint32 timestamp;
	uint32 unk1;
};

/*
0100 ffffffffffffffffffffffffffffffff 0000
0100 ffffffffffffffffffffffffffffffff 0200 3100 30930e4c 436865636b6564206f75742074686520626c6f673f00 496620796f7520686176656e2774207265616420746865206f6666696369616c2054463220626c6f672c20697427732066756c6c206f6620696e73696768747320696e746f206f757220646576656c6f706d656e742070726f636573732c206c696e6b7320746f206e6f7461626c6520636f6d6d756e6974792070726f64756374696f6e732c20616e642072616e646f6d2073746f726965732061626f7574206f7572206c6f7665206f6620686174732e204869742074686520627574746f6e2062656c6f7720746f2074616b652061206c6f6f6b2100 687474703a5c5c7777772e7465616d666f7274726573732e636f6d5c00 3200 b0e52c4c 4f6666696369616c2057696b69206f70656e732100 576527766520726563656e746c79206f70656e65642074686520646f6f7273206f6e20746865204f6666696369616c205446322077696b692e20546865726520796f752063616e2066696e64206f75742065766572797468696e67205446322072656c617465642c2066726f6d20746865206e756d65726963616c206e75747320616e6420626f6c7473206f6620657665727920776561706f6e20746f2074686520656173746572206567677320696e7369646520746865204d65657420746865205465616d206d6f766965732e205468657927726520616c77617973206c6f6f6b696e6720666f72206d6f726520636f6e7472696275746f72732c20736f20776879206e6f742068656164206f76657220616e642068656c70207468656d3f00 687474703a5c5c77696b692e7465616d666f7274726573732e636f6d5c00
*/
struct GCMOTDRequestResponse_t
{
	enum { k_iMessage = k_EMsgGCMOTDRequestResponse };
	GCMsgHeader_t header;
	
	uint16 NumberOfNews;
	// Variable length data:
	// [GCMOTDRequestResponse_News_t] * NumberOfNews
};

struct GCMOTDRequestResponse_News_t
{
	// Variable length data:
	// char id[];
	// uint32 timestamp;
	// char Title[];
	// char Content[];
	// char URL[];
};

/*
These two messages were recently added to TF2 along with two convars (tf_server_identity_token and tf_server_identity_account_id)
Every TF2 server is sent a GC_GameServer_AuthChallenge message on start up, by default the two convars are blank and the server does not respond to the challenge.
If however the convars are set, it responds in the following manner:
accountID is set to the value of the tf_server_identity_account_id convar.
hash is set to the result of the md5 hash of the value of the tf_server_identity_token convar prepended to the salt recieved in the challenge.
For example, if tf_server_identity_token was set to "Derp" and 4203408982 was the salt from the challenge, hash would be the md5 hash of "Derp4203408982"
*/
struct GC_GameServer_AuthChallenge_t
{
	enum { k_iMessage = k_EMsgGC_GameServer_AuthChallenge };
	GCMsgHeader_t header;
	
	uint8 unknown; // Possibly the terminator for an empty string.
	// Variable length data:
	// char salt[];
};

struct GC_GameServer_AuthChallengeResponse_t
{
	enum { k_iMessage = k_EMsgGC_GameServer_AuthChallengeResponse };
	GCMsgHeader_t header;
	
	uint32 accountID;
	// Variable length data:
	// char hash[];
};


struct GC_GameServer_LevelInfo_t
{
	enum { k_iMessage = k_EMsgGC_GameServer_LevelInfo };
	GCMsgHeader_t header;
	
	uint8 unknown;
	// Variable length data:
	// char mapName[];
};

struct GCTrading_InitiateTradeRequest_t
{
	enum { k_iMessage = k_EMsgGCTrading_InitiateTradeRequest };
	GCMsgHeader_t header;
	
	uint32 challenge;
	CSteamID steamID;
	// Variable length data:
	// char playerName[]; // Only present on incoming requests.
};

struct GCTrading_InitiateTradeResponse_t
{
	enum { k_iMessage = k_EMsgGCTrading_InitiateTradeResponse };
	GCMsgHeader_t header;
	
	/*ETFInitTradeResult*/ uint32 result;
	uint32 challenge; // When sending this message as a response, make sure to set this as the same value from the request.
};

struct GCTrading_TradeChatMsg_t
{
	enum { k_iMessage = k_EMsgGCTrading_TradeChatMsg };
	GCMsgHeader_t header;
	
	uint8 unknown; // possibly a 0-length string
	// Variable length data:
	// char chatMsg[];
};

struct GCTrading_TradeTypingChatMsg_t
{
	enum { k_iMessage = k_EMsgGCTrading_TradeTypingChatMsg };
	GCMsgHeader_t header;
};

struct GCTrading_StartSession_t
{
	enum { k_iMessage = k_EMsgGCTrading_StartSession };
	GCMsgHeader_t header;
	
	CSteamID steamID1;
	CSteamID steamID2;
	// Variable length data:
	// char player1Name[];
	// char player2Name[];
};

struct GCTrading_SetItem_t
{
	enum { k_iMessage = k_EMsgGCTrading_SetItem };
	GCMsgHeader_t header;
	
	uint8 showcase;
	uint64 itemID;
	uint8 slot; // Trade 'slot' it goes in, see below.
};

struct GCTrading_RemoveItem_t
{
	enum { k_iMessage = k_EMsgGCTrading_RemoveItem };
	GCMsgHeader_t header;
	
	uint64 itemID;
};

struct GCTrading_UpdateTradeInfo_t
{
	enum { k_iMessage = k_EMsgGCTrading_UpdateTradeInfo };
	GCMsgHeader_t header;
	
	uint32 version;
	uint8 plyr1_numItems;
	uint8 plyr2_numItems;
	uint8 plyr1_numItems_showcase;
	uint8 plyr2_numItems_showcase;
	uint64 plyr1_showcase;
	uint64 plyr1_slot0;
	uint64 plyr1_slot1;
	uint64 plyr1_slot2;
	uint64 plyr1_slot3;
	uint64 plyr1_slot4;
	uint64 plyr1_slot5;
	uint64 plyr1_slot6;
	uint64 plyr1_slot7;
	uint64 plyr2_showcase;
	uint64 plyr2_slot0;
	uint64 plyr2_slot1;
	uint64 plyr2_slot2;
	uint64 plyr2_slot3;
	uint64 plyr2_slot4;
	uint64 plyr2_slot5;
	uint64 plyr2_slot6;
	uint64 plyr2_slot7;
};

// All 4 need to be true
struct GCTrading_ReadinessResponse_t
{
	enum { k_iMessage = k_EMsgGCTrading_ReadinessResponse };
	GCMsgHeader_t header;
	
	uint32 version;
	uint8 player1ready;
	uint8 player2ready;
	uint8 player1confirmed;
	uint8 player2confirmed;
};

struct GCTrading_SetReadiness_t
{
	enum { k_iMessage = k_EMsgGCTrading_SetReadiness };
	GCMsgHeader_t header;
	
	uint32 version;
	uint8 response;
};

struct GCTrading_ConfirmOffer_t
{
	enum { k_iMessage = k_EMsgGCTrading_ConfirmOffer };
	GCMsgHeader_t header;
	
	uint32 version;
};

struct GCTrading_SessionClosed_t
{
	enum { k_iMessage = k_EMsgGCTrading_SessionClosed };
	GCMsgHeader_t header;
	
	/*ETFTradeResult*/ uint32 result;
};

struct GCRespawnPostLoadoutChange_t
{
	enum { k_iMessage = k_EMsgGCRespawnPostLoadoutChange };
	GCMsgHeader_t header;
	
	CSteamID steamID;
};

#pragma pack(pop)

#endif // ISTEAMGAMECOORDINATORCOMMON_H
