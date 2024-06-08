using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using OpenSteamworks.Client.Apps.Assets;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;
using OpenSteamworks.Utils;
using Profiler;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps.Library;

public class LibraryManager : ILogonLifetime
{
    public enum ELibraryAssetType {
        Icon,
        Logo,
        Hero,
        Portrait
    }
    
    private readonly CloudConfigStore cloudConfigStore;
    private readonly ISteamClient steamClient;
    private readonly ClientMessaging clientMessaging;
    private readonly Logger logger;
    private readonly InstallManager installManager;
    private readonly LoginManager loginManager;
    private readonly AppsManager appsManager;
    private Library? currentUserLibrary;
    private readonly object libraryAssetsFileLock = new();
    private LibraryAssetsFile libraryAssetsFile = new(new KVObject("", new List<KVObject>()));
    private Thread? assetUpdateThread;
    
    // Allow 30 max download tasks at a time (to avoid getting blocked)
    private readonly SemaphoreSlim assetUpdateSemaphore = new(30);
    private readonly object appsToGenerateLock = new(); 
    private readonly List<LibraryAssetsGenerator.GenerateAssetRequest> appsToGenerate = new();
    private ConcurrentDictionary<string, LibraryAssetsFile.LibraryAsset>? assetsConcurrent;
    public string LibraryAssetsPath { get; private set; }
    

    public LibraryManager(ISteamClient steamClient, CloudConfigStore cloudConfigStore, ClientMessaging clientMessaging, LoginManager loginManager, InstallManager installManager, AppsManager appsManager) {
        this.logger = Logger.GetLogger("LibraryManager", installManager.GetLogPath("LibraryManager"));
        this.installManager = installManager;
        this.steamClient = steamClient;
        this.loginManager = loginManager;
        this.cloudConfigStore = cloudConfigStore;
        this.clientMessaging = clientMessaging;
        this.appsManager = appsManager;

        this.LibraryAssetsPath = Path.Combine(this.installManager.CacheDir, "librarycache");
        Directory.CreateDirectory(this.LibraryAssetsPath);
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        Library library = new(steamClient, cloudConfigStore, loginManager, appsManager, installManager);
        HashSet<CGameID> allUserAppIDs = await library.InitializeLibrary();
        await appsManager.ClientApps.UpdateAppInfo(allUserAppIDs.Where(a => a.IsSteamApp()).Select(a => a.AppID).ToArray());

        currentUserLibrary = library;

        LoadLibraryAssetsFile();

        //NOTE: It doesn't really matter if you use async or sync code here.
        assetUpdateThread = new Thread(async () =>
        {
            var appsToLoadAssetsFor = allUserAppIDs.Select(appid => appsManager.GetApp(appid));
            foreach (var item in appsToLoadAssetsFor)
            {
                try
                {
                    TryLoadLocalLibraryAssets(item);
                }
                catch (System.Exception e)
                {
                    logger.Error("Got error while loading library assets from cache for " + item.AppID + ": ");
                    logger.Error(e);
                }
            }

            EnsureConcurrentAssetDict();

            List<Task> processingTasks = new();
            foreach (var item in appsToLoadAssetsFor)
            {
                if (!item.NeedsLibraryAssetUpdate) {
                    continue;
                }

                if (item is not SteamApp) {
                    continue;
                }

                processingTasks.Add(DownloadAppAssets((item as SteamApp)!));
            }
            
            Task.WaitAll(processingTasks.ToArray());
            WriteConcurrentAssetDict();

            if (appsToGenerate.Any()) {
                LibraryAssetsGenerator generator = new(installManager, steamClient, clientMessaging, appsToGenerate.ToList(), LibraryAssetToFilename);
                var expectedApps = appsToGenerate.Select(r => r.AppID);
                var generatedApps = await generator.Generate();
                foreach (var item in expectedApps)
                {
                    if (!generatedApps.Contains(item)) {
                        logger.Error($"Failed to generate library assets for {item}");
                    }
                }

                appsToGenerate.Clear();
            }

            assetUpdateThread = null;
        });

        assetUpdateThread.Name = "Library Asset Update Thread";
        assetUpdateThread.Start();
    }

