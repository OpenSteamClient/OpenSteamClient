
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
using static OpenSteamworks.Callbacks.CallbackManager;

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

public class AppsManager : ILogonLifetime
{
    public HashSet<AppId_t> OwnedAppIDs { get; init; } = new();
    // I'm not a huge fan of this as it means the appid gets duplicated, but whatever
    private Dictionary<AppId_t, App> LoadedApps { get; init; } = new();

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

    public AppsManager(SteamClient steamClient, CloudConfigStore cloudConfigStore, ClientMessaging clientMessaging, LoginManager loginManager, ConfigManager configManager) {
        instance = this;
        this.steamClient = steamClient;
        this.cloudConfigStore = cloudConfigStore;
        this.clientMessaging = clientMessaging;
        this.loginManager = loginManager;
        this.logger = new Logger("AppsManager", Path.Combine(configManager.LogsDir, "AppsManager.txt"));
    }

    [CallbackListener<AppMinutesPlayedDataNotice_t>]
    public void OnAppMinutesPlayedDataNotice(CallbackHandler handler, AppMinutesPlayedDataNotice_t notice) {
        UInt32 allTime = 0;
        UInt32 lastTwoWeeks = 0;
        if (steamClient.NativeClient.IClientUser.BGetAppMinutesPlayed(notice.m_nAppID, ref allTime, ref lastTwoWeeks))
        {
            this.AppPlaytimeChanged?.Invoke(this, new AppPlaytimeChangedEventArgs(notice.m_nAppID, new AppPlaytime_t(allTime, lastTwoWeeks)));
        }
    }

    [CallbackListener<AppLastPlayedTimeChanged_t>]
    public void OnAppLastPlayedTimeChanged(CallbackHandler handler, AppLastPlayedTimeChanged_t lastPlayedTimeChanged) {
        AppLastPlayedChanged?.Invoke(this, new AppLastPlayedChangedEventArgs(lastPlayedTimeChanged.m_nAppID, lastPlayedTimeChanged.m_lastPlayed));
    }

    [CallbackListener<AppLicensesChanged_t>]
    public void OnAppLicensesChanged(CallbackHandler handler, AppLicensesChanged_t licensesChanged) {
        lock (ownedAppsLock)
        {
            if (licensesChanged.bReloadAll) {
                OwnedAppIDs.Clear();
            }

            AppId_t[] actualAppsUpdated = new AppId_t[licensesChanged.m_unAppsUpdated];
            Array.Copy(licensesChanged.m_rgAppsUpdated, actualAppsUpdated, licensesChanged.m_unAppsUpdated);
            OwnedAppIDs.UnionWith(actualAppsUpdated);
        }
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        // silly
        var ownedApps = await this.GetAppsForSteamID(e.User.SteamID!.Value);

        progress.SetSubOperation("#WaitingForAppinfoUpdate");
        Console.WriteLine("Waiting for appinfo update");
        await RequestAppInfoUpdateForApps(ownedApps);
        Console.WriteLine("Appinfo update finished");

        UpdateMultipleAppObjectsSync(ownedApps);
        lock (ownedAppsLock)
        {
            OwnedAppIDs.UnionWith(ownedApps);
        }
    }

    /// <summary>
    /// Creates app objects for all the appids in the appids array.
    /// </summary>
    private void UpdateMultipleAppObjectsSync(IEnumerable<AppId_t> appids) {
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
                UpdateAppObjectSync(appid);
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
        if (this.LoadedApps.TryGetValue(appid, out App? val)) {
            return val;
        } else {
            return await Task.Run(() => UpdateAppObjectSync(appid));
        }
    }
    
    private App UpdateAppObjectSync(AppId_t appid) {
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

        logger.Debug($"{appid} got appinfo with length: (common: " + commonBytes.Length + ", config: " + configBytes.Length + ")");
        if (commonBytes.Length != 0 && configBytes.Length != 0) {
            appObj.FillWithAppInfoBinary(commonBytes, configBytes);
        } else {
            throw new Exception(appid + " did not have an app data section. Not cached, no network or invalid app type?");
        }

        LoadedApps[appid] = appObj;
        return appObj;
    }

    public Task RequestAppInfoUpdateForApp(AppId_t appid) {
        return RequestAppInfoUpdateForApps(new AppId_t[1] { appid });
    }

    public async Task RequestAppInfoUpdateForApps(IEnumerable<AppId_t> apps) {
        this.steamClient.NativeClient.IClientApps.RequestAppInfoUpdate(apps.ToArray(), (uint)apps.LongCount());
        await this.steamClient.CallbackManager.WaitForCallback(AppInfoUpdateComplete_t.CallbackID);
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress) {
        if (currentUserLibrary != null) {
            await currentUserLibrary.SaveLibrary();
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
    public string GetBetaForApp(AppId_t appid) {
        StringBuilder betaName = new(256);
        steamClient.NativeClient.IClientAppManager.GetActiveBeta(appid, betaName, betaName.Length);
        return betaName.ToString();
    }

    /// <summary>
    /// Gets all owned apps for a SteamID. 
    /// Will not work in offline mode. Yet. TODO: we need a robust caching system.
    /// </summary>
    /// <param name="steamid">SteamID of the user to request apps for</param>
    public async Task<HashSet<AppId_t>> GetAppsForSteamID(CSteamID steamid, bool includeSteamPackageGames = true, bool includeFreeGames = true) {
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

        return ownedApps;
    }
}