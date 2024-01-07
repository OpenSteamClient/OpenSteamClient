using System;
using System.Collections.Generic;
using OpenSteamworks.ConCommands;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;

namespace OpenSteamworks.IPCClient;

public class IPCClientEngine : IClientEngine
{
    private readonly Dictionary<HSteamPipe, IPCClient> pipes = new();
    private readonly Dictionary<HSteamPipe, HSteamUser> createdUsers = new();

    public IPCClient GetIPCClient(HSteamPipe hSteamPipe) {
        return pipes[hSteamPipe];
    }
    
    public bool BOverlayNeedsPresent()
    {
        throw new System.NotImplementedException();
    }

    public bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
    {
        if (!pipes.ContainsKey(hSteamPipe)) {
            Logging.GeneralLogger.Error("Pipe is invalid");
            return false;
        }

        createdUsers.Remove(hSteamPipe);
        pipes[hSteamPipe].Shutdown();
        pipes.Remove(hSteamPipe);
        return true;
    }

    public bool BShutdownIfAllPipesClosed()
    {
        // No need to do actual shutdown logic here since all pipes are closed already.
        return pipes.Count == 0;
    }

    public unsafe void ConCommandInit(IConCommandBaseAccessor* pAccessor)
    {
        throw new System.NotImplementedException("IPCClientEngine does not support ConVars!");
    }

    public HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
    {
        if (!pipes.ContainsKey(hSteamPipe)) {
            throw new InvalidOperationException("Pipe is invalid");
        }
        
        var user = pipes[hSteamPipe].ConnectToGlobalUser();
        createdUsers[hSteamPipe] = user;
        return user;
    }

    public HSteamUser CreateGlobalUser(ref HSteamPipe phSteamPipe)
    {
        throw new System.NotImplementedException("Cannot create a global user with IPCClientEngine!");
    }

    public HSteamUser CreateLocalUser(ref HSteamPipe phSteamPipe, EAccountType eAccountType)
    {
        throw new System.NotImplementedException("Cannot create a local user with IPCClientEngine!");
    }

    public void CreatePipeToLocalUser(HSteamUser hSteamUser, ref HSteamPipe phSteamPipe)
    {
        throw new System.NotImplementedException();
    }

    public HSteamPipe CreateSteamPipe()
    {
        var newclient = new IPCClient("127.0.0.1:57343", IPCClient.IPCConnectionType.Client, true);
        newclient.ConnectToSteamPipe(out uint hostPid);
        SteamClient.IsIPCCrossProcess = hostPid != Environment.ProcessId;
        
        pipes.Add(pipes.Count+1, newclient);
        return pipes.Count;
    }

    public void Destructor1()
    {
        throw new System.NotImplementedException();
    }

    public void Destructor2()
    {
        throw new System.NotImplementedException();
    }

    public void FlushBeforeValidate()
    {
        throw new System.NotImplementedException();
    }

    public unsafe bool GetAPICallResult(HSteamPipe hSteamPipe, SteamAPICall_t hSteamAPICall, void* pCallback, int cubCallback, int iCallbackExpected, ref bool pbFailed)
    {
        GetIClientUtils(hSteamPipe).GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
    }

    private T GenerateClass<T>(HSteamUser hSteamUser, HSteamPipe hSteamPipe) where T: class {
        if (!pipes.ContainsKey(hSteamPipe)) {
            throw new InvalidOperationException("Pipe is invalid");
        }

        return IPCJITGenerator.GenerateClass<T>(pipes[hSteamPipe], hSteamUser);
    }

    private T GenerateClass<T>(HSteamPipe hSteamPipe) where T: class {
        if (!pipes.ContainsKey(hSteamPipe)) {
            throw new InvalidOperationException("Pipe is invalid");
        }

        return IPCJITGenerator.GenerateClass<T>(pipes[hSteamPipe], 0);
    }