    [MemberNotNull(nameof(libraryAssetsFile))]
    [MemberNotNull(nameof(assetsConcurrent))]
    private void EnsureConcurrentAssetDict() {
        if (assetsConcurrent == null || libraryAssetsFile == null) {
            lock (libraryAssetsFileLock)
            {
                if (libraryAssetsFile == null) {
                    LoadLibraryAssetsFile();
                }

                if (assetsConcurrent == null) {
                    assetsConcurrent = new(libraryAssetsFile.Assets);
                }
            }
        }
    }

    private void WriteConcurrentAssetDict() {
        lock (libraryAssetsFileLock)
        {
            if (libraryAssetsFile == null) {
                logger.Info("WriteConcurrentAssetDict: libraryAssetsFile is null. Loading");
                LoadLibraryAssetsFile();
            }

            if (assetsConcurrent == null) {
                logger.Info("WriteConcurrentAssetDict: assetsConcurrent is null. Creating new");
                assetsConcurrent = new(libraryAssetsFile.Assets);
            }

            libraryAssetsFile.Assets = new(assetsConcurrent);
            SaveLibraryAssetsFile();
        }
    }

    [MemberNotNull(nameof(libraryAssetsFile))]
    private string LoadLibraryAssetsFile() {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("LibraryManager.LoadLibraryAssetsFile");
        string libraryAssetsFilePath = Path.Combine(LibraryAssetsPath, "assets.vdf");
        if (File.Exists(libraryAssetsFilePath)) {
            try
            {
                using (var stream = File.OpenRead(libraryAssetsFilePath))
                {
                    lock (libraryAssetsFileLock)
                    {
                        libraryAssetsFile = new(KVBinaryDeserializer.Deserialize(stream)); 
                    }
                }
            }
            catch (System.Exception e2)
            {
                logger.Error("Failed to load cached asset metadata. Starting from scratch.");
                logger.Error(e2);
                libraryAssetsFile = new(new KVObject("", new List<KVObject>()));
            }
        } 
        else
        {
            logger.Info("No cached asset metadata. Starting from scratch.");
            libraryAssetsFile = new(new KVObject("", new List<KVObject>()));
        }

        return libraryAssetsFilePath;
    }

    private void SaveLibraryAssetsFile() {
        logger.Info("Saving library assets.vdf");
        string libraryAssetsFilePath = Path.Combine(LibraryAssetsPath, "assets.vdf");
        string libraryAssetsTextFilePath = Path.Combine(LibraryAssetsPath, "assets_text.vdf");
        lock (libraryAssetsFileLock)
        {
            File.WriteAllText(libraryAssetsTextFilePath, KVTextSerializer.Serialize(libraryAssetsFile.UnderlyingObject));
            File.WriteAllBytes(libraryAssetsFilePath, KVBinarySerializer.SerializeToArray(libraryAssetsFile.UnderlyingObject));
        }
    }

