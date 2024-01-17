
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Apps.Assets;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;

using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using ValveKeyValue;
using static OpenSteamworks.Callbacks.CallbackManager;

namespace OpenSteamworks.Client.Apps;

public class AppPlaytimeChangedEventArgs : EventArgs {
    public AppPlaytimeChangedEventArgs(uint appid, AppPlaytime_t playtime) { AppID = appid; PlaytimeAllTime = TimeSpan.FromMinutes(playtime.AllTime); PlaytimeLastTwoWeeks = TimeSpan.FromMinutes(playtime.LastTwoWeeks); }
    public uint AppID { get; }
    public TimeSpan PlaytimeAllTime { get; }
    public TimeSpan PlaytimeLastTwoWeeks { get; }
}

public class AppLastPlayedChangedEventArgs : EventArgs {
    public AppLastPlayedChangedEventArgs(uint appid, UInt32 lastPlayed) { AppID = appid; LastPlayed = DateTimeOffset.FromUnixTimeSeconds(lastPlayed).DateTime; }
    public uint AppID { get; }
    public DateTime LastPlayed { get; }
}

public class AppsManager : ILogonLifetime
{
    private readonly ISteamClient steamClient;
    private readonly ClientMessaging clientMessaging;
    private readonly Logger logger;
    private readonly InstallManager installManager;
    private readonly LoginManager loginManager;
    public readonly ClientApps ClientApps;
    public string LibraryAssetsPath { get; private set; }

    public EventHandler<AppPlaytimeChangedEventArgs>? AppPlaytimeChanged;
    public EventHandler<AppLastPlayedChangedEventArgs>? AppLastPlayedChanged;

    // These apps cause issues. They have no appinfo sections, so they're blacklisted. Apps that fail to initialize during startup are also added to this list.
    private static readonly HashSet<AppId_t> appsFilter = new() { 5, 3482, 346790, 375350, 470950, 472500, 483470, 503590, 561370, 957691, 957692, 972340, 977941, 1275680, 1331320, 2130210, 2596140 };
    /// <summary>
    /// Gets ALL owned AppIDs of the current user. Includes all configs. Will probably show 1000+ apps.
    /// </summary>
    public HashSet<AppId_t> OwnedApps {
        get {
            IncrementingUIntArray iua = new(256);
            iua.RunToFit(() => this.steamClient.IClientUser.GetSubscribedApps(iua.Data, iua.UIntLength, true));
            return iua.Data.Select(a => (AppId_t)a).Except(appsFilter).ToHashSet();
        }
    }

    public HashSet<AppId_t> InstalledApps {
        get {
            var len = this.steamClient.IClientAppManager.GetNumInstalledApps();
            var arr = new uint[len];
            this.steamClient.IClientAppManager.GetInstalledApps(arr, len);
            return arr.Select(a => (AppId_t)a).ToHashSet();
        }
    }

    //TODO: streamable apps
    public HashSet<AppId_t> ReadyToPlayApps {
        get {
            var apps = InstalledApps;
            Console.WriteLine("apps: " + apps.Count);
            apps = apps.Where(this.steamClient.IClientAppManager.BIsAppUpToDate).ToHashSet();
            Console.WriteLine("apps2: " + apps.Count);
            return apps;
        }
    }
    
    private Dictionary<AppId_t, RTime32> appLastPlayedMap = new();
    private Dictionary<AppId_t, AppPlaytime_t> appPlaytimeMap = new();

    public HashSet<AppId_t> PlayedApps {
        get {
            HashSet<AppId_t> apps = new();
            foreach (var item in appLastPlayedMap)
            {
                apps.Add(item.Key);
            }

            return apps;
        }
    }

    public HashSet<AppId_t> UnplayedApps {
        get {
            HashSet<AppId_t> apps = OwnedApps;
            apps = apps.Except(PlayedApps).ToHashSet();
            return apps;
        }
    }

