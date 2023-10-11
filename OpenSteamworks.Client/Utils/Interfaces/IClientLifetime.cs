namespace OpenSteamworks.Client.Utils.Interfaces;

public interface IClientLifetime {
    public Task RunStartup();
    public Task RunShutdown();
}

