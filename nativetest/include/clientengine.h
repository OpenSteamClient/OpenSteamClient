#pragma once
#include <types.h>

class IClientApps;
class IClientBilling;
class IClientContentServer;
class IClientFriends;
class IClientGameCoordinator;
class IClientGameServer;
class IClientGameServerItems;
class IClientGameStats;
class IClientMasterServerUpdater;
class IClientMatchmaking;
class IClientMatchmakingServers;
class IClientNetworking;
class IClientRemoteStorage;
class IClientUser;
class IClientUserItems;
class IClientUserStats;
class IClientUtils;
class IP2PController;
class IClientAppManager;
class IClientDepotBuilder;
class IConCommandBaseAccessor;
class IClientGameCoordinator;
class IClientHTTP;
class IClientGameServerStats;
class IClientConfigStore;
class IClientScreenshots;
class IClientAudio;
class IClientUnifiedMessages;
class IClientStreamLauncher;
class IClientNetworkDeviceManager;
class IClientController;
class IClientParentalSettings;
class IClientDeviceAuth;
class IClientMusic;
class IClientProductBuilder;
class IClientRemoteClientManager;
class IClientRemoteControlManager;
class IClientShortcuts;
class IClientStreamClient;
class IClientUGC;
class IClientVR;
class IClientGameServerInternal;
class IClientGameServerPacketHandler;
class IClientGameSearch;
class IClientSystemManager;
class IClientSystemPerfManager;
class IClientSystemDockManager;
class IClientSystemAudioManager;
class IClientSystemDisplayManager;
class IClientInventory;
class IClientGameNotifications;
class IClientHTMLSurface;
class IClientVideo;
class IClientControllerSerialized;
class IClientAppDisableUpdate;
class IClientBluetoothManager;
class IClientSharedConnection;
class IClientShader;
class IClientNetworkingSocketsSerialized;
class IClientCompat;
class IClientParties;
class IClientNetworkingMessages;
class IClientNetworkingSockets;
class IClientNetworkingUtils;
class IClientNetworkingUtilsSerialized;
class IClientRemotePlay;

