using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

public class ShortcutApp : AppBase {
    private string shortcutName = "";
    public override string Name => GetValueOverride(NameOverride, shortcutName);
    public override string HeroURL => GetValueOverride(HeroOverrideURL, this.UserSetApp?.HeroURL);
    public override string LogoURL => GetValueOverride(LogoOverrideURL, this.UserSetApp?.LogoURL);
    public override string IconURL => GetValueOverride(IconOverrideURL, this.UserSetApp?.IconURL);
    public override string PortraitURL => GetValueOverride(PortraitOverrideURL, this.UserSetApp?.PortraitURL);
    
    /// <summary>
    /// We allow the user to set a custom appid for non-steam games. This will be used in the library to provide proton compat data and art for the game, as well as activity feeds. <br/>
    /// This will NOT allow the user to earn achievements or post activity statuses for the game however.
    /// </summary>
    public AppId_t UserSetAppID { get; set; } = 0;

    public AppBase? UserSetApp {
        get {
            if (UserSetAppID == 0) {
                return null;
            }

            return GetAppIfValidAppID(UserSetAppID);
        }
    }

    internal ShortcutApp(AppsManager appsManager, string name, string exe, string workingDir) : base(appsManager) {
        this.GameID = new CGameID(Path.Combine(workingDir, exe), name);
        this.shortcutName = name;
    }
}