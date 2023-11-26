//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientSystemManager
{
    // WARNING: Arguments are unknown!
    public unknown_ret GetSettings();  // argc: 1, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateSettings();  // argc: 1, index: 2
    public unknown_ret ShutdownSystem();  // argc: 0, index: 3
    public unknown_ret SuspendSystem();  // argc: 0, index: 4
    public unknown_ret RestartSystem();  // argc: 0, index: 5
    public unknown_ret FactoryReset();  // argc: 0, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret RebootToFactoryTestImage();  // argc: 1, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret GetDisplayBrightness();  // argc: 1, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret SetDisplayBrightness();  // argc: 1, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret FormatRemovableStorage();  // argc: 1, index: 10
    public unknown_ret GetOSBranchList();  // argc: 0, index: 11
    public unknown_ret GetCurrentOSBranch();  // argc: 0, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret SelectOSBranch();  // argc: 1, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret GetUpdateState();  // argc: 1, index: 14
    public unknown_ret CheckForUpdate();  // argc: 0, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret ApplyUpdate();  // argc: 1, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret SetBackgroundUpdateCheckInterval();  // argc: 1, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret ClearAudioDefaults();  // argc: 1, index: 18
    public unknown_ret RunDeckMicEnableHack();  // argc: 0, index: 19
    public unknown_ret RunDeckEchoCancellationHack();  // argc: 0, index: 20
}