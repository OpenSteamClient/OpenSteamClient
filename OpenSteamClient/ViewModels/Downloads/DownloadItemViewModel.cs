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

        if (downloadManager.BIsAppUpToDate(AppID)) {
            // If there's no update, deregister to allow for this object to be GCd
            this.downloadManager.DownloadStatsChanged -= OnDownloadStatsChanged;
            return;
        }

        if (appManager.GetUpdateInfo(this.AppID, out AppUpdateInfo_s updateInfo)) {
            //Console.WriteLine($"{AppID}: " + updateInfo.ToString());
            if (updateInfo.m_unBytesToProcess != 0 && updateInfo.m_unBytesToProcess != updateInfo.m_unBytesProcessed) {
                this.CurrentDownloadProgress = (double)updateInfo.m_unBytesProcessed / (double)updateInfo.m_unBytesToProcess;
                Console.WriteLine($"{AppID} prog: " + this.CurrentDownloadProgress + $"({updateInfo.m_unBytesProcessed} / {updateInfo.m_unBytesToProcess})");
            } else if (updateInfo.m_unBytesToDownload != 0 && updateInfo.m_unBytesToDownload != updateInfo.m_unBytesDownloaded) {
                this.CurrentDownloadProgress = (double)updateInfo.m_unBytesDownloaded / (double)updateInfo.m_unBytesToDownload;
                Console.WriteLine($"{AppID} progd: " + this.CurrentDownloadProgress);
            }
            
            this.DownloadSize = DataUnitStrings.GetStringForSize(updateInfo.m_unBytesToDownload, DataSizeUnit.Auto_GB_MB_KB_B);
            this.DiskSize = DataUnitStrings.GetStringForSize(updateInfo.m_unBytesToProcess, DataSizeUnit.Auto_GB_MB_KB_B);
        } else {
            // If there's no update info, deregister to allow for this object to be GCd
            this.downloadManager.DownloadStatsChanged -= OnDownloadStatsChanged;
            return;
        }

        this.DownloadStarted = downloadManager.GetDownloadStartTime(AppID);
        this.DownloadFinished = downloadManager.GetDownloadFinishTime(AppID);
    }
}