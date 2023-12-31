using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

public class Program
{

    public static void Main(string[] args)
    {
        if (args.Length < 2 || args[1] == "client") {
            IPCClient client = new("127.0.0.1:57343", IPCClient.IPCConnectionType.Client);
            Console.WriteLine("Post connect client");
            Console.WriteLine("BLoggedOn: " + client.CallIPCFunctionClient<bool>(1, 0x6585013d, 0x658ba6d7));
            Console.WriteLine("GetSteamID: " + client.CallIPCFunctionClient<ulong>(1, 0xd6fc3207, 0xd70451a7));
            Console.WriteLine("GetInstallPath: " + client.CallIPCFunctionClient<string>(4, 0xab7236cd, 0xad0b8048));
        } else {
            IPCClient serviceclient = new("127.0.0.1:57344", IPCClient.IPCConnectionType.Service);

            Console.WriteLine("Post connect service");
            // IClientModuleManager::SetProtonEnvironment fence: 0xfe6168aa (4267796650) (func id: 0xfe43df34 (4265860916))
            // Calculate static fence with:
            //   4267796650-254 = 4267796396
            //   4265860916-254 = 4265860662
            // 4267796650-4265860916 = 1935734 (WORKS!)
            // 4267796650-4267796396 = 254 (DOESNT FUCKIGN WORK)
            // 4265860916-4265860662 = 254 (also doesn't work)
            // Expected after operation = 1935734 OR 254
            // Internal math operation = ID+1935734 == 4267796650 || ID+254 == 4267796650


            // What we want = 4265860916+254 = 4265861170
            serviceclient.CallIPCFunctionService<uint>(2, 0xfe43df34, (uint)231430, "ENVVAR=TEST");
        }
       
        // PrintInfoReader();
        // Console.WriteLine();

        // ? | ? | ? | interface id
        // var ifaceName = System.Text.Encoding.UTF8.GetBytes("IClientUser");
        // var ifaceFunc = System.Text.Encoding.UTF8.GetBytes("BLoggedOn");
        
        // // AppId_t appid, DepotId_t depotId, uint workshopItemID, uint unk2, ulong targetManifestID, ulong deltaManifestID, string? targetInstallPath
        // Console.WriteLine("DownloadDepot: " + client.CallIPCFunction<ulong>(16, 0x279a7a09, 0x2a1205ff, (uint)730, (uint)2347771, (uint)0, (ulong)0, (ulong)0, (ulong)0, (uint)0, "/mnt/deathclaw/test/"));
        //Console.WriteLine("LogOn: " + client.CallIPCFunction<uint>(1, 0x3e52b2fc, 0x3e589b93, (ulong)76561198264836001, true));
        

        // using (var stream = new MemoryStream()) {
        //     var writer = new EndianAwareBinaryWriter(stream);
        //     // writer.Write((byte)hSteamUser);
        //     // writer.Write((byte)hSteamPipe);
        //     //writer.Write(client.HSteamPipe);

        //     // IClientXXXX (interface ID)
        //     //writer.Write((uint)0x4f20728f);
        //     // stream.WriteByte(3);
        //     // stream.WriteByte(1);
        //     // stream.WriteByte(0);
        //     // 1 = IClientUser
        //     // 2 = "Narrowing to GameServer failed"
        //     // 3 = IClientFriends
        //     // 4 = IClientUtils (!requiresuser)
        //     // 5 = IClientBilling
        //     // 6 = IClientMatchmaking
        //     // 7 = "not found"
        //     // 8 = IClientApps
        //     // 9 = "not found"
        //     // 11 = IClientUserStats
        //     // 12 = IClientNetworking
        //     // 13 = IClientRemoteStorage
        //     // 14 = "not found"
        //     // 15 = "not found"
        //     // 16 = IClientDepotBuilder
        //     // 17 = IClientAppManager
        //     // 18 = IClientConfigStore
        //     // 19 = IClientGameCoordinator
        //     // 20 = "Narrowing to GameServer failed"
        //     // 21 = IClientGameStats
        //     // 22 = IClientHTTP
        //     // 23 = IClientScreenshots
        //     // 24 = IClientAudio
        //     // 25 = IClientUnifiedMessages
        //     // 26 = IClientStreamLauncher
        //     // 27 = IClientParentalSettings
        //     // 28 = IClientDeviceAuth
        //     // 29 = IClientNetworkDeviceManager (!requiresuser)
        //     // 30 = IClientMusic
        //     // 31 = IClientRemoteClientManager (!requiresuser)
        //     // 32 = IClientUGC
        //     // 33 = IClientStreamClient
        //     // 34 = IClientProductBuilder
        //     // 35 = IClientShortcuts
        //     // 36 = "not found"
        //     // 37 = IClientGameNotifications
        //     // 38 = IClientVideo
        //     // 39 = IClientInventory
        //     // 40 = IClientVR (!requiresuser)
        //     // 41 = IClientControllerSerialized (!requiresuser)
        //     // 42 = IClientAppDisableUpdate
        //     // 43 = IClientBluetoothManager (!requiresuser)
        //     // 44 = IClientSharedConnection
        //     // 45 = IClientShader
        //     // 46 = IClientNetworkingSocketsSerialized
        //     // 47 = IClientGameSearch
        //     // 48 = IClientCompat
        //     // 49 = IClientParties
        //     // 50 = IClientNetworkingUtilsSerialized (!requiresuser)
        //     // 51 = IClientSTARInternal 
        //     // 52 = IClientRemotePlay
        //     // 53 = "Narrowing to GameServer failed"
        //     // 54 = IClientSystemManager (!requiresuser)
        //     // 55 = "not found"
        //     // 56 = "not found"
        //     // 57 = IClientSystemPerfManager (!requiresuser)
        //     // 58 = IClientSystemDockManager (!requiresuser)
        //     // 59 = IClientSystemAudioManager (!requiresuser)
        //     // 60 = IClientSystemDisplayManager (!requiresuser)
        //     // 61 = 
        //     stream.WriteByte(1);
        //     // stream.WriteByte(0);
        //     //stream.WriteByte(99);
        //     //stream.WriteByte(0);

        //     // User
        //     // stream.WriteByte(0);
        //     // stream.WriteByte(0);
        //     // stream.WriteByte(0);
        //     // stream.WriteByte(1);
        //     Console.WriteLine("USER: " + client.HSteamUser);
        //     writer.Write(client.HSteamUser);
        //     writer.Flush();

        //     // Function ID
        //     //uint id = 0x6585013d; // BLoggedOn
        //     uint id = 0x3e52b2fc;
        //     Console.WriteLine("Function ID: " + id);
        //     writer.Write(id);
        //     writer.Flush();

        //     ulong steamid = 76561198264836001;
        //     bool interactive = true;
        //     writer.Write(steamid);
        //     writer.Flush();

        //     writer.Write(interactive);
        //     writer.Flush();

        //     // Function Fencepost
        //     // uint fencepost = 0x658ba6d7; // BLoggedOn
        //     uint fencepost = 0x3e589b93;
        //     Console.WriteLine("Fencepost: " + fencepost);
        //     writer.Write(fencepost);
        //     writer.Flush();

        //     // writer.Write(client.HSteamPipe);
        //     // writer.Flush();

        //     // // HSteamUser 
        //     // writer.Write(client.HSteamUser);
        //     // writer.Flush();


        //     // stream.WriteByte(1);
        //     // stream.WriteByte(1);
        //     // stream.WriteByte(1);

        //     // for (int i = 0; i < 128; i++)
        //     // {
        //     //     stream.WriteByte(0);
        //     // }
        //     // stream.Write(ifaceName);
        //     // stream.Write(ifaceFunc);
        //     // writer.Write((UInt32)30);
        //     // writer.Write((UInt32)22222); // Fencepost
        //     client.SendAndWaitForResponse(IPCClient.IPCCommandCode.Interface, stream.ToArray());
        // }

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    // private static void PrintInfoReader() {
    //     var info = reader.ReadStruct<SteamSharedMemory>();
    //     PrintInfo(info);
    //     stream!.Seek(0, SeekOrigin.Begin);
    // }

    private static string GetShmFilename() {
        [DllImport("libc")]
        static extern int getuid();
    
        return $"/dev/shm/u{getuid()}-ValveIPCSharedObj-Steam";
    }
}