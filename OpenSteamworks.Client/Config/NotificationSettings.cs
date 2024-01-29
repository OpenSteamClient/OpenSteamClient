using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;

namespace OpenSteamworks.Client.Config;

public class NotificationSettings: IConfigFile {
    static string IConfigFile.ConfigName => "NotificationSettings_001";
    static bool IConfigFile.PerUser => true;
    static bool IConfigFile.AlwaysSave => false;

    [ConfigName("Enable notifications", "#NotificationSettings_EnableNotifications")]
    [ConfigDescription("Allows notifications to be sent", "#NotificationSettings_EnableNotificationsDesc")]
    public ELanguage Language { get; set; } = ELanguage.None;
}