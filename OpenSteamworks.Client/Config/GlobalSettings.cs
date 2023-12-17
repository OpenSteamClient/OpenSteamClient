using OpenSteamworks.Enums;
using OpenSteamworks.Client.Config.Attributes;

namespace OpenSteamworks.Client.Config;

public class GlobalSettings: IConfigFile {
    static string IConfigFile.ConfigName => "GlobalSettings_001";
    static bool IConfigFile.PerUser => false;
    static bool IConfigFile.AlwaysSave => false;
    
    [ConfigName("Enable Webhelper", "#GlobalSettings_EnableWebHelper")]
    [ConfigDescription("Enables/disables Webhelper. Required for some games and for browsing the store and community pages in-client.", "#GlobalSettings_EnableWebHelperDesc")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public bool EnableWebHelper { get; set; } = false;

    [ConfigName("Enable Webhelper GPU Acceleration", "#GlobalSettings_WebhelperGPUAcceleration")]
    [ConfigDescription("Enables/disables GPU hardware rendering in Webhelper.", "#GlobalSettings_WebhelperGPUAccelerationDesc")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public bool WebhelperGPUAcceleration { get; set; } = true;

    [ConfigName("Enable Webhelper smooth scrolling", "#GlobalSettings_WebhelperSmoothScrolling")]
    [ConfigDescription("Enables/disables smooth scrolling in Webhelper.", "#GlobalSettings_WebhelperSmoothScrollingDesc")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public bool WebhelperSmoothScrolling { get; set; } = true;

    [ConfigName("Enable Webhelper GPU video decoding", "#GlobalSettings_WebhelperGPUVideoDecode")]
    [ConfigDescription("Enables/disables GPU video decoding in Webhelper.", "#GlobalSettings_WebhelperGPUVideoDecodeDesc")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public bool WebhelperGPUVideoDecode { get; set; } = true;

    [ConfigName("Enable HighDPI support for Webhelper", "#GlobalSettings_WebhelperHighDPI")]
    [ConfigDescription("Enables/disables HighDPI support in Webhelper.", "#GlobalSettings_WebhelperHighDPIDesc")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public bool WebhelperHighDPI { get; set; } = true;

    [ConfigName("Webhelper proxy URL", "#GlobalSettings_WebhelperProxy")]
    [ConfigDescription("Sets a proxy for Webhelper to use. Does not affect connections outside Webhelper. Leave empty for no proxy.", "#GlobalSettings_WebhelperProxyDesc")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public string WebhelperProxy { get; set; } = "";

    [ConfigName("Webhelper ignore proxy for localhost", "#GlobalSettings_WebhelperIgnoreProxyForLocalhost")]
    [ConfigDescription("If the proxy should be ignored for localhost connections.", "#GlobalSettings_WebhelperIgnoreProxyForLocalhost")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    public bool WebhelperIgnoreProxyForLocalhost { get; set; } = true;
    
    [ConfigAdvanced]
    [ConfigDescription("We don't actually know what this does. Doesn't seem to be a vanilla CEF thing.", "")]
    public int WebhelperComposerMode { get; set; } = 0;

    [ConfigName("Webhelper ignore GPU blocklist", "#GlobalSettings_WebhelperIgnoreGPUBlocklist")]
    [ConfigDescription("If CEFs internal GPU blocklist should be disabled. ", "#GlobalSettings_WebhelperIgnoreGPUBlocklist")]
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    [ConfigAdvanced]
    public bool WebhelperIgnoreGPUBlocklist { get; set; } = true;
    
    [ConfigCategory("Webhelper", "#GlobalSettings_Category_Webhelper")]
    [ConfigAdvanced]
    public bool WebhelperAllowWorkarounds { get; set; } = true;
}