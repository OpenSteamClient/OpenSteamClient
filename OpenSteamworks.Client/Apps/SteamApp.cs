using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenSteamworks.Client.Apps.Compat;
using OpenSteamworks.Client.Apps.Sections;
using OpenSteamworks.Client.Extensions;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps;

public class SteamApp : AppBase
{
    public class LaunchOption : ILaunchOption
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string CommandLine { get; init; }

        public LaunchOption(int id, string name, string desc, string commandLine)
        {
            this.ID = id;
            this.Name = name;
            this.Description = desc;
            this.CommandLine = commandLine;
        }
    }

    protected override string ActualName => Common.Name;
    protected override string ActualHeroURL => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/library_hero.jpg?t={this.Common.StoreAssetModificationTime}";
    protected override string ActualLogoURL => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/logo.jpg?t={this.Common.StoreAssetModificationTime}";
    protected override string ActualIconURL => $"https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/{this.AppID}/{this.Common.Icon}.jpg";
    protected override string ActualPortraitURL => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/library_600x900.jpg?t={this.Common.StoreAssetModificationTime}";

    public AppBase? ParentApp => GetAppIfValidGameID(new CGameID(this.Common.ParentAppID));
    protected readonly Logger logger;

    public AppDataCommonSection Common { get; private set; }
    public AppDataConfigSection Config { get; private set; }
    public AppDataExtendedSection Extended { get; private set; }
    public AppDataInstallSection Install { get; private set; }
    public AppDataDepotsSection Depots { get; private set; }
    public AppDataCommunitySection Community { get; private set; }
    public AppDataLocalizationSection Localization { get; private set; }
    public EAppType Type
    {
        get
        {
            // The entire Steam2 catalogue still exists, search on SteamDB for all apps of unknown type and you'll find all kinds of strange things
            // Because some people might still own these, don't throw here, instead give out an invalid app type.
            if (string.IsNullOrEmpty(this.Common.Type))
            {
                logger.Warning("Encountered empty type field for app " + this.AppID);
                return EAppType.Invalid;
            }

            return this.Common.Type.ToLowerInvariant() switch
            {

                // These are handled by this "main" SteamApp class
                "game" => EAppType.Game,
                "application" => EAppType.Application,
                "beta" => EAppType.Beta,
                "demo" => EAppType.Demo,

                // These are not playable (most of the time, some DLC's are separate and playable as well)
                //TODO: handle configs elegantly
                "config" => EAppType.Config,
                "dlc" => EAppType.Dlc,

                // These mean dedicated servers, not "actual" tools like Hammer, etc. We could maybe setup some sort of server hosting UI for dedicated servers? 
                // Or atleast allow the user to specify the command line to run the server from OpenSteamClient.
                "tool" => EAppType.Tool,

                // This is handled via SteamSoundtrackApp
                "music" => EAppType.Music,

                // These are videos, which should just popup in the user's browser. TODO: eventually support this as well, but maybe not
                "video" => EAppType.Video,
                "media" => EAppType.Media,

                _ => throw new InvalidOperationException("Unknown app type " + this.Common.Type.ToLowerInvariant()),
            };
        }
    }

    internal SteamApp(AppsManager appsManager, AppId_t appid) : base(appsManager)
    {
        this.logger = appsManager.GetLoggerForApp(this);

        var sections = appsManager.ClientApps.GetMultipleAppDataSectionsSync(appid, new EAppInfoSection[] {EAppInfoSection.Common, EAppInfoSection.Config, EAppInfoSection.Extended, EAppInfoSection.Install, EAppInfoSection.Depots, EAppInfoSection.Community, EAppInfoSection.Localization});
        
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
    }

    private static T TryCreateSection<T>(KVObject? obj, string sectionName, Func<KVObject, T> factory) where T: KVObjectEx {
        if (obj == null) {
            return factory(new KVObject(sectionName, ""));
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
        if (!this.AppsManager.ClientApps.IsAppInstalled(this.AppID)) {
            return false;
        }

        installDir = this.AppsManager.ClientApps.GetAppInstallDir(this.AppID);
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
        if (!this.AppsManager.ClientApps.IsAppInstalled(this.AppID)) {
            return false;
        }

        installDir = this.AppsManager.ClientApps.GetAppInstallDir(this.AppID);
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
                this.AppsManager.SetDefaultCompatToolForApp(this.GameID);
            } else {
                this.AppsManager.DisableCompatToolForApp(this.GameID);
            }
        }
    }

    public string CompatTool {
        get {
            return this.AppsManager.GetCurrentCompatToolForApp(this.GameID);
        }

        set {
            this.AppsManager.SetCompatToolForApp(this.GameID, value);
        }
    }

    private int? defaultLaunchOptionId;
    private readonly List<LaunchOption> launchOptions = new();
    public override IEnumerable<LaunchOption> LaunchOptions => launchOptions;
    public override int? DefaultLaunchOptionID => defaultLaunchOptionId;
    
    public override async Task<EAppUpdateError> Launch(string userLaunchOptions, int launchOptionID)
    {
        if (this.Config.CheckForUpdatesBeforeLaunch) {
            logger.Info("Checking for updates (due to CheckForUpdatesBeforeLaunch)");
            if (!this.AppsManager.ClientApps.BIsAppUpToDate(this.AppID)) {
                return EAppUpdateError.UpdateRequired;
            }
        }

        await AppsManager.RunInstallScriptAsync(AppID);

        return EAppUpdateError.NoError;
    }
}
