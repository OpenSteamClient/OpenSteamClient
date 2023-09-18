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
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Generated;

public unsafe interface IClientUser
{
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_0_DONTUSE();  // argc: -1, index: 1
    public EResult LogOn(CSteamID steamid, bool interactive = true);  // argc: 2, index: 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret InvalidateCredentials();  // argc: 2, index: 3
    public void LogOff();  // argc: 0, index: 4
    public bool BLoggedOn();  // argc: 0, index: 5
    public ELogonState GetLogonState();  // argc: 0, index: 6
    public bool BConnected();  // argc: 0, index: 7
    public bool BInitiateReconnect();  // argc: 0, index: 8
    public unknown_ret EConnect();  // argc: 0, index: 9
    public unknown_ret BTryingToLogin();  // argc: 0, index: 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public CSteamID GetSteamID();  // argc: 1, index: 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetConsoleSteamID();  // argc: 1, index: 12
    public unknown_ret GetClientInstanceID();  // argc: 0, index: 13
    public string GetUserCountry();  // argc: 0, index: 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret IsVACBanned();  // argc: 1, index: 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetEmail();  // argc: 1, index: 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetConfigString();  // argc: 3, index: 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetConfigString(int eRegistrySubTree, string key, StringBuilder stringOut, Int32 stringOutMax);  // argc: 4, index: 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetConfigInt();  // argc: 3, index: 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetConfigInt();  // argc: 3, index: 20
    public unknown_ret SetConfigBinaryBlob(int eRegistrySubTree, string key, CUtlBuffer* buf);  // argc: 3, index: 21
    public unknown_ret GetConfigBinaryBlob(int eRegistrySubTree, string key, CUtlBuffer* buf);  // argc: 3, index: 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret DeleteConfigKey();  // argc: 2, index: 23
    public unknown_ret GetConfigStoreKeyName(int eRegistrySubTree, string pchKey, StringBuilder pchStoreName, Int32 cbStoreName);  // argc: 4, index: 24
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret InitiateGameConnection();  // argc: 8, index: 25
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret InitiateGameConnectionOld();  // argc: 10, index: 26
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret TerminateGameConnection();  // argc: 2, index: 27
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret TerminateGame();  // argc: 2, index: 28
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetSelfAsChatDestination();  // argc: 1, index: 29
    public unknown_ret IsPrimaryChatDestination();  // argc: 0, index: 30
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestLegacyCDKey();  // argc: 1, index: 31
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret AckGuestPass();  // argc: 1, index: 32
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RedeemGuestPass();  // argc: 1, index: 33
    public unknown_ret GetGuestPassToGiveCount();  // argc: 0, index: 34
    public unknown_ret GetGuestPassToRedeemCount();  // argc: 0, index: 35
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetGuestPassToGiveInfo();  // argc: 9, index: 36
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetGuestPassToGiveOut();  // argc: 1, index: 37
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetGuestPassToRedeem();  // argc: 1, index: 38
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetGuestPassToRedeemInfo();  // argc: 7, index: 39
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetGuestPassToRedeemSenderName();  // argc: 3, index: 40
    public unknown_ret GetNumAppsInGuestPassesToRedeem();  // argc: 0, index: 41
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppsInGuestPassesToRedeem();  // argc: 2, index: 42
    public unknown_ret GetCountUserNotifications();  // argc: 0, index: 43
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetCountUserNotification();  // argc: 1, index: 44
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestStoreAuthURL();  // argc: 1, index: 45
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetLanguage(string language);  // argc: 1, index: 46
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret TrackAppUsageEvent();  // argc: 3, index: 47
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RaiseConnectionPriority();  // argc: 2, index: 48
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ResetConnectionPriority();  // argc: 1, index: 49
    public unknown_ret GetDesiredNetQOSLevel();  // argc: 0, index: 50
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BHasCachedCredentials(string username);  // argc: 1, index: 51
    public bool SetAccountNameForCachedCredentialLogin(string username, bool unk1);  // argc: 2, index: 52
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret DestroyCachedCredentials(string username, int revokeAction = (int)Protobuf.EAuthTokenRevokeAction.EauthTokenRevokeLogout);  // argc: 2, index: 53
    public bool GetCurrentWebAuthToken(StringBuilder tokenOut, UInt32 bufSize);  // argc: 2, index: 54
    public unknown_ret RequestWebAuthToken();  // argc: 0, index: 55
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetLoginInformation(string username, string password, bool remember);  // argc: 3, index: 56
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetTwoFactorCode(string twoFactorCode);  // argc: 1, index: 57
    public unknown_ret SetLoginToken(string token, string username);  // argc: 2, index: 58
    public UInt64 GetLoginTokenID();  // argc: 0, index: 59
    public unknown_ret ClearAllLoginInformation();  // argc: 0, index: 60
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BEnableEmbeddedClient();  // argc: 1, index: 61
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ResetEmbeddedClient();  // argc: 1, index: 62
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BHasEmbeddedClientToken();  // argc: 1, index: 63
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestEmbeddedClientToken();  // argc: 1, index: 64
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret AuthorizeNewDevice();  // argc: 3, index: 65
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetLanguage();  // argc: 2, index: 66
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret TrackNatTraversalStat();  // argc: 1, index: 67
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret TrackSteamUsageEvent();  // argc: 3, index: 68
    public unknown_ret SetComputerInUse();  // argc: 0, index: 69
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BIsGameRunning();  // argc: 1, index: 70
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BIsGameWindowReady();  // argc: 1, index: 71
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BUpdateAppOwnershipTicket(uint unk1, int pTicket);  // argc: 2, index: 72
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetCustomBinariesState();  // argc: 3, index: 73
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestCustomBinaries();  // argc: 4, index: 74
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetCellID();  // argc: 1, index: 75
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetCellList();  // argc: 1, index: 76
    public unknown_ret GetUserBaseFolder();  // argc: 0, index: 77
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetUserDataFolder();  // argc: 3, index: 78
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetUserConfigFolder();  // argc: 2, index: 79
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAccountName(StringBuilder usernameOut, uint strMaxLen);  // argc: 2, index: 80
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAccountName();  // argc: 4, index: 81
    public unknown_ret IsPasswordRemembered();  // argc: 0, index: 82
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret CheckoutSiteLicenseSeat();  // argc: 1, index: 83
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAvailableSeats();  // argc: 1, index: 84
    public unknown_ret GetAssociatedSiteName();  // argc: 0, index: 85
    public unknown_ret BIsRunningInCafe();  // argc: 0, index: 86
    public unknown_ret BAllowCachedCredentialsInCafe();  // argc: 0, index: 87
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequiresLegacyCDKey();  // argc: 2, index: 88
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetLegacyCDKey();  // argc: 3, index: 89
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetLegacyCDKey();  // argc: 2, index: 90
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret WriteLegacyCDKey();  // argc: 1, index: 91
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RemoveLegacyCDKey();  // argc: 1, index: 92
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestLegacyCDKeyFromApp();  // argc: 3, index: 93
    public unknown_ret BIsAnyGameRunning();  // argc: 0, index: 94
    public unknown_ret GetSteamGuardDetails();  // argc: 0, index: 95
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public uint GetSentryFileData(CUtlBuffer* data);  // argc: 1, index: 96
    public unknown_ret GetTwoFactorDetails();  // argc: 0, index: 97
    public bool BHasTwoFactor();  // argc: 0, index: 98
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetEmail();  // argc: 3, index: 99
    public unknown_ret Test_FakeConnectionTimeout();  // argc: 0, index: 100
    public unknown_ret RunInstallScript(AppId_t appid, string unk, bool uninstall);  // argc: 3, index: 101
    public AppId_t IsInstallScriptRunning();  // argc: 0, index: 102
    public bool GetInstallScriptState(ref string pchDescription, UInt32 cchDescription, ref UInt32 punNumSteps, ref UInt32 punCurrStep);  // argc: 4, index: 103
    public unknown_ret StopInstallScript(AppId_t appid);  // argc: 1, index: 104
    public unknown_ret SpawnProcess(string lpApplicationName, string lpCommandLine, string lpCurrentDirectory, CGameID gameID, string pchGameName, UInt32 uUnk, UInt32 uUnk2, UInt32 uUnk3);  // argc: 9, index: 105
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppOwnershipTicketLength(AppId_t app);  // argc: 1, index: 106
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppOwnershipTicketData();  // argc: 3, index: 107
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppOwnershipTicketExtendedData();  // argc: 7, index: 108
    public unknown_ret GetMarketingMessageCount();  // argc: 0, index: 109
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetMarketingMessage();  // argc: 5, index: 110
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret MarkMarketingMessageSeen();  // argc: 2, index: 111
    public unknown_ret CheckForPendingMarketingMessages();  // argc: 0, index: 112
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAuthSessionTicket();  // argc: 3, index: 113
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAuthSessionTicketV2();  // argc: 4, index: 114
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAuthSessionTicketV3();  // argc: 4, index: 115
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAuthTicketForWebApi();  // argc: 1, index: 116
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAuthSessionTicketForGameID();  // argc: 5, index: 117
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BeginAuthSession();  // argc: 4, index: 118
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret EndAuthSession();  // argc: 2, index: 119
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret CancelAuthTicket();  // argc: 1, index: 120
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret IsUserSubscribedAppInTicket();  // argc: 3, index: 121
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret AdvertiseGame();  // argc: 5, index: 122
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestEncryptedAppTicket();  // argc: 2, index: 123
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetEncryptedAppTicket();  // argc: 3, index: 124
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetGameBadgeLevel();  // argc: 2, index: 125
    public unknown_ret GetPlayerSteamLevel();  // argc: 0, index: 126
    /// <summary>
    /// Don't use this. It does strange things and will probably get you flagged.
    /// </summary>
    public void SetAccountLimited(bool val);  // argc: 1, index: 127
    public bool BIsAccountLimited();  // argc: 0, index: 128
    /// <summary>
    /// Don't use this. It does strange things and will probably get you flagged.
    /// </summary>
    public void SetAccountCommunityBanned(bool val);  // argc: 1, index: 129
    public bool BIsAccountCommunityBanned();  // argc: 0, index: 130
    /// <summary>
    /// Don't use this. It does strange things and will probably get you flagged.
    /// </summary>
    public void SetLimitedAccountCanInviteFriends(bool val);  // argc: 1, index: 131
    public bool BLimitedAccountCanInviteFriends();  // argc: 0, index: 132
    /// <summary>
    /// This function will always send an email to your account's current email address. 
    /// Even if it is already verified.
    /// </summary>
    public SteamAPICall_t SendValidationEmail();  // argc: 0, index: 133
    public unknown_ret BGameConnectTokensAvailable();  // argc: 0, index: 134
    public int NumGamesRunning();  // argc: 0, index: 135
    public CGameID GetRunningGameID(int index, int unk);  // argc: 2, index: 136
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetRunningGamePID();  // argc: 1, index: 137
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RaiseWindowForGame();  // argc: 1, index: 138
    public unknown_ret GetAccountSecurityPolicyFlags();  // argc: 0, index: 139
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetClientStat();  // argc: 6, index: 140
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public SteamAPICall_t VerifyPassword(string password);  // argc: 1, index: 141
    public unknown_ret BSupportUser();  // argc: 0, index: 142
    public unknown_ret BNeedsSSANextSteamLogon();  // argc: 0, index: 143
    public unknown_ret ClearNeedsSSANextSteamLogon();  // argc: 0, index: 144
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BIsAppOverlayEnabled();  // argc: 1, index: 145
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BOverlayIgnoreChildProcesses();  // argc: 1, index: 146
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetOverlayState();  // argc: 2, index: 147
    public unknown_ret NotifyOverlaySettingsChanged();  // argc: 0, index: 148
    public unknown_ret BIsBehindNAT();  // argc: 0, index: 149
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetMicroTxnAppID();  // argc: 2, index: 150
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetMicroTxnOrderID();  // argc: 2, index: 151
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetMicroTxnPrice();  // argc: 6, index: 152
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetMicroTxnSteamRealm();  // argc: 2, index: 153
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetMicroTxnLineItemCount();  // argc: 2, index: 154
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetMicroTxnLineItem();  // argc: 11, index: 155
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BIsSandboxMicroTxn();  // argc: 3, index: 156
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BMicroTxnRequiresCachedPmtMethod();  // argc: 3, index: 157
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret AuthorizeMicroTxn();  // argc: 3, index: 158
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetWalletBalance();  // argc: 3, index: 159
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestMicroTxnInfo();  // argc: 2, index: 160
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BMicroTxnRefundable();  // argc: 2, index: 161
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BGetAppMinutesPlayed(AppId_t appid, ref UInt32 allTime, ref UInt32 lastTwoWeeks);  // argc: 3, index: 162
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public RTime32 GetAppLastPlayedTime(AppId_t appid);  // argc: 1, index: 163
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public uint GetAppUpdateDisabledSecondsRemaining(AppId_t appid);  // argc: 1, index: 164
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetGuideURL();  // argc: 3, index: 165
    public unknown_ret BPromptToChangePassword();  // argc: 0, index: 166
    public unknown_ret BAccountExtraSecurity();  // argc: 0, index: 167
    public unknown_ret BAccountShouldShowLockUI();  // argc: 0, index: 168
    public unknown_ret GetCountAuthedComputers();  // argc: 0, index: 169
    public unknown_ret GetSteamGuardEnabledTime();  // argc: 0, index: 170
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetPhoneIsVerified();  // argc: 1, index: 171
    public unknown_ret BIsPhoneVerified();  // argc: 0, index: 172
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetPhoneIsIdentifying();  // argc: 1, index: 173
    public unknown_ret BIsPhoneIdentifying();  // argc: 0, index: 174
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetPhoneIsRequiringVerification();  // argc: 1, index: 175
    public unknown_ret BIsPhoneRequiringVerification();  // argc: 0, index: 176
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ChangeTwoFactorAuthOptions();  // argc: 1, index: 177
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret Set2ndFactorAuthCode(string code, bool remember);  // argc: 2, index: 178
    public void SetUserMachineName(string name);  // argc: 1, index: 179
    public void GetUserMachineName(char* name, int len);  // argc: 2, index: 180
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetEmailDomainFromLogonFailure();  // argc: 2, index: 181
    public unknown_ret GetAgreementSessionUrl();  // argc: 0, index: 182
    public unknown_ret GetDurationControl();  // argc: 0, index: 183
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetDurationControlForApp();  // argc: 1, index: 184
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BSetDurationControlOnlineState();  // argc: 1, index: 185
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BSetDurationControlOnlineStateForApp();  // argc: 2, index: 186
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetDurationControlExtendedResults();  // argc: 3, index: 187
    /// <summary>
    /// Checks if the active user owns a specific app.
    /// </summary>
    /// <param name="appid">AppID to check</param>
    /// <returns>Whether user owns game or not</returns>
    public bool BIsSubscribedApp(AppId_t appid);  // argc: 1, index: 188
    /// <summary>
    /// Gets a list of all appid's the current user owns.
    /// </summary>
    /// <param name="arr">The preallocated array to populate</param>
    /// <param name="lengthOfArr">The length of the preallocated array</param>
    /// <param name="unk">Unknown...</param>
    /// <returns>How many apps the user owns</returns>
    public uint GetSubscribedApps(AppId_t[] arr, uint lengthOfArr, bool unk);  // argc: 3, index: 189
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret AckSystemIM();  // argc: 2, index: 190
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestSpecialSurvey(uint unk);  // argc: 1, index: 191
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SendSpecialSurveyResponse();  // argc: 3, index: 192
    public unknown_ret RequestNotifications();  // argc: 0, index: 193
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppOwnershipInfo();  // argc: 4, index: 194
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SendGameWebCallback();  // argc: 2, index: 195
    public unknown_ret BIsStreamingUIToRemoteDevice();  // argc: 0, index: 196
    public unknown_ret BIsCurrentlyNVStreaming();  // argc: 0, index: 197
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret OnBigPictureForStreamingStartResult();  // argc: 2, index: 198
    public unknown_ret OnBigPictureForStreamingDone();  // argc: 0, index: 199
    public unknown_ret OnBigPictureForStreamingRestarting();  // argc: 0, index: 200
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret StopStreaming();  // argc: 1, index: 201
    public unknown_ret LockParentalLock();  // argc: 0, index: 202
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UnlockParentalLock();  // argc: 1, index: 203
    public unknown_ret BIsParentalLockEnabled();  // argc: 0, index: 204
    public unknown_ret BIsParentalLockLocked();  // argc: 0, index: 205
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BlockApp();  // argc: 1, index: 206
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UnblockApp();  // argc: 1, index: 207
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BIsAppBlocked();  // argc: 1, index: 208
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BIsAppInBlockList();  // argc: 1, index: 209
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BlockFeature();  // argc: 1, index: 210
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UnblockFeature();  // argc: 1, index: 211
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BIsFeatureBlocked();  // argc: 1, index: 212
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public bool BIsFeatureInBlockList();  // argc: 1, index: 213
    public unknown_ret GetParentalUnlockTime();  // argc: 0, index: 214
    public bool BGetRecoveryEmail(StringBuilder email, uint strMaxLen);  // argc: 2, index: 215
    public unknown_ret RequestParentalRecoveryEmail();  // argc: 0, index: 216
    public bool BIsLockFromSiteLicense();  // argc: 0, index: 217
    public bool BGetSerializedParentalSettings(CUtlBuffer* serialized);  // argc: 1, index: 218
    public bool BSetParentalSettings(CUtlBuffer* serialized);  // argc: 1, index: 219
    public bool BDisableParentalSettings();  // argc: 0, index: 220
    public bool BGetParentalWebToken(StringBuilder tokenOut, uint strMaxLen);  // argc: 2, index: 221
    /// <summary>
    /// Only preference 0 seems to exist.
    /// </summary>
    public uint GetCommunityPreference(int preference);  // argc: 1, index: 222
    /// <summary>
    /// Only preference 0 seems to exist.
    /// </summary>
    public void SetCommunityPreference(int preference, uint value);  // argc: 2, index: 223
    public uint GetTextFilterSetting();  // argc: 0, index: 224
    public bool BTextFilterIgnoresFriends();  // argc: 0, index: 225
    /// <summary>
    /// Can return 0, 1 or 2 or a mystery uint. Probably is an enum of some sort.
    /// </summary>
    public unknown_ret CanLogonOffline();  // argc: 0, index: 226
    /// <summary>
    /// Returns 19, same mystery uint as CanLogonOffline or 1
    /// Also unk1 only seems to affect the callback that gets posted.
    /// </summary>
    public unknown_ret LogOnOffline(byte unk1);  // argc: 1, index: 227
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ValidateOfflineLogonTicket();  // argc: 1, index: 228
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BGetOfflineLogonTicket();  // argc: 2, index: 229
    public unknown_ret UploadLocalClientLogs();  // argc: 0, index: 230
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetAsyncNotificationEnabled();  // argc: 2, index: 231
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret BIsOtherSessionPlaying();  // argc: 1, index: 232
    public unknown_ret BKickOtherPlayingSession();  // argc: 0, index: 233
    public unknown_ret BIsAccountLockedDown();  // argc: 0, index: 234
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ClearAndSetAppTags();  // argc: 2, index: 235
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RemoveAppTag();  // argc: 2, index: 236
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret AddAppTag();  // argc: 2, index: 237
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ClearAppTags();  // argc: 1, index: 238
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetAppHidden();  // argc: 2, index: 239
    public SteamAPICall_t RequestAccountLinkInfo();  // argc: 0, index: 240
    public unknown_ret RequestSurveySchedule();  // argc: 0, index: 241
    public unknown_ret RequestNewSteamAnnouncementState();  // argc: 0, index: 242
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UpdateSteamAnnouncementLastRead();  // argc: 3, index: 243
    public unknown_ret GetMarketEligibility();  // argc: 0, index: 244
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UpdateGameVrDllState();  // argc: 3, index: 245
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret KillVRTheaterPancakeGame();  // argc: 1, index: 246
    public bool BIsAnyGameOrServiceAppRunning();  // argc: 0, index: 247
    public bool BGetAppPlaytimeMap(CUtlMap<AppId_t, AppPlaytime_t>* vec);  // argc: 1, index: 248
    public bool BGetAppsLastPlayedMap(CUtlMap<AppId_t, RTime32>* vec);  // argc: 1, index: 249
    /// <summary>
    /// App tags were the old version of the "categories" system in place now.
    /// These are stored in sharedconfig.vdf in Software/Valve/Steam/Apps/*/tags
    /// Don't use this if wanting to get categories.
    /// The new category system is stored here:
    /// https://store.steampowered.com/account/userconfigstore
    /// As for how to get it programmatically, I have no idea.
    /// </summary>
    public bool BGetAppTagsMap(CUtlMap<AppId_t, AppTags_t>* vec);  // argc: 1, index: 250
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SendSteamServiceStatusUpdate();  // argc: 2, index: 251
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestSteamGroupChatMessageNotifications();  // argc: 5, index: 252
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestSteamGroupChatMessageHistory();  // argc: 5, index: 253
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestSendSteamGroupChatMessage();  // argc: 6, index: 254
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret OnNewGroupChatMsgAdded();  // argc: 8, index: 255
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret OnGroupChatUserStateChange();  // argc: 4, index: 256
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret OnReceivedGroupChatSubscriptionResponse();  // argc: 5, index: 257
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetTimedTrialStatus();  // argc: 4, index: 258
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret RequestTimedTrialStatus();  // argc: 1, index: 259
    public unknown_ret PrepareForSystemSuspend();  // argc: 0, index: 260
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret ResumeSuspendedGames();  // argc: 1, index: 261
    public unknown_ret GetClientInstallationID();  // argc: 0, index: 262
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret Test_SetClientInstallationID();  // argc: 2, index: 263
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret GetAppIDForGameID();  // argc: 1, index: 264
    public unknown_ret BDoNotDisturb();  // argc: 0, index: 265
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetAdditionalClientArgData();  // argc: 1, index: 266
}