namespace OpenSteamworks.Client.Config.Serializers;

public class ConfigSerializerJSON : IConfigSerializer
{
    public bool PrettyPrint { get; init; } = true;
    public T Deserialize<T>(byte[] data)
    {
        string text = System.Text.Encoding.Default.GetString(data);
        T? deserialized = System.Text.Json.JsonSerializer.Deserialize<T>(text);
        if (deserialized == null) {
            throw new NullReferenceException("Failed to deserialize JSON file");
        }

        return deserialized;
    }

    public byte[] Serialize<T>(T instance)
    {
        return System.Text.Encoding.Default.GetBytes(System.Text.Json.JsonSerializer.Serialize<T>(instance, new System.Text.Json.JsonSerializerOptions {
            WriteIndented = PrettyPrint
        }));
    }
}