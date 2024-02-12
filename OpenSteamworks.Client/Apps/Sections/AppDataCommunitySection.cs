using OpenSteamworks.Client.Utils;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataCommunitySection : TypedKVObject
{
    public AppDataCommunitySection(KVObject kv) : base(kv) { }
}