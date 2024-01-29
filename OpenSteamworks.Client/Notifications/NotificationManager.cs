using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Generated;

namespace OpenSteamworks.Client.Notifications;

public class NotificationManager
{
    private readonly IClientUser user;
    private readonly UserSettings userSettings;

    public INotificationUI? NotificationUI { get; set; }

    public NotificationManager(ISteamClient client, UserSettings userSettings) {
        this.user = client.IClientUser;
        this.userSettings = userSettings;
    }
}