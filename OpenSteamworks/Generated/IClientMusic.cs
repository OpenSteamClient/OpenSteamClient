//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientMusic
{
    public bool BIsEnabled();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret Enable();  // argc: 1, index: 2, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EnableCrawlLogging();  // argc: 1, index: 3, ipc args: [bytes1], ipc returns: []
    public unknown_ret BIsPlaying();  // argc: 0, index: 4, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetQueueCount();  // argc: 0, index: 5, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetCurrentQueueEntry();  // argc: 0, index: 6, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueueEntryURI();  // argc: 3, index: 7, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueueEntryData();  // argc: 10, index: 8, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes_length_from_mem, bytes_length_from_mem, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueueEntryOrigin();  // argc: 3, index: 9, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4]
    public unknown_ret EmptyQueue();  // argc: 0, index: 10, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveQueueEntry();  // argc: 2, index: 11, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddArtistToQueue();  // argc: 3, index: 12, ipc args: [string, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTracksOfAlbumIDToQueue();  // argc: 7, index: 13, ipc args: [bytes4, bytes4, bytes1, string, bytes1, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddAllTracksOfAlbumIDToQueue();  // argc: 5, index: 14, ipc args: [bytes4, bytes4, bytes1, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddTracksOfPlaylistIDToQueue();  // argc: 6, index: 15, ipc args: [bytes4, bytes4, bytes1, bytes1, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetSuppressAutoTrackAdvance();  // argc: 1, index: 16, ipc args: [bytes1], ipc returns: []
    public unknown_ret GetPlaybackStatus();  // argc: 0, index: 17, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlayingRepeatStatus();  // argc: 1, index: 18, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetPlayingRepeatStatus();  // argc: 0, index: 19, ipc args: [], ipc returns: [bytes4]
    public unknown_ret TogglePlayingRepeatStatus();  // argc: 0, index: 20, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlayingShuffled();  // argc: 1, index: 21, ipc args: [bytes1], ipc returns: []
    public unknown_ret IsPlayingShuffled();  // argc: 0, index: 22, ipc args: [], ipc returns: [boolean]
    public unknown_ret Play();  // argc: 0, index: 23, ipc args: [], ipc returns: []
    public unknown_ret Pause();  // argc: 0, index: 24, ipc args: [], ipc returns: []
    public unknown_ret PlayPrevious();  // argc: 0, index: 25, ipc args: [], ipc returns: []
    public unknown_ret PlayNext();  // argc: 0, index: 26, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret PlayEntry();  // argc: 1, index: 27, ipc args: [bytes4], ipc returns: []
    public unknown_ret TogglePlayPause();  // argc: 0, index: 28, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVolume();  // argc: 1, index: 29, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetVolume();  // argc: 0, index: 30, ipc args: [], ipc returns: [bytes4]
    public unknown_ret ToggleMuteVolume();  // argc: 0, index: 31, ipc args: [], ipc returns: []
    public unknown_ret IncreaseVolume();  // argc: 0, index: 32, ipc args: [], ipc returns: []
    public unknown_ret DecreaseVolume();  // argc: 0, index: 33, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlaybackPosition();  // argc: 1, index: 34, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetPlaybackPosition();  // argc: 0, index: 35, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetPlaybackDuration();  // argc: 0, index: 36, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LocalLibraryCrawlTrack();  // argc: 1, index: 37, ipc args: [string], ipc returns: []
    public unknown_ret ResetLocalLibrary();  // argc: 0, index: 38, ipc args: [], ipc returns: []
    public unknown_ret GetStatusLocalLibrary();  // argc: 0, index: 39, ipc args: [], ipc returns: [bytes4]
    public unknown_ret SaveLocalLibraryDirectorySettings();  // argc: 0, index: 40, ipc args: [], ipc returns: []
    public unknown_ret GetLocalLibraryDirectoryEntryCount();  // argc: 0, index: 41, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryDirectoryEntry();  // argc: 3, index: 42, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret AddLocalLibraryDirectoryEntry();  // argc: 1, index: 43, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ResetLocalLibraryDirectories();  // argc: 1, index: 44, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetDefaultLocalLibraryDirectory();  // argc: 2, index: 45, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public unknown_ret LocalLibraryStopCrawling();  // argc: 0, index: 46, ipc args: [], ipc returns: []
    public unknown_ret UpdateLocalLibraryDirectoriesToCrawl();  // argc: 0, index: 47, ipc args: [], ipc returns: []
    public unknown_ret BLocalLibraryIsCrawling();  // argc: 0, index: 48, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetLocalLibraryTrackCount();  // argc: 0, index: 49, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetLocalLibraryAlbumCount();  // argc: 0, index: 50, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumID();  // argc: 2, index: 51, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumIDEntry();  // argc: 8, index: 52, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes_length_from_mem, bytes4, bytes4, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumIDTrackEntry();  // argc: 9, index: 53, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4, bytes_length_from_mem, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumDirectoryForAlbumID();  // argc: 3, index: 54, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumSortNameForAlbumID();  // argc: 3, index: 55, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryArtistSortNameForAlbumID();  // argc: 3, index: 56, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryTrackCountForAlbumID();  // argc: 1, index: 57, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumIDTrackKey();  // argc: 4, index: 58, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumIDForTrackKey();  // argc: 1, index: 59, ipc args: [string], ipc returns: [bytes4]
    public unknown_ret GetLocalLibraryArtistCount();  // argc: 0, index: 60, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryArtistName();  // argc: 5, index: 61, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumCountForArtistName();  // argc: 1, index: 62, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryTrackAndAlbumCountOfArtistName();  // argc: 3, index: 63, ipc args: [string], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryAlbumIDForArtist();  // argc: 2, index: 64, ipc args: [string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryRepresentativeAlbumIDForArtist();  // argc: 2, index: 65, ipc args: [string], ipc returns: [bytes4, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryTrackEntry();  // argc: 6, index: 66, ipc args: [string, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret BRequestAllArtistListData();  // argc: 1, index: 67, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BRequestAllAlbumListData();  // argc: 1, index: 68, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BRequestAllTracksListDataForAlbumID();  // argc: 2, index: 69, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    public unknown_ret GetPlaylistCount();  // argc: 0, index: 70, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlaylistID();  // argc: 1, index: 71, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPositionForPlaylistID();  // argc: 1, index: 72, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlaylistIDForPosition();  // argc: 1, index: 73, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BRequestAllPlayListData();  // argc: 1, index: 74, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNextPlaylistName();  // argc: 3, index: 75, ipc args: [string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret InsertPlaylistWithNameAtPosition();  // argc: 2, index: 76, ipc args: [string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret MovePlaylistFromPositionToPosition();  // argc: 2, index: 77, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret DeletePlaylistWithID();  // argc: 1, index: 78, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RenamePlaylistWithID();  // argc: 2, index: 79, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRandomTracksToPlaylistID();  // argc: 2, index: 80, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlaylistIDData();  // argc: 8, index: 81, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4, bytes4, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlaylistIDTrackCount();  // argc: 1, index: 82, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BRequestAllTracksListDataForPlaylistID();  // argc: 2, index: 83, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPlaylistIDTrackKey();  // argc: 5, index: 84, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetPositionForPlaylistItemID();  // argc: 1, index: 85, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddTrackKeyToPlaylistID();  // argc: 2, index: 86, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddAlbumIDToPlaylistID();  // argc: 3, index: 87, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddArtistNameToPlaylistID();  // argc: 2, index: 88, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddPlaylistIDToPlaylistID();  // argc: 2, index: 89, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemovePlaylistPositionFromPlaylistID();  // argc: 2, index: 90, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveAllTracksFromPlaylistID();  // argc: 2, index: 91, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret MoveTrackFromPositionToPositonInPlaylistID();  // argc: 3, index: 92, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AppendQueueToPlaylistID();  // argc: 2, index: 93, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalLibraryRepresentativeAlbumIDForPlaylist();  // argc: 2, index: 94, ipc args: [bytes4], ipc returns: [bytes4, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret MarkTrackKeyAsPlayed();  // argc: 1, index: 95, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMostRecentlyPlayedAlbums();  // argc: 2, index: 96, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMostRecentlyAddedAlbums();  // argc: 2, index: 97, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateRemotePlayerWithID();  // argc: 1, index: 98, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret GetActiveRemotePlayerID();  // argc: 0, index: 99, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetRemotePlayerCount();  // argc: 0, index: 100, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetRemotePlayerIDAndName();  // argc: 4, index: 101, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCurrentEntryTextForRemotePlayerWithID();  // argc: 3, index: 102, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret RegisterSteamMusicRemote();  // argc: 1, index: 103, ipc args: [string], ipc returns: [bytes1]
    public unknown_ret DeregisterSteamMusicRemote();  // argc: 0, index: 104, ipc args: [], ipc returns: [bytes1]
    public unknown_ret BIsCurrentMusicRemote();  // argc: 0, index: 105, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BActivationSuccess();  // argc: 1, index: 106, ipc args: [bytes1], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetDisplayName();  // argc: 1, index: 107, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetPNGIcon_64x64();  // argc: 2, index: 108, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EnablePlayPrevious();  // argc: 1, index: 109, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EnablePlayNext();  // argc: 1, index: 110, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EnableShuffled();  // argc: 1, index: 111, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EnableLooped();  // argc: 1, index: 112, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EnableQueue();  // argc: 1, index: 113, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret EnablePlaylists();  // argc: 1, index: 114, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdatePlaybackStatus();  // argc: 1, index: 115, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateShuffled();  // argc: 1, index: 116, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateLooped();  // argc: 1, index: 117, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateVolume();  // argc: 1, index: 118, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret CurrentEntryWillChange();  // argc: 0, index: 119, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CurrentEntryIsAvailable();  // argc: 1, index: 120, ipc args: [bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateCurrentEntryText();  // argc: 1, index: 121, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateCurrentEntryElapsedSeconds();  // argc: 1, index: 122, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret UpdateCurrentEntryCoverArt();  // argc: 2, index: 123, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    public unknown_ret CurrentEntryDidChange();  // argc: 0, index: 124, ipc args: [], ipc returns: [bytes1]
    public unknown_ret QueueWillChange();  // argc: 0, index: 125, ipc args: [], ipc returns: [bytes1]
    public unknown_ret ResetQueueEntries();  // argc: 0, index: 126, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetQueueEntry();  // argc: 3, index: 127, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCurrentQueueEntry();  // argc: 1, index: 128, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret QueueDidChange();  // argc: 0, index: 129, ipc args: [], ipc returns: [bytes1]
    public unknown_ret PlaylistWillChange();  // argc: 0, index: 130, ipc args: [], ipc returns: [bytes1]
    public unknown_ret ResetPlaylistEntries();  // argc: 0, index: 131, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetPlaylistEntry();  // argc: 3, index: 132, ipc args: [bytes4, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCurrentPlaylistEntry();  // argc: 1, index: 133, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret PlaylistDidChange();  // argc: 0, index: 134, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestAlbumCoverForAlbumID();  // argc: 1, index: 135, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestAlbumCoverForTrackKey();  // argc: 1, index: 136, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAlbumCoverForIndex();  // argc: 3, index: 137, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret CancelAlbumCoverRequestForIndex();  // argc: 1, index: 138, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetAlbumCoverURLForAlbumID();  // argc: 3, index: 139, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAlbumCoverURLForTrackKey();  // argc: 3, index: 140, ipc args: [string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public unknown_ret StartUsingSpotify();  // argc: 0, index: 141, ipc args: [], ipc returns: []
    public unknown_ret StopUsingSpotify();  // argc: 0, index: 142, ipc args: [], ipc returns: []
    public unknown_ret GetStatusSpotify();  // argc: 0, index: 143, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret LoginSpotify();  // argc: 2, index: 144, ipc args: [string, string], ipc returns: []
    public unknown_ret ReloginSpotify();  // argc: 0, index: 145, ipc args: [], ipc returns: []
    public unknown_ret GetCurrentUserSpotify();  // argc: 0, index: 146, ipc args: [], ipc returns: [string]
    public unknown_ret ForgetCurrentUserSpotify();  // argc: 0, index: 147, ipc args: [], ipc returns: []
    public unknown_ret LogoutSpotify();  // argc: 0, index: 148, ipc args: [], ipc returns: []
    public unknown_ret DumpStatusToConsole();  // argc: 0, index: 149, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ReplacePlaylistWithSoundtrackAlbum();  // argc: 1, index: 150, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret GetQueueSoundtrackAppID();  // argc: 0, index: 151, ipc args: [], ipc returns: [bytes4]
}