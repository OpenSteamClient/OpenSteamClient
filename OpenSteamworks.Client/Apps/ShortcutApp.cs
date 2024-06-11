using System.Diagnostics;
using OpenSteamworks.Enums;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

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

    public void SetName(string newName) {
        SteamClient.GetIClientShortcuts().SetShortcutAppName(this.ShortcutAppID, newName);
    }

    protected override string ActualName => ShortcutInfo.AppName;
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
    public override uint StoreAssetsLastModified => 0;

    public override bool IsOwnedAndPlayable => true;
    public override EAppState State => EAppState.FullyInstalled;
    public override ILibraryAssetAlignment? LibraryAssetAlignment => null;
    public CMsgShortcutInfo ShortcutInfo {
        get {
            using (var hack = ProtobufHack.Create<CMsgShortcutInfo>())
            {
                if (SteamClient.GetIClientShortcuts().GetShortcutInfoByAppID(this.ShortcutAppID, hack.ptr)) {
                    return hack.GetManaged();
                }
            }

            return new CMsgShortcutInfo();
        }
    }

    public AppId_t ShortcutAppID { get; init; }

    internal ShortcutApp(AppId_t appidShortcut)
    {
        this.ShortcutAppID = appidShortcut;
        this.GameID = CGameID.Zero;
// #if !_WINDOWS
//         this.GameID = SteamClient.GetInstance().IPCClientShortcuts.GetGameIDForAppID(appidShortcut);
// #else
//         this.GameID = CGameID.Zero;
// #endif
    }

    public override async Task<EAppError> Launch(string userLaunchOptions, int launchOption, ELaunchSource launchSource)
    {
        await Task.CompletedTask;
        return SteamClient.GetIClientShortcuts().LaunchShortcut(this.ShortcutAppID, launchOption);
    }

    public override void PauseUpdate()
    {
        
    }

    public override void Update()
    {
        
    }
}