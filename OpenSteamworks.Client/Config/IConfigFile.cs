namespace OpenSteamworks.Client.Config;

public interface IConfigFile {
    public abstract static string ConfigName { get; }
    public abstract static bool PerUser { get; }
    public abstract static bool AlwaysSave { get; }
}