using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;

namespace OpenSteamworks.ClientInterfaces;

public class ClientRemoteStorage {
    private readonly IClientRemoteStorage remoteStorage;
    private readonly CallbackManager callbackManager;

    public ClientRemoteStorage(ISteamClient steamClient) {
        this.remoteStorage = steamClient.IClientRemoteStorage;
        this.callbackManager = steamClient.CallbackManager;
    }

    public async Task<EResult> LoadLocalFileInfoCache(AppId_t appid) {
        TaskCompletionSource<EResult> tcs = new();
        callbackManager.RegisterHandler<RemoteStorageAppInfoLoaded_t>((CallbackManager.CallbackHandler<RemoteStorageAppInfoLoaded_t> handler, RemoteStorageAppInfoLoaded_t data) =>
        {
            if (data.m_nAppID == appid) {
                tcs.SetResult(data.m_eResult);
                callbackManager.DeregisterHandler(handler);
            }
        });
        
        remoteStorage.LoadLocalFileInfoCache(appid);
        return await tcs.Task;
    }

    public async Task<EResult> SyncApp(AppId_t appid, ERemoteStorageSyncType type, ERemoteStorageSyncFlags flags) {
        if (!this.remoteStorage.IsCloudEnabledForAccount()) {
            return EResult.FeatureDisabled;
        }

        if (!this.remoteStorage.IsCloudEnabledForApp(appid)) {
            return EResult.FeatureDisabled;
        }

        var remoteStorageAppInfoLoaded = callbackManager.AsTask<RemoteStorageAppInfoLoaded_t>();
        this.remoteStorage.LoadLocalFileInfoCache(appid);
        await remoteStorageAppInfoLoaded;

        Console.WriteLine("Got RemoteStorageAppInfoLoaded_t");
        TaskCompletionSource<EResult> tcs = new();
        callbackManager.RegisterHandler((CallbackManager.CallbackHandler<RemoteStorageAppSyncedClient_t> handler, RemoteStorageAppSyncedClient_t data) =>
        {
            if (data.appid == appid) {
                Console.WriteLine("Got appid==appid");
                tcs.SetResult(data.result);
                callbackManager.DeregisterHandler(handler);
            }
        });

        Console.WriteLine("Registered");

        if (!remoteStorage.SynchronizeApp(appid, type, flags)) {
            Console.WriteLine("Got failure. Why?");
            return EResult.Failure;
        }

        return await tcs.Task;
    } 

    public async Task<EResult> SyncAppPreLaunch(AppId_t appid) {
        return await SyncApp(appid, ERemoteStorageSyncType.Down, ERemoteStorageSyncFlags.AutoCloud_Launch);
    }

    public async Task<EResult> SyncAppAfterExit(AppId_t appid) {
        return await SyncApp(appid, ERemoteStorageSyncType.Up, ERemoteStorageSyncFlags.AutoCloud_Exit);
    }

    internal void Shutdown()
    {
        //TODO: cloud sync all apps we can here
    }
}