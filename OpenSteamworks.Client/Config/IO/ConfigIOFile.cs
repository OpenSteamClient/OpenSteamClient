namespace OpenSteamworks.Client.Config.IO;

public class ConfigIOFile : IConfigIO
{
    public required string SavePath { get; init; }
    public byte[] Load()
    {
        return File.ReadAllBytes(SavePath);
    }

    public void Save(byte[] data)
    {
        File.WriteAllBytes(SavePath, data);
    }
}