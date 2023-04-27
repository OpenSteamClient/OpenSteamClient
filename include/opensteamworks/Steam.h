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


#ifndef STEAM_H
#define STEAM_H
#ifdef _WIN32
#pragma once
#endif


#if defined(__GNUC__) && defined(_WIN32)
	// This ugly hack allows us to provide GCC compatibility on windows without much effort
	#pragma push_macro("virtual")
	#undef virtual
	#define virtual virtual __thiscall
#endif

#if defined(__GNUC__) && defined(_WIN32)
	#pragma pop_macro("virtual")
#endif


// steam.dll exports

/*
** Initialization
*/

S_API int STEAM_CALL SteamStartEngine( TSteamError *pError );
S_API int STEAM_CALL SteamStartEngineEx( TSteamError *pError, bool bStartOffline, bool bDetectOnlineOfflineState );
S_API int STEAM_CALL SteamStartup( unsigned int uUsingMask, TSteamError *pError );

S_API int STEAM_CALL SteamGetVersion( char *szVersion, unsigned int uVersionBufSize );
S_API int STEAM_CALL SteamGetLocalClientVersion( unsigned int* puBootstrapperVersion, unsigned int* puClientVersion, TSteamError *pError );

S_API int STEAM_CALL SteamCleanup( TSteamError *pError );
S_API int STEAM_CALL SteamShutdownEngine( TSteamError *pError );
S_API int STEAM_CALL SteamShutdownSteamBridgeInterface( TSteamError *pError );

/*
** Asynchrounous call handling
*/

S_API int STEAM_CALL SteamProcessCall( SteamCallHandle_t handle, TSteamProgress *pProgress, TSteamError *pError );
S_API int STEAM_CALL SteamAbortCall( SteamCallHandle_t handle, TSteamError *pError );
S_API int STEAM_CALL SteamBlockingCall( SteamCallHandle_t handle, uint32 uiProcessTickMS, TSteamError *pError );
S_API int STEAM_CALL SteamSetMaxStallCount( uint32 uNumStalls, TSteamError *pError );

/*
** Filesystem
*/


S_API int STEAM_CALL SteamMountAppFilesystem( TSteamError *pError );
S_API int STEAM_CALL SteamUnmountAppFilesystem( TSteamError *pError );

S_API SteamHandle_t STEAM_CALL SteamMountFilesystem( unsigned int uAppId, const char *szMountPath, TSteamError *pError );
S_API int STEAM_CALL SteamUnmountFilesystem( SteamHandle_t hFile, TSteamError *pError );

S_API SteamHandle_t STEAM_CALL SteamOpenFile2( const char* cszPath, const char* cszMode, int nFlags, unsigned int* puFileSize, int* pbLocal, TSteamError* pError );
S_API SteamHandle_t STEAM_CALL SteamOpenFileEx( const char *cszPath, const char *cszMode, unsigned int *puFileSize, TSteamError *pError );
S_API SteamHandle_t STEAM_CALL SteamOpenFile64( const char *cszPath, const char *cszMode, SteamUnsigned64_t *pu64FileSize, TSteamError *pError );
S_API SteamHandle_t STEAM_CALL SteamOpenFile( const char *cszPath, const char *cszMode, TSteamError *pError );
S_API SteamHandle_t STEAM_CALL SteamOpenTmpFile( TSteamError *pError );

S_API int STEAM_CALL SteamFlushFile( SteamHandle_t hFile, TSteamError *pError );
S_API int STEAM_CALL SteamCloseFile( SteamHandle_t hFile, TSteamError *pError );

S_API int STEAM_CALL SteamSetvBuf( SteamHandle_t hFile, void* pBuf, ESteamBufferMethod eMethod, unsigned int uBytes, TSteamError *pError );
S_API int STEAM_CALL SteamGetc( SteamHandle_t hFile, TSteamError *pError );
S_API int STEAM_CALL SteamPutc( int cChar, SteamHandle_t hFile, TSteamError *pError );

