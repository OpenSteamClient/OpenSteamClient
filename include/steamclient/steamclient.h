#include "../steamtypes.h"

extern "C" void *CreateInterface(const char *pName, int *pReturnCode);

extern "C" void Breakpad_SteamMiniDumpInit(uint32 pAppId, const char *b, const char *c);
extern "C" void Breakpad_SteamSetAppID(AppId_t pAppId);
extern "C" void Breakpad_SteamSetSteamID(uint64 ulSteamID);
extern "C" void Breakpad_SteamWriteMiniDumpSetComment(const char *pchMsg);
extern "C" void Breakpad_SteamWriteMiniDumpUsingExceptionInfoWithBuildId(int a, int b);

extern "C" bool Steam_BConnected(HSteamUser hUser, HSteamPipe hSteamPipe);
extern "C" bool Steam_BGetCallback(HSteamPipe hSteamPipe, CallbackMsg_t *pCallbackMsg);
extern "C" bool Steam_BLoggedOn(HSteamUser hUser, HSteamPipe hSteamPipe);
extern "C" bool Steam_BReleaseSteamPipe(HSteamPipe hSteamPipe);
extern "C" HSteamUser Steam_ConnectToGlobalUser(HSteamPipe hSteamPipe);
extern "C" HSteamUser Steam_CreateGlobalUser(HSteamPipe *phSteamPipe);
extern "C" HSteamUser Steam_CreateLocalUser(HSteamPipe *phSteamPipe, EAccountType eAccountType);
extern "C" HSteamPipe Steam_CreateSteamPipe();
extern "C" void Steam_FreeLastCallback(HSteamPipe hSteamPipe);
extern "C" bool Steam_GSBLoggedOn(void *phSteamHandle);
extern "C" bool Steam_GSBSecure(void *phSteamHandle);
extern "C" bool Steam_GSGetSteam2GetEncryptionKeyToSendToNewClient(void *phSteamHandle, void *pvEncryptionKey, uint32 *pcbEncryptionKey, uint32 cbMaxEncryptionKey);
extern "C" uint64 Steam_GSGetSteamID();
extern "C" void Steam_GSLogOff(void *phSteamHandle);
extern "C" void Steam_GSLogOn(void *phSteamHandle);
extern "C" bool Steam_GSRemoveUserConnect(void *phSteamHandle, uint32 unUserID);
extern "C" bool Steam_GSSendSteam2UserConnect(void *phSteamHandle, uint32 unUserID, const void *pvRawKey, uint32 unKeyLen, uint32 unIPPublic, uint16 usPort, const void *pvCookie, uint32 cubCookie);
extern "C" bool Steam_GSSendSteam3UserConnect(void *phSteamHandle, uint64 steamID, uint32 unIPPublic, const void *pvCookie, uint32 cubCookie);
extern "C" bool Steam_GSSendUserDisconnect(void *phSteamHandle, uint64 ulSteamID, uint32 unUserID);
extern "C" bool Steam_GSSendUserStatusResponse(void *phSteamHandle, uint64 ulSteamID, int nSecondsConnected, int nSecondsSinceLast);
extern "C" bool Steam_GSSetServerType(void *phSteamHandle, int32 nAppIdServed, uint32 unServerFlags, uint32 unGameIP, uint32 unGamePort, const char *pchGameDir, const char *pchVersion);
extern "C" void Steam_GSSetSpawnCount(void *phSteamHandle, uint32 ucSpawn);
extern "C" bool Steam_GSUpdateStatus(void *phSteamHandle, int cPlayers, int cPlayersMax, int cBotPlayers, const char *pchServerName, const char *pchMapName);
extern "C" bool Steam_GetAPICallResult(HSteamPipe hSteamPipe, SteamAPICall_t hSteamAPICall, void *pCallback, int cubCallback, int iCallbackExpected, bool *pbFailed);
extern "C" void *Steam_GetGSHandle(HSteamUser hUser, HSteamPipe hSteamPipe);
extern "C" int Steam_InitiateGameConnection(HSteamUser hUser, HSteamPipe hSteamPipe, void *pBlob, int cbMaxBlob, uint64 steamID, int nGameAppID, uint32 unIPServer, uint16 usPortServer, bool bSecure);
extern "C" void Steam_LogOff(HSteamUser hUser, HSteamPipe hSteamPipe);
extern "C" void Steam_LogOn(HSteamUser hUser, HSteamPipe hSteamPipe);
extern "C" void Steam_ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser);
extern "C" void Steam_SetLocalIPBinding(uint32 unIP, uint16 usLocalPort);
extern "C" void Steam_TerminateGameConnection(HSteamUser hUser, HSteamPipe hSteamPipe, uint32 unIPServer, uint16 usPortServer);
extern "C" char *SteamRealPath(char *param_1, char *param_2, uint param_3);