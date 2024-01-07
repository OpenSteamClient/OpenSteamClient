//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientGameStats
{
    // WARNING: Arguments are unknown!
    public unknown_ret GetNewSession();  // argc: 5, index: 1, ipc args: [bytes1, bytes8, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret EndSession();  // argc: 4, index: 2, ipc args: [bytes8, bytes4, bytes2], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret AddSessionAttributeInt();  // argc: 4, index: 3, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddSessionAttributeString();  // argc: 4, index: 4, ipc args: [bytes8, string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddSessionAttributeFloat();  // argc: 4, index: 5, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddNewRow();  // argc: 4, index: 6, ipc args: [bytes8, string], ipc returns: [bytes4, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CommitRow();  // argc: 2, index: 7, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret CommitOutstandingRows();  // argc: 2, index: 8, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRowAttributeInt();  // argc: 4, index: 9, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRowAttributeString();  // argc: 4, index: 10, ipc args: [bytes8, string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRowAttributeFloat();  // argc: 4, index: 11, ipc args: [bytes8, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddSessionAttributeInt64();  // argc: 5, index: 12, ipc args: [bytes8, string, bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRowAttributeInt64();  // argc: 5, index: 13, ipc args: [bytes8, string, bytes8], ipc returns: [bytes4]
}