S_API int STEAM_CALL SteamPrintFile( SteamHandle_t hFile, TSteamError *pError, const char *cszFormat, ... );
S_API unsigned int STEAM_CALL SteamReadFile( void *pBuf, unsigned int uSize, unsigned int uCount, SteamHandle_t hFile, TSteamError *pError );
S_API unsigned int STEAM_CALL SteamWriteFile( const void *pBuf, unsigned int uSize, unsigned int uCount, SteamHandle_t hFile, TSteamError *pError );

S_API int STEAM_CALL SteamSeekFile64( SteamHandle_t hFile, SteamSigned64_t s64Offset, ESteamSeekMethod eMethod, TSteamError *pError );
S_API int STEAM_CALL SteamSeekFile( SteamHandle_t hFile, long int lOffset, ESteamSeekMethod eMethod, TSteamError *pError );
S_API SteamSigned64_t STEAM_CALL SteamSizeFile64( SteamHandle_t hFile, TSteamError *pError );
S_API long STEAM_CALL SteamSizeFile( SteamHandle_t hFile, TSteamError *pError );
S_API SteamSigned64_t STEAM_CALL SteamTellFile64( SteamHandle_t hFile, TSteamError *pError );
S_API long STEAM_CALL SteamTellFile( SteamHandle_t hFile, TSteamError *pError );

S_API int STEAM_CALL SteamStat64( const char *cszName, TSteamElemInfo64 *pInfo, TSteamError *pError );
S_API int STEAM_CALL SteamStat( const char *cszName, TSteamElemInfo *pInfo, TSteamError *pError );

S_API SteamHandle_t STEAM_CALL SteamFindFirst64( const char *cszPattern, ESteamFindFilter eFilter, TSteamElemInfo64 *pFindInfo, TSteamError *pError );
S_API SteamHandle_t STEAM_CALL SteamFindFirst( const char *cszPattern, ESteamFindFilter eFilter, TSteamElemInfo *pFindInfo, TSteamError *pError );
S_API int STEAM_CALL SteamFindNext64( SteamHandle_t hDirectory, TSteamElemInfo64 *pFindInfo, TSteamError *pError );
S_API int STEAM_CALL SteamFindNext( SteamHandle_t hDirectory, TSteamElemInfo *pFindInfo, TSteamError *pError );
S_API int STEAM_CALL SteamFindClose( SteamHandle_t hDirectory, TSteamError *pError );

S_API int STEAM_CALL SteamGetLocalFileCopy( const char *cszPath, TSteamError *pError );

S_API int STEAM_CALL SteamIsFileImmediatelyAvailable( const char *cszName, TSteamError *pError );

S_API int STEAM_CALL SteamHintResourceNeed( const char* cszMasterList, int bForgetEverything, TSteamError* pError );
S_API int STEAM_CALL SteamForgetAllHints( TSteamError *pError );

