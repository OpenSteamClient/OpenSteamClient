//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUGC
{
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryUserUGCRequest();  // argc: 7, index: 1
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryAllUGCRequest(bool unk1);  // argc: 5, index: 2
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryAllUGCRequest(double unk1, bool unk2);  // argc: 5, index: 3
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryUGCDetailsRequest();  // argc: 2, index: 4
    // WARNING: Arguments are unknown!
    public unknown_ret SendQueryUGCRequest();  // argc: 2, index: 5
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCResult();  // argc: 4, index: 6
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCNumTags();  // argc: 3, index: 7
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCTag();  // argc: 6, index: 8
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCTagDisplayName();  // argc: 6, index: 9
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCPreviewURL();  // argc: 5, index: 10
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCImageURL();  // argc: 7, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCMetadata();  // argc: 5, index: 12
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCChildren();  // argc: 5, index: 13
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCStatistic();  // argc: 5, index: 14
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCNumAdditionalPreviews();  // argc: 3, index: 15
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCAdditionalPreview();  // argc: 9, index: 16
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCNumKeyValueTags();  // argc: 3, index: 17
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCKeyValueTag(bool unk1);  // argc: 8, index: 18
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCKeyValueTag(double unk1, bool unk2);  // argc: 6, index: 19
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCContentDescriptors();  // argc: 5, index: 20
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCIsDepotBuild();  // argc: 4, index: 21
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseQueryUGCRequest();  // argc: 2, index: 22
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequiredTag();  // argc: 3, index: 23
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequiredTagGroup();  // argc: 3, index: 24
    // WARNING: Arguments are unknown!
    public unknown_ret AddExcludedTag();  // argc: 3, index: 25
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnOnlyIDs();  // argc: 3, index: 26
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnKeyValueTags();  // argc: 3, index: 27
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnLongDescription();  // argc: 3, index: 28
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnMetadata();  // argc: 3, index: 29
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnChildren();  // argc: 3, index: 30
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnAdditionalPreviews();  // argc: 3, index: 31
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnTotalOnly();  // argc: 3, index: 32
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnPlaytimeStats();  // argc: 3, index: 33
    // WARNING: Arguments are unknown!
    public unknown_ret SetLanguage();  // argc: 3, index: 34
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowCachedResponse();  // argc: 3, index: 35
    // WARNING: Arguments are unknown!
    public unknown_ret SetCloudFileNameFilter();  // argc: 3, index: 36
    // WARNING: Arguments are unknown!
    public unknown_ret SetMatchAnyTag();  // argc: 3, index: 37
    // WARNING: Arguments are unknown!
    public unknown_ret SetSearchText();  // argc: 3, index: 38
    // WARNING: Arguments are unknown!
    public unknown_ret SetRankedByTrendDays();  // argc: 3, index: 39
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimeCreatedDateRange();  // argc: 4, index: 40
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimeUpdatedDateRange();  // argc: 4, index: 41
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequiredKeyValueTag();  // argc: 4, index: 42
    // WARNING: Arguments are unknown!
    public unknown_ret RequestUGCDetails();  // argc: 3, index: 43
    // WARNING: Arguments are unknown!
    public unknown_ret CreateItem();  // argc: 2, index: 44
    // WARNING: Arguments are unknown!
    public unknown_ret StartItemUpdate();  // argc: 3, index: 45
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemTitle();  // argc: 3, index: 46
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemDescription();  // argc: 3, index: 47
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemUpdateLanguage();  // argc: 3, index: 48
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemMetadata();  // argc: 3, index: 49
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemVisibility();  // argc: 3, index: 50
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemTags();  // argc: 3, index: 51
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemContent();  // argc: 3, index: 52
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemPreview();  // argc: 3, index: 53
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowLegacyUpload();  // argc: 3, index: 54
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveAllItemKeyValueTags();  // argc: 2, index: 55
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveItemKeyValueTags();  // argc: 3, index: 56
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemKeyValueTag();  // argc: 4, index: 57
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemPreviewFile();  // argc: 4, index: 58
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemPreviewVideo();  // argc: 3, index: 59
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateItemPreviewFile();  // argc: 4, index: 60
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateItemPreviewVideo();  // argc: 4, index: 61
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveItemPreview();  // argc: 3, index: 62
    // WARNING: Arguments are unknown!
    public unknown_ret AddContentDescriptor();  // argc: 3, index: 63
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveContentDescriptor();  // argc: 3, index: 64
    // WARNING: Arguments are unknown!
    public unknown_ret SubmitItemUpdate();  // argc: 3, index: 65
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemUpdateProgress();  // argc: 4, index: 66
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserItemVote();  // argc: 3, index: 67
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserItemVote();  // argc: 2, index: 68
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemToFavorites();  // argc: 3, index: 69
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveItemFromFavorites();  // argc: 3, index: 70
    // WARNING: Arguments are unknown!
    public unknown_ret SubscribeItem();  // argc: 4, index: 71
    // WARNING: Arguments are unknown!
    public unknown_ret UnsubscribeItem();  // argc: 3, index: 72
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumSubscribedItems();  // argc: 1, index: 73
    // WARNING: Arguments are unknown!
    public unknown_ret GetSubscribedItems();  // argc: 3, index: 74
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemState();  // argc: 3, index: 75
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemInstallInfo();  // argc: 7, index: 76
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemDownloadInfo();  // argc: 5, index: 77
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadItem();  // argc: 4, index: 78
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppItemsStatus();  // argc: 3, index: 79
    // WARNING: Arguments are unknown!
    public unknown_ret BInitWorkshopForGameServer();  // argc: 3, index: 80
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendDownloads();  // argc: 2, index: 81
    // WARNING: Arguments are unknown!
    public unknown_ret GetAllItemsSizeOnDisk();  // argc: 1, index: 82
    // WARNING: Arguments are unknown!
    public unknown_ret StartPlaytimeTracking();  // argc: 3, index: 83
    // WARNING: Arguments are unknown!
    public unknown_ret StopPlaytimeTracking();  // argc: 3, index: 84
    // WARNING: Arguments are unknown!
    public unknown_ret StopPlaytimeTrackingForAllItems();  // argc: 1, index: 85
    // WARNING: Arguments are unknown!
    public unknown_ret AddDependency();  // argc: 4, index: 86
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveDependency();  // argc: 4, index: 87
    // WARNING: Arguments are unknown!
    public unknown_ret AddAppDependency();  // argc: 3, index: 88
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveAppDependency();  // argc: 3, index: 89
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppDependencies();  // argc: 2, index: 90
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteItem();  // argc: 2, index: 91
    public unknown_ret ShowWorkshopEULA();  // argc: 0, index: 92
    public unknown_ret GetWorkshopEULAStatus();  // argc: 0, index: 93
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserContentDescriptorPreferences();  // argc: 2, index: 0
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumDownloadedItems();  // argc: 1, index: 94
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadedItems();  // argc: 3, index: 95
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetFullQueryUGCResponse();  // argc: 3, index: 96
    // WARNING: Arguments are unknown!
    public unknown_ret GetSerializedQueryUGCResponse();  // argc: 3, index: 97
}