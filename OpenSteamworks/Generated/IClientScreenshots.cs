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
    public unknown_ret GetShortcutDisplayName();  // argc: 1, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret SetShortcutDisplayName();  // argc: 2, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret WriteScreenshot();  // argc: 5, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret AddScreenshotToLibrary();  // argc: 7, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerScreenshot();  // argc: 1, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret RequestScreenshotFromGame();  // argc: 1, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocation();  // argc: 3, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret TagUser();  // argc: 4, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret TagPublishedFile();  // argc: 4, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret ResolvePath();  // argc: 5, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret GetSizeOnDisk();  // argc: 2, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetSizeInCloud();  // argc: 2, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret IsPersisted();  // argc: 2, index: 13
    public unknown_ret GetNumGamesWithLocalScreenshots();  // argc: 0, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret GetGameWithLocalScreenshots();  // argc: 2, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalScreenshotCount();  // argc: 1, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalScreenshot();  // argc: 11, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalScreenshotByHandle();  // argc: 10, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocalScreenshotCaption();  // argc: 3, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocalScreenshotPrivacy();  // argc: 3, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret SetLocalScreenshotSpoiler();  // argc: 3, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLastScreenshot();  // argc: 2, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret StartBatch();  // argc: 1, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret AddToBatch();  // argc: 1, index: 24
    // WARNING: Arguments are unknown!
    public unknown_ret UploadBatch();  // argc: 1, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteBatch();  // argc: 1, index: 26
    public unknown_ret CancelBatch();  // argc: 0, index: 27
    public unknown_ret RecoverOldScreenshots();  // argc: 0, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedUserCount();  // argc: 2, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedUser();  // argc: 4, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocation();  // argc: 4, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedPublishedFileCount();  // argc: 2, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret GetTaggedPublishedFile();  // argc: 3, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret GetScreenshotVRType();  // argc: 2, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret BGetUserScreenshotDirectory();  // argc: 2, index: 35
}