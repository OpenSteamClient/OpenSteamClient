using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

/// <summary>
/// Provides the base class for all kinds of apps (and configs, etc other steam types).
/// </summary>
public abstract class AppBase
{
    /// <summary>
    /// Generic launch option for implementation and display purposes
    /// </summary>
    public interface ILaunchOption {
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }
    }

    public interface ILibraryAssetAlignment {
        /// <summary>
        /// Width percentage of the logo overlay relative to the full size of the hero
        /// </summary>
        public float LogoWidthPercentage { get; }

        /// <summary>
        /// Height percentage of the logo overlay relative to the full size of the hero
        /// </summary>
        public float LogoHeightPercentage { get; }
        public string LogoPinnedPosition { get; }
    }

    public abstract EAppState State { get; }
    public abstract EAppType Type { get; }
    public abstract IEnumerable<ILaunchOption> LaunchOptions { get; }
    public abstract int? DefaultLaunchOptionID { get; }
    public abstract Task<EAppError> Launch(string userLaunchOptions, int launchOptionID);

    protected abstract string ActualName { get; }
    protected abstract string ActualHeroURL { get; }
    protected abstract string ActualLogoURL { get; }
    protected abstract string ActualIconURL { get; }
    protected abstract string ActualPortraitURL { get; }
    
    public string Name => GetValueOverride(NameOverride, ActualName);
    public string HeroURL => GetValueOverride(HeroOverrideURL, ActualHeroURL);
    public string LogoURL => GetValueOverride(LogoOverrideURL, ActualLogoURL);
    public string IconURL => GetValueOverride(IconOverrideURL, ActualIconURL);
    public string PortraitURL => GetValueOverride(PortraitOverrideURL, ActualPortraitURL);

    public abstract bool IsOwnedAndPlayable { get; }
    public abstract uint StoreAssetsLastModified { get; }
    public CGameID GameID { get; protected set; } = CGameID.Zero;
    public AppId_t AppID
    {
        get
        {
            return GameID.AppID;
        }
    }

    /// <summary>
    /// Use this to set a custom name. <br/> 
    /// It will override the name defined in the app's appdata sections, or in the case of mods it will override the mod's name (from it's gameinfo.txt)
    /// </summary>
    public string NameOverride { get; set; } = "";

    /// <summary>
    /// Use this to set a custom hero artwork url. <br/> 
    /// It will override the artwork defined in the app's appdata sections, or in the case of mods it will override the mod's parent's hero art. 
    /// </summary>
    public string HeroOverrideURL { get; set; } = "";

    /// <summary>
    /// Use this to set a custom logo url. <br/> 
    /// It will override the artwork defined in the app's appdata sections, or in the case of mods it will override the mod's parent's logo. 
    /// </summary>
    public string LogoOverrideURL { get; set; } = "";

    /// <summary>
    /// Use this to set a custom icon url. <br/> 
    /// It will override the icon defined in the app's appdata sections, or in the case of mods it will override the mod's (gameinfo.txt) icon. 
    /// </summary>
    public string IconOverrideURL { get; set; } = "";

    /// <summary>
    /// Use this to set a custom portrait artwork url. <br/> 
    /// It will override the artwork defined in the app's appdata sections, or in the case of mods it will override the default grey portrait. 
    /// </summary>
    public string PortraitOverrideURL { get; set; } = "";
    
    public string? LocalIconPath { get; protected set; }
    public string? LocalLogoPath { get; protected set; }
    //TODO: this sucks and should be done some other way. Also the library assets system in general is very sphagetti code and should be rewritten
    public bool IsUsingParentLogo { get; set; } = false;
    public string? LocalHeroPath { get; protected set; }
    public string? LocalPortraitPath { get; protected set; }
    internal bool NeedsLibraryAssetUpdate { get; private set; }
    public abstract ILibraryAssetAlignment? LibraryAssetAlignment { get; }
    protected static AppsManager AppsManager => Client.Instance!.Container.Get<AppsManager>();
    protected static ClientRemoteStorage ClientRemoteStorage => Client.Instance!.Container.Get<ClientRemoteStorage>();

    internal void SetLibraryAssetPaths(string? iconPath, string? logoPath, string? heroPath, string? portraitPath) {
        NeedsLibraryAssetUpdate = (string.IsNullOrEmpty(iconPath) && string.IsNullOrEmpty(this.LocalIconPath)) || (string.IsNullOrEmpty(logoPath) && string.IsNullOrEmpty(this.LocalLogoPath)) || (string.IsNullOrEmpty(heroPath) && string.IsNullOrEmpty(this.LocalHeroPath)) || (string.IsNullOrEmpty(portraitPath) && string.IsNullOrEmpty(this.LocalPortraitPath));
        if (!string.IsNullOrEmpty(iconPath)) {
            this.LocalIconPath = iconPath;
        }
        
        if (!string.IsNullOrEmpty(logoPath)) {
            this.LocalLogoPath = logoPath;
        }
        
        if (!string.IsNullOrEmpty(heroPath)) {
            this.LocalHeroPath = heroPath;
        }

        if (!string.IsNullOrEmpty(portraitPath)) {
            this.LocalPortraitPath = portraitPath;
        }

        LibraryAssetsUpdated?.Invoke(this, EventArgs.Empty);
    }

    public abstract void PauseUpdate();
    public abstract void Update();

    public event EventHandler? LibraryAssetsUpdated;
    public bool IsMod => this.GameID.IsMod();
    public bool IsShortcut => this.GameID.IsShortcut();
    public bool IsSteamApp => this.GameID.IsSteamApp();
    public void Kill() {
        AppsManager.Kill(GameID);
    }

    protected static string GetValueOverride(string? overridestr, string? valuestr)
    {
        if (!string.IsNullOrEmpty(overridestr))
        {
            return overridestr;
        }

        if (string.IsNullOrEmpty(valuestr))
        {
            return string.Empty;
        }

        return valuestr;
    }

    public static SteamApp CreateSteamApp(AppId_t appid) {
        return new SteamApp(appid);
    }

    public static ShortcutApp CreateShortcut(AppId_t shortcutAppId) {
        return new ShortcutApp(shortcutAppId);
    }

    public static SourcemodApp CreateSourcemod(string sourcemodDir, uint modid) {
        return new SourcemodApp(sourcemodDir, modid);
    }

    protected AppBase? GetAppIfValidGameID(CGameID gameid) {
        if (!gameid.IsValid()) {
            return null;
        }

        return AppsManager.GetApp(gameid);
    }
}