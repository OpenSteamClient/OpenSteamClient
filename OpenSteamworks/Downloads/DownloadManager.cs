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
    private List<AppId_t> downloadQueue = new();
    private List<AppId_t> scheduledDownloads = new();
    private List<AppId_t> unscheduledDownloads = new();
    public IEnumerable<AppId_t> DownloadQueue {
        get {
            lock (downloadQueueLock) {
                return downloadQueue.AsEnumerable();
            }
        }
    }


    public IEnumerable<AppId_t> ScheduledDownloads {
        get {
            lock (scheduledDownloadsLock) {
                return scheduledDownloads.AsEnumerable();
            }
        }
    }

    public IEnumerable<AppId_t> UnscheduledDownloads {
        get {
            lock (scheduledDownloadsLock) {
                return unscheduledDownloads.AsEnumerable();
            }
        }
    }

    public event EventHandler? DownloadsChanged;
    public event EventHandler<DownloadStats>? DownloadStatsChanged;

    private readonly ISteamClient steamClient;
    public AppId_t CurrentDownload { get; private set; }

    internal bool stopPoll = false;
    internal bool pollStopped = false;
    private Thread? downloadInfoUpdateThread;
    private readonly Dictionary<AppId_t, DateTime> downloadFinishTimes = new();
    public DownloadManager(ISteamClient steamClient)
    {
        this.steamClient = steamClient;
        steamClient.CallbackManager.RegisterHandler<DownloadScheduleChanged_t>(OnDownloadScheduleChanged);
        steamClient.CallbackManager.RegisterHandler<DownloadingAppChanged_t>(OnDownloadingAppChanged);
        steamClient.CallbackManager.RegisterHandler<AppEventStateChange_t>(OnAppEventStateChange);
        steamClient.CallbackManager.RegisterHandler<PostLogonState_t>(OnPostLogonState);
        downloadInfoUpdateThread = new Thread(DownloadInfoUpdate);
        downloadInfoUpdateThread.Start();
    }

    private bool isListeningForAppEventStateChanges = false;
    private void OnPostLogonState(CallbackManager.CallbackHandler<PostLogonState_t> handler, PostLogonState_t t)
    {
        if (t.connectedToCMs == 1 && t.hasAppInfo == 1) {
            isListeningForAppEventStateChanges = true;
            UpdateDownloadQueue();
        }   
    }

    private void OnAppEventStateChange(CallbackManager.CallbackHandler<AppEventStateChange_t> handler, AppEventStateChange_t t)
    {
        if (!isListeningForAppEventStateChanges) {
            return;
        }

        //TODO: This is inefficient, should update the app into the middle of the array
        UpdateDownloadQueue([t.m_nAppID]);
    }

    private void OnDownloadingAppChanged(CallbackManager.CallbackHandler<DownloadingAppChanged_t> handler, DownloadingAppChanged_t t)
    {
        UpdateDownloadQueue();
    }

    private IEnumerable<AppId_t> GetAppsWithUpdates() {
        // There's no function in the API to get apps that have updates, so do this instead (and assume only installed apps can be updated)
        List<AppId_t> installedApps = steamClient.ClientApps.GetInstalledApps().ToList();
        return installedApps.Where(a => !steamClient.ClientApps.BIsAppUpToDate(a));
    }

    private ulong BytesDownloadedLast;
    private ulong BytesProcessedLast;

    private void DownloadInfoUpdate() {
        while (!stopPoll)
        {
            DownloadStats downloadStats = new();
            
            if (steamClient.IClientAppManager.GetUpdateInfo(CurrentDownload, out AppUpdateInfo_s updateInfo)) {
                // This allows us to calculate the download rate very easily
                downloadStats.DownloadRateBytes = (updateInfo.m_unBytesDownloaded - this.BytesDownloadedLast);
                downloadStats.DiskRateBytes = (updateInfo.m_unBytesProcessed - this.BytesProcessedLast);

                this.BytesDownloadedLast = updateInfo.m_unBytesDownloaded;
                this.BytesProcessedLast = updateInfo.m_unBytesProcessed;
            } else {
                this.BytesDownloadedLast = 0;
                this.BytesProcessedLast = 0;
            }
            
            this.DownloadStatsChanged?.Invoke(this, downloadStats);
            System.Threading.Thread.Sleep(1000);
        }

        pollStopped = true;
        stopPoll = false;
    }

    private void UpdateDownloadQueue(IEnumerable<AppId_t>? extraApps = null) {
        HashSet<AppId_t> appsWithUpdates = GetAppsWithUpdates().ToHashSet();
        if (extraApps != null) {
            appsWithUpdates.UnionWith(extraApps);
        }

        appsWithUpdates.Remove(0);

        lock (downloadQueueLock)
        {
            lock (scheduledDownloadsLock)
            {
                scheduledDownloads.Clear();
                unscheduledDownloads.Clear();

                AbstractOrderedList<AppId_t> queue = new();
                foreach (var appid in appsWithUpdates)
                {
                    var idx = steamClient.IClientAppManager.GetAppDownloadQueueIndex(appid);
                    var scheduledTime = steamClient.IClientAppManager.GetAppAutoUpdateDelayedUntilTime(appid);
                    if (idx > -1) {
                        queue.Insert(idx, appid);
                    } else if (scheduledTime > 0) {
                        scheduledDownloads.Add(appid);
                    } else {
                        unscheduledDownloads.Add(appid);
                    }
                }

                var newDownloadQueue = queue.ConvertToList();
                foreach (var item in downloadQueue)
                {
                    if (!newDownloadQueue.Contains(item)) {
                        downloadFinishTimes[item] = DateTime.Now;
                    }
                }

                downloadQueue = newDownloadQueue;
            }
        }

        CurrentDownload = steamClient.IClientAppManager.GetDownloadingAppID();
        DownloadsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnDownloadScheduleChanged(CallbackManager.CallbackHandler<DownloadScheduleChanged_t> handler, DownloadScheduleChanged_t data)
    {
        UpdateDownloadQueue(data.m_rgunAppSchedule[0..data.m_nTotalAppsScheduled]);
    }

    public void DequeueDownload(AppId_t appid) {
        steamClient.IClientAppManager.ChangeAppDownloadQueuePlacement(appid, EAppDownloadQueuePlacement.PriorityNone);
    }

    internal void Shutdown(IProgress<string> progress)
    {
        stopPoll = true;
        while (!this.pollStopped)
        {
            System.Threading.Thread.Sleep(20);
        }
        
        downloadInfoUpdateThread = null;

        //TODO: wait for the download to finish more appropriately here. How to do?
        progress.Report("Stopping downloads");
        Logging.GeneralLogger.Info("Waiting for downloads to finish");
        steamClient.IClientAppManager.SetDownloadingEnabled(false);
        while (steamClient.IClientAppManager.GetDownloadingAppID() != AppId_t.Invalid)
        {
            if (steamClient.IClientAppManager.BIsDownloadingEnabled()) {
                steamClient.IClientAppManager.SetDownloadingEnabled(false);
            }

            System.Threading.Thread.Sleep(20);
        }

        downloadFinishTimes.Clear();
    }

    public DateTime? GetDownloadStartTime(AppId_t appID)
    {
        var startTime = steamClient.IClientAppManager.GetAppAutoUpdateDelayedUntilTime(appID);
        if (startTime == 0) {
            return null;
        }

        return (DateTime)startTime;
    }

    public DateTime? GetDownloadFinishTime(AppId_t appID)
    {
        lock (downloadQueueLock)
        {
            if (!downloadFinishTimes.TryGetValue(appID, out DateTime val)) {
                return null;
            }
    
            return val;
        }
    }
}