using System.Reflection;
using System.Runtime.Versioning;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Config.IO;
using OpenSteamworks.Client.Config.Serializers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.OSSpecific;
using OpenSteamworks;
using OpenSteamworks.Client.Utils.DI;

namespace OpenSteamworks.Client.Managers;

public class ConfigManager : IClientLifetime
{
    private readonly ConfigSerializerJSON jsonSerializer = new();

    private static ConfigIOFile CreateSimpleConfigIOFile(string path) {
        return new ConfigIOFile() {
            SavePath = path
        };
    }

    public ConfigManager(Container container, InstallManager installManager) {
        container.RegisterInstance(AdvancedConfig.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(installManager.ConfigDir, "AdvancedConfig.json"))));
        container.RegisterInstance(BootstrapperState.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(installManager.ConfigDir, "BootstrapperState.json"))));
        container.RegisterInstance(GlobalSettings.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(installManager.ConfigDir, "GlobalSettings.json"))));
        container.RegisterInstance(LoginUsers.LoadWithOrCreate(jsonSerializer, CreateSimpleConfigIOFile(Path.Combine(installManager.ConfigDir, "LoginUsers.json"))));
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