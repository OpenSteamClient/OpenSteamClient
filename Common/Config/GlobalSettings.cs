using OpenSteamworks.Enums;

namespace Common.Config;

public class GlobalSettings : ConfigFile<GlobalSettings>
{
    public override GlobalSettings GetThis() => this;
    public ELanguage Language { get; set; } = ELanguage.English;
    public GlobalSettings() {}
}