using ValveKeyValue;

namespace Common.Config.Serializers;

public class ConfigSerializerKV : IConfigSerializer
{
    public required string RootObjectName { get; init; }
    public KVSerializationFormat Format { get; init; } = KVSerializationFormat.KeyValues1Text;
    public T Deserialize<T>(byte[] data)
    {
        KVSerializer serializer = KVSerializer.Create(Format);
        using (Stream stream = new MemoryStream(data))
        {
           return serializer.Deserialize<T>(stream);
        }
    }

    public byte[] Serialize<T>(T instance)
    {
        var serializer = KVSerializer.Create(Format);
        using (MemoryStream stream = new())
        {
            serializer.Serialize<T>(stream, instance, RootObjectName);
            return stream.ToArray();
        }
    }
}