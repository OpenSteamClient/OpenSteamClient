//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;

namespace OpenSteamworks.Generated;

/// <summary>
/// The main interface for family sharing.
/// </summary>
public unsafe interface IClientDeviceAuth
{
    // WARNING: Arguments are unknown!
    public SteamAPICall_t AuthorizeLocalDevice(string unk, uint unk1);  // argc: 2, index: 1
    // WARNING: Arguments are unknown!
    public SteamAPICall_t DeauthorizeDevice(UInt64 wtf);  // argc: 2, index: 2
    public SteamAPICall_t RequestAuthorizationInfos();  // argc: 0, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizations();  // argc: 3, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizationInfo();  // argc: 13, index: 5
    // WARNING: Arguments are unknown!
    public int GetAuthorizedBorrowers(uint[] owners, int ownersMax);  // argc: 2, index: 6
    // WARNING: Arguments are unknown!
    public int GetLocalUsers(uint[] owners, int ownersMax);  // argc: 2, index: 7
    /// <summary>
    /// Gets borrower info for an accountid.
    /// </summary>
    /// <param name="accountid"></param>
    /// <param name="username"></param>
    /// <param name="usernameMax"></param>
    /// <param name="isSharingLibrary"></param>
    /// <returns>Always return true.</returns>
    public bool GetBorrowerInfo(uint accountid, StringBuilder username, int usernameMax, out bool isSharingLibrary);  // argc: 4, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateAuthorizedBorrowers(void* unk, uint accountid, bool authorized);  // argc: 3, index: 9
    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountid">accountid of the shared library to test</param>
    /// <returns>0 if not locked, accountid of current locker otherwise</returns>
    public uint GetSharedLibraryLockedBy(uint accountid);  // argc: 1, index: 10
    public int GetSharedLibraryOwners(uint[] owners, int ownersMax);  // argc: 2, index: 11
}