using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Apps.Library;

public class Collection
{
    public string ID { get; private set; }
    public string Name { get; set; }

    [MemberNotNullWhen(true, nameof(StateFilter))]
    [MemberNotNullWhen(true, nameof(FeatureAndSupportFilter))]
    [MemberNotNullWhen(true, nameof(StoreTagsFilter))]
    [MemberNotNullWhen(true, nameof(FriendsInCommonFilter))]
    public bool IsDynamic { get; private set; }
    public bool IsSystem { get; private set; }
    public bool IsPartner { get; private set; }
    public bool HasFilters
    {
        get
        {
            ThrowIfStatic();
            if (this.StateFilter!.FilterOptions.Count > 0)
                return true;

            if (this.FeatureAndSupportFilter!.FilterOptions.Count > 0)
                return true;

            if (this.StoreTagsFilter!.FilterOptions.Count > 0)
                return true;

            if (this.FriendsInCommonFilter!.FilterOptions.Count > 0)
                return true;

            return false;
        }
    }
    public FilterGroup<ELibraryAppStateFilter>? StateFilter { get; set; }
    public FilterGroup<ELibraryAppFeaturesFilter>? FeatureAndSupportFilter { get; set; }
    public FilterGroup<int>? StoreTagsFilter { get; set; }
    public FilterGroup<uint>? FriendsInCommonFilter { get; set; }

    internal HashSet<uint> explicitlyAddedApps = new();
    internal HashSet<uint> explicitlyRemovedApps = new();

    internal Collection(string name, string id, bool system = false)
    {
        this.Name = name;
        this.ID = id;
        this.IsSystem = system;
        this.IsPartner = id.StartsWith("partner-");
        this.IsDynamic = false;
    }

    public static Collection FromJSONCollection(JSONCollection json)
    {
        Collection collection = new(json.name, json.id);
        // Determine if this is a dynamic collection
        collection.IsDynamic = json.filterSpec != null;

        if (collection.IsDynamic)
        {
            // No need to do any special processing here, just save the filter specs
            UtilityFunctions.AssertNotNull(json.filterSpec);

            if (json.filterSpec.filterGroups.Length < 7)
            {
                throw new InvalidOperationException("There are less filter groups than 7. Have the filters changed? New length is: " + json.filterSpec.filterGroups.Length);
            }

            if (json.filterSpec.filterGroups.Length > 7)
            {
                throw new InvalidOperationException("The amount of filterGroups is greater than 7. New filters? New length: " + json.filterSpec.filterGroups.Length);
            }

            collection.StateFilter = FilterGroup<ELibraryAppStateFilter>.FromJSONFilterGroup(json.filterSpec.filterGroups[1]);
            collection.FeatureAndSupportFilter = FilterGroup<ELibraryAppFeaturesFilter>.FromJSONFilterGroup(json.filterSpec.filterGroups[2]);
            collection.StoreTagsFilter = FilterGroup<int>.FromJSONFilterGroup(json.filterSpec.filterGroups[4]);
            collection.FriendsInCommonFilter = FilterGroup<uint>.FromJSONFilterGroup(json.filterSpec.filterGroups[6]);
        }
        else
        {
            if (json.added == null)
            {
                json.added = new List<uint>();
            }

            if (json.removed == null)
            {
                json.removed = new List<uint>();
            }

            collection.explicitlyAddedApps = json.added.ToHashSet();
            collection.explicitlyRemovedApps = json.removed.ToHashSet();
        }

        return collection;
    }

    /// <summary>
    /// Adds an app to this collection.
    /// </summary>
    public void AddApp(AppId_t appid)
    {
        this.explicitlyAddedApps.Add(appid);
    }

    /// <summary>
    /// Adds multiple apps to this collection.
    /// </summary>
    public void AddApps(IEnumerable<AppId_t> appids) {
        foreach (var item in appids)
        {
            this.explicitlyAddedApps.Add(item);
        }
    }

    /// <summary>
    /// Removes an app from the collection.
    /// If the collection is dynamic, it will blacklist the app from being visible in the collection
    /// </summary>
    public void RemoveApp(AppId_t appid)
    {
        if (this.IsDynamic)
        {
            this.explicitlyRemovedApps.Add(appid);
        }
        this.explicitlyAddedApps.Remove(appid);
    }

    /// <summary>
    /// Removes an exclusion for an app from a dynamic collection.
    /// </summary>
    public void RemoveExcludedApp(AppId_t appid)
    {
        this.ThrowIfStatic();
        this.explicitlyRemovedApps.Remove(appid);
    }

    internal void ThrowIfDynamic()
    {
        if (this.IsDynamic)
        {
            throw new InvalidOperationException("This operation is invalid for dynamic collections.");
        }
    }

    internal void ThrowIfStatic()
    {
        if (!this.IsDynamic)
        {
            throw new InvalidOperationException("This operation is invalid for static collections.");
        }
    }

    internal void ThrowIfSystem()
    {
        if (this.IsSystem)
        {
            throw new InvalidOperationException("This operation is invalid for system collections.");
        }
    }

    internal JSONCollection ToJSON()
    {
        JSONCollection json = new()
        {
            id = this.ID,
            name = this.Name,
            added = this.explicitlyAddedApps.ToList(),
            removed = this.explicitlyRemovedApps.ToList()
        };

        if (this.IsDynamic)
        {
            json.filterSpec = new JSONFilterSpec();
            json.filterSpec.filterGroups = new JSONFilterGroup[] {
                new JSONFilterGroup(),
                this.StateFilter.ToJSON(),
                this.FeatureAndSupportFilter.ToJSON(),
                new JSONFilterGroup(),
                this.StoreTagsFilter.ToJSON(),
                new JSONFilterGroup(),
                this.FriendsInCommonFilter.ToJSON()
            };
        }

        return json;
    }
}