class IClientEngine {
public:
    virtual HSteamPipe CreateSteamPipe() = 0;
	virtual bool BReleaseSteamPipe( HSteamPipe hSteamPipe ) = 0;
	virtual HSteamUser CreateGlobalUser( HSteamPipe *phSteamPipe ) = 0;
	virtual HSteamUser ConnectToGlobalUser( HSteamPipe hSteamPipe ) = 0;
	virtual HSteamUser CreateLocalUser( HSteamPipe *phSteamPipe, EAccountType eAccountType ) = 0;
	virtual void CreatePipeToLocalUser( HSteamUser hSteamUser, HSteamPipe *phSteamPipe ) = 0;
	virtual void ReleaseUser( HSteamPipe hSteamPipe, HSteamUser hUser ) = 0;
    virtual bool IsValidHSteamUserPipe( HSteamPipe hSteamPipe, HSteamUser hUser ) = 0;
	virtual IClientUser *GetIClientUser( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientGameServerInternal *GetIClientGameServerInternal( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientGameServerPacketHandler *GetIClientGameServerPacketHandler( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
    virtual void SetLocalIPBinding( UInt32 ip, UInt16 port ) = 0;
    virtual const char* GetUniverseName( EUniverse universe ) = 0;
	virtual IClientFriends *GetIClientFriends( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientUtils *GetIClientUtils( HSteamPipe hSteamPipe ) = 0;
	virtual IClientBilling *GetIClientBilling( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientMatchmaking *GetIClientMatchmaking( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientApps *GetIClientApps( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientMatchmakingServers *GetIClientMatchmakingServers( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientGameSearch *GetIClientGameSearch( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual void RunFrame() = 0;
	virtual UInt32 GetIPCCallCount() = 0;
	virtual IClientUserStats *GetIClientUserStats( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientGameServerStats *GetIClientGameServerStats( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworking *GetIClientNetworking( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientRemoteStorage *GetIClientRemoteStorage( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientScreenshots *GetIClientScreenshots( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual void SetWarningMessageHook( void* func ) = 0;
	virtual IClientGameCoordinator *GetIClientGameCoordinator( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual void SetOverlayNotificationPosition( ENotificationPosition eNotificationPosition ) = 0;
	virtual void SetOverlayNotificationInsert( Int32 unk1, Int32 unk2 ) = 0;
	virtual bool HookScreenshots( bool bHook ) = 0;
	virtual bool IsScreenshotsHooked() = 0;
	virtual bool IsOverlayEnabled() = 0;
	virtual bool GetAPICallResult( HSteamPipe hSteamPipe, SteamAPICall_t hSteamAPICall, void* pCallback, int cubCallback, int iCallbackExpected, bool *pbFailed ) = 0;
	virtual IClientProductBuilder *GetIClientProductBuilder( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientDepotBuilder *GetIClientDepotBuilder( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworkDeviceManager *GetIClientNetworkDeviceManager( HSteamPipe hSteamPipe ) = 0;
	virtual IClientSystemPerfManager *GetIClientSystemPerfManager( HSteamPipe hSteamPipe ) = 0;
	virtual IClientSystemManager *GetIClientSystemManager( HSteamPipe hSteamPipe ) = 0;
	virtual IClientSystemDockManager *GetIClientSystemDockManager( HSteamPipe hSteamPipe ) = 0;
	virtual IClientSystemAudioManager *GetIClientSystemAudioManager( HSteamPipe hSteamPipe ) = 0;
	virtual IClientSystemDisplayManager *GetIClientSystemDisplayManager( HSteamPipe hSteamPipe ) = 0;
	virtual void ConCommandInit( IConCommandBaseAccessor* pAccessor ) = 0;
	virtual IClientAppManager *GetIClientAppManager( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientConfigStore *GetIClientConfigStore( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual bool BOverlayNeedsPresent() = 0;
	virtual IClientGameStats *GetIClientGameStats( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientHTTP *GetIClientHTTP( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual void FlushBeforeValidate() = 0;
	virtual bool BShutdownIfAllPipesClosed() = 0;
	virtual IClientAudio *GetIClientAudio( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientMusic *GetIClientMusic( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientUnifiedMessages *GetIClientUnifiedMessages( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientController *GetIClientController( HSteamPipe hSteamPipe ) = 0;
	virtual IClientParentalSettings *GetIClientParentalSettings( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientStreamLauncher *GetIClientStreamLauncher( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientDeviceAuth *GetIClientDeviceAuth( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientRemoteClientManager *GetIClientRemoteClientManager( HSteamPipe hSteamPipe ) = 0;
	virtual IClientStreamClient *GetIClientStreamClient( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientShortcuts *GetIClientShortcuts( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientUGC *GetIClientUGC( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientInventory *GetIClientInventory( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientVR *GetIClientVR( HSteamPipe hSteamPipe ) = 0;
	virtual IClientGameNotifications *GetIClientGameNotifications( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientHTMLSurface *GetIClientHTMLSurface( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientVideo *GetIClientVideo( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientControllerSerialized *GetIClientControllerSerialized( HSteamPipe hSteamPipe ) = 0;
	virtual IClientAppDisableUpdate *GetIClientAppDisableUpdate( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual unknown_ret Set_ClientAPI_CPostAPIResultInProcess( void* callback ) = 0;
	virtual IClientBluetoothManager *GetIClientBluetoothManager( HSteamPipe hSteamPipe) = 0;
	virtual IClientSharedConnection *GetIClientSharedConnection( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientShader *GetIClientShader( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworkingSocketsSerialized *GetIClientNetworkingSocketsSerialized( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientCompat *GetIClientCompat( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual void SetClientCommandLine( Int32 argc, char** argv ) = 0; 
	virtual IClientParties *GetIClientParties( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworkingMessages *GetIClientNetworkingMessages( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworkingSockets *GetIClientNetworkingSockets( HSteamUser hSteamUser, HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworkingUtils *GetIClientNetworkingUtils( HSteamPipe hSteamPipe ) = 0;
	virtual IClientNetworkingUtilsSerialized *GetIClientNetworkingUtilsSerialized( HSteamPipe hSteamPipe ) = 0;
	virtual IClientRemotePlay *GetIClientRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe) = 0;
    virtual void Destructor1() = 0;
	virtual void Destructor2() = 0;
	virtual void* GetIPCServerMap() = 0;
    virtual void OnDebugTextArrived(const char* msg) = 0;
	virtual unknown_ret OnThreadLocalRegistration() = 0;
    virtual unknown_ret OnThreadBuffersOverLimit() = 0;
};