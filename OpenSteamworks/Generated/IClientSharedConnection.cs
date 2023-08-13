//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Generated;

using HSharedConnection = System.UInt32;

public unsafe interface IClientSharedConnection
{
    public HSharedConnection AllocateSharedConnection();  // argc: 0, index: 1
    public void ReleaseSharedConnection(HSharedConnection connection);  // argc: 1, index: 2
    public int SendMessage(HSharedConnection connection, void *msg, size_t size);  // argc: 3, index: 3
    public int SendMessageAndAwaitResponse(HSharedConnection connection, void *msg, size_t size);  // argc: 3, index: 4
    public void RegisterEMsgHandler(HSharedConnection hConn, UInt32 eMsg);  // argc: 2, index: 5
    public void RegisterServiceMethodHandler(HSharedConnection hConn, string method);  // argc: 2, index: 6
    public bool BPopReceivedMessage(HSharedConnection hConn, CUtlBuffer* bufOut, ref UInt32 hCall);  // argc: 3, index: 7
    public void InitiateConnection(HSharedConnection connection);  // argc: 1, index: 8
}