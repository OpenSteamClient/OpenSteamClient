using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;

namespace OpenSteamworks.Client.Config;

public class UserSettings: IConfigFile {
    static string IConfigFile.ConfigName => "UserSettings_001";
    static bool IConfigFile.PerUser => true;
    static bool IConfigFile.AlwaysSave => false;

    [ConfigName("Language", "#UserSettings_Language")]
    [ConfigDescription("Sets the language of the client.", "#UserSettings_LanguageDesc")]
    public ELanguage Language { get; set; } = ELanguage.None;
}