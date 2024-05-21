using System.Text.Json;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;

using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
using Profiler;

namespace OpenSteamworks.Client.Apps.Library;

/// <summary>
/// The Library is responsible for telling you about the user's categories, and providing facilities to update the categories.
/// For all other functions (listing all apps the user owns, installing apps, getting app objects, etc) consult AppsManager.
/// </summary>
public class Library
{
    public List<Collection> Collections { get; init; } = new();
    private readonly ISteamClient steamClient;
    private readonly CloudConfigStore cloudConfigStore;
    private NamespaceData? namespaceData;
    private readonly AppsManager appsManager;
    private readonly LoginManager loginManager;
    private readonly InstallManager installManager;
    private readonly Logger logger;

    public event EventHandler? LibraryUpdated;
    internal Library(ISteamClient steamClient, CloudConfigStore cloudConfigStore, LoginManager loginManager, AppsManager appsManager, InstallManager installManager)
    {
        this.installManager = installManager;
        this.logger = Logger.GetLogger("Library", installManager.GetLogPath("Library"));
        this.steamClient = steamClient;
        this.cloudConfigStore = cloudConfigStore;
        cloudConfigStore.NamespaceUpdated += OnNamespaceUpdated;
        this.loginManager = loginManager;
        this.appsManager = appsManager;
        this.Collections.Add(new Collection("Uncategorized", "uncategorized", true));
    }

    private void OnNamespaceUpdated(object? sender, EUserConfigStoreNamespace e)
    {
        if (e != EUserConfigStoreNamespace.Library) {
            return;
        }

        Console.WriteLine("Library updated!!!");

        //TODO: The whole library system needs a rework.
        // We should be able to listen to individual collections in-ui and outside the UI, but this is what we'll do for now
        Task.Run(() => InitializeLibrary());
    }

