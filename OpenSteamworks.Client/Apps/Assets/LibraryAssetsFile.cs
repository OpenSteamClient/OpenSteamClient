using OpenSteamworks.Client.Utils;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps.Assets;

public class LibraryAssetsFile : KVObjectEx
{
    public class LibraryAsset : KVObjectEx
    {
        public LibraryAsset(KVObject kv) : base(kv) { }
        public uint LastChangeNumber {
            get => DefaultIfUnset("change", 0u);
            set => SetValue("change", value);
        }
        public string Version {
            get => DefaultIfUnset("v", "3");
            set => SetValue("v", value);
        }
        public string IconHash {
            get => DefaultIfUnset("icon", "");
            set => SetValue("icon", value);
        }
        public string PortraitLastModified {
            get => DefaultIfUnset("0x", "");
            set => SetValue("0x", value);
        }
        public uint PortraitExpires {
            get => DefaultIfUnset("0", 0u);
            set => SetValue("0", value);
        }
        public string HeroLastModified {
            get => DefaultIfUnset("1x", "");
            set => SetValue("1x", value);
        }
        public uint HeroExpires {
            get => DefaultIfUnset("1", 0u);
            set => SetValue("1", value);
        }
        public string LogoLastModified {
            get => DefaultIfUnset("2x", "");
            set => SetValue("2x", value);
        }
        public uint LogoExpires {
            get => DefaultIfUnset("2", 0u);
            set => SetValue("2", value);
        }
        public string HeaderLastModified {
            get => DefaultIfUnset("3x", "");
            set => SetValue("3x", value);
        }
        public uint HeaderExpires {
            get => DefaultIfUnset("3", 0u);
            set => SetValue("3", value);
        }

        internal void SetLastModified(string lastModified, AppsManager.ELibraryAssetType assetType)
        {
            switch (assetType)
            {
                case AppsManager.ELibraryAssetType.Icon:
                    break;

                case AppsManager.ELibraryAssetType.Logo:
                    LogoLastModified = lastModified;
                    break;
                case AppsManager.ELibraryAssetType.Hero:
                    HeroLastModified = lastModified;
                    break;
                case AppsManager.ELibraryAssetType.Portrait:
                    PortraitLastModified = lastModified;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(assetType));
            }
        }
    }

    public LibraryAssetsFile(KVObject kv) : base(kv) { }
    public ProxyDictionary<LibraryAsset> Assets => EmptyDictionaryIfUnset("0", kv => new LibraryAsset(kv));
}