S_API int STEAM_CALL SteamPauseCachePreloading( TSteamError *pError );
S_API int STEAM_CALL SteamResumeCachePreloading( TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamWaitForResources( const char *cszMasterList, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamFlushCache( unsigned int uAppId, TSteamError *pError );

S_API int STEAM_CALL SteamGetCacheDecryptionKey( unsigned int uCacheId, char* pchKeyBuffer, unsigned int cubBuff, unsigned int* pcubKey, TSteamError *pError );
S_API int STEAM_CALL SteamGetCacheDefaultDirectory( char *szPath, TSteamError *pError );
S_API int STEAM_CALL SteamSetCacheDefaultDirectory( const char *szPath, TSteamError *pError );
S_API int STEAM_CALL SteamGetCacheFilePath( unsigned int uCacheId,  char* szPathBuf, unsigned int uBufSize, unsigned int* puPathChars, TSteamError *pError );

S_API int STEAM_CALL SteamIsFileNeededByCache( unsigned int uCacheId, const char* pchFileName, SteamUnsigned64_t u64FileSize, TSteamError* pError );

S_API int STEAM_CALL SteamIsFileNeededByApp( unsigned int uAppId, const char* pchFileName, SteamUnsigned64_t u64FileSize, unsigned int* puCacheId, TSteamError* pError );

S_API int STEAM_CALL SteamRepairOrDecryptCaches( unsigned int uAppId, int bForceValidation, TSteamError *pError );

S_API int STEAM_CALL SteamCreateCachePreloaders( TSteamError *pError );
S_API int STEAM_CALL SteamIsCacheLoadingEnabled( unsigned int uAppId, int *pbIsLoading, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamLoadCacheFromDir( unsigned int uAppId, const char *szPath, TSteamError *pError );
S_API int STEAM_CALL SteamLoadFileToApp( unsigned int uAppId, const char* pchFileName, const void* pubDataChunk, unsigned int cubDataChunk, SteamUnsigned64_t cubDataOffset, TSteamError *pError );
S_API int STEAM_CALL SteamLoadFileToCache( unsigned int uCacheId, const char* pchFileName, const void* pubDataChunk, unsigned int cubDataChunk, SteamUnsigned64_t cubDataOffset, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamStartLoadingCache( unsigned int uAppId, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamStopLoadingCache( unsigned int uAppId, TSteamError *pError );

S_API int STEAM_CALL SteamGetFileAttributeFlags( const char* cszName, int* puFlags, TSteamError *pError );

S_API int STEAM_CALL SteamReleaseCacheFiles( unsigned int uAppId, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamRefreshMinimumFootprintFiles( unsigned int uAppId, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamDefragCaches( unsigned int uAppId, TSteamError* pError );
S_API SteamCallHandle_t STEAM_CALL SteamWaitForAppResources( unsigned int uAppId, const char* cszMasterList, TSteamError* pError );

S_API SteamCallHandle_t STEAM_CALL SteamGetCachePercentFragmentation( unsigned int uAppId, unsigned int* puPctFragmentation, TSteamError* pError );

/*
** Logging
*/

S_API SteamHandle_t STEAM_CALL SteamCreateLogContext( const char *cszName );
S_API int STEAM_CALL SteamLog( SteamHandle_t hContext, const char *cszMsg );
S_API void STEAM_CALL SteamLogResourceLoadStarted( const char *cszMsg );
S_API void STEAM_CALL SteamLogResourceLoadFinished( const char *cszMsg );

/*
** Account
*/

S_API SteamCallHandle_t STEAM_CALL SteamCreateAccount( const char *cszUser, const char *cszEmailAddress, const char *cszPassphrase, const char *cszCreationKey, const char *cszPersonalQuestion, const char *cszAnswerToQuestion, int *pbCreated, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamDeleteAccount( TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamChangeAccountName( const char* cszCurrentPassphrase, const char* cszNewAccountName, TSteamError* pError );
S_API SteamCallHandle_t STEAM_CALL SteamChangeEmailAddress( const char *cszNewEmailAddress, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamChangeForgottenPassword( const char* cszUser, const char* cszAnswerToQuestion, const char* cszEmailVerificationKey, const char* cszNewPassphrase, int* pbChanged, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamChangePassword( const char *cszCurrentPassphrase, const char *cszNewPassphrase, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamChangePersonalQA( const char *cszCurrentPassphrase, const char *cszNewPersonalQuestion, const char *cszNewAnswerToQuestion, TSteamError *pError );

S_API int STEAM_CALL SteamEnumerateApp( unsigned int uAppId, TSteamApp *pApp, TSteamError *pError );
S_API int STEAM_CALL SteamEnumerateAppDependency( unsigned int uAppId, unsigned int uIndex, TSteamAppDependencyInfo *pDependencyInfo, TSteamError *pError);
S_API int STEAM_CALL SteamEnumerateAppIcon( unsigned int uAppId, unsigned int uIconIndex, unsigned char *pIconData, unsigned int uIconDataBufSize, unsigned int *puSizeOfIconData, TSteamError *pError );
S_API int STEAM_CALL SteamEnumerateAppLaunchOption( unsigned int uAppId, unsigned int uLaunchOptionIndex, TSteamAppLaunchOption *pLaunchOption, TSteamError *pError );
S_API int STEAM_CALL SteamEnumerateAppVersion( unsigned int uAppId, unsigned int uVersionIndex, TSteamAppVersion *pAppVersion, TSteamError *pError );
S_API int STEAM_CALL SteamEnumerateSubscription( unsigned int uSubscriptionId, TSteamSubscription *pSubscription, TSteamError *pError );

S_API int STEAM_CALL SteamEnumerateSubscriptionDiscount( unsigned int uSubscriptionId, unsigned int uDiscountIndex, TSteamSubscriptionDiscount* pSubscriptionDiscount, TSteamError *pError );
S_API int STEAM_CALL SteamEnumerateSubscriptionDiscountQualifier( unsigned int uSubscriptionId, unsigned int uDiscountIndex, unsigned int uQualifierIndex, TSteamDiscountQualifier* pDiscountQualifier, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamGenerateSuggestedAccountNames( const char* cszAccountNameToSelectMasterAS, const char* cszGenerateNamesLikeAccountName, char* pSuggestedNamesBuf, unsigned int uBufSize, unsigned int* puNumSuggestedChars, TSteamError *pError );

S_API int STEAM_CALL SteamGetAccountStatus( unsigned int* puAccountStatusFlags, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamGetAppCacheSize( unsigned int uAppId, unsigned int *pCacheSizeInMb, TSteamError *pError );
S_API int STEAM_CALL SteamGetAppDependencies( unsigned int uAppId, unsigned int puCacheIds[], unsigned int uMaxIds, TSteamError *pError );
S_API int STEAM_CALL SteamGetAppDir( unsigned int uAppId, char* szAppDir, TSteamError *pError );
S_API int STEAM_CALL SteamGetAppIds( unsigned int puIds[], unsigned int uMaxIds, TSteamError *pError );
S_API int STEAM_CALL SteamGetAppPurchaseCountry( unsigned int uAppId, char* szCountryBuf, unsigned int uBufSize, unsigned int* pPurchaseTime, TSteamError* pError );
S_API int STEAM_CALL SteamGetAppStats( TSteamAppStats *pAppStats, TSteamError *pError );
S_API int STEAM_CALL SteamGetAppUpdateStats( unsigned int uAppId, ESteamAppUpdateStatsQueryType eQueryType, TSteamUpdateStats* pUpdateStats, TSteamError* pError );
S_API int STEAM_CALL SteamGetAppUserDefinedInfo( unsigned int uAppId, const char *cszPropertyName, char *szPropertyValue, unsigned int uBufSize, unsigned int *puPropertyValueLength, TSteamError *pError );
S_API int STEAM_CALL SteamGetAppUserDefinedRecord(unsigned int uAppId, KeyValueIteratorCallback_t AddEntryToKeyValueFunc, void* pvCKeyValue, TSteamError *pError);

S_API int STEAM_CALL SteamGetCurrentEmailAddress( char *szEmailaddress, unsigned int uBufSize, unsigned int *puEmailaddressChars, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamGetNumAccountsWithEmailAddress( const char *cszEmailAddress, unsigned int *puNumAccounts, TSteamError *pError );

S_API int STEAM_CALL SteamGetSponsorUrl( unsigned int uAppId, char *szUrl, unsigned int uBufSize, unsigned int *pUrlChars, TSteamError *pError );

S_API int STEAM_CALL SteamGetSubscriptionExtendedInfo( unsigned int uSubscriptionId, const char* cszKeyName, char* szKeyValue, unsigned int uBufferLength, unsigned int* puRecievedLength, TSteamError *pError );
S_API int STEAM_CALL SteamGetSubscriptionIds( unsigned int puIds[], unsigned int uMaxIds, TSteamError *pError );
S_API int STEAM_CALL SteamGetSubscriptionPurchaseCountry( unsigned int uSubscriptionId, char* szCountryBuf, unsigned int uBufSize , int* pPurchaseTime, TSteamError *pError );
S_API int STEAM_CALL SteamGetSubscriptionReceipt( unsigned int uSubscriptionId , TSteamSubscriptionReceipt* pSteamSubscriptionReceipt, TSteamError *pError );
S_API int STEAM_CALL SteamGetSubscriptionStats( TSteamSubscriptionStats *pSubscriptionStats, TSteamError *pError );

S_API int STEAM_CALL SteamGetTotalUpdateStats( TSteamUpdateStats *pUpdateStats, TSteamError *pError );

S_API int STEAM_CALL SteamGetUser( char *szUser, unsigned int uBufSize, unsigned int *puUserChars, TSteamGlobalUserID* pOptionalReceiveUserID, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamSetUser( const char *cszUser, int *pbUserSet, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamSetUser2( const char *cszUser, TSteamError *pError );

S_API int STEAM_CALL SteamGetUserType( unsigned int* puUserTypeFlags, TSteamError* pError );

S_API int STEAM_CALL SteamIsAccountNameInUse( const char* cszAccountName, int* pbIsUsed, TSteamError *pError );

S_API int STEAM_CALL SteamIsAppSubscribed( unsigned int uAppId, int *pbIsAppSubscribed, int *pbIsSubscriptionPending, TSteamError *pError );
S_API int STEAM_CALL SteamIsSubscribed( unsigned int uSubscriptionId, int *pbIsSubscribed, int *pbIsSubscriptionPending, TSteamError *pError );

S_API int STEAM_CALL SteamIsLoggedIn( int *pbIsLoggedIn, TSteamError *pError );
S_API int STEAM_CALL SteamIsSecureComputer( int *pbIsSecureComputer, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamLaunchApp( unsigned int uAppId, unsigned int uLaunchOption, const char *cszArgs, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamLogin( const char *cszUser, const char *cszPassphrase, int bIsSecureComputer, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamLogout( TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamMoveApp( unsigned int uAppId, const char *szPath, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamRefreshAccountInfo( TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamRefreshAccountInfoEx( int bContentDescriptionOnly, TSteamError* pError );
S_API SteamCallHandle_t STEAM_CALL SteamRefreshAccountInfo2( int bRefreshAccount, int bRefreshCDDB, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamRefreshLogin( const char *cszPassphrase, int bIsSecureComputer, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamRequestAccountsByCdKeyEmail( const char* cszCdKey, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamRequestAccountsByEmailAddressEmail( const char* cszEmailAddress, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamRequestEmailAddressVerificationEmail( TSteamError* pError );
S_API SteamCallHandle_t STEAM_CALL SteamRequestForgottenPasswordEmail( const char* cszUser, char* ReceivePersonalQuestion, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamSubscribe( unsigned int uSubscriptionId, const TSteamSubscriptionBillingInfo *pSubscriptionBillingInfo, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamUnsubscribe( unsigned int uSubscriptionId, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamUpdateAccountBillingInfo( const TSteamPaymentCardInfo *pPaymentCardInfo, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamUpdateSubscriptionBillingInfo( unsigned int uSubscriptionId, const TSteamSubscriptionBillingInfo *pSubscriptionBillingInfo, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamVerifyEmailAddress( const char* cszEmailVerificationKey, TSteamError* pError );
S_API int STEAM_CALL SteamVerifyPassword( const char* cszPassword, int* pbCorrect, TSteamError* pError );

S_API SteamCallHandle_t STEAM_CALL SteamWaitForAppReadyToLaunch( unsigned int uAppId, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamAckSubscriptionReceipt( unsigned int uSubscriptionId, TSteamError *pError );
S_API int STEAM_CALL SteamRemoveAppDependency( unsigned int uAppId, unsigned int uIndex, TSteamError *pError );
S_API int STEAM_CALL SteamInsertAppDependency( unsigned int uAppId, unsigned int uIndex, TSteamAppDependencyInfo *pDependencyInfo, TSteamError *pError );

S_API SteamCallHandle_t STEAM_CALL SteamSetAppCacheSize( unsigned int uAppId, unsigned int nCacheSizeInMb, TSteamError *pError );
S_API SteamCallHandle_t STEAM_CALL SteamSetAppVersion( unsigned int uAppId, unsigned int uAppVersionId, TSteamError *pError );

S_API unsigned int STEAM_CALL SteamNumAppsRunning( TSteamError *pError );
S_API int STEAM_CALL SteamFindApp( const char* cszAppName, unsigned int* puAppId, TSteamError *pError );

S_API int STEAM_CALL SteamCheckAppOwnership( unsigned int uAppId, int* pbOwned, TSteamGlobalUserID* pSteamID, TSteamError* pError );

/*
** Minidump
*/

S_API void STEAM_CALL SteamMiniDumpInit();

S_API int STEAM_CALL SteamWriteMiniDumpFromAssert(unsigned int unknown1, unsigned int unknown2, unsigned int unknown3, const char *szMessage, const char *szFileName);
S_API int STEAM_CALL SteamWriteMiniDumpSetComment( const char* cszComment );
S_API int STEAM_CALL SteamWriteMiniDumpUsingExceptionInfo();
S_API int STEAM_CALL SteamWriteMiniDumpUsingExceptionInfoWithBuildId();


/*
** User ID functions
*/

S_API ESteamError STEAM_CALL SteamGetEncryptedUserIDTicket( const void *pEncryptionKeyReceivedFromAppServer, unsigned int uEncryptionKeyLength, void *pOutputBuffer, unsigned int uSizeOfOutputBuffer, unsigned int *pReceiveSizeOfEncryptedTicket, TSteamError *pError );

S_API ESteamError STEAM_CALL SteamInitializeUserIDTicketValidator( const char * pszOptionalPublicEncryptionKeyFilename, const char *	pszOptionalPrivateDecryptionKeyFilename, unsigned int ClientClockSkewToleranceInSeconds, unsigned int ServerClockSkewToleranceInSeconds, unsigned int MaxNumLoginsWithinClientClockSkewTolerancePerClient, unsigned int	HintPeakSimultaneousValidations, unsigned int AbortValidationAfterStallingForNProcessSteps );
S_API ESteamError STEAM_CALL SteamShutdownUserIDTicketValidator();

S_API const unsigned char* STEAM_CALL SteamGetEncryptionKeyToSendToNewClient( unsigned int * pReceiveSizeOfEncryptionKey );

S_API ESteamError STEAM_CALL SteamStartValidatingUserIDTicket( void *pEncryptedUserIDTicketFromClient, unsigned int uSizeOfEncryptedUserIDTicketFromClient, unsigned int ObservedClientIPAddr, SteamUserIDTicketValidationHandle_t *pReceiveHandle );
S_API ESteamError STEAM_CALL SteamStartValidatingNewValveCDKey( void *pEncryptedNewValveCDKeyFromClient, unsigned int uSizeOfEncryptedNewValveCDKeyFromClient, unsigned int ObservedClientIPAddr, struct sockaddr *pPrimaryValidateNewCDKeyServerSockAddr, struct sockaddr *pSecondaryValidateNewCDKeyServerSockAddr, SteamUserIDTicketValidationHandle_t *pReceiveHandle );

S_API ESteamError STEAM_CALL SteamProcessOngoingUserIDTicketValidation( SteamUserIDTicketValidationHandle_t Handle, TSteamGlobalUserID *pReceiveValidSteamGlobalUserID, unsigned int *pReceiveClientLocalIPAddr, unsigned char *pOptionalReceiveProofOfAuthenticationToken, size_t SizeOfOptionalAreaToReceiveProofOfAuthenticationToken, size_t *pOptionalReceiveSizeOfProofOfAuthenticationToken );

S_API void STEAM_CALL SteamAbortOngoingUserIDTicketValidation( SteamUserIDTicketValidationHandle_t Handle );

S_API ESteamError STEAM_CALL SteamOptionalCleanUpAfterClientHasDisconnected( unsigned int ObservedClientIPAddr, unsigned int ClientLocalIPAddr );


/*
** Misc
*/

S_API void STEAM_CALL SteamClearError( TSteamError *pError );

S_API unsigned int STEAM_CALL InternalSteamNumClientsConnectedToEngine( TSteamError *pError );
S_API int STEAM_CALL InternalSteamShouldShutdownEngine2( TSteamError *pError );

S_API int STEAM_CALL SteamChangeOfflineStatus( TSteamOfflineStatus* pSteamOfflineStatus, TSteamError* pError );
S_API int STEAM_CALL SteamGetOfflineStatus( TSteamOfflineStatus* pSteamOfflineStatus, TSteamError* pError );

S_API SteamCallHandle_t STEAM_CALL SteamUninstall( TSteamError *pError );

S_API int STEAM_CALL SteamWeakVerifyNewValveCDKey( const char *pszCDKeyFormattedForCDLabel, unsigned int *pReceiveGameCode, unsigned int *pReceiveSalesTerritoryCode, unsigned int *pReceiveUniqueSerialNumber );
S_API int STEAM_CALL SteamGetEncryptedNewValveCDKey( const char *pszCDKeyFormattedForCDLabel, unsigned int ClientLocalIPAddr, const void * pEncryptionKeyReceivedFromAppServer, unsigned int uEncryptionKeyLength, void* pOutputBuffer, unsigned int uSizeOfOutputBuffer, unsigned int* pReceiveSizeOfEncryptedNewValveCDKey );

// Encrypts machine-specific data from the machine that is executing.
S_API ESteamError STEAM_CALL SteamDecryptDataForThisMachine
(
	const uint8 *pEncryptedData,	// [in]		The encrypted data to decrypt. This is usually stored as a hex string.
	uint32 uEncryptedDataSize,		// [in]		The size of the encrypted data in bytes. This is usually the number of characters in the hex string.
	uint8 *pDecryptedBuffer,		// [in/out]	The buffer to decrypt the data into.
	uint32 uDecryptedBufferSize,	// [in]		The size of the decryption buffer in bytes.
	uint32 *puDecryptedDataSize		// [out]	The number of bytes that were decrypted.
);
// Decrypts machine-specific data from the machine that is executing.
S_API ESteamError STEAM_CALL SteamEncryptDataForThisMachine
(
	const uint8 *pPlaintextData,	// [in]		The plaintext data to encrypt.
	uint32 uPlaintextDataSize,		// [in]		The size of the plaintext data in bytes.
	uint8 *pEncryptedBuffer,		// [in/out]	The buffer to encrypt the data into.
	uint32 uEncryptedBufferSize,	// [in]		The size of the encryption buffer in bytes.
	uint32 *puEncryptedDataSize		// [out]	The number of bytes that were used in the encryption buffer.
);

S_API const char* STEAM_CALL SteamFindServersGetErrorString();
S_API int STEAM_CALL SteamFindServersIterateServer(ESteamServerType eSteamServerType, unsigned int uIndex, char *szServerAddress, int iServerAddressChars);
S_API int STEAM_CALL SteamFindServersNumServers( ESteamServerType eSteamServerType );

S_API int STEAM_CALL SteamGetContentServerInfo( unsigned int uAppId, unsigned int* pServerId, unsigned int* pServerIpAddress, TSteamError *pError );

S_API int STEAM_CALL SteamSetNotificationCallback( SteamNotificationCallback_t pCallbackFunction, TSteamError *pError );

S_API int STEAM_CALL SteamGetAppDLCStatus( unsigned int uAppId, unsigned int uDLCCacheId, int *pbDownloaded, TSteamError *pError );

S_API int STEAM_CALL SteamIsUsingSdkContentServer( int *pbUsingSdkCS, TSteamError *pError );

S_API int STEAM_CALL SteamForceCellId( unsigned int uCellId, TSteamError* pError );

S_API int STEAM_CALL SteamWasBlobRegistryDeleted( int *puWasDeleted, TSteamError *pError );

S_API int STEAM_CALL SteamGetDepotParent( unsigned int uDepotId, unsigned int* puParentId, TSteamError* pError );

S_API int STEAM_CALL GetCurrentCellID( unsigned int* puCellID, unsigned int* puPing, TSteamError* pError );

#endif // STEAM_H
