using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Downloads;

namespace OpenSteamClient.ViewModels.Downloads;

public partial class DownloadsPageViewModel : AvaloniaCommon.ViewModelBase {
    private readonly object downloadsLock = new();
    public ObservableCollection<DownloadItemViewModel> DownloadQueue { get; init; } = new();
    public ObservableCollection<DownloadItemViewModel> ScheduledDownloads { get; init; } = new();
    public ObservableCollection<DownloadItemViewModel> UnscheduledDownloads { get; init; } = new();
    

    [ObservableProperty]
    private DownloadItemViewModel? currentDownload;

    [ObservableProperty]
    private ulong peakDownloadRateNum;

    [ObservableProperty]
    private ulong peakDiskRateNum;

    [ObservableProperty]
    private string currentDownloadRate;

    [ObservableProperty]
    private string currentDiskRate;

    [ObservableProperty]
    private string peakDownloadRate;

    [ObservableProperty]
    private string peakDiskRate;

    private readonly DownloadManager downloadManager;
    private readonly UserSettings userSettings;
    public DownloadsPageViewModel(DownloadManager downloadManager, UserSettings userSettings) {
        this.userSettings = userSettings;
        this.downloadManager = downloadManager;
        downloadManager.DownloadStatsChanged += OnDownloadStatsChanged;
        downloadManager.DownloadsChanged += OnDownloadQueueChanged;
        UpdateDownloadQueue();
        UpdateRates(new());
    }

    private void OnDownloadStatsChanged(object? sender, DownloadStats e)
    {
        UpdateRates(e);
    }

    private void OnDownloadQueueChanged(object? sender, EventArgs e)
    {
        UpdateDownloadQueue();
    }

    private void UpdateDownloadQueue() {
        AvaloniaApp.Current?.RunOnUIThread(DispatcherPriority.Normal, UpdateDownloadQueueInternal);
    }

    private void UpdateDownloadQueueInternal() {
        lock (downloadsLock)
        {
            // Update download queue
            var queue = this.downloadManager.DownloadQueue;
            this.DownloadQueue.Clear();
            Console.WriteLine("Queue len " + queue.Count());
            foreach (var newitem in queue)
            {
                Console.WriteLine("queue: " + newitem);
                this.DownloadQueue.Add(new DownloadItemViewModel(downloadManager, newitem));
            }
    
            // Update scheduled downloads
            var scheduled = this.downloadManager.ScheduledDownloads;
            this.ScheduledDownloads.Clear();
            Console.WriteLine("scheduled len " + scheduled.Count());
            foreach (var newitem in scheduled)
            {
                Console.WriteLine("scheduled: " + newitem);
                this.ScheduledDownloads.Add(new DownloadItemViewModel(downloadManager, newitem));
            }
    
            // Update unscheduled downloads
            var unscheduled = this.downloadManager.UnscheduledDownloads;
            this.UnscheduledDownloads.Clear();
            Console.WriteLine("unscheduled len " + unscheduled.Count());
            foreach (var newitem in unscheduled)
            {
                Console.WriteLine("unscheduled: " + newitem);
                this.UnscheduledDownloads.Add(new DownloadItemViewModel(downloadManager, newitem));
            }
    
            if (downloadManager.CurrentDownload != 0) {
                this.CurrentDownload = new DownloadItemViewModel(downloadManager, downloadManager.CurrentDownload);
            } else {
                this.CurrentDownload = null;
            } 
        }
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(currentDownloadRate))]
    [MemberNotNull(nameof(currentDiskRate))]
    [MemberNotNull(nameof(peakDownloadRate))]
    [MemberNotNull(nameof(peakDiskRate))]
#pragma warning restore MVVMTK0034
    private void UpdateRates(DownloadStats downloadStats) {
        if (downloadStats.DownloadRateBytes > PeakDownloadRateNum) {
            PeakDownloadRateNum = downloadStats.DownloadRateBytes;
        }

        if (downloadStats.DiskRateBytes > PeakDiskRateNum) {
            PeakDiskRateNum = downloadStats.DiskRateBytes;
        }

        CurrentDownloadRate = DataUnitStrings.GetStringForDownloadSpeed(downloadStats.DownloadRateBytes, userSettings.DownloadDataRateUnit);
        CurrentDiskRate = DataUnitStrings.GetStringForDownloadSpeed(downloadStats.DiskRateBytes, userSettings.DownloadDataRateUnit);
        PeakDownloadRate = DataUnitStrings.GetStringForDownloadSpeed(PeakDownloadRateNum, userSettings.DownloadDataRateUnit);
        PeakDiskRate = DataUnitStrings.GetStringForDownloadSpeed(PeakDiskRateNum, userSettings.DownloadDataRateUnit);
    }
}