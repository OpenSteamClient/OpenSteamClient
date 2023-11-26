using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

public class ShortcutApp : AppBase {
    private string shortcutName = "";
    public override string Name => GetValueOverride(NameOverride, shortcutName);
    public override string HeroURL => HeroOverrideURL;
    public override string LogoURL => LogoOverrideURL;
    public override string IconURL => IconOverrideURL;
    public override string PortraitURL => PortraitOverrideURL;

    internal ShortcutApp(string name, string exe, string workingDir) {
        this.GameID = new CGameID(Path.Combine(workingDir, exe), name);
        this.shortcutName = name;
    }

    public override async Task Launch() {
        await Task.CompletedTask;
    }
}