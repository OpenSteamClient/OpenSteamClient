//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientRemotePlay
{
    public int GetSessionCount();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    public RemotePlaySessionID_t GetSessionID(int index);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    public CSteamID GetSessionSteamID(RemotePlaySessionID_t unSessionID, int unk);  // argc: 2, index: 1, ipc args: [bytes4], ipc returns: [uint64]
    public string GetSessionClientName(RemotePlaySessionID_t unSessionID);  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [string]
    public ESteamDeviceFormFactor GetSessionClientFormFactor(RemotePlaySessionID_t unSessionID);  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes4]
    public bool BGetSessionClientResolution(RemotePlaySessionID_t unSessionID, out int pnResolutionX, out int pnResolutionY);  // argc: 3, index: 4, ipc args: [bytes4], ipc returns: [boolean, bytes4, bytes4]
    public bool BStartRemotePlayTogether(bool showOverlay = true);  // argc: 1, index: 5, ipc args: [bytes1], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BSendRemotePlayTogetherInvite(CSteamID recipientFriend);  // argc: 2, index: 6, ipc args: [uint64], ipc returns: [boolean]
}