    private void TryLoadLocalLibraryAsset(AppBase app, ELibraryAssetType assetType, out string? localPathOut) {
        EnsureConcurrentAssetDict();

        var uri = new Uri(GetURLForAssetType(app, assetType));
        if (uri.IsFile) {
            localPathOut = uri.LocalPath;
            return;
        } else {
            string targetPath = LibraryAssetToFilename(app.AppID, assetType);

            if (!File.Exists(targetPath)) {
                localPathOut = null;
                return;
            }

            // Check if our library assets are up to date
            bool upToDate = false;
            string notUpToDateReason = "";

            if (assetsConcurrent.TryGetValue(app.AppID.ToString(), out LibraryAssetsFile.LibraryAsset? assetData)) {
                if(assetData.LastChangeNumber != 0 && assetData.LastChangeNumber == steamClient.IClientApps.GetLastChangeNumberReceived()) {
                    localPathOut = targetPath;
                    return;
                }
                
                if (assetType == ELibraryAssetType.Icon) {
                    //TODO: support other app types
                    if (!string.IsNullOrEmpty(assetData.IconHash) && (app as SteamApp)?.Common.Icon == assetData.IconHash) {
                        upToDate = true;
                    } else {
                        notUpToDateReason += $"Icon hash does not match: '" + assetData.IconHash + "' app: '" + (app as SteamApp)?.Common.Icon + "' ";
                    }
                } else { 
                    var expireDate = assetType switch
                    {
                        ELibraryAssetType.Logo => assetData.LogoExpires,
                        ELibraryAssetType.Hero => assetData.HeroExpires,
                        ELibraryAssetType.Portrait => assetData.PortraitExpires,
                        _ => throw new ArgumentOutOfRangeException(nameof(assetType)),
                    };

                    if (expireDate > DateTimeOffset.UtcNow.ToUnixTimeSeconds()) {
                        upToDate = true;
                    } else {
                        notUpToDateReason += $"ExpireDate {DateTimeOffset.FromUnixTimeSeconds(expireDate)} passed, current time: {DateTimeOffset.UtcNow} ";
                    }

                    if (app.StoreAssetsLastModified != 0 && assetData.StoreAssetsLastModified < app.StoreAssetsLastModified) {
                        notUpToDateReason += $"StoreAssetsLastModified does not match cached: {assetData.StoreAssetsLastModified} app: {app.StoreAssetsLastModified} ";
                        upToDate = false;
                    }
                }
            }
            
            if (upToDate) {
                localPathOut = targetPath;
                return;
            } else {
                logger.Info($"Library asset {assetType} for {app.AppID} not up to date: {notUpToDateReason} ");
            }

            localPathOut = null;
            return;
        }
    }

    private static string GetURLForAssetType(AppBase app, ELibraryAssetType assetType) {
        return assetType switch
        {
            ELibraryAssetType.Icon => app.IconURL,
            ELibraryAssetType.Logo => app.LogoURL,
            ELibraryAssetType.Hero => app.HeroURL,
            ELibraryAssetType.Portrait => app.PortraitURL,
            _ => throw new ArgumentOutOfRangeException(nameof(assetType)),
        };
    }

    private string LibraryAssetToFilename(AppId_t appid, ELibraryAssetType assetType) {
        var suffix = assetType switch
        {
            ELibraryAssetType.Icon => "icon.jpg",
            ELibraryAssetType.Logo => "logo.png",
            ELibraryAssetType.Hero => "library_hero.jpg",
            ELibraryAssetType.Portrait => "library_600x900.jpg",
            _ => throw new ArgumentOutOfRangeException(nameof(assetType)),
        };

        return Path.Combine(this.LibraryAssetsPath, $"{appid}_{suffix}");
    }

    public async Task UpdateLibraryAssets(SteamApp app, bool suppressConcurrentDictSave = false) {
        string? iconPath = await UpdateLibraryAsset(app, ELibraryAssetType.Icon, true, true, false);
        string? logoPath = await UpdateLibraryAsset(app, ELibraryAssetType.Logo, true, true, false);
        string? heroPath = await UpdateLibraryAsset(app, ELibraryAssetType.Hero, true, true, false);
        string? portraitPath = await UpdateLibraryAsset(app, ELibraryAssetType.Portrait, true, true, true);
        app.SetLibraryAssetPaths(iconPath, logoPath, heroPath, portraitPath);

        if (!suppressConcurrentDictSave) {
            WriteConcurrentAssetDict();
        }
    }

