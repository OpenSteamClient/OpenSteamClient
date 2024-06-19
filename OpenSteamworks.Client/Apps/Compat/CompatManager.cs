using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Apps.Compat;

public class CompatManager : ILogonLifetime {
    public const int STEAMPLAY_MANIFESTS = 891390;

    private readonly ISteamClient steamClient;
    private readonly IClientCompat clientCompat;
    private readonly Dictionary<string, ERemoteStoragePlatform> compatToolPlatforms = new();
    private readonly Dictionary<string, string> compatToolFriendlyNames = new();
    private readonly Logger logger;
    private readonly AppsManager appsManager;

    public IEnumerable<string> AllCompatTools => compatToolPlatforms.Keys;
    public IEnumerable<KeyValuePair<string, ERemoteStoragePlatform>> CompatToolPlatforms => compatToolPlatforms;

    public bool IsCompatEnabled {
        get => clientCompat.BIsCompatLayerEnabled();
        set => clientCompat.EnableCompat(value);
    }

    public CompatManager(ISteamClient steamClient, AppsManager appsManager, InstallManager installManager, CallbackManager callbackManager) {
        this.appsManager = appsManager;
        this.steamClient = steamClient;
        this.logger = Logger.GetLogger("CompatManager", installManager.GetLogPath("CompatManager"));
        this.clientCompat = steamClient.IClientCompat;
        RefreshCompatTools();
    }

    private unsafe void LoadAppCompatToolPreferences()
    {
        // I'm not sure why we have to do this ourselves. steamclient.so seems to already have code for doing this, but I'm not sure how it gets activated.
        
        if (steamClient.ConnectedWith == ConnectionType.ExistingClient) {
            return;
        }
        
        logger.Info("Loading compat tool preferences for apps");
        CUtlVector<AppWhitelistSetting_t> whitelistN = new();
        clientCompat.GetWhitelistedGameList(&whitelistN);

        var whitelist = whitelistN.ToManagedAndFree();
        var steamPlayManifests = (appsManager.GetApp(STEAMPLAY_MANIFESTS) as SteamApp)?.Extended.UnderlyingObject;
        foreach (var item in whitelist)
        {
            var gameid = new CGameID(item.AppID);
            if (IsCompatEnabledForApp(item.AppID)) {
                continue;
            }

            string toolToUse;
            string config = string.Empty;
            int priority = 85;
            if (item.unk == 0) {
                // Use the default app for that platform (proton for windows, slr for linux)
                string os = appsManager.GetCurrentEffectiveOSForApp(gameid.AppID);
                if (os == "linux") {
                    toolToUse = "steamlinuxruntime";
                } else {
                    toolToUse = GetDefaultWindowsCompatTool();
                }
            } else if (item.unk == 8) {
                priority = 100;

                if (steamPlayManifests == null) {
                    logger.Error("No SteamPlay manifests but we need them!");
                    continue;
                }

                if (!steamPlayManifests.TryGetChild("app_mappings", out KVObject? app_mappings)) {
                    logger.Error("No SteamPlay app_mappings but we need them!");
                    continue;
                }

                if (!app_mappings.TryGetChild(item.AppID.ToString(), out KVObject? mapping)) {
                    logger.Error("No SteamPlay app_mappings entry for app " + item.AppID);
                    continue;
                }

                // Use the tool as specified in SteamPlay 2.0 Manifests
                toolToUse = mapping["tool"].GetValueAsString();

                // And also load the config
                if (steamPlayManifests.TryGetChild("app_compat_configs", out KVObject? app_compat_configs)) {
                    if (app_compat_configs.TryGetChild(item.AppID.ToString(), out KVObject? compat_config)) {
                        //TODO: There's also an environment field. Unsure if that is automatically used
                        if (compat_config.TryGetChild("config", out KVObject? configKV)) {
                            config = configKV.GetValueAsString();
                        }
                    } else {
                        logger.Error("No SteamPlay app_compat_configs entry for app " + item.AppID);
                    }
                } else {
                    logger.Warning("No SteamPlay app_compat_configs!");
                }  
            } else {
                logger.Error("Unknown compat whitelist mode " + item.unk + " for app " + item.AppID);
                continue;
            }

            clientCompat.SpecifyCompatTool(item.AppID, toolToUse, config, priority);
        }
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

        logger.Info($"Refresh: Default compat tool is '{clientCompat.GetCompatToolName(0)}'");
    }

    private unsafe void RefreshCompatToolsForPlatform(ERemoteStoragePlatform platform) {
        if (steamClient.ConnectedWith == ConnectionType.ExistingClient) {
            //TODO: look at ValveSteam's non-steam compat tools in this case
            logger.Warning("Connected to existing client, returning placeholder RefreshCompatToolsForPlatform");
            if (platform == ERemoteStoragePlatform.PlatformWindows) {
                // Feel free to open PRs if you want other compat tools in this list
                foreach (var item in new string[] { "proton_experimental" })
                {
                    compatToolPlatforms[item] = platform;
                }
            } else if (platform == ERemoteStoragePlatform.PlatformWindows) {
                foreach (var item in new string[] { "steamlinuxruntime", "steamlinuxruntime_sniper", "steamlinuxruntime_scout" })
                {
                    compatToolPlatforms[item] = platform;
                }
            }
            
            return;
        }
        
        CUtlStringList compatToolsTarget = new(4096);
        clientCompat.GetAvailableCompatToolsFiltered(&compatToolsTarget, platform);
        foreach (var item in compatToolsTarget.ToManagedAndFree())
        {
            compatToolPlatforms[item] = platform;
        }
    }

    public bool IsCompatEnabledForApp(AppId_t appid) => clientCompat.BIsCompatibilityToolEnabled(appid);
    public bool IsCompatEnabledForNonWhitelisted() => clientCompat.BIsCompatibilityToolEnabled(0);

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

    public void SpecifyCompatTool(AppId_t appid, string compatToolID, string config, int priority)
        => clientCompat.SpecifyCompatTool(appid, compatToolID, config, priority);

    public unsafe IEnumerable<string> GetCompatToolsForApp(CGameID gameid) {
        if (steamClient.ConnectedWith == ConnectionType.ExistingClient) {
            logger.Warning("Connected to existing client, returning self calculated GetCompatToolsForApp");
            var list = new List<string>();
            var app = appsManager.GetApp(gameid);
            if (app is SteamApp sapp) {
                if (sapp.Common.OSList.Contains("windows")) {
                    list.AddRange(compatToolPlatforms.Where(p => p.Value == ERemoteStoragePlatform.PlatformWindows).Select(p => p.Key));
                }

                if (sapp.Common.OSList.Contains("macos")) {
                    list.AddRange(compatToolPlatforms.Where(p => p.Value == ERemoteStoragePlatform.PlatformOSX).Select(p => p.Key));
                }

                if (sapp.Common.OSList.Contains("linux")) {
                    list.AddRange(compatToolPlatforms.Where(p => p.Value == ERemoteStoragePlatform.PlatformLinux).Select(p => p.Key));
                }
            } else {
                logger.Warning("Unsupported app type for app " + gameid);
            } 

            return list.AsEnumerable();
        }

        CUtlStringList compatToolsForApp = new(4096);
        clientCompat.GetAvailableCompatToolsForApp(&compatToolsForApp, gameid.AppID);
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
        await Task.Run(() => LoadAppCompatToolPreferences());
    }

    public async Task OnLoggingOff(IProgress<string> progress)
    {
        this.compatToolPlatforms.Clear();
        this.compatToolFriendlyNames.Clear();
        await Task.CompletedTask;
    }
}