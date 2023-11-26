using OpenSteamworks.Client.Utils;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataConfigSection : KVObjectEx
{
    public class LaunchOption : KVObjectEx {

        public LaunchOption(KVObject kv) : base(kv) { }
    }
    public bool CheckForUpdatesBeforeLaunch => DefaultIfUnset("checkforupdatesbeforelaunch", false);
    public IEnumerable<LaunchOption> LaunchOptions => EmptyListIfUnset("launch", (kv) => new LaunchOption(kv));
    public AppDataConfigSection(KVObject kv) : base(kv) { }
}