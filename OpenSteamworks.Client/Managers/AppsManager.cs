
using OpenSteamworks;
using OpenSteamworks.Client.Utils.Interfaces;

namespace OpenSteamworks.Client.Managers;

public class Library {
    public List<Category> Categories = new();
    internal Library() {

    }
}

public class Category {
    //TODO: support dynamic collections (how?)
    public string Name;
    public List<LibraryApp> Apps = new();
    public Category(string name) {
        this.Name = name;
    }
}

public class LibraryApp {
    public string Name;
    public UInt32 AppID;
    public LibraryApp(string name, UInt32 appid) {
        this.Name = name;
        this.AppID = appid;
    }
}

public class AppsManager : Component
{
    private SteamClient steamClient;
    
    public AppsManager(SteamClient steamClient, IContainer container) : base(container) {
        this.steamClient = steamClient;
    }
    // public async Task<Library> GetLibrary() {

    // }
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