using Common.Autofac;
using OpenSteamworks;

namespace Common.Managers;

public class LoginManager : IHasStartupTasks
{
    private SteamClient steamClient;
    public LoginManager(SteamClient client) {
        steamClient = client;
    }
    public void RunStartup()
    {
        steamClient.LogClientState();
    }
    
    public bool ShouldPromptForLogin() {
        return true;
    }
}