using OpenSteamworks.Client.Utils;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataExtendedSection : TypedKVObject
{
    public string Developer => DefaultIfUnset("developer", "");
    public string GameDir => DefaultIfUnset("gamedir", "");
    public string Homepage => DefaultIfUnset("homepage", "");
    public string ServerBrowserName => DefaultIfUnset("serverbrowsername", "");
    public string State => DefaultIfUnset("state", "");
    public bool VisibleOnlyWhenInstalled => DefaultIfUnset("VisibleOnlyWhenInstalled", false);

    /// <summary>
    /// This seems to be duplicated between the common section's "associations".
    /// Which one should we use?
    /// </summary>
    public string Publisher => DefaultIfUnset("publisher", "");
    public string Aliases => DefaultIfUnset("aliases", "");
    public IEnumerable<string> ListOfDLC => DefaultIfUnset("listofdlc", "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).AsEnumerable();
    public AppDataExtendedSection(KVObject kv) : base(kv) { }
}