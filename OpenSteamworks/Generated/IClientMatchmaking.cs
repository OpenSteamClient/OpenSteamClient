//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientMatchmaking
{
    public unknown_ret GetFavoriteGameCount();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFavoriteGame();  // argc: 7, index: 0, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes2, bytes2, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddFavoriteGame();  // argc: 6, index: 1, ipc args: [bytes4, bytes4, bytes2, bytes2, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveFavoriteGame();  // argc: 5, index: 2, ipc args: [bytes4, bytes4, bytes2, bytes2, bytes4], ipc returns: [bytes1]
    public unknown_ret RequestLobbyList();  // argc: 0, index: 3, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListStringFilter();  // argc: 3, index: 0, ipc args: [string, string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListNumericalFilter();  // argc: 3, index: 1, ipc args: [string, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListNearValueFilter();  // argc: 2, index: 2, ipc args: [string, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListFilterSlotsAvailable();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListDistanceFilter();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListResultCountFilter();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListCompatibleMembersFilter();  // argc: 2, index: 6, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyByIndex();  // argc: 2, index: 7, ipc args: [bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateLobby();  // argc: 2, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret JoinLobby();  // argc: 2, index: 9, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret LeaveLobby();  // argc: 2, index: 10, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret InviteUserToLobby();  // argc: 4, index: 11, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumLobbyMembers();  // argc: 2, index: 12, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyMemberByIndex();  // argc: 4, index: 13, ipc args: [uint64, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyData();  // argc: 3, index: 14, ipc args: [uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyData();  // argc: 4, index: 15, ipc args: [uint64, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyDataCount();  // argc: 2, index: 16, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyDataByIndex();  // argc: 7, index: 17, ipc args: [uint64, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteLobbyData();  // argc: 3, index: 18, ipc args: [uint64, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyMemberData();  // argc: 5, index: 19, ipc args: [uint64, uint64, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyMemberData();  // argc: 4, index: 20, ipc args: [uint64, string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SendLobbyChatMsg();  // argc: 4, index: 21, ipc args: [uint64, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyChatEntry();  // argc: 7, index: 22, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg, uint64, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestLobbyData();  // argc: 2, index: 23, ipc args: [uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyGameServer();  // argc: 6, index: 24, ipc args: [uint64, bytes4, bytes2, uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyGameServer();  // argc: 5, index: 25, ipc args: [uint64], ipc returns: [bytes1, bytes4, bytes2, uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyMemberLimit();  // argc: 3, index: 26, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyMemberLimit();  // argc: 2, index: 27, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyVoiceEnabled();  // argc: 3, index: 28, ipc args: [uint64, bytes1], ipc returns: []
    public unknown_ret RequestFriendsLobbies();  // argc: 0, index: 29, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyType();  // argc: 3, index: 0, ipc args: [uint64, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyJoinable();  // argc: 3, index: 1, ipc args: [uint64, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyOwner();  // argc: 3, index: 2, ipc args: [uint64], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyOwner();  // argc: 4, index: 3, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLinkedLobby();  // argc: 4, index: 4, ipc args: [uint64, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BeginGMSQuery();  // argc: 3, index: 5, ipc args: [bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret PollGMSQuery();  // argc: 2, index: 6, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGMSQueryResults();  // argc: 3, index: 7, ipc args: [bytes8], ipc returns: [bytes1, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseGMSQuery();  // argc: 2, index: 8, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret QueryServerByFakeIP();  // argc: 4, index: 9, ipc args: [bytes4, bytes2, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EnsureFavoriteGameAccountsUpdated();  // argc: 1, index: 10, ipc args: [bytes1], ipc returns: [bytes8]
}