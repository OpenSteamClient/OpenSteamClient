namespace OpenSteamworks.Client.Apps;

//TODO: switch from concrete class to interface(s)
//TODO: (long term goal) add support for plugins to allow auto importing shortcuts from other stores
public interface IApp {
    public AppIdentifier ID { get; }
    public string Name { get; }
}