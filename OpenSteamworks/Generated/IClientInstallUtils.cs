//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientInstallUtils
{
    // WARNING: Arguments are unknown!
    public unknown_ret SetUniverse();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [unknown, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret AddShortcut();  // argc: 5, index: 2, ipc args: [string, string, string, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveShortcut();  // argc: 1, index: 3, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddUninstallEntry();  // argc: 7, index: 4, ipc args: [bytes4, string, string, string, string, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveUninstallEntry();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddToFirewall();  // argc: 2, index: 6, ipc args: [string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveFromFirewall();  // argc: 2, index: 7, ipc args: [string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RegisterSteamProtocolHandler();  // argc: 2, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddInstallScriptToWhiteList();  // argc: 2, index: 9, ipc args: [string, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RunInstallScript();  // argc: 3, index: 10, ipc args: [string, bytes4, bytes1], ipc returns: [bytes1]
    public unknown_ret GetInstallScriptExitCode();  // argc: 0, index: 11, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ConfigureNetworDeviceIPAddresses();  // argc: 7, index: 0, ipc args: [string, bytes1, bytes4, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes1]
}