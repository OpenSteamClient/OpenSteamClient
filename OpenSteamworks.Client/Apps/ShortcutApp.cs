using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

public class ShortcutApp : AppBase {
    private string shortcutName = "";
    public override string Name => GetValueOverride(NameOverride, shortcutName);
    public override string HeroURL => HeroOverrideURL;
    public override string LogoURL => LogoOverrideURL;
    public override string IconURL => IconOverrideURL;
    public override string PortraitURL => PortraitOverrideURL;

    internal ShortcutApp(AppsManager appsManager, string name, string exe, string workingDir) : base(appsManager) {
        this.GameID = new CGameID(Path.Combine(workingDir, exe), name);
        this.shortcutName = name;
    }
}