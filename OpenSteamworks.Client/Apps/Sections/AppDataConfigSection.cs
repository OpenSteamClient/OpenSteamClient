using OpenSteamworks.Client.Utils;
using OpenSteamworks.KeyValues;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataConfigSection : TypedKVObject
{
    public class LaunchOption : TypedKVObject {

        public LaunchOption(KVObject kv) : base(kv) { }
    }
    public bool CheckForUpdatesBeforeLaunch => DefaultIfUnset("checkforupdatesbeforelaunch", false);
    public IEnumerable<LaunchOption> LaunchOptions => EmptyListIfUnset("launch", (kv) => new LaunchOption(kv));
    public AppDataConfigSection(KVObject kv) : base(kv) { }
}