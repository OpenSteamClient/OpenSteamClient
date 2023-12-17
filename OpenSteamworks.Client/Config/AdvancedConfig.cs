using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;

namespace OpenSteamworks.Client.Config;

public class AdvancedConfig : IConfigFile
{
    static string IConfigFile.ConfigName => "AdvancedConfig_001";
    static bool IConfigFile.PerUser => false;
    static bool IConfigFile.AlwaysSave => false;

    [ConfigName("Connection Type", "#AdvancedConfig_EnabledConnectionTypes")]
    [ConfigDescription("Sets the ways to connect to Steam. If ExistingClient is specified, you can run ValveSteam in the background and OpenSteamClient will connect to it.", "#AdvancedConfig_EnabledConnectionTypesDesc")]
    [ConfigCategory("Misc", "#AdvancedConfig_Category_Misc")]
    public OpenSteamworks.SteamClient.ConnectionType EnabledConnectionTypes { get; set; } = OpenSteamworks.SteamClient.ConnectionType.ExistingClient | OpenSteamworks.SteamClient.ConnectionType.NewClient;

    [ConfigName("Steam Service", "#AdvancedConfig_EnableSteamService")]
    [ConfigDescription("If the Steam Client Service should be enabled. Disabling this may cause stuff to break, so you should keep it on.", "#AdvancedConfig_EnableSteamServiceDesc")]
    [ConfigCategory("Misc", "#AdvancedConfig_Category_Misc")]
    public bool EnableSteamService { get; set; } = true;

    [ConfigName("Force steamserviced (Windows)", "#AdvancedConfig_ServiceAsAdminHostOnWindows")]
    [ConfigDescription("If the Steam Client Service should run through steamserviced instead of as an installed service. Will cause an admin prompt every time OpenSteamClient is started.", "#AdvancedConfig_ServiceAsAdminHostOnWindowsDesc")]
    [ConfigCategory("Misc", "#AdvancedConfig_Category_Misc")]
    public bool ServiceAsAdminHostOnWindows { get; set; } = false;

    [ConfigName("Spew logs", "#AdvancedConfig_SteamClientSpew")]
    [ConfigDescription("Sets all logging channels to spew. Use this when reporting issues!", "#AdvancedConfig_SteamClientSpewDesc")]
    [ConfigCategory("Logging", "#AdvancedConfig_Category_Logging")]
    public bool SteamClientSpew { get; set; } = true;

    [ConfigName("Log callbacks", "#AdvancedConfig_LogIncomingCallbacks")]
    [ConfigDescription("Logs all incoming callbacks (ID's, names, sizes, processing delays) but not their data.", "#AdvancedConfig_LogIncomingCallbacksDesc")]
    [ConfigCategory("Logging", "#AdvancedConfig_Category_Logging")]
    public bool LogIncomingCallbacks { get; set; } = true;

    [ConfigName("Log callback data", "#AdvancedConfig_LogCallbackContents")]
    [ConfigDescription("Logs all incoming callback's data. Will leak private info like login tokens. Do not share or show logs if you have this setting enabled!", "#AdvancedConfig_LogCallbackContentsDesc")]
    [ConfigCategory("Logging", "#AdvancedConfig_Category_Logging")]
    public bool LogCallbackContents { get; set; } = false;
}