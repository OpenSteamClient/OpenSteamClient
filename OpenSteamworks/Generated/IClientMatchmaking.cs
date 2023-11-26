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
    public unknown_ret GetFavoriteGameCount();  // argc: 0, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret GetFavoriteGame();  // argc: 7, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret AddFavoriteGame();  // argc: 6, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveFavoriteGame();  // argc: 5, index: 4
    public unknown_ret RequestLobbyList();  // argc: 0, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListStringFilter();  // argc: 3, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListNumericalFilter();  // argc: 3, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListNearValueFilter();  // argc: 2, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListFilterSlotsAvailable();  // argc: 1, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListDistanceFilter();  // argc: 1, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListResultCountFilter();  // argc: 1, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequestLobbyListCompatibleMembersFilter();  // argc: 2, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyByIndex();  // argc: 2, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret CreateLobby();  // argc: 2, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret JoinLobby();  // argc: 2, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret LeaveLobby();  // argc: 2, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret InviteUserToLobby();  // argc: 4, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumLobbyMembers();  // argc: 2, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyMemberByIndex();  // argc: 4, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyData();  // argc: 3, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyData();  // argc: 4, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyDataCount();  // argc: 2, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyDataByIndex();  // argc: 7, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteLobbyData();  // argc: 3, index: 24
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyMemberData();  // argc: 5, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyMemberData();  // argc: 4, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret SendLobbyChatMsg();  // argc: 4, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyChatEntry();  // argc: 7, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret RequestLobbyData();  // argc: 2, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyGameServer();  // argc: 6, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyGameServer();  // argc: 5, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyMemberLimit();  // argc: 3, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyMemberLimit();  // argc: 2, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyVoiceEnabled();  // argc: 3, index: 34
    public unknown_ret RequestFriendsLobbies();  // argc: 0, index: 35
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyType();  // argc: 3, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyJoinable();  // argc: 3, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret GetLobbyOwner();  // argc: 3, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret SetLobbyOwner();  // argc: 4, index: 39
    // WARNING: Arguments are unknown!
    public unknown_ret SetLinkedLobby();  // argc: 4, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret BeginGMSQuery();  // argc: 3, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret PollGMSQuery();  // argc: 2, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret GetGMSQueryResults();  // argc: 3, index: 43
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseGMSQuery();  // argc: 2, index: 44
    // WARNING: Arguments are unknown!
    public unknown_ret QueryServerByFakeIP();  // argc: 4, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret EnsureFavoriteGameAccountsUpdated();  // argc: 1, index: 46
}