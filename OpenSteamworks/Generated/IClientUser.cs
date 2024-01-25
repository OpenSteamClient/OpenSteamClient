//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Structs;
using OpenSteamworks.Enums;
using System.Text;

using OpenSteamworks.Protobuf;
using Google.Protobuf;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using OpenSteamworks.Attributes;

namespace OpenSteamworks.Generated;

//TODO: some API's take and use pointers to protobuf classes, which is not even remotely valid in C#. 
// support that (really strange) operation with C# structs (which will be really difficult), but for now we have a native library for doing that
public unsafe interface IClientUser
{
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_0_DONTUSE();  // argc: -1, index: 1, ipc args: [], ipc returns: []
    public EResult LogOn(CSteamID steamid, bool interactive = true);  // argc: 2, index: 2, ipc args: [uint64], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InvalidateCredentials();  // argc: 2, index: 3, ipc args: [string, bytes4], ipc returns: [bytes8]
    public void LogOff();  // argc: 0, index: 4, ipc args: [], ipc returns: []
    public bool BLoggedOn();  // argc: 0, index: 5, ipc args: [], ipc returns: [boolean]
    public ELogonState GetLogonState();  // argc: 0, index: 6, ipc args: [], ipc returns: [bytes4]
    public bool BConnected();  // argc: 0, index: 7, ipc args: [], ipc returns: [boolean]
    public bool BInitiateReconnect();  // argc: 0, index: 8, ipc args: [], ipc returns: [boolean]
    public unknown_ret EConnect();  // argc: 0, index: 9, ipc args: [], ipc returns: [bytes4]
    public bool BTryingToLogin();  // argc: 0, index: 10, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public CSteamID GetSteamID();  // argc: 1, index: 11, ipc args: [], ipc returns: [uint64]
    // WARNING: Arguments are unknown!
    public CSteamID GetConsoleSteamID();  // argc: 1, index: 12, ipc args: [], ipc returns: [uint64]
    public ulong GetClientInstanceID();  // argc: 0, index: 13, ipc args: [], ipc returns: [bytes8]
    public string GetUserCountry();  // argc: 0, index: 14, ipc args: [], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret IsVACBanned();  // argc: 1, index: 15, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetEmail();  // argc: 1, index: 16, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetConfigString();  // argc: 3, index: 17, ipc args: [bytes4, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetConfigString(int eRegistrySubTree, string key, StringBuilder stringOut, Int32 stringOutMax);  // argc: 4, index: 18, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret SetConfigInt();  // argc: 3, index: 19, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetConfigInt();  // argc: 3, index: 20, ipc args: [bytes4, string], ipc returns: [bytes1, bytes4]
    public unknown_ret SetConfigBinaryBlob(int eRegistrySubTree, string key, [IPCIn] CUtlBuffer* buf);  // argc: 3, index: 21, ipc args: [bytes4, string, unknown], ipc returns: [bytes1]
    public unknown_ret GetConfigBinaryBlob(int eRegistrySubTree, string key, [IPCOut] CUtlBuffer* buf);  // argc: 3, index: 22, ipc args: [bytes4, string], ipc returns: [bytes1, utlbuffer]
    // WARNING: Arguments are unknown!
    public unknown_ret DeleteConfigKey();  // argc: 2, index: 23, ipc args: [bytes4, string], ipc returns: [bytes1]
    public unknown_ret GetConfigStoreKeyName(int eRegistrySubTree, string pchKey, StringBuilder pchStoreName, Int32 cbStoreName);  // argc: 4, index: 24, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret InitiateGameConnection();  // argc: 8, index: 25, ipc args: [bytes4, uint64, bytes8, bytes4, bytes2, bytes1], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret InitiateGameConnectionOld();  // argc: 10, index: 26, ipc args: [bytes4, uint64, bytes8, bytes4, bytes2, bytes1, bytes4, bytes_length_from_mem], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret TerminateGameConnection();  // argc: 2, index: 27, ipc args: [bytes4, bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool TerminateGame(AppId_t appid, bool force);  // argc: 2, index: 28, ipc args: [bytes8, bytes1], ipc returns: [bytes1]
    /// <summary>
    /// Apparently deprecated, but still called by ValveSteam.
    /// </summary>
    /// <returns></returns>
    public void SetSelfAsChatDestination(bool val);  // argc: 1, index: 29, ipc args: [bytes1], ipc returns: []
    public bool IsPrimaryChatDestination();  // argc: 0, index: 30, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestLegacyCDKey(AppId_t appid);  // argc: 1, index: 31, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AckGuestPass(string guestPassId);  // argc: 1, index: 32, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RedeemGuestPass(string guestPassId);  // argc: 1, index: 33, ipc args: [string], ipc returns: [bytes1]
    public unknown_ret GetGuestPassToGiveCount();  // argc: 0, index: 34, ipc args: [], ipc returns: [bytes4]
    public unknown_ret GetGuestPassToRedeemCount();  // argc: 0, index: 35, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGuestPassToGiveInfo();  // argc: 9, index: 36, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes8, bytes4, bytes4, bytes4, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGuestPassToGiveOut(AppId_t appid);  // argc: 1, index: 37, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGuestPassToRedeem(AppId_t appid);  // argc: 1, index: 38, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGuestPassToRedeemInfo();  // argc: 7, index: 39, ipc args: [bytes4], ipc returns: [bytes1, bytes8, bytes4, bytes4, bytes4, bytes4, bytes4]
    public bool GetGuestPassToRedeemSenderName(AppId_t appid, StringBuilder name, int nameMax);  // argc: 3, index: 40, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public unknown_ret GetNumAppsInGuestPassesToRedeem();  // argc: 0, index: 41, ipc args: [], ipc returns: [bytes4]
    public uint GetAppsInGuestPassesToRedeem(AppId_t[] appids, uint appidsMax);  // argc: 2, index: 42, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    public unknown_ret GetCountUserNotifications();  // argc: 0, index: 43, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetCountUserNotification(int EUserNotification);  // argc: 1, index: 44, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestStoreAuthURL();  // argc: 1, index: 45, ipc args: [string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public void SetLanguage(string language);  // argc: 1, index: 46, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret TrackAppUsageEvent();  // argc: 3, index: 47, ipc args: [bytes8, bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RaiseConnectionPriority();  // argc: 2, index: 48, ipc args: [bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ResetConnectionPriority();  // argc: 1, index: 49, ipc args: [bytes4], ipc returns: []
    public unknown_ret GetDesiredNetQOSLevel();  // argc: 0, index: 50, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool BHasCachedCredentials(string username);  // argc: 1, index: 51, ipc args: [string], ipc returns: [boolean]
    public bool SetAccountNameForCachedCredentialLogin(string username, bool unk1);  // argc: 2, index: 52, ipc args: [string, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret DestroyCachedCredentials(string username, int revokeAction = (int)Protobuf.EAuthTokenRevokeAction.EauthTokenRevokeLogout);  // argc: 2, index: 53, ipc args: [string, bytes4], ipc returns: []
    public bool GetCurrentWebAuthToken(StringBuilder tokenOut, UInt32 bufSize);  // argc: 2, index: 54, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public SteamAPICall_t RequestWebAuthToken();  // argc: 0, index: 55, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLoginInformation(string username, string password, bool remember);  // argc: 3, index: 56, ipc args: [string, string, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTwoFactorCode(string twoFactorCode);  // argc: 1, index: 57, ipc args: [string], ipc returns: []
    public unknown_ret SetLoginToken(string token, string username);  // argc: 2, index: 58, ipc args: [string, string], ipc returns: []
    public UInt64 GetLoginTokenID();  // argc: 0, index: 59, ipc args: [], ipc returns: [bytes8]
    public unknown_ret ClearAllLoginInformation();  // argc: 0, index: 60, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BEnableEmbeddedClient();  // argc: 1, index: 61, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret ResetEmbeddedClient();  // argc: 1, index: 62, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BHasEmbeddedClientToken();  // argc: 1, index: 63, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestEmbeddedClientToken();  // argc: 1, index: 64, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AuthorizeNewDevice();  // argc: 3, index: 65, ipc args: [bytes4, bytes4, string], ipc returns: []
    public bool GetLanguage(StringBuilder langOut, int maxOut);  // argc: 2, index: 66, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret TrackSteamUsageEvent();  // argc: 3, index: 67, ipc args: [bytes4, bytes4, bytes_length_from_mem], ipc returns: []
    public unknown_ret SetComputerInUse();  // argc: 0, index: 68, ipc args: [], ipc returns: []
    public bool BIsGameRunning(CGameID gameid);  // argc: 1, index: 69, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BIsGameWindowReady(CGameID gameid);  // argc: 1, index: 70, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BUpdateAppOwnershipTicket(uint unk1, int pTicket);  // argc: 2, index: 71, ipc args: [bytes4, bytes1], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public UInt32 GetCustomBinariesState(AppId_t unAppID, ref UInt32 punProgress);  // argc: 3, index: 72, ipc args: [bytes4], ipc returns: [bytes4, bytes8, bytes8]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret RequestCustomBinaries();  // argc: 4, index: 73, ipc args: [bytes4, bytes1, bytes1, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetCellID(uint cellid);  // argc: 1, index: 74, ipc args: [bytes4], ipc returns: []
    /// <summary>
    /// Takes a pointer to a protobuf object and populates it with the cell list.
    /// </summary>
    public unknown_ret GetCellList([ProtobufPtrType(typeof(CMsgCellList))] IntPtr cells);  // argc: 1, index: 75, ipc args: [], ipc returns: [bytes1, protobuf]
    public string GetUserBaseFolder();  // argc: 0, index: 76, ipc args: [], ipc returns: [string]
    public bool GetUserDataFolder(ref AppId_t appid, StringBuilder buf, int bufMax);  // argc: 3, index: 77, ipc args: [bytes8, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool GetUserConfigFolder(StringBuilder buf, int bufMax);  // argc: 2, index: 78, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool GetAccountName(StringBuilder usernameOut, int strMaxLen);  // argc: 2, index: 79, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool GetAccountName(CSteamID steamid, StringBuilder usernameOut, int strMaxLen);  // argc: 4, index: 80, ipc args: [uint64, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public bool IsPasswordRemembered();  // argc: 0, index: 81, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret CheckoutSiteLicenseSeat(AppId_t appid);  // argc: 1, index: 82, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetAvailableSeats(AppId_t appid);  // argc: 1, index: 83, ipc args: [bytes4], ipc returns: []
    public string GetAssociatedSiteName();  // argc: 0, index: 84, ipc args: [], ipc returns: [string]
    public bool BIsRunningInCafe();  // argc: 0, index: 85, ipc args: [], ipc returns: [boolean]
    public bool BAllowCachedCredentialsInCafe();  // argc: 0, index: 86, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret RequiresLegacyCDKey(AppId_t appid, out bool unk);  // argc: 2, index: 87, ipc args: [bytes4], ipc returns: [bytes1, bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLegacyCDKey(AppId_t appid, StringBuilder keyData, int keyDataMax);  // argc: 3, index: 88, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret SetLegacyCDKey(AppId_t appid, string keyData);  // argc: 2, index: 89, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret WriteLegacyCDKey(AppId_t appid);  // argc: 1, index: 90, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveLegacyCDKey(AppId_t appid);  // argc: 1, index: 91, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RequestLegacyCDKeyFromApp();  // argc: 3, index: 92, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    public bool BIsAnyGameRunning();  // argc: 0, index: 93, ipc args: [], ipc returns: [boolean]
    public unknown_ret GetSteamGuardDetails();  // argc: 0, index: 94, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public uint GetSentryFileData([IPCOut] CUtlBuffer* data);  // argc: 1, index: 95, ipc args: [], ipc returns: [bytes4, bytes20]
    public unknown_ret GetTwoFactorDetails();  // argc: 0, index: 96, ipc args: [], ipc returns: []
    public bool BHasTwoFactor();  // argc: 0, index: 97, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEmail(StringBuilder email, int emailMax, out bool validated);  // argc: 3, index: 98, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes1]
    public unknown_ret Test_FakeConnectionTimeout();  // argc: 0, index: 99, ipc args: [], ipc returns: []
    public unknown_ret RunInstallScript(AppId_t appid, string unk, bool uninstall);  // argc: 3, index: 100, ipc args: [bytes4, string, bytes1], ipc returns: [bytes1]
    public AppId_t IsInstallScriptRunning();  // argc: 0, index: 101, ipc args: [], ipc returns: [bytes4]
    public bool GetInstallScriptState(ref string pchDescription, UInt32 cchDescription, ref UInt32 punNumSteps, ref UInt32 punCurrStep);  // argc: 4, index: 102, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4, bytes4]
    public unknown_ret StopInstallScript(AppId_t appid);  // argc: 1, index: 103, ipc args: [bytes4], ipc returns: [bytes1]
    /// <summary>
    /// Spawns a process with a given lpCommandLine. <br/>
    /// Kind of similar to Windows's CreateProcess, but doesn't try to mutate the args dumbly (and is cross platform). <br/>
    /// You can ignore passing in an lpApplicationName, and it will automatically infer it from lpCommandLine. <br/>
    /// And CGameID is a pointer for some reason... <br/>
    /// AND pchGameName needs to be specified manually. <br/>
    /// AND we don't know the last three flags. <br/>
    /// FiveM (the GTA mod) uses this internally, so we can copy them in certain cases. <br/>
    /// </summary>
    /// <param name="lpApplicationName"></param>
    /// <param name="lpCommandLine"></param>
    /// <param name="lpCurrentDirectory"></param>
    /// <param name="gameID"></param>
    /// <param name="pchGameName"></param>
    /// <param name="uUnk"></param>
    /// <param name="uUnk2"></param>
    /// <param name="uUnk3"></param>
    /// <returns></returns>
    // WARNING: Arguments are unknown!
    public unknown_ret SpawnProcess(string lpApplicationName, string lpCommandLine, string lpCurrentDirectory, ref CGameID pGameID, string pchGameName, UInt32 uUnk = 0, UInt32 uUnk2 = 0, UInt32 uUnk3 = 0);  // argc: 9, index: 104, ipc args: [string, string, string, bytes8, string, bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppOwnershipTicketLength(AppId_t app);  // argc: 1, index: 105, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppOwnershipTicketData();  // argc: 3, index: 106, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppOwnershipTicketExtendedData();  // argc: 7, index: 107, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_mem, bytes4, bytes4, bytes4, bytes4]
    public int GetMarketingMessageCount();  // argc: 0, index: 108, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMarketingMessage(int cMarketingMessage, out GID_t gidMarketingMessageID, StringBuilder pubMsgUrl, int cubMessageUrl, out EMarketingMessageFlags eMarketingMssageFlags);  // argc: 5, index: 109, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes8, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret MarkMarketingMessageSeen(GID_t gidMarketingMessageID);  // argc: 2, index: 110, ipc args: [bytes8], ipc returns: []
    public unknown_ret CheckForPendingMarketingMessages();  // argc: 0, index: 111, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthSessionTicket();  // argc: 3, index: 112, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthSessionTicketV2();  // argc: 4, index: 113, ipc args: [bytes4, steamnetworkingidentity], ipc returns: [bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthSessionTicketV3();  // argc: 4, index: 114, ipc args: [bytes4, steamnetworkingidentity], ipc returns: [bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthTicketForWebApi();  // argc: 1, index: 115, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAuthSessionTicketForGameID();  // argc: 5, index: 116, ipc args: [bytes4, bytes8, steamnetworkingidentity], ipc returns: [bytes4, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BeginAuthSession();  // argc: 4, index: 117, ipc args: [bytes4, uint64, bytes_length_from_mem], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret EndAuthSession();  // argc: 2, index: 118, ipc args: [uint64], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CancelAuthTicket();  // argc: 1, index: 119, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsUserSubscribedAppInTicket();  // argc: 3, index: 120, ipc args: [uint64, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AdvertiseGame(CSteamID steamIDGameServer, UInt32 unIPServer, UInt16 usPortServer);  // argc: 5, index: 121, ipc args: [bytes8, uint64, bytes4, bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public SteamAPICall_t RequestEncryptedAppTicket();  // argc: 2, index: 122, ipc args: [bytes4, bytes_length_from_mem], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEncryptedAppTicket();  // argc: 3, index: 123, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGameBadgeLevel();  // argc: 2, index: 124, ipc args: [bytes4, bytes1], ipc returns: [bytes4]
    public unknown_ret GetPlayerSteamLevel();  // argc: 0, index: 125, ipc args: [], ipc returns: [bytes4]
    /// <summary>
    /// Don't use this. It does strange things and will probably get you flagged.
    /// </summary>
    public void SetAccountLimited(bool val);  // argc: 1, index: 126, ipc args: [bytes1], ipc returns: []
    public bool BIsAccountLimited();  // argc: 0, index: 127, ipc args: [], ipc returns: [boolean]
    /// <summary>
    /// Don't use this. It does strange things and will probably get you flagged.
    /// </summary>
    public void SetAccountCommunityBanned(bool val);  // argc: 1, index: 128, ipc args: [bytes1], ipc returns: []
    public bool BIsAccountCommunityBanned();  // argc: 0, index: 129, ipc args: [], ipc returns: [boolean]
    /// <summary>
    /// Don't use this. It does strange things and will probably get you flagged.
    /// </summary>
    public void SetLimitedAccountCanInviteFriends(bool val);  // argc: 1, index: 130, ipc args: [bytes1], ipc returns: []
    public bool BLimitedAccountCanInviteFriends();  // argc: 0, index: 131, ipc args: [], ipc returns: [boolean]
    /// <summary>
    /// This function will always send an email to your account's current email address. 
    /// Even if it is already verified.
    /// </summary>
    public SteamAPICall_t SendValidationEmail();  // argc: 0, index: 132, ipc args: [], ipc returns: []
    public unknown_ret BGameConnectTokensAvailable();  // argc: 0, index: 133, ipc args: [], ipc returns: [boolean]
    public int NumGamesRunning();  // argc: 0, index: 134, ipc args: [], ipc returns: [bytes4]
    public CGameID GetRunningGameID(int index, int unk);  // argc: 2, index: 135, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public UInt32 GetRunningGamePID(int index);  // argc: 1, index: 136, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RaiseWindowForGame(CGameID gameid);  // argc: 1, index: 137, ipc args: [bytes8], ipc returns: [bytes4]
    public UInt32 GetAccountSecurityPolicyFlags();  // argc: 0, index: 138, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetClientStat();  // argc: 6, index: 139, ipc args: [bytes4, bytes8, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public SteamAPICall_t VerifyPassword(string password);  // argc: 1, index: 140, ipc args: [string], ipc returns: []
    public bool BSupportUser();  // argc: 0, index: 141, ipc args: [], ipc returns: [boolean]
    public unknown_ret BNeedsSSANextSteamLogon();  // argc: 0, index: 142, ipc args: [], ipc returns: [boolean]
    public unknown_ret ClearNeedsSSANextSteamLogon();  // argc: 0, index: 143, ipc args: [], ipc returns: []
    public bool BIsAppOverlayEnabled(CGameID gameid);  // argc: 1, index: 144, ipc args: [bytes8], ipc returns: [boolean]
    public bool BOverlayIgnoreChildProcesses(CGameID gameid);  // argc: 1, index: 145, ipc args: [bytes8], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetOverlayState(AppId_t appid, bool visible);  // argc: 2, index: 146, ipc args: [bytes8, bytes4], ipc returns: []
    public unknown_ret NotifyOverlaySettingsChanged();  // argc: 0, index: 147, ipc args: [], ipc returns: []
    public bool BIsBehindNAT();  // argc: 0, index: 148, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMicroTxnAppID(GID_t transactionId);  // argc: 2, index: 149, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMicroTxnOrderID(GID_t transactionId);  // argc: 2, index: 150, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetMicroTxnPrice(GID_t transactionId, ref CAmount unk, ref CAmount unk2, ref bool unk3, int unk4);  // argc: 6, index: 151, ipc args: [bytes8], ipc returns: [boolean, bytes12, bytes12, boolean, bytes12]
    // WARNING: Arguments are unknown!
    public ESteamRealm GetMicroTxnSteamRealm(GID_t transactionId);  // argc: 2, index: 152, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetMicroTxnLineItemCount(GID_t transactionId);  // argc: 2, index: 153, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BGetMicroTxnLineItem(GID_t transactionId, uint unk, ref int unk2, ref uint unk3, StringBuilder name, int nameMax, ref int unk4, byte* unk5, ref int unk6, ref bool unk7);  // argc: 11, index: 154, ipc args: [bytes8, bytes4, bytes4], ipc returns: [boolean, bytes12, bytes4, bytes_length_from_mem, bytes4, boolean, bytes12, boolean]
    // WARNING: Arguments are unknown!
    public bool BIsSandboxMicroTxn(GID_t transactionId, ref bool unk);  // argc: 3, index: 155, ipc args: [bytes8], ipc returns: [boolean, boolean]
    // WARNING: Arguments are unknown!
    public bool BMicroTxnRequiresCachedPmtMethod(GID_t transactionId, ref bool unk);  // argc: 3, index: 156, ipc args: [bytes8], ipc returns: [boolean, boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret AuthorizeMicroTxn(GID_t transactionId, int unk);  // argc: 3, index: 157, ipc args: [bytes8, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public bool BGetWalletBalance(out bool hasWallet, out CAmount amount, out CAmount amountPending);  // argc: 3, index: 158, ipc args: [], ipc returns: [boolean, boolean, bytes12, bytes12]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestMicroTxnInfo(GID_t transactionId);  // argc: 2, index: 159, ipc args: [bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret BMicroTxnRefundable(GID_t transactionId);  // argc: 2, index: 160, ipc args: [bytes8], ipc returns: [boolean]
    public bool BGetAppMinutesPlayed(AppId_t appid, ref UInt32 allTime, ref UInt32 lastTwoWeeks);  // argc: 3, index: 161, ipc args: [bytes4], ipc returns: [boolean, bytes4, bytes4]
    public RTime32 GetAppLastPlayedTime(AppId_t appid);  // argc: 1, index: 162, ipc args: [bytes4], ipc returns: [bytes4]
    public uint GetAppUpdateDisabledSecondsRemaining(AppId_t appid);  // argc: 1, index: 163, ipc args: [bytes4], ipc returns: [bytes4]
    public bool BGetGuideURL(AppId_t appid, StringBuilder url, int urlMax);  // argc: 3, index: 164, ipc args: [bytes4, bytes4], ipc returns: [boolean, bytes_length_from_mem]
    public bool BPromptToChangePassword();  // argc: 0, index: 165, ipc args: [], ipc returns: [boolean]
    public bool BAccountExtraSecurity();  // argc: 0, index: 166, ipc args: [], ipc returns: [boolean]
    public bool BAccountShouldShowLockUI();  // argc: 0, index: 167, ipc args: [], ipc returns: [boolean]
    public int GetCountAuthedComputers();  // argc: 0, index: 168, ipc args: [], ipc returns: [bytes4]
    public RTime32 GetSteamGuardEnabledTime();  // argc: 0, index: 169, ipc args: [], ipc returns: [bytes4]
    public void SetPhoneIsVerified(bool val);  // argc: 1, index: 170, ipc args: [bytes1], ipc returns: []
    public bool BIsPhoneVerified();  // argc: 0, index: 171, ipc args: [], ipc returns: [boolean]
    public void SetPhoneIsIdentifying(bool val);  // argc: 1, index: 172, ipc args: [bytes1], ipc returns: []
    public bool BIsPhoneIdentifying();  // argc: 0, index: 173, ipc args: [], ipc returns: [boolean]
    public void SetPhoneIsRequiringVerification(bool val);  // argc: 1, index: 174, ipc args: [bytes1], ipc returns: []
    public bool BIsPhoneRequiringVerification();  // argc: 0, index: 175, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret Set2ndFactorAuthCode(string code, bool remember);  // argc: 2, index: 176, ipc args: [string, bytes1], ipc returns: []
    public void SetUserMachineName(string name);  // argc: 1, index: 177, ipc args: [string], ipc returns: []
    public void GetUserMachineName(StringBuilder name, int len);  // argc: 2, index: 178, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public bool GetEmailDomainFromLogonFailure(StringBuilder domain, int domainMax);  // argc: 2, index: 179, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    public string GetAgreementSessionUrl();  // argc: 0, index: 180, ipc args: [], ipc returns: [string]
    public SteamAPICall_t GetDurationControl();  // argc: 0, index: 181, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public UInt64 GetDurationControlForApp(AppId_t appid);  // argc: 1, index: 182, ipc args: [bytes4], ipc returns: [bytes8]
    public bool BSetDurationControlOnlineState(EDurationControlOnlineState eNewState);  // argc: 1, index: 183, ipc args: [bytes4], ipc returns: [boolean]
    public bool BSetDurationControlOnlineStateForApp(AppId_t appid, EDurationControlOnlineState eNewState);  // argc: 2, index: 184, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BGetDurationControlExtendedResults();  // argc: 3, index: 185, ipc args: [bytes4], ipc returns: [boolean, boolean, boolean]
    /// <summary>
    /// Checks if the active user owns a specific app.
    /// </summary>
    /// <param name="appid">AppID to check</param>
    /// <returns>Whether user owns game or not</returns>
    public bool BIsSubscribedApp(AppId_t appid);  // argc: 1, index: 186, ipc args: [bytes4], ipc returns: [boolean]
    /// <summary>
    /// Gets a list of all appid's the current user owns.
    /// </summary>
    /// <param name="arr">The preallocated array to populate</param>
    /// <param name="lengthOfArr">The length of the preallocated array</param>
    /// <param name="unk">Unknown...</param>
    /// <returns>How many apps the user owns</returns>
    public uint GetSubscribedApps(uint[] arr, uint lengthOfArr, bool unk);  // argc: 3, index: 187, ipc args: [bytes4, bytes1], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret AckSystemIM(GID_t id);  // argc: 2, index: 188, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RequestSpecialSurvey(uint surveyId);  // argc: 1, index: 189, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SendSpecialSurveyResponse(uint surveyId, byte[] data, int dataLength);  // argc: 3, index: 190, ipc args: [bytes4, bytes4, bytes_length_from_mem], ipc returns: [bytes8]
    public unknown_ret RequestNotifications();  // argc: 0, index: 191, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appid"></param>
    /// <param name="timeCreated"></param>
    /// <param name="country">Use a 3 byte buffer for the country</param>
    /// <returns></returns>
    public unknown_ret GetAppOwnershipInfo(AppId_t appid, out RTime32 timeCreated, out UInt32 unk, StringBuilder country);  // argc: 4, index: 192, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes3]
    // WARNING: Arguments are unknown!
    public unknown_ret SendGameWebCallback(AppId_t appid, string data);  // argc: 2, index: 193, ipc args: [bytes4, string], ipc returns: []
    public bool BIsStreamingUIToRemoteDevice();  // argc: 0, index: 194, ipc args: [], ipc returns: [boolean]
    public bool BIsCurrentlyNVStreaming();  // argc: 0, index: 195, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret OnBigPictureForStreamingStartResult();  // argc: 2, index: 196, ipc args: [bytes1, bytes4], ipc returns: []
    public unknown_ret OnBigPictureForStreamingDone();  // argc: 0, index: 197, ipc args: [], ipc returns: []
    public unknown_ret OnBigPictureForStreamingRestarting();  // argc: 0, index: 198, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StopStreaming(AppId_t appid);  // argc: 1, index: 199, ipc args: [bytes4], ipc returns: []
    public unknown_ret LockParentalLock();  // argc: 0, index: 200, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UnlockParentalLock(string unk);  // argc: 1, index: 201, ipc args: [string], ipc returns: [bytes1]
    public bool BIsParentalLockEnabled();  // argc: 0, index: 202, ipc args: [], ipc returns: [boolean]
    public bool BIsParentalLockLocked();  // argc: 0, index: 203, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BlockApp(AppId_t appid);  // argc: 1, index: 204, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UnblockApp(AppId_t appid);  // argc: 1, index: 205, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool BIsAppBlocked(AppId_t appid);  // argc: 1, index: 206, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BIsAppInBlockList(AppId_t appid);  // argc: 1, index: 207, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BlockFeature(EParentalFeature eParentalFeature);  // argc: 1, index: 208, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UnblockFeature(EParentalFeature eParentalFeature);  // argc: 1, index: 209, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public bool BIsFeatureBlocked(EParentalFeature eParentalFeature);  // argc: 1, index: 210, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool BIsFeatureInBlockList(EParentalFeature eParentalFeature);  // argc: 1, index: 211, ipc args: [bytes4], ipc returns: [boolean]
    public unknown_ret GetParentalUnlockTime();  // argc: 0, index: 212, ipc args: [], ipc returns: [bytes4]
    public bool BGetRecoveryEmail(StringBuilder email, uint strMaxLen);  // argc: 2, index: 213, ipc args: [bytes4], ipc returns: [boolean, bytes_length_from_mem]
    public SteamAPICall_t RequestParentalRecoveryEmail();  // argc: 0, index: 214, ipc args: [], ipc returns: []
    public bool BIsLockFromSiteLicense();  // argc: 0, index: 215, ipc args: [], ipc returns: [boolean]
    public unknown_ret EIsParentalPlaytimeBlocked();  // argc: 0, index: 216, ipc args: [], ipc returns: [bytes4]
    public bool BGetSerializedParentalSettings([IPCOut] CUtlBuffer* serialized);  // argc: 1, index: 217, ipc args: [], ipc returns: [boolean, utlbuffer]
    public bool BSetParentalSettings([IPCIn] CUtlBuffer* serialized);  // argc: 1, index: 218, ipc args: [unknown], ipc returns: [boolean]
    public bool BDisableParentalSettings();  // argc: 0, index: 219, ipc args: [], ipc returns: [boolean]
    public bool BGetParentalWebToken([IPCOut] CUtlBuffer* unk1, [IPCOut] CUtlBuffer* unk2);  // argc: 2, index: 220, ipc args: [], ipc returns: [boolean, utlbuffer, utlbuffer]
    /// <summary>
    /// Only preference 0 seems to exist.
    /// </summary>
    public uint GetCommunityPreference(int preference);  // argc: 1, index: 221, ipc args: [bytes4], ipc returns: [bytes1]
    /// <summary>
    /// Only preference 0 seems to exist.
    /// </summary>
    public void SetCommunityPreference(int preference, uint value);  // argc: 2, index: 222, ipc args: [bytes4, bytes1], ipc returns: []
    public uint GetTextFilterSetting();  // argc: 0, index: 223, ipc args: [], ipc returns: [bytes4]
    public bool BTextFilterIgnoresFriends();  // argc: 0, index: 224, ipc args: [], ipc returns: [boolean]
    /// <summary>
    /// Can return 0, 1 or 2 or a mystery uint. Probably is an enum of some sort.
    /// </summary>
    public unknown_ret CanLogonOffline();  // argc: 0, index: 225, ipc args: [], ipc returns: [bytes4]
    /// <summary>
    /// Returns 19, same mystery uint as CanLogonOffline or 1
    /// Also unk1 only seems to affect the callback that gets posted.
    /// </summary>
    public unknown_ret LogOnOffline(byte unk1);  // argc: 1, index: 226, ipc args: [bytes1], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ValidateOfflineLogonTicket();  // argc: 1, index: 227, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public bool BGetOfflineLogonTicket(StringBuilder ticket, int ticketMax);  // argc: 2, index: 228, ipc args: [string], ipc returns: [boolean, protobuf]
    public SteamAPICall_t UploadLocalClientLogs();  // argc: 0, index: 229, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public void SetAsyncNotificationEnabled(AppId_t appid, bool val);  // argc: 2, index: 230, ipc args: [bytes4, bytes1], ipc returns: []
    public bool BIsOtherSessionPlaying(ref UInt32 accountID);  // argc: 1, index: 231, ipc args: [], ipc returns: [boolean, bytes4]
    public bool BKickOtherPlayingSession();  // argc: 0, index: 232, ipc args: [], ipc returns: [boolean]
    public bool BIsAccountLockedDown();  // argc: 0, index: 233, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearAndSetAppTags();  // argc: 2, index: 234, ipc args: [bytes8, utlvector], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveAppTag();  // argc: 2, index: 235, ipc args: [bytes8, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AddAppTag();  // argc: 2, index: 236, ipc args: [bytes8, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ClearAppTags();  // argc: 1, index: 237, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetAppHidden();  // argc: 2, index: 238, ipc args: [bytes8, bytes1], ipc returns: []
    public SteamAPICall_t RequestAccountLinkInfo();  // argc: 0, index: 239, ipc args: [], ipc returns: [bytes8]
    public unknown_ret RequestSurveySchedule();  // argc: 0, index: 240, ipc args: [], ipc returns: []
    public unknown_ret RequestNewSteamAnnouncementState();  // argc: 0, index: 241, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateSteamAnnouncementLastRead();  // argc: 3, index: 242, ipc args: [bytes8, bytes4], ipc returns: []
    public unknown_ret GetMarketEligibility();  // argc: 0, index: 243, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateGameVrDllState(CGameID gameid, bool unk, bool unk2);  // argc: 3, index: 244, ipc args: [bytes8, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret KillVRTheaterPancakeGame(CGameID gameid);  // argc: 1, index: 245, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetVRIsHMDAwake();  // argc: 1, index: 246, ipc args: [bytes1], ipc returns: []
    public bool BIsAnyGameOrServiceAppRunning();  // argc: 0, index: 247, ipc args: [], ipc returns: [boolean]
    [BlacklistedInCrossProcessIPC]
    public bool BGetAppPlaytimeMap(CUtlMap<AppId_t, AppPlaytime_t>* vec);  // argc: 1, index: 248, ipc args: [bytes4], ipc returns: [boolean]
    [BlacklistedInCrossProcessIPC]
    public bool BGetAppsLastPlayedMap(CUtlMap<AppId_t, RTime32>* vec);  // argc: 1, index: 249, ipc args: [bytes4], ipc returns: [boolean]
    /// <summary>
    /// App tags were the old version of the "categories" system in place now.
    /// These are stored in sharedconfig.vdf in Software/Valve/Steam/Apps/*/tags
    /// Don't use this if wanting to get categories.
    /// The new category system is stored here:
    /// https://store.steampowered.com/account/userconfigstore
    /// To get it, you can roll your own cloud config manager or use OpenSteamworks.Client
    /// </summary>
    [BlacklistedInCrossProcessIPC]
    public bool BGetAppTagsMap(CUtlMap<AppId_t, AppTags_t>* vec);  // argc: 1, index: 250, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SendSteamServiceStatusUpdate(EResult unk1, ESteamServiceStatusUpdate unk2);  // argc: 2, index: 251, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public EResult RequestSteamGroupChatMessageNotifications();  // argc: 5, index: 252, ipc args: [bytes8, bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestSteamGroupChatMessageHistory();  // argc: 5, index: 253, ipc args: [bytes8, bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestSendSteamGroupChatMessage();  // argc: 6, index: 254, ipc args: [bytes8, bytes8, bytes4, string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret OnNewGroupChatMsgAdded();  // argc: 8, index: 255, ipc args: [bytes8, bytes8, bytes4, bytes4, bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OnGroupChatUserStateChange();  // argc: 4, index: 256, ipc args: [bytes8, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OnReceivedGroupChatSubscriptionResponse();  // argc: 5, index: 257, ipc args: [bytes8, bytes8, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetTimedTrialStatus();  // argc: 4, index: 258, ipc args: [bytes4], ipc returns: [bytes1, bytes1, bytes4, bytes4]
    public SteamAPICall_t RequestTimedTrialStatus(AppId_t appid);  // argc: 1, index: 259, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret PrepareForSystemSuspend();  // argc: 0, index: 260, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ResumeSuspendedGames();  // argc: 1, index: 261, ipc args: [bytes1], ipc returns: [bytes8]
    public UInt64 GetClientInstallationID();  // argc: 0, index: 262, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret Test_SetClientInstallationID(UInt64 id);  // argc: 2, index: 263, ipc args: [bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public AppId_t GetAppIDForGameID(in CGameID gameid);  // argc: 1, index: 264, ipc args: [bytes8], ipc returns: [bytes4]
    public bool BDoNotDisturb();  // argc: 0, index: 265, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetAdditionalClientArgData(string data);  // argc: 1, index: 266, ipc args: [bytes5], ipc returns: []
    public unknown_ret GetFamilyGroupID();  // argc: 0, index: 267, ipc args: [], ipc returns: [bytes8]
    public unknown_ret GetFamilyGroupName();  // argc: 0, index: 268, ipc args: [], ipc returns: [string]
    public unknown_ret GetFamilyGroupRole();  // argc: 0, index: 269, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetFamilyGroupMembers();  // argc: 2, index: 270, ipc args: [bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret NotifyPendingGameLaunch_FetchSteamStreamingEncoderConfig();  // argc: 1, index: 271, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BShouldWaitForSteamStreamingEncoderConfig();  // argc: 1, index: 272, ipc args: [bytes4], ipc returns: [boolean]
}