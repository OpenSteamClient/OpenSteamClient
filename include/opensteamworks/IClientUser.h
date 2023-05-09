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

abstract_class UNSAFE_INTERFACE IClientUser
{
public:
    virtual HSteamUser GetHSteamUser() = 0;
    virtual unknown_ret LogOn(CSteamID steamid, bool) = 0;
    virtual unknown_ret InvalidateCredentials() = 0;
    virtual unknown_ret LogOff() = 0;
    virtual bool BLoggedOn() = 0;
    virtual unknown_ret GetLogonState() = 0;
    virtual unknown_ret BConnected() = 0;
    virtual bool BInitiateReconnect() = 0;
    // Returns an enum. What are it's values?
    virtual unknown_ret EConnect() = 0;
    virtual bool BTryingToLogin() = 0;
    virtual CSteamID GetSteamId(const char* pszUsername) = 0;
    // WARNING: unknown arguments
    virtual CSteamID GetConsoleSteamID(const char* pszUsername) = 0;
    // WARNING: retval is pulled from a hat
    virtual char* GetClientInstanceID() = 0;
    // WARNING: untested
    virtual bool IsVACBanned(CSteamID steamid) = 0;
    virtual void SetEmail() = 0; //args: 1
    virtual void SetConfigString() = 0; //args: 3
    virtual void GetConfigString() = 0; //args: 4
    virtual void SetConfigInt() = 0; //args: 3
    virtual void GetConfigInt() = 0; //args: 3
    virtual void SetConfigBinaryBlob() = 0; //args: 3
    virtual void GetConfigBinaryBlob() = 0; //args: 3
    virtual void DeleteConfigKey() = 0; //args: 2
    virtual void GetConfigStoreKeyName() = 0; //args: 4
    // WARNING: unknown arguments
    virtual void InitiateGameConnection() = 0;
    // WARNING: unknown arguments
    virtual void InitiateGameConnectionOld() = 0;
    // WARNING: unknown arguments
    virtual void TerminateGameConnection() = 0;
    // WARNING: unknown arguments 
    virtual void TerminateGame(CGameID game) = 0;
    // WARNING: unknown arguments (has 1)
    virtual void SetSelfAsChatDestination(bool) = 0;
    virtual bool IsPrimaryChatDestination() = 0;
    virtual void RequestLegacyCDKey() = 0; //args: 1
    virtual void AckGuestPass() = 0; //args: 1
    virtual void RedeemGuestPass() = 0; //args: 1
    virtual void GetGuestPassToGiveCount() = 0; //args: 0
    virtual void GetGuestPassToRedeemCount() = 0; //args: 0
    virtual void GetGuestPassToGiveInfo() = 0; //args: 9
    virtual void GetGuestPassToGiveOut() = 0; //args: 1
    virtual void GetGuestPassToRedeem() = 0; //args: 1
    virtual void GetGuestPassToRedeemInfo() = 0; //args: 7
    virtual void GetGuestPassToRedeemSenderName() = 0; //args: 3
    virtual void GetNumAppsInGuestPassesToRedeem() = 0; //args: 0
    virtual void GetAppsInGuestPassesToRedeem() = 0; //args: 2
    virtual void GetCountUserNotifications() = 0;
    virtual void GetCountUserNotification() = 0;
    virtual void RequestStoreAuthURL() = 0;
    virtual void SetLanguage(const char* language) = 0;
    virtual void TrackAppUsageEvent() = 0;
    virtual void RaiseConnectionPriority() = 0;
    virtual void ResetConnectionPriority() = 0;
    // Reads config/config.vdf (Software/Valve/Steam/ConnectCache)
    virtual bool BHasCachedCredentials(const char* pszUsername) = 0;
    virtual void SetLogonNameForCachedCredentialLogin(const char* pszUsername) = 0;
    virtual void DestroyCachedCredentials(const char* pszUsername) = 0;
    virtual bool GetCurrentWebAuthToken(char *pchBuffer, int32 cubBuffer) = 0;
    virtual void RequestWebAuthToken() = 0;
    virtual void SetLoginInformation(const char* pszUsername, const char* pszPassword, bool bRememberPassword) = 0;
    virtual void SetTwoFactorCode(const char* steamGuardCode) = 0;
    virtual unknown_ret SetLoginToken(const char* pszToken, void *steamidOut) = 0;
    virtual void GetLoginTokenID() = 0; //args: 0
    virtual void ClearAllLoginInformation() = 0; //args: 0
    virtual void BEnableEmbeddedClient() = 0; //args: 1
    virtual void ResetEmbeddedClient() = 0; //args: 1
    virtual void BHasEmbeddedClientToken() = 0; //args: 1
    virtual void RequestEmbeddedClientToken() = 0; //args: 1
    virtual void AuthorizeNewDevice() = 0; //args: 3
    virtual void GetLanguage() = 0; //args: 2
    virtual void BIsCyberCafe() = 0; //args: 0
    virtual void BIsAcademicAccount() = 0; //args: 0
    virtual void BIsPortal2EducationAccount() = 0; //args: 0
    virtual void TrackNatTraversalStat() = 0; //args: 1
    virtual void TrackSteamUsageEvent() = 0; //args: 3
    virtual void TrackSteamGUIUsage() = 0; //args: 1
    virtual void SetComputerInUse() = 0; //args: 0
    virtual void BIsGameRunning() = 0; //args: 1
    virtual void BIsGameWindowReady() = 0; //args: 1
    virtual void BUpdateAppOwnershipTicket() = 0; //args: 2
    virtual void GetCustomBinariesState() = 0; //args: 3
    virtual void RequestCustomBinaries() = 0; //args: 4
    virtual void SetCellID(uint32) = 0; //args: 1
    virtual bool GetCellList(CUtlVector<uint32> *map) = 0; //args: 1
    virtual void GetUserBaseFolder() = 0; //args: 0
    virtual void GetUserDataFolder() = 0; //args: 3
    virtual void GetUserConfigFolder() = 0; //args: 2
    virtual void GetAccountName(char*, unsigned int) = 0; //args: 2
    virtual void GetAccountName2() = 0; //args: 4
    virtual void IsPasswordRemembered() = 0; //args: 0
    virtual void CheckoutSiteLicenseSeat() = 0; //args: 1
    virtual void GetAvailableSeats() = 0; //args: 1
    virtual void GetAssociatedSiteName() = 0; //args: 0
    virtual void BIsRunningInCafe() = 0; //args: 0
    virtual void BAllowCachedCredentialsInCafe() = 0; //args: 0
    virtual void RequiresLegacyCDKey() = 0; //args: 2
    virtual void GetLegacyCDKey() = 0; //args: 3
    virtual void SetLegacyCDKey() = 0; //args: 2
    virtual void WriteLegacyCDKey() = 0; //args: 1
    virtual void RemoveLegacyCDKey() = 0; //args: 1
    virtual void RequestLegacyCDKeyFromApp() = 0; //args: 3
    virtual void BIsAnyGameRunning() = 0; //args: 0
    virtual void GetSteamGuardDetails() = 0; //args: 0
    virtual void GetSentryFileData() = 0; //args: 1
    virtual void GetTwoFactorDetails() = 0; //args: 0
    virtual bool BHasTwoFactor() = 0; //args: 0
    virtual void GetEmail() = 0; //args: 3
    virtual void Test_FakeConnectionTimeout() = 0; //args: 0
    virtual void RunInstallScript() = 0; //args: 3
    virtual void IsInstallScriptRunning() = 0; //args: 0
    virtual void GetInstallScriptState() = 0; //args: 4
    virtual void StopInstallScript() = 0; //args: 1
    virtual void SpawnProcess() = 0; //args: 9
    virtual void GetAppOwnershipTicketLength() = 0; //args: 1
    virtual void GetAppOwnershipTicketData() = 0; //args: 3
    virtual void GetAppOwnershipTicketExtendedData() = 0; //args: 7
    virtual void GetMarketingMessageCount() = 0; //args: 0
    virtual void GetMarketingMessage() = 0; //args: 5
    virtual void MarkMarketingMessageSeen() = 0; //args: 2
    virtual void CheckForPendingMarketingMessages() = 0; //args: 0
    virtual void GetAuthSessionTicket() = 0; //args: 3
    virtual void GetAuthSessionTicketV2() = 0; //args: 4
    virtual void BeginAuthSession() = 0; //args: 4
    virtual void EndAuthSession() = 0; //args: 2
    virtual void CancelAuthTicket() = 0; //args: 1
    virtual void IsUserSubscribedAppInTicket() = 0; //args: 3
    virtual void GetAuthSessionTicketForGameID() = 0; //args: 5
    virtual void AdvertiseGame() = 0; //args: 5
    virtual void RequestEncryptedAppTicket() = 0; //args: 2
    virtual void GetEncryptedAppTicket() = 0; //args: 3
    virtual void GetGameBadgeLevel() = 0; //args: 2
    virtual void GetPlayerSteamLevel() = 0; //args: 0
    virtual void SetAccountLimited() = 0; //args: 1
    virtual void BIsAccountLimited() = 0; //args: 0
    virtual void SetAccountCommunityBanned() = 0; //args: 1
    virtual void BIsAccountCommunityBanned() = 0; //args: 0
    virtual void SetLimitedAccountCanInviteFriends() = 0; //args: 1
    virtual void BLimitedAccountCanInviteFriends() = 0; //args: 0
    virtual void SendValidationEmail() = 0; //args: 0
    virtual void BGameConnectTokensAvailable() = 0; //args: 0
    virtual void NumGamesRunning() = 0; //args: 0
    virtual void GetRunningGameID() = 0; //args: 2
    virtual void GetRunningGamePID() = 0; //args: 1
    virtual void RaiseWindowForGame() = 0; //args: 1
    virtual void GetAccountSecurityPolicyFlags() = 0; //args: 0
    virtual void SetClientStat() = 0; //args: 6
    virtual void VerifyPassword() = 0; //args: 1
    virtual void BSupportUser() = 0; //args: 0
    virtual void BNeedsSSANextSteamLogon() = 0; //args: 0
    virtual void ClearNeedsSSANextSteamLogon() = 0; //args: 0
    virtual void BIsAppOverlayEnabled() = 0; //args: 1
    virtual void BOverlayIgnoreChildProcesses() = 0; //args: 1
    virtual void SetOverlayState() = 0; //args: 2
    virtual void NotifyOverlaySettingsChanged() = 0; //args: 0
    virtual void BIsBehindNAT() = 0; //args: 0
    virtual void GetMicroTxnAppID() = 0; //args: 2
    virtual void GetMicroTxnOrderID() = 0; //args: 2
    virtual void BGetMicroTxnPrice() = 0; //args: 6
    virtual void GetMicroTxnSteamRealm() = 0; //args: 2
    virtual void GetMicroTxnLineItemCount() = 0; //args: 2
    virtual void BGetMicroTxnLineItem() = 0; //args: 11
    virtual void BIsSandboxMicroTxn() = 0; //args: 3
    virtual void BMicroTxnRequiresCachedPmtMethod() = 0; //args: 3
    virtual void AuthorizeMicroTxn() = 0; //args: 3
    virtual bool BGetWalletBalance(bool *pbHasWallet, CAmount *pamtBalance, CAmount *pamtPending) = 0; //args: 3
    virtual void RequestMicroTxnInfo() = 0; //args: 2
    virtual void BMicroTxnRefundable() = 0; //args: 2
    virtual void BGetAppMinutesPlayed() = 0; //args: 3
    virtual void GetAppLastPlayedTime() = 0; //args: 1
    virtual void GetAppUpdateDisabledSecondsRemaining() = 0; //args: 1
    virtual void BGetGuideURL() = 0; //args: 3
    virtual void BPromptToChangePassword() = 0; //args: 0
    virtual void BAccountExtraSecurity() = 0; //args: 0
    virtual void BAccountShouldShowLockUI() = 0; //args: 0
    virtual void GetCountAuthedComputers() = 0; //args: 0
    virtual void GetSteamGuardEnabledTime() = 0; //args: 0
    virtual void SetPhoneIsVerified() = 0; //args: 1
    virtual void BIsPhoneVerified() = 0; //args: 0
    virtual void SetPhoneIsIdentifying() = 0; //args: 1
    virtual void BIsPhoneIdentifying() = 0; //args: 0
    virtual void SetPhoneIsRequiringVerification() = 0; //args: 1
    virtual void BIsPhoneRequiringVerification() = 0; //args: 0
    virtual void ChangeTwoFactorAuthOptions() = 0; //args: 1
    virtual void Set2ndFactorAuthCode(const char* pchAuthCode, bool bDontRememberComputer) = 0; //args: 2
    virtual void SetUserMachineName(const char* newMachineName) = 0; //args: 1
    virtual char* GetUserMachineName() = 0; //args: 2
    virtual void GetEmailDomainFromLogonFailure() = 0; //args: 2
    virtual void GetAgreementSessionUrl() = 0; //args: 0
    virtual void GetDurationControl() = 0; //args: 0
    virtual void GetDurationControlForApp() = 0; //args: 1
    virtual void BSetDurationControlOnlineState() = 0; //args: 1
    virtual void BSetDurationControlOnlineStateForApp() = 0; //args: 2
    virtual void BGetDurationControlExtendedResults() = 0; //args: 3
    virtual void BIsSubscribedApp() = 0; //args: 1
    virtual void GetSubscribedApps() = 0; //args: 3
    virtual void RegisterActivationCode() = 0; //args: 1
    virtual void AckSystemIM() = 0; //args: 2
    virtual void RequestSpecialSurvey() = 0; //args: 1
    virtual void SendSpecialSurveyResponse() = 0; //args: 3
    virtual void RequestNotifications() = 0; //args: 0
    virtual void GetAppOwnershipInfo() = 0; //args: 4
    virtual void SendGameWebCallback() = 0; //args: 2
    virtual void BIsStreamingUIToRemoteDevice() = 0; //args: 0
    virtual void BIsCurrentlyNVStreaming() = 0; //args: 0
    virtual void OnBigPictureForStreamingStartResult() = 0; //args: 2
    virtual void OnBigPictureForStreamingDone() = 0; //args: 0
    virtual void OnBigPictureForStreamingRestarting() = 0; //args: 0
    virtual void StopStreaming() = 0; //args: 1
    virtual void LockParentalLock() = 0; //args: 0
    virtual void UnlockParentalLock() = 0; //args: 1
    virtual void BIsParentalLockEnabled() = 0; //args: 0
    virtual void BIsParentalLockLocked() = 0; //args: 0
    virtual void BlockApp() = 0; //args: 1
    virtual void UnblockApp() = 0; //args: 1
    virtual void BIsAppBlocked() = 0; //args: 1
    virtual void BIsAppInBlockList() = 0; //args: 1
    virtual void BlockFeature() = 0; //args: 1
    virtual void UnblockFeature() = 0; //args: 1
    virtual void BIsFeatureBlocked() = 0; //args: 1
    virtual void BIsFeatureInBlockList() = 0; //args: 1
    virtual void GetParentalUnlockTime() = 0; //args: 0
    virtual void BGetRecoveryEmail() = 0; //args: 2
    virtual void RequestParentalRecoveryEmail() = 0; //args: 0
    virtual void BIsLockFromSiteLicense() = 0; //args: 0
    virtual void BGetSerializedParentalSettings() = 0; //args: 1
    virtual void BSetParentalSettings() = 0; //args: 1
    virtual void BDisableParentalSettings() = 0; //args: 0
    virtual void BGetParentalWebToken() = 0; //args: 2
    virtual void GetCommunityPreference() = 0; //args: 1
    virtual void SetCommunityPreference() = 0; //args: 2
    virtual void GetTextFilterSetting() = 0; //args: 0
    virtual void BTextFilterIgnoresFriends() = 0; //args: 0
    virtual void CanLogonOffline() = 0; //args: 0
    virtual void LogOnOffline() = 0; //args: 1
    virtual void ValidateOfflineLogonTicket() = 0; //args: 1
    virtual void BGetOfflineLogonTicket() = 0; //args: 2
    virtual void UploadLocalClientLogs() = 0; //args: 0
    virtual void SetAsyncNotificationEnabled() = 0; //args: 2
    virtual void BIsOtherSessionPlaying() = 0; //args: 1
    virtual void BKickOtherPlayingSession() = 0; //args: 0
    virtual void BIsAccountLockedDown() = 0; //args: 0
    virtual void ClearAndSetAppTags() = 0; //args: 2
    virtual void RemoveAppTag(AppId_t, const char* tag) = 0; //args: 2, index: 236
    virtual void AddAppTag(AppId_t, const char* tag) = 0; //args: 2, index: 237
    virtual void ClearAppTags(AppId_t) = 0; //args: 1, index: 238
    virtual void SetAppHidden(AppId_t, bool) = 0; //args: 2, index: 239
    virtual void RequestAccountLinkInfo() = 0; //args: 0
    virtual void RequestSurveySchedule() = 0; //args: 0
    virtual void RequestNewSteamAnnouncementState() = 0; //args: 0
    virtual void UpdateSteamAnnouncementLastRead() = 0; //args: 3
    virtual void GetMarketEligibility() = 0; //args: 0
    virtual void UpdateGameVrDllState() = 0; //args: 3
    virtual void KillVRTheaterPancakeGame() = 0; //args: 1
    virtual void BIsAnyGameOrServiceAppRunning() = 0; //args: 0
    virtual void BGetAppPlaytimeMap(CUtlMap<AppId_t, uint64> *mapOut) = 0; //args: 1
    virtual void BGetAppsLastPlayedMap(CUtlMap<AppId_t, uint64> *mapOut) = 0; //args: 1
    virtual bool BGetAppTagsMap(CUtlMap<AppId_t, AppTags_t> *mapOut) = 0; //args: 1
    virtual void SendSteamServiceStatusUpdate() = 0; //args: 2
    virtual void RequestSteamGroupChatMessageNotifications() = 0; //args: 5
    virtual void RequestSteamGroupChatMessageHistory() = 0; //args: 5
    virtual void RequestSendSteamGroupChatMessage() = 0; //args: 6
    virtual void OnNewGroupChatMsgAdded() = 0; //args: 8
    virtual void OnGroupChatUserStateChange() = 0; //args: 4
    virtual void OnReceivedGroupChatSubscriptionResponse() = 0; //args: 5
    virtual void GetTimedTrialStatus() = 0; //args: 3
    virtual void RequestTimedTrialStatus() = 0; //args: 1
    virtual void PrepareForSystemSuspend() = 0; //args: 0
    virtual void ResumeSuspendedGames() = 0; //args: 1
    virtual void GetClientInstallationID() = 0; //args: 0
    virtual void Test_SetClientInstallationID() = 0; //args: 2
    virtual AppId_t GetAppIDForGameID(CGameID) = 0; //args: 1
    virtual void BDoNotDisturb() = 0; //args: 0
    virtual void SetAdditionalClientArgData() = 0; //args: 1
};

#endif // ICLIENTUSER_H
