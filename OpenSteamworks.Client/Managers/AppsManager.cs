
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
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Managers;

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

public class AppsManager : Component
{
    public HashSet<uint> OwnedAppIDs { get; init; } = new();
    private HashSet<App> LoadedApps { get; init; } = new();

    private object ownedAppsLock = new();
    private SteamClient steamClient;
    private CloudConfigStore cloudConfigStore;
    // This is bad, but the collections need to know about the library in order to be able to get apps.
    internal static Library? currentUserLibrary;

    // This is even more terrible, but avoids allocating extra references per each app
    internal static AppsManager? instance;
    private LoginManager loginManager;
    private ClientMessaging clientMessaging;
    private Logger logger;

    public EventHandler<AppPlaytimeChangedEventArgs>? AppPlaytimeChanged;
    public EventHandler<AppLastPlayedChangedEventArgs>? AppLastPlayedChanged;

    public AppsManager(SteamClient steamClient, CloudConfigStore cloudConfigStore, ClientMessaging clientMessaging, LoginManager loginManager, ConfigManager configManager, IContainer container) : base(container) {
        instance = this;
        this.steamClient = steamClient;
        this.cloudConfigStore = cloudConfigStore;
        this.clientMessaging = clientMessaging;
        this.loginManager = loginManager;
        this.logger = new Logger("AppsManager", Path.Combine(configManager.LogsDir, "AppsManager.txt"));
        this.loginManager.LoggedOn += LoggedOn;
        this.loginManager.LoggingOff += LoggingOff;
    }

    [CallbackListener<AppMinutesPlayedDataNotice_t>]
    public void OnAppMinutesPlayedDataNotice(AppMinutesPlayedDataNotice_t notice) {
        UInt32 allTime = 0;
        UInt32 lastTwoWeeks = 0;
        if (steamClient.NativeClient.IClientUser.BGetAppMinutesPlayed(notice.m_nAppID, ref allTime, ref lastTwoWeeks))
        {
            this.AppPlaytimeChanged?.Invoke(this, new AppPlaytimeChangedEventArgs(notice.m_nAppID, new AppPlaytime_t(allTime, lastTwoWeeks)));
        }
    }

    [CallbackListener<AppLastPlayedTimeChanged_t>]
    public void OnAppLastPlayedTimeChanged(AppLastPlayedTimeChanged_t lastPlayedTimeChanged) {
        AppLastPlayedChanged?.Invoke(this, new AppLastPlayedChangedEventArgs(lastPlayedTimeChanged.m_nAppID, lastPlayedTimeChanged.m_lastPlayed));
    }

    [CallbackListener<AppLicensesChanged_t>]
    public void OnAppLicensesChanged(AppLicensesChanged_t licensesChanged) {
        lock (ownedAppsLock)
        {
            if (licensesChanged.bReloadAll) {
                OwnedAppIDs.Clear();
            }

            uint[] actualAppsUpdated = new uint[licensesChanged.m_unAppsUpdated];
            Array.Copy(licensesChanged.m_rgAppsUpdated, actualAppsUpdated, licensesChanged.m_unAppsUpdated);
            lock (ownedAppsLock)
            {
                OwnedAppIDs.UnionWith(actualAppsUpdated);
            }
        }
    }

    // Can't have async handlers as it'll fuck everything up horribly (doesn't wait for event handler to finish before continuing calling code)
    public void LoggedOn(object sender, LoggedOnEventArgs e) {
        // Get all owned apps
        // uint[] ownedApps = new uint[8192];
        // uint ownedAppsCount = steamClient.NativeClient.IClientUser.GetSubscribedApps(ownedApps, 8192, false);
        // logger.Debug("SubscribedApps: " + ownedAppsCount);
        // uint[] actualOwnedApps = new uint[ownedAppsCount];
        // Array.Copy(ownedApps, actualOwnedApps, ownedAppsCount);

        // silly
        var task = this.GetAppsForSteamID(e.User.SteamID!.Value);
        task.Wait();

        var actualOwnedApps = task.Result.ToArray();
        CreateAppsSync(actualOwnedApps);
        lock (ownedAppsLock)
        {
            OwnedAppIDs.UnionWith(actualOwnedApps);
        }
    }

    /// <summary>
    /// Creates app objects for all the appids in the appids array.
    /// </summary>
    private void CreateAppsSync(uint[] appids) {
        // I don't think these are necessary here, but let's have them just in case they improve performance
        steamClient.NativeClient.IClientApps.TakeUpdateLock();

        foreach (var appid in appids)
        {
            // Skip the 0 appid, it's used by the client internally to set defaults, etc
            if (appid == 0) {
                continue;
            }
            
            try
            {
                CreateAppSync(appid);
            }
            catch (Exception e)
            {
                // These aren't fatal during init
                logger.Warning("Initializing " + appid + " failed: " + e.ToString());
            }
        }

        steamClient.NativeClient.IClientApps.ReleaseUpdateLock();
    }

