namespace OpenSteamworks.Client.Config;

public interface IConfigSerializer {
    T Deserialize<T>(byte[] data);
    byte[] Serialize<T>(T instance);
}