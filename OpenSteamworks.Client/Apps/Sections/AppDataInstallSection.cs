using OpenSteamworks.Client.Utils;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataInstallSection : TypedKVObject
{
    public AppDataInstallSection(KVObject kv) : base(kv) { }
}