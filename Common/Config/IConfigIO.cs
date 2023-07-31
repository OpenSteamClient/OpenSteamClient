namespace Common.Config;

/// <summary>
/// An interface that allows you to save and load config data to be Serialized/Deserialized by IConfigSerializer's
/// </summary>
public interface IConfigIO {
    public void Save(byte[] data);
    public byte[] Load();
}