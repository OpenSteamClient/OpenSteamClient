//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using System.Text;
using OpenSteamworks.Attributes;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Generated;

public unsafe interface IClientApps
{
    public int GetAppData(AppId_t unAppID, string pchKey, StringBuilder pchValue, int cchValueMax);  // argc: 4, index: 1, ipc args: [bytes4, string, bytes4], ipc returns: [bytes4]
    public bool SetLocalAppConfig(AppId_t unAppID, void* pchBuffer, int cbBuffer);  // argc: 3, index: 2, ipc args: [bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    public AppId_t GetInternalAppIDFromGameID(in CGameID id);  // argc: 1, index: 3, ipc args: [bytes8], ipc returns: [bytes4]
    public unknown_ret GetAllOwnedMultiplayerApps(uint[] punAppIDs, int cAppIDsMax);  // argc: 2, index: 4, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public int GetAvailableLaunchOptions(AppId_t unAppID, uint[] options, uint cuOptionsMax);  // argc: 3, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public unknown_ret GetAppDataSection(AppId_t unAppID, EAppInfoSection eSection, byte[] pchBuffer, int cbBufferMax, bool bSharedKVSymbols);  // argc: 5, index: 6, ipc args: [bytes4, bytes4, bytes4, bytes1], ipc returns: [bytes4]
    /// <summary>
    /// Called by ValveSteam 444 times.
    /// </summary>
    /// <returns></returns>
    public unknown_ret GetMultipleAppDataSections(AppId_t unAppID, EAppInfoSection[] sections, int sectionsCount, byte[] pchBuffer, int cbBufferMax, bool bSharedKVSymbols, int[] sectionLengths);  // argc: 7, index: 7, ipc args: [bytes4, bytes4, bytes_length_from_reg, bytes4, bytes1], ipc returns: [bytes4]
    public bool RequestAppInfoUpdate(uint[] appIds, int appIdsLength);  // argc: 2, index: 8, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes1]
    public int GetDLCCount(AppId_t app);  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [bytes4]
    public bool BGetDLCDataByIndex(AppId_t app, int iDLC, out uint dlcID, out bool availableOnStore, StringBuilder name, int nameMax);  // argc: 6, index: 10, ipc args: [bytes4, bytes4, bytes4], ipc returns: [boolean, bytes4, boolean, bytes_length_from_mem]
    public EAppType GetAppType(AppId_t app);  // argc: 1, index: 11, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetStoreTagLocalization(ELanguage language, uint* unk1, int unk2, void* unk3, int unk3Max);  // argc: 5, index: 12, ipc args: [bytes4, bytes4, bytes_length_from_reg, bytes4], ipc returns: [bytes4]
    /// <summary>
    /// Locks the app info cache from changes. Required when calling GetAppKVRaw.
    /// </summary>
    /// <returns>True if locked successfully, false if locking failed or a lock is already in use</returns>
    public bool TakeUpdateLock();  // argc: 0, index: 13, ipc args: [], ipc returns: [bytes1]
    /// <summary>
    /// A "newer" method ValveSteam uses to get app info. Seems to rely on internal KeyValue class structs to work.
    /// </summary>
    [BlacklistedInCrossProcessIPC]
    public bool GetAppKVRaw(AppId_t app, byte** outPtrToAppInfoData, byte** outPtrToComputedKVData);  // argc: 3, index: 14, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    /// <summary>
    /// Unlocks the app info cache.
    /// </summary>
    public void ReleaseUpdateLock();  // argc: 0, index: 15, ipc args: [], ipc returns: []
    /// <summary>
    /// Gets the current user's AppInfoChangeNumber.
    /// </summary>
    public int GetLastChangeNumberReceived();  // argc: 0, index: 16, ipc args: [], ipc returns: [bytes4]
}