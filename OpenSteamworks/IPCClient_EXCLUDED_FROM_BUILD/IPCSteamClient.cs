using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Callbacks;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamworks.IPCClient;

public class IPCSteamClient : ISteamClient {
    public IClientEngine IClientEngine => IPCClientEngine;
    public IPCClientEngine IPCClientEngine { get; private set; }
    public IClientAppDisableUpdate IClientAppDisableUpdate { get; private set; }
    public IClientAppManager IClientAppManager { get; private set; }
    public IClientApps IClientApps { get; private set; }
    public IClientAudio IClientAudio { get; private set; }
    public IClientBilling IClientBilling { get; private set; }
    public IClientBluetoothManager IClientBluetoothManager { get; private set; }
    public IClientCompat IClientCompat { get; private set; }
    public IClientConfigStore IClientConfigStore { get; private set; }
    public IClientController IClientController { get; private set; }
    public IClientControllerSerialized IClientControllerSerialized { get; private set; }
    public IClientDepotBuilder IClientDepotBuilder { get; private set; }
    public IClientDeviceAuth IClientDeviceAuth { get; private set; }
    public IClientFriends IClientFriends { get; private set; }
    public IClientGameCoordinator IClientGameCoordinator { get; private set; }
    public IClientGameNotifications IClientGameNotifications { get; private set; }
    public IClientGameSearch IClientGameSearch { get; private set; }
    public IClientGameStats IClientGameStats { get; private set; }
    public IClientHTMLSurface IClientHTMLSurface { get; private set; }
    public IClientHTTP IClientHTTP { get; private set; }
    public IClientInventory IClientInventory { get; private set; }
    public IClientMatchmaking IClientMatchmaking { get; private set; }
    public IClientMatchmakingServers IClientMatchmakingServers { get; private set; }
    public IClientMusic IClientMusic { get; private set; }
    public IClientNetworkDeviceManager IClientNetworkDeviceManager { get; private set; }
    public IClientNetworking IClientNetworking { get; private set; }
    public IClientNetworkingSockets IClientNetworkingSockets { get; private set; }
    public IClientNetworkingSocketsSerialized IClientNetworkingSocketsSerialized { get; private set; }
    public IClientNetworkingUtils IClientNetworkingUtils { get; private set; }
    public IClientNetworkingUtilsSerialized IClientNetworkingUtilsSerialized { get; private set; }
    public IClientParentalSettings IClientParentalSettings { get; private set; }
    public IClientParties IClientParties { get; private set; }
    public IClientProductBuilder IClientProductBuilder { get; private set; }
    public IClientRemoteClientManager IClientRemoteClientManager { get; private set; }
    public IClientRemotePlay IClientRemotePlay { get; private set; }
    public IClientRemoteStorage IClientRemoteStorage { get; private set; }
    public IClientScreenshots IClientScreenshots { get; private set; }
    public IClientShader IClientShader { get; private set; }
    public IClientSharedConnection IClientSharedConnection { get; private set; }
    public IClientShortcuts IClientShortcuts { get; private set; }
    public IClientSTARInternal IClientSTARInternal { get; private set; }
    public IClientStreamClient IClientStreamClient { get; private set; }
    public IClientStreamLauncher IClientStreamLauncher { get; private set; }
    public IClientSystemAudioManager IClientSystemAudioManager { get; private set; }
    public IClientSystemDisplayManager IClientSystemDisplayManager { get; private set; }
    public IClientSystemDockManager IClientSystemDockManager { get; private set; }
    public IClientSystemManager IClientSystemManager { get; private set; }
    public IClientSystemPerfManager IClientSystemPerfManager { get; private set; }
    public IClientUGC IClientUGC { get; private set; }
    public IClientUnifiedMessages IClientUnifiedMessages { get; private set; }
    public IClientUser IClientUser { get; private set; }
    public IClientUserStats IClientUserStats { get; private set; }
    public IClientUtils IClientUtils { get; private set; }
    public IClientVideo IClientVideo { get; private set; }
    public IClientVR IClientVR { get; private set; }

    public ClientApps ClientApps { get; private set; }
    public ClientConfigStore ClientConfigStore { get; private set; }
    public ClientMessaging ClientMessaging { get; private set; }

    public CallbackManager CallbackManager { get; private set; }

