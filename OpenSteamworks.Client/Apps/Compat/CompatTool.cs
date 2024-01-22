using OpenSteamworks.Client.Utils;
using OpenSteamworks.Utils;
using OpenSteamworks.KeyValues;

namespace OpenSteamworks.Client.Apps.Compat;

public class ToolManifest : TypedKVObject {
    public int Version {
        get {
            return DefaultIfUnset("version", 2);
        }
    }
    
    public string Commandline {
        get {
            return DefaultIfUnset("commandline", "");
        }
    }

    public AppId_t RequiredAppID {
        get {
            return DefaultIfUnset("require_tool_appid", (uint)0);
        }
    }

    public string CompatManagerLayerName {
        get {
            return DefaultIfUnset("compatmanager_layer_name", "");
        }
    }

    public ToolManifest(KVObject kv) : base(kv) { }
}

public class CompatTool {
    public enum CompatToolOS {
        Windows,
        Linux
    }

    public AppId_t AppID { get; internal set; } = 0;
    public string InstallDir { get; init; }
    private List<CompatToolOS> fromoslist = new();
    public ReadOnlyCollectionEx<CompatToolOS> FromOSList {
        get {
            return new ReadOnlyCollectionEx<CompatToolOS>(fromoslist);
        }
    }

    private List<CompatToolOS> tooslist = new();
    public ReadOnlyCollectionEx<CompatToolOS> ToOSList {
        get {
            return new ReadOnlyCollectionEx<CompatToolOS>(tooslist);
        }
    }

    public ToolManifest ToolManifest { get; init; }

    private CompatTool(string pathToTool) {
        InstallDir = pathToTool;
        string toolmanifestpath = Path.Combine(pathToTool, "toolmanifest.vdf");
        string compatmanifestpath = Path.Combine(pathToTool, "compatibilitytool.vdf");
        if (!File.Exists(toolmanifestpath)) {
            throw new Exception("Tool manifest doesn't exist at " + toolmanifestpath);
        }

        ToolManifest = new ToolManifest(KVTextDeserializer.Deserialize(File.ReadAllText(toolmanifestpath)));
        if (ToolManifest.Version != 2) {
            throw new InvalidOperationException("Version " + ToolManifest.Version + " compat tools are unsupported");
        }
    }

    public static CompatTool CreateFromApp(SteamApp app) {
        if (!app.TryGetInstallDir(out string? installDir)) {
            throw new InvalidOperationException("App not installed.");
        }

        var compatTool = new CompatTool(installDir);
        compatTool.AppID = app.AppID;
        compatTool.fromoslist.Add(CompatToolOS.Windows);
        return compatTool;
    }
}