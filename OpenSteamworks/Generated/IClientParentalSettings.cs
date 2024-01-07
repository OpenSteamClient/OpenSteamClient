//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;

namespace OpenSteamworks.Generated;

public unsafe interface IClientParentalSettings
{
    public bool BIsParentalLockEnabled();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    public bool BIsParentalLockLocked();  // argc: 0, index: 0, ipc args: [], ipc returns: [boolean]
    public bool BIsAppBlocked(AppId_t appid);  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [boolean]
    public bool BIsAppInBlockList(AppId_t appid);  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BIsFeatureBlocked();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BIsFeatureInBlockList();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BGetSerializedParentalSettings([IPCOut] CUtlBuffer* data);  // argc: 1, index: 4, ipc args: [], ipc returns: [boolean, unknown]
    // WARNING: Arguments are unknown!
    public bool BGetRecoveryEmail();  // argc: 2, index: 5, ipc args: [bytes4], ipc returns: [boolean, bytes_length_from_mem]
    public bool BIsLockFromSiteLicense();  // argc: 0, index: 6, ipc args: [], ipc returns: [boolean]
}