    public async Task<string?> UpdateLibraryAsset(SteamApp app, ELibraryAssetType assetType, bool suppressSet = false, bool suppressConcurrentDictSave = false, bool lastInBatch = true) {
        EnsureConcurrentAssetDict();

        bool willGenerate = false;
        bool success = false;
        bool shouldDownload = false;

        LibraryAssetsFile.LibraryAsset? asset;
        assetsConcurrent.TryGetValue(app.AppID.ToString(), out asset);

        if (asset == null) {
            asset = new(new(app.AppID.ToString(), new List<KVObject>()));
        }

        HttpStatusCode statusCode = HttpStatusCode.Unused;
        string targetPath;
        var uri = new Uri(GetURLForAssetType(app, assetType));
        if (uri.IsFile) {
            return uri.LocalPath;
        } else {
            targetPath = LibraryAssetToFilename(app.AppID, assetType);

            // If the store assets last modified is set to 0, don't download store assets since they don't exist (but the icon is fine to download, as it's not a library asset)
            if (app.StoreAssetsLastModified == 0 && assetType != ELibraryAssetType.Icon) {
                shouldDownload = false;
            } else {
                if (asset.StoreAssetsLastModified < app.StoreAssetsLastModified) {
                    logger.Info($"Downloading {assetType} for {app.AppID} due to StoreAssetsLastModified ({asset.StoreAssetsLastModified} < {app.StoreAssetsLastModified})");
                    shouldDownload = true;
                }

                if (assetType != ELibraryAssetType.Icon && asset.GetExpires(assetType) == 0) {
                    logger.Info($"Downloading {assetType} for {app.AppID} due to GetExpires");
                    shouldDownload = true;
                }
            }

            // Don't try to download if icon hash is empty
            if (assetType == ELibraryAssetType.Icon) {
                if (string.IsNullOrEmpty(app.Common.Icon)) {
                    shouldDownload = false;
                }
            }

            if (shouldDownload) {
                logger.Info($"Downloading library asset {assetType} for {app.AppID} with url {uri}");
                using (var response = await Client.HttpClient.GetAsync(uri))
                {
                    success = response.IsSuccessStatusCode;
                    statusCode = response.StatusCode;

                    if (response.IsSuccessStatusCode) {
                        logger.Info($"Downloaded library asset {assetType} for {app.AppID} successfully, saving");
                        using var file = File.OpenWrite(targetPath);
                        response.Content.ReadAsStream().CopyTo(file);
                        logger.Info($"Saved library asset {assetType} for {app.AppID} to disk successfully");
                        if (assetType == ELibraryAssetType.Icon) {
                            logger.Info("Setting icon hash to '" + app.Common.Icon + "'");
                            asset.IconHash = app.Common.Icon;
                        } else {
                            if (response.Content.Headers.LastModified.HasValue) {
                                string headerContent = response.Content.Headers.LastModified.Value.ToString(DateTimeFormatInfo.InvariantInfo.RFC1123Pattern);
                                asset.SetLastModified(headerContent, assetType);
                            } else {
                                logger.Warning("Failed to get Last-Modified header.");
                            }
                            
                            if (response.Content.Headers.Expires.HasValue) {
                                asset.SetExpires(response.Content.Headers.Expires.Value.ToUnixTimeSeconds(), assetType);
                            } else {
                                logger.Warning("Failed to get Expires header.");
                            }
                        }
                    }

                    if (!success && response.StatusCode == HttpStatusCode.NotFound && assetType != ELibraryAssetType.Icon) {
                        lock (appsToGenerateLock)
                        {
                            int index = appsToGenerate.FindIndex(r => r.AppID == app.AppID);
                            if (index == -1) {
                                appsToGenerate.Add(new LibraryAssetsGenerator.GenerateAssetRequest(app.AppID, assetType == ELibraryAssetType.Hero, assetType == ELibraryAssetType.Portrait));
                            } else {
                                bool needsHero = appsToGenerate[index].NeedsHero;
                                bool needsPortrait = appsToGenerate[index].NeedsPortrait;
                                if (assetType == ELibraryAssetType.Hero) {
                                    needsHero = true;
                                }

                                if (assetType == ELibraryAssetType.Portrait) {
                                    needsPortrait = true;
                                }

                                appsToGenerate[index] = new LibraryAssetsGenerator.GenerateAssetRequest(appsToGenerate[index].AppID, needsHero, needsPortrait);
                            }
                        }

                        willGenerate = true;

                        if (willGenerate) {
                            logger.Debug($"Fabricating fake expire and last-modified date for asset generation of {assetType} for {app.AppID}");
                        } else {
                            logger.Debug($"Fabricating fake expire and last-modified date for failed download of {assetType} for {app.AppID}");
                        }

                        asset.SetLastModified(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(DateTimeFormatInfo.InvariantInfo.RFC1123Pattern), assetType);
                        asset.SetExpires(DateTimeOffset.UtcNow.AddYears(5).ToUnixTimeSeconds(), assetType);
                    }        
                }
            }
            
            if (!suppressSet && success) {
                switch (assetType)
                {
                    case ELibraryAssetType.Icon:
                        app.SetLibraryAssetPaths(targetPath, null, null, null);
                        break;
                    case ELibraryAssetType.Logo:
                        app.SetLibraryAssetPaths(null, targetPath, null, null);
                        break;
                    case ELibraryAssetType.Hero:
                        app.SetLibraryAssetPaths(null, null, targetPath, null);
                        break;
                    case ELibraryAssetType.Portrait:
                        app.SetLibraryAssetPaths(null, null, null, targetPath);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(assetType));
                }
            }

            if (lastInBatch) {
                asset.LastChangeNumber = steamClient.IClientApps.GetLastChangeNumberReceived();
                asset.StoreAssetsLastModified = app.StoreAssetsLastModified;
            }
        }

