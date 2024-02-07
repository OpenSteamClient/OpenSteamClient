using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenSteamworks.Callbacks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Downloads;

public class DownloadManager
{
    private readonly object scheduledDownloadsLock = new();
    private readonly object downloadQueueLock = new();
    private Queue<DownloadItem> downloadQueue = new();
    private List<DownloadItem> scheduledDownloads = new();
    public IEnumerable<DownloadItem> DownloadQueue => downloadQueue.AsEnumerable();

    private readonly ISteamClient steamClient;
    public ulong TotalBytesDownloaded { get; private set; }
    public DownloadItem? CurrentDownload { get; private set; }

    public event EventHandler? DownloadQueueChanged;
    public event EventHandler? CurrentDownloadChanged;

    // Forwarded from DownloadItem
    public event EventHandler? DownloadRateChanged;
    public event EventHandler? DownloadStateChanged;
    public event EventHandler? DownloadProgressChanged;
    public event EventHandler? DownloadPaused;
    public event EventHandler? DownloadFinished;

    internal bool stopPoll = false;
    internal bool pollStopped = false;
    private Thread? downloadInfoUpdateThread;
    public DownloadManager(ISteamClient steamClient)
    {
        this.steamClient = steamClient;
        steamClient.CallbackManager.RegisterHandler<DownloadScheduleChanged_t>(OnDownloadScheduleChanged);
        downloadInfoUpdateThread = new Thread(DownloadInfoUpdate);
        downloadInfoUpdateThread.Start();
    }

    private void DownloadInfoUpdate() {
        while (!stopPoll)
        {
            if (this.CurrentDownload != null && (this.CurrentDownload.DownloadState == DownloadState.ExtendedRunning || this.CurrentDownload.DownloadState == DownloadState.Running))
            {
                this.CurrentDownload.UpdateRates();
            }

            System.Threading.Thread.Sleep(1000);
        }

        pollStopped = true;
        stopPoll = false;
    }

    private void UpdateDownloadQueue(IEnumerable<AppId_t>? extraApps = null) {
        List<AppId_t> appsToCheck = new(SteamClient.GetClientApps().GetInstalledApps());
        if (extraApps != null) {
            appsToCheck.AddRange(extraApps);
        }
        
        // Update the download queue
        lock (downloadQueueLock)
        {
            AbstractOrderedList<DownloadItem> queue = new();
            foreach (var appid in appsToCheck)
            {
                var idx = SteamClient.GetIClientAppManager().GetAppDownloadQueueIndex(appid);
                queue.Insert(idx, new DownloadItem(appid));
            }

            downloadQueue = new(queue);
        }

        // Update the scheduled downloads
        lock (scheduledDownloadsLock)
        {
            scheduledDownloads.Clear();
            foreach (var item in appsToCheck)
            {
                scheduledDownloads.Add(new DownloadItem(item));
            }
        }
    }

    private void OnDownloadScheduleChanged(CallbackManager.CallbackHandler<DownloadScheduleChanged_t> handler, DownloadScheduleChanged_t data)
    {
        lock (downloadQueueLock)
        {
            UpdateDownloadQueue(data.m_rgunAppSchedule[0..data.m_nTotalAppsScheduled]);
        }
    }

    public void DequeueDownload(DownloadItem download) {
        if (download.DownloadState == DownloadState.Running) {
            // Stop the download first
            PauseDownload();
        }

        lock (downloadQueueLock)
        {
            var asList = downloadQueue.ToList();
            asList.Remove(download);
            downloadQueue = new(asList);
            UpdateQueueToSteamClient();
            DownloadQueueChanged?.Invoke(this, EventArgs.Empty);
        }
    }


    private DownloadItem? TryGetItemByAppID(AppId_t appid) {
        lock (downloadQueueLock)
        {
            var asList = downloadQueue.ToList();
            foreach (var item in asList)
            {
                if (item is DownloadItem aitem) {
                    if (aitem.AppID == appid) {
                        return aitem;
                    }
                }
            }
        }

        return null;
    }
    
    private DownloadItem GetItemByAppID(AppId_t appid) {
        var item = TryGetItemByAppID(appid);
        if (item == null) {
            throw new InvalidOperationException($"AppID {appid} is not queued");
        }

        return item;
    }


    public void DequeueDownload(AppId_t appid) => DequeueDownload(GetItemByAppID(appid));
    public void ChangeDownloadIndex(AppId_t appid, EAppDownloadQueuePlacement newPlacement, int manualPlacement = -1) => ChangeDownloadIndex(GetItemByAppID(appid), newPlacement, manualPlacement);

