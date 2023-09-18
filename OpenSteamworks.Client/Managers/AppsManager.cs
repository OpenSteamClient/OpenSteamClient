
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.Generated;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Client.Managers;

public class Library {
    public List<Category> Categories = new();
    private object appsLockObj = new object();
    private Dictionary<uint, App> knownApps = new();
    private SteamClient steamClient;
    internal Library(SteamClient steamClient) {
        this.steamClient = steamClient;
        this.Categories.Add(new Category("Uncategorized"));
    }

    [CallbackListener<AppMinutesPlayedDataNotice_t>]
    public void OnAppMinutesPlayedDataNotice(AppMinutesPlayedDataNotice_t notice) {
        lock (appsLockObj)
        {
            if (this.knownApps.ContainsKey(notice.m_nAppID)) {
                UInt32 allTime = 0;
                UInt32 lastTwoWeeks = 0;
                if (steamClient.NativeClient.IClientUser.BGetAppMinutesPlayed(notice.m_nAppID, ref allTime, ref lastTwoWeeks)) {
                    this.knownApps[notice.m_nAppID].UpdatePlaytime(allTime, lastTwoWeeks);
                }
            }
        }
    }

    [CallbackListener<AppLastPlayedTimeChanged_t>]
    public void OnAppLastPlayedTimeChanged(AppLastPlayedTimeChanged_t lastPlayedTimeChanged) {
        lock (appsLockObj)
        {
            if (this.knownApps.ContainsKey(lastPlayedTimeChanged.m_nAppID)) {
                this.knownApps[lastPlayedTimeChanged.m_nAppID].UpdateLastPlayedTime(lastPlayedTimeChanged.m_lastPlayed);
            }
        }
    }

    internal async Task InitializeLibrary() {
        await Task.Run(() => {
            List<uint> appIDsToInitialize = new();
            Dictionary<uint, uint> lastPlayed;
            Dictionary<uint, AppPlaytime_t> playtime;

            {
                uint[] ownedApps = new uint[4096];
                uint ownedAppsCount = steamClient.NativeClient.IClientUser.GetSubscribedApps(ownedApps, 4096, false);
                Console.WriteLine("SubscribedApps: " + ownedAppsCount);
                uint[] actualOwnedApps = new uint[ownedAppsCount];
                Array.Copy(ownedApps, actualOwnedApps, ownedAppsCount);
                appIDsToInitialize.AddRange(actualOwnedApps);
            }

            unsafe {
                var map = new CUtlMap<uint, uint>(1, 80000);
                Console.WriteLine("LastPlayedMap: " + steamClient.NativeClient.IClientUser.BGetAppsLastPlayedMap(&map));
                lastPlayed = map.ToManagedAndFree();
                foreach (var item in lastPlayed)
                {
                    if (!appIDsToInitialize.Contains(item.Key)) {
                        appIDsToInitialize.Add(item.Key);
                    }

                    Console.WriteLine(item.Key+":"+item.Value);
                }
            }

            unsafe {
                var map = new CUtlMap<uint, AppPlaytime_t>(1, 80000);
                Console.WriteLine("PlaytimeMap: " + steamClient.NativeClient.IClientUser.BGetAppPlaytimeMap(&map));
                playtime = map.ToManagedAndFree();
                foreach (var item in playtime)
                {
                    if (!appIDsToInitialize.Contains(item.Key)) {
                        appIDsToInitialize.Add(item.Key);
                    }

                    Console.WriteLine(item.Key+":"+item.Value);
                }
            }

            AppPlaytime_t appPlaytime;
            uint appLastPlayed;
            foreach (var app in appIDsToInitialize)
            {
                if (!playtime.TryGetValue(app, out appPlaytime)) {
                    appPlaytime = new AppPlaytime_t();
                }

                if (!lastPlayed.TryGetValue(app, out appLastPlayed)) {
                    appLastPlayed = 0;
                }

                knownApps.Add(app, new App(app, appLastPlayed, appPlaytime));
            }
        });
    }

    public async Task<App> GetApp(uint appid) {
        return await Task.Run(() => {
            if (this.knownApps.ContainsKey(appid)) {
                return this.knownApps[appid];
            }

            throw new Exception("Appid " + appid + " not known by AppsManager");
        });
    }
}

public class Category {
    //TODO: support dynamic collections (how?)
    public string Name;
    public List<uint> AppsInCollection = new();
    public Category(string name) {
        this.Name = name;
    }
}

public class App {
    public UInt32 AppID;
    public DateTime LastPlayed;
    public TimeSpan PlaytimeAllTime;
    public TimeSpan PlaytimeLastTwoWeeks;
    internal App(UInt32 appid, uint lastPlayed, AppPlaytime_t playtime) {
        this.AppID = appid;
        this.UpdateLastPlayedTime(lastPlayed);
        this.UpdatePlaytime(playtime.allTime, playtime.lastTwoWeeks);
    }

    internal void UpdatePlaytime(UInt32 allTime, UInt32 lastTwoWeeks) {
        this.PlaytimeAllTime = TimeSpan.FromMinutes(allTime);
        this.PlaytimeLastTwoWeeks = TimeSpan.FromMinutes(lastTwoWeeks);
    }

    internal void UpdateLastPlayedTime(UInt32 lastPlayed) {
        this.LastPlayed = DateTimeOffset.FromUnixTimeSeconds(lastPlayed).DateTime;
    }

    public override string ToString()
    {
        return string.Format("[App {0}] Last Played: {1}, Playtime: {2} (All time) {3} (Last two weeks)", this.AppID, this.LastPlayed, this.PlaytimeAllTime, this.PlaytimeLastTwoWeeks);
    }
}

public class AppsManager : Component
{
    private SteamClient steamClient;
    
    public AppsManager(SteamClient steamClient, IContainer container) : base(container) {
        this.steamClient = steamClient;
    }
    public async Task<Library> GetLibrary() {
        Library library = new(steamClient);
        await library.InitializeLibrary();
        return library;
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