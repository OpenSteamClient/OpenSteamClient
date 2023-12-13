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
using OpenSteamworks.Utils;
using ValveKeyValue;

namespace OpenSteamworks.ClientInterfaces;

public class ClientApps {
    private readonly IClientApps nativeClientApps;
    private readonly IClientAppManager nativeClientAppManager;
    private readonly CallbackManager callbackManager;
    private static readonly KVSerializer serializertext = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    private static readonly KVSerializer serializerbinary = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
    public ClientApps(SteamClient client) {
        this.nativeClientApps = client.NativeClient.IClientApps;
        this.nativeClientAppManager = client.NativeClient.IClientAppManager;
        this.callbackManager = client.CallbackManager;
    }

    public KVObject GetAppDataSection(AppId_t appid, EAppInfoSection section) {
        IncrementingBuffer buf = new();
        buf.RunUntilFits(() => nativeClientApps.GetAppDataSection(appid, section, buf.Data, buf.Length, false));
        using (var stream = new MemoryStream(buf.Data))
        {
            return serializerbinary.Deserialize(stream);
        }
    }

    public ReadOnlyDictionary<EAppInfoSection, KVObject> GetMultipleAppDataSectionsSync(AppId_t app, EAppInfoSection[] sections) {
        IncrementingBuffer buf = new(1024*sections.Length);
        int[] lengths = new int[sections.Length];
        buf.RunToFit(() => nativeClientApps.GetMultipleAppDataSections(app, sections, sections.Length, buf.Data, buf.Length, false, lengths));
        Dictionary<EAppInfoSection, KVObject> objects = new();
        int position = 0;
        int index = 0;
        foreach (var length in lengths)
        {
            Console.WriteLine("Idx: " + index + ", pos: " + position + ", len: " + length);
            if (length > 0) {
                using (var stream = new MemoryStream(buf.Data, position, length))
                {
                    objects.Add(sections.ElementAt(index), serializerbinary.Deserialize(stream));
                }
            }

            position += length;
            index++;
        }

        return objects.AsReadOnly();
    }

    public async Task EnsureHasAppData(AppId_t[] apps) {
        this.nativeClientApps.RequestAppInfoUpdate(apps, apps.Length);
        await this.callbackManager.WaitForCallback<AppInfoUpdateComplete_t>();
    }

    public async Task EnsureHasAppData(AppId_t app) {
        this.nativeClientApps.RequestAppInfoUpdate(new [] { app }, 1);
        await this.callbackManager.WaitForCallback<AppInfoUpdateProgress_t>((prog) => prog.m_nAppID == app);
    }

    public bool IsAppInstalled(AppId_t app) {
        // This is probably good enough.
        return !this.nativeClientAppManager.GetAppInstallState(app).HasFlag(EAppState.Uninstalled);
    }

    public string GetAppInstallDir(AppId_t app) {
        IncrementingStringBuilder installDir = new();
        installDir.RunUntilFits(() => this.nativeClientAppManager.GetAppInstallDir(app, installDir.Data, installDir.Length));
        return installDir.ToString();
    }
    
    /// <summary>
    /// Installs an app.
    /// </summary>
    /// <remarks>
    /// Use ClientCompat to specify compat tools if needed.
    /// </remarks>
    public EAppUpdateError InstallApp(AppId_t app, LibraryFolder_t libraryFolder) {
        return this.nativeClientAppManager.InstallApp(app, libraryFolder, false);
    }

    public string? GetLibraryFolderPath(LibraryFolder_t libraryFolder) {
        if (libraryFolder > this.nativeClientAppManager.GetNumLibraryFolders()) {
            return null;
        }

        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.nativeClientAppManager.GetLibraryFolderPath(libraryFolder, builder.Data, builder.Length));
        return builder.ToString();
    }

    public string? GetLibraryFolderLabel(LibraryFolder_t libraryFolder) {
        if (libraryFolder > this.nativeClientAppManager.GetNumLibraryFolders()) {
            return null;
        }

        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.nativeClientAppManager.GetLibraryFolderLabel(libraryFolder, builder.Data, builder.Length));
        return builder.ToString();
    }

    public LibraryFolder_t GetAppLibraryFolder(AppId_t appid) {
        return this.nativeClientAppManager.GetAppLibraryFolder(appid);
    }

    public string GetAppConfigValue(AppId_t appid, string key) {
        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.nativeClientAppManager.GetAppConfigValue(appid, key, builder.Data, builder.Length));
        return builder.ToString();
    }

    public void SetAppConfigValue(AppId_t appid, string key, string value) {
        this.nativeClientAppManager.SetAppConfigValue(appid, key, value);
    }

    public string GetAppBeta(AppId_t appid) {
        IncrementingStringBuilder builder = new();
        builder.RunUntilFits(() => this.nativeClientAppManager.GetActiveBeta(appid, builder.Data, builder.Length));
        return builder.ToString();
    }

    public void SetAppBeta(AppId_t appid, string betaname) {
        SetAppConfigValue(appid, "betakey", betaname);
    }
}