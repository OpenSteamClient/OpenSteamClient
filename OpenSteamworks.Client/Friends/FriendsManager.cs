using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
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
    private readonly Container container;
    private IFriendsUI? FriendsUI => container.GetNullable<IFriendsUI>();

    public FriendsManager(Container container, ISteamClient client, UserSettings userSettings) {
        this.container = container;
        this.user = client.IClientUser;
        this.userSettings = userSettings;
        client.CallbackManager.RegisterHandler<OpenFriendsDialog_t>(OnOpenFriendsDialog);
        client.CallbackManager.RegisterHandler<OpenChatDialog_t>(OnOpenChatDialog);
    }

    private void OnOpenChatDialog(CallbackManager.CallbackHandler<OpenChatDialog_t> handler, OpenChatDialog_t t)
    {
        FriendsUI?.ShowChatUI(t.ChatID);
    }

    private void OnOpenFriendsDialog(CallbackManager.CallbackHandler<OpenFriendsDialog_t> handler, OpenFriendsDialog_t t)
    {
        FriendsUI?.ShowFriendsList();
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        if (userSettings.LoginToFriendsNetworkAutomatically) {
            this.user.SetSelfAsChatDestination(true);
        }

        await Task.CompletedTask;
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress)
    {
        await Task.CompletedTask;
    }
}