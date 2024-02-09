using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;

namespace OpenSteamworks.Client.Config;

public class LibrarySettings: IConfigFile {
    static string IConfigFile.ConfigName => "LibrarySettings_001";
    static bool IConfigFile.PerUser => true;
    static bool IConfigFile.AlwaysSave => true;

    [ConfigNeverVisible]
    public AppId_t LastSelectedAppID { get; set; } = 0;
}