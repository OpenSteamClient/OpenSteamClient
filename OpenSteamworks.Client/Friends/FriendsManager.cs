using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Generated;

namespace OpenSteamworks.Client.Friends;

public class FriendsManager : ILogonLifetime
{
    private readonly IClientUser user;
    private readonly UserSettings userSettings;

    public FriendsManager(ISteamClient client, UserSettings userSettings) {
        this.user = client.IClientUser;
        this.userSettings = userSettings;
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        if (userSettings.LoginToFriendsNetworkAutomatically) {
            this.user.SetSelfAsChatDestination(true);
        }
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress)
    {
        
    }
}