        assetsConcurrent[app.AppID.ToString()] = asset;
        if (!suppressConcurrentDictSave) {
            WriteConcurrentAssetDict();
        }

        if (willGenerate) {
            return null;
        }

        if (!success && shouldDownload) {
            UtilityFunctions.Assert(statusCode != HttpStatusCode.Unused);
            logger.Error($"Failed downloading library asset {assetType} for {app.AppID} (url: {uri}) (err: {statusCode})");
            return null;
        }

        if (!File.Exists(targetPath)) {
            return null;
        }

        return targetPath;
    }
    
    private async Task DownloadAppAssets(SteamApp app) {
        await assetUpdateSemaphore.WaitAsync();
        try
        {
            await UpdateLibraryAssets(app, true);
        }
        finally {
            assetUpdateSemaphore.Release();
        }
    }

    public void TryLoadLocalLibraryAssets(AppBase app) {
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Icon, out string? iconPath);
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Logo, out string? logoPath);
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Hero, out string? heroPath);
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Portrait, out string? portraitPath);
        if ((iconPath == null || logoPath == null || heroPath == null || portraitPath == null) && app is SteamApp sapp && sapp.ParentApp != null) {
            iconPath ??= sapp.ParentApp.LocalIconPath;
            logoPath ??= sapp.ParentApp.LocalLogoPath;
            app.IsUsingParentLogo = logoPath == sapp.ParentApp.LocalLogoPath;
            heroPath ??= sapp.ParentApp.LocalHeroPath;
            portraitPath ??= sapp.ParentApp.LocalPortraitPath;
        }

        app.SetLibraryAssetPaths(iconPath, logoPath, heroPath, portraitPath);
    }

    public Library GetLibrary()
    {
        if (currentUserLibrary == null) {
            throw new NullReferenceException("Attempted to get library before logon has finished.");
        }

        return currentUserLibrary;
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress) {
        if (currentUserLibrary != null) {
            await currentUserLibrary.SaveLibrary();
            currentUserLibrary = null;
        }
    }
}