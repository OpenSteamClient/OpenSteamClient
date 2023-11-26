using ValveKeyValue;

namespace OpenSteamworks.Client.Extensions;

public static class KVValueExtensions
{
    public static IEnumerable<KVValue> GetChildren(this KVValue val) {
        return (IEnumerable<KVValue>)val;
    }

    public static IEnumerable<string> GetChildrenAsString(this KVValue val) {
        List<string> asStr = new();
        foreach (var item in GetChildren(val))
        {
            asStr.Add((string)item);
        }

        return asStr.AsEnumerable();
    }

    public static IEnumerable<KVObject> GetChildrenAsKVObjects(this KVValue val) {
        return ((IEnumerable<KVObject>)val);
    }

    public static KVObject GetAsKVObject(this KVValue val) {
        return ((IEnumerable<KVObject>)val).First();
    }
}