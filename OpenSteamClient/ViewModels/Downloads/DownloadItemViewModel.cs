using System;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Downloads;

namespace OpenSteamClient.ViewModels.Downloads;

public partial class DownloadItemViewModel : AvaloniaCommon.ViewModelBase {
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private AppId_t appID;

    [ObservableProperty]
    private string currentDownloadRate;
    [ObservableProperty]
    private string currentDiskRate;

    [ObservableProperty]
    private string peakDownloadRate;

    [ObservableProperty]
    private string peakDiskRate;

    [ObservableProperty]
    private double currentDownloadProgress;

    [ObservableProperty]
    private bool downloadProgressKnown;

    private ulong peakDownloadRateNum;
    private ulong peakDiskRateNum;

    private UserSettings userSettings => AvaloniaApp.Container.Get<UserSettings>();
    private readonly DownloadItem downloadItem;
    public DownloadItemViewModel(DownloadItem downloadItem) {
        this.downloadItem = downloadItem;
        this.Name = downloadItem.Name;
        this.DownloadProgressKnown = true;
        this.CurrentDownloadProgress = 0.0;
        AppID = downloadItem.AppID;

        downloadItem.DownloadRateChanged += OnDownloadRateChanged;
        downloadItem.DownloadProgressChanged += OnDownloadProgressChanged;
        UpdateCurrentRates();
        UpdatePeakRates();
    }

    private void OnDownloadProgressChanged(object? sender, EventArgs e)
    {
        this.DownloadProgressKnown = downloadItem.DownloadProgressKnown;
        this.CurrentDownloadProgress = downloadItem.DownloadProgress;
    }

    private void OnDownloadRateChanged(object? sender, EventArgs e)
    {
        UpdateCurrentRates();
        UpdatePeakRates();
    }
#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(currentDownloadRate))]
    [MemberNotNull(nameof(currentDiskRate))]
#pragma warning restore MVVMTK0034
    private void UpdateCurrentRates() {
        CurrentDownloadRate = GetStringForDownloadSpeed(downloadItem.DownloadRate, userSettings.DownloadDataRateUnit);
        CurrentDiskRate = GetStringForDownloadSpeed(downloadItem.DiskRate, userSettings.DownloadDataRateUnit);
    }
#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(peakDownloadRate))]
    [MemberNotNull(nameof(peakDiskRate))]
#pragma warning restore MVVMTK0034
    private void UpdatePeakRates() {
        if (downloadItem.DownloadRate > peakDownloadRateNum) {
            peakDownloadRateNum = downloadItem.DownloadRate;
        }

        if (downloadItem.DiskRate > peakDiskRateNum) {
            peakDiskRateNum = downloadItem.DiskRate;
        }

        PeakDownloadRate = GetStringForDownloadSpeed(peakDownloadRateNum, userSettings.DownloadDataRateUnit);
        PeakDiskRate = GetStringForDownloadSpeed(peakDiskRateNum, userSettings.DownloadDataRateUnit);
    }

    private static string GetStringForDownloadSpeed(ulong speedInBytesPerSecond, DataRateUnit unit) {
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
}