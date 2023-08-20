using OpenSteamworks.Enums;

namespace OpenSteamworks.Client.Config;

public class GlobalSettings : ConfigFile<GlobalSettings>
{
    public override GlobalSettings GetThis() => this;
    public ELanguage Language { get; set; } = ELanguage.English;
    public bool EnableWebHelper { get; set; } = false;
    public GlobalSettings() {}
}