public static class InterfaceMap {
    public static readonly Dictionary<byte, string> ClientInterfaces = new()
    {
        {1, "IClientUser"},
        {3, "IClientFriends"},
        {4, "IClientUtils"},
        {5, "IClientBilling"},
        {6, "IClientMatchmaking"},
        {8, "IClientApps"},
        {11, "IClientUserStats"},
        {12, "IClientNetworking"},
        {13, "IClientRemoteStorage"},
        {16, "IClientDepotBuilder"},
        {17, "IClientAppManager"},
        {18, "IClientConfigStore"},
        {19, "IClientGameCoordinator"},
        {21, "IClientGameStats"},
        {22, "IClientHTTP"},
        {23, "IClientScreenshots"},
        {24, "IClientAudio"},
        {25, "IClientUnifiedMessages"},
        {26, "IClientStreamLauncher"},
        {27, "IClientParentalSettings"},
        {28, "IClientDeviceAuth"},
        {29, "IClientNetworkDeviceManager"},
        {30, "IClientMusic"},
        {31, "IClientRemoteClientManager"},
        {32, "IClientUGC"},
        {33, "IClientStreamClient"},
        {34, "IClientProductBuilder"},
        {35, "IClientShortcuts"},
        {37, "IClientGameNotifications"},
        {38, "IClientVideo"},
        {39, "IClientInventory"},
        {40, "IClientVR"},
        {41, "IClientControllerSerialized"},
        {42, "IClientAppDisableUpdate"},
        {43, "IClientBluetoothManager"},
        {44, "IClientSharedConnection"},
        {45, "IClientShader"},
        {46, "IClientNetworkingSocketsSerialized"},
        {47, "IClientGameSearch"},
        {48, "IClientCompat"},
        {49, "IClientParties"},
        {50, "IClientNetworkingUtilsSerialized"},
        {51, "IClientSTARInternal "},
        {52, "IClientRemotePlay"},
        {54, "IClientSystemManager"},
        {57, "IClientSystemPerfManager"},
        {58, "IClientSystemDockManager"},
        {59, "IClientSystemAudioManager"},
        {60, "IClientSystemDisplayManager"},
    };

    public static readonly List<byte> ClientInterfacesNoUser = new()
    {
        4, 29, 31, 41, 43, 50, 54, 57, 58, 59, 60
    };

     // 1 = IClientUser
    // 2 = "Narrowing to GameServer failed"
    // 3 = IClientFriends
    // 4 = IClientUtils (!requiresuser)
    // 5 = IClientBilling
    // 6 = IClientMatchmaking
    // 7 = "not found"
    // 8 = IClientApps
    // 9 = "not found"
    // 11 = IClientUserStats
    // 12 = IClientNetworking
    // 13 = IClientRemoteStorage
    // 14 = "not found"
    // 15 = "not found"
    // 16 = IClientDepotBuilder
    // 17 = IClientAppManager
    // 18 = IClientConfigStore
    // 19 = IClientGameCoordinator
    // 20 = "Narrowing to GameServer failed"
    // 21 = IClientGameStats
    // 22 = IClientHTTP
    // 23 = IClientScreenshots
    // 24 = IClientAudio
    // 25 = IClientUnifiedMessages
    // 26 = IClientStreamLauncher
    // 27 = IClientParentalSettings
    // 28 = IClientDeviceAuth
    // 29 = IClientNetworkDeviceManager (!requiresuser)
    // 30 = IClientMusic
    // 31 = IClientRemoteClientManager (!requiresuser)
    // 32 = IClientUGC
    // 33 = IClientStreamClient
    // 34 = IClientProductBuilder
    // 35 = IClientShortcuts
    // 36 = "not found"
    // 37 = IClientGameNotifications
    // 38 = IClientVideo
    // 39 = IClientInventory
    // 40 = IClientVR (!requiresuser)
    // 41 = IClientControllerSerialized (!requiresuser)
    // 42 = IClientAppDisableUpdate
    // 43 = IClientBluetoothManager (!requiresuser)
    // 44 = IClientSharedConnection
    // 45 = IClientShader
    // 46 = IClientNetworkingSocketsSerialized
    // 47 = IClientGameSearch
    // 48 = IClientCompat
    // 49 = IClientParties
    // 50 = IClientNetworkingUtilsSerialized (!requiresuser)
    // 51 = IClientSTARInternal 
    // 52 = IClientRemotePlay
    // 53 = "Narrowing to GameServer failed"
    // 54 = IClientSystemManager (!requiresuser)
    // 55 = "not found"
    // 56 = "not found"
    // 57 = IClientSystemPerfManager (!requiresuser)
    // 58 = IClientSystemDockManager (!requiresuser)
    // 59 = IClientSystemAudioManager (!requiresuser)
    // 60 = IClientSystemDisplayManager (!requiresuser)

     public static readonly Dictionary<byte, string> ServiceInterfaces = new()
    {
        {1, "IClientInstallUtils"},
        {2, "IClientModuleManager"},
        {3, "IRegistryInterface"},
        {4, "IClientProcessMonitor"},
        {5, "IClientSecureDesktop"},
    };
}