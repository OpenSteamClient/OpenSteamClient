using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;

namespace OpenSteamworks.Client.Config;

public class BootstrapperState : IConfigFile
{
    static string IConfigFile.ConfigName => "BootstrapperState_001";
    static bool IConfigFile.PerUser => false;
    static bool IConfigFile.AlwaysSave => false;

    [ConfigNeverVisible]
    public uint NativeBuildDate { get; set; } = 0;

    [ConfigNeverVisible]
    public uint InstalledVersion { get; set; } = 0;

    [ConfigNeverVisible]
    public string CommitHash { get; set; } = "";

    [ConfigName("Skip file verification", "#BootstrapperState_SkipVerification")]
    [ConfigDescription("Skips all file verification in the bootstrapper.", "#BootstrapperState_SkipVerificationDesc")]
    [ConfigCategory("Bootstrapper", "#BootstrapperState_Category_Bootstrapper")]
    [ConfigAdvanced]
    public bool SkipVerification { get; set; } = false;

    [ConfigNeverVisible]
    public Dictionary<string, long> InstalledFiles { get; set; } = new();

    [ConfigNeverVisible]
    public string LinuxRuntimeChecksum { get; set; } = "";

    [ConfigNeverVisible]
    public bool LinuxPermissionsSet { get; set; } = false;

    [ConfigNeverVisible]
    public bool LastConfigLinkSuccess { get; set; } = false;
}