    private IPCClient ClientIPC => IPCClientEngine.GetIPCClient(this.Pipe);
    public IPCSteamClient(bool enableSpew = false) {
        LoadEngine();
        LoadInterfaces();
        
        this.CallbackManager = new CallbackManager(this);

        Logging.GeneralLogger.Info($"Successfully initialized IPCSteamClient with HSteamPipe={this.Pipe} HSteamUser={this.User}");
        
        if (enableSpew)
        {
            for (int i = 0; i < (int)ESpewGroup.k_ESpew_ArraySize; i++)
            {
                // These are really noisy and don't provide much value, so don't enable them
                if ((ESpewGroup)i == ESpewGroup.Httpclient) {
                    continue;
                }
                //this.IClientUtils.SetSpew((ESpewGroup)i, 9, 9);
            }
        }

        this.ClientApps = new ClientApps(this);
        this.ClientConfigStore = new ClientConfigStore(this);
        this.ClientMessaging = new ClientMessaging(this);

        // Before this, most important callbacks should be registered
        this.CallbackManager.StartThread();
    }

    public bool BGetCallback(out CallbackMsg_t callbackMsg)
    {
        return this.ClientIPC.CallbackQueue.TryDequeue(out callbackMsg);
    }

    public void FreeLastCallback() {

    }

    public ConnectionType ConnectedWith => ConnectionType.ExistingClient;
    private HSteamPipe Pipe;
    private HSteamUser User;

    [MemberNotNull(nameof(IPCClientEngine))]
    private void LoadEngine() {
        this.IPCClientEngine = new IPCClientEngine();
        this.Pipe = this.IPCClientEngine.CreateSteamPipe();
        this.User = this.IPCClientEngine.ConnectToGlobalUser(this.Pipe);
    }
    
