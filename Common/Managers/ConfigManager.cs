using Common.Autofac;
using OpenSteamworks;

namespace Common.Managers;

public class ConfigManager : IHasStartupTasks
{
    private SteamClient steamClient;
    public ConfigManager(SteamClient client) {
        steamClient = client;
    }
    public void RunStartup()
    {
        Console.WriteLine("ConfigManager startup");
    }
}