    public AppsManager(ISteamClient steamClient, ClientApps clientApps, ClientMessaging clientMessaging, InstallManager installManager, LoginManager loginManager) {
        this.logger = Logger.GetLogger("AppsManager", installManager.GetLogPath("AppsManager"));
        this.ClientApps = clientApps;
        this.loginManager = loginManager;
        this.steamClient = steamClient;
        this.clientMessaging = clientMessaging;
        this.installManager = installManager;
        this.LibraryAssetsPath = Path.Combine(this.installManager.CacheDir, "librarycache");
        Directory.CreateDirectory(this.LibraryAssetsPath);
        steamClient.CallbackManager.RegisterHandler<AppMinutesPlayedDataNotice_t>(OnAppMinutesPlayedDataNotice);
        steamClient.CallbackManager.RegisterHandler<AppLastPlayedTimeChanged_t>(OnAppLastPlayedTimeChanged);
    }

    public void OnAppMinutesPlayedDataNotice(CallbackHandler<AppMinutesPlayedDataNotice_t> handler, AppMinutesPlayedDataNotice_t notice) {
        UInt32 allTime = 0;
        UInt32 lastTwoWeeks = 0;
        if (steamClient.IClientUser.BGetAppMinutesPlayed(notice.m_nAppID, ref allTime, ref lastTwoWeeks))
        {
            this.AppPlaytimeChanged?.Invoke(this, new AppPlaytimeChangedEventArgs(notice.m_nAppID, new AppPlaytime_t(allTime, lastTwoWeeks)));
        }
    }

    public void OnAppLastPlayedTimeChanged(CallbackHandler<AppLastPlayedTimeChanged_t> handler, AppLastPlayedTimeChanged_t lastPlayedTimeChanged) {
        AppLastPlayedChanged?.Invoke(this, new AppLastPlayedChangedEventArgs(lastPlayedTimeChanged.m_nAppID, lastPlayedTimeChanged.m_lastPlayed));
    }

    private bool hasLogOnFinished = false;
    private readonly object libraryAssetsFileLock = new();
    private LibraryAssetsFile libraryAssetsFile = new(new KVObject("", new List<KVObject>()));
    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        hasLogOnFinished = true;

        if (steamClient.ConnectedWith == ConnectionType.NewClient) {
            unsafe {
                CUtlMap<AppId_t, RTime32> mapn = new(0, 4096);
                if (steamClient.IClientUser.BGetAppsLastPlayedMap(&mapn)) {
                    appLastPlayedMap = mapn.ToManagedAndFree();
                } else {
                    mapn.Free();
                }
            }

            unsafe {
                CUtlMap<AppId_t, AppPlaytime_t> mapn = new(0, 4096);
                if (steamClient.IClientUser.BGetAppPlaytimeMap(&mapn)) {
                    appPlaytimeMap = mapn.ToManagedAndFree();
                } else {
                    mapn.Free();
                }
            }
        } else {
            foreach (var ownedAppID in OwnedApps)
            {
                var lastPlayed = steamClient.IClientUser.GetAppLastPlayedTime(ownedAppID);
                if (lastPlayed != 0) {
                    appLastPlayedMap[ownedAppID] = lastPlayed;
                }

                var playtime = this.steamClient.ClientConfigStore.GetInt(EConfigStore.UserLocal, $"Software\\Valve\\Steam\\Apps\\{ownedAppID}\\Playtime") ?? 0;
                var playtime2wks = this.steamClient.ClientConfigStore.GetInt(EConfigStore.UserLocal, $"Software\\Valve\\Steam\\Apps\\{ownedAppID}\\Playtime2wks") ?? 0;
                if (playtime != 0 || playtime2wks != 0) {
                    appPlaytimeMap[ownedAppID] = new AppPlaytime_t((uint)playtime, (uint)playtime2wks);
                }
            }
        }

        List<AppBase> createdApps = new();
        foreach (var item in OwnedApps)
        {
            try
            {
                createdApps.Add(GetApp(new CGameID(item)));
            }
            catch (System.Exception e2)
            {
                appsFilter.Add(item);
                logger.Warning("Failed to initialize owned app " + item + " at logon time");
                logger.Warning(e2);
            }
        }

