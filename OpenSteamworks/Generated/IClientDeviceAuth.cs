//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;
using OpenSteamworks.Callbacks.Structs;

namespace OpenSteamworks.Generated;

/// <summary>
/// The main interface for the old family sharing system.
/// </summary>
public unsafe interface IClientDeviceAuth
{
    // WARNING: Arguments are unknown!
    public SteamAPICall_t AuthorizeLocalDevice(string description, uint accountID);  // argc: 2, index: 1, ipc args: [string, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t DeauthorizeDevice(UInt64 tokenID);  // argc: 2, index: 2, ipc args: [bytes8], ipc returns: [bytes8]
    public SteamAPICall_t RequestAuthorizationInfos();  // argc: 0, index: 3, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizations(UInt64[] authorizations, int authorizationsMax, bool unk);  // argc: 3, index: 4, ipc args: [bytes4, bytes1], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public bool GetDeviceAuthorizationInfo(UInt64 tokenID, out UInt32 userID, out UInt32 unk2, out RTime32 authorizeTime, out bool unk4, StringBuilder usernameAuthorizer, int usernameAuthorizerMax, StringBuilder unk6, int unk6Max, StringBuilder description, int descriptionMax, out UInt32 unk8);  // argc: 13, index: 5, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes4, bytes1, bytes_length_from_mem, bytes_length_from_mem, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public int GetAuthorizedBorrowers(uint[] owners, int ownersMax);  // argc: 2, index: 6, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public int GetLocalUsers(uint[] owners, int ownersMax);  // argc: 2, index: 7, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    /// <summary>
    /// Gets borrower info for an accountid.
    /// </summary>
    /// <param name="accountid"></param>
    /// <param name="username"></param>
    /// <param name="usernameMax"></param>
    /// <param name="isSharingLibrary"></param>
    /// <returns>Always returns true.</returns>
    public bool GetBorrowerInfo(uint accountid, StringBuilder username, int usernameMax, out bool isSharingLibrary);  // argc: 4, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes1]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t UpdateAuthorizedBorrowers(uint[] accountids, uint accountidsLen, bool authorized);  // argc: 3, index: 9, ipc args: [bytes4, bytes_length_from_reg, bytes1], ipc returns: [bytes8]
    public int GetSharedLibraryOwners(uint[] owners, int ownersMax);  // argc: 2, index: 10, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
}