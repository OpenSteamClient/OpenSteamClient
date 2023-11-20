using System.Reflection;
using System.Runtime.Versioning;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.OSSpecific;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Managers;

public class InstallManager
{
    /// <summary>
    /// The path where OpenSteamClient is installed.
    /// </summary>
    public string InstallDir { get; private set; }
    
    /// <summary>
    /// The directory where all machine-local config files are kept.
    /// </summary>
    public string ConfigDir { get; private set; }

    /// <summary>
    /// Location of the datalink.
    /// Should always be the .steam directory inside the current user's home directory.
    /// </summary>
    [SupportedOSPlatform("linux")]
    public string DatalinkDir { get; private set; } = "";

    /// <summary>
    /// The current OS user's home directory.
    /// </summary>
    public string HomeDir { get; private set; }

    /// <summary>
    /// Location of log files. Currently always a subdirectory 'logs' in <see href="InstallDir"/>.
    /// </summary>
    public string LogsDir { get; private set; }

    /// <summary>
    /// Location of cachable files for all users.
    /// </summary>
    public string CacheDir { get; private set; }

    /// <summary>
    /// The directory of OpenSteamClient's consumer's executable (ClientUI in the case of OpenSteamClient). In debug builds, this is the output folder.
    /// In release builds, this can be anywhere, but usually should be the same directory as <see href="InstallDir"/>.
    /// </summary>
    public string AssemblyDirectory { get; private set; }

    public InstallManager() {
        AssemblyDirectory = UtilityFunctions.AssertNotNull(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        //TODO: use registry to get installdir on Windows when we build an installer.
        // Currently just use LocalApplicationData, which maps:
        // Linux: $HOME/.local/share/OpenSteam
        // Windows: C:\Users\USERNAME\AppData\Local
        // Mac: /Users/USERNAME/.local/share
        var localShare = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        InstallDir = Path.Combine(localShare, "OpenSteam");

        HomeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsLinux()) {
            DatalinkDir = Path.Combine(HomeDir, ".steam");
            CacheDir = LinuxSpecifics.GetXDGSpecPath("XDG_CACHE_HOME", ".cache", "OpenSteam");
            LogsDir = LinuxSpecifics.GetXDGSpecPath("XDG_STATE_HOME", ".local/state", "OpenSteam/logs");
            ConfigDir = LinuxSpecifics.GetXDGSpecPath("XDG_CONFIG_HOME", ".config", "OpenSteam/config");
            Directory.CreateDirectory(DatalinkDir);
        } else {
            CacheDir = Path.Combine(InstallDir, "cache");
            LogsDir = Path.Combine(InstallDir, "logs");
            ConfigDir = Path.Combine(InstallDir, "config");
        }

        Directory.CreateDirectory(InstallDir);
        Directory.CreateDirectory(ConfigDir);
        Directory.CreateDirectory(LogsDir);
        Directory.CreateDirectory(CacheDir);
    }

    public string GetLogPath(string logFileName) {
        Directory.CreateDirectory(LogsDir);
        return Path.Combine(LogsDir, logFileName + ".log");
    }
}