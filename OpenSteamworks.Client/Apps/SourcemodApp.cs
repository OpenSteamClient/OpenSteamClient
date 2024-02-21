using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

public class SourcemodGameInfo : TypedKVObject {
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

    public SourcemodGameInfo SourcemodGameInfo { get; private set; }
    public AppBase? ParentApp => GetAppIfValidGameID(new CGameID(this.SourcemodGameInfo.SteamAppID));
    protected override string ActualName => this.SourcemodGameInfo.Name;
    protected override string ActualHeroURL => this.ParentApp?.HeroURL ?? "";
    protected override string ActualLogoURL => this.ParentApp?.LogoURL ?? "";
    protected override string ActualIconURL => this.ParentApp?.IconURL ?? "";
    protected override string ActualPortraitURL => this.ParentApp?.PortraitURL ?? "";
    public override EAppType Type => EAppType.Game;

    public override IEnumerable<LaunchOption> LaunchOptions => new List<LaunchOption>() { new(0, "Play " + this.Name, "") };
    public override int? DefaultLaunchOptionID => 0;

    public override uint StoreAssetsLastModified => 0;

    public override bool IsOwnedAndPlayable => true;
    private EAppState state;
    public override EAppState State => state;
    public override ILibraryAssetAlignment? LibraryAssetAlignment => ParentApp?.LibraryAssetAlignment;

    internal SourcemodApp(string sourcemodDir, uint modid) {
        SourcemodGameInfo = new SourcemodGameInfo(KVTextDeserializer.Deserialize(File.ReadAllText(Path.Combine(sourcemodDir, "gameinfo.txt"))));
        this.GameID = new CGameID(SourcemodGameInfo.SteamAppID, modid);
        state = EAppState.FullyInstalled;
    }

    public override async Task<EAppUpdateError> Launch(string userLaunchOptions, int launchOptionID)
    {
        //TODO: implement (or possibly scrap) this
        await Task.CompletedTask;
        return EAppUpdateError.NoError;
    }

    public override void PauseUpdate()
    {
        
    }

    public override void Update()
    {
        
    }
}