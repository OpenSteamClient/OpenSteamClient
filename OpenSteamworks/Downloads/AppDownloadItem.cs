using System;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Downloads;

public sealed class AppDownloadItem : IDownloadItem
{
    public AppId_t AppID { get; init; }
    public DownloadState DownloadState { get; private set; } = DownloadState.NotStarted;

    /// <summary>
    /// What type of update we're downloading
    /// </summary>
    public AppDownloadType DownloadType { get; private set; } = AppDownloadType.App;
    public string Name { get; private set; }

    public ulong DownloadRate { get; private set; } = 0;
    public ulong DiskRate { get; private set; } = 0;

    public bool DownloadProgressKnown { get; private set; } = true;
    public float DownloadProgress { get; private set; } = 0.0f;

    public EAppUpdateError DownloadError { get; private set; } = EAppUpdateError.NoError;
    public string DownloadErrorExtended { get; private set; } = "";

    public string DownloadStateExtended { get; private set; } = "";

    public DateTime DownloadStartTime { get; private set; } = DateTime.MinValue;
    public DateTime DownloadAutoStartTime { get; private set; } = DateTime.MinValue;

    public event EventHandler? DownloadStateChanged;
    public event EventHandler? DownloadProgressChanged;

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
}