namespace OpenSteamworks.Downloads;

public struct DownloadStats {
    public ulong DownloadRateBytes { get; set; } = 0;
    public ulong DiskRateBytes { get; set; } = 0;

    public DownloadStats()
    {
    }
}