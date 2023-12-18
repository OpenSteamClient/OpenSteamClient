using System.Text.Json;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Apps.Library;

/// <summary>
/// The Library is responsible for telling you about the user's categories, and providing facilities to update the categories.
/// For all other functions (listing all apps the user owns, installing apps, getting app objects, etc) consult AppsManager.
/// </summary>
public class Library
{
    public List<Collection> Collections { get; init; } = new();
    private readonly SteamClient steamClient;
    private readonly CloudConfigStore cloudConfigStore;
    private NamespaceData? namespaceData;
    private readonly AppsManager appsManager;
    private readonly LoginManager loginManager;
    private readonly InstallManager installManager;
    private readonly Logger logger;

    internal Library(SteamClient steamClient, CloudConfigStore cloudConfigStore, LoginManager loginManager, AppsManager appsManager, InstallManager installManager)
    {
        this.installManager = installManager;
        this.logger = Logger.GetLogger("Library", installManager.GetLogPath("Library"));
        this.steamClient = steamClient;
        this.cloudConfigStore = cloudConfigStore;
        this.loginManager = loginManager;
        this.appsManager = appsManager;
        this.Collections.Add(new Collection("Uncategorized", "uncategorized", true));
    }

    internal async Task InitializeLibrary()
    {
        HashSet<AppId_t> AppIDsInCollections = new();

        // Get all collections
        try
        {
            namespaceData = await cloudConfigStore.GetNamespaceData(Enums.EUserConfigStoreNamespace.Library);
            var keyValues = namespaceData.GetKeysStartingWith("user-collections.");
            foreach (var entry in keyValues)
            {
                JSONCollection? json = System.Text.Json.JsonSerializer.Deserialize<JSONCollection>(entry.Value);
                if (json == null)
                {
                    throw new NullReferenceException("Deserializing collection " + entry.Key + " failed");
                }

                var collection = Collection.FromJSONCollection(json);
                this.Collections.Add(collection);
                foreach (var item in this.GetAppsInCollection(collection))
                {
                    AppIDsInCollections.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            logger.Error("Collections failed to deserialize: " + e.ToString());
            throw;
        }

        // Add all apps not in any categories to Uncategorized
        var uncategorized = GetCollectionByID("uncategorized");
        HashSet<AppId_t> uncategorizedAppIDs = new(this.appsManager.OwnedAppIDs);
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
    }

    private void UnionOrIntersect<T>(ref HashSet<T> set, HashSet<T> target, bool union) {
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
        bool union = collection.StateFilter.FilterOptions.Count == 1;
        if (union) {
            logger.Info("Have filters " + string.Join(",", collection.StateFilter.FilterOptions));
        } else {
            // Begin by adding all apps the user has
            apps.UnionWith(this.appsManager.OwnedAppIDs);
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

            if (collection.StateFilter.FilterOptions.Count == 1) {
                logger.Info("Have filters " + string.Join(",", collection.StateFilter.FilterOptions));
                foreach (ELibraryAppStateFilter opt in collection.StateFilter.FilterOptions)
                {
                    switch (opt)
                    {
                        case ELibraryAppStateFilter.InstalledLocally:
                            apps.UnionWith(appsManager.InstalledApps);
                            break;
                        case ELibraryAppStateFilter.ReadyToPlay:
                            apps.UnionWith(appsManager.ReadyToPlayApps);
                            break;
                        case ELibraryAppStateFilter.Played:
                            apps.UnionWith(appsManager.PlayedApps);
                            break;
                        case ELibraryAppStateFilter.Unplayed:
                            apps.UnionWith(appsManager.UnplayedApps);
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
            } else {
                // Begin by adding all apps the user has
                apps.UnionWith(this.appsManager.OwnedAppIDs);
            }

            // Remove apps not in common with friends if in online mode
            if (this.loginManager.IsOnline())
            {
                foreach (var friendAccountID in collection.FriendsInCommonFilter.FilterOptions)
                {
                    CSteamID steamid = CSteamID.FromAccountID(friendAccountID);
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

        await this.cloudConfigStore.SaveNamespace(this.namespaceData);
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
    /// Gets the apps in a collection. Dynamic collections will run through the filters.
    /// </summary>
    public HashSet<AppId_t> GetAppsInCollection(Collection collection)
    {
        HashSet<AppId_t> apps = new();

        foreach (var item in collection.dynamicCollectionAppsCached)
        {
            apps.Add(item);
        }

        // Add explicitly added apps after filtering processes
        foreach (var item in collection.explicitlyAddedApps)
        {
            apps.Add(item);
        }

        // You can explicitly exclude apps from dynamic collections.
        foreach (var item in collection.explicitlyRemovedApps)
        {
            apps.Remove(item);
        }

        return apps;
    }

    /// <summary>
    /// Gets the apps in a collection. Dynamic collections will run through the filters.
    /// </summary>
    public HashSet<AppId_t> GetAppsInCollection(string id)
    {
        return GetAppsInCollection(GetCollectionByID(id));
    }
}