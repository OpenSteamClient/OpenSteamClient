namespace Common.Config;

public class AdvancedConfig : ConfigFile<AdvancedConfig>
{
    public override AdvancedConfig GetThis() => this;
    public OpenSteamworks.SteamClient.ConnectionType EnabledConnectionTypes { get; set; } = OpenSteamworks.SteamClient.ConnectionType.ExistingClient | OpenSteamworks.SteamClient.ConnectionType.NewClient;
    public AdvancedConfig() {}
}