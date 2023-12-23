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
    public int GetSessionCount();  // argc: 0, index: 1
    public RemotePlaySessionID_t GetSessionID(int index);  // argc: 1, index: 2
    public CSteamID GetSessionSteamID(RemotePlaySessionID_t unSessionID, int unk);  // argc: 2, index: 3
    public string GetSessionClientName(RemotePlaySessionID_t unSessionID);  // argc: 1, index: 4
    public ESteamDeviceFormFactor GetSessionClientFormFactor(RemotePlaySessionID_t unSessionID);  // argc: 1, index: 5
    public bool BGetSessionClientResolution(RemotePlaySessionID_t unSessionID, out int pnResolutionX, out int pnResolutionY);  // argc: 3, index: 6
    public bool BStartRemotePlayTogether(bool showOverlay = true);  // argc: 1, index: 7
    public bool BSendRemotePlayTogetherInvite(CSteamID recipientFriend);  // argc: 2, index: 8
}