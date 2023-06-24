//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code, comments, and all related files, projects, resources,
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo,
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef ICLIENTMUSIC_H
#define ICLIENTMUSIC_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "MusicCommon.h"

enum MediaController_Status
{
	// TODO: Reverse this enum
};

#define CLIENTMUSIC_INTERFACE_VERSION "CLIENTMUSIC_INTERFACE_VERSION001"

abstract_class IClientMusic
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsEnabled() = 0; //argc: 0, index 1
    virtual unknown_ret Enable(bool) = 0; //argc: 1, index 2
    virtual unknown_ret EnableCrawlLogging(bool) = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsPlaying() = 0; //argc: 0, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueueCount() = 0; //argc: 0, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetCurrentQueueEntry() = 0; //argc: 0, index 6
    virtual unknown_ret GetQueueEntryURI(int, char*, int) = 0; //argc: 3, index 7
    virtual unknown_ret GetQueueEntryData(int, char*, int, char*, int, char*, int, int*, void*, int*) = 0; //argc: 10, index 8
    virtual unknown_ret GetQueueEntryOrigin(int, void*, int*) = 0; //argc: 3, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EmptyQueue() = 0; //argc: 0, index 10
    virtual unknown_ret RemoveQueueEntry(int, bool) = 0; //argc: 2, index 11
    virtual unknown_ret AddArtistToQueue(char const*, bool, bool) = 0; //argc: 3, index 12
    virtual unknown_ret AddTracksOfAlbumIDToQueue(int, int, bool, char const*, bool, bool, bool) = 0; //argc: 7, index 13
    virtual unknown_ret AddAllTracksOfAlbumIDToQueue(int, int, bool, bool, bool) = 0; //argc: 5, index 14
    virtual unknown_ret AddTracksOfPlaylistIDToQueue(int, int, bool, bool, bool, bool) = 0; //argc: 6, index 15
    virtual unknown_ret SetSuppressAutoTrackAdvance(bool) = 0; //argc: 1, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPlaybackStatus() = 0; //argc: 0, index 17
    virtual unknown_ret SetPlayingRepeatStatus(void) = 0; //argc: 1, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPlayingRepeatStatus() = 0; //argc: 0, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TogglePlayingRepeatStatus() = 0; //argc: 0, index 20
    virtual unknown_ret SetPlayingShuffled(bool) = 0; //argc: 1, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IsPlayingShuffled() = 0; //argc: 0, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret Play() = 0; //argc: 0, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret Pause() = 0; //argc: 0, index 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PlayPrevious() = 0; //argc: 0, index 25
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PlayNext() = 0; //argc: 0, index 26
    virtual unknown_ret PlayEntry(int) = 0; //argc: 1, index 27
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TogglePlayPause() = 0; //argc: 0, index 28
    virtual unknown_ret SetVolume(float) = 0; //argc: 1, index 29
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetVolume() = 0; //argc: 0, index 30
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ToggleMuteVolume() = 0; //argc: 0, index 31
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IncreaseVolume() = 0; //argc: 0, index 32
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DecreaseVolume() = 0; //argc: 0, index 33
    virtual unknown_ret SetPlaybackPosition(int) = 0; //argc: 1, index 34
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPlaybackPosition() = 0; //argc: 0, index 35
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPlaybackDuration() = 0; //argc: 0, index 36
    virtual unknown_ret LocalLibraryCrawlTrack(char const*) = 0; //argc: 1, index 37
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ResetLocalLibrary() = 0; //argc: 0, index 38
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStatusLocalLibrary() = 0; //argc: 0, index 39
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SaveLocalLibraryDirectorySettings() = 0; //argc: 0, index 40
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalLibraryDirectoryEntryCount() = 0; //argc: 0, index 41
    virtual unknown_ret GetLocalLibraryDirectoryEntry(int, char*, int) = 0; //argc: 3, index 42
    virtual unknown_ret AddLocalLibraryDirectoryEntry(char const*) = 0; //argc: 1, index 43
    virtual unknown_ret ResetLocalLibraryDirectories(bool) = 0; //argc: 1, index 44
    virtual unknown_ret GetDefaultLocalLibraryDirectory(char*, int) = 0; //argc: 2, index 45
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LocalLibraryStopCrawling() = 0; //argc: 0, index 46
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret UpdateLocalLibraryDirectoriesToCrawl() = 0; //argc: 0, index 47
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BLocalLibraryIsCrawling() = 0; //argc: 0, index 48
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalLibraryTrackCount() = 0; //argc: 0, index 49
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalLibraryAlbumCount() = 0; //argc: 0, index 50
    virtual unknown_ret GetLocalLibraryAlbumID(int, bool) = 0; //argc: 2, index 51
    virtual unknown_ret GetLocalLibraryAlbumIDEntry(int, char*, int, char*, int, int*, int*, bool*) = 0; //argc: 8, index 52
    virtual unknown_ret GetLocalLibraryAlbumIDTrackEntry(int, int, char*, int, int*, char*, int, char*, int) = 0; //argc: 9, index 53
    virtual unknown_ret GetLocalLibraryAlbumDirectoryForAlbumID(int, char*, int) = 0; //argc: 3, index 54
    virtual unknown_ret GetLocalLibraryAlbumSortNameForAlbumID(int, char*, int) = 0; //argc: 3, index 55
    virtual unknown_ret GetLocalLibraryArtistSortNameForAlbumID(int, char*, int) = 0; //argc: 3, index 56
    virtual unknown_ret GetLocalLibraryTrackCountForAlbumID(int) = 0; //argc: 1, index 57
    virtual unknown_ret GetLocalLibraryAlbumIDTrackKey(int, int, char*, int) = 0; //argc: 4, index 58
    virtual unknown_ret GetLocalLibraryAlbumIDForTrackKey(char const*) = 0; //argc: 1, index 59
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLocalLibraryArtistCount() = 0; //argc: 0, index 60
    virtual unknown_ret GetLocalLibraryArtistName(int, char*, int, char*, int) = 0; //argc: 5, index 61
    virtual unknown_ret GetLocalLibraryAlbumCountForArtistName(char const*) = 0; //argc: 1, index 62
    virtual unknown_ret GetLocalLibraryTrackAndAlbumCountOfArtistName(char const*, int*, int*) = 0; //argc: 3, index 63
    virtual unknown_ret GetLocalLibraryAlbumIDForArtist(char const*, int) = 0; //argc: 2, index 64
    virtual unknown_ret GetLocalLibraryRepresentativeAlbumIDForArtist(char const*, bool*) = 0; //argc: 2, index 65
    virtual unknown_ret GetLocalLibraryTrackEntry(char const*, char*, int, int*, char*, int) = 0; //argc: 6, index 66
    virtual unknown_ret BRequestAllArtistListData(int) = 0; //argc: 1, index 67
    virtual unknown_ret BRequestAllAlbumListData(int) = 0; //argc: 1, index 68
    virtual unknown_ret BRequestAllTracksListDataForAlbumID(int, int) = 0; //argc: 2, index 69
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPlaylistCount() = 0; //argc: 0, index 70
    virtual unknown_ret GetPlaylistID(int) = 0; //argc: 1, index 71
    virtual unknown_ret GetPositionForPlaylistID(int) = 0; //argc: 1, index 72
    virtual unknown_ret GetPlaylistIDForPosition(int) = 0; //argc: 1, index 73
    virtual unknown_ret BRequestAllPlayListData(int) = 0; //argc: 1, index 74
    virtual unknown_ret GetNextPlaylistName(char const*, char*, int) = 0; //argc: 3, index 75
    virtual unknown_ret InsertPlaylistWithNameAtPosition(char const*, int) = 0; //argc: 2, index 76
    virtual unknown_ret MovePlaylistFromPositionToPosition(int, int) = 0; //argc: 2, index 77
    virtual unknown_ret DeletePlaylistWithID(int) = 0; //argc: 1, index 78
    virtual unknown_ret RenamePlaylistWithID(int, char const*) = 0; //argc: 2, index 79
    virtual unknown_ret AddRandomTracksToPlaylistID(int, int) = 0; //argc: 2, index 80
    virtual unknown_ret GetPlaylistIDData(int, char*, int, int*, int*, int*, int*, int*) = 0; //argc: 8, index 81
    virtual unknown_ret GetPlaylistIDTrackCount(int) = 0; //argc: 1, index 82
    virtual unknown_ret BRequestAllTracksListDataForPlaylistID(int, int) = 0; //argc: 2, index 83
    virtual unknown_ret GetPlaylistIDTrackKey(int, int, char*, int, int*) = 0; //argc: 5, index 84
    virtual unknown_ret GetPositionForPlaylistItemID(int) = 0; //argc: 1, index 85
    virtual unknown_ret AddTrackKeyToPlaylistID(int, char const*) = 0; //argc: 2, index 86
    virtual unknown_ret AddAlbumIDToPlaylistID(int, int, char const*) = 0; //argc: 3, index 87
    virtual unknown_ret AddArtistNameToPlaylistID(int, char const*) = 0; //argc: 2, index 88
    virtual unknown_ret AddPlaylistIDToPlaylistID(int, int) = 0; //argc: 2, index 89
    virtual unknown_ret RemovePlaylistPositionFromPlaylistID(int, int) = 0; //argc: 2, index 90
    virtual unknown_ret RemoveAllTracksFromPlaylistID(int, bool) = 0; //argc: 2, index 91
    virtual unknown_ret MoveTrackFromPositionToPositonInPlaylistID(int, int, int) = 0; //argc: 3, index 92
    virtual unknown_ret AppendQueueToPlaylistID(int, bool) = 0; //argc: 2, index 93
    virtual unknown_ret GetLocalLibraryRepresentativeAlbumIDForPlaylist(int, bool*) = 0; //argc: 2, index 94
    virtual unknown_ret MarkTrackKeyAsPlayed(char const*) = 0; //argc: 1, index 95
    virtual unknown_ret GetMostRecentlyPlayedAlbums(int, int*) = 0; //argc: 2, index 96
    virtual unknown_ret GetMostRecentlyAddedAlbums(int, int*) = 0; //argc: 2, index 97
    virtual unknown_ret ActivateRemotePlayerWithID(int) = 0; //argc: 1, index 98
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetActiveRemotePlayerID() = 0; //argc: 0, index 99
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetRemotePlayerCount() = 0; //argc: 0, index 100
    virtual unknown_ret GetRemotePlayerIDAndName(int, int*, char*, int) = 0; //argc: 4, index 101
    virtual unknown_ret GetCurrentEntryTextForRemotePlayerWithID(int, char*, int) = 0; //argc: 3, index 102
    virtual unknown_ret RegisterSteamMusicRemote(char const*) = 0; //argc: 1, index 103
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DeregisterSteamMusicRemote() = 0; //argc: 0, index 104
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BIsCurrentMusicRemote() = 0; //argc: 0, index 105
    virtual unknown_ret BActivationSuccess(bool) = 0; //argc: 1, index 106
    virtual unknown_ret SetDisplayName(char const*) = 0; //argc: 1, index 107
    virtual unknown_ret SetPNGIcon_64x64(void*, unsigned int) = 0; //argc: 2, index 108
    virtual unknown_ret EnablePlayPrevious(bool) = 0; //argc: 1, index 109
    virtual unknown_ret EnablePlayNext(bool) = 0; //argc: 1, index 110
    virtual unknown_ret EnableShuffled(bool) = 0; //argc: 1, index 111
    virtual unknown_ret EnableLooped(bool) = 0; //argc: 1, index 112
    virtual unknown_ret EnableQueue(bool) = 0; //argc: 1, index 113
    virtual unknown_ret EnablePlaylists(bool) = 0; //argc: 1, index 114
    virtual unknown_ret UpdatePlaybackStatus(AudioPlayback_Status) = 0; //argc: 1, index 115
    virtual unknown_ret UpdateShuffled(bool) = 0; //argc: 1, index 116
    virtual unknown_ret UpdateLooped(bool) = 0; //argc: 1, index 117
    virtual unknown_ret UpdateVolume(float) = 0; //argc: 1, index 118
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CurrentEntryWillChange() = 0; //argc: 0, index 119
    virtual unknown_ret CurrentEntryIsAvailable(bool) = 0; //argc: 1, index 120
    virtual unknown_ret UpdateCurrentEntryText(char const*) = 0; //argc: 1, index 121
    virtual unknown_ret UpdateCurrentEntryElapsedSeconds(int) = 0; //argc: 1, index 122
    virtual unknown_ret UpdateCurrentEntryCoverArt(void*, unsigned int) = 0; //argc: 2, index 123
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CurrentEntryDidChange() = 0; //argc: 0, index 124
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret QueueWillChange() = 0; //argc: 0, index 125
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ResetQueueEntries() = 0; //argc: 0, index 126
    virtual unknown_ret SetQueueEntry(int, int, char const*) = 0; //argc: 3, index 127
    virtual unknown_ret SetCurrentQueueEntry(int) = 0; //argc: 1, index 128
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret QueueDidChange() = 0; //argc: 0, index 129
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PlaylistWillChange() = 0; //argc: 0, index 130
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ResetPlaylistEntries() = 0; //argc: 0, index 131
    virtual unknown_ret SetPlaylistEntry(int, int, char const*) = 0; //argc: 3, index 132
    virtual unknown_ret SetCurrentPlaylistEntry(int) = 0; //argc: 1, index 133
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret PlaylistDidChange() = 0; //argc: 0, index 134
    virtual unknown_ret RequestAlbumCoverForAlbumID(int) = 0; //argc: 1, index 135
    virtual unknown_ret RequestAlbumCoverForTrackKey(char const*) = 0; //argc: 1, index 136
    virtual unknown_ret GetAlbumCoverForIndex(int, void*, unsigned int) = 0; //argc: 3, index 137
    virtual unknown_ret CancelAlbumCoverRequestForIndex(int) = 0; //argc: 1, index 138
    virtual unknown_ret GetAlbumCoverURLForAlbumID(int, char*, unsigned int) = 0; //argc: 3, index 139
    virtual unknown_ret GetAlbumCoverURLForTrackKey(char const*, char*, unsigned int) = 0; //argc: 3, index 140
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StartUsingSpotify() = 0; //argc: 0, index 141
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StopUsingSpotify() = 0; //argc: 0, index 142
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetStatusSpotify() = 0; //argc: 0, index 143
    virtual unknown_ret LoginSpotify(char const*, char const*) = 0; //argc: 2, index 144
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ReloginSpotify() = 0; //argc: 0, index 145
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetCurrentUserSpotify() = 0; //argc: 0, index 146
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ForgetCurrentUserSpotify() = 0; //argc: 0, index 147
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LogoutSpotify() = 0; //argc: 0, index 148
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret DumpStatusToConsole() = 0; //argc: 0, index 149
    virtual unknown_ret ReplacePlaylistWithSoundtrackAlbum(unsigned int) = 0; //argc: 1, index 150
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetQueueSoundtrackAppID() = 0; //argc: 0, index 151
};

#endif // ICLIENTMUSIC_H