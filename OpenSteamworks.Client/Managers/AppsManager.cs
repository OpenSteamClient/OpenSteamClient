
using OpenSteamworks;
using OpenSteamworks.Client.Utils.Interfaces;

namespace OpenSteamworks.Client.Managers;

public class AppsManager : Component
{
    private SteamClient steamClient;
    
    public AppsManager(SteamClient steamClient, IContainer container) : base(container) {
        this.steamClient = steamClient;
    }
    public override async Task RunStartup()
    {
        Console.WriteLine("AppsManager startup");
        await EmptyAwaitable();
    }
    public override async Task RunShutdown()
    {
        Console.WriteLine("AppsManager shutdown");
        await EmptyAwaitable();
    }
}