        string libraryAssetsFilePath = Path.Combine(LibraryAssetsPath, "assets.vdf");
        KVSerializer serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
        if (File.Exists(libraryAssetsFilePath)) {
            try
            {
                using (var stream = File.OpenRead(libraryAssetsFilePath))
                {
                    libraryAssetsFile = new(serializer.Deserialize(stream));
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

        //NOTE: It doesn't really matter if you use async or sync code here.
        new Thread(() =>
        {
            foreach (var item in createdApps)
            {
                try
                {
                    if (item.Type == EAppType.Game || item.Type == EAppType.Application || item.Type == EAppType.Music || item.Type == EAppType.Beta) {
                        TryLoadLocalLibraryAssets(item);
                    }
                }
                catch (System.Exception e)
                {
                    logger.Error("Got error while loading library assets from cache for " + item.AppID + ": ");
                    logger.Error(e);
                }
            }

            // Allow 15 max download tasks at a time
            SemaphoreSlim semaphore = new(15);
            List<Task> processingTasks = new();
            foreach (var item in createdApps)
            {
                if (!item.NeedsLibraryAssetUpdate) {
                    continue;
                }

                processingTasks.Add(new Task(async () =>
                {
                    Console.WriteLine("Waiting for semaphore");
                    await semaphore.WaitAsync();
                    Console.WriteLine("Got semaphore");
                    try
                    {
                        await UpdateLibraryAssets(item);
                    }
                    finally {
                        semaphore.Release();
                    }
                }));
            }

            foreach (var item in processingTasks)
            {
                item.Start();
            }
            
            Task.WaitAll(processingTasks.ToArray());
            using (var writeStream = File.OpenWrite(libraryAssetsFilePath))
            {
                serializer.Serialize(writeStream, libraryAssetsFile.UnderlyingObject, "");
            }
        }).Start();
    }

    public enum ELibraryAssetType {
        Icon,
        Logo,
        Hero,
        Portrait
    }

    public void TryLoadLocalLibraryAssets(AppBase app) {
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Icon, out string? iconPath);
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Logo, out string? logoPath);
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Hero, out string? heroPath);
        TryLoadLocalLibraryAsset(app, ELibraryAssetType.Portrait, out string? portraitPath);
        app.SetLibraryAssetPaths(iconPath, logoPath, heroPath, portraitPath);
    }

    private void TryLoadLocalLibraryAsset(AppBase app, ELibraryAssetType assetType, out string? localPathOut) {
        var uri = new Uri(GetURLForAssetType(app, assetType));
        if (uri.IsFile) {
            localPathOut = uri.LocalPath;
            return;
        } else {
            // Check if our library assets are up to date
            bool upToDate = false;
            if (libraryAssetsFile.Assets.TryGetValue(app.AppID.ToString(), out LibraryAssetsFile.LibraryAsset? assetData)) {
                if (assetType == ELibraryAssetType.Icon) {
                    //TODO: support other app types
                    if ((app as SteamApp)?.Common.Icon == assetData.IconHash) {
                        upToDate = true;
                    }
                } else {
                    uint expireDate = assetType switch
                    {
                        ELibraryAssetType.Logo => assetData.LogoExpires,
                        ELibraryAssetType.Hero => assetData.HeroExpires,
                        ELibraryAssetType.Portrait => assetData.PortraitExpires,
                        _ => throw new ArgumentOutOfRangeException(nameof(assetType)),
                    };

                    if (expireDate > DateTimeOffset.UtcNow.ToUnixTimeSeconds()) {
                        upToDate = true;
                    }
                }

                // If these don't match, always redownload
                if (assetData.LastChangeNumber < app.LibraryAssetChangeNumber) {
                    upToDate = false;
                }
            }

            string targetPath = LibraryAssetToFilename(app.AppID, assetType);
            
            if (upToDate) {
                localPathOut = targetPath;
                return;
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

    public async Task UpdateLibraryAssets(AppBase app) {
        string iconPath = await UpdateLibraryAsset(app, ELibraryAssetType.Icon, true);
        string logoPath = await UpdateLibraryAsset(app, ELibraryAssetType.Logo, true);
        string heroPath = await UpdateLibraryAsset(app, ELibraryAssetType.Hero, true);
        string portraitPath = await UpdateLibraryAsset(app, ELibraryAssetType.Portrait, true);
        app.SetLibraryAssetPaths(iconPath, logoPath, heroPath, portraitPath);
    }

    public async Task<string> UpdateLibraryAsset(AppBase app, ELibraryAssetType assetType, bool suppressSet = false) {
        LibraryAssetsFile.LibraryAsset? asset;
        //Console.WriteLine("Waiting for libraryAssetsFileLock");
        libraryAssetsFile.Assets.TryGetValue(app.AppID.ToString(), out asset);

        if (asset == null) {
            asset = new(new("", ""));
        }

        string targetPath;
        var uri = new Uri(GetURLForAssetType(app, assetType));
        if (uri.IsFile) {
            return uri.LocalPath;
        } else {
            targetPath = LibraryAssetToFilename(app.AppID, assetType);

            //TODO: how should we check for the correct version of the assets without redownloading each time
            if (!File.Exists(targetPath) || app.LibraryAssetChangeNumber > asset.LastChangeNumber) {
                using (var response = await Client.HttpClient.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode) {
                        using var file = File.OpenWrite(targetPath);
                        response.Content.ReadAsStream().CopyTo(file);
                    }

                    if (response.Headers.TryGetValues("Last-Modified", out IEnumerable<string>? lastModifiedMulti)) {
                        if (!lastModifiedMulti.Any() || lastModifiedMulti.Count() > 1) {
                            logger.Warning("Got 0 or >1 Last-Modified headers.");
                        } else {
                            string headerContent = lastModifiedMulti.First();
                            asset.SetLastModified(headerContent, assetType);
                        }
                    }
                }
            }
            
            if (!suppressSet) {
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
        }

        //Console.WriteLine("Waiting for libraryAssetsFileLock");
        // lock (libraryAssetsFileLock)
        // {
            //Console.WriteLine("Got lock");
            libraryAssetsFile.Assets[app.AppID.ToString()] = asset;
        // }
        //Console.WriteLine("Free'd lock");

        return targetPath;
    }

    private List<AppBase> apps = new();

    /// <summary>
    /// Gets an app. Initializes it synchronously if not previously initialized.
    /// </summary>
    /// <param name="gameid"></param>
    /// <returns></returns>
    public AppBase GetApp(CGameID gameid) {
        var existingApp = apps.Where((a) => a.GameID == gameid).FirstOrDefault();
        if (existingApp != null) {
            return existingApp;
        }

        var app = AppBase.CreateSteamApp(this, gameid.AppID);
        apps.Add(app);
        return app;
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress) {
        hasLogOnFinished = false;
        appLastPlayedMap.Clear();
        appPlaytimeMap.Clear();
        apps.Clear();
        await Task.CompletedTask;
    }

    public void Kill(CGameID gameid) {
        this.steamClient.IClientUser.TerminateGame(gameid.AppID, true);
    }

    /// <summary>
    /// May return '' or 'public' depending on the phase of the moon, angle of the sun and some other unknown factors (public seems to be the correct behaviour, does '' stand for failure?)
    /// </summary>
    public string GetBetaForApp(AppId_t appid) {
        IncrementingStringBuilder betaName = new();
        betaName.RunUntilFits(() => steamClient.IClientAppManager.GetActiveBeta(appid, betaName.Data, betaName.Length));
        return betaName.ToString();
    }

    /// <summary>
    /// Gets all owned apps for a SteamID. 
    /// Will not work in offline mode. Yet. TODO: we need a robust caching system.
    /// </summary>
    /// <param name="steamid">SteamID of the user to request apps for</param>
    public async Task<HashSet<AppId_t>> GetAppsForSteamID(CSteamID steamid, bool includeSteamPackageGames = false, bool includeFreeGames = true) {
        logger.Debug("Attempting to get owned apps for " + steamid);
        ProtoMsg<Protobuf.CPlayer_GetOwnedGames_Request> request = new("Player.GetOwnedGames#1");
        request.body.Steamid = steamid;
        request.body.IncludeAppinfo = false;
        request.body.IncludeExtendedAppinfo = false;
        request.body.IncludeFreeSub = includeSteamPackageGames;
        request.body.IncludePlayedFreeGames = includeFreeGames;

        ProtoMsg<Protobuf.CPlayer_GetOwnedGames_Response> response;
        HashSet<AppId_t> ownedApps = new();
        using (var conn = this.clientMessaging.AllocateConnection())
        {
            response = await conn.ProtobufSendMessageAndAwaitResponse<Protobuf.CPlayer_GetOwnedGames_Response, Protobuf.CPlayer_GetOwnedGames_Request>(request);
        }

        foreach (var protoApp in response.body.Games)
        {
            // Why the fuck is the AppID field an int here?????
            ownedApps.Add((uint)protoApp.Appid);
        }

        logger.Debug(steamid + " owns " + ownedApps.Count + " games");
        return ownedApps;
    }

    public async Task RunInstallScriptAsync(AppId_t appid, bool uninstall = false) {
        await Task.Run(() => RunInstallScriptSync(appid, uninstall));
    }

    public void RunInstallScriptSync(AppId_t appid, bool uninstall = false) {
        //TODO: we still aren't 100% sure about the second arg.
        steamClient.IClientUser.RunInstallScript(appid, "english", false);
        while (steamClient.IClientUser.IsInstallScriptRunning() != 0)
        {
            Thread.Sleep(30);
        }
    }

    public void SetDefaultCompatToolForApp(CGameID gameid) {
        string defaultCompatTool = this.steamClient.IClientCompat.GetCompatToolName(0);
        if (string.IsNullOrEmpty(defaultCompatTool)) {
            throw new InvalidOperationException("Can't set default config tool for app " + gameid + ", since no default compat tool has been specified!");
        }

        this.SetCompatToolForApp(gameid, defaultCompatTool);
    }

    public void DisableCompatToolForApp(CGameID gameid) {
        this.steamClient.IClientCompat.SpecifyCompatTool(gameid.AppID, "", "", 0);
    }

    public string GetCurrentCompatToolForApp(CGameID gameid) {
        if (!this.steamClient.IClientCompat.BIsCompatibilityToolEnabled(gameid.AppID)) {
            return "";
        }
        
        return this.steamClient.IClientCompat.GetCompatToolName(gameid.AppID);
    }

    public void SetCompatToolForApp(CGameID gameid, string compatToolName) {
        this.steamClient.IClientCompat.SpecifyCompatTool(gameid.AppID, compatToolName, "", 250);
    }

    public void SetDefaultCompatTool(string compatToolName) {
        this.steamClient.IClientCompat.SpecifyCompatTool(0, compatToolName, "", 75);
    }

    public async Task<EResult> LaunchApp(AppBase app, int launchOption, string userLaunchOptions) {
        return EResult.OK;
        // //TODO: make this actually async, not spaghetti, use compatmanager, use app class, add validation, test on windows, create a better keyvalue system with arrays, maybe other issues
        // var logger = this.logger.CreateSubLogger("LaunchApp");

        // string workingDir = "";
        // string gameExe = "";
        // if (app.IsSteamApp) {
        //     SteamApp steamApp = (SteamApp)app;
           

        //     //TODO: What function should we use here?
        //     if (this.steamClient.IClientRemoteStorage.IsCloudEnabledForAccount() && this.steamClient.IClientRemoteStorage.IsCloudEnabledForApp(app.AppID)) {
        //         this.steamClient.IClientRemoteStorage.RunAutoCloudOnAppLaunch(app.AppID);
        //     }

        //     logger.Info("Getting launch info");
        //     StringBuilder gameInstallDir = new(1024);
        //     StringBuilder launchOptionExe = new(1024);
        //     StringBuilder launchOptionCommandLine = new(1024);
        //     this.steamClient.IClientAppManager.GetAppInstallDir(app.AppID, gameInstallDir, 1024);
            
        //     //TODO: do these keys exist 100% of the time for all apps?
        //     // For EA games, they usually only have an executable "link2ea://launchgame/" which isn't valid on filesystem, and also are missing the 'arguments' key. WTF?
        //     //TODO: how to handle link2ea protocol links (especially with proton) 
        //     this.steamClient.IClientApps.GetAppData(app.AppID, $"config/launch/{launchOption}/executable", launchOptionExe, 1024);
        //     this.steamClient.IClientApps.GetAppData(app.AppID, $"config/launch/{launchOption}/arguments", launchOptionCommandLine, 1024);
        //     workingDir = gameInstallDir.ToString();
        //     gameExe = launchOptionExe.ToString();
        // } else if (app.IsShortcut) {

        // } else if (app.IsMod) {

        // } else {
        //     throw new UnreachableException("Something is seriously wrong.");
        // }

        // string commandLine = "";

        // // First fill compat tools
        // if (this.steamClient.IClientCompat.BIsCompatLayerEnabled() && this.steamClient.IClientCompat.BIsCompatibilityToolEnabled(appid))
        // {
        //     //TODO: how to handle "selected by valve testing" tools (like for csgo)
        //     string compattool = this.steamClient.IClientCompat.GetCompatToolName(app.AppID);
        //     if (!string.IsNullOrEmpty(compattool)) {
        //         KVSerializer serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        //         bool useSessions = false;
        //         List<AppId_t> compatToolAppIDs = new();
        //         //TODO: this is terrible, but the API's don't seem to provide other ways to do this (HOW DO NON-STEAM COMPAT TOOLS WORK??????)
        //         bool hasDepTool = true;
        //         StringBuilder compatToolAppidStr = new(1024);
        //         StringBuilder compatToolInstallDir = new(1024);

        //         //TODO: loop over compat_tools, map into dictionaries
        //         this.steamClient.IClientApps.GetAppData(891390, $"extended/compat_tools/{compattool}/appid", compatToolAppidStr, 1024);

        //         AppId_t depTool = uint.Parse(compatToolAppidStr.ToString());
        //         compatToolAppIDs.Add(depTool);
        //         while (hasDepTool) {
        //             this.steamClient.IClientAppManager.GetAppInstallDir(depTool, compatToolInstallDir, 1024);
        //             var bytes = File.ReadAllBytes(Path.Combine(compatToolInstallDir.ToString(), "toolmanifest.vdf"));
        //             KVObject manifest;
        //             using (var stream = new MemoryStream(bytes))
        //             {
        //                 manifest = serializer.Deserialize(stream);
        //             }

        //             hasDepTool = (string?)manifest["require_tool_appid"] != null;
        //             if (hasDepTool) {
        //                 depTool = uint.Parse((string)manifest["require_tool_appid"]);
        //                 compatToolAppIDs.Add(depTool);
        //             }
        //         }

                
        //         foreach (var compatToolAppID in compatToolAppIDs)
        //         {
        //             this.steamClient.IClientAppManager.GetAppInstallDir(compatToolAppID, compatToolInstallDir, 1024);
        //             var bytes = File.ReadAllBytes(Path.Combine(compatToolInstallDir.ToString(), "toolmanifest.vdf"));
        //             //TODO: how to decide correct verb to use?
        //             var verb = "waitforexitandrun";
        //             KVObject manifest;
        //             using (var stream = new MemoryStream(bytes))
        //             {
        //                 manifest = serializer.Deserialize(stream);
        //             }

        //             if ((string)manifest["version"] != "2") {
        //                 throw new Exception("Unsupported manifest version '" + (string)manifest["version"] + "'");
        //             }

        //             if ((string)manifest["use_sessions"] == "1") {
        //                 useSessions = true;
        //             }

        //             var commandline = (string)manifest["commandline"];
        //             commandline = commandline.Replace("%verb%", verb);
        //             var compatToolCommandLine = $"'{compatToolInstallDir}'{commandline}";
        //             commandLine = compatToolCommandLine + " " + commandLine;
        //         }

        //         if (useSessions) {
        //             this.steamClient.IClientCompat.StartSession(app.AppID);
        //         }
        //     }
        // }

        // // Then prefix with reaper, launchwrapper
        // if (OperatingSystem.IsLinux()) {
        //     string steamToolsPath = Path.Combine(this.installManager.InstallDir, "linux64");

        //     // This is the base command line. It seems to always be used for all games, regardless of if you have compat tools enabled or not.
        //     commandLine = $"{steamToolsPath}/reaper SteamLaunch AppId={app.AppID} -- {steamToolsPath}/steam-launch-wrapper -- " + commandLine;
        // }

        // //TODO: does windows use x86launcher.exe or x64launcher.exe?

        // commandLine += $"'{workingDir}/{gameExe}' {launchOptionCommandLine}";

        // if (userLaunchOptions.Contains("%command%")) {
        //     string prefix = userLaunchOptions.Split("%command%")[0];
        //     string suffix = userLaunchOptions.Split("%command%")[1];
        //     // Yep. It's this simple. this allows stuff like %command%-mono for tModLoader
        //     commandLine = prefix + commandLine + suffix;
        // }

        // //TODO: synchronize controller config (how?)
        // //TODO: handle "site licenses" (extra wtf???)

        // if (app.IsMod) {
        //     //TODO: where to get sourcemod path?
        //     commandLine += " -game sourcemodpathhere";
        // }

        // logger.Info("Built launch command line: " + commandLine);
        // logger.Info("Creating process");
        // using (var vars = new TemporaryEnvVars())
        // {
        //     // Do we really need all these vars?
        //     // Valid vars: STEAM_GAME_LAUNCH_SHELL STEAM_RUNTIME_LIBRARY_PATH STEAM_COMPAT_FLAGS SYSTEM_PATH SYSTEM_LD_LIBRARY_PATH SUPPRESS_STEAM_OVERLAY STEAM_CLIENT_CONFIG_FILE SteamRealm SteamLauncherUI
        //     vars.SetEnvironmentVariable("SteamAppUser", this.loginManager.CurrentUser!.AccountName);
        //     vars.SetEnvironmentVariable("SteamAppId", app.AppID.ToString());
        //     vars.SetEnvironmentVariable("SteamGameId", ((ulong)app.GameID).ToString());
        //     vars.SetEnvironmentVariable("SteamClientLaunch", "1");
        //     vars.SetEnvironmentVariable("SteamClientService", "127.0.0.1:57344");
        //     vars.SetEnvironmentVariable("AppId", app.AppID.ToString());
        //     vars.SetEnvironmentVariable("SteamLauncherUI", "clientui");
        //     //vars.SetEnvironmentVariable("PROTON_LOG", "1");
        //     vars.SetEnvironmentVariable("STEAM_COMPAT_CLIENT_INSTALL_PATH", installManager.InstallDir);
        //     this.steamClient.IClientUtils.SetLastGameLaunchMethod(0);  
        //     StringBuilder libraryFolder = new(1024);
        //     this.steamClient.IClientAppManager.GetLibraryFolderPath(this.steamClient.IClientAppManager.GetAppLibraryFolder(app.AppID), libraryFolder, libraryFolder.Capacity);
        //     logger.Info("Appid " + app.AppID + " is installed into library folder " + this.steamClient.IClientAppManager.GetAppLibraryFolder(app.AppID) + " with path " + libraryFolder);
        //     vars.SetEnvironmentVariable("STEAM_COMPAT_DATA_PATH", Path.Combine(libraryFolder.ToString(), "steamapps", "compatdata", app.AppID.ToString()));
            
        //     logger.Info("Vars for launch: ");
        //     foreach (var item in UtilityFunctions.GetEnvironmentVariables())
        //     {
        //         logger.Info(item.Key + "=" + item.Value);
        //     }

        //     // For some reason, the CGameID SpawnProcess takes is a pointer.
        //     CGameID gameidref = app.GameID;
        //     this.steamClient.IClientUser.SpawnProcess("", commandLine, workingDir, ref gameidref, app.Name);
        // }

        // return EResult.OK;
    }
    
    public Logger GetLoggerForApp(AppBase app) {
        string name;
        if (app.IsSteamApp) {
            name = "SteamApp";
        } else if (app.IsMod) {
            name = "Mod";
        } else if (app.IsShortcut) {
            name = "Shortcut";
        } else {
            name = "App";
        }

        name += app.GameID;
        return Logger.GetLogger(name, installManager.GetLogPath(name));
    }
}