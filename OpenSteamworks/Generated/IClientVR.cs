//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Generated;

public unsafe interface IClientVR
{
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetCurrentHmd();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes4]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetCompositor();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes4]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetOverlay();  // argc: 0, index: 3, ipc args: [], ipc returns: [bytes4]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetChaperone();  // argc: 0, index: 4, ipc args: [], ipc returns: [bytes4]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetSettings();  // argc: 0, index: 5, ipc args: [], ipc returns: [bytes4]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetProperties();  // argc: 0, index: 6, ipc args: [], ipc returns: [bytes4]
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetPaths();  // argc: 0, index: 7, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetOverlayHandle();  // argc: 0, index: 8, ipc args: [], ipc returns: [bytes8]
    public unknown_ret IsHmdPresent();  // argc: 0, index: 9, ipc args: [], ipc returns: [boolean]
    public unknown_ret UpdateHmdStatus();  // argc: 0, index: 10, ipc args: [], ipc returns: []
    public unknown_ret IsVRModeActive();  // argc: 0, index: 11, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret InitVR(bool unk1, ref uint clientVRError, ref uint vrInitError);  // argc: 3, index: 12, ipc args: [bytes1], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret StartSteamVR();  // argc: 1, index: 13, ipc args: [string], ipc returns: [bytes1]
    public unknown_ret CleanupVR();  // argc: 0, index: 14, ipc args: [], ipc returns: []
    public unknown_ret QuitAllVR();  // argc: 0, index: 15, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret QuitApplication();  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetStringForHmdError();  // argc: 1, index: 17, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret LaunchApplication();  // argc: 1, index: 18, ipc args: [string], ipc returns: [bytes1]
    public unknown_ret GetSteamVRAppId();  // argc: 0, index: 19, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetSteamVRPid();  // argc: 0, index: 20, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetWebSecret();  // argc: 0, index: 21, ipc args: [], ipc returns: [bytes8]
    public unknown_ret BSteamCanMakeVROverlays();  // argc: 0, index: 22, ipc args: [], ipc returns: [boolean]
    public unknown_ret BServeVRGamepadUIOverlay();  // argc: 0, index: 23, ipc args: [], ipc returns: [boolean]
    public unknown_ret BServeTenfootOverlay();  // argc: 0, index: 24, ipc args: [], ipc returns: [boolean]
    public unknown_ret BSuppressDesktopBPM();  // argc: 0, index: 25, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetVRConnectionParams();  // argc: 1, index: 26, ipc args: [string], ipc returns: []
    public unknown_ret GetVRConnectionParams();  // argc: 0, index: 27, ipc args: [], ipc returns: [string]
    public unknown_ret BVRDeviceSeenRecently();  // argc: 0, index: 28, ipc args: [], ipc returns: [boolean]
}