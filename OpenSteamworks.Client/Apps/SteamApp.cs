using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
                return EAppType.k_EAppTypeInvalid;
            }

            return this.Common.Type.ToLowerInvariant() switch
            {
                "game" => EAppType.k_EAppTypeGame,
                "application" => EAppType.k_EAppTypeApplication,
                "music" => EAppType.k_EAppTypeMusic,
                "tool" => EAppType.k_EAppTypeTool,
                "beta" => EAppType.k_EAppTypeBeta,
                "demo" => EAppType.k_EAppTypeDemo,
                "config" => EAppType.k_EAppTypeConfig,
                "dlc" => EAppType.k_EAppTypeDlc,
                "media" => EAppType.k_EAppTypeMedia,
                "video" => EAppType.k_EAppTypeVideo,
                _ => throw new InvalidOperationException("Unknown app type " + this.Common.Type.ToLowerInvariant()),
            };
        }
    }

    public IEnumerable<AppDataConfigSection.LaunchOption> LaunchOptions => this.Config.LaunchOptions;
    public bool RequiresLaunchOption => true;

    internal SteamApp(AppsManager appsManager, AppId_t appid, MemoryStream commonsection) : base(appsManager)
    {
        SetAppInfoCommonSection(commonsection);
        if (this.Common.GameID.IsValid())
        {
            this.GameID = this.Common.GameID;
        }
        else
        {
            this.GameID = new CGameID(appid);
        }
    }

    private static T SetAppInfoSection<T>(MemoryStream stream, Func<KVObject, T> ctor) {
        if (stream == null) {
            throw new NullReferenceException("stream was null");
        }

        if (stream.Length == 0) {
            throw new Exception("Stream was empty");
        }

        KVObject obj;
        try
        {
            obj = KVSerializer.Deserialize(stream);
        }
        catch (System.Exception e)
        {
            throw new Exception("Failed to deserialize given stream.", e);
        }

        return ctor(obj)!;
    }


    [MemberNotNull(nameof(Common))]
    internal void SetAppInfoCommonSection(MemoryStream stream) {
        Common = SetAppInfoSection(stream, (kv) => new AppDataCommonSection(kv));
    }

    [MemberNotNull(nameof(Extended))]
    internal void SetAppInfoExtendedSection(MemoryStream stream) {
        Extended = SetAppInfoSection(stream, (kv) => new AppDataExtendedSection(kv));
    }

    [MemberNotNull(nameof(Config))]
    internal void SetAppInfoConfigSection(MemoryStream stream) {
        Config = SetAppInfoSection(stream, (kv) => new AppDataConfigSection(kv));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="installDir"></param>
    /// <returns>True if the game is installed</returns>
    public bool TryGetInstallDir([NotNullWhen(true)] out string? installDir) {
        installDir = null;
        return false;
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

        await AppsManager.RunInstallScript(AppID);

        return EResult.k_EResultOK;
    }
}
