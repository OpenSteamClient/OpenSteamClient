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
    // WARNING: Arguments are unknown!
    public void SetPersonaName(string name);  // argc: 1, index: 0, ipc args: [string], ipc returns: [bytes8]
    public bool IsPersonaNameSet();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public EPersonaState GetPersonaState();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public void SetPersonaState(EPersonaState state);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyUIOfMenuChange();  // argc: 4, index: 1, ipc args: [bytes1, bytes1, bytes1, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    // 4 = Direct
    // 8 = Group
    public unknown_ret GetFriendCount(EFriendFlags flags);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendArray();  // argc: 4, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendArrayInGame();  // argc: 3, index: 4, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_retu64 GetFriendByIndex(int index, EFriendFlags flags, int unk);  // argc: 3, index: 5, ipc args: [bytes4, bytes4], ipc returns: [uint64]
    public unknown_ret GetOnlineFriendCount();  // argc: 0, index: 6, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public EFriendRelationship GetFriendRelationship(CSteamID steamid);  // argc: 2, index: 0, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public EPersonaState GetFriendPersonaState(CSteamID friend);  // argc: 2, index: 1, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaName(CSteamID steamid);  // argc: 2, index: 2, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSmallFriendAvatar(CSteamID steamid);  // argc: 2, index: 3, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMediumFriendAvatar(CSteamID steamid);  // argc: 2, index: 4, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLargeFriendAvatar(CSteamID steamid);  // argc: 2, index: 5, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool BGetFriendAvatarURL(StringBuilder avatarOut, int avatarOutMax, CSteamID steamid, int unk);  // argc: 5, index: 6, ipc args: [bytes4, uint64, bytes4], ipc returns: [boolean, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendAvatarHash(StringBuilder avatarOut, int avatarOutMax, CSteamID steamid);  // argc: 4, index: 7, ipc args: [bytes4, uint64], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public void SetFriendRegValue(CSteamID steamIDFriend, string key, string value);  // argc: 4, index: 8, ipc args: [uint64, string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public string GetFriendRegValue(CSteamID steamIDFriend, string key);  // argc: 3, index: 9, ipc args: [uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public bool DeleteFriendRegValue(CSteamID steamIDFriend, string key);  // argc: 3, index: 10, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool GetFriendGamePlayed(CSteamID steamid, out FriendGameInfo_t info);  // argc: 3, index: 11, ipc args: [uint64], ipc returns: [bytes1, bytes24]
    // WARNING: Arguments are unknown!
    public string GetFriendGamePlayedExtraInfo(CSteamID steamid);  // argc: 2, index: 12, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendGameServer();  // argc: 3, index: 13, ipc args: [uint64], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public EPersonaStateFlag GetFriendPersonaStateFlags(CSteamID steamid);  // argc: 2, index: 14, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendSessionStateInfo(CSteamID steamid, out FriendSessionStateInfo_t info);  // argc: 3, index: 15, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendRestrictions(CSteamID steamid);  // argc: 2, index: 16, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret64 GetFriendBroadcastID(CSteamID steamid);  // argc: 2, index: 17, ipc args: [uint64], ipc returns: [bytes8]
    /// <summary>
    /// Get old friend names by doing a while call.
    /// Returns an empty string when no more names are available.
    /// </summary>
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaNameHistory(CSteamID steamid, int index);  // argc: 3, index: 18, ipc args: [uint64, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t RequestPersonaNameHistory(CSteamID steamid);  // argc: 2, index: 19, ipc args: [uint64], ipc returns: [bytes8]
    /// <summary>
    /// Get old friend names by doing a while call.
    /// Returns an empty string when no more names are available.
    /// Also returns a timestamp of change.
    /// </summary>
    // WARNING: Arguments are unknown!
    public string GetFriendPersonaNameHistoryAndDate(CSteamID steamid, int index, out RTime32 timestamp);  // argc: 4, index: 20, ipc args: [uint64, bytes4], ipc returns: [string, bytes4]
    // WARNING: Arguments are unknown!
    public bool AddFriend(CSteamID steamID);  // argc: 2, index: 21, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool RemoveFriend(CSteamID steamID);  // argc: 2, index: 22, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool HasFriend(CSteamID steamID, EFriendFlags flags);  // argc: 3, index: 23, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool RequestUserInformation(CSteamID steamID, bool requireNameOnly);  // argc: 3, index: 24, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetIgnoreFriend(CSteamID steamID, bool blocked);  // argc: 3, index: 25, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ReportChatDeclined(CSteamID steamID);  // argc: 2, index: 26, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateFriendsGroup(string groupname);  // argc: 1, index: 27, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteFriendsGroup(short id);  // argc: 1, index: 28, ipc args: [bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RenameFriendsGroup(string newname, short id);  // argc: 2, index: 29, ipc args: [string, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddFriendToGroup(CSteamID steamid, short groupid);  // argc: 3, index: 30, ipc args: [uint64, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveFriendFromGroup(CSteamID steamid, short groupid);  // argc: 3, index: 31, ipc args: [uint64, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool IsFriendMemberOfFriendsGroup(CSteamID steamid, short groupid);  // argc: 3, index: 32, ipc args: [uint64, bytes2], ipc returns: [boolean]
    public int GetFriendsGroupCount();  // argc: 0, index: 33, ipc args: [], ipc returns: [bytes2]
    public short GetFriendsGroupIDByIndex(int index);  // argc: 1, index: 0, ipc args: [bytes2], ipc returns: [bytes2]
    public string GetFriendsGroupName(short groupid);  // argc: 1, index: 1, ipc args: [bytes2], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public int GetFriendsGroupMembershipCount(short groupid);  // argc: 1, index: 2, ipc args: [bytes2], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public CSteamID GetFirstFriendsGroupMember(short groupid);  // argc: 2, index: 3, ipc args: [bytes2], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public CSteamID GetNextFriendsGroupMember(short groupid);  // argc: 2, index: 4, ipc args: [bytes2], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGroupFriendsMembershipCount();  // argc: 2, index: 5, ipc args: [uint64], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFirstGroupFriendsMember();  // argc: 2, index: 6, ipc args: [uint64], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNextGroupFriendsMember();  // argc: 2, index: 7, ipc args: [uint64], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlayerNickname(CSteamID steamid);  // argc: 2, index: 8, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlayerNickname(CSteamID steamid, string nickname);  // argc: 3, index: 9, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendSteamLevel(CSteamID steamid);  // argc: 2, index: 10, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatMessagesCount(CSteamID steamid);  // argc: 2, index: 11, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatMessage();  // argc: 8, index: 12, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4, bytes8, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SendMsgToFriend(CSteamID steamIDFriend, EChatEntryType eFriendMsgType, string msg);  // argc: 4, index: 13, ipc args: [uint64, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearChatHistory(CSteamID steamIDFriend);  // argc: 2, index: 14, ipc args: [uint64], ipc returns: []
    public unknown_ret GetKnownClanCount();  // argc: 0, index: 15, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetKnownClanByIndex();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [uint64]
    public unknown_ret GetClanCount();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanByIndex();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanName();  // argc: 2, index: 1, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanTag();  // argc: 2, index: 2, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendActivityCounts();  // argc: 3, index: 3, ipc args: [bytes1], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanActivityCounts();  // argc: 5, index: 4, ipc args: [uint64], ipc returns: [bytes1, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadClanActivityCounts();  // argc: 2, index: 5, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendsGroupActivityCounts();  // argc: 3, index: 6, ipc args: [bytes2], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanPublic();  // argc: 2, index: 7, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanOfficialGameGroup();  // argc: 2, index: 8, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret JoinClanChatRoom(CSteamID steamID);  // argc: 2, index: 9, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret LeaveClanChatRoom();  // argc: 2, index: 10, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanChatMemberCount();  // argc: 2, index: 11, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatMemberByIndex();  // argc: 4, index: 12, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret SendClanChatMessage();  // argc: 3, index: 13, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanChatMessage();  // argc: 7, index: 14, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4, uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanChatAdmin();  // argc: 4, index: 15, ipc args: [uint64, uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret IsClanChatWindowOpenInSteam();  // argc: 2, index: 16, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret OpenClanChatWindowInSteam();  // argc: 2, index: 17, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CloseClanChatWindowInSteam();  // argc: 2, index: 18, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetListenForFriendsMessages(bool listen);  // argc: 1, index: 19, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ReplyToFriendMessage(CSteamID steamid, string msg);  // argc: 3, index: 20, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendMessage();  // argc: 6, index: 21, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InviteFriendToClan();  // argc: 4, index: 22, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AcknowledgeInviteToClan();  // argc: 3, index: 23, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendCountFromSource();  // argc: 2, index: 24, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendFromSourceByIndex();  // argc: 4, index: 25, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret IsUserInSource();  // argc: 4, index: 26, ipc args: [uint64, uint64], ipc returns: [boolean]
    public unknown_ret GetCoplayFriendCount();  // argc: 0, index: 27, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCoplayFriend();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendCoplayTime();  // argc: 2, index: 1, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendCoplayGame();  // argc: 2, index: 2, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRichPresence(AppId_t appid, string key, string value);  // argc: 3, index: 3, ipc args: [bytes4, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearRichPresence(AppId_t appid);  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public string GetFriendRichPresence(AppId_t appid, CSteamID steamid, string key);  // argc: 4, index: 5, ipc args: [bytes4, uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendRichPresenceKeyCount(AppId_t appid, CSteamID steamid);  // argc: 3, index: 6, ipc args: [bytes4, uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendRichPresenceKeyByIndex();  // argc: 4, index: 7, ipc args: [bytes4, uint64, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestFriendRichPresence();  // argc: 3, index: 8, ipc args: [bytes4, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret JoinChatRoom(CSteamID steamID);  // argc: 2, index: 9, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LeaveChatRoom(CSteamID steamID);  // argc: 2, index: 10, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret InviteUserToChatRoom(CSteamID steamID,CSteamID steamID2);  // argc: 4, index: 11, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SendChatMsg();  // argc: 4, index: 12, ipc args: [uint64, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomMessagesCount(CSteamID steamID);  // argc: 2, index: 13, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomEntry();  // argc: 7, index: 14, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, uint64, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearChatRoomHistory();  // argc: 2, index: 15, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SerializeChatRoomDlg();  // argc: 4, index: 16, ipc args: [uint64, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSizeOfSerializedChatRoomDlg();  // argc: 2, index: 17, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSerializedChatRoomDlg();  // argc: 6, index: 18, ipc args: [uint64, bytes4], ipc returns: [bytes1, bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearSerializedChatRoomDlg();  // argc: 2, index: 19, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret KickChatMember();  // argc: 4, index: 20, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BanChatMember();  // argc: 4, index: 21, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UnBanChatMember();  // argc: 4, index: 22, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetChatRoomType();  // argc: 3, index: 23, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomLockState();  // argc: 3, index: 24, ipc args: [uint64], ipc returns: [bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomPermissions();  // argc: 3, index: 25, ipc args: [uint64], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetChatRoomModerated();  // argc: 3, index: 26, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BChatRoomModerated();  // argc: 2, index: 27, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyChatRoomDlgsOfUIChange();  // argc: 6, index: 28, ipc args: [uint64, bytes1, bytes1, bytes1, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TerminateChatRoom();  // argc: 2, index: 29, ipc args: [uint64], ipc returns: [bytes1]
    public unknown_ret GetChatRoomCount();  // argc: 0, index: 30, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomByIndex();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomName();  // argc: 2, index: 1, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetChatRoomMemberDetails();  // argc: 6, index: 2, ipc args: [uint64, uint64], ipc returns: [boolean, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateChatRoom();  // argc: 14, index: 3, ipc args: [bytes4, bytes8, string, bytes4, uint64, uint64, uint64, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret JoinChatRoomGroup();  // argc: 4, index: 4, ipc args: [bytes8, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ShowChatRoomGroupInvite();  // argc: 1, index: 5, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceCallNew();  // argc: 4, index: 6, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceCall();  // argc: 4, index: 7, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceHangUp();  // argc: 3, index: 8, ipc args: [uint64, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVoiceSpeakerVolume();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVoiceMicrophoneVolume();  // argc: 1, index: 10, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoAnswer();  // argc: 1, index: 11, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceAnswer();  // argc: 1, index: 12, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AcceptVoiceCall();  // argc: 4, index: 13, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoicePutOnHold();  // argc: 2, index: 14, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BVoiceIsLocalOnHold();  // argc: 1, index: 15, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BVoiceIsRemoteOnHold();  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetDoNotDisturb();  // argc: 1, index: 17, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EnableVoiceNotificationSounds();  // argc: 1, index: 18, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPushToTalkEnabled();  // argc: 2, index: 19, ipc args: [bytes1, bytes1], ipc returns: []
    public unknown_ret IsPushToTalkEnabled();  // argc: 0, index: 20, ipc args: [], ipc returns: [boolean]
    public unknown_ret IsPushToMuteEnabled();  // argc: 0, index: 0, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPushToTalkKey();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetPushToTalkKey();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    public unknown_ret IsPushToTalkKeyDown();  // argc: 0, index: 0, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret EnableVoiceCalibration();  // argc: 1, index: 0, ipc args: [bytes1], ipc returns: []
    public unknown_ret IsVoiceCalibrating();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetVoiceCalibrationSamplePeak();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetMicBoost();  // argc: 1, index: 0, ipc args: [bytes1], ipc returns: []
    public unknown_ret GetMicBoost();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes1]
    public unknown_ret HasHardwareMicBoost();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetMicDeviceName();  // argc: 0, index: 0, ipc args: [], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret StartTalking();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EndTalking();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret VoiceIsValid();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAutoReflectVoice();  // argc: 1, index: 3, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetCallState();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret GetVoiceMicrophoneVolume();  // argc: 0, index: 5, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetVoiceSpeakerVolume();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TimeSinceLastVoiceDataReceived();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TimeSinceLastVoiceDataSend();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BCanSend();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BCanReceive();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEstimatedBitsPerSecond();  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPeakSample();  // argc: 2, index: 5, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SendResumeRequest();  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OpenFriendsDialog();  // argc: 2, index: 7, ipc args: [bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OpenChatDialog();  // argc: 2, index: 8, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OpenInviteToTradeDialog();  // argc: 2, index: 9, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartChatRoomVoiceSpeaking();  // argc: 4, index: 10, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EndChatRoomVoiceSpeaking();  // argc: 4, index: 11, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendLastLogonTime();  // argc: 2, index: 12, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendLastLogoffTime();  // argc: 2, index: 13, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceTotalSlotCount();  // argc: 2, index: 14, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceUsedSlotCount();  // argc: 2, index: 15, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceUsedSlot();  // argc: 4, index: 16, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomVoiceStatus();  // argc: 4, index: 17, ipc args: [uint64, uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BChatRoomHasAvailableVoiceSlots();  // argc: 2, index: 18, ipc args: [uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsChatRoomVoiceSpeaking();  // argc: 4, index: 19, ipc args: [uint64, uint64], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetChatRoomPeakSample();  // argc: 5, index: 20, ipc args: [uint64, uint64, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ChatRoomVoiceRetryConnections();  // argc: 2, index: 21, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPortTypes();  // argc: 1, index: 22, ipc args: [bytes4], ipc returns: []
    public unknown_ret ReinitAudio();  // argc: 0, index: 23, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetInGameVoiceSpeaking();  // argc: 3, index: 0, ipc args: [uint64, bytes1], ipc returns: []
    public unknown_ret IsInGameVoiceSpeaking();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlay();  // argc: 1, index: 0, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayToUser();  // argc: 3, index: 1, ipc args: [string, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayToWebPage();  // argc: 2, index: 2, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayToStore();  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayInviteDialog();  // argc: 2, index: 4, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayRemotePlayTogetherInviteDialog();  // argc: 2, index: 5, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateGameOverlayInviteDialogConnectString();  // argc: 1, index: 6, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ProcessActivateGameOverlayInMainUI();  // argc: 6, index: 7, ipc args: [string, uint64, bytes4, bytes1, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyGameOverlayStateChanged();  // argc: 4, index: 8, ipc args: [bytes4, bytes4, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyGameServerChangeRequested();  // argc: 2, index: 9, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyLobbyJoinRequested();  // argc: 5, index: 10, ipc args: [bytes4, uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyRichPresenceJoinRequested();  // argc: 4, index: 11, ipc args: [bytes4, uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanRelationship();  // argc: 2, index: 12, ipc args: [uint64], ipc returns: [bytes4]
    public unknown_ret GetClanInviteCount();  // argc: 0, index: 13, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendClanRank();  // argc: 4, index: 0, ipc args: [uint64, uint64], ipc returns: [bytes4]
    public unknown_ret VoiceIsAvailable();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TestVoiceDisconnect();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TestChatRoomPeerDisconnect();  // argc: 4, index: 1, ipc args: [uint64, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TestVoicePacketLoss();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FindFriendVoiceChatHandle();  // argc: 2, index: 3, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestFriendsWhoPlayGame();  // argc: 1, index: 4, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetCountFriendsWhoPlayGame();  // argc: 1, index: 5, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendWhoPlaysGame();  // argc: 3, index: 6, ipc args: [bytes4, bytes8], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCountFriendsInGame();  // argc: 1, index: 7, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlayedWith();  // argc: 2, index: 8, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RequestClanOfficerList();  // argc: 2, index: 9, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanOwner();  // argc: 3, index: 10, ipc args: [uint64], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanOfficerCount();  // argc: 2, index: 11, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetClanOfficerByIndex();  // argc: 4, index: 12, ipc args: [uint64, bytes4], ipc returns: [uint64]
    public unknown_ret GetUserRestrictions();  // argc: 0, index: 13, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestFriendProfileInfo();  // argc: 2, index: 0, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendProfileInfo();  // argc: 3, index: 1, ipc args: [uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret InviteUserToGame();  // argc: 4, index: 2, ipc args: [bytes4, uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestTrade();  // argc: 2, index: 3, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret TradeResponse();  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CancelTradeRequest();  // argc: 2, index: 5, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret HideFriend();  // argc: 3, index: 6, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFollowerCount();  // argc: 2, index: 7, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret IsFollowing();  // argc: 2, index: 8, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateFollowingList();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestFriendMessageHistory();  // argc: 2, index: 10, ipc args: [uint64], ipc returns: []
    public unknown_ret RequestFriendMessageHistoryForOfflineMessages();  // argc: 0, index: 11, ipc args: [], ipc returns: []
    public unknown_ret GetCountFriendsWithOfflineMessages();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendWithOfflineMessage();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearFriendHasOfflineMessage();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    public unknown_ret RequestEmoticonList();  // argc: 0, index: 2, ipc args: [], ipc returns: []
    public unknown_ret GetEmoticonCount();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEmoticonName();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [string]
    public unknown_ret ClientLinkFilterInit();  // argc: 0, index: 1, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret LinkDisposition();  // argc: 1, index: 0, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFriendPersonaName_Public();  // argc: 2, index: 1, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlayerNickname_Public();  // argc: 2, index: 2, ipc args: [uint64], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetFriendsUIActiveClanChatList();  // argc: 2, index: 3, ipc args: [bytes4, bytes_length_from_reg], ipc returns: []
    public unknown_ret GetNumChatsWithUnreadPriorityMessages();  // argc: 0, index: 4, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetNumChatsWithUnreadPriorityMessages();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RegisterProtocolInOverlayBrowser();  // argc: 1, index: 1, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret HandleProtocolForOverlayBrowser();  // argc: 2, index: 2, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestEquippedProfileItems();  // argc: 2, index: 3, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BHasEquippedProfileItem();  // argc: 3, index: 4, ipc args: [uint64, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetProfileItemPropertyString();  // argc: 4, index: 5, ipc args: [uint64, bytes4, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetProfileItemPropertyUint();  // argc: 4, index: 6, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadCommunityItemAsset();  // argc: 4, index: 7, ipc args: [bytes8, string, string], ipc returns: [bytes8]
}