    [MemberNotNull(nameof(IClientAppDisableUpdate))]
    [MemberNotNull(nameof(IClientAppManager))]
    [MemberNotNull(nameof(IClientApps))]
    [MemberNotNull(nameof(IClientAudio))]
    [MemberNotNull(nameof(IClientBilling))]
    [MemberNotNull(nameof(IClientBluetoothManager))]
    [MemberNotNull(nameof(IClientCompat))]
    [MemberNotNull(nameof(IClientConfigStore))]
    [MemberNotNull(nameof(IClientController))]
    [MemberNotNull(nameof(IClientControllerSerialized))]
    [MemberNotNull(nameof(IClientDepotBuilder))]
    [MemberNotNull(nameof(IClientDeviceAuth))]
    [MemberNotNull(nameof(IClientFriends))]
    [MemberNotNull(nameof(IClientGameCoordinator))]
    [MemberNotNull(nameof(IClientGameNotifications))]
    [MemberNotNull(nameof(IClientGameSearch))]
    //[MemberNotNull(nameof(IClientGameServerInternal))]
    //[MemberNotNull(nameof(IClientGameServerPacketHandler))]
    //[MemberNotNull(nameof(IClientGameServerStats))]
    [MemberNotNull(nameof(IClientGameStats))]
    [MemberNotNull(nameof(IClientHTMLSurface))]
    [MemberNotNull(nameof(IClientHTTP))]
    //[MemberNotNull(nameof(IClientInstallUtils))]
    [MemberNotNull(nameof(IClientInventory))]
    [MemberNotNull(nameof(IClientMatchmaking))]
    [MemberNotNull(nameof(IClientMatchmakingServers))]
    //[MemberNotNull(nameof(IClientModuleManager))]
    [MemberNotNull(nameof(IClientMusic))]
    [MemberNotNull(nameof(IClientNetworkDeviceManager))]
    [MemberNotNull(nameof(IClientNetworking))]
    //[MemberNotNull(nameof(IClientNetworkingMessages))]
    [MemberNotNull(nameof(IClientNetworkingSockets))]
    [MemberNotNull(nameof(IClientNetworkingSocketsSerialized))]
    [MemberNotNull(nameof(IClientNetworkingUtils))]
    [MemberNotNull(nameof(IClientNetworkingUtilsSerialized))]
    [MemberNotNull(nameof(IClientParentalSettings))]
    [MemberNotNull(nameof(IClientParties))]
    //[MemberNotNull(nameof(IClientProcessMonitor))]
    [MemberNotNull(nameof(IClientProductBuilder))]
    [MemberNotNull(nameof(IClientRemoteClientManager))]
    [MemberNotNull(nameof(IClientRemotePlay))]
    [MemberNotNull(nameof(IClientRemoteStorage))]
    [MemberNotNull(nameof(IClientScreenshots))]
    //[MemberNotNull(nameof(IClientSecureDesktop))]
    [MemberNotNull(nameof(IClientShader))]
    [MemberNotNull(nameof(IClientSharedConnection))]
    [MemberNotNull(nameof(IClientShortcuts))]
    [MemberNotNull(nameof(IClientSTARInternal))]
    [MemberNotNull(nameof(IClientStreamClient))]
    [MemberNotNull(nameof(IClientStreamLauncher))]
    [MemberNotNull(nameof(IClientSystemAudioManager))]
    [MemberNotNull(nameof(IClientSystemDisplayManager))]
    [MemberNotNull(nameof(IClientSystemDockManager))]
    [MemberNotNull(nameof(IClientSystemManager))]
    [MemberNotNull(nameof(IClientSystemPerfManager))]
    [MemberNotNull(nameof(IClientUGC))]
    [MemberNotNull(nameof(IClientUnifiedMessages))]
    [MemberNotNull(nameof(IClientUser))]
    [MemberNotNull(nameof(IClientUserStats))]
    [MemberNotNull(nameof(IClientUtils))]
    [MemberNotNull(nameof(IClientVideo))]
    [MemberNotNull(nameof(IClientVR))]
    private void LoadInterfaces() {
        this.IClientAppDisableUpdate = this.IClientEngine.GetIClientAppDisableUpdate(this.User, this.Pipe);
        this.IClientAppManager = this.IClientEngine.GetIClientAppManager(this.User, this.Pipe);
        this.IClientApps = this.IClientEngine.GetIClientApps(this.User, this.Pipe);
        this.IClientAudio = this.IClientEngine.GetIClientAudio(this.User, this.Pipe);
        this.IClientBilling = this.IClientEngine.GetIClientBilling(this.User, this.Pipe);
        this.IClientBluetoothManager = this.IClientEngine.GetIClientBluetoothManager(this.Pipe);
        this.IClientCompat = this.IClientEngine.GetIClientCompat(this.User, this.Pipe);
        this.IClientConfigStore = this.IClientEngine.GetIClientConfigStore(this.User, this.Pipe);
        //this.IClientController = this.IClientEngine.GetIClientController(this.Pipe);
        this.IClientControllerSerialized = this.IClientEngine.GetIClientControllerSerialized(this.Pipe);
        this.IClientDepotBuilder = this.IClientEngine.GetIClientDepotBuilder(this.User, this.Pipe);
        this.IClientDeviceAuth = this.IClientEngine.GetIClientDeviceAuth(this.User, this.Pipe);
        this.IClientFriends = this.IClientEngine.GetIClientFriends(this.User, this.Pipe);
        this.IClientGameCoordinator = this.IClientEngine.GetIClientGameCoordinator(this.User, this.Pipe);
        this.IClientGameNotifications = this.IClientEngine.GetIClientGameNotifications(this.User, this.Pipe);
        this.IClientGameSearch = this.IClientEngine.GetIClientGameSearch(this.User, this.Pipe);
        //this.IClientGameServerInternal = this.IClientEngine.GetIClientGameServerInternal(this.User, this.Pipe);
        //this.IClientGameServerPacketHandler = this.IClientEngine.GetIClientGameServerPacketHandler(this.User, this.Pipe);
        //this.IClientGameServerStats = this.IClientEngine.GetIClientGameServerStats(this.User, this.Pipe);
        this.IClientGameStats = this.IClientEngine.GetIClientGameStats(this.User, this.Pipe);
        //this.IClientHTMLSurface = this.IClientEngine.GetIClientHTMLSurface(this.User, this.Pipe);
        this.IClientHTTP = this.IClientEngine.GetIClientHTTP(this.User, this.Pipe);
        //this.IClientInstallUtils = this.IClientEngine.GetIClientInstallUtils(this.Pipe);
        this.IClientInventory = this.IClientEngine.GetIClientInventory(this.User, this.Pipe);
        this.IClientMatchmaking = this.IClientEngine.GetIClientMatchmaking(this.User, this.Pipe);
        //this.IClientMatchmakingServers = this.IClientEngine.GetIClientMatchmakingServers(this.User, this.Pipe);
        //this.IClientModuleManager = this.IClientEngine.GetIClientModuleManager(this.Pipe);
        this.IClientMusic = this.IClientEngine.GetIClientMusic(this.User, this.Pipe);
        this.IClientNetworkDeviceManager = this.IClientEngine.GetIClientNetworkDeviceManager(this.Pipe);
        this.IClientNetworking = this.IClientEngine.GetIClientNetworking(this.User, this.Pipe);
        //this.IClientNetworkingMessages = this.IClientEngine.GetIClientNetworkingMessages(this.User, this.Pipe);
        //this.IClientNetworkingSockets = this.IClientEngine.GetIClientNetworkingSockets(this.User, this.Pipe);
        this.IClientNetworkingSocketsSerialized = this.IClientEngine.GetIClientNetworkingSocketsSerialized(this.User, this.Pipe);
        //this.IClientNetworkingUtils = this.IClientEngine.GetIClientNetworkingUtils(this.Pipe);
        this.IClientNetworkingUtilsSerialized = this.IClientEngine.GetIClientNetworkingUtilsSerialized(this.Pipe);
        this.IClientParentalSettings = this.IClientEngine.GetIClientParentalSettings(this.User, this.Pipe);
        this.IClientParties = this.IClientEngine.GetIClientParties(this.User, this.Pipe);
        //this.IClientProcessMonitor = this.IClientEngine.GetIClientProcessMonitor(this.User, this.Pipe);
        this.IClientProductBuilder = this.IClientEngine.GetIClientProductBuilder(this.User, this.Pipe);
        this.IClientRemoteClientManager = this.IClientEngine.GetIClientRemoteClientManager(this.Pipe);
        this.IClientRemotePlay = this.IClientEngine.GetIClientRemotePlay(this.User, this.Pipe);
        this.IClientRemoteStorage = this.IClientEngine.GetIClientRemoteStorage(this.User, this.Pipe);
        this.IClientScreenshots = this.IClientEngine.GetIClientScreenshots(this.User, this.Pipe);
        //this.IClientSecureDesktop = this.IClientEngine.GetIClientSecureDesktop(this.Pipe);
        this.IClientShader = this.IClientEngine.GetIClientShader(this.User, this.Pipe);
        this.IClientSharedConnection = this.IClientEngine.GetIClientSharedConnection(this.User, this.Pipe);
        this.IClientShortcuts = this.IClientEngine.GetIClientShortcuts(this.User, this.Pipe);
        this.IClientSTARInternal = this.IClientEngine.GetIClientSTARInternal(this.User, this.Pipe);
        this.IClientStreamClient = this.IClientEngine.GetIClientStreamClient(this.User, this.Pipe);
        this.IClientStreamLauncher = this.IClientEngine.GetIClientStreamLauncher(this.User, this.Pipe);
        this.IClientSystemAudioManager = this.IClientEngine.GetIClientSystemAudioManager(this.Pipe);
        this.IClientSystemDisplayManager = this.IClientEngine.GetIClientSystemDisplayManager(this.Pipe);
        this.IClientSystemDockManager = this.IClientEngine.GetIClientSystemDockManager(this.Pipe);
        this.IClientSystemManager = this.IClientEngine.GetIClientSystemManager(this.Pipe);
        this.IClientSystemPerfManager = this.IClientEngine.GetIClientSystemPerfManager(this.Pipe);
        this.IClientUGC = this.IClientEngine.GetIClientUGC(this.User, this.Pipe);
        this.IClientUnifiedMessages = this.IClientEngine.GetIClientUnifiedMessages(this.User, this.Pipe);
        this.IClientUser = this.IClientEngine.GetIClientUser(this.User, this.Pipe);
        this.IClientUserStats = this.IClientEngine.GetIClientUserStats(this.User, this.Pipe);
        this.IClientUtils = this.IClientEngine.GetIClientUtils(this.Pipe);
        this.IClientVideo = this.IClientEngine.GetIClientVideo(this.User, this.Pipe);
        this.IClientVR = this.IClientEngine.GetIClientVR(this.Pipe);
    }

    public void Shutdown()
    {
        this.ClientIPC.Shutdown();
    }
}