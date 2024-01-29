using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Apps.Compat;

public class CompatManager : ILogonLifetime {
    private readonly IClientCompat clientCompat;
    private readonly Dictionary<string, ERemoteStoragePlatform> compatToolPlatforms = new();
    private readonly Dictionary<string, string> compatToolFriendlyNames = new();
    private readonly Logger logger;

    public IEnumerable<string> AllCompatTools => compatToolPlatforms.Keys;
    public IEnumerable<KeyValuePair<string, ERemoteStoragePlatform>> CompatToolPlatforms => compatToolPlatforms;

    public bool IsCompatEnabled {
        get => clientCompat.BIsCompatLayerEnabled();
        set => clientCompat.EnableCompat(value);
    }

    public CompatManager(IClientCompat clientCompat, InstallManager installManager, CallbackManager callbackManager) {
        this.logger = Logger.GetLogger("CompatManager", installManager.GetLogPath("CompatManager"));
        this.clientCompat = clientCompat;
        RefreshCompatTools();
    }

    public unsafe void RefreshCompatTools() {
        //TODO: refresh compat tools in steamclient.so as well
        logger.Info("Refreshing all compat tools");
        compatToolPlatforms.Clear();
        compatToolFriendlyNames.Clear();
        RefreshCompatToolsForPlatform(ERemoteStoragePlatform.PlatformWindows);
        RefreshCompatToolsForPlatform(ERemoteStoragePlatform.PlatformLinux);

        // OSX has no known compat tools, but let's include it just in case
        RefreshCompatToolsForPlatform(ERemoteStoragePlatform.PlatformOSX);

        foreach (var item in AllCompatTools)
        {
            compatToolFriendlyNames[item] = clientCompat.GetCompatToolDisplayName(item);
        }

        foreach (var item in CompatToolPlatforms)
        {
            logger.Info($"Refresh: Got compat tool {item.Key} with target platform {item.Value}");
        }
    }

    private unsafe void RefreshCompatToolsForPlatform(ERemoteStoragePlatform platform) {
        CUtlStringList compatToolsTarget = new(4096);
        clientCompat.GetAvailableCompatToolsFiltered(&compatToolsTarget, platform);
        foreach (var item in compatToolsTarget.ToManagedAndFree())
        {
            compatToolPlatforms[item] = platform;
        }
    }

    public bool IsCompatEnabledForApp(AppId_t appid) => clientCompat.BIsCompatibilityToolEnabled(appid);

    public string GetCompatToolForApp(AppId_t appid) {
        return clientCompat.GetCompatToolName(appid);
    }

    public string GetFriendlyNameForCompatTool(string compatTool) {
        if (compatToolFriendlyNames.ContainsKey(compatTool)) {
            return compatToolFriendlyNames[compatTool];
        }

        return compatToolFriendlyNames[compatTool] = clientCompat.GetCompatToolDisplayName(compatTool);
    }

    public string GetDefaultWindowsCompatTool() {
        return GetCompatToolForApp(0);
    }

    public unsafe IEnumerable<string> GetCompatToolsForApp(AppId_t appid) {
        CUtlStringList compatToolsForApp = new(4096);
        clientCompat.GetAvailableCompatToolsForApp(&compatToolsForApp, appid);
        return compatToolsForApp.ToManagedAndFree();
    }

    public string GetPlatformStringForCompatTool(string compatToolName) {
        var plat = GetPlatformForCompatTool(compatToolName);
        return plat switch
        {
            ERemoteStoragePlatform.PlatformWindows => "windows",
            ERemoteStoragePlatform.PlatformOSX => "macos",
            ERemoteStoragePlatform.PlatformLinux => "linux",
            _ => throw new ArgumentOutOfRangeException(nameof(compatToolName), $"Compat tool {compatToolName} is for platform {plat} which is unsupported"),
        };
    }

    public ERemoteStoragePlatform GetPlatformForCompatTool(string compatToolName) {
        foreach (var item in compatToolPlatforms)
        {
            if (item.Key == compatToolName) {
                return item.Value;
            }
        }

        //TODO: fucking hating this shit (this codepath should never be called, but some compat tools don't show up through GetAvailableCompatToolsFiltered -_-)
        if (compatToolName.StartsWith("proton")) {
            return ERemoteStoragePlatform.PlatformWindows;
        }

        if (compatToolName.StartsWith("steamlinuxruntime")) {
            return ERemoteStoragePlatform.PlatformLinux;
        }

        throw new ArgumentException($"Compat tool '{compatToolName}' not found and making best guess failed", nameof(compatToolName));
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        await Task.Run(() => RefreshCompatTools());
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress)
    {
        this.compatToolPlatforms.Clear();
        this.compatToolFriendlyNames.Clear();
    }
}