    internal async Task<HashSet<CGameID>> InitializeLibrary()
    {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Library.InitializeLibrary");
        HashSet<CGameID> AppIDsInCollections = new();

        // Get all collections
        try
        {
            namespaceData = await cloudConfigStore.GetNamespaceData(Enums.EUserConfigStoreNamespace.Library);
            var keyValues = namespaceData.GetEntriesStartingWithKeyName("user-collections.");

            List<string> collections = new();
            foreach (var entry in keyValues)
            {
                logger.Trace("Attempting to deserialize: " + entry.Value);
                JSONCollection? json = System.Text.Json.JsonSerializer.Deserialize<JSONCollection>(entry.Value);
                if (json == null)
                {
                    throw new NullReferenceException("Deserializing collection " + entry.Key + " failed");
                }

                collections.Add(json.id);
                var collection = UpdateOrCreateCollection(Collection.FromJSONCollection(json));
                foreach (var item in this.GetAppsInCollection(collection))
                {
                    AppIDsInCollections.Add(item);
                }
            }

            // Remove collections that no longer exist
            this.Collections.RemoveAll(c => !collections.Contains(c.ID));
        }
        catch (Exception e)
        {
            logger.Error("Collections failed to deserialize: " + e.ToString());
            throw;
        }

        // Add all apps not in any categories to Uncategorized
        var uncategorized = GetCollectionByID("uncategorized");
        HashSet<CGameID> uncategorizedAppIDs = new(this.appsManager.OwnedAppsAsGameIDs);
        uncategorizedAppIDs.SymmetricExceptWith(AppIDsInCollections);
        foreach (var item in uncategorizedAppIDs)
        {
            uncategorized.AddApp(item);
        }

        foreach (var item in this.Collections)
        {
            if (item.IsDynamic) {
                item.dynamicCollectionAppsCached = await ProcessFilters(item);
            }
        }

        var all = new HashSet<CGameID>(AppIDsInCollections);
        all.UnionWith(this.appsManager.OwnedAppsAsGameIDs);

        Console.WriteLine("Firing library updated");
        try
        {
            LibraryUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (System.Exception e)
        {
            logger.Error("Error in LibraryUpdated event");
            logger.Error(e);
            throw;
        }
        
        return all;
    }

    private Collection UpdateOrCreateCollection(Collection collection) {
        var existingCollection = this.Collections.Find(c => c.ID == collection.ID);
        if (existingCollection != null) {
            existingCollection.MergeFrom(collection);
            return existingCollection;
        } else {
            this.Collections.Add(collection);
            return collection;
        }
    }

    private static void UnionOrIntersect<T>(ref HashSet<T> set, HashSet<T> target, bool union) {
        if (union) {
            set.UnionWith(target);
        } else {
            set = set.Intersect(target).ToHashSet();
        }
    }

    private HashSet<AppId_t> RunStateFilterAndGetInitialApps(Collection collection) {
        if (!collection.IsDynamic) {
            throw new InvalidOperationException("Collection not dynamic");
        }

        HashSet<AppId_t> apps = new();
        bool hasStateFilters = collection.StateFilter.FilterOptions.Count > 0;
        bool union = hasStateFilters;
        if (union) {
            logger.Info("Have filters " + string.Join(",", collection.StateFilter.FilterOptions));
        } else {
            // Begin by adding all apps the user has
            apps.UnionWith(this.appsManager.OwnedApps);
        }

        foreach (ELibraryAppStateFilter opt in collection.StateFilter.FilterOptions)
        {
            switch (opt)
            {
                case ELibraryAppStateFilter.InstalledLocally:
                    UnionOrIntersect(ref apps, appsManager.InstalledApps, union);
                    union = false;
                    break;
                case ELibraryAppStateFilter.ReadyToPlay:
                    UnionOrIntersect(ref apps, appsManager.ReadyToPlayApps, union);
                    union = false;
                    break;
                case ELibraryAppStateFilter.Played:
                    UnionOrIntersect(ref apps, appsManager.PlayedApps, union);
                    union = false;
                    break;
                case ELibraryAppStateFilter.Unplayed:
                    UnionOrIntersect(ref apps, appsManager.UnplayedApps, union);
                    union = false;
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        return apps;
    }


    /// <summary>
    /// Run through the filters of a dynamic collection.
    /// </summary>
    private async Task<HashSet<AppId_t>> ProcessFilters(Collection collection) {
        HashSet<AppId_t> apps = new();

        if (!collection.IsDynamic) {
            throw new InvalidOperationException("Collection not dynamic");
        }

        if (collection.HasFilters)
        {
            logger.Warning("Dynamic collections support is still incomplete.");
            apps = RunStateFilterAndGetInitialApps(collection);

            // Remove apps not in common with friends if in online mode
            if (this.loginManager.IsOnline())
            {
                foreach (var friendAccountID in collection.FriendsInCommonFilter.FilterOptions)
                {
                    CSteamID steamid = new(friendAccountID, EUniverse.Public, EAccountType.Individual);
                    var appsFriendOwnsTask = await appsManager.GetAppsForSteamID(steamid);

                    // An intersection takes the common elements of both arrays and returns them, abandoning all that weren't mentioned in both lists.
                    apps = apps.Intersect(appsFriendOwnsTask).ToHashSet();
                }
            }
            else
            {
                logger.Warning("Skipping friends filter since we're not connected! TODO: implement cache");
            }
        }

        return apps;
    }


    /// <summary>
    /// Uploads the library to CloudConfigStore.
    /// Will also cache locally.
    /// </summary>
    public async Task SaveLibrary()
    {
        if (this.namespaceData == null)
        {
            logger.Error("Cannot upload library, not initialized prior.");
            throw new InvalidOperationException("Cannot upload library, not initialized prior.");
        }

        foreach (var category in this.Collections)
        {
            if (category.IsSystem)
            {
                // Never save system collections
                continue;
            }

            if (category.IsPartner)
            {
                // Never save partner collections
                continue;
            }

            this.namespaceData["user-collections." + category.ID] = JsonSerializer.Serialize<JSONCollection>(category.ToJSON());
        }
        //TODO: editing collections
        //await this.cloudConfigStore.SaveNamespace(this.namespaceData);
        await this.cloudConfigStore.CacheNamespace(this.namespaceData);
    }

    /// <summary>
    /// Creates a new dynamic collection. Specify filters later with *Filter properties.
    /// </summary>
    public Collection CreateDynamicCollection(string name)
    {
        var coll = Collection.FromJSONCollection(new JSONCollection()
        {
            id = "uc-" + UtilityFunctions.GenerateRandomString(12),
            filterSpec = new JSONFilterSpec(),
            name = name,
            added = new(),
            removed = new()
        });

        this.Collections.Add(coll);
        return coll;
    }

    /// <summary>
    /// Creates a new standard collection
    /// </summary>
    public Collection CreateCollection(string name)
    {
        var coll = Collection.FromJSONCollection(new JSONCollection()
        {
            id = "uc-" + UtilityFunctions.GenerateRandomString(12),
            filterSpec = null,
            name = name,
            added = new(),
            removed = new()
        });

        this.Collections.Add(coll);
        return coll;
    }

    public Collection GetCollectionByID(string id)
    {
        return this.Collections.Where(e => e.ID == id).First();
    }

    /// <summary>
    /// Makes a dynamic collection static
    /// </summary>
    public void MakeCollectionStatic(string id)
    {
        Collection originalCollection = GetCollectionByID(id);

        this.Collections.Remove(originalCollection);
        Collection newCollection = new(originalCollection.ID, originalCollection.ID);
        this.Collections.Add(newCollection);
        newCollection.AddApps(GetAppsInCollection(originalCollection));
    }

    /// <summary>
    /// Gets the apps in a collection. Dynamic collections will run through the filters at library init time.
    /// </summary>
    private HashSet<CGameID> GetAppsInCollection(Collection collection)
    {
        HashSet<CGameID> apps = new();
        Console.WriteLine(collection.Name + " d: " + collection.dynamicCollectionAppsCached.Count + " ea: " + collection.explicitlyAddedApps.Count + " er: " + collection.explicitlyRemovedApps.Count);

        foreach (var item in collection.dynamicCollectionAppsCached)
        {
            apps.Add(new CGameID(item));
        }

        // Add explicitly added apps after filtering processes
        foreach (var item in collection.explicitlyAddedApps)
        {
            apps.Add(new CGameID(item));
        }

        // You can explicitly exclude apps from dynamic collections.
        foreach (var item in collection.explicitlyRemovedApps)
        {
            apps.Remove(new CGameID(item));
        }

        return apps;
    }

    /// <summary>
    /// Gets the apps in a collection. Dynamic collections will run through the filters.
    /// </summary>
    public HashSet<CGameID> GetAppsInCollection(string id)
    {
        return GetAppsInCollection(GetCollectionByID(id));
    }
}