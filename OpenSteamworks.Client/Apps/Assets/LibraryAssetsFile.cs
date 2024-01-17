using OpenSteamworks.Client.Utils;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps.Assets;

public class LibraryAssetsFile : KVObjectEx
{
    public class LibraryAsset : KVObjectEx
    {
        public LibraryAsset(KVObject kv) : base(kv) { }
        public string LastChangeNumber => DefaultIfUnset("change", "0");
        public string Version => DefaultIfUnset("v", "3");
        public string IconHash => DefaultIfUnset("icon", "");
        public string PortraitLastModified => DefaultIfUnset("0x", "");
        public uint PortraitExpires => DefaultIfUnset("0", 0u);
        public string HeroLastModified => DefaultIfUnset("1x", "");
        public uint HeroExpires => DefaultIfUnset("1", 0u);
        public string LogoLastModified => DefaultIfUnset("2x", "");
        public uint LogoExpires => DefaultIfUnset("2", 0u);
        public string HeaderLastModified => DefaultIfUnset("3x", "");
        public uint HeaderExpires => DefaultIfUnset("3", 0u);
    }

    public LibraryAssetsFile(KVObject kv) : base(kv) { }
    public IDictionary<uint, LibraryAsset> Assets => EmptyDictionaryIfUnsetUInt("0", kv => new LibraryAsset(kv));
}