using Common.Autofac;
using Common.Config;
using Common.Config.IO;
using Common.Config.Serializers;
using Common.Utils;
using OpenSteamworks;

namespace Common.Managers;

public class ConfigManager : IHasStartupTasks
{
    private SteamClient? steamClient;
    public AdvancedConfig AdvancedConfig { get; set; }
    internal BootstrapperState BootstrapperState { get; set; }

    public string InstallDir {
        get {
            //TODO: use registry to get installdir on Windows when we build an installer.
            // On all platforms, just use LocalApplicationData, which maps:
            // Linux: .local/share/OpenSteam
            // Windows: C:\Users\USERNAME\AppData\Local
            // Mac: /Users/USERNAME/.local/share
            var localShare = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localShare, "OpenSteam");
        }
    }

    public string ConfigDir {
        get {
            if (OperatingSystem.IsLinux()) {
                return UtilityFunctions.GetXDGSpecPath("XDG_CONFIG_HOME", ".config", "OpenSteam/config");
            } else {
                return Path.Combine(InstallDir, "config");
            }
        }
    }

    public string LogsDir {
        get {
            if (OperatingSystem.IsLinux()) {
                return UtilityFunctions.GetXDGSpecPath("XDG_STATE_HOME", ".local/state", "OpenSteam/logs");
            } else {
                return Path.Combine(InstallDir, "logs");
            }
        }
    }
    public string CacheDir {
        get {
            if (OperatingSystem.IsLinux()) {
                return UtilityFunctions.GetXDGSpecPath("XDG_CACHE_HOME", ".cache", "OpenSteam");
            } else {
                return Path.Combine(InstallDir, "cache");
            }
        }
    }
    private string AdvancedConfigPath => Path.Combine(ConfigDir, "AdvancedConfig.json");
    private string BootstrapperStatePath => Path.Combine(ConfigDir, "BootstrapperState.json");
    private readonly ConfigSerializerJSON jsonSerializer = new();

    private static ConfigIOFile CreateSimpleConfigIOFile(string path) {
        return new ConfigIOFile() {
            SavePath = path
        };
    }
    public ConfigManager() {
        Directory.CreateDirectory(InstallDir);
        Directory.CreateDirectory(ConfigDir);
        Directory.CreateDirectory(LogsDir);
        Directory.CreateDirectory(CacheDir);
        
        this.AdvancedConfig = AdvancedConfig.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(AdvancedConfigPath));
        this.BootstrapperState = BootstrapperState.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(BootstrapperStatePath));
    }
    public void FlushToDisk() {
        this.AdvancedConfig.Save();
        this.BootstrapperState.Save();
        steamClient?.ClientConfigStore.FlushToDisk();
    }
    public void SetClient(SteamClient client) {
        steamClient = client;
    }
    public void RunStartup()
    {
        Console.WriteLine("ConfigManager startup");
    }
}