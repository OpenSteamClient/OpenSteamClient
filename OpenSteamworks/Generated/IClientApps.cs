//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientApps
{
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppData();  // argc: 4, index: 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetLocalAppConfig();  // argc: 3, index: 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public AppId_t GetInternalAppIDFromGameID(in CGameID id);  // argc: 1, index: 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAllOwnedMultiplayerApps();  // argc: 2, index: 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAvailableLaunchOptions();  // argc: 3, index: 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppDataSection();  // argc: 5, index: 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetMultipleAppDataSections();  // argc: 7, index: 7
    public unknown_ret RequestAppInfoUpdate(AppId_t[] appIds, int appIdsLength);  // argc: 2, index: 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public int GetDLCCount(AppId_t app);  // argc: 1, index: 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetDLCDataByIndex(AppId_t app, int iDLC, AppId_t *pAppID, bool *pbAvailable, char *pchName, int cchNameBufferSize);  // argc: 6, index: 10
    public unknown_ret GetAppType(AppId_t app);  // argc: 1, index: 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetStoreTagLocalization();  // argc: 5, index: 12
    /// <summary>
    /// Locks the app info cache from changes. Required when calling GetAppKVRaw.
    /// </summary>
    /// <returns>True if locked successfully, false if locking failed or a lock is already in use</returns>
    public bool TakeUpdateLock();  // argc: 0, index: 13
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAppKVRaw(AppId_t app, uint* unk1, uint* unk2);  // argc: 3, index: 14
    /// <summary>
    /// Unlocks the app info cache.
    /// </summary>
    public void ReleaseUpdateLock();  // argc: 0, index: 15
    /// <summary>
    /// Gets the current user's AppInfoChangeNumber.
    /// </summary>
    public uint GetLastChangeNumberReceived();  // argc: 0, index: 16
}