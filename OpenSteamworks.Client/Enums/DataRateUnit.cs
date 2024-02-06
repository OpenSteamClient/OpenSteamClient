namespace OpenSteamworks.Client.Enums;

public enum DataRateUnit
{
    /// <summary>
    /// Automatically choose from GB/s, MB/s, KB/s and B/s
    /// </summary>
    Auto_GB_MB_KB_B,

    /// <summary>
    /// Always use GB/s
    /// </summary>
    GB,

    /// <summary>
    /// Always use MB/s
    /// </summary>
    MB,

    /// <summary>
    /// Always use KB/s
    /// </summary>
    KB,

    /// <summary>
    /// Always use B/s (bytes per second)
    /// </summary>
    B,

    /// <summary>
    /// Automatically choose from Gbps, Mbps, Kbps and bits
    /// </summary>
    Auto_Gbps_Mbps_Kbps_bits,

    /// <summary>
    /// Always use Gbps
    /// </summary>
    Gbps,

    /// <summary>
    /// Always use Mbps
    /// </summary>
    Mbps,

    /// <summary>
    /// Always use Kbps
    /// </summary>
    Kbps,

    /// <summary>
    /// Always use bit/s (bits per second)
    /// </summary>
    bits
}