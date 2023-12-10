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

public class SteamApp : AppBase, ILaunchableApp<AppDataConfigSection.LaunchOption, EResult>
{
    public override string Name => GetValueOverride(NameOverride, Common.Name);
    public AppBase? ParentApp => GetAppIfValidAppID(this.Common.ParentAppID);
    public override string HeroURL => GetValueOverride(HeroOverrideURL, $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/library_hero.jpg?t={this.Common.StoreAssetModificationTime}");
    public override string LogoURL => GetValueOverride(LogoOverrideURL, $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/logo.jpg?t={this.Common.StoreAssetModificationTime}");
    public override string IconURL => GetValueOverride(IconOverrideURL, $"https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/{this.AppID}/{this.Common.Icon}.jpg");
    public override string PortraitURL => GetValueOverride(PortraitOverrideURL, $"https://cdn.cloudflare.steamstatic.com/steam/apps/{this.AppID}/library_600x900.jpg?t={this.Common.StoreAssetModificationTime}");

    private static readonly KVSerializer KVSerializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    public AppDataCommonSection Common { get; private set; }
    public AppDataExtendedSection? Extended { get; private set; }
    public AppDataConfigSection? Config { get; private set; }

    public EAppType Type
    {
        get
        {
            // The entire Steam2 catalogue still exists, search on SteamDB for all apps of unknown type and you'll find all kinds of strange things
            // Because some people might still own these, don't throw here, instead give out an invalid app type.
            if (string.IsNullOrEmpty(this.Common.Type))
            {
                return EAppType.Invalid;
            }

            return this.Common.Type.ToLowerInvariant() switch
            {
                "game" => EAppType.Game,
                "application" => EAppType.Application,
                "music" => EAppType.Music,
                "tool" => EAppType.Tool,
                "beta" => EAppType.Beta,
                "demo" => EAppType.Demo,
                "config" => EAppType.Config,
                "dlc" => EAppType.Dlc,
                "media" => EAppType.Media,
                "video" => EAppType.Video,
                _ => throw new InvalidOperationException("Unknown app type " + this.Common.Type.ToLowerInvariant()),
            };
        }
    }

    public IEnumerable<AppDataConfigSection.LaunchOption> LaunchOptions => this.Config.LaunchOptions;
    public bool RequiresLaunchOption => true;

    internal SteamApp(AppsManager appsManager, AppId_t appid) : base(appsManager)
    {
        var sections = appsManager.ClientApps.GetMultipleAppDataSectionsSync(appid, new EAppInfoSection[] {EAppInfoSection.Common, EAppInfoSection.Config, EAppInfoSection.Extended, EAppInfoSection.Install, EAppInfoSection.Depots, EAppInfoSection.Community, EAppInfoSection.Localization});
        SetAppInfoCommonSection(sections[EAppInfoSection.Common]);
        if (this.Common.GameID.IsValid())
        {
            this.GameID = this.Common.GameID;
        }
        else
        {
            this.GameID = new CGameID(appid);
        }
    }

    private static T SetAppInfoSection<T>(KVObject obj, Func<KVObject, T> ctor) {
        return ctor(obj)!;
    }


    [MemberNotNull(nameof(Common))]
    internal void SetAppInfoCommonSection(KVObject obj) {
        Common = SetAppInfoSection(obj, (kv) => new AppDataCommonSection(kv));
    }

    [MemberNotNull(nameof(Extended))]
    internal void SetAppInfoExtendedSection(KVObject obj) {
        Extended = SetAppInfoSection(obj, (kv) => new AppDataExtendedSection(kv));
    }

    [MemberNotNull(nameof(Config))]
    internal void SetAppInfoConfigSection(KVObject obj) {
        Config = SetAppInfoSection(obj, (kv) => new AppDataConfigSection(kv));
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

    public async Task<EResult> Launch(string userLaunchOptions, AppDataConfigSection.LaunchOption? launchOption = null)
    {
        if (launchOption == null) {
            throw new NullReferenceException("launchOption was null. SteamApp launching requires a launch option");
        }

        var logger = AppsManager.GetLoggerForApp(this);

        if (this.Config.CheckForUpdatesBeforeLaunch) {
            logger.Info("Checking for updates (due to CheckForUpdatesBeforeLaunch)");
            //await AppsManager.ClientApps.EnsureHasAppData(AppID);
        }

        await AppsManager.RunInstallScriptAsync(AppID);

        return EResult.OK;
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

}
