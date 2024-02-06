using System;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Downloads;

public interface IDownloadItem {
    public event EventHandler DownloadStateChanged;
    public event EventHandler DownloadProgressChanged;
    public event EventHandler DownloadRateChanged;

    /// <summary>
    /// Download speed in bytes per second
    /// </summary>
    public ulong DownloadRate { get; }

    /// <summary>
    /// Disk usage rate in bytes per second
    /// </summary>
    public ulong DiskRate { get; }

    /// <summary>
    /// Download progress as a value of 0 to 100
    /// </summary>
    public double DownloadProgress { get; }

    /// <summary>
    /// The state of this download.
    /// </summary>
    public DownloadState DownloadState { get; }

    /// <summary>
    /// Error which occurred during the download. 
    /// Is set when DownloadState is Errored
    /// To check for an error state, check if DownloadState is Errored or ExtendedErrored and choose the correct property to display.
    /// </summary>
    public EAppUpdateError DownloadError { get; }
    
    /// <summary>
    /// The text that should be displayed on the download.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Custom operation name. Will be set if DownloadState is ExtendedRunning.
    /// </summary>
    public string DownloadStateExtended { get; }

    /// <summary>
    /// Custom error text. Will be set if DownloadState is ExtendedErrored.
    /// </summary>
    public string DownloadErrorExtended { get; }

    /// <summary>
    /// If the progress of the download is known. UI should show an indeterminate progress indicator if the progress is unknown.
    /// </summary>
    public bool DownloadProgressKnown { get; }

    /// <summary>
    /// When the download was started. DateTime.MinValue if it wasn't started yet.
    /// </summary>
    public DateTime DownloadStartTime { get; }

    /// <summary>
    /// When the download will auto start. DateTime.MinValue if it will never auto start.
    /// </summary>
    //TODO: allow the user to change this. 
    public DateTime DownloadAutoStartTime { get; }

    /// <summary>
    /// Start this download. Will try to bump it to the top of the download queue.
    /// This should not be called manually, it will be called automatically by DownloadManager.
    /// </summary>
    public void StartDownload();

    /// <summary>
    /// Pause this download. 
    /// This should not be called manually, it will be called automatically by DownloadManager.
    /// </summary>
    public void PauseDownload();

    /// <summary>
    /// Called every second by DownloadManager. Use this to update download and disk rates.
    /// This should not be called manually, it will be called automatically by DownloadManager.
    /// </summary>
    public void UpdateRates();
}