    public void ChangeDownloadIndex(DownloadItem download, EAppDownloadQueuePlacement newPlacement, int manualPlacement = -1) {
        if (newPlacement == EAppDownloadQueuePlacement.PriorityManual && manualPlacement == -1) {
            throw new InvalidOperationException("PriorityManual specified but no manualPlacement was given.");
        }

        lock (downloadQueueLock)
        {
            var asList = downloadQueue.ToList();
            int currentIndexOfDownload = asList.IndexOf(download);
            if (currentIndexOfDownload == -1) {
                throw new InvalidOperationException("Cannot change download index of item that is not in the queue");
            }

            int newIndex = -1;

            switch (newPlacement)
            {
                case EAppDownloadQueuePlacement.PriorityPaused:
                case EAppDownloadQueuePlacement.PriorityAutoUpdate:
                case EAppDownloadQueuePlacement.PriorityNone:
                    newIndex = currentIndexOfDownload;
                    break;
                
                case EAppDownloadQueuePlacement.PriorityUserInitiated:
                case EAppDownloadQueuePlacement.PriorityFirst:
                    newIndex = 0;
                    break;
                
                case EAppDownloadQueuePlacement.PriorityUp:
                    if (currentIndexOfDownload > 0) {
                        newIndex = currentIndexOfDownload--;
                    } else {
                        newIndex = 0;
                    }

                    break;
                
                case EAppDownloadQueuePlacement.PriorityDown:
                    if (currentIndexOfDownload == asList.Count) {
                        newIndex = currentIndexOfDownload;
                    } else {
                        newIndex = currentIndexOfDownload--;
                    }

                    break;
                
                case EAppDownloadQueuePlacement.PriorityManual:
                    newIndex = manualPlacement;
                    break;
            }

            if (newIndex == -1) {
                throw new InvalidOperationException("Tried to queue download at invalid index");
            }

            asList.Insert(newIndex, download);
            asList.RemoveAt(currentIndexOfDownload);

            downloadQueue = new(asList);
            UpdateQueueToSteamClient();
            DownloadQueueChanged?.Invoke(this, EventArgs.Empty);

            if (newPlacement == EAppDownloadQueuePlacement.PriorityPaused) {
                if (CurrentDownload == download) {
                    download.PauseDownload();
                }
            }
        }
    }

    public void StartDownload() {
        lock (downloadQueueLock)
        {
            if (!downloadQueue.Any()) {
                return;
            }

            // Re-queue current download
            if (CurrentDownload != null) {
                PauseDownload();
                ChangeDownloadIndex(CurrentDownload, EAppDownloadQueuePlacement.PriorityDown);
            }

            var item = downloadQueue.Peek();
            CurrentDownload = item;

            CurrentDownloadChanged?.Invoke(this, EventArgs.Empty);
            
            // Setup handlers
            item.DownloadProgressChanged += this.DownloadProgressChanged;
            item.DownloadStateChanged += (object? sender, EventArgs e) => {
                this.DownloadStateChanged?.Invoke(sender, e);

                if (item.DownloadState == DownloadState.Finished) {
                    lock (downloadQueueLock)
                    {
                        downloadQueue.Dequeue();
                        UpdateQueueToSteamClient();
                    }

                    this.DownloadFinished?.Invoke(this, EventArgs.Empty);
                } else if (item.DownloadState == DownloadState.Paused) {
                    this.DownloadPaused?.Invoke(this, EventArgs.Empty);
                }
            };

            // Start the download
            item.StartDownload();
        }
    }

    public void PauseDownload() {
        CurrentDownload?.PauseDownload();
    }

    private void UpdateQueueToSteamClient() {
        int actualIndex = 0;
        for (int i = 0; i < downloadQueue.Count; i++)
        {
            var item = downloadQueue.ElementAt(i);

            // Only update app downloads to the client
            if (item is DownloadItem aitem) {
                if (!SteamClient.GetIClientAppManager().SetAppDownloadQueueIndex(aitem.AppID, actualIndex)) {
                    Logging.GeneralLogger.Warning("Failed to set download queue index " + actualIndex + " for " + aitem.AppID);
                }

                actualIndex++;
            }
        }
    }

    internal void Shutdown()
    {
        stopPoll = true;
        while (!this.pollStopped)
        {
            System.Threading.Thread.Sleep(20);
        }
        
        downloadInfoUpdateThread = null;

        //TODO: wait for the download to finish more appropriately here. How to do?
        SteamClient.GetIClientAppManager().SetDownloadingEnabled(false);
        while (SteamClient.GetIClientAppManager().GetDownloadingAppID() != AppId_t.Invalid)
        {
            System.Threading.Thread.Sleep(20);
        }
    }
}