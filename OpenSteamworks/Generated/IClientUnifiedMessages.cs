//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUnifiedMessages
{
    // WARNING: Arguments are unknown!
    public ulong SendMethod(string method, byte[] msg, uint msgLen, ulong previousCallHandle = 0);  // argc: 5, index: 1, ipc args: [string, bytes4, bytes_length_from_mem, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public bool GetMethodResponseInfo(ulong methodID, out uint unk, out EResult eResult);  // argc: 4, index: 2, ipc args: [bytes8], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public bool GetMethodResponseData(ulong methodID, byte[] buf, uint bufLen, bool unk);  // argc: 5, index: 3, ipc args: [bytes8, bytes4, bytes1], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool ReleaseMethod(ulong methodID);  // argc: 2, index: 4, ipc args: [bytes8], ipc returns: [bytes1]
    public bool SendNotification(string method, byte[] msg, uint msgLen);  // argc: 3, index: 5, ipc args: [string, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
}