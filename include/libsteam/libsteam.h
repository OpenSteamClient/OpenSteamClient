#include "../steamtypes.h"

extern "C" void *CreateInterface(const char *pName, int *pReturnCode);

extern "C" void CleanupFIFOs(char param_1);
extern "C" void CleanupSemaphores(char param_1);
/*
** Initialization and misc
*/
extern "C" int            SteamStartEngine( TSteamError *pError );
extern "C" int            SteamStartEngineEx( TSteamError *pError, bool bStartOffline, bool bDetectOnlineOfflineState );
extern "C" int            SteamStartup( unsigned int uUsingMask, TSteamError *pError );
extern "C" int            SteamCleanup( TSteamError *pError );
extern "C" void           SteamClearError( TSteamError *pError );
extern "C" int            SteamGetVersion( char *szVersion, unsigned int uVersionBufSize );
extern "C" int            SteamShutdownEngine( TSteamError *pError );
extern "C" int            SteamShutdownSteamBridgeInterface(TSteamError *param_1);

/*
** Asynchrounous call handling
*/
 
extern "C" int                SteamProcessCall( SteamCallHandle_t handle, TSteamProgress *pProgress, TSteamError *pError );
extern "C" int                SteamAbortCall( SteamCallHandle_t handle, TSteamError *pError );
extern "C" int                SteamBlockingCall( SteamCallHandle_t handle, unsigned int uiProcessTickMS, TSteamError *pError );
extern "C" int                SteamSetMaxStallCount( unsigned int uNumStalls, TSteamError *pError );
                             
/*
** Filesystem
*/
 
