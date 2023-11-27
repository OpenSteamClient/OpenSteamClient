using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

/// <summary>
/// Provides the base class for all kinds of apps (and configs, etc other steam types).
/// </summary>
public abstract class AppBase
{
    public abstract string Name { get; }
    public abstract string HeroURL { get; }
    public abstract string LogoURL { get; }
    public abstract string IconURL { get; }
    public abstract string PortraitURL { get; }

    public CGameID GameID { get; protected set; } = CGameID.Zero;
    public AppId_t AppID
    {
        get
        {
            return GameID.AppID();
        }
    }

    /// <summary>
    /// Use this to set a custom name. <br/> 
    /// It will override the name defined in the app's appdata sections, or in the case of mods it will override the mod's name (from it's gameinfo.txt)
    /// For inheriters: Please check this property's contents before using your actual name in get_Name.
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

    public bool IsMod => this.GameID.IsMod();
    public bool IsShortcut => this.GameID.IsShortcut();
    public bool IsSteamApp => this.GameID.IsSteamApp();
    public bool IsMisc => !(IsMod || IsShortcut || IsSteamApp);

    protected AppsManager AppsManager;
    public AppBase(AppsManager appsManager)
    {
        AppsManager = appsManager;
    }

    protected static string GetValueOverride(string? overridestr, string? valuestr)
    {
        if (!string.IsNullOrEmpty(overridestr))
        {
            return overridestr;
        }

        if (string.IsNullOrEmpty(valuestr))
        {
            return "";
        }

        return valuestr;
    }
    
    public static SteamApp CreateSteamApp(AppsManager appsManager, AppId_t appid, MemoryStream commonsection) {
        return new SteamApp(appsManager, appid, commonsection);
    }

    public static ShortcutApp CreateShortcut(AppsManager appsManager, string name, string exe, string workingDir) {
        return new ShortcutApp(appsManager, name, exe, workingDir);
    }

    public static SourcemodApp CreateSourcemod(AppsManager appsManager, string sourcemodDir, uint modid) {
        return new SourcemodApp(appsManager, sourcemodDir, modid);
    }

    protected AppBase? GetAppIfValidAppID(AppId_t appid) {
        if (appid == 0) {
            return null;
        }

        if (AppsManager == null) {
            throw new InvalidOperationException("AppsManager was null when getting ParentApp");
        }

        return AppsManager.GetAppSync(appid);
    }
}