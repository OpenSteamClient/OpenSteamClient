using System.Globalization;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.KeyValues;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataCommonSection : TypedKVObject
{
    public class LibraryAssetsT : TypedKVObject, AppBase.ILibraryAssetAlignment {
        public string CapsuleLanguages => DefaultIfUnset("library_capsule", "");
        public string HeroLanguages => DefaultIfUnset("library_hero", "");
        public string LogoLanguages => DefaultIfUnset("library_logo", "");

        /// <summary>
        /// Width percentage of the logo overlay relative to the full size of the hero
        /// </summary>
        public float LogoWidthPercentage => float.Parse(DefaultIfUnset("logo_position/width_pct", "50"), CultureInfo.InvariantCulture.NumberFormat);

        /// <summary>
        /// Height percentage of the logo overlay relative to the full size of the hero
        /// </summary>
        public float LogoHeightPercentage => float.Parse(DefaultIfUnset("logo_position/height_pct", "100"), CultureInfo.InvariantCulture.NumberFormat);
        public string LogoPinnedPosition => DefaultIfUnset("logo_position/pinned_position", "BottomLeft");

        public LibraryAssetsT(KVObject kv) : base(kv) { }
    }

    public string Name => DefaultIfUnset("name", "");
    public string Type => DefaultIfUnset("type", "");
    public IEnumerable<string> OSList => DefaultIfUnset("oslist", "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).AsEnumerable();
    public string OSArch => DefaultIfUnset("osarch", "");
    public CGameID GameID => new(ulong.Parse(DefaultIfUnset("gameid", "0")));
    public string ReleaseState => DefaultIfUnset("ReleaseState", "");
    public string ControllerSupport => DefaultIfUnset("controller_support", "");
    public bool ExcludeFromGameLibrarySharing => DefaultIfUnset("exfgls", false);
    public LibraryAssetsT? LibraryAssets => DefaultIfUnset("library_assets", (kv) => new LibraryAssetsT(kv), null);
    public string StoreAssetModificationTime => DefaultIfUnset("store_asset_mtime", "0");
    // Well why tf is this an int and not an uint
    public AppId_t ParentAppID => (uint)DefaultIfUnset("parent", (int)0);
    public string Icon => DefaultIfUnset("icon", "");

    public AppDataCommonSection(KVObject kv) : base(kv) { }
}