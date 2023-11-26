using OpenSteamworks.Client.Utils;
using OpenSteamworks.Structs;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps;

public class SourcemodGameInfo : KVObjectEx {
    public string Name => DefaultIfUnset("game", "");

    public string IconRelativePath => DefaultIfUnset("icon", "");

    public bool SupportsVR => DefaultIfUnset("supportsvr", false);

    public string Type => DefaultIfUnset("type", "");

    public AppId_t SteamAppID => DefaultIfUnset("FileSystem/SteamAppId", (uint)0);

    public SourcemodGameInfo(KVObject kv) : base(kv) { }
}

public class SourcemodApp : AppBase {
    private static readonly KVSerializer kvserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    public SourcemodGameInfo SourcemodGameInfo { get; private set; }
    public AppBase? ParentApp => GetAppIfValidAppID(this.SourcemodGameInfo.SteamAppID);
    public override string Name {
        get {
            if (!string.IsNullOrEmpty(NameOverride)) {
                return NameOverride;
            }
            
            return this.SourcemodGameInfo.Name;
        }
    }

    public override string HeroURL => GetValueOverride(HeroOverrideURL, this.ParentApp?.HeroURL);
    public override string LogoURL => GetValueOverride(LogoOverrideURL, this.ParentApp?.LogoURL);
    public override string IconURL => GetValueOverride(IconOverrideURL, this.ParentApp?.IconURL);
    public override string PortraitURL => GetValueOverride(PortraitOverrideURL, this.ParentApp?.PortraitURL);

    internal SourcemodApp(string sourcemodDir, uint modid) {
        SourcemodGameInfo = new SourcemodGameInfo(SourcemodApp.kvserializer.Deserialize(File.OpenRead(Path.Combine(sourcemodDir, "gameinfo.txt"))));
        this.GameID = new CGameID(SourcemodGameInfo.SteamAppID, modid);
    }

    public override async Task Launch() {
        await Task.CompletedTask;
    }
}