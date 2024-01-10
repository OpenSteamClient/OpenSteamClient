//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientGameSearch
{
    // WARNING: Arguments are unknown!
    public unknown_ret AddGameSearchParams();  // argc: 2, index: 1, ipc args: [string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SearchForGameWithLobby();  // argc: 4, index: 2, ipc args: [uint64, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SearchForGameSolo();  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    public unknown_ret AcceptGame();  // argc: 0, index: 4, ipc args: [], ipc returns: [bytes4]
    public unknown_ret DeclineGame();  // argc: 0, index: 5, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RetrieveConnectionDetails();  // argc: 4, index: 6, ipc args: [uint64, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    public unknown_ret EndGameSearch();  // argc: 0, index: 7, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameHostParams();  // argc: 2, index: 8, ipc args: [string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetConnectionDetails();  // argc: 2, index: 9, ipc args: [string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestPlayersForGame();  // argc: 3, index: 10, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret HostConfirmGameStart();  // argc: 2, index: 11, ipc args: [bytes8], ipc returns: [bytes4]
    public unknown_ret CancelRequestPlayersForGame();  // argc: 0, index: 12, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SubmitPlayerResult();  // argc: 5, index: 13, ipc args: [bytes8, uint64, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret EndGame();  // argc: 2, index: 14, ipc args: [bytes8], ipc returns: [bytes4]
}