extern "C" int                SteamMountAppFilesystem( TSteamError *pError );
extern "C" int                SteamUnmountAppFilesystem( TSteamError *pError );
extern "C" int                SteamMountFilesystem( unsigned int uAppId, const char *szMountPath, TSteamError *pError );
extern "C" int                SteamUnmountFilesystem( const char *szMountPath, TSteamError *pError );
extern "C" int                SteamStat( const char *cszName, TSteamElemInfo *pInfo, TSteamError *pError );
extern "C" int                SteamSetvBuf( SteamHandle_t hFile, void* pBuf, ESteamBufferMethod eMethod, unsigned int uBytes, TSteamError *pError );
extern "C" int                SteamFlushFile( SteamHandle_t hFile, TSteamError *pError );
extern "C" SteamHandle_t      SteamOpenFile( const char *cszName, const char *cszMode, TSteamError *pError );
extern "C" SteamHandle_t      SteamOpenTmpFile( TSteamError *pError );
extern "C" int                SteamCloseFile( SteamHandle_t hFile, TSteamError *pError );
extern "C" unsigned int       SteamReadFile( void *pBuf, unsigned int uSize, unsigned int uCount, SteamHandle_t hFile, TSteamError *pError );
extern "C" unsigned int       SteamWriteFile( const void *pBuf, unsigned int uSize, unsigned int uCount, SteamHandle_t hFile, TSteamError *pError );
extern "C" int                SteamGetc( SteamHandle_t hFile, TSteamError *pError );
extern "C" int                SteamPutc( int cChar, SteamHandle_t hFile, TSteamError *pError );
extern "C" int                SteamPrintFile( SteamHandle_t hFile, TSteamError *pError, const char *cszFormat, ... );
extern "C" int                SteamSeekFile( SteamHandle_t hFile, long lOffset, ESteamSeekMethod, TSteamError *pError );
extern "C" long               SteamTellFile( SteamHandle_t hFile, TSteamError *pError );
extern "C" long               SteamSizeFile( SteamHandle_t hFile, TSteamError *pError );
extern "C" SteamHandle_t      SteamFindFirst( const char *cszPattern, ESteamFindFilter eFilter, TSteamElemInfo *pFindInfo, TSteamError *pError );
extern "C" int                SteamFindNext( SteamHandle_t hDirectory, TSteamElemInfo *pFindInfo, TSteamError *pError );
extern "C" int                SteamFindClose( SteamHandle_t hDirectory, TSteamError *pError );
extern "C" int                SteamGetLocalFileCopy( const char *cszName, TSteamError *pError );
extern "C" int                SteamIsFileImmediatelyAvailable( const char *cszName, TSteamError *pError );
extern "C" int                SteamHintResourceNeed( const char *cszMountPath, const char *cszMasterList, int bForgetEverything, TSteamError *pError );
extern "C" int                SteamForgetAllHints( const char *cszMountPath, TSteamError *pError );
extern "C" int                SteamPauseCachePreloading( const char *cszMountPath, TSteamError *pError );
extern "C" int                SteamResumeCachePreloading( const char *cszMountPath, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamWaitForResources( const char *cszMountPath, const char *cszMasterList, TSteamError *pError );

/*
** Logging
*/
 
extern "C" SteamHandle_t      SteamCreateLogContext( const char *cszName );
extern "C" int                SteamLog( SteamHandle_t hContext, const char *cszMsg );
extern "C" void               SteamLogResourceLoadStarted( const char *cszMsg );
extern "C" void               SteamLogResourceLoadFinished( const char *cszMsg );
 
/*
** Account
*/
 
extern "C" SteamCallHandle_t  SteamCreateAccount( const char *cszUser, const char *cszPassphrase, const char *cszCreationKey, const char *cszPersonalQuestion, const char *cszAnswerToQuestion, int *pbCreated, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamDeleteAccount( TSteamError *pError );
extern "C" int                SteamIsLoggedIn( int *pbIsLoggedIn, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamSetUser( const char *cszUser, int *pbUserSet, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamSetUser2( const char *cszUser, TSteamError *pError );
extern "C" int                SteamGetUser( char *szUser, unsigned int uBufSize, unsigned int *puUserChars, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamLogin( const char *cszUser, const char *cszPassphrase, int bIsSecureComputer, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamLogout( TSteamError *pError );
extern "C" int                SteamIsSecureComputer(  int *pbIsSecure, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamRefreshLogin( const char *cszPassphrase, int bIsSecureComputer, TSteamError * pError );
extern "C" SteamCallHandle_t  SteamSubscribe( unsigned int uSubscriptionId, const TSteamSubscriptionBillingInfo *pSubscriptionBillingInfo, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamUnsubscribe( unsigned int uSubscriptionId, TSteamError *pError );
extern "C" int                SteamIsSubscribed( unsigned int uSubscriptionId, int *pbIsSubscribed, TSteamError *pError );
extern "C" int                SteamIsAppSubscribed( unsigned int uAppId, int *pbIsAppSubscribed, TSteamError *pError );
extern "C" int                SteamGetSubscriptionStats( TSteamSubscriptionStats *pSubscriptionStats, TSteamError *pError );
extern "C" int                SteamGetSubscriptionIds( unsigned int *puIds, unsigned int uMaxIds, TSteamError *pError );
extern "C" int                SteamEnumerateSubscription( unsigned int uId, TSteamSubscription *pSubscription, TSteamError *pError );
extern "C" int                SteamGetAppStats( TSteamAppStats *pAppStats, TSteamError *pError );
extern "C" int                SteamGetAppIds( unsigned int *puIds, unsigned int uMaxIds, TSteamError *pError );
extern "C" int                SteamEnumerateApp( unsigned int uId, TSteamApp *pApp, TSteamError *pError );
extern "C" int                SteamEnumerateAppLaunchOption( unsigned int uAppId, unsigned int uLaunchOptionIndex, TSteamAppLaunchOption *pLaunchOption, TSteamError *pError );
extern "C" int                SteamEnumerateAppIcon( unsigned int uAppId, unsigned int uIconIndex, unsigned char *pIconData, unsigned int uIconDataBufSize, unsigned int *puSizeOfIconData, TSteamError *pError );
extern "C" int                SteamEnumerateAppVersion( unsigned int uAppId, unsigned int uVersionIndex, TSteamAppVersion *pAppVersion, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamWaitForAppReadyToLaunch( unsigned int uAppId, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamLaunchApp( unsigned int uAppId, unsigned int uLaunchOption, const char *cszArgs, TSteamError *pError );
extern "C" int                SteamIsCacheLoadingEnabled( unsigned int uAppId, int *pbIsLoading, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamStartLoadingCache( unsigned int uAppId, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamStopLoadingCache( unsigned int uAppId, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamFlushCache( unsigned int uAppId, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamLoadCacheFromDir( unsigned int uAppId, const char *szPath, TSteamError *pError );
extern "C" int                SteamGetCacheDefaultDirectory( char *szPath, TSteamError *pError );
extern "C" int                SteamSetCacheDefaultDirectory( const char *szPath, TSteamError *pError );
extern "C" int                SteamGetAppCacheDir( unsigned int uAppId, char *szPath, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamMoveApp( unsigned int uAppId, const char *szPath, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamGetAppCacheSize( unsigned int uAppId, unsigned int *pCacheSizeInMb, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamSetAppCacheSize( unsigned int uAppId, unsigned int nCacheSizeInMb, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamSetAppVersion( unsigned int uAppId, unsigned int uAppVersionId, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamUninstall( TSteamError *pError );
extern "C" int                SteamSetNotificationCallback( SteamNotificationCallback_t pCallbackFunction, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamSetNewPassword( const char *cszUser, const char *cszAnswerToQuestion, const SteamSalt_t *cpSaltForAnswer, const char *cszNewPassphrase, int *pbChanged, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamGetPersonalQuestion( const char *cszUser, SteamPersonalQuestion_t PersonalQuestion, SteamSalt_t *pSaltForAnswer, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamChangePassword( const char *cszCurrentPassphrase, const char *cszNewPassphrase, int *pbChanged, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamChangePersonalQA( const char *cszCurrentPassphrase, const char *cszNewPersonalQuestion, const char *cszNewAnswerToQuestion, int *pbChanged, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamChangeEmailAddress( const char *cszNewEmailAddress, int *pbChanged, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamEmailVerified( const char *cszEmailVerificationKey, int *pbVerified, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamSendVerificationEmail( int *pbSent, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamUpdateAccountBillingInfo( const TSteamPaymentCardInfo *pPaymentCardInfo, int *pbChanged, TSteamError *pError );
extern "C" SteamCallHandle_t  SteamUpdateSubscriptionBillingInfo( unsigned int uSubscriptionId, const TSteamSubscriptionBillingInfo *pSubscriptionBillingInfo, int *pbChanged, TSteamError *pError );
extern "C" int                SteamGetSponsorUrl( unsigned int uAppId, char *szUrl, unsigned int uBufSize, unsigned int *pUrlChars, TSteamError *pError );
extern "C" int                SteamGetAppUpdateStats( unsigned int uAppId, TSteamUpdateStats *pUpdateStats, TSteamError *pError );
extern "C" int                SteamGetTotalUpdateStats( TSteamUpdateStats *pUpdateStats, TSteamError *pError );
extern "C" int                GetCurrentCellID( unsigned int* puCellID, unsigned int* puPing, TSteamError* pError );