    public async Task<App> GetAppAsync(uint appid) {
        // Check for an existing app
        var matches = this.LoadedApps.Where(e => e.AppID == appid);
        if (matches.Any()) {
            return matches.First();
        }

        return await Task.Run(() => CreateAppSync(appid));
    }
    
    private App CreateAppSync(uint appid) {
        var appObj = new App(appid);

        byte[] commonBytes;
        byte[] configBytes;

        // Get rid of the huge allocation as early as possible
        {
            // This should be enough for most sections, however localization sections tend to be huge and this might not work for those
            byte[] bytes = new byte[32768];

            var commonLength = steamClient.NativeClient.IClientApps.GetAppDataSection(appid, EAppInfoSection.k_EAppInfoSectionCommon, bytes, 1000000, false);
            commonBytes = new byte[commonLength];
            Array.Copy(bytes, commonBytes, commonLength);

            var configLength = steamClient.NativeClient.IClientApps.GetAppDataSection(appid, EAppInfoSection.k_EAppInfoSectionConfig, bytes, 1000000, false);
            configBytes = new byte[configLength];
            Array.Copy(bytes, configBytes, configLength);
        }

        if (commonBytes.Length != 0 && configBytes.Length != 0) {
            logger.Debug($"Initializing {appid} with appinfo length: (common: " + commonBytes.Length + ", config: " + configBytes.Length + ")");
            appObj.FillWithAppInfoBinary(commonBytes, configBytes);
        } else {
            throw new Exception(appid + " did not have an app data section. Not cached, no network or invalid app type?");
        }

        LoadedApps.Add(appObj);
        return appObj;
    }

    public void LoggingOff(object? sender, EventArgs e) {
        if (currentUserLibrary != null) {
            currentUserLibrary.SaveLibrary();
            currentUserLibrary = null;
        }

        OwnedAppIDs.Clear();
    }

    public async Task<Library> GetLibrary() {
        if (currentUserLibrary != null) {
            return currentUserLibrary;
        }

        Library library = new(steamClient, cloudConfigStore, loginManager, this);
        await library.InitializeLibrary();
        currentUserLibrary = library;
        return library;
    }

    /// <summary>
    /// May return '' or 'public' depending on the phase of the moon, angle of the sun and some other unknown factors (public seems to be the correct behaviour, does '' stand for failure?)
    /// </summary>
    public string GetBetaForApp(uint appid) {
        StringBuilder betaName = new(256);
        steamClient.NativeClient.IClientAppManager.GetActiveBeta(appid, betaName, betaName.Length);
        return betaName.ToString();
    }

    /// <summary>
    /// Gets all owned apps for a SteamID. 
    /// Will not work in offline mode. Yet. TODO: we need a robust caching system.
    /// </summary>
    /// <param name="steamid">SteamID of the user to request apps for</param>
    public async Task<HashSet<uint>> GetAppsForSteamID(CSteamID steamid, bool includeSteamPackageGames = true, bool includeFreeGames = true) {
        ProtoMsg<Protobuf.CPlayer_GetOwnedGames_Request> request = new("Player.GetOwnedGames#1");
        request.body.Steamid = steamid;
        request.body.IncludeAppinfo = false;
        request.body.IncludeExtendedAppinfo = false;
        request.body.IncludeFreeSub = includeSteamPackageGames;
        request.body.IncludePlayedFreeGames = includeFreeGames;

        ProtoMsg<Protobuf.CPlayer_GetOwnedGames_Response> response;
        HashSet<uint> ownedApps = new();
        using (var conn = this.clientMessaging.AllocateConnection())
        {
            response = await conn.ProtobufSendMessageAndAwaitResponse<Protobuf.CPlayer_GetOwnedGames_Response, Protobuf.CPlayer_GetOwnedGames_Request>(request);
        }

        foreach (var protoApp in response.body.Games)
        {
            // Why the fuck is the AppID field an int here?????
            ownedApps.Add((uint)protoApp.Appid);
        }

        return ownedApps;
    }
    public override async Task RunStartup()
    {
        Console.WriteLine("AppsManager startup");
        await EmptyAwaitable();
    }
    public override async Task RunShutdown()
    {
        Console.WriteLine("AppsManager shutdown");
        await EmptyAwaitable();
    }
}