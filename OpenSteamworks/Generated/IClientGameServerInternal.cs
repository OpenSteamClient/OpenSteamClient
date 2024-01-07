//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Enums;

using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientGameServerInternal
{
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_0_DONTUSE();  // argc: -1, index: 1, ipc args: [], ipc returns: []
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_1_DONTUSE();  // argc: -1, index: 2, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetSDRLogin([IPCIn] CUtlBuffer* buf);  // argc: 1, index: 3, ipc args: [unknown], ipc returns: []
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_3_DONTUSE();  // argc: -1, index: 4, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret InitGameServerSerialized();  // argc: 8, index: 5, ipc args: [bytes4, bytes2, bytes2, bytes4, bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret SetProduct(string product);  // argc: 1, index: 6, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameDescription(string desc);  // argc: 1, index: 7, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetModDir(string dir);  // argc: 1, index: 8, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetDedicatedServer(bool val);  // argc: 1, index: 9, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret LogOn(string unk);  // argc: 1, index: 10, ipc args: [string], ipc returns: []
    public unknown_ret LogOnAnonymous();  // argc: 0, index: 11, ipc args: [], ipc returns: []
    public unknown_ret LogOff();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public CSteamID GetSteamID();  // argc: 1, index: 0, ipc args: [], ipc returns: [uint64]
    public bool BLoggedOn();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public bool BSecure();  // argc: 0, index: 0, ipc args: [], ipc returns: [boolean]
    public unknown_ret WasRestartRequested();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetMaxPlayerCount(int maxPlayerCount);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetBotPlayerCount(int maxPlayerCount);  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetServerName(string name);  // argc: 1, index: 2, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetMapName(string name);  // argc: 1, index: 3, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPasswordProtected(bool isPasswordProtected);  // argc: 1, index: 4, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetSpectatorPort();  // argc: 1, index: 5, ipc args: [bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetSpectatorServerName();  // argc: 1, index: 6, ipc args: [string], ipc returns: []
    public unknown_ret ClearAllKeyValues();  // argc: 0, index: 7, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetKeyValue();  // argc: 2, index: 0, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameTags();  // argc: 1, index: 1, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameData();  // argc: 1, index: 2, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetRegion(string region);  // argc: 1, index: 3, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SendUserConnectAndAuthenticate();  // argc: 4, index: 4, ipc args: [bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes4, uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateUnauthenticatedUserConnection();  // argc: 1, index: 5, ipc args: [], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret SendUserDisconnect();  // argc: 2, index: 6, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BUpdateUserData();  // argc: 4, index: 7, ipc args: [uint64, string, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthSessionTicket();  // argc: 3, index: 8, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthSessionTicketV2();  // argc: 4, index: 9, ipc args: [bytes4, steamnetworkingidentity], ipc returns: [bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BeginAuthSession();  // argc: 4, index: 10, ipc args: [bytes4, bytes_length_from_mem, uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret EndAuthSession();  // argc: 2, index: 11, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CancelAuthTicket();  // argc: 1, index: 12, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsUserSubscribedAppInTicket();  // argc: 3, index: 13, ipc args: [uint64, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestUserGroupStatus();  // argc: 4, index: 14, ipc args: [uint64, uint64], ipc returns: [bytes1]
    public unknown_ret GetGameplayStats();  // argc: 0, index: 15, ipc args: [], ipc returns: []
    public unknown_ret GetServerReputation();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPublicIP();  // argc: 1, index: 0, ipc args: [], ipc returns: [bytes20]
    // WARNING: Arguments are unknown!
    public unknown_ret EnableHeartbeats();  // argc: 1, index: 1, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetHeartbeatInterval();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    public unknown_ret ForceHeartbeat();  // argc: 0, index: 3, ipc args: [], ipc returns: []
    public ELogonState GetLogonState();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes4]
    public bool BConnected();  // argc: 0, index: 0, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!§
    public unknown_ret RaiseConnectionPriority();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ResetConnectionPriority();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetCellID(uint cellid);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TrackSteamUsageEvent();  // argc: 3, index: 3, ipc args: [bytes4, bytes4, bytes_length_from_mem], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetCountOfSimultaneousGuestUsersPerSteamAccount();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateConnectedUsers();  // argc: 2, index: 5, ipc args: [bytes4], ipc returns: [bytes1, bytes40]
    // WARNING: Arguments are unknown!
    public unknown_ret AssociateWithClan();  // argc: 2, index: 6, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ComputeNewPlayerCompatibility();  // argc: 2, index: 7, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret _BGetUserAchievementStatus();  // argc: 3, index: 8, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSSetSpawnCount();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret _GSGetSteam2GetEncryptionKeyToSendToNewClient();  // argc: 3, index: 10, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSSendSteam2UserConnect();  // argc: 7, index: 11, ipc args: [bytes4, bytes4, bytes_length_from_mem, bytes4, bytes2, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSSendSteam3UserConnect();  // argc: 5, index: 12, ipc args: [uint64, bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSSendUserConnect();  // argc: 5, index: 13, ipc args: [bytes4, bytes4, bytes2, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSRemoveUserConnect();  // argc: 1, index: 14, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSUpdateStatus();  // argc: 6, index: 15, ipc args: [bytes4, bytes4, bytes4, string, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSCreateUnauthenticatedUser();  // argc: 1, index: 16, ipc args: [], ipc returns: [bytes1, uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret _GSSetServerType();  // argc: 9, index: 17, ipc args: [bytes4, bytes4, bytes4, bytes2, bytes2, bytes2, string, string, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret _SetBasicServerData();  // argc: 7, index: 18, ipc args: [bytes2, bytes1, string, string, bytes2, bytes1, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret _GSSendUserDisconnect();  // argc: 3, index: 19, ipc args: [uint64, bytes4], ipc returns: [bytes1]
}