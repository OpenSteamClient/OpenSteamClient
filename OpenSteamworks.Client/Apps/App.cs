using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Structs;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps;

public class AppDataSection {
    private KVObject kv;
    public AppDataSection(KVObject kv) {
        this.kv = kv;
    }

    public KVValue this[string key] {
        get {
            return kv[key];
        }
    }

    protected int DefaultIfUnset(string key, int def) {
        if (kv[key] == null) {
            return def;
        }

        return (int)this[key];
    }

    protected uint DefaultIfUnset(string key, uint def) {
        if (kv[key] == null) {
            return def;
        }

        return (uint)this[key];
    }

    protected long DefaultIfUnset(string key, long def) {
        if (kv[key] == null) {
            return def;
        }

        return (long)this[key];
    }

    protected ulong DefaultIfUnset(string key, ulong def) {
        if (kv[key] == null) {
            return def;
        }

        return (ulong)this[key];
    }

    protected string DefaultIfUnset(string key, string def) {
        if (kv[key] == null) {
            return def;
        }

        return (string)this[key];
    }

    protected bool DefaultIfUnset(string key, bool def) {
        if (kv[key] == null) {
            return def;
        }

        return (bool)this[key];
    }


}

public class AppDataConfigSection : AppDataSection
{
    public bool CheckForUpdatesBeforeLaunch { 
        get {
            return DefaultIfUnset("checkforupdatesbeforelaunch", false);
        }
    }

    public AppDataConfigSection(KVObject kv) : base(kv) { }
}

public class App {
    private static KVSerializer kvserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    public AppDataConfigSection config;
    public CGameID GameID { get; init; }
    public AppId_t AppID { 
        get {
            return this.GameID.AppID();
        }
    }

    public bool IsMod {
        get {
            return this.GameID.IsMod();
        }
    }

    public bool IsShortcut {
        get {
            return this.GameID.IsShortcut();
        }
    }

    public bool IsSteamApp {
        get {
            return this.GameID.IsSteamApp();
        }
    }

    private App(CGameID gameid, MemoryStream kvstream) {
        SetAppInfoSections(kvstream);
    }

    [MemberNotNull(nameof(config))]
    internal void SetAppInfoSections(MemoryStream kvstream) {
        //TODO: How to use GetMultipleAppDataSections?
        config = new AppDataConfigSection(kvserializer.Deserialize(kvstream));
    }
}