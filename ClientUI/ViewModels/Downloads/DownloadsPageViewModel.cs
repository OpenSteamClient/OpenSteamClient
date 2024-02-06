using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Downloads;

namespace ClientUI.ViewModels.Downloads;

public partial class DownloadsPageViewModel : ViewModelBase {
    public ObservableCollection<DownloadItemViewModel> DownloadQueue { get; init; } = new();

    [ObservableProperty]
    private DownloadItemViewModel? currentDownload;

    private readonly DownloadManager downloadManager;
    public DownloadsPageViewModel(DownloadManager downloadManager) {
        this.downloadManager = downloadManager;
        downloadManager.CurrentDownloadChanged += OnCurrentDownloadChanged;
        downloadManager.DownloadRateChanged += OnDownloadRateChanged;
        downloadManager.DownloadQueueChanged += OnDownloadQueueChanged;
        UpdateDownloadQueue();
    }

    private void OnDownloadQueueChanged(object? sender, EventArgs e)
    {
        UpdateDownloadQueue();
    }

    private void UpdateDownloadQueue() {
        //TODO: This is terrible.
        var newQueue = this.downloadManager.DownloadQueue;
        this.DownloadQueue.Clear();
        Console.WriteLine("Queue len " + newQueue.Count());
        foreach (var newitem in newQueue)
        {
            Console.WriteLine("queue: " + newitem);
            this.DownloadQueue.Add(new DownloadItemViewModel(newitem));
        }
    }

    private void OnDownloadRateChanged(object? sender, EventArgs e)
    {
        
    }

    private void OnCurrentDownloadChanged(object? sender, EventArgs e)
    {
        if (downloadManager.CurrentDownload == null) {
            CurrentDownload = null;
        } else {
            CurrentDownload = new DownloadItemViewModel(downloadManager.CurrentDownload);
        }
    }
}