using System;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Downloads;

public sealed class AppDownloadItem : IDownloadItem
{
    public AppId_t AppID { get; init; }
    public DownloadState DownloadState { get; private set; } = DownloadState.NotStarted;

    /// <summary>
    /// What type of update we're downloading
    /// </summary>
    public AppDownloadType DownloadType { get; private set; } = AppDownloadType.App;

    /// <summary>
    /// The workshop item ID we're downloading. 0 if not downloading a workshop item
    /// </summary>
    public uint WorkshopItemID { get; private set; }

    public string Name { get; private set; }

    public ulong DownloadRate { get; private set; } = 0;
    public ulong DiskRate { get; private set; } = 0;

    public bool DownloadProgressKnown { get; private set; } = true;
    public double DownloadProgress { get; private set; } = 0.0f;

    public EAppUpdateError DownloadError { get; private set; } = EAppUpdateError.NoError;
    public string DownloadErrorExtended { get; private set; } = "";

    public string DownloadStateExtended { get; private set; } = "";

    public DateTime DownloadStartTime { get; private set; } = DateTime.MinValue;
    public DateTime DownloadAutoStartTime { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// Downloaded bytes
    /// </summary>
    public ulong DownloadedBytes { get; private set; }

    /// <summary>
    /// Total bytes to download
    /// </summary>
    public ulong BytesToDownload { get; private set; }

    /// <summary>
    /// Processed (decompressed, written) bytes
    /// </summary>
    public ulong ProcessedBytes { get; private set; }

    /// <summary>
    /// Total bytes to process
    /// </summary>
    public ulong BytesToProcess { get; private set; }

    public event EventHandler? DownloadStateChanged;
    public event EventHandler? DownloadProgressChanged;
    public event EventHandler? DownloadRateChanged;

    public AppDownloadItem(AppId_t appid) {
        this.AppID = appid;
        Name = SteamClient.GetClientApps().GetAppName(appid);
    }

    void IDownloadItem.PauseDownload()
    {
        SteamClient.GetIClientAppManager().SetDownloadingEnabled(false);
        this.DownloadState = DownloadState.Paused;
        this.DownloadStateChanged?.Invoke(this, EventArgs.Empty);
    }

    void IDownloadItem.StartDownload()
    {
        SteamClient.GetIClientAppManager().SetDownloadingEnabled(true);
        this.DownloadState = DownloadState.Running;
        this.DownloadStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private ulong BytesDownloadedLast;
    private ulong BytesProcessedLast;

    void IDownloadItem.UpdateRates()
    {
        //SteamClient.GetIClientAppManager().GetDownloadStats(out DownloadStats_s stats);
        SteamClient.GetIClientAppManager().GetUpdateInfo(this.AppID, out AppUpdateInfo_s updateInfo);
        
        //TODO: this should be done elsewhere
        this.WorkshopItemID = updateInfo.downloadingWorkshopItemID;
        this.DownloadStartTime = (DateTime)updateInfo.m_timeUpdateStart;

        this.DownloadedBytes = updateInfo.m_unBytesDownloaded;
        this.BytesToDownload = updateInfo.m_unBytesToDownload;
        this.ProcessedBytes = updateInfo.m_unBytesProcessed;
        this.BytesToProcess = updateInfo.m_unBytesToProcess;

        // UpdateRates gets called every second, never more, never less. This allows us to "calculate" the download rate very easily
        this.DownloadRate = (updateInfo.m_unBytesDownloaded - this.BytesDownloadedLast);
        this.DiskRate = (updateInfo.m_unBytesProcessed - this.BytesProcessedLast);

        this.BytesDownloadedLast = updateInfo.m_unBytesDownloaded;
        this.BytesProcessedLast = updateInfo.m_unBytesProcessed;

        this.DownloadProgress = updateInfo.m_unBytesProcessed / updateInfo.m_unBytesToProcess;
        this.DownloadProgressChanged?.Invoke(this, EventArgs.Empty);
    }
}