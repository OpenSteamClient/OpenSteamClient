//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientDepotBuilder
{
    // WARNING: Arguments are unknown!
    public unknown_ret BGetDepotBuildStatus();  // argc: 5, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret VerifyChunkStore();  // argc: 3, index: 2
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_2_DONTUSE();  // argc: -1, index: 3
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret DownloadChunk();  // argc: 3, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret StartDepotBuild();  // argc: 4, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret CommitAppBuild();  // argc: 6, index: 6
}