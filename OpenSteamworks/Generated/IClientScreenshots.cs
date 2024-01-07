//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientScreenshots
{
    // WARNING: Arguments are unknown!
    public unknown_ret GetShortcutDisplayName();  // argc: 1, index: 1, ipc args: [bytes8], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutDisplayName();  // argc: 2, index: 2, ipc args: [bytes8, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WriteScreenshot();  // argc: 5, index: 3, ipc args: [bytes8, bytes4, bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddScreenshotToLibrary();  // argc: 7, index: 4, ipc args: [bytes8, bytes4, string, string, string, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerScreenshot();  // argc: 1, index: 5, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RequestScreenshotFromGame();  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocation();  // argc: 3, index: 7, ipc args: [bytes8, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TagUser();  // argc: 4, index: 8, ipc args: [bytes8, bytes4, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TagPublishedFile();  // argc: 4, index: 9, ipc args: [bytes8, bytes4, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ResolvePath();  // argc: 5, index: 10, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSizeOnDisk();  // argc: 2, index: 11, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSizeInCloud();  // argc: 2, index: 12, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret IsPersisted();  // argc: 2, index: 13, ipc args: [bytes8, bytes4], ipc returns: [boolean]
    public unknown_ret GetNumGamesWithLocalScreenshots();  // argc: 0, index: 14, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGameWithLocalScreenshots();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalScreenshotCount();  // argc: 1, index: 1, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalScreenshot();  // argc: 11, index: 2, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes4, bytes4, bytes4, bytes8, bytes_length_from_mem, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalScreenshotByHandle();  // argc: 10, index: 3, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes4, bytes4, bytes8, bytes1, bytes_length_from_mem, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocalScreenshotCaption();  // argc: 3, index: 4, ipc args: [bytes8, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocalScreenshotPrivacy();  // argc: 3, index: 5, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocalScreenshotSpoiler();  // argc: 3, index: 6, ipc args: [bytes8, bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLastScreenshot();  // argc: 2, index: 7, ipc args: [], ipc returns: [bytes1, bytes8, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret StartBatch();  // argc: 1, index: 8, ipc args: [bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddToBatch();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UploadBatch();  // argc: 1, index: 10, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteBatch();  // argc: 1, index: 11, ipc args: [bytes1], ipc returns: [bytes8]
    public unknown_ret CancelBatch();  // argc: 0, index: 12, ipc args: [], ipc returns: [bytes1]
    public unknown_ret RecoverOldScreenshots();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedUserCount();  // argc: 2, index: 0, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedUser();  // argc: 4, index: 1, ipc args: [bytes8, bytes4, bytes4], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocation();  // argc: 4, index: 2, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedPublishedFileCount();  // argc: 2, index: 3, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedPublishedFile();  // argc: 3, index: 4, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetScreenshotVRType();  // argc: 2, index: 5, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetUserScreenshotDirectory();  // argc: 2, index: 6, ipc args: [bytes4], ipc returns: [boolean, bytes_length_from_mem]
}