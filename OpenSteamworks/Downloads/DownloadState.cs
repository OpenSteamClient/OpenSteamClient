namespace OpenSteamworks.Downloads;

public enum DownloadState {
    /// <summary>
    /// Download has never been started
    /// Will be placed in Starting state when started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Download is currently initializing
    /// Will be placed in Running or ExtendedRunning state when initialization is done
    /// </summary>
    Starting,

    /// <summary>
    /// Download is running
    /// </summary>
    Running,

    /// <summary>
    /// Download is pausing, may be still downloading
    /// Will be placed in Paused state
    /// </summary>
    Pausing,

    /// <summary>
    /// Download is paused, can be resumed
    /// Will be placed in Resuming state
    /// </summary>
    Paused,

    /// <summary>
    /// Download failed, can be retried
    /// Will be placed in Starting state
    /// </summary>
    Errored,

    /// <summary>
    /// Download failed, can be retried
    /// Will be placed in Starting state
    /// Try to avoid using this, as you'll have to manually translate your error text
    /// </summary>
    ExtendedErrored,

    /// <summary>
    /// Download is resuming, will be placed in Downloading state when done
    /// </summary>
    Resuming,

    /// <summary>
    /// Download state is running with a custom status text
    /// Try to avoid using this, as you'll have to manually translate your status text
    /// </summary>
    ExtendedRunning,

    /// <summary>
    /// Download is finishing, for example writing data to the disk or creating proton compat data
    /// Will be placed in Finished state when done
    /// </summary>
    Finishing,

    /// <summary>
    /// Download is finished, cannot be resumed or started. Can be removed completely from the queue
    /// </summary>
    Finished
}