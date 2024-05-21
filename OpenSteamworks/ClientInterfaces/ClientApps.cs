using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using Profiler;

namespace OpenSteamworks.ClientInterfaces;

public class ClientApps {
    public IClientApps NativeClientApps { get; init; }
    public IClientAppManager NativeClientAppManager { get; init; }
    public IClientUser NativeClientUser { get; init; }
    public IClientRemoteStorage NativeClientRemoteStorage { get; init; }
    
    private readonly CallbackManager callbackManager;
    public ClientApps(ISteamClient client) {
        this.NativeClientApps = client.IClientApps;
        this.NativeClientAppManager = client.IClientAppManager;
        this.NativeClientUser = client.IClientUser;
        this.NativeClientRemoteStorage = client.IClientRemoteStorage;
        this.callbackManager = client.CallbackManager;
    }

    public IEnumerable<AppId_t> GetInstalledApps() {
        var arr = new uint[2];
        var len = this.NativeClientAppManager.GetInstalledApps(arr, (uint)arr.Length);
        Console.WriteLine("len: " + len);
        arr = new uint[4096];
        len = this.NativeClientAppManager.GetInstalledApps(arr, (uint)arr.Length);
        Console.WriteLine("newlen: " + len);
        return arr.Select(a => (AppId_t)a);
    }

    public bool BIsAppUpToDate(AppId_t appid)
    {
        var state = this.NativeClientAppManager.GetAppInstallState(appid);
        if (state.HasFlag(EAppState.UpdateRequired)) {
            return false;
        }

        if (state.HasFlag(EAppState.FilesCorrupt)) {
            return false;
        }

        if (state.HasFlag(EAppState.FilesMissing)) {
            return false;
        }

        return this.NativeClientAppManager.BIsAppUpToDate(appid);
    }

    public string GetAppName(AppId_t appid) {
        var namechild = GetAppDataSection(appid, EAppInfoSection.Common).GetChild("name");
        if (namechild == null || namechild.Value is not string) {
            return "SteamApp" + appid;
        }

        return namechild.GetValueAsString();
    }

    public EAppType GetAppType(AppId_t appid) => NativeClientApps.GetAppType(appid);

    public static string GetRootNameForAppInfoSection(EAppInfoSection section) {
        return section switch
        {
            EAppInfoSection.Common => "common",
            EAppInfoSection.Extended => "extended",
            EAppInfoSection.Config => "config",
            EAppInfoSection.Stats => "stats",
            EAppInfoSection.Install => "install",
            EAppInfoSection.Depots => "depots",
            EAppInfoSection.Vac => "vac",
            EAppInfoSection.Drm => "drm",
            EAppInfoSection.Ufs => "ufs",
            EAppInfoSection.Ogg => "ogg",
            EAppInfoSection.Items => "items",
            EAppInfoSection.Policies => "policies",
            EAppInfoSection.Sysreqs => "sysreqs",
            EAppInfoSection.Community => "community",
            EAppInfoSection.Store => "store",
            EAppInfoSection.Localization => "localization",
            EAppInfoSection.Broadcastgamedata => "broadcastgamedata",
            EAppInfoSection.Computed => "computed",
            EAppInfoSection.Albummetadata => "albummetadata",
            _ => throw new ArgumentOutOfRangeException(nameof(section)),
        };
    }

    public int[] GetValidLaunchOptions(AppId_t appid) {
        int[] launchOpts = new int[4096];
        int len = NativeClientApps.GetAvailableLaunchOptions(appid, launchOpts, (uint)launchOpts.Length);
        return launchOpts[0..len];
    }

    public IEnumerable<AppId_t> GetOwnedDLCs(AppId_t app) {
        var dlcLen = this.NativeClientApps.GetDLCCount(app);
        var dlcs = new List<AppId_t>();
        StringBuilder builder = new(128);
        for (int i = 0; i < dlcLen; i++)
        {
            if (NativeClientApps.BGetDLCDataByIndex(app, i, out uint dlcID, out bool _, builder, builder.Capacity)) {
                dlcs.Add(dlcID);
            }
        }

        return dlcs.AsEnumerable();
    }

    public KVObject GetAppDataSection(AppId_t appid, EAppInfoSection section) {
        IncrementingBuffer buf = new();
        buf.RunToFit(() => NativeClientApps.GetAppDataSection(appid, section, buf.Data, buf.Length, false));
        if (!buf.Data.Any(b => b != 0)) {
            return new KVObject(GetRootNameForAppInfoSection(section), new List<KVObject>());
        }
        
        using (var stream = new MemoryStream(buf.Data))
        {
            return KVBinaryDeserializer.Deserialize(stream);
        }
    }

