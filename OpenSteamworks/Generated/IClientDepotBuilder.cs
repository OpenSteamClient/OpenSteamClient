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
    public unknown_ret BGetDepotBuildStatus();  // argc: 5, index: 1, ipc args: [bytes8], ipc returns: [boolean, bytes4, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret VerifyChunkStore();  // argc: 3, index: 2, ipc args: [bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public SteamAPICall_t DownloadDepot(AppId_t appid, DepotId_t depotId, uint workshopItemID, uint unk2, ulong targetManifestID, ulong deltaManifestID, string? targetInstallPath);  // argc: -1, index: 3, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret DownloadChunk();  // argc: 3, index: 4, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret StartDepotBuild();  // argc: 4, index: 5, ipc args: [bytes4, bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CommitAppBuild();  // argc: 6, index: 6, ipc args: [bytes4, bytes4, bytes_length_from_reg, bytes_length_from_reg, string, string], ipc returns: [bytes8]
}