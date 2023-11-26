using OpenSteamworks.Client.Utils;
using OpenSteamworks.Structs;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataCommonSection : KVObjectEx
{
    public class LibraryAssetsT : KVObjectEx {
        public string CapsuleLanguages => DefaultIfUnset("library_capsule", "");
        public string HeroLanguages => DefaultIfUnset("library_hero", "");
        public string LogoLanguages => DefaultIfUnset("library_logo", "");

        /// <summary>
        /// Width percentage of the logo overlay relative to the full size of the banner
        /// </summary>
        public float LogoWidthPercentage => DefaultIfUnset("logo_position/width_pct", 0.0F);

        /// <summary>
        /// Height percentage of the logo overlay relative to the full size of the banner
        /// </summary>
        public float LogoHeightPercentage => DefaultIfUnset("logo_position/height_pct", 0.0F);
        public string LogoPinnedPosition => DefaultIfUnset("logo_position/pinned_position", "BottomLeft");

        public LibraryAssetsT(KVObject kv) : base(kv) { }
    }

    public string Name => DefaultIfUnset("name", "");
    public string Type => DefaultIfUnset("type", "");
    public IEnumerable<string> OSList => DefaultIfUnset("oslist", "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).AsEnumerable();
    public string OSArch => DefaultIfUnset("osarch", "");
    public CGameID GameID => new(DefaultIfUnset("gameid", (ulong)0));
    public string ReleaseState => DefaultIfUnset("ReleaseState", "");
    public string ControllerSupport => DefaultIfUnset("controller_support", "");
    public bool ExcludeFromGameLibrarySharing => DefaultIfUnset("exfgls", false);
    public LibraryAssetsT? LibraryAssets => DefaultIfUnset("library_assets", (kv) => new LibraryAssetsT(kv), null);
    public string StoreAssetModificationTime => DefaultIfUnset("store_asset_mtime", "0");
    public AppId_t ParentAppID => DefaultIfUnset("parent", (uint)0);
    public string Icon => DefaultIfUnset("icon", "");

    public AppDataCommonSection(KVObject kv) : base(kv) { }
}