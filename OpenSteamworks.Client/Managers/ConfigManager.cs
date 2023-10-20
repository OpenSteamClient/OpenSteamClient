using System.Reflection;
using System.Runtime.Versioning;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Config.IO;
using OpenSteamworks.Client.Config.Serializers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.OSSpecific;
using OpenSteamworks;
using OpenSteamworks.Client.Utils.Interfaces;

namespace OpenSteamworks.Client.Managers;

public class ConfigManager : IClientLifetime
{
    private SteamClient? steamClient;

    public string InstallDir { get; private set; }

    public string ConfigDir { get; private set; }

    [SupportedOSPlatform("linux")]
    public string DatalinkDir { get; private set; } = "";

    public string HomeDir { get; private set; }

    public string LogsDir { get; private set; }
    public string CacheDir { get; private set; }
    public string AssemblyDirectory { get; private set; }
    private readonly ConfigSerializerJSON jsonSerializer = new();

    private static ConfigIOFile CreateSimpleConfigIOFile(string path) {
        return new ConfigIOFile() {
            SavePath = path
        };
    }
    public void SetClient(SteamClient client) {
        steamClient = client;
    }

    public ConfigManager(Container container) {
        AssemblyDirectory = OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        //TODO: use registry to get installdir on Windows when we build an installer.
        // On all platforms, just use LocalApplicationData, which maps:
        // Linux: .local/share/OpenSteam
        // Windows: C:\Users\USERNAME\AppData\Local
        // Mac: /Users/USERNAME/.local/share
        var localShare = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        InstallDir = Path.Combine(localShare, "OpenSteam");

        HomeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsLinux()) {
            DatalinkDir = Path.Combine(HomeDir, ".steam");
            CacheDir = LinuxSpecifics.GetXDGSpecPath("XDG_CACHE_HOME", ".cache", "OpenSteam");
            LogsDir = LinuxSpecifics.GetXDGSpecPath("XDG_STATE_HOME", ".local/state", "OpenSteam/logs");
            ConfigDir = LinuxSpecifics.GetXDGSpecPath("XDG_CONFIG_HOME", ".config", "OpenSteam/config");
            Directory.CreateDirectory(DatalinkDir);
        } else {
            CacheDir = Path.Combine(InstallDir, "cache");
            LogsDir = Path.Combine(InstallDir, "logs");
            ConfigDir = Path.Combine(InstallDir, "config");
        }

        Directory.CreateDirectory(InstallDir);
        Directory.CreateDirectory(ConfigDir);
        Directory.CreateDirectory(LogsDir);
        Directory.CreateDirectory(CacheDir);

        container.RegisterInstance(AdvancedConfig.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(ConfigDir, "AdvancedConfig.json"))));
        container.RegisterInstance(BootstrapperState.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(ConfigDir, "BootstrapperState.json"))));
        container.RegisterInstance(GlobalSettings.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(ConfigDir, "GlobalSettings.json"))));
        container.RegisterInstance(LoginUsers.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(ConfigDir, "LoginUsers.json"))));
    }

    public async Task RunStartup()
    {
        await Task.CompletedTask;
    }

    public async Task RunShutdown()
    {
        await Task.CompletedTask;
    }
}