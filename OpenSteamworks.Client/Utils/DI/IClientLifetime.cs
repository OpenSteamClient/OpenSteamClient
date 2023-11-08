namespace OpenSteamworks.Client.Utils.DI;

public interface IClientLifetime {
    public Task RunStartup();
    public Task RunShutdown();
}

