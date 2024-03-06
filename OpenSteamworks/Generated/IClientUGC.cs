//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUGC
{
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryUserUGCRequest();  // argc: 7, index: 1, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryAllUGCRequest(bool unk1);  // argc: 5, index: 2, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryAllUGCRequest(double unk1, bool unk2);  // argc: 5, index: 3, ipc args: [bytes4, bytes4, bytes4, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateQueryUGCDetailsRequest();  // argc: 2, index: 4, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SendQueryUGCRequest();  // argc: 2, index: 5, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCResult();  // argc: 4, index: 6, ipc args: [bytes8, bytes4], ipc returns: [bytes1, bytes9772]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCNumTags();  // argc: 3, index: 7, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCTag();  // argc: 6, index: 8, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCTagDisplayName();  // argc: 6, index: 9, ipc args: [bytes8, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCPreviewURL();  // argc: 5, index: 10, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCImageURL();  // argc: 7, index: 11, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCMetadata();  // argc: 5, index: 12, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCChildren();  // argc: 5, index: 13, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCStatistic();  // argc: 5, index: 14, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCNumAdditionalPreviews();  // argc: 3, index: 15, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCAdditionalPreview();  // argc: 9, index: 16, ipc args: [bytes8, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCNumKeyValueTags();  // argc: 3, index: 17, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCKeyValueTag(bool unk1);  // argc: 8, index: 18, ipc args: [bytes8, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCKeyValueTag(double unk1, bool unk2);  // argc: 6, index: 19, ipc args: [bytes8, bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCContentDescriptors();  // argc: 5, index: 20, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetQueryUGCIsDepotBuild();  // argc: 4, index: 21, ipc args: [bytes8, bytes4], ipc returns: [bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ReleaseQueryUGCRequest();  // argc: 2, index: 22, ipc args: [bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequiredTag();  // argc: 3, index: 23, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequiredTagGroup();  // argc: 3, index: 24, ipc args: [bytes8, utlvector], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddExcludedTag();  // argc: 3, index: 25, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnOnlyIDs();  // argc: 3, index: 26, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnKeyValueTags();  // argc: 3, index: 27, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnLongDescription();  // argc: 3, index: 28, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnMetadata();  // argc: 3, index: 29, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnChildren();  // argc: 3, index: 30, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnAdditionalPreviews();  // argc: 3, index: 31, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnTotalOnly();  // argc: 3, index: 32, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetReturnPlaytimeStats();  // argc: 3, index: 33, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLanguage();  // argc: 3, index: 34, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowCachedResponse();  // argc: 3, index: 35, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAdminQuery();  // argc: 3, index: 36, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCloudFileNameFilter();  // argc: 3, index: 37, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetMatchAnyTag();  // argc: 3, index: 38, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetSearchText();  // argc: 3, index: 39, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRankedByTrendDays();  // argc: 3, index: 40, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimeCreatedDateRange();  // argc: 4, index: 41, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetTimeUpdatedDateRange();  // argc: 4, index: 42, ipc args: [bytes8, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddRequiredKeyValueTag();  // argc: 4, index: 43, ipc args: [bytes8, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestUGCDetails();  // argc: 3, index: 44, ipc args: [bytes8, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret CreateItem();  // argc: 2, index: 45, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret StartItemUpdate();  // argc: 3, index: 46, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemTitle();  // argc: 3, index: 47, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemDescription();  // argc: 3, index: 48, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemUpdateLanguage();  // argc: 3, index: 49, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemMetadata();  // argc: 3, index: 50, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemVisibility();  // argc: 3, index: 51, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemTags();  // argc: 4, index: 52, ipc args: [bytes8, utlvector, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemContent();  // argc: 3, index: 53, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetItemPreview();  // argc: 3, index: 54, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAllowLegacyUpload();  // argc: 3, index: 55, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveAllItemKeyValueTags();  // argc: 2, index: 56, ipc args: [bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveItemKeyValueTags();  // argc: 3, index: 57, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemKeyValueTag();  // argc: 4, index: 58, ipc args: [bytes8, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemPreviewFile();  // argc: 4, index: 59, ipc args: [bytes8, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemPreviewVideo();  // argc: 3, index: 60, ipc args: [bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateItemPreviewFile();  // argc: 4, index: 61, ipc args: [bytes8, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateItemPreviewVideo();  // argc: 4, index: 62, ipc args: [bytes8, bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveItemPreview();  // argc: 3, index: 63, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret AddContentDescriptor();  // argc: 3, index: 64, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveContentDescriptor();  // argc: 3, index: 65, ipc args: [bytes8, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetExternalAssetID();  // argc: 4, index: 66, ipc args: [bytes8, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SubmitItemUpdate();  // argc: 3, index: 67, ipc args: [bytes8, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemUpdateProgress();  // argc: 4, index: 68, ipc args: [bytes8], ipc returns: [bytes4, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserItemVote();  // argc: 3, index: 69, ipc args: [bytes8, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserItemVote();  // argc: 2, index: 70, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret AddItemToFavorites();  // argc: 3, index: 71, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveItemFromFavorites();  // argc: 3, index: 72, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SubscribeItem();  // argc: 4, index: 73, ipc args: [bytes4, bytes8, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UnsubscribeItem();  // argc: 3, index: 74, ipc args: [bytes4, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumSubscribedItems();  // argc: 1, index: 75, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetSubscribedItems();  // argc: 3, index: 76, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetSubscribedItemsInternal();  // argc: 2, index: 77, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetWorkshopItemsDisabledLocally();  // argc: 3, index: 78, ipc args: [bytes4, bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetSubscriptionsLoadOrder();  // argc: 2, index: 79, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret MoveSubscriptionsLoadOrder();  // argc: 3, index: 80, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemState();  // argc: 3, index: 81, ipc args: [bytes4, bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemInstallInfo();  // argc: 7, index: 82, ipc args: [bytes4, bytes8, bytes4], ipc returns: [bytes1, bytes8, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemDownloadInfo();  // argc: 5, index: 83, ipc args: [bytes4, bytes8], ipc returns: [bytes1, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DownloadItem();  // argc: 4, index: 84, ipc args: [bytes4, bytes8, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppItemsStatus();  // argc: 3, index: 85, ipc args: [bytes4], ipc returns: [bytes1, bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BInitWorkshopForGameServer();  // argc: 3, index: 86, ipc args: [bytes4, bytes4, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SuspendDownloads();  // argc: 2, index: 87, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetAllItemsSizeOnDisk();  // argc: 1, index: 88, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret StartPlaytimeTracking();  // argc: 3, index: 89, ipc args: [bytes4, bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret StopPlaytimeTracking();  // argc: 3, index: 90, ipc args: [bytes4, bytes4, bytes_length_from_reg], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret StopPlaytimeTrackingForAllItems();  // argc: 1, index: 91, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret AddDependency();  // argc: 4, index: 92, ipc args: [bytes8, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveDependency();  // argc: 4, index: 93, ipc args: [bytes8, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret AddAppDependency();  // argc: 3, index: 94, ipc args: [bytes8, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveAppDependency();  // argc: 3, index: 95, ipc args: [bytes8, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppDependencies();  // argc: 2, index: 96, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteItem();  // argc: 2, index: 97, ipc args: [bytes8], ipc returns: [bytes8]
    public unknown_ret ShowWorkshopEULA();  // argc: 0, index: 98, ipc args: [], ipc returns: [bytes1]
    public unknown_ret GetWorkshopEULAStatus();  // argc: 0, index: 99, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetUserContentDescriptorPreferences();  // argc: 2, index: 100, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetNumDownloadedItems();  // argc: 1, index: 101, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDownloadedItems();  // argc: 3, index: 102, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetFullQueryUGCResponse();  // argc: 3, index: 103, ipc args: [bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public uint GetSerializedQueryUGCResponse(UInt64 unk, CUtlBuffer* data);  // argc: 3, index: 104, ipc args: [bytes8], ipc returns: [bytes4, unknown]
}