//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientGameNotifications
{
    // WARNING: Arguments are unknown!
    public unknown_ret EnumerateNotifications();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNotificationCount();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNotification();  // argc: 3, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes4124]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveSession();  // argc: 3, index: 4, ipc args: [bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateSession();  // argc: 3, index: 5, ipc args: [bytes4, bytes8], ipc returns: []
}