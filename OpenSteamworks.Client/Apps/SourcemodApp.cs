using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;
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
    public class LaunchOption : ILaunchOption
    {
        public int ID { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        
        public LaunchOption(int id, string name, string desc) {
            this.ID = id;
            this.Name = name;
            this.Description = desc;
        }
    }

    private static readonly KVSerializer kvserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    public SourcemodGameInfo SourcemodGameInfo { get; private set; }
    public AppBase? ParentApp => GetAppIfValidGameID(new CGameID(this.SourcemodGameInfo.SteamAppID));
    protected override string ActualName => this.SourcemodGameInfo.Name;
    protected override string ActualHeroURL => this.ParentApp?.HeroURL ?? "";
    protected override string ActualLogoURL => this.ParentApp?.LogoURL ?? "";
    protected override string ActualIconURL => this.ParentApp?.IconURL ?? "";
    protected override string ActualPortraitURL => this.ParentApp?.PortraitURL ?? "";

    public override IEnumerable<LaunchOption> LaunchOptions => new List<LaunchOption>() { new(0, "Play " + this.Name, "") };
    public override int? DefaultLaunchOptionID => 0;

    public override int ChangeNumber => 0;

    internal SourcemodApp(AppsManager appsManager, string sourcemodDir, uint modid) : base(appsManager) {
        SourcemodGameInfo = new SourcemodGameInfo(SourcemodApp.kvserializer.Deserialize(File.OpenRead(Path.Combine(sourcemodDir, "gameinfo.txt"))));
        this.GameID = new CGameID(SourcemodGameInfo.SteamAppID, modid);
    }

    public override Task<EAppUpdateError> Launch(string userLaunchOptions, int launchOptionID)
    {
        throw new NotImplementedException();
    }
}