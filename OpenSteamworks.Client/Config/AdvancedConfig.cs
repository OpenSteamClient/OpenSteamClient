namespace OpenSteamworks.Client.Config;

public class AdvancedConfig : ConfigFile<AdvancedConfig>
{
    public override AdvancedConfig GetThis() => this;
    public OpenSteamworks.SteamClient.ConnectionType EnabledConnectionTypes { get; set; } = OpenSteamworks.SteamClient.ConnectionType.ExistingClient | OpenSteamworks.SteamClient.ConnectionType.NewClient;
    public bool EnableSteamService { get; set; } = true;
    public bool ServiceAsAdminHostOnWindows { get; set; } = false;
    public bool SteamClientSpew { get; set; } = true;
    public AdvancedConfig() {}
}