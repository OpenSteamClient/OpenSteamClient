//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientGameServerPacketHandler
{
    // WARNING: Arguments are unknown!
    public unknown_ret HandleIncomingPacket();  // argc: 4, index: 1, ipc args: [bytes4, bytes_length_from_mem, bytes4, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNextOutgoingPacket();  // argc: 4, index: 2, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg, bytes4, bytes2]
}