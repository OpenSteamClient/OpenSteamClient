//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

using HSharedConnection = System.UInt32;

public unsafe interface IClientSharedConnection
{
    public HSharedConnection AllocateSharedConnection();  // argc: 0, index: 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ReleaseSharedConnection(HSharedConnection connection);  // argc: 1, index: 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SendMessage(HSharedConnection connection, void *msg, size_t size);  // argc: 3, index: 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SendMessageAndAwaitResponse();  // argc: 3, index: 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RegisterEMsgHandler();  // argc: 2, index: 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RegisterServiceMethodHandler();  // argc: 2, index: 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BPopReceivedMessage();  // argc: 3, index: 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret InitiateConnection(HSharedConnection connection);  // argc: 1, index: 8
}