    public ReadOnlyDictionary<EAppInfoSection, KVObject?> GetMultipleAppDataSectionsSync(AppId_t app, EAppInfoSection[] sections) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("ClientApps.GetMultipleAppDataSectionsSync");

        IncrementingBuffer buf = new(4096*sections.Length);
        int[] lengths = new int[sections.Length];
        Dictionary<EAppInfoSection, KVObject?> objects = new();

        {
            using var subScope = CProfiler.CurrentProfiler?.EnterScope("IClientApps.GetMultipleAppDataSections");
            buf.RunToFit(() => NativeClientApps.GetMultipleAppDataSections(app, sections, sections.Length, buf.Data, buf.Length, false, lengths)); 
        }
        
        int position = 0;
        int index = 0;
        foreach (var length in lengths)
        {
            if (length > 0) {
                using (var stream = new MemoryStream(buf.Data, position, length))
                {
                    objects.Add(sections.ElementAt(index), KVBinaryDeserializer.Deserialize(stream));
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
        var task = this.callbackManager.AsTask<AppInfoUpdateComplete_t>();
        if (this.NativeClientApps.RequestAppInfoUpdate(apps.Select(a => (uint)a).ToArray(), apps.Length)) {
            await task;
        }
    }

    public async Task UpdateAppInfo(AppId_t app) {
        await UpdateAppInfo([app]);
    }

    public void QueueUpdate(AppId_t app) {
        //TODO: find a way to update a single game at a time. Will probably need a DownloadQueueManager or something similar.
        // Even ValveSteam cannot do this, as it starts downloading all other apps in download queue afterwards.
        this.NativeClientAppManager.ChangeAppDownloadQueuePlacement(app, EAppDownloadQueuePlacement.PriorityUserInitiated);
    }

    public void Update(AppId_t app) {
        if (!IsAppInstalled(app)) {
            throw new InvalidOperationException("Cannot update app that isn't installed");
        }
        
        var libraryFolder = GetAppLibraryFolder(app);
        var result = this.NativeClientAppManager.InstallApp(app, libraryFolder, true);
        if (result != EAppError.NoError) {
            throw new Exception("Update failed with result " + result);
        }
    }

    public bool IsAppInstalled(AppId_t app) {
        // This is probably good enough.
        return this.NativeClientAppManager.GetAppInstallState(app).HasFlag(EAppState.FullyInstalled);
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
    public EAppError InstallApp(AppId_t app, LibraryFolder_t libraryFolder) {
        var result = this.NativeClientAppManager.InstallApp(app, libraryFolder, false);
        if (result == EAppError.NoError) {
            this.NativeClientAppManager.SetDownloadingEnabled(true);
        }

        return result;
    }

    public string GetLibraryFolderPath(LibraryFolder_t libraryFolder) {
        ThrowIfLibraryFolderOutOfBounds(libraryFolder);

        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.NativeClientAppManager.GetLibraryFolderPath(libraryFolder, builder.Data, builder.Length));
        return builder.ToString();
    }

    public string GetLibraryFolderLabel(LibraryFolder_t libraryFolder) {
        ThrowIfLibraryFolderOutOfBounds(libraryFolder);

        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.NativeClientAppManager.GetLibraryFolderLabel(libraryFolder, builder.Data, builder.Length));
        return builder.ToString();
    }

    private void ThrowIfLibraryFolderOutOfBounds(LibraryFolder_t libraryFolder) {
        if (libraryFolder > this.NativeClientAppManager.GetNumLibraryFolders()) {
            throw new InvalidOperationException("Library folder " + libraryFolder + " is not in range of 0-" + this.NativeClientAppManager.GetNumLibraryFolders());
        }
    }

    public int GetNumLibraryFolders() => this.NativeClientAppManager.GetNumLibraryFolders();
    public uint GetNumAppsInFolder(LibraryFolder_t libraryFolder) {
        ThrowIfLibraryFolderOutOfBounds(libraryFolder);
        return this.NativeClientAppManager.GetNumAppsInFolder(libraryFolder);
    }

    public unsafe IEnumerable<AppId_t> GetAppsInFolder(LibraryFolder_t libraryFolder) {
        ThrowIfLibraryFolderOutOfBounds(libraryFolder);
        IncrementingUIntArray arr = new();
        arr.RunUntilFits(() => this.NativeClientAppManager.GetAppsInFolder(libraryFolder, arr.Data, arr.Length));
        return arr.Data.Select(u => (AppId_t)u).ToList();
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