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
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_0_DONTUSE() = 0; //argc: -1, index 1
    virtual unknown_ret LogOn(CSteamID steamid, bool) = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InvalidateCredentials() = 0; //argc: 2, index 3
    virtual unknown_ret LogOff() = 0; //argc: 0, index 4
    virtual bool BLoggedOn() = 0; //argc: 0, index 1
    virtual unknown_ret GetLogonState() = 0; //argc: 0, index 1
    virtual unknown_ret BConnected() = 0; //argc: 0, index 1
    virtual bool BInitiateReconnect() = 0; //argc: 0, index 1
    // Returns an enum. What are it's values?
    virtual unknown_ret EConnect() = 0; //argc: 0, index 1
    virtual bool BTryingToLogin() = 0; //argc: 0, index 2
    virtual CSteamID GetSteamID(const char *username) = 0; //argc: 1, index 1
    // WARNING: unknown arguments
    virtual CSteamID GetConsoleSteamID(const char* pszUsername) = 0; //argc: 1, index 2
    // WARNING: retval is pulled from a hat
    virtual char* GetClientInstanceID() = 0; //argc: 0, index 3
    virtual unknown_ret GetUserCountry() = 0; //argc: 0, index 2
    // WARNING: untested
    virtual bool IsVACBanned(CSteamID steamid) = 0; //argc: 1, index 1
    virtual void SetEmail() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetConfigString() = 0; //argc: 3, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigString() = 0; //argc: 4, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetConfigInt() = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigInt() = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetConfigBinaryBlob() = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigBinaryBlob() = 0; //argc: 3, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DeleteConfigKey() = 0; //argc: 2, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetConfigStoreKeyName() = 0; //argc: 4, index 10
    // WARNING: unknown arguments
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void InitiateGameConnection() = 0; //argc: 8, index 11
    // WARNING: unknown arguments
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void InitiateGameConnectionOld() = 0; //argc: 10, index 12
    // WARNING: unknown arguments
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TerminateGameConnection() = 0; //argc: 2, index 13
    // WARNING: unknown arguments 
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TerminateGame(CGameID game) = 0; //argc: 2, index 14
    // WARNING: unknown arguments (has 1)
    virtual void SetSelfAsChatDestination(bool) = 0; //argc: 1, index 15
    virtual bool IsPrimaryChatDestination() = 0; //argc: 0, index 16
    virtual void RequestLegacyCDKey() = 0; //argc: 1, index 1
    virtual void AckGuestPass() = 0; //argc: 1, index 2
    virtual void RedeemGuestPass() = 0; //argc: 1, index 3
    virtual void GetGuestPassToGiveCount() = 0; //argc: 0, index 4
    virtual void GetGuestPassToRedeemCount() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToGiveInfo() = 0; //argc: 9, index 1
    virtual void GetGuestPassToGiveOut() = 0; //argc: 1, index 2
    virtual void GetGuestPassToRedeem() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToRedeemInfo() = 0; //argc: 7, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGuestPassToRedeemSenderName() = 0; //argc: 3, index 5
    virtual void GetNumAppsInGuestPassesToRedeem() = 0; //argc: 0, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppsInGuestPassesToRedeem() = 0; //argc: 2, index 1
    virtual void GetCountUserNotifications() = 0; //argc: 0, index 2
    virtual void GetCountUserNotification() = 0; //argc: 1, index 1
    virtual void RequestStoreAuthURL() = 0; //argc: 1, index 2
    virtual void SetLanguage(const char* language) = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TrackAppUsageEvent() = 0; //argc: 3, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RaiseConnectionPriority() = 0; //argc: 2, index 5
    virtual void ResetConnectionPriority() = 0; //argc: 1, index 6
    virtual unknown_ret GetDesiredNetQOSLevel() = 0; //argc: 0, index 7
    // Reads config/config.vdf (Software/Valve/Steam/ConnectCache)
    virtual bool BHasCachedCredentials(const char* pszUsername) = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetAccountNameForCachedCredentialLogin(const char* pszUsername, bool bUnk) = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void DestroyCachedCredentials(const char* pszUsername) = 0; //argc: 2, index 3
    virtual bool GetCurrentWebAuthToken(char *pchBuffer, int32 cubBuffer) = 0; //argc: 2, index 4
    virtual void RequestWebAuthToken() = 0; //argc: 0, index 5
    virtual void SetLoginInformation(const char* pszUsername, const char* pszPassword, bool bRememberPassword) = 0; //argc: 3, index 1
    virtual void SetTwoFactorCode(const char* steamGuardCode) = 0; //argc: 1, index 2
    virtual unknown_ret SetLoginToken(const char* pszToken, const char *pszUsername) = 0; //argc: 2, index 3
    virtual void GetLoginTokenID() = 0; //argc: 0, index 4
    virtual void ClearAllLoginInformation() = 0; //argc: 0, index 1
    virtual void BEnableEmbeddedClient() = 0; //argc: 1, index 1
    virtual void ResetEmbeddedClient() = 0; //argc: 1, index 2
    virtual void BHasEmbeddedClientToken() = 0; //argc: 1, index 3
    virtual void RequestEmbeddedClientToken() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AuthorizeNewDevice() = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetLanguage() = 0; //argc: 2, index 6
    virtual void TrackNatTraversalStat() = 0; //argc: 1, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void TrackSteamUsageEvent() = 0; //argc: 3, index 8
    virtual void SetComputerInUse() = 0; //argc: 0, index 9
    virtual void BIsGameRunning() = 0; //argc: 1, index 1
    virtual void BIsGameWindowReady() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BUpdateAppOwnershipTicket() = 0; //argc: 2, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetCustomBinariesState() = 0; //argc: 3, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestCustomBinaries() = 0; //argc: 4, index 5
    virtual void SetCellID(uint32) = 0; //argc: 1, index 6
    virtual bool GetCellList(CUtlVector<uint32> *map) = 0; //argc: 1, index 7
    virtual void GetUserBaseFolder() = 0; //argc: 0, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetUserDataFolder() = 0; //argc: 3, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetUserConfigFolder() = 0; //argc: 2, index 2
    virtual void GetAccountName(char*, unsigned int) = 0; //argc: 2, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAccountName() = 0; //argc: 4, index 4
    virtual void IsPasswordRemembered() = 0; //argc: 0, index 5
    virtual void CheckoutSiteLicenseSeat() = 0; //argc: 1, index 1
    virtual void GetAvailableSeats() = 0; //argc: 1, index 2
    virtual void GetAssociatedSiteName() = 0; //argc: 0, index 3
    virtual void BIsRunningInCafe() = 0; //argc: 0, index 1
    virtual void BAllowCachedCredentialsInCafe() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequiresLegacyCDKey() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetLegacyCDKey() = 0; //argc: 3, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetLegacyCDKey() = 0; //argc: 2, index 3
    virtual void WriteLegacyCDKey() = 0; //argc: 1, index 4
    virtual void RemoveLegacyCDKey() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestLegacyCDKeyFromApp() = 0; //argc: 3, index 6
    virtual void BIsAnyGameRunning() = 0; //argc: 0, index 7
    virtual void GetSteamGuardDetails() = 0; //argc: 0, index 1
    virtual void GetSentryFileData() = 0; //argc: 1, index 1
    virtual void GetTwoFactorDetails() = 0; //argc: 0, index 2
    virtual bool BHasTwoFactor() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEmail() = 0; //argc: 3, index 1
    virtual void Test_FakeConnectionTimeout() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RunInstallScript() = 0; //argc: 3, index 1
    virtual void IsInstallScriptRunning() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetInstallScriptState() = 0; //argc: 4, index 1
    virtual void StopInstallScript() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SpawnProcess() = 0; //argc: 9, index 3
    virtual void GetAppOwnershipTicketLength() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppOwnershipTicketData() = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppOwnershipTicketExtendedData() = 0; //argc: 7, index 6
    virtual void GetMarketingMessageCount() = 0; //argc: 0, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMarketingMessage() = 0; //argc: 5, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void MarkMarketingMessageSeen() = 0; //argc: 2, index 2
    virtual void CheckForPendingMarketingMessages() = 0; //argc: 0, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAuthSessionTicket() = 0; //argc: 3, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAuthSessionTicketV2() = 0; //argc: 4, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAuthSessionTicketV3() = 0; //argc: 4, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAuthTicketForWebApi() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAuthSessionTicketForGameID() = 0; //argc: 5, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BeginAuthSession() = 0; //argc: 4, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void EndAuthSession() = 0; //argc: 2, index 7
    virtual void CancelAuthTicket() = 0; //argc: 1, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void IsUserSubscribedAppInTicket() = 0; //argc: 3, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AdvertiseGame() = 0; //argc: 5, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestEncryptedAppTicket() = 0; //argc: 2, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEncryptedAppTicket() = 0; //argc: 3, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetGameBadgeLevel() = 0; //argc: 2, index 13
    virtual void GetPlayerSteamLevel() = 0; //argc: 0, index 14
    virtual void SetAccountLimited() = 0; //argc: 1, index 1
    virtual void BIsAccountLimited() = 0; //argc: 0, index 2
    virtual void SetAccountCommunityBanned() = 0; //argc: 1, index 1
    virtual void BIsAccountCommunityBanned() = 0; //argc: 0, index 2
    virtual void SetLimitedAccountCanInviteFriends() = 0; //argc: 1, index 1
    virtual void BLimitedAccountCanInviteFriends() = 0; //argc: 0, index 2
    virtual void SendValidationEmail() = 0; //argc: 0, index 1
    virtual void BGameConnectTokensAvailable() = 0; //argc: 0, index 1
    virtual void NumGamesRunning() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetRunningGameID() = 0; //argc: 2, index 1
    virtual void GetRunningGamePID() = 0; //argc: 1, index 2
    virtual void RaiseWindowForGame() = 0; //argc: 1, index 3
    virtual void GetAccountSecurityPolicyFlags() = 0; //argc: 0, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetClientStat() = 0; //argc: 6, index 1
    virtual void VerifyPassword() = 0; //argc: 1, index 2
    virtual void BSupportUser() = 0; //argc: 0, index 3
    virtual void BNeedsSSANextSteamLogon() = 0; //argc: 0, index 1
    virtual void ClearNeedsSSANextSteamLogon() = 0; //argc: 0, index 1
    virtual void BIsAppOverlayEnabled() = 0; //argc: 1, index 1
    virtual void BOverlayIgnoreChildProcesses() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetOverlayState() = 0; //argc: 2, index 3
    virtual void NotifyOverlaySettingsChanged() = 0; //argc: 0, index 4
    virtual void BIsBehindNAT() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnAppID() = 0; //argc: 2, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnOrderID() = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetMicroTxnPrice() = 0; //argc: 6, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnSteamRealm() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetMicroTxnLineItemCount() = 0; //argc: 2, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetMicroTxnLineItem() = 0; //argc: 11, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BIsSandboxMicroTxn() = 0; //argc: 3, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BMicroTxnRequiresCachedPmtMethod() = 0; //argc: 3, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AuthorizeMicroTxn() = 0; //argc: 3, index 9
    virtual bool BGetWalletBalance(bool *pbHasWallet, CAmount *pamtBalance, CAmount *pamtPending) = 0; //argc: 3, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestMicroTxnInfo() = 0; //argc: 2, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BMicroTxnRefundable() = 0; //argc: 2, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetAppMinutesPlayed() = 0; //argc: 3, index 13
    virtual void GetAppLastPlayedTime() = 0; //argc: 1, index 14
    virtual void GetAppUpdateDisabledSecondsRemaining() = 0; //argc: 1, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetGuideURL() = 0; //argc: 3, index 16
    virtual void BPromptToChangePassword() = 0; //argc: 0, index 17
    virtual void BAccountExtraSecurity() = 0; //argc: 0, index 1
    virtual void BAccountShouldShowLockUI() = 0; //argc: 0, index 1
    virtual void GetCountAuthedComputers() = 0; //argc: 0, index 1
    virtual void GetSteamGuardEnabledTime() = 0; //argc: 0, index 1
    virtual void SetPhoneIsVerified() = 0; //argc: 1, index 1
    virtual void BIsPhoneVerified() = 0; //argc: 0, index 2
    virtual void SetPhoneIsIdentifying() = 0; //argc: 1, index 1
    virtual void BIsPhoneIdentifying() = 0; //argc: 0, index 2
    virtual void SetPhoneIsRequiringVerification() = 0; //argc: 1, index 1
    virtual void BIsPhoneRequiringVerification() = 0; //argc: 0, index 2
    virtual void ChangeTwoFactorAuthOptions() = 0; //argc: 1, index 1
    virtual void Set2ndFactorAuthCode(const char* pchAuthCode, bool bDontRememberComputer) = 0; //argc: 2, index 2
    virtual void SetUserMachineName(const char* newMachineName) = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual char* GetUserMachineName() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetEmailDomainFromLogonFailure() = 0; //argc: 2, index 5
    virtual void GetAgreementSessionUrl() = 0; //argc: 0, index 6
    virtual void GetDurationControl() = 0; //argc: 0, index 1
    virtual void GetDurationControlForApp() = 0; //argc: 1, index 1
    virtual void BSetDurationControlOnlineState() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BSetDurationControlOnlineStateForApp() = 0; //argc: 2, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetDurationControlExtendedResults() = 0; //argc: 3, index 4
    virtual void BIsSubscribedApp(AppId_t) = 0; //argc: 1, index 5
    virtual void GetSubscribedApps(AppId_t*, uint32, bool) = 0; //argc: 3, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void AckSystemIM() = 0; //argc: 2, index 7
    virtual void RequestSpecialSurvey() = 0; //argc: 1, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendSpecialSurveyResponse() = 0; //argc: 3, index 9
    virtual void RequestNotifications() = 0; //argc: 0, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetAppOwnershipInfo() = 0; //argc: 4, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendGameWebCallback() = 0; //argc: 2, index 2
    virtual void BIsStreamingUIToRemoteDevice() = 0; //argc: 0, index 3
    virtual void BIsCurrentlyNVStreaming() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnBigPictureForStreamingStartResult() = 0; //argc: 2, index 1
    virtual void OnBigPictureForStreamingDone() = 0; //argc: 0, index 2
    virtual void OnBigPictureForStreamingRestarting() = 0; //argc: 0, index 1
    virtual void StopStreaming() = 0; //argc: 1, index 1
    virtual void LockParentalLock() = 0; //argc: 0, index 2
    virtual void UnlockParentalLock() = 0; //argc: 1, index 1
    virtual void BIsParentalLockEnabled() = 0; //argc: 0, index 2
    virtual void BIsParentalLockLocked() = 0; //argc: 0, index 1
    virtual void BlockApp() = 0; //argc: 1, index 1
    virtual void UnblockApp() = 0; //argc: 1, index 2
    virtual void BIsAppBlocked() = 0; //argc: 1, index 3
    virtual void BIsAppInBlockList() = 0; //argc: 1, index 4
    virtual void BlockFeature() = 0; //argc: 1, index 5
    virtual void UnblockFeature() = 0; //argc: 1, index 6
    virtual void BIsFeatureBlocked() = 0; //argc: 1, index 7
    virtual void BIsFeatureInBlockList() = 0; //argc: 1, index 8
    virtual void GetParentalUnlockTime() = 0; //argc: 0, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetRecoveryEmail() = 0; //argc: 2, index 1
    virtual void RequestParentalRecoveryEmail() = 0; //argc: 0, index 2
    virtual void BIsLockFromSiteLicense() = 0; //argc: 0, index 1
    virtual void BGetSerializedParentalSettings() = 0; //argc: 1, index 1
    virtual void BSetParentalSettings() = 0; //argc: 1, index 2
    virtual void BDisableParentalSettings() = 0; //argc: 0, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetParentalWebToken() = 0; //argc: 2, index 1
    virtual void GetCommunityPreference() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetCommunityPreference() = 0; //argc: 2, index 3
    virtual void GetTextFilterSetting() = 0; //argc: 0, index 4
    virtual void BTextFilterIgnoresFriends() = 0; //argc: 0, index 1
    virtual void CanLogonOffline() = 0; //argc: 0, index 1
    virtual void LogOnOffline() = 0; //argc: 1, index 1
    virtual void ValidateOfflineLogonTicket() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void BGetOfflineLogonTicket() = 0; //argc: 2, index 3
    virtual void UploadLocalClientLogs() = 0; //argc: 0, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SetAsyncNotificationEnabled() = 0; //argc: 2, index 1
    virtual void BIsOtherSessionPlaying() = 0; //argc: 1, index 2
    virtual void BKickOtherPlayingSession() = 0; //argc: 0, index 3
    virtual void BIsAccountLockedDown() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void ClearAndSetAppTags() = 0; //argc: 2, index 1
    virtual void RemoveAppTag(AppId_t, const char* tag) = 0; //argc: 2, index 2
    virtual void AddAppTag(AppId_t, const char* tag) = 0; //argc: 2, index 3
    virtual void ClearAppTags(AppId_t) = 0; //argc: 1, index 4
    virtual void SetAppHidden(AppId_t, bool) = 0; //argc: 2, index 5
    virtual void RequestAccountLinkInfo() = 0; //argc: 0, index 6
    virtual void RequestSurveySchedule() = 0; //argc: 0, index 1
    virtual void RequestNewSteamAnnouncementState() = 0; //argc: 0, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void UpdateSteamAnnouncementLastRead() = 0; //argc: 3, index 1
    virtual void GetMarketEligibility() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void UpdateGameVrDllState() = 0; //argc: 3, index 1
    virtual void KillVRTheaterPancakeGame() = 0; //argc: 1, index 2
    virtual void BIsAnyGameOrServiceAppRunning() = 0; //argc: 0, index 3
    virtual void BGetAppPlaytimeMap(CUtlMap<AppId_t, uint64> *mapOut) = 0; //argc: 1, index 1
    virtual void BGetAppsLastPlayedMap(CUtlMap<AppId_t, uint64> *mapOut) = 0; //argc: 1, index 2
    virtual bool BGetAppTagsMap(CUtlMap<AppId_t, AppTags_t> *mapOut) = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void SendSteamServiceStatusUpdate() = 0; //argc: 2, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSteamGroupChatMessageNotifications() = 0; //argc: 5, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSteamGroupChatMessageHistory() = 0; //argc: 5, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void RequestSendSteamGroupChatMessage() = 0; //argc: 6, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnNewGroupChatMsgAdded() = 0; //argc: 8, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnGroupChatUserStateChange() = 0; //argc: 4, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void OnReceivedGroupChatSubscriptionResponse() = 0; //argc: 5, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void GetTimedTrialStatus() = 0; //argc: 4, index 11
    virtual void RequestTimedTrialStatus() = 0; //argc: 1, index 12
    virtual void PrepareForSystemSuspend() = 0; //argc: 0, index 13
    virtual void ResumeSuspendedGames() = 0; //argc: 1, index 1
    virtual void GetClientInstallationID() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual void Test_SetClientInstallationID() = 0; //argc: 2, index 1
    virtual AppId_t GetAppIDForGameID(CGameID) = 0; //argc: 1, index 2
    virtual void BDoNotDisturb() = 0; //argc: 0, index 3
    virtual void SetAdditionalClientArgData() = 0; //argc: 1, index 1
};

#endif // ICLIENTUSER_H