using OpenSteamworks.Client.Enums;

namespace OpenSteamworks.Client.Utils;

public static class DataUnitStrings {
    public static string GetStringForDownloadSpeed(ulong speedInBytesPerSecond, DataRateUnit unit) {
        switch (unit)
        {
            case DataRateUnit.Auto_GB_MB_KB_B:
                if (speedInBytesPerSecond < 1000) {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.B);
                } else if (speedInBytesPerSecond < 1000000) {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.KB);
                } else if (speedInBytesPerSecond < 1000000000) {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.MB);
                } else {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.GB);
                }
            case DataRateUnit.Auto_Gbps_Mbps_Kbps_bits:
                if (speedInBytesPerSecond < 125) {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.bits);
                } else if (speedInBytesPerSecond < 125000) {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.Kbps);
                } else if (speedInBytesPerSecond < 125000000) {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.Mbps);
                } else {
                    return GetStringForDownloadSpeed(speedInBytesPerSecond, DataRateUnit.Gbps);
                }
            
            case DataRateUnit.GB:
                return $"{speedInBytesPerSecond / 1024 / 1024 / 1024 } GB/s";

            case DataRateUnit.MB:
                return $"{speedInBytesPerSecond / 1024 / 1024 } MB/s";

            case DataRateUnit.KB:
                return $"{speedInBytesPerSecond / 1024 } KB/s";

            case DataRateUnit.B:
                return $"{speedInBytesPerSecond} B/s";


            case DataRateUnit.Gbps:
                return $"{(speedInBytesPerSecond * 8) / 1000 / 1000 / 1000 } Gbps";

            case DataRateUnit.Mbps:
                return $"{(speedInBytesPerSecond * 8) / 1000 / 1000 } Mbps";
            
            case DataRateUnit.Kbps:
                return $"{(speedInBytesPerSecond * 8) / 1000 } Kbps";

            case DataRateUnit.bits:
                return $"{speedInBytesPerSecond * 8 }bit/s";

            default:
                throw new ArgumentOutOfRangeException(nameof(speedInBytesPerSecond));
        }
    }

    public static string GetStringForSize(ulong sizeInBytes, DataSizeUnit unit)
    {
        switch (unit)
        {
            case DataSizeUnit.Auto_GB_MB_KB_B:
                if (sizeInBytes < 1000) {
                    return GetStringForSize(sizeInBytes, DataSizeUnit.B);
                } else if (sizeInBytes < 1000000) {
                    return GetStringForSize(sizeInBytes, DataSizeUnit.KB);
                } else if (sizeInBytes < 1000000000) {
                    return GetStringForSize(sizeInBytes, DataSizeUnit.MB);
                } else {
                    return GetStringForSize(sizeInBytes, DataSizeUnit.GB);
                }
            
            case DataSizeUnit.GB:
                return $"{sizeInBytes / 1024 / 1024 / 1024 } GB";

            case DataSizeUnit.MB:
                return $"{sizeInBytes / 1024 / 1024 } MB";

            case DataSizeUnit.KB:
                return $"{sizeInBytes / 1024 } KB";

            case DataSizeUnit.B:
                return $"{sizeInBytes} B";

            default:
                throw new ArgumentOutOfRangeException(nameof(sizeInBytes));
        }
    }
}