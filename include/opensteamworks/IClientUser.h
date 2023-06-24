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

#ifndef ICLIENTUSER_H
#define ICLIENTUSER_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "UserCommon.h"
#include "ContentServerCommon.h"

// Protobuf'ed class
class ClientAppInfo
#ifdef _S4N_
{
	int m_iPadding;
}
#endif
;

enum EParentalFeature
{
	k_EParentalFeatureInvalid = 0,
	k_EParentalFeatureStore = 1,
	k_EParentalFeatureCommunity = 2,
	k_EParentalFeatureProfile = 3,
	k_EParentalFeatureFriends = 4,
	k_EParentalFeatureNews = 5,
	k_EParentalFeatureTrading = 6,
	k_EParentalFeatureSettings = 7,
	k_EParentalFeatureConsole = 8,
	k_EParentalFeatureBrowser = 9,
	k_EParentalFeatureParentalSetup = 10,
	k_EParentalFeatureLibrary= 11,
	k_EParentalFeatureTest = 12,
};

// Protobuf, see steammessages_offline.steamclient.proto
class COffline_OfflineLogonTicket
#ifdef _S4N_
{
	int m_iPadding;
}
#endif
;

// No idea what goes here
// Ended up using UserRoamingConfigStore
struct AppTag {
    uint32_t unk1;
    uint32_t unk2;
    uint32_t unk3;
    uint32_t unk4;
    uint32_t unk11;
    uint32_t unk21;
    uint32_t unk31;
    uint32_t unk41;
    uint32_t unk12;
    uint32_t unk22;
    uint32_t unk32;
    uint32_t unk42;
    uint32_t unk13;
    uint32_t unk23;
    uint32_t unk33;
    uint32_t unk43;
    uint32_t unk14;
    uint32_t unk24;
    uint32_t unk34;
    uint32_t unk44;
    uint32_t unk15;
    uint32_t unk25;
    uint32_t unk35;
    uint32_t unk45;
};

// Should be 8 long
struct AppTags_t
{
public:
    AppTag *categories;
    uint32_t unk1;
};

