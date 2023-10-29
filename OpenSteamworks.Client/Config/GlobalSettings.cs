using OpenSteamworks.Enums;

namespace OpenSteamworks.Client.Config;

public class GlobalSettings : ConfigFile<GlobalSettings>
{
    public override GlobalSettings GetThis() => this;
    public ELanguage Language { get; set; } = ELanguage.English;
    public bool EnableWebHelper { get; set; } = false;
    public bool WebhelperGPUAcceleration { get; set; } = true;
    public bool WebhelperSmoothScrolling { get; set; } = true;
    public bool WebhelperGPUVideoDecode { get; set; } = true;
    public bool WebhelperHighDPI { get; set; } = true;
    public string WebhelperProxy { get; set; } = "";
    public bool WebhelperIgnoreProxyForLocalhost { get; set; } = true;
    /// <summary>
    /// We don't actually know what this does. Doesn't seem to be a vanilla CEF thing.
    /// </summary>
    public int WebhelperComposerMode { get; set; } = 0;
    public bool WebhelperIgnoreGPUBlocklist { get; set; } = true;
    public bool WebhelperAllowWorkarounds { get; set; } = true;

    public GlobalSettings() {}
}