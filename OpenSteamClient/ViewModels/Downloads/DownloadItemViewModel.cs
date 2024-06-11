using System;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Downloads;
using OpenSteamworks.Structs;

namespace OpenSteamClient.ViewModels.Downloads;

public partial class DownloadItemViewModel : AvaloniaCommon.ViewModelBase {
    public string Name => AvaloniaApp.Container.Get<ClientApps>().GetAppName(AppID);

    [ObservableProperty]
    private AppId_t appID;

    [ObservableProperty]
    private double currentDownloadProgress;

    [ObservableProperty]
    private string downloadSize = string.Empty;

    [ObservableProperty]
    private string diskSize = string.Empty;

    [ObservableProperty]
    private DateTime? downloadStarted;

    [ObservableProperty]
    private DateTime? downloadFinished;

    private readonly DownloadManager downloadManager;
    public DownloadItemViewModel(DownloadManager downloadManager, AppId_t appid) {
        this.downloadManager = downloadManager;
        this.downloadManager.DownloadStatsChanged += OnDownloadStatsChanged;
        AppID = appid;
        UpdateDownloadInfo();
    }

    private void OnDownloadStatsChanged(object? sender, DownloadStats e)
    {
        UpdateDownloadInfo();
    }

    private void UpdateDownloadInfo() {
        var appManager = SteamClient.GetIClientAppManager();
        if (appManager.GetUpdateInfo(this.AppID, out AppUpdateInfo_s updateInfo)) {
            if (updateInfo.m_unBytesToProcess != 0) {
                this.CurrentDownloadProgress = updateInfo.m_unBytesProcessed / updateInfo.m_unBytesToProcess;
                Console.WriteLine("prog: " + this.CurrentDownloadProgress);
            } else if (updateInfo.m_unBytesToDownload != 0) {
                this.CurrentDownloadProgress = updateInfo.m_unBytesDownloaded / updateInfo.m_unBytesToDownload;
                Console.WriteLine("progd: " + this.CurrentDownloadProgress);
            }
            
            this.DownloadSize = DataUnitStrings.GetStringForSize(updateInfo.m_unBytesToDownload, DataSizeUnit.Auto_GB_MB_KB_B);
            this.DiskSize = DataUnitStrings.GetStringForSize(updateInfo.m_unBytesToProcess, DataSizeUnit.Auto_GB_MB_KB_B);
        }

        this.DownloadStarted = downloadManager.GetDownloadStartTime(AppID);
        this.DownloadFinished = downloadManager.GetDownloadFinishTime(AppID);
    }
}