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
    public SteamAPICall_t AuthorizeLocalDevice(string unk, uint unk1);  // argc: 2, index: 1, ipc args: [string, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t DeauthorizeDevice(UInt64 wtf);  // argc: 2, index: 2, ipc args: [bytes8], ipc returns: [bytes8]
    public SteamAPICall_t RequestAuthorizationInfos();  // argc: 0, index: 3, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizations(UInt64[] authorizations, int authorizationsMax, bool unk);  // argc: 3, index: 0, ipc args: [bytes4, bytes1], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDeviceAuthorizationInfo(UInt64 unk, ref UInt32 unk1, ref UInt32 unk2, ref UInt32 unk3, ref bool unk4, StringBuilder unk5, int unk5Max, StringBuilder unk6, int unk6Max, StringBuilder unk7, int unk7Max, ref UInt32 unk8);  // argc: 13, index: 1, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes4, bytes1, bytes_length_from_mem, bytes_length_from_mem, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public int GetAuthorizedBorrowers(uint[] owners, int ownersMax);  // argc: 2, index: 2, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public int GetLocalUsers(uint[] owners, int ownersMax);  // argc: 2, index: 3, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    /// <summary>
    /// Gets borrower info for an accountid.
    /// </summary>
    /// <param name="accountid"></param>
    /// <param name="username"></param>
    /// <param name="usernameMax"></param>
    /// <param name="isSharingLibrary"></param>
    /// <returns>Always returns true.</returns>
    public bool GetBorrowerInfo(uint accountid, StringBuilder username, int usernameMax, out bool isSharingLibrary);  // argc: 4, index: 4, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateAuthorizedBorrowers(uint[] accountids, uint accountid, bool authorized);  // argc: 3, index: 5, ipc args: [bytes4, bytes_length_from_reg, bytes1], ipc returns: [bytes8]
    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountid">accountid of the shared library to test</param>
    /// <returns>0 if not locked, accountid of current locker otherwise</returns>
    public uint GetSharedLibraryLockedBy(uint accountid);  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [bytes4]
    public int GetSharedLibraryOwners(uint[] owners, int ownersMax);  // argc: 2, index: 7, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
}