    public IClientAppDisableUpdate GetIClientAppDisableUpdate(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientAppDisableUpdate>(hSteamUser, hSteamPipe);
    

    public IClientAppManager GetIClientAppManager(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientAppManager>(hSteamUser, hSteamPipe);
    public IClientApps GetIClientApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientApps>(hSteamUser, hSteamPipe);
    public IClientAudio GetIClientAudio(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientAudio>(hSteamUser, hSteamPipe);
    public IClientBilling GetIClientBilling(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientBilling>(hSteamUser, hSteamPipe);
    public IClientBluetoothManager GetIClientBluetoothManager(HSteamPipe hSteamPipe) => GenerateClass<IClientBluetoothManager>(hSteamPipe);
    public IClientCompat GetIClientCompat(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientCompat>(hSteamUser, hSteamPipe);
    public IClientConfigStore GetIClientConfigStore(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientConfigStore>(hSteamUser, hSteamPipe);
    public IClientController GetIClientController(HSteamPipe hSteamPipe) => throw new InvalidOperationException("IPCClient does not support creating non-serializable interfaces!");
    public IClientControllerSerialized GetIClientControllerSerialized(HSteamPipe hSteamPipe) => GenerateClass<IClientControllerSerialized>(hSteamPipe);
    public IClientDepotBuilder GetIClientDepotBuilder(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientDepotBuilder>(hSteamUser, hSteamPipe);
    public IClientDeviceAuth GetIClientDeviceAuth(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientDeviceAuth>(hSteamUser, hSteamPipe);
    public IClientFriends GetIClientFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientFriends>(hSteamUser, hSteamPipe);
    public IClientGameCoordinator GetIClientGameCoordinator(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameCoordinator>(hSteamUser, hSteamPipe);
    public IClientGameNotifications GetIClientGameNotifications(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameNotifications>(hSteamUser, hSteamPipe);
    public IClientGameSearch GetIClientGameSearch(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameSearch>(hSteamUser, hSteamPipe);
    public IClientGameServerInternal GetIClientGameServerInternal(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameServerInternal>(hSteamUser, hSteamPipe);
    public IClientGameServerPacketHandler GetIClientGameServerPacketHandler(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameServerPacketHandler>(hSteamUser, hSteamPipe);
    public IClientGameServerStats GetIClientGameServerStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameServerStats>(hSteamUser, hSteamPipe);
    public IClientGameStats GetIClientGameStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientGameStats>(hSteamUser, hSteamPipe);
    public IClientHTMLSurface GetIClientHTMLSurface(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => throw new InvalidOperationException("IPCClient does not support creating non-serializable interfaces!");
    public IClientHTTP GetIClientHTTP(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientHTTP>(hSteamUser, hSteamPipe);
    public IClientInventory GetIClientInventory(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientInventory>(hSteamUser, hSteamPipe);
    public IClientMatchmaking GetIClientMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientMatchmaking>(hSteamUser, hSteamPipe);
    public IClientMatchmakingServers GetIClientMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => throw new InvalidOperationException("IPCClient does not support creating non-serializable interfaces!");
    public IClientMusic GetIClientMusic(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientMusic>(hSteamUser, hSteamPipe);
    public IClientNetworkDeviceManager GetIClientNetworkDeviceManager(HSteamPipe hSteamPipe) => GenerateClass<IClientNetworkDeviceManager>(hSteamPipe);
    public IClientNetworking GetIClientNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientNetworking>(hSteamUser, hSteamPipe);
    public IClientNetworkingMessages GetIClientNetworkingMessages(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientNetworkingMessages>(hSteamUser, hSteamPipe);
    public IClientNetworkingSockets GetIClientNetworkingSockets(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => throw new InvalidOperationException("IPCClient does not support creating non-serializable interfaces!");
    public IClientNetworkingSocketsSerialized GetIClientNetworkingSocketsSerialized(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientNetworkingSocketsSerialized>(hSteamUser, hSteamPipe);
    public IClientNetworkingUtils GetIClientNetworkingUtils(HSteamPipe hSteamPipe) => throw new InvalidOperationException("IPCClient does not support creating non-serializable interfaces!");
    public IClientNetworkingUtilsSerialized GetIClientNetworkingUtilsSerialized(HSteamPipe hSteamPipe) => GenerateClass<IClientNetworkingUtilsSerialized>(hSteamPipe);
    public IClientParentalSettings GetIClientParentalSettings(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientParentalSettings>(hSteamUser, hSteamPipe);
    public IClientParties GetIClientParties(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientParties>(hSteamUser, hSteamPipe);
    public IClientProductBuilder GetIClientProductBuilder(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientProductBuilder>(hSteamUser, hSteamPipe);
    public IClientRemoteClientManager GetIClientRemoteClientManager(HSteamPipe hSteamPipe) => GenerateClass<IClientRemoteClientManager>(hSteamPipe);
    public IClientRemotePlay GetIClientRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientRemotePlay>(hSteamUser, hSteamPipe);
    public IClientRemoteStorage GetIClientRemoteStorage(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientRemoteStorage>(hSteamUser, hSteamPipe);
    public IClientScreenshots GetIClientScreenshots(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientScreenshots>(hSteamUser, hSteamPipe);
    public IClientShader GetIClientShader(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientShader>(hSteamUser, hSteamPipe);
    public IClientSharedConnection GetIClientSharedConnection(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientSharedConnection>(hSteamUser, hSteamPipe);
    public IClientShortcuts GetIClientShortcuts(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientShortcuts>(hSteamUser, hSteamPipe);
    public IClientSTARInternal GetIClientSTARInternal(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientSTARInternal>(hSteamUser, hSteamPipe);
    public IClientStreamClient GetIClientStreamClient(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientStreamClient>(hSteamUser, hSteamPipe);
    public IClientStreamLauncher GetIClientStreamLauncher(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientStreamLauncher>(hSteamUser, hSteamPipe);
    public IClientSystemAudioManager GetIClientSystemAudioManager(HSteamPipe hSteamPipe) => GenerateClass<IClientSystemAudioManager>(hSteamPipe);
    public IClientSystemDisplayManager GetIClientSystemDisplayManager(HSteamPipe hSteamPipe) => GenerateClass<IClientSystemDisplayManager>(hSteamPipe);
    public IClientSystemDockManager GetIClientSystemDockManager(HSteamPipe hSteamPipe) => GenerateClass<IClientSystemDockManager>(hSteamPipe);
    public IClientSystemManager GetIClientSystemManager(HSteamPipe hSteamPipe) => GenerateClass<IClientSystemManager>(hSteamPipe);
    public IClientSystemPerfManager GetIClientSystemPerfManager(HSteamPipe hSteamPipe) => GenerateClass<IClientSystemPerfManager>(hSteamPipe);
    public IClientUGC GetIClientUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientUGC>(hSteamUser, hSteamPipe);
    public IClientUnifiedMessages GetIClientUnifiedMessages(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientUnifiedMessages>(hSteamUser, hSteamPipe);
    public IClientUser GetIClientUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientUser>(hSteamUser, hSteamPipe);
    public IClientUserStats GetIClientUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientUserStats>(hSteamUser, hSteamPipe);
    public IClientUtils GetIClientUtils(HSteamPipe hSteamPipe) => GenerateClass<IClientUtils>(hSteamPipe);
    public IClientVideo GetIClientVideo(HSteamUser hSteamUser, HSteamPipe hSteamPipe) => GenerateClass<IClientVideo>(hSteamUser, hSteamPipe);
    public IClientVR GetIClientVR(HSteamPipe hSteamPipe) => GenerateClass<IClientVR>(hSteamPipe);
    public uint GetIPCCallCount()
    {
        uint totalIPCCalls = 0;
        foreach (var pipe in pipes)
        {
            totalIPCCalls += pipe.Value.IPCCallCount;
            pipe.Value.ResetIPCCallCount();
        }

        return totalIPCCalls;
    }

    public unsafe void* GetIPCServerMap()
    {
        throw new System.NotImplementedException();
    }

    public string GetUniverseName(EUniverse universe)
    {
        return universe.ToString();
    }

    public bool HookScreenshots(bool bHook)
    {
        throw new System.NotImplementedException();
    }

    public bool IsOverlayEnabled()
    {
        throw new System.NotImplementedException();
    }

    public bool IsScreenshotsHooked()
    {
        throw new System.NotImplementedException();
    }

    public bool IsValidHSteamUserPipe(HSteamPipe hSteamPipe, HSteamUser hUser)
    {
        if (!pipes.ContainsKey(hSteamPipe)) {
            return false;
        }

        if (!createdUsers.ContainsKey(hSteamPipe)) {
            return false;
        }

        return createdUsers[hSteamPipe] == hUser;
    }

    public void OnDebugTextArrived(string msg)
    {
        Logging.GeneralLogger.Debug(msg);
    }

    public int OnThreadBuffersOverLimit()
    {
        throw new System.NotImplementedException();
    }

    public int OnThreadLocalRegistration()
    {
        throw new System.NotImplementedException();
    }

    public void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
    {
        if (hSteamPipe >= pipes.Count) {
            throw new InvalidOperationException("Pipe is invalid");
        }

        createdUsers.Remove(hSteamPipe);
        pipes[hSteamPipe].ReleaseUser(hUser);
    }

    public void RunFrame()
    {
        
    }

    public void SetClientCommandLine(int argc, string[] argv)
    {
        
    }

    public void SetLocalIPBinding(uint ip, ushort port)
    {
        throw new System.NotImplementedException();
    }

    public void SetOverlayNotificationInsert(int unk1, int unk2)
    {
        throw new System.NotImplementedException();
    }

    public void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
    {
        throw new System.NotImplementedException();
    }

    public void SetWarningMessageHook(ClientAPI_WarningMessageHook_t func)
    {
        throw new System.NotImplementedException();
    }

    public int Set_ClientAPI_CPostAPIResultInProcess(ClientAPI_PostAPIResultInProcess_t callback)
    {
        throw new System.NotImplementedException();
    }
}