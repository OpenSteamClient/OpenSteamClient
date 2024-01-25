using OpenSteamworks.Client.Utils;
using OpenSteamworks.KeyValues;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataConfigSection : TypedKVObject
{
    public class LaunchOption : TypedKVObject, AppBase.ILaunchOption {
        public class KVConfig : TypedKVObject {
            public string OSList => DefaultIfUnset("oslist", "");
            public string OSArch => DefaultIfUnset("osarch", "");
            public string Realm => DefaultIfUnset("realm", "steamglobal");
            public string BetaKey => DefaultIfUnset("BetaKey", "");
            public string OwnsDLC => DefaultIfUnset("ownsdlc", "");
            
            public KVConfig(KVObject kv) : base(kv) { }
        }

        public string Executable => DefaultIfUnset("executable", "");
        public string Arguments => DefaultIfUnset("arguments", "");
        public string Type => DefaultIfUnset("type", "none");
        public string Description => DefaultIfUnset("description", "");
        public KVConfig? Config => DefaultIfUnset("config", kv => new KVConfig(kv), null);

        public int ID => int.Parse(kv.Name);
        string AppBase.ILaunchOption.Name => "";
        string AppBase.ILaunchOption.Description => Description;

        //TODO: handle description_loc (which doesn't really seem to be used by any games)
        public LaunchOption(KVObject kv) : base(kv) { }
    }
    public bool CheckForUpdatesBeforeLaunch => DefaultIfUnset("checkforupdatesbeforelaunch", false);
    public IEnumerable<LaunchOption> LaunchOptions => EmptyListIfUnset("launch", (kv) => new LaunchOption(kv));
    public AppDataConfigSection(KVObject kv) : base(kv) { }
}