using System.Text.Json;

namespace Common.Config;

public abstract class ConfigFile<T> where T : ConfigFile<T>, new() { 
    private IConfigSerializer? serializer;
    private IConfigIO? io;
    public abstract T GetThis();
    public static T LoadWith(IConfigSerializer serializer, IConfigIO io) {
        var deserialized = serializer.Deserialize<T>(io.Load());
        deserialized.io = io;
        deserialized.serializer = serializer;
        return deserialized;
    }
    public static T LoadWithOrCreate(IConfigSerializer serializer, IConfigIO io) {
        try {
            return LoadWith(serializer, io);
        } catch (Exception) {}
        var created = new T
        {
            io = io,
            serializer = serializer
        };
        return created;
    }
    public void SaveWith(IConfigSerializer serializer, IConfigIO io) {
        io.Save(serializer.Serialize<T>(GetThis()));
    }
    public void Save() {
        Common.Utils.UtilityFunctions.AssertNotNull(this.io);
        Common.Utils.UtilityFunctions.AssertNotNull(this.serializer);
        
        this.SaveWith(this.serializer, this.io);
    }
}