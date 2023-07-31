using Common.Autofac;
using OpenSteamworks;

namespace Common.Managers;

public class LoginManager : IHasStartupTasks
{
    public required SteamClient steamClient { protected get; init; }
    public required ConfigManager configManager { protected get; init; }
    public LoginManager() {
        
    }
    public void RunStartup()
    {
        steamClient.LogClientState();
    }
    
    public bool ShouldPromptForLogin() {
        return true;
    }
}