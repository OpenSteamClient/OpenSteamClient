using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

public class ShortcutApp : AppBase {
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

    
    private string shortcutName = "";
    protected override string ActualName => shortcutName;
    protected override string ActualHeroURL => this.UserSetApp?.HeroURL ?? "";
    protected override string ActualLogoURL => this.UserSetApp?.LogoURL ?? "";
    protected override string ActualIconURL => this.UserSetApp?.IconURL ?? "";
    protected override string ActualPortraitURL => this.UserSetApp?.PortraitURL ?? "";
    
    /// <summary>
    /// We allow the user to set a custom appid for non-steam games. This will be used in the library to provide proton compat data and art for the game, as well as activity feeds. <br/>
    /// This will NOT allow the user to earn achievements or post activity statuses for the game however.
    /// </summary>
    public AppId_t UserSetAppID { get; set; } = 0;

    /// <summary>
    /// We allow the user to set a custom app type. If unset, defaults to Application and not Game.
    /// </summary>
    public EAppType UserSetAppType { get; set; } = EAppType.Application;

    public AppBase? UserSetApp {
        get {
            if (UserSetAppID == 0) {
                return null;
            }

            return GetAppIfValidGameID(new CGameID(UserSetAppID));
        }
    }

    public override IEnumerable<LaunchOption> LaunchOptions => new List<LaunchOption>() { new(0, "", "") };
    public override int? DefaultLaunchOptionID => 0;
    public override EAppType Type => UserSetAppType;
    public override uint LibraryAssetChangeNumber => 0;

    public override EAppState State => EAppState.FullyInstalled;

    internal ShortcutApp(AppsManager appsManager, string name, string exe, string workingDir) : base(appsManager) {
        this.GameID = new CGameID(Path.Combine(workingDir, exe), name);
        this.shortcutName = name;
    }

    public override async Task<EAppUpdateError> Launch(string userLaunchOptions, int launchOption)
    {
        return EAppUpdateError.NoError;
    }

    public override void PauseUpdate()
    {
        
    }

    public override void Update()
    {
        
    }
}