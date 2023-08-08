using Common.Autofac;
using OpenSteamworks;

namespace Common.Managers;

public class AppsManager : IHasStartupTasks
{
    public required SteamClient steamClient { protected get; init; }
    public required ConfigManager configManager { protected get; init; }
    public AppsManager() {
        
    }
    public void RunStartup()
    {
        Console.WriteLine("AppsManager startup");
        //steamClient.LogClientState();
    }
}