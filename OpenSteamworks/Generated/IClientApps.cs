//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientApps
{
    public unknown_ret GetAppData(AppId_t unAppID, string pchKey, StringBuilder pchValue, int cchValueMax);  // argc: 4, index: 1
    public unknown_ret SetLocalAppConfig(AppId_t unAppID, void* pchBuffer, int cbBuffer);  // argc: 3, index: 2
    public AppId_t GetInternalAppIDFromGameID(in CGameID id);  // argc: 1, index: 3
    public unknown_ret GetAllOwnedMultiplayerApps(uint[] punAppIDs, int cAppIDsMax);  // argc: 2, index: 4
    public unknown_ret GetAvailableLaunchOptions(AppId_t unAppID, uint[] options, uint cuOptionsMax);  // argc: 3, index: 5
    public unknown_ret GetAppDataSection(AppId_t unAppID, EAppInfoSection eSection, byte[] pchBuffer, int cbBufferMax, bool bSharedKVSymbols);  // argc: 5, index: 6
    /// <summary>
    /// Called by ValveSteam 444 times.
    /// </summary>
    /// <returns></returns>
    public unknown_ret GetMultipleAppDataSections(AppId_t unAppID, EAppInfoSection[] sections, int sectionsCount, byte[] pchBuffer, int cbBufferMax, bool bSharedKVSymbols, ref int unk2);  // argc: 7, index: 7
    public bool RequestAppInfoUpdate(AppId_t[] appIds, uint appIdsLength);  // argc: 2, index: 8
    public int GetDLCCount(AppId_t app);  // argc: 1, index: 9
    public unknown_ret BGetDLCDataByIndex(AppId_t app, int iDLC, AppId_t *pAppID, bool *pbAvailable, char *pchName, int cchNameBufferSize);  // argc: 6, index: 10
    public unknown_ret GetAppType(AppId_t app);  // argc: 1, index: 11
    // WARNING: Arguments are unknown!
    public unknown_ret GetStoreTagLocalization(ELanguage language, uint* unk1, int unk2, void* unk3, int unk3Max);  // argc: 5, index: 12
    /// <summary>
    /// Locks the app info cache from changes. Required when calling GetAppKVRaw.
    /// </summary>
    /// <returns>True if locked successfully, false if locking failed or a lock is already in use</returns>
    public bool TakeUpdateLock();  // argc: 0, index: 13
    [BlacklistedInCrossProcessIPC]
    // Called by ValveSteam for each appid you own. Should we?
    public unknown_ret GetAppKVRaw(AppId_t app, byte[] pchBuffer, int cbBufferMax);  // argc: 3, index: 14
    /// <summary>
    /// Unlocks the app info cache.
    /// </summary>
    public void ReleaseUpdateLock();  // argc: 0, index: 15
    /// <summary>
    /// Gets the current user's AppInfoChangeNumber.
    /// </summary>
    public uint GetLastChangeNumberReceived();  // argc: 0, index: 16
}