using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using ValveKeyValue;

namespace OpenSteamworks.ClientInterfaces;

public class ClientApps {
    public IClientApps NativeClientApps { get; init; }
    public IClientAppManager NativeClientAppManager { get; init; }
    public IClientUser NativeClientUser { get; init; }
    public IClientRemoteStorage NativeClientRemoteStorage { get; init; }
    
    private readonly CallbackManager callbackManager;
    private static readonly KVSerializer serializertext = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    private static readonly KVSerializer serializerbinary = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
    public ClientApps(ISteamClient client) {
        this.NativeClientApps = client.IClientApps;
        this.NativeClientAppManager = client.IClientAppManager;
        this.NativeClientUser = client.IClientUser;
        this.NativeClientRemoteStorage = client.IClientRemoteStorage;
        this.callbackManager = client.CallbackManager;
    }

    public bool BIsAppUpToDate(AppId_t appid) => this.NativeClientAppManager.BIsAppUpToDate(appid);
    public KVObject GetAppDataSection(AppId_t appid, EAppInfoSection section) {
        IncrementingBuffer buf = new();
        buf.RunUntilFits(() => NativeClientApps.GetAppDataSection(appid, section, buf.Data, buf.Length, false));
        using (var stream = new MemoryStream(buf.Data))
        {
            return serializerbinary.Deserialize(stream);
        }
    }

    public ReadOnlyDictionary<EAppInfoSection, KVObject?> GetMultipleAppDataSectionsSync(AppId_t app, EAppInfoSection[] sections) {
        IncrementingBuffer buf = new(1024*sections.Length);
        int[] lengths = new int[sections.Length];
        Dictionary<EAppInfoSection, KVObject?> objects = new();
        buf.RunToFit(() => NativeClientApps.GetMultipleAppDataSections(app, sections, sections.Length, buf.Data, buf.Length, false, lengths)); 
        
        int position = 0;
        int index = 0;
        foreach (var length in lengths)
        {
            if (length > 0) {
                using (var stream = new MemoryStream(buf.Data, position, length))
                {
                    objects.Add(sections.ElementAt(index), serializerbinary.Deserialize(stream));
                }
            } else {
                objects.Add(sections.ElementAt(index), null);
            }

            position += length;
            index++;
        }

        return objects.AsReadOnly();
    }

    public async Task UpdateAppInfo(AppId_t[] apps) {
        this.NativeClientApps.RequestAppInfoUpdate(apps, apps.Length);
        await this.callbackManager.WaitForCallback<AppInfoUpdateComplete_t>();
    }

    public async Task UpdateAppInfo(AppId_t app) {
        this.NativeClientApps.RequestAppInfoUpdate(new [] { app }, 1);
        await this.callbackManager.WaitForCallback<AppInfoUpdateProgress_t>((prog) => prog.m_nAppID == app);
    }

    public async Task SyncCloud(AppId_t app) {
        await Task.Run(() => NativeClientRemoteStorage.RunAutoCloudOnAppLaunch(app));
    }

    public async Task LaunchSteamApp(CGameID gameid, uint launchOptionID, ELaunchSource launchSource, string userLaunchOptions = "") {
        
    }

    public bool IsAppInstalled(AppId_t app) {
        // This is probably good enough.
        return !this.NativeClientAppManager.GetAppInstallState(app).HasFlag(EAppState.Uninstalled);
    }

    public string GetAppInstallDir(AppId_t app) {
        IncrementingStringBuilder installDir = new();
        installDir.RunUntilFits(() => this.NativeClientAppManager.GetAppInstallDir(app, installDir.Data, installDir.Length));
        return installDir.ToString();
    }
    
    /// <summary>
    /// Installs an app.
    /// </summary>
    /// <remarks>
    /// Use ClientCompat to specify compat tools if needed.
    /// </remarks>
    public EAppUpdateError InstallApp(AppId_t app, LibraryFolder_t libraryFolder) {
        return this.NativeClientAppManager.InstallApp(app, libraryFolder, false);
    }

    public string? GetLibraryFolderPath(LibraryFolder_t libraryFolder) {
        if (libraryFolder > this.NativeClientAppManager.GetNumLibraryFolders()) {
            return null;
        }

        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.NativeClientAppManager.GetLibraryFolderPath(libraryFolder, builder.Data, builder.Length));
        return builder.ToString();
    }

    public string? GetLibraryFolderLabel(LibraryFolder_t libraryFolder) {
        if (libraryFolder > this.NativeClientAppManager.GetNumLibraryFolders()) {
            return null;
        }

        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.NativeClientAppManager.GetLibraryFolderLabel(libraryFolder, builder.Data, builder.Length));
        return builder.ToString();
    }

    public LibraryFolder_t GetAppLibraryFolder(AppId_t appid) {
        return this.NativeClientAppManager.GetAppLibraryFolder(appid);
    }

    public string GetAppBeta(AppId_t appid) {
        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.NativeClientAppManager.GetActiveBeta(appid, builder.Data, builder.Length));
        return builder.ToString();
    }

    public void SetAppBeta(AppId_t appid, string betaname) {
        this.NativeClientAppManager.SetActiveBeta(appid, betaname);
    }
}