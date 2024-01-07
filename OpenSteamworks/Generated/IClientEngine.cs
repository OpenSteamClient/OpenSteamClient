using System;
using OpenSteamworks.Enums;
using OpenSteamworks.Native;
using OpenSteamworks.ConCommands;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenSteamworks.Generated;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void ClientAPI_WarningMessageHook_t(int nSeverity, string pchDebugText);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void ClientAPI_PostAPIResultInProcess_t(SteamAPICall_t callHandle, IntPtr callbackData, uint unCallbackSize, int iCallbackNum);

public unsafe interface IClientEngine {
    public HSteamPipe CreateSteamPipe();
	public bool BReleaseSteamPipe( HSteamPipe hSteamPipe );
	public HSteamUser CreateGlobalUser( ref HSteamPipe phSteamPipe );
	public HSteamUser ConnectToGlobalUser( HSteamPipe hSteamPipe );
	public HSteamUser CreateLocalUser( ref HSteamPipe phSteamPipe, EAccountType eAccountType );
	public void CreatePipeToLocalUser( HSteamUser hSteamUser, ref HSteamPipe phSteamPipe );
	public void ReleaseUser( HSteamPipe hSteamPipe, HSteamUser hUser );
    public bool IsValidHSteamUserPipe( HSteamPipe hSteamPipe, HSteamUser hUser );
	public IClientUser GetIClientUser( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientGameServerInternal GetIClientGameServerInternal( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientGameServerPacketHandler GetIClientGameServerPacketHandler( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
    public void SetLocalIPBinding( UInt32 ip, UInt16 port );
    public string GetUniverseName( EUniverse universe );
	public IClientFriends GetIClientFriends( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientUtils GetIClientUtils( HSteamPipe hSteamPipe );
	public IClientBilling GetIClientBilling( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientMatchmaking GetIClientMatchmaking( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientApps GetIClientApps( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientMatchmakingServers GetIClientMatchmakingServers( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientGameSearch GetIClientGameSearch( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	/// <summary>
    /// Runs a frame. 
    /// Needed for IClientHTMLSurface to send callbacks properly.
    /// What interval should we call this at? Calling this too quickly results in strange backoff times being logged in connection_log.txt
    /// </summary>
	public void RunFrame();
	/// <summary>
    /// Gets how many IPC calls have occurred (which way?).
    /// </summary>
	public UInt32 GetIPCCallCount();
	public IClientUserStats GetIClientUserStats( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientGameServerStats GetIClientGameServerStats( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientNetworking GetIClientNetworking( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientRemoteStorage GetIClientRemoteStorage( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientScreenshots GetIClientScreenshots( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	/// <summary>
    /// Sets a steam api warning message hook.
    /// There's lots of docs for this online, but you should use a SteamAPIWarningMessageHook_t
    /// It is currently unknown whether this affects any client api's (us)
    /// </summary>
	public void SetWarningMessageHook( ClientAPI_WarningMessageHook_t func );
	public IClientGameCoordinator GetIClientGameCoordinator( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public void SetOverlayNotificationPosition( ENotificationPosition eNotificationPosition );
	public void SetOverlayNotificationInsert( Int32 unk1, Int32 unk2 );
	public bool HookScreenshots( bool bHook );
	public bool IsScreenshotsHooked();
	public bool IsOverlayEnabled();
	/// <summary>
    /// Gets a API call's result
    /// </summary>
    /// <param name="hSteamPipe">Steam Pipe which contains the call</param>
    /// <param name="hSteamAPICall">The reference number of the call</param>
    /// <param name="pCallback">An array of bytes to store the call's result</param>
    /// <param name="cubCallback">The size of pCallback. </param>
    /// <param name="iCallbackExpected">k_iCallback number that you expected back</param>
    /// <param name="pbFailed">A bool that tells you if the callback failed</param>
    /// <returns>If the callback is finished or not</returns>
	public bool GetAPICallResult( HSteamPipe hSteamPipe, SteamAPICall_t hSteamAPICall, void* pCallback, int cubCallback, int iCallbackExpected, ref bool pbFailed );
	public IClientProductBuilder GetIClientProductBuilder( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientDepotBuilder GetIClientDepotBuilder( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientNetworkDeviceManager GetIClientNetworkDeviceManager( HSteamPipe hSteamPipe );
	public IClientSystemPerfManager GetIClientSystemPerfManager( HSteamPipe hSteamPipe );
	public IClientSystemManager GetIClientSystemManager( HSteamPipe hSteamPipe );
	public IClientSystemDockManager GetIClientSystemDockManager( HSteamPipe hSteamPipe );
	public IClientSystemAudioManager GetIClientSystemAudioManager( HSteamPipe hSteamPipe );
	public IClientSystemDisplayManager GetIClientSystemDisplayManager( HSteamPipe hSteamPipe );
	public void ConCommandInit( IConCommandBaseAccessor* pAccessor );
	public IClientAppManager GetIClientAppManager( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientConfigStore GetIClientConfigStore( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public bool BOverlayNeedsPresent();
	public IClientGameStats GetIClientGameStats( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientHTTP GetIClientHTTP( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public void FlushBeforeValidate();
	/// <summary>
    /// Shuts the client down if all pipes have been closed.
    /// </summary>
    /// <returns>If it closed successfully</returns>
	public bool BShutdownIfAllPipesClosed();
	public IClientAudio GetIClientAudio( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientMusic GetIClientMusic( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientUnifiedMessages GetIClientUnifiedMessages( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientController GetIClientController( HSteamPipe hSteamPipe );
	public IClientParentalSettings GetIClientParentalSettings( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientStreamLauncher GetIClientStreamLauncher( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientDeviceAuth GetIClientDeviceAuth( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientRemoteClientManager GetIClientRemoteClientManager( HSteamPipe hSteamPipe );
	public IClientStreamClient GetIClientStreamClient( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientShortcuts GetIClientShortcuts( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientUGC GetIClientUGC( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientInventory GetIClientInventory( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientVR GetIClientVR( HSteamPipe hSteamPipe );
	public IClientGameNotifications GetIClientGameNotifications( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientHTMLSurface GetIClientHTMLSurface( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientVideo GetIClientVideo( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientControllerSerialized GetIClientControllerSerialized( HSteamPipe hSteamPipe );
	public IClientAppDisableUpdate GetIClientAppDisableUpdate( HSteamUser hSteamUser, HSteamPipe hSteamPipe );

	/// <summary>
    /// We don't know what this does yet.
    /// You should use ClientAPI_PostAPIResultInProcess_t and store it somewhere to prevent it being GC'd.
    /// </summary>
	public unknown_ret Set_ClientAPI_CPostAPIResultInProcess( ClientAPI_PostAPIResultInProcess_t callback );
	public IClientBluetoothManager GetIClientBluetoothManager( HSteamPipe hSteamPipe);
	public IClientSharedConnection GetIClientSharedConnection( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientShader GetIClientShader( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientNetworkingSocketsSerialized GetIClientNetworkingSocketsSerialized( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientCompat GetIClientCompat( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public void SetClientCommandLine( Int32 argc, string[] argv ); 
	public IClientParties GetIClientParties( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientNetworkingMessages GetIClientNetworkingMessages( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientNetworkingSockets GetIClientNetworkingSockets( HSteamUser hSteamUser, HSteamPipe hSteamPipe );
	public IClientNetworkingUtils GetIClientNetworkingUtils( HSteamPipe hSteamPipe );
	public IClientNetworkingUtilsSerialized GetIClientNetworkingUtilsSerialized( HSteamPipe hSteamPipe );
	public IClientSTARInternal GetIClientSTARInternal(HSteamUser hSteamUser, HSteamPipe hSteamPipe);
	public IClientRemotePlay GetIClientRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe);
    public void Destructor1();
	public void Destructor2();
	/// <summary>
	/// What is the format of the data here?
	/// </summary>
	/// <returns></returns>
	public void* GetIPCServerMap();
    public void OnDebugTextArrived(string msg);
	/// <summary>
	/// Unsure if this exists
	/// </summary>
	public unknown_ret OnThreadLocalRegistration();

	/// <summary>
	/// Unsure if this exists
	/// </summary>
    public unknown_ret OnThreadBuffersOverLimit();
}
