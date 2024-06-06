//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;
using OpenSteamworks.Enums;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientFriends
{
    public string GetPersonaName();  // argc: 0, index: 1, ipc args: [], ipc returns: [string]
    public SteamAPICall_t SetPersonaName(string name);  // argc: 1, index: 2, ipc args: [string], ipc returns: [bytes8]
    public bool IsPersonaNameSet();  // argc: 0, index: 3, ipc args: [], ipc returns: [boolean]
    public EPersonaState GetPersonaState();  // argc: 0, index: 4, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public void SetPersonaState(EPersonaState state);  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool NotifyUIOfMenuChange(bool bShowAvatars, bool bSortByName, bool bShowOnlineOnly, bool bShowUntaggedFriends);  // argc: 4, index: 6, ipc args: [bytes1, bytes1, bytes1, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    // 4 = Direct
    // 8 = Group
    public unknown_ret GetFriendCount(EFriendFlags flags = EFriendFlags.Immediate);  // argc: 1, index: 7, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendArray(CSteamID[] steamid, StringBuilder wtf, int max, EFriendFlags flags);  // argc: 4, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendArrayInGame();  // argc: 3, index: 9, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public CSteamID GetFriendByIndex(int index, EFriendFlags flags = EFriendFlags.Immediate);  // argc: 3, index: 10, ipc args: [bytes4, bytes4], ipc returns: [uint64]
    public unknown_ret GetOnlineFriendCount();  // argc: 0, index: 11, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public EFriendRelationship GetFriendRelationship(CSteamID steamid);  // argc: 2, index: 12, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public EPersonaState GetFriendPersonaState(CSteamID friend);  // argc: 2, index: 13, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaName(CSteamID steamid);  // argc: 2, index: 14, ipc args: [uint64], ipc returns: [string]
    public HImage GetSmallFriendAvatar(CSteamID steamid);  // argc: 2, index: 15, ipc args: [uint64], ipc returns: [bytes4]
    public HImage GetMediumFriendAvatar(CSteamID steamid);  // argc: 2, index: 16, ipc args: [uint64], ipc returns: [bytes4]
    public HImage GetLargeFriendAvatar(CSteamID steamid);  // argc: 2, index: 17, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool BGetFriendAvatarURL(StringBuilder avatarOut, int avatarOutMax, CSteamID steamid, int unk);  // argc: 5, index: 18, ipc args: [bytes4, uint64, bytes4], ipc returns: [boolean, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool GetFriendAvatarHash(StringBuilder avatarOut, int avatarOutMax, CSteamID steamid);  // argc: 4, index: 19, ipc args: [bytes4, uint64], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public void SetFriendRegValue(CSteamID steamIDFriend, string key, string value);  // argc: 4, index: 20, ipc args: [uint64, string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public string GetFriendRegValue(CSteamID steamIDFriend, string key);  // argc: 3, index: 21, ipc args: [uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public bool DeleteFriendRegValue(CSteamID steamIDFriend, string key);  // argc: 3, index: 22, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool GetFriendGamePlayed(CSteamID steamid, out FriendGameInfo_t info);  // argc: 3, index: 23, ipc args: [uint64], ipc returns: [bytes1, bytes24]
    // WARNING: Arguments are unknown!
    public string GetFriendGamePlayedExtraInfo(CSteamID steamid);  // argc: 2, index: 24, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendGameServer();  // argc: 3, index: 25, ipc args: [uint64], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public EPersonaStateFlag GetFriendPersonaStateFlags(CSteamID steamid);  // argc: 2, index: 26, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendSessionStateInfo(CSteamID steamid, out FriendSessionStateInfo_t info);  // argc: 3, index: 27, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendRestrictions(CSteamID steamid);  // argc: 2, index: 28, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret64 GetFriendBroadcastID(CSteamID steamid);  // argc: 2, index: 29, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaNameHistory(CSteamID steamid, int index);  // argc: 3, index: 30, ipc args: [uint64, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t RequestPersonaNameHistory(CSteamID steamid);  // argc: 2, index: 31, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaNameHistoryAndDate(CSteamID steamid, int index, out RTime32 timestamp);  // argc: 4, index: 32, ipc args: [uint64, bytes4], ipc returns: [string, bytes4]
    // WARNING: Arguments are unknown!
    public bool AddFriend(CSteamID steamID);  // argc: 2, index: 33, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool RemoveFriend(CSteamID steamID);  // argc: 2, index: 34, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool HasFriend(CSteamID steamID, EFriendFlags flags);  // argc: 3, index: 35, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool RequestUserInformation(CSteamID steamID, bool requireNameOnly);  // argc: 3, index: 36, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetIgnoreFriend(CSteamID steamID, bool blocked);  // argc: 3, index: 37, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ReportChatDeclined(CSteamID steamID);  // argc: 2, index: 38, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateFriendsGroup(string groupname);  // argc: 1, index: 39, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteFriendsGroup(short id);  // argc: 1, index: 40, ipc args: [bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RenameFriendsGroup(string newname, short id);  // argc: 2, index: 41, ipc args: [string, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddFriendToGroup(CSteamID steamid, short groupid);  // argc: 3, index: 42, ipc args: [uint64, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveFriendFromGroup(CSteamID steamid, short groupid);  // argc: 3, index: 43, ipc args: [uint64, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool IsFriendMemberOfFriendsGroup(CSteamID steamid, short groupid);  // argc: 3, index: 44, ipc args: [uint64, bytes2], ipc returns: [boolean]
    public int GetFriendsGroupCount();  // argc: 0, index: 45, ipc args: [], ipc returns: [bytes2]
    public short GetFriendsGroupIDByIndex(int index);  // argc: 1, index: 46, ipc args: [bytes2], ipc returns: [bytes2]
    public string GetFriendsGroupName(short groupid);  // argc: 1, index: 47, ipc args: [bytes2], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public int GetFriendsGroupMembershipCount(short groupid);  // argc: 1, index: 48, ipc args: [bytes2], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public CSteamID GetFirstFriendsGroupMember(short groupid);  // argc: 2, index: 49, ipc args: [bytes2], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public CSteamID GetNextFriendsGroupMember(short groupid);  // argc: 2, index: 50, ipc args: [bytes2], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGroupFriendsMembershipCount();  // argc: 2, index: 51, ipc args: [uint64], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFirstGroupFriendsMember();  // argc: 2, index: 52, ipc args: [uint64], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNextGroupFriendsMember();  // argc: 2, index: 53, ipc args: [uint64], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public string GetPlayerNickname(CSteamID steamid);  // argc: 2, index: 54, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public bool SetPlayerNickname(CSteamID steamid, string nickname);  // argc: 3, index: 55, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendSteamLevel(CSteamID steamid);  // argc: 2, index: 56, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatMessagesCount(CSteamID steamid);  // argc: 2, index: 57, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatMessage();  // argc: 8, index: 58, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4, bytes8, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SendMsgToFriend(CSteamID steamIDFriend, EChatEntryType eFriendMsgType, string msg);  // argc: 4, index: 59, ipc args: [uint64, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearChatHistory(CSteamID steamIDFriend);  // argc: 2, index: 60, ipc args: [uint64], ipc returns: []
    public unknown_ret GetKnownClanCount();  // argc: 0, index: 61, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetKnownClanByIndex();  // argc: 2, index: 62, ipc args: [bytes4], ipc returns: [uint64]
    public unknown_ret GetClanCount();  // argc: 0, index: 63, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanByIndex();  // argc: 2, index: 64, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanName();  // argc: 2, index: 65, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanTag();  // argc: 2, index: 66, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendActivityCounts();  // argc: 3, index: 67, ipc args: [bytes1], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanActivityCounts();  // argc: 5, index: 68, ipc args: [uint64], ipc returns: [bytes1, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadClanActivityCounts();  // argc: 2, index: 69, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendsGroupActivityCounts();  // argc: 3, index: 70, ipc args: [bytes2], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanPublic();  // argc: 2, index: 71, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanOfficialGameGroup();  // argc: 2, index: 72, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret JoinClanChatRoom(CSteamID steamID);  // argc: 2, index: 73, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret LeaveClanChatRoom();  // argc: 2, index: 74, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanChatMemberCount();  // argc: 2, index: 75, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatMemberByIndex();  // argc: 4, index: 76, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret SendClanChatMessage();  // argc: 3, index: 77, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanChatMessage();  // argc: 7, index: 78, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4, uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanChatAdmin();  // argc: 4, index: 79, ipc args: [uint64, uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanChatWindowOpenInSteam();  // argc: 2, index: 80, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret OpenClanChatWindowInSteam();  // argc: 2, index: 81, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CloseClanChatWindowInSteam();  // argc: 2, index: 82, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetListenForFriendsMessages(bool listen);  // argc: 1, index: 83, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ReplyToFriendMessage(CSteamID steamid, string msg);  // argc: 3, index: 84, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendMessage();  // argc: 6, index: 85, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InviteFriendToClan();  // argc: 4, index: 86, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AcknowledgeInviteToClan();  // argc: 3, index: 87, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendCountFromSource();  // argc: 2, index: 88, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendFromSourceByIndex();  // argc: 4, index: 89, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret IsUserInSource();  // argc: 4, index: 90, ipc args: [uint64, uint64], ipc returns: [boolean]
    public unknown_ret GetCoplayFriendCount();  // argc: 0, index: 91, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCoplayFriend();  // argc: 2, index: 92, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendCoplayTime();  // argc: 2, index: 93, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendCoplayGame();  // argc: 2, index: 94, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool SetRichPresence(AppId_t appid, string key, string value);  // argc: 3, index: 95, ipc args: [bytes4, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearRichPresence(AppId_t appid);  // argc: 1, index: 96, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public string GetFriendRichPresence(AppId_t appid, CSteamID steamid, string key);  // argc: 4, index: 97, ipc args: [bytes4, uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public int GetFriendRichPresenceKeyCount(AppId_t appid, CSteamID steamid);  // argc: 3, index: 98, ipc args: [bytes4, uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public string GetFriendRichPresenceKeyByIndex(AppId_t appid, CSteamID steamid, int index);  // argc: 4, index: 99, ipc args: [bytes4, uint64, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public void RequestFriendRichPresence(AppId_t appid, CSteamID steamid);  // argc: 3, index: 100, ipc args: [bytes4, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret JoinChatRoom(CSteamID steamID);  // argc: 2, index: 101, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LeaveChatRoom(CSteamID steamID);  // argc: 2, index: 102, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret InviteUserToChatRoom(CSteamID steamID,CSteamID steamID2);  // argc: 4, index: 103, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SendChatMsg(CSteamID steamIDFriend, EChatEntryType eFriendMsgType, string msg);  // argc: 4, index: 104, ipc args: [uint64, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomMessagesCount(CSteamID steamID);  // argc: 2, index: 105, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomEntry();  // argc: 7, index: 106, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, uint64, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearChatRoomHistory();  // argc: 2, index: 107, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SerializeChatRoomDlg();  // argc: 4, index: 108, ipc args: [uint64, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSizeOfSerializedChatRoomDlg();  // argc: 2, index: 109, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSerializedChatRoomDlg();  // argc: 6, index: 110, ipc args: [uint64, bytes4], ipc returns: [bytes1, bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearSerializedChatRoomDlg();  // argc: 2, index: 111, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret KickChatMember();  // argc: 4, index: 112, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BanChatMember();  // argc: 4, index: 113, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UnBanChatMember();  // argc: 4, index: 114, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetChatRoomType();  // argc: 3, index: 115, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomLockState();  // argc: 3, index: 116, ipc args: [uint64], ipc returns: [bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomPermissions();  // argc: 3, index: 117, ipc args: [uint64], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetChatRoomModerated();  // argc: 3, index: 118, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BChatRoomModerated();  // argc: 2, index: 119, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyChatRoomDlgsOfUIChange();  // argc: 6, index: 120, ipc args: [uint64, bytes1, bytes1, bytes1, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TerminateChatRoom();  // argc: 2, index: 121, ipc args: [uint64], ipc returns: [bytes1]
    public unknown_ret GetChatRoomCount();  // argc: 0, index: 122, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomByIndex();  // argc: 2, index: 123, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public string GetChatRoomName(CSteamID steamidGroup);  // argc: 2, index: 124, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetChatRoomMemberDetails();  // argc: 6, index: 125, ipc args: [uint64, uint64], ipc returns: [boolean, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateChatRoom();  // argc: 14, index: 126, ipc args: [bytes4, bytes8, string, bytes4, uint64, uint64, uint64, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret JoinChatRoomGroup();  // argc: 4, index: 127, ipc args: [bytes8, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ShowChatRoomGroupInvite();  // argc: 1, index: 128, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceCallNew(CSteamID steamIDLocalPeer, CSteamID steamIDRemotePeer);  // argc: 4, index: 129, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceCall(CSteamID steamIDLocalPeer, CSteamID steamIDRemotePeer);  // argc: 4, index: 130, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceHangUp(CSteamID steamIDLocalPeer, int hVoiceCall);  // argc: 3, index: 131, ipc args: [uint64, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVoiceSpeakerVolume();  // argc: 1, index: 132, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVoiceMicrophoneVolume();  // argc: 1, index: 133, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoAnswer(bool autoAnswer);  // argc: 1, index: 134, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceAnswer(int hVoiceCall);  // argc: 1, index: 135, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AcceptVoiceCall(CSteamID steamIDLocalPeer, CSteamID steamIDRemotePeer);  // argc: 4, index: 136, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoicePutOnHold();  // argc: 2, index: 137, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BVoiceIsLocalOnHold();  // argc: 1, index: 138, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BVoiceIsRemoteOnHold();  // argc: 1, index: 139, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetDoNotDisturb();  // argc: 1, index: 140, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EnableVoiceNotificationSounds(bool enable);  // argc: 1, index: 141, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPushToTalkEnabled();  // argc: 2, index: 142, ipc args: [bytes1, bytes1], ipc returns: []
    public unknown_ret IsPushToTalkEnabled();  // argc: 0, index: 143, ipc args: [], ipc returns: [boolean]
    public unknown_ret IsPushToMuteEnabled();  // argc: 0, index: 144, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPushToTalkKey(int nVirtualKey);  // argc: 1, index: 145, ipc args: [bytes4], ipc returns: []
    public int GetPushToTalkKey();  // argc: 0, index: 146, ipc args: [], ipc returns: [bytes4]
    public bool IsPushToTalkKeyDown();  // argc: 0, index: 147, ipc args: [], ipc returns: [boolean]
    public void EnableVoiceCalibration(bool enable);  // argc: 1, index: 148, ipc args: [bytes1], ipc returns: []
    public bool IsVoiceCalibrating();  // argc: 0, index: 149, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetVoiceCalibrationSamplePeak();  // argc: 0, index: 150, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetMicBoost();  // argc: 1, index: 151, ipc args: [bytes1], ipc returns: []
    public unknown_ret GetMicBoost();  // argc: 0, index: 152, ipc args: [], ipc returns: [bytes1]
    public unknown_ret HasHardwareMicBoost();  // argc: 0, index: 153, ipc args: [], ipc returns: [bytes1]
    public string GetMicDeviceName();  // argc: 0, index: 154, ipc args: [], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret StartTalking();  // argc: 1, index: 155, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EndTalking();  // argc: 1, index: 156, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceIsValid();  // argc: 1, index: 157, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoReflectVoice();  // argc: 1, index: 158, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetCallState();  // argc: 1, index: 159, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret GetVoiceMicrophoneVolume();  // argc: 0, index: 160, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetVoiceSpeakerVolume();  // argc: 0, index: 161, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TimeSinceLastVoiceDataReceived();  // argc: 1, index: 162, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TimeSinceLastVoiceDataSend();  // argc: 1, index: 163, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BCanSend();  // argc: 1, index: 164, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BCanReceive();  // argc: 1, index: 165, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEstimatedBitsPerSecond();  // argc: 2, index: 166, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPeakSample();  // argc: 2, index: 167, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SendResumeRequest();  // argc: 1, index: 168, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public void OpenFriendsDialog(bool unk = false, bool unk1 = false);  // argc: 2, index: 169, ipc args: [bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public void OpenChatDialog(CSteamID steamIDuser);  // argc: 2, index: 170, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public void OpenInviteToTradeDialog(CSteamID steamIDuser);  // argc: 2, index: 171, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public void StartChatRoomVoiceSpeaking(CSteamID steamIDchat, CSteamID steamIDuser);  // argc: 4, index: 172, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public void EndChatRoomVoiceSpeaking(CSteamID steamIDchat, CSteamID steamIDuser);  // argc: 4, index: 173, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public RTime32 GetFriendLastLogonTime(CSteamID steamid);  // argc: 2, index: 174, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public RTime32 GetFriendLastLogoffTime(CSteamID steamid);  // argc: 2, index: 175, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceTotalSlotCount();  // argc: 2, index: 176, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceUsedSlotCount();  // argc: 2, index: 177, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceUsedSlot();  // argc: 4, index: 178, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceStatus();  // argc: 4, index: 179, ipc args: [uint64, uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BChatRoomHasAvailableVoiceSlots();  // argc: 2, index: 180, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsChatRoomVoiceSpeaking();  // argc: 4, index: 181, ipc args: [uint64, uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomPeakSample();  // argc: 5, index: 182, ipc args: [uint64, uint64, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ChatRoomVoiceRetryConnections();  // argc: 2, index: 183, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPortTypes();  // argc: 1, index: 184, ipc args: [bytes4], ipc returns: []
    public void ReinitAudio();  // argc: 0, index: 185, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetInGameVoiceSpeaking();  // argc: 3, index: 186, ipc args: [uint64, bytes1], ipc returns: []
    public unknown_ret IsInGameVoiceSpeaking();  // argc: 0, index: 187, ipc args: [], ipc returns: [boolean]
    public void ActivateGameOverlay(string dialog);  // argc: 1, index: 188, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayToUser(string dialog, CSteamID steamid);  // argc: 3, index: 189, ipc args: [string, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayToWebPage();  // argc: 2, index: 190, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayToStore();  // argc: 2, index: 191, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayInviteDialog(CSteamID serverid);  // argc: 2, index: 192, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayRemotePlayTogetherInviteDialog();  // argc: 2, index: 193, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayInviteDialogConnectString();  // argc: 1, index: 194, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public byte ProcessActivateGameOverlayInMainUI(string unk, CSteamID unk1, uint unk2, bool unk3, uint unk4);  // argc: 6, index: 195, ipc args: [string, uint64, bytes4, bytes1, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public void NotifyGameOverlayStateChanged(uint unk1, uint unk2, bool unk3, bool unk4);  // argc: 4, index: 196, ipc args: [bytes4, bytes4, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyGameServerChangeRequested();  // argc: 2, index: 197, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool NotifyLobbyJoinRequested(AppId_t appid, CSteamID steamidFriend, CSteamID steamidLobby);  // argc: 5, index: 198, ipc args: [bytes4, uint64, uint64], ipc returns: [bytes1]
    public bool NotifyRichPresenceJoinRequested(AppId_t appid, CSteamID friendid, string data);  // argc: 4, index: 199, ipc args: [bytes4, uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanRelationship();  // argc: 2, index: 200, ipc args: [uint64], ipc returns: [bytes4]
    public unknown_ret GetClanInviteCount();  // argc: 0, index: 201, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendClanRank();  // argc: 4, index: 202, ipc args: [uint64, uint64], ipc returns: [bytes4]
    public bool VoiceIsAvailable();  // argc: 0, index: 203, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TestVoiceDisconnect();  // argc: 1, index: 204, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TestChatRoomPeerDisconnect();  // argc: 4, index: 205, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TestVoicePacketLoss();  // argc: 1, index: 206, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FindFriendVoiceChatHandle(CSteamID steamid);  // argc: 2, index: 207, ipc args: [uint64], ipc returns: [bytes4]
    public void RequestFriendsWhoPlayGame(CGameID gameid);  // argc: 1, index: 208, ipc args: [bytes8], ipc returns: []
    public int GetCountFriendsWhoPlayGame(CGameID gameid);  // argc: 1, index: 209, ipc args: [bytes8], ipc returns: [bytes4]
    public CSteamID GetFriendWhoPlaysGame(int index, CGameID gameid);  // argc: 3, index: 210, ipc args: [bytes4, bytes8], ipc returns: [uint64]
    public int GetCountFriendsInGame(CGameID gameid);  // argc: 1, index: 211, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlayedWith(CSteamID steamid);  // argc: 2, index: 212, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RequestClanOfficerList();  // argc: 2, index: 213, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanOwner();  // argc: 3, index: 214, ipc args: [uint64], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanOfficerCount();  // argc: 2, index: 215, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanOfficerByIndex();  // argc: 4, index: 216, ipc args: [uint64, bytes4], ipc returns: [uint64]
    public unknown_ret GetUserRestrictions();  // argc: 0, index: 217, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t RequestFriendProfileInfo(CSteamID steamid);  // argc: 2, index: 218, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public string GetFriendProfileInfo(CSteamID steamid, string key);  // argc: 3, index: 219, ipc args: [uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public bool InviteUserToGame(AppId_t appid, CSteamID friendid, string msg);  // argc: 4, index: 220, ipc args: [bytes4, uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t RequestTrade(CSteamID friendid);  // argc: 2, index: 221, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret TradeResponse();  // argc: 2, index: 222, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CancelTradeRequest();  // argc: 2, index: 223, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret HideFriend();  // argc: 3, index: 224, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFollowerCount();  // argc: 2, index: 225, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret IsFollowing();  // argc: 2, index: 226, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateFollowingList();  // argc: 1, index: 227, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestFriendMessageHistory();  // argc: 2, index: 228, ipc args: [uint64], ipc returns: []
    public unknown_ret RequestFriendMessageHistoryForOfflineMessages();  // argc: 0, index: 229, ipc args: [], ipc returns: []
    public unknown_ret GetCountFriendsWithOfflineMessages();  // argc: 0, index: 230, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendWithOfflineMessage();  // argc: 1, index: 231, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearFriendHasOfflineMessage();  // argc: 1, index: 232, ipc args: [bytes4], ipc returns: []
    public unknown_ret RequestEmoticonList();  // argc: 0, index: 233, ipc args: [], ipc returns: []
    public unknown_ret GetEmoticonCount();  // argc: 0, index: 234, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEmoticonName();  // argc: 1, index: 235, ipc args: [bytes4], ipc returns: [string]
    public void ClientLinkFilterInit();  // argc: 0, index: 236, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret LinkDisposition();  // argc: 1, index: 237, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaName_Public();  // argc: 2, index: 238, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public string GetPlayerNickname_Public();  // argc: 2, index: 239, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetFriendsUIActiveClanChatList();  // argc: 2, index: 240, ipc args: [bytes4, bytes_length_from_reg], ipc returns: []
    public unknown_ret GetNumChatsWithUnreadPriorityMessages();  // argc: 0, index: 241, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetNumChatsWithUnreadPriorityMessages();  // argc: 1, index: 242, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RegisterProtocolInOverlayBrowser();  // argc: 1, index: 243, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret HandleProtocolForOverlayBrowser();  // argc: 2, index: 244, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestEquippedProfileItems();  // argc: 2, index: 245, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BHasEquippedProfileItem();  // argc: 3, index: 246, ipc args: [uint64, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetProfileItemPropertyString();  // argc: 4, index: 247, ipc args: [uint64, bytes4, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetProfileItemPropertyUint();  // argc: 4, index: 248, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadCommunityItemAsset();  // argc: 4, index: 249, ipc args: [bytes8, string, string], ipc returns: [bytes8]
}