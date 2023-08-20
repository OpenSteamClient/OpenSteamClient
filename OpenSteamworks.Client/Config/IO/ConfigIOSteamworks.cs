using OpenSteamworks;

namespace OpenSteamworks.Client.Config.IO;

public class ConfigIOSteamworks : IConfigIO
{
    public required string Key { get; init; }
    public required OpenSteamworks.Enums.EConfigStore ConfigStore { get; init; }
    public required SteamClient Client { get; init; }
    public byte[] Load()
    {
        byte[]? data = Client.ClientConfigStore.GetBinary(ConfigStore, Key);
        if (data == null) {
            throw new NullReferenceException($"Failed to get data from store {ConfigStore} with key {Key}");
        }

        return data;
    }

    public void Save(byte[] data)
    {
        Client.ClientConfigStore.SetBinary(ConfigStore, Key, data);
    }
}