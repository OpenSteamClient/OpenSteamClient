//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IRegistryInterface
{
    // WARNING: Arguments are unknown!
    public unknown_ret BGetValueStr();  // argc: 5, index: 1, ipc args: [bytes4, string, string, bytes4], ipc returns: [boolean, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetValueUint();  // argc: 4, index: 2, ipc args: [bytes4, string, string], ipc returns: [boolean, bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BGetValueBin();  // argc: 5, index: 3, ipc args: [bytes4, string, string, bytes4], ipc returns: [boolean, bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret BSetValueStr();  // argc: 4, index: 4, ipc args: [bytes4, string, string, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BSetValueUint();  // argc: 4, index: 5, ipc args: [bytes4, string, string, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BSetValueBin();  // argc: 5, index: 6, ipc args: [bytes4, string, string, bytes4, bytes_length_from_mem], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BDeleteValue();  // argc: 3, index: 7, ipc args: [bytes4, string, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BDeleteKey();  // argc: 2, index: 8, ipc args: [bytes4, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BKeyExists();  // argc: 2, index: 9, ipc args: [bytes4, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BGetSubKeys();  // argc: 4, index: 10, ipc args: [bytes4, string, bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BGetValues();  // argc: 5, index: 11, ipc args: [bytes4, string, bytes4, bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BEnumerateKey();  // argc: 5, index: 12, ipc args: [bytes4, string, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret BEnumerateValue();  // argc: 6, index: 13, ipc args: [bytes4, string, bytes4, bytes4], ipc returns: [bytes4, boolean, bytes4]
}