abstract_class IClientUser
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_0_DONTUSE() = 0; //argc: -1, index 1
    virtual unknown_ret LogOn(CSteamID steamid, bool) = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InvalidateCredentials() = 0; //argc: 2, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LogOff() = 0; //argc: 0, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool BLoggedOn() = 0; //argc: 0, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetLogonState() = 0; //argc: 0, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BConnected() = 0; //argc: 0, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool BInitiateReconnect() = 0; //argc: 0, index 8
    // Returns an enum. What are it's values?
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EConnect() = 0; //argc: 0, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool BTryingToLogin() = 0; //argc: 0, index 10
    virtual CSteamID GetSteamID(const char *username) = 0; //argc: 1, index 11
    // WARNING: unknown arguments
    virtual CSteamID GetConsoleSteamID(const char* pszUsername) = 0; //argc: 1, index 12
    // WARNING: retval is pulled from a hat
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual char* GetClientInstanceID() = 0; //argc: 0, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetUserCountry() = 0; //argc: 0, index 14
    // WARNING: untested
    virtual bool IsVACBanned(CSteamID steamid) = 0; //argc: 1, index 15
    virtual void SetEmail() = 0; //argc: 1, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetConfigString() = 0; //argc: 3, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigString() = 0; //argc: 4, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetConfigInt() = 0; //argc: 3, index 19
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigInt() = 0; //argc: 3, index 20
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetConfigBinaryBlob() = 0; //argc: 3, index 21
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigBinaryBlob() = 0; //argc: 3, index 22
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DeleteConfigKey() = 0; //argc: 2, index 23
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigStoreKeyName() = 0; //argc: 4, index 24
    // WARNING: unknown arguments
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void InitiateGameConnection() = 0; //argc: 8, index 25
    // WARNING: unknown arguments
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void InitiateGameConnectionOld() = 0; //argc: 10, index 26
    // WARNING: unknown arguments
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TerminateGameConnection() = 0; //argc: 2, index 27
    // WARNING: unknown arguments 
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TerminateGame(CGameID game) = 0; //argc: 2, index 28
    // WARNING: unknown arguments (has 1)
    virtual void SetSelfAsChatDestination(bool) = 0; //argc: 1, index 29
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool IsPrimaryChatDestination() = 0; //argc: 0, index 30
    virtual void RequestLegacyCDKey() = 0; //argc: 1, index 31
    virtual void AckGuestPass() = 0; //argc: 1, index 32
    virtual void RedeemGuestPass() = 0; //argc: 1, index 33
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToGiveCount() = 0; //argc: 0, index 34
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToRedeemCount() = 0; //argc: 0, index 35
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToGiveInfo() = 0; //argc: 9, index 36
    virtual void GetGuestPassToGiveOut() = 0; //argc: 1, index 37
    virtual void GetGuestPassToRedeem() = 0; //argc: 1, index 38
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToRedeemInfo() = 0; //argc: 7, index 39
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToRedeemSenderName() = 0; //argc: 3, index 40
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetNumAppsInGuestPassesToRedeem() = 0; //argc: 0, index 41
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppsInGuestPassesToRedeem() = 0; //argc: 2, index 42
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetCountUserNotifications() = 0; //argc: 0, index 43
    virtual void GetCountUserNotification() = 0; //argc: 1, index 44
    virtual void RequestStoreAuthURL() = 0; //argc: 1, index 45
    virtual void SetLanguage(const char* language) = 0; //argc: 1, index 46
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TrackAppUsageEvent() = 0; //argc: 3, index 47
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RaiseConnectionPriority() = 0; //argc: 2, index 48
    virtual void ResetConnectionPriority() = 0; //argc: 1, index 49
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetDesiredNetQOSLevel() = 0; //argc: 0, index 50
    // Reads config/config.vdf (Software/Valve/Steam/ConnectCache)
    virtual bool BHasCachedCredentials(const char* pszUsername) = 0; //argc: 1, index 51
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetAccountNameForCachedCredentialLogin(const char* pszUsername, bool bUnk) = 0; //argc: 2, index 52
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DestroyCachedCredentials(const char* pszUsername) = 0; //argc: 2, index 53
    virtual bool GetCurrentWebAuthToken(char *pchBuffer, int32 cubBuffer) = 0; //argc: 2, index 54
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestWebAuthToken() = 0; //argc: 0, index 55
    virtual void SetLoginInformation(const char* pszUsername, const char* pszPassword, bool bRememberPassword) = 0; //argc: 3, index 56
    virtual void SetTwoFactorCode(const char* steamGuardCode) = 0; //argc: 1, index 57
    virtual unknown_ret SetLoginToken(const char* pszToken, const char *pszUsername) = 0; //argc: 2, index 58
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetLoginTokenID() = 0; //argc: 0, index 59
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ClearAllLoginInformation() = 0; //argc: 0, index 60
    virtual void BEnableEmbeddedClient() = 0; //argc: 1, index 61
    virtual void ResetEmbeddedClient() = 0; //argc: 1, index 62
    virtual void BHasEmbeddedClientToken() = 0; //argc: 1, index 63
    virtual void RequestEmbeddedClientToken() = 0; //argc: 1, index 64
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AuthorizeNewDevice() = 0; //argc: 3, index 65
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetLanguage() = 0; //argc: 2, index 66
    virtual void TrackNatTraversalStat() = 0; //argc: 1, index 67
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TrackSteamUsageEvent() = 0; //argc: 3, index 68
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetComputerInUse() = 0; //argc: 0, index 69
    virtual void BIsGameRunning() = 0; //argc: 1, index 70
    virtual void BIsGameWindowReady() = 0; //argc: 1, index 71
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BUpdateAppOwnershipTicket() = 0; //argc: 2, index 72
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetCustomBinariesState() = 0; //argc: 3, index 73
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestCustomBinaries() = 0; //argc: 4, index 74
    virtual void SetCellID(uint32) = 0; //argc: 1, index 75
    virtual bool GetCellList(CUtlVector<uint32> *map) = 0; //argc: 1, index 76
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetUserBaseFolder() = 0; //argc: 0, index 77
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetUserDataFolder() = 0; //argc: 3, index 78
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetUserConfigFolder() = 0; //argc: 2, index 79
    virtual void GetAccountName(char*, unsigned int) = 0; //argc: 2, index 80
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAccountName() = 0; //argc: 4, index 81
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void IsPasswordRemembered() = 0; //argc: 0, index 82
    virtual void CheckoutSiteLicenseSeat() = 0; //argc: 1, index 83
    virtual void GetAvailableSeats() = 0; //argc: 1, index 84
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAssociatedSiteName() = 0; //argc: 0, index 85
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsRunningInCafe() = 0; //argc: 0, index 86
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BAllowCachedCredentialsInCafe() = 0; //argc: 0, index 87
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequiresLegacyCDKey() = 0; //argc: 2, index 88
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetLegacyCDKey() = 0; //argc: 3, index 89
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetLegacyCDKey() = 0; //argc: 2, index 90
    virtual void WriteLegacyCDKey() = 0; //argc: 1, index 91
    virtual void RemoveLegacyCDKey() = 0; //argc: 1, index 92
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestLegacyCDKeyFromApp() = 0; //argc: 3, index 93
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsAnyGameRunning() = 0; //argc: 0, index 94
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetSteamGuardDetails() = 0; //argc: 0, index 95
    virtual void GetSentryFileData() = 0; //argc: 1, index 96
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetTwoFactorDetails() = 0; //argc: 0, index 97
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool BHasTwoFactor() = 0; //argc: 0, index 98
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEmail() = 0; //argc: 3, index 99
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void Test_FakeConnectionTimeout() = 0; //argc: 0, index 100
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RunInstallScript() = 0; //argc: 3, index 101
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void IsInstallScriptRunning() = 0; //argc: 0, index 102
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetInstallScriptState() = 0; //argc: 4, index 103
    virtual void StopInstallScript() = 0; //argc: 1, index 104
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SpawnProcess() = 0; //argc: 9, index 105
    virtual void GetAppOwnershipTicketLength() = 0; //argc: 1, index 106
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppOwnershipTicketData() = 0; //argc: 3, index 107
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppOwnershipTicketExtendedData() = 0; //argc: 7, index 108
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMarketingMessageCount() = 0; //argc: 0, index 109
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMarketingMessage() = 0; //argc: 5, index 110
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void MarkMarketingMessageSeen() = 0; //argc: 2, index 111
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void CheckForPendingMarketingMessages() = 0; //argc: 0, index 112
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAuthSessionTicket() = 0; //argc: 3, index 113
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAuthSessionTicketV2() = 0; //argc: 4, index 114
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAuthSessionTicketV3() = 0; //argc: 4, index 115
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAuthTicketForWebApi() = 0; //argc: 1, index 116
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAuthSessionTicketForGameID() = 0; //argc: 5, index 117
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BeginAuthSession() = 0; //argc: 4, index 118
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void EndAuthSession() = 0; //argc: 2, index 119
    virtual void CancelAuthTicket() = 0; //argc: 1, index 120
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void IsUserSubscribedAppInTicket() = 0; //argc: 3, index 121
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AdvertiseGame() = 0; //argc: 5, index 122
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestEncryptedAppTicket() = 0; //argc: 2, index 123
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEncryptedAppTicket() = 0; //argc: 3, index 124
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGameBadgeLevel() = 0; //argc: 2, index 125
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetPlayerSteamLevel() = 0; //argc: 0, index 126
    virtual void SetAccountLimited() = 0; //argc: 1, index 127
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsAccountLimited() = 0; //argc: 0, index 128
    virtual void SetAccountCommunityBanned() = 0; //argc: 1, index 129
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsAccountCommunityBanned() = 0; //argc: 0, index 130
    virtual void SetLimitedAccountCanInviteFriends() = 0; //argc: 1, index 131
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BLimitedAccountCanInviteFriends() = 0; //argc: 0, index 132
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendValidationEmail() = 0; //argc: 0, index 133
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGameConnectTokensAvailable() = 0; //argc: 0, index 134
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void NumGamesRunning() = 0; //argc: 0, index 135
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetRunningGameID() = 0; //argc: 2, index 136
    virtual void GetRunningGamePID() = 0; //argc: 1, index 137
    virtual void RaiseWindowForGame() = 0; //argc: 1, index 138
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAccountSecurityPolicyFlags() = 0; //argc: 0, index 139
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetClientStat() = 0; //argc: 6, index 140
    virtual void VerifyPassword() = 0; //argc: 1, index 141
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BSupportUser() = 0; //argc: 0, index 142
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BNeedsSSANextSteamLogon() = 0; //argc: 0, index 143
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ClearNeedsSSANextSteamLogon() = 0; //argc: 0, index 144
    virtual void BIsAppOverlayEnabled() = 0; //argc: 1, index 145
    virtual void BOverlayIgnoreChildProcesses() = 0; //argc: 1, index 146
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetOverlayState() = 0; //argc: 2, index 147
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void NotifyOverlaySettingsChanged() = 0; //argc: 0, index 148
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsBehindNAT() = 0; //argc: 0, index 149
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnAppID() = 0; //argc: 2, index 150
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnOrderID() = 0; //argc: 2, index 151
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetMicroTxnPrice() = 0; //argc: 6, index 152
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnSteamRealm() = 0; //argc: 2, index 153
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnLineItemCount() = 0; //argc: 2, index 154
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetMicroTxnLineItem() = 0; //argc: 11, index 155
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsSandboxMicroTxn() = 0; //argc: 3, index 156
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BMicroTxnRequiresCachedPmtMethod() = 0; //argc: 3, index 157
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AuthorizeMicroTxn() = 0; //argc: 3, index 158
    virtual bool BGetWalletBalance(bool *pbHasWallet, CAmount *pamtBalance, CAmount *pamtPending) = 0; //argc: 3, index 159
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestMicroTxnInfo() = 0; //argc: 2, index 160
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BMicroTxnRefundable() = 0; //argc: 2, index 161
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetAppMinutesPlayed() = 0; //argc: 3, index 162
    virtual void GetAppLastPlayedTime() = 0; //argc: 1, index 163
    virtual void GetAppUpdateDisabledSecondsRemaining() = 0; //argc: 1, index 164
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetGuideURL() = 0; //argc: 3, index 165
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BPromptToChangePassword() = 0; //argc: 0, index 166
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BAccountExtraSecurity() = 0; //argc: 0, index 167
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BAccountShouldShowLockUI() = 0; //argc: 0, index 168
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetCountAuthedComputers() = 0; //argc: 0, index 169
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetSteamGuardEnabledTime() = 0; //argc: 0, index 170
    virtual void SetPhoneIsVerified() = 0; //argc: 1, index 171
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsPhoneVerified() = 0; //argc: 0, index 172
    virtual void SetPhoneIsIdentifying() = 0; //argc: 1, index 173
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsPhoneIdentifying() = 0; //argc: 0, index 174
    virtual void SetPhoneIsRequiringVerification() = 0; //argc: 1, index 175
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsPhoneRequiringVerification() = 0; //argc: 0, index 176
    virtual void ChangeTwoFactorAuthOptions() = 0; //argc: 1, index 177
    virtual void Set2ndFactorAuthCode(const char* pchAuthCode, bool bDontRememberComputer) = 0; //argc: 2, index 178
    virtual void SetUserMachineName(const char* newMachineName) = 0; //argc: 1, index 179
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual char* GetUserMachineName() = 0; //argc: 2, index 180
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEmailDomainFromLogonFailure() = 0; //argc: 2, index 181
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAgreementSessionUrl() = 0; //argc: 0, index 182
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetDurationControl() = 0; //argc: 0, index 183
    virtual void GetDurationControlForApp() = 0; //argc: 1, index 184
    virtual void BSetDurationControlOnlineState() = 0; //argc: 1, index 185
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BSetDurationControlOnlineStateForApp() = 0; //argc: 2, index 186
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetDurationControlExtendedResults() = 0; //argc: 3, index 187
    virtual void BIsSubscribedApp(AppId_t) = 0; //argc: 1, index 188
    virtual void GetSubscribedApps(AppId_t*, uint32, bool) = 0; //argc: 3, index 189
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AckSystemIM() = 0; //argc: 2, index 190
    virtual void RequestSpecialSurvey() = 0; //argc: 1, index 191
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendSpecialSurveyResponse() = 0; //argc: 3, index 192
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestNotifications() = 0; //argc: 0, index 193
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppOwnershipInfo() = 0; //argc: 4, index 194
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendGameWebCallback() = 0; //argc: 2, index 195
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsStreamingUIToRemoteDevice() = 0; //argc: 0, index 196
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsCurrentlyNVStreaming() = 0; //argc: 0, index 197
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnBigPictureForStreamingStartResult() = 0; //argc: 2, index 198
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnBigPictureForStreamingDone() = 0; //argc: 0, index 199
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnBigPictureForStreamingRestarting() = 0; //argc: 0, index 200
    virtual void StopStreaming() = 0; //argc: 1, index 201
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void LockParentalLock() = 0; //argc: 0, index 202
    virtual void UnlockParentalLock() = 0; //argc: 1, index 203
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsParentalLockEnabled() = 0; //argc: 0, index 204
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsParentalLockLocked() = 0; //argc: 0, index 205
    virtual void BlockApp() = 0; //argc: 1, index 206
    virtual void UnblockApp() = 0; //argc: 1, index 207
    virtual void BIsAppBlocked() = 0; //argc: 1, index 208
    virtual void BIsAppInBlockList() = 0; //argc: 1, index 209
    virtual void BlockFeature() = 0; //argc: 1, index 210
    virtual void UnblockFeature() = 0; //argc: 1, index 211
    virtual void BIsFeatureBlocked() = 0; //argc: 1, index 212
    virtual void BIsFeatureInBlockList() = 0; //argc: 1, index 213
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetParentalUnlockTime() = 0; //argc: 0, index 214
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetRecoveryEmail() = 0; //argc: 2, index 215
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestParentalRecoveryEmail() = 0; //argc: 0, index 216
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsLockFromSiteLicense() = 0; //argc: 0, index 217
    virtual void BGetSerializedParentalSettings() = 0; //argc: 1, index 218
    virtual void BSetParentalSettings() = 0; //argc: 1, index 219
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BDisableParentalSettings() = 0; //argc: 0, index 220
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetParentalWebToken() = 0; //argc: 2, index 221
    virtual void GetCommunityPreference() = 0; //argc: 1, index 222
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetCommunityPreference() = 0; //argc: 2, index 223
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetTextFilterSetting() = 0; //argc: 0, index 224
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BTextFilterIgnoresFriends() = 0; //argc: 0, index 225
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void CanLogonOffline() = 0; //argc: 0, index 226
    virtual void LogOnOffline() = 0; //argc: 1, index 227
    virtual void ValidateOfflineLogonTicket() = 0; //argc: 1, index 228
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetOfflineLogonTicket() = 0; //argc: 2, index 229
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void UploadLocalClientLogs() = 0; //argc: 0, index 230
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetAsyncNotificationEnabled() = 0; //argc: 2, index 231
    virtual void BIsOtherSessionPlaying() = 0; //argc: 1, index 232
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BKickOtherPlayingSession() = 0; //argc: 0, index 233
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsAccountLockedDown() = 0; //argc: 0, index 234
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ClearAndSetAppTags() = 0; //argc: 2, index 235
    virtual void RemoveAppTag(AppId_t, const char* tag) = 0; //argc: 2, index 236
    virtual void AddAppTag(AppId_t, const char* tag) = 0; //argc: 2, index 237
    virtual void ClearAppTags(AppId_t) = 0; //argc: 1, index 238
    virtual void SetAppHidden(AppId_t, bool) = 0; //argc: 2, index 239
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestAccountLinkInfo() = 0; //argc: 0, index 240
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSurveySchedule() = 0; //argc: 0, index 241
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestNewSteamAnnouncementState() = 0; //argc: 0, index 242
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void UpdateSteamAnnouncementLastRead() = 0; //argc: 3, index 243
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMarketEligibility() = 0; //argc: 0, index 244
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void UpdateGameVrDllState() = 0; //argc: 3, index 245
    virtual void KillVRTheaterPancakeGame() = 0; //argc: 1, index 246
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsAnyGameOrServiceAppRunning() = 0; //argc: 0, index 247
    virtual void BGetAppPlaytimeMap(CUtlMap<AppId_t, uint64> *mapOut) = 0; //argc: 1, index 248
    virtual void BGetAppsLastPlayedMap(CUtlMap<AppId_t, uint64> *mapOut) = 0; //argc: 1, index 249
    virtual bool BGetAppTagsMap(CUtlMap<AppId_t, AppTags_t> *mapOut) = 0; //argc: 1, index 250
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendSteamServiceStatusUpdate() = 0; //argc: 2, index 251
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSteamGroupChatMessageNotifications() = 0; //argc: 5, index 252
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSteamGroupChatMessageHistory() = 0; //argc: 5, index 253
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSendSteamGroupChatMessage() = 0; //argc: 6, index 254
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnNewGroupChatMsgAdded() = 0; //argc: 8, index 255
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnGroupChatUserStateChange() = 0; //argc: 4, index 256
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnReceivedGroupChatSubscriptionResponse() = 0; //argc: 5, index 257
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetTimedTrialStatus() = 0; //argc: 4, index 258
    virtual void RequestTimedTrialStatus() = 0; //argc: 1, index 259
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void PrepareForSystemSuspend() = 0; //argc: 0, index 260
    virtual void ResumeSuspendedGames() = 0; //argc: 1, index 261
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetClientInstallationID() = 0; //argc: 0, index 262
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void Test_SetClientInstallationID() = 0; //argc: 2, index 263
    virtual AppId_t GetAppIDForGameID(CGameID) = 0; //argc: 1, index 264
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BDoNotDisturb() = 0; //argc: 0, index 265
    virtual void SetAdditionalClientArgData() = 0; //argc: 1, index 266
};

#endif // ICLIENTUSER_H