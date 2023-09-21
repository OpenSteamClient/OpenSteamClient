
using System.Collections.ObjectModel;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.Generated;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Client.Managers;

public class JSONFilterGroup {
    /// <summary>
    /// Probably an enum of some sort.
    /// </summary>
    public List<int> rgOptions { get; set; } = new();
    public bool bAcceptUnion { get; set; } = false;
}

public class JSONFilterSpec {
    public int nFormatVersion { get; set; } = 2;
    public string strSearchText { get; set; } = "";
    public List<JSONFilterGroup> filterGroups { get; set; } = new();
    public List<object>? setSuggestions { get; set; } = new();
}

public class JSONCollection {
    public string id { get; set; } = "";
    public string name { get; set; } = "";

    public List<uint>? added { get; set; } = new();
    /// <summary>
    /// What does this field do?
    /// </summary>
    public List<uint>? removed { get; set; } = new();
    public JSONFilterSpec? filterSpec { get; set; } = null;
}

public class Library {
    public List<Category> Categories { get; init; } = new();
    private object appsLockObj = new object();
    private Dictionary<uint, App> knownApps = new();
    private SteamClient steamClient;
    private CloudConfigStore cloudConfigStore;
    internal Library(SteamClient steamClient, CloudConfigStore cloudConfigStore) {
        this.steamClient = steamClient;
        this.cloudConfigStore = cloudConfigStore;
        this.Categories.Add(new Category("Uncategorized", "uncategorized", true));
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
            List<uint> appIDsToInitialize = new();
            Dictionary<uint, uint> lastPlayed;
            Dictionary<uint, AppPlaytime_t> playtime;

            // First get all collections
            try
            {
                var libraryData = await cloudConfigStore.GetNamespaceData(Enums.EUserConfigStoreNamespace.k_EUserConfigStoreNamespaceLibrary);
                var keyValues = libraryData.GetKeysStartingWith("user-collections.");
                foreach (var entry in keyValues)
                {
                    JSONCollection? json = System.Text.Json.JsonSerializer.Deserialize<JSONCollection>(entry.Value);
                    if (json == null) {
                        throw new NullReferenceException("Deserializing collection " + entry.Key + " failed");
                    }
                    this.Categories.Add(Category.FromJSONCollection(json));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Collections failed to deserialize: " + e.ToString());
                throw;
            }

            await Task.Run(() => {
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
    public string ID { get; private set; }
    public string Name { get; set; }
    public bool IsDynamic { get; private set; }
    public bool IsSystem { get; private set; }
    private List<uint> explicitlyAddedApps = new();
    internal Category(string name, string id, bool system = false) {
        this.Name = name;
        this.ID = id;
        this.IsSystem = system;
    }

    /// <summary>
    /// Creates a new standard category
    /// </summary>
    /// <param name="name"></param>
    public static Category CreateCategory(string name) {

    }

    /// <summary>
    /// Makes the current dynamic collection static
    /// </summary>
    /// <param name="name"></param>
    public Category MakeStatic() {
        Category staticCategory = new Category(this.Name, this.ID);
        staticCategory.explicitlyAddedApps = this.GetApps();
    }

    /// <summary>
    /// Creates a new dynamic
    /// </summary>
    /// <param name="name"></param>
    public static Category CreateDynamicCategory(string name) {

    }

    public static Category FromJSONCollection(JSONCollection json) {
        Category category = new Category(json.name, json.id);
        // Determine if this is a dynamic category
        category.IsDynamic = json.filterSpec != null;

        if (category.IsDynamic) {
            // No need to do any special processing here, just save the filter specs
            
        } else {
            if (json.added == null) {
                json.added = new List<uint>();
            }
            category.explicitlyAddedApps = json.added;
        }

        return category;
    }

    /// <summary>
    /// Gets the apps in this category. Works for static and dynamic categories.
    /// </summary>
    public List<uint> GetApps() {
        List<uint> apps = new();
        apps.AddRange(this.explicitlyAddedApps);
        if (this.IsDynamic) {
            //TODO: process dynamic collections here
        }
        return apps;
    }

    /// <summary>
    /// Adds an app to this category.
    /// </summary>
    public void AddApp(uint appid) {
        this.explicitlyAddedApps.Add(appid);
    }

    private void ThrowIfDynamic() {
        if (this.IsDynamic) {
            throw new InvalidOperationException("This operation is invalid for dynamic categories.");
        }
    }

    private void ThrowIfStatic() {
        if (!this.IsDynamic) {
            throw new InvalidOperationException("This operation is invalid for static categories.");
        }
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
    private CloudConfigStore cloudConfigStore;
    
    public AppsManager(SteamClient steamClient, CloudConfigStore cloudConfigStore, IContainer container) : base(container) {
        this.steamClient = steamClient;
        this.cloudConfigStore = cloudConfigStore;
    }
    public async Task<Library> GetLibrary() {
        Library library = new(steamClient, cloudConfigStore);
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