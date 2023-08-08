namespace Common.Config;

internal class BootstrapperState : ConfigFile<BootstrapperState>
{
    public override BootstrapperState GetThis() => this;
    public uint NativeBuildDate { get; set; }
    public uint InstalledVersion { get; set; }
    public string CommitHash { get; set; }
    public bool SkipVerification { get; set; }
    public Dictionary<string, long> InstalledFiles { get; set; }
    public string LinuxRuntimeChecksum { get; set; }
    public bool LinuxPermissionsSet { get; set; }
    public BootstrapperState() {
        NativeBuildDate = 0;
        InstalledVersion = 0;
        CommitHash = "";
        SkipVerification = false;
        InstalledFiles = new Dictionary<string, long>();
        LinuxRuntimeChecksum = "";
        LinuxPermissionsSet = false;
    }
}