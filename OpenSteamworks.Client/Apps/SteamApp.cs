using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenSteamworks.Client.Apps.Compat;
using OpenSteamworks.Client.Apps.Sections;
using OpenSteamworks.Client.Extensions;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;
using OpenSteamworks.KeyValues;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Apps;

public class SteamApp : AppBase
{
    protected override string ActualName => Common.Name;
    protected override string ActualHeroURL => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/library_hero.jpg?t={this.Common.StoreAssetModificationTime}";
    protected override string ActualLogoURL => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/logo.png?t={this.Common.StoreAssetModificationTime}";
    protected override string ActualIconURL => $"https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/{this.AppID}/{this.Common.Icon}.jpg";
    protected override string ActualPortraitURL => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/library_600x900.jpg?t={this.Common.StoreAssetModificationTime}";

    public override uint StoreAssetsLastModified => uint.Parse(this.Common.StoreAssetModificationTime);

    public AppBase? ParentApp => GetAppIfValidGameID(new CGameID(this.Common.ParentAppID));
    protected readonly Logger logger;

    public AppDataCommonSection Common { get; private set; }
    public AppDataConfigSection Config { get; private set; }
    public AppDataExtendedSection Extended { get; private set; }
    public AppDataInstallSection Install { get; private set; }
    public AppDataDepotsSection Depots { get; private set; }
    public AppDataCommunitySection Community { get; private set; }
    public AppDataLocalizationSection Localization { get; private set; }

    private int? defaultLaunchOptionId;
    public IEnumerable<AppDataConfigSection.LaunchOption> AllLaunchOptions => this.Config.LaunchOptions;
    private readonly List<AppDataConfigSection.LaunchOption> filteredLaunchOptions = new();
    public override IEnumerable<AppDataConfigSection.LaunchOption> LaunchOptions => filteredLaunchOptions.AsEnumerable();
    public override int? DefaultLaunchOptionID => defaultLaunchOptionId;

    //TODO: check for app playability, eg expired playtests or expired timed trials
    public override bool IsOwnedAndPlayable => AppsManager.OwnedAppIDs.Contains(this.AppID);
    public override EAppState State => AppsManager.ClientApps.NativeClientAppManager.GetAppInstallState(AppID);
    public override ILibraryAssetAlignment? LibraryAssetAlignment => Common.LibraryAssets;
    public override EAppType Type => AppsManager.ClientApps.NativeClientApps.GetAppType(this.AppID);
    public IEnumerable<AppId_t> OwnedDLCs => AppsManager.ClientApps.GetOwnedDLCs(this.AppID);

    internal SteamApp(AppId_t appid)
    {
        var sections = AppsManager.ClientApps.GetMultipleAppDataSectionsSync(appid, new EAppInfoSection[] {EAppInfoSection.Common, EAppInfoSection.Config, EAppInfoSection.Extended, EAppInfoSection.Install, EAppInfoSection.Depots, EAppInfoSection.Community, EAppInfoSection.Localization});
        
        // The common section should always exist for all app types.
        if (sections[EAppInfoSection.Common] == null) {
            throw new NullReferenceException("Common section does not exist for app " + appid);
        }
        
        Common = TryCreateSection(sections[EAppInfoSection.Common], "common", obj => new AppDataCommonSection(obj))!;
        Config = TryCreateSection(sections[EAppInfoSection.Config], "config", obj => new AppDataConfigSection(obj))!;
        Extended = TryCreateSection(sections[EAppInfoSection.Extended], "extended", obj => new AppDataExtendedSection(obj));
        Install = TryCreateSection(sections[EAppInfoSection.Install], "install", obj => new AppDataInstallSection(obj));
        Depots = TryCreateSection(sections[EAppInfoSection.Depots], "depots", obj => new AppDataDepotsSection(obj));
        Community = TryCreateSection(sections[EAppInfoSection.Community], "community", obj => new AppDataCommunitySection(obj));
        Localization = TryCreateSection(sections[EAppInfoSection.Localization], "localization", obj => new AppDataLocalizationSection(obj));

        if (this.Common.GameID.IsValid())
        {
            this.GameID = this.Common.GameID;
        }
        else
        {
            this.GameID = new CGameID(appid);
        }

        this.logger = AppsManager.GetLoggerForApp(this);
        PopulateLaunchOptions();
    }

    private void PopulateLaunchOptions() {
        foreach (var id in AppsManager.ClientApps.GetValidLaunchOptions(this.AppID))
        {
            filteredLaunchOptions.Add(Config.LaunchOptions.Where(l => l.ID == id).First());
        }

        // foreach (var launchOption in Config.LaunchOptions)
        // {
        //     if (launchOption.Config == null) {
        //         Console.WriteLine(this.AppID + "config==null");
        //         continue;
        //     }

        //     if (!string.IsNullOrEmpty(launchOption.Config.Realm)) {
        //         if (!launchOption.Config.Realm.Contains("steamglobal")) {
        //             Console.WriteLine($"{this.AppID} realm, allowed '{launchOption.Config.Realm}', have: 'steamglobal'");
        //             continue;
        //         }
        //     }

        //     if (!string.IsNullOrEmpty(launchOption.Config.BetaKey)) {
        //         if (!launchOption.Config.BetaKey.Contains(AppsManager.GetBetaForApp(this.AppID))) {
        //             Console.WriteLine($"{this.AppID} betakey, allowed '{launchOption.Config.BetaKey}', have: '{AppsManager.GetBetaForApp(this.AppID)}'");
        //             continue;
        //         }
        //     }

        //     if (!string.IsNullOrEmpty(launchOption.Config.OSArch)) {
        //         //TODO: fix this to later consider non-64 bit platforms like ARM and whatnot when that becomes a problem
        //         if (!launchOption.Config.OSArch.Contains("64")) {
        //             continue;
        //         }
        //     }

        //     if (!string.IsNullOrEmpty(launchOption.Config.OSList)) {
        //         if (!launchOption.Config.OSList.Contains(AppsManager.GetCurrentEffectiveOSForApp(this.AppID))) {
        //             Console.WriteLine($"{this.AppID} oslist, allowed '{launchOption.Config.OSList}', have: '{AppsManager.GetCurrentEffectiveOSForApp(this.AppID)}'");
        //             continue;
        //         }
        //     }

        //     if (!string.IsNullOrEmpty(launchOption.Config.OwnsDLC)) {
        //         foreach (var item in launchOption.Config.OwnsDLC.Split(" "))
        //         {
        //             if (!OwnedDLCs.Contains(uint.Parse(item))) {
        //                 Console.WriteLine($"{this.AppID} owneddlc, allowed '{launchOption.Config.OwnsDLC}', have: '{string.Join(", ", OwnedDLCs)}'");
        //                 continue;
        //             }
        //         }
        //     }

        //     filteredLaunchOptions.Add(launchOption);
        // }

        if (filteredLaunchOptions.Count == 1) {
            defaultLaunchOptionId = filteredLaunchOptions.First().ID;
        }
    }

    private static T TryCreateSection<T>(KVObject? obj, string sectionName, Func<KVObject, T> factory) where T: TypedKVObject {
        if (obj == null) {
            return factory(new KVObject(sectionName, new List<KVObject>()));
        }

        return factory(obj);
    }
    
    /// <summary>
    /// Gets the path to the app's install dir.
    /// </summary>
    /// <param name="installDir"></param>
    /// <returns>True if the game is installed, false if it is not installed</returns>
    public bool TryGetInstallDir([NotNullWhen(true)] out string? installDir) {
        installDir = null;
        if (!AppsManager.ClientApps.IsAppInstalled(this.AppID)) {
            return false;
        }

        installDir = AppsManager.ClientApps.GetAppInstallDir(this.AppID);
        if (string.IsNullOrEmpty(installDir)) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the path to the app's libray folder's root path "/path/to/SteamLibrary/steamapps/"
    /// </summary>
    /// <param name="installDir"></param>
    /// <returns>True if the game is installed, false if it is not installed</returns>
    public bool TryGetAppLibraryFolderDir([NotNullWhen(true)] out string? installDir) {
        installDir = null;
        if (!AppsManager.ClientApps.IsAppInstalled(this.AppID)) {
            return false;
        }

        installDir = AppsManager.ClientApps.GetAppInstallDir(this.AppID);
        if (string.IsNullOrEmpty(installDir)) {
            return false;
        }

        return true;
    }

    public async Task<ProtonDBInfo> GetProtonDBCompatData() {
        string response = await Client.HttpClient.GetStringAsync($"https://www.protondb.com/api/v1/reports/summaries/{this.AppID}.json");
        JsonSerializerOptions.Default.Converters.Add(new JsonStringEnumConverter());
        var json = JsonSerializer.Deserialize<ProtonDBInfo>(response);
        if (json == null) {
            throw new NullReferenceException("Failed to get compatibility data from ProtonDB");
        }

        return json;
    }

    public bool IsCompatEnabled {
        get {
            return this.CompatTool != "";
        }

        set {
            if (value == true) {
                AppsManager.SetDefaultCompatToolForApp(this.GameID);
            } else {
                AppsManager.DisableCompatToolForApp(this.GameID);
            }
        }
    }

    public string CompatTool {
        get {
            return AppsManager.GetCurrentCompatToolForApp(this.GameID);
        }

        set {
            AppsManager.SetCompatToolForApp(this.GameID, value);
        }
    }

    public override async Task<EAppUpdateError> Launch(string userLaunchOptions, int launchOptionID)
    {
        if (this.Config.CheckForUpdatesBeforeLaunch) {
            logger.Info("Checking for updates (due to CheckForUpdatesBeforeLaunch)");
            await AppsManager.ClientApps.UpdateAppInfo(AppID);
            if (!AppsManager.ClientApps.BIsAppUpToDate(AppID)) {
                logger.Info("Not up to date, aborting launch and queuing update");
                AppsManager.ClientApps.QueueUpdate(AppID);
                return EAppUpdateError.UpdateRequired;
            }
        }

        logger.Info("Running install scripts");
        await AppsManager.RunInstallScriptAsync(AppID);

        logger.Info("Synchronizing cloud");
        await ClientRemoteStorage.SyncAppPreLaunch(AppID);

        logger.Info("Site license seat checkout");
        AppsManager.ClientApps.NativeClientUser.CheckoutSiteLicenseSeat(this.AppID);

        if (this.IsCompatEnabled) {
            logger.Info("Starting compat session (due to IsCompatEnabled == true)");
            AppsManager.StartCompatSession(this.AppID);
        }

        logger.Info("Creating process");
        return AppsManager.ClientApps.NativeClientAppManager.LaunchApp(this.GameID, (uint)launchOptionID, ELaunchSource.None, "");
    }

    public override void PauseUpdate()
    {
        AppsManager.ClientApps.NativeClientAppManager.SetDownloadingEnabled(false);
    }

    public override void Update()
    {
        AppsManager.ClientApps.QueueUpdate(this.AppID);
    }
}
