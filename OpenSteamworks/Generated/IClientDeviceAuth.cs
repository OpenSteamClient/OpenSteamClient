//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

/// <summary>
/// The main interface for family sharing.
/// </summary>
public unsafe interface IClientDeviceAuth
{
    // WARNING: Arguments are unknown!
    public unknown_ret AuthorizeLocalDevice();  // argc: 2, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret DeauthorizeDevice();  // argc: 2, index: 2
    public unknown_ret RequestAuthorizationInfos();  // argc: 0, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizations();  // argc: 3, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizationInfo();  // argc: 13, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthorizedBorrowers();  // argc: 2, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalUsers();  // argc: 2, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret GetBorrowerInfo();  // argc: 4, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateAuthorizedBorrowers();  // argc: 3, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret GetSharedLibraryLockedBy();  // argc: 1, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret GetSharedLibraryOwners();  // argc: 2, index: 11
}