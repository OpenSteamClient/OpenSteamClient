using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using OpenSteamworks.Generated;
using OpenSteamworks.Extensions;
using System.Security.Cryptography;
using OpenSteamworks.Client.Utils;
using System.Formats.Tar;
using System.Diagnostics;
using System.Runtime.Versioning;
using OpenSteamworks.Client.Managers;
using System.Runtime.InteropServices;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Utils;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;
using System.Collections.ObjectModel;
using Profiler;

namespace OpenSteamworks.Client.Startup;

//TODO: this whole thing needs a rewrite badly
public class Bootstrapper {

    public string OverrideURL { get; set; } = "";
    public string BaseURL {
        get {
            if (!string.IsNullOrEmpty(OverrideURL)) {
                return OverrideURL;
            }

            //TODO: We shouldn't hardcode this but it will do...
            return "https://client-update.akamai.steamstatic.com/";
        }
    }

    private InstallManager installManager;
    public string SteamclientLibPath {
        get {
            return Path.Combine(MainBinaryDir, OpenSteamworks.Client.Utils.OSSpecifics.Instance.SteamClientBinaryName);
        }
    }
    public string PlatformClientManifest {
        get {
            return OpenSteamworks.Client.Utils.OSSpecifics.Instance.SteamClientManifestName;
        }
    }
    public string PackageDir => Path.Combine(installManager.InstallDir, "package");
    public string MainBinaryDir {
        get {
            if (OperatingSystem.IsLinux()) {
                return Path.Combine(installManager.InstallDir, "linux64");
            }

            return installManager.InstallDir;
        }
    }

    [SupportedOSPlatform("linux")]
    public string Ubuntu12_32Dir => Path.Combine(installManager.InstallDir, "ubuntu12_32");

    [SupportedOSPlatform("linux")]
    public string Ubuntu12_64Dir => Path.Combine(installManager.InstallDir, "ubuntu12_64");
    
    //TODO: make this into an interface so clients can decide what packages they want
    private bool IsPackageBlacklisted(string packageName) {
        if (packageName.StartsWith("tenfoot_")) {
            return true;
        }

        if (packageName.StartsWith("resources_")) {
            return true;
        }

        if (packageName.StartsWith("friendsui_")) {
            return true;
        }

        if (packageName.StartsWith("public_") && !packageName.StartsWith("public_all")) {
            return true;
        }

        if (packageName.StartsWith("steamui_")) {
            return true;
        }

        return false;
    }
    private int RetryCount = 0;
    private readonly Dictionary<string, string> downloadedPackages = new();

    private bool restartRequired = false;

    private IExtendedProgress<int>? progressHandler;

    public void SetProgressObject(IExtendedProgress<int>? progressHandler) {
        this.progressHandler = progressHandler;
    }

    private readonly BootstrapperState bootstrapperState;
    private readonly Logger logger;
    private readonly ConfigManager configManager;

    public Bootstrapper(InstallManager installManager, BootstrapperState bootstrapperState, ConfigManager configManager) {
        this.logger = Logger.GetLogger("Bootstrapper", installManager.GetLogPath("Bootstrapper"));
        this.installManager = installManager;
        this.bootstrapperState = bootstrapperState;
        this.configManager = configManager;
    }

    /// <summary>
    /// Restarts OpenSteamClient immediately, without running any handlers (except waiting for debugger detach)
    /// </summary>
    public async Task Restart() {
        bool hadDebugger = false;

        // Can't forcibly detach the debugger, which is needed since:
        // - execvp breaks debugger, but is supposedly not a problem
        // - child processes also can't be debugged, which is also apparently not a problem
        // So that leaves us no way to "transfer" the debugger from the old process to the new one, unlike with the old C++ solution where gdb just does it when execvp:ing
        if (Debugger.IsAttached) {
            hadDebugger = true;
            progressHandler?.SetOperation("Please detach your debugger. ");
            progressHandler?.SetSubOperation("You should re-attach once the process has restarted.");
            Debugger.Log(5, "DetachDebugger", "Please detach your debugger. You should re-attach once the process has restarted.");
            await Task.Run(() =>
            {
                while (Debugger.IsAttached)
                {
                    Thread.Sleep(500);
                }
            });
        }

        if (OperatingSystem.IsLinux()) {
            this.ReExecWithEnvs(hadDebugger);
        } else if (OperatingSystem.IsWindows()) {
            // Poor hack, windows sucks.
            // - Will have 2 processes running simultaneously (lots of problems)
            // - exit is still semi graceful
            // - we made it wait until this process exits
            // - doesn't keep same terminal handle
            // - will probably break the debugger as well
            UtilityFunctions.SetEnvironmentVariable("OPENSTEAM_RESTART_WAIT", "1");
            Process.Start(UtilityFunctions.AssertNotNull(Environment.ProcessPath), Environment.GetCommandLineArgs());
            Environment.Exit(0);
        }
    }

    public async Task RunBootstrap(Action<string, string> msgBoxProvider) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.RunBootstrap");
        if (progressHandler == null) {
            progressHandler = new ExtendedProgress<int>(0, 100);
        }

        progressHandler.SetOperation("Bootstrapping");

        if (OperatingSystem.IsWindows()) {
            if (OSCheck.IsWindows11()) {
                msgBoxProvider("Unsupported OS", "Windows 11 is unsupported.");
            }
        } else if (OperatingSystem.IsLinux()) {
            if (!OSCheck.IsArchLinux()) {
                msgBoxProvider("Unsupported distro", "Distros other than Arch Linux are unsupported.");
            }
        } else {
            msgBoxProvider("Unsupported OS", "Only Windows and Linux are supported.");
        }
        
        try
        {
            using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.RunBootstrap - Detect local package server");
            using (HttpResponseMessage resp = await Client.HttpClient.GetAsync(bootstrapperState.LocalPackageServerURL+PlatformClientManifest))
            {
                if (resp.IsSuccessStatusCode) {
                    var str = await resp.Content.ReadAsStringAsync();
                    var deserialized = KVTextDeserializer.Deserialize(str);
                    string? serverVersion = deserialized.GetChild("version")?.GetValueAsString();
                    if (serverVersion == VersionInfo.STEAM_MANIFEST_VERSION.ToString()) {
                        OverrideURL = bootstrapperState.LocalPackageServerURL;
                        logger.Info("Using local package server at " + OverrideURL);
                    } else {
                        logger.Debug("Local package server has different version: " + serverVersion + " than ours " + VersionInfo.STEAM_MANIFEST_VERSION.ToString());
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            logger.Debug("Not using local package server, error occurred:");
            logger.Debug(e.ToString());
        }
        
        // steamclient blindly dumps certain files to the CWD, so set it to the install dir
        Directory.SetCurrentDirectory(installManager.InstallDir);

        Directory.CreateDirectory(PackageDir);

        // Windows only hack.
        if (OperatingSystem.IsWindows()) {
            using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.RunBootstrap - Wait for existing process termination");
            IEnumerable<Process> processes;
            while (true)
            {
                processes = Process.GetProcessesByName("ClientUI").Where(p => p.Id != Environment.ProcessId);
                
                if (!processes.Any()) {
                    break;
                } else {
                    bool hadClientUIProcess = false;
                    foreach (var item in processes)
                    {
                        if (item.MainModule?.FileName == Environment.ProcessPath) {
                            hadClientUIProcess = true;
                        }
                    }

                    if (!hadClientUIProcess) {
                        break;
                    }
                }

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Waiting for ClientUI to terminate");
            }

            // Blacklist these as well so we don't get file lock errors in the bootstrapper
            while (true)
            {
                processes = Process.GetProcessesByName("steamerrorreporter64").Concat(Process.GetProcessesByName("steamerrorreporter"));
                if (!processes.Any()) {
                    break;
                }

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Waiting for steamerrorreporter to terminate");
            }
        }

        // Linux only hack.
        if (OperatingSystem.IsLinux()) {
            using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.RunBootstrap - Wait for existing process termination");
            IEnumerable<Process> processes;
            while (true)
            {
                processes = Process.GetProcessesByName("steamserviced").Concat(Process.GetProcessesByName("htmlhost"));
                
                if (!processes.Any()) {
                    break;
                }

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Waiting for steamserviced/htmlhost to terminate");
            }
        }

        // Skip verification and package processing if user requests it
        if (!bootstrapperState.SkipVerification) {
            if (!VerifyFiles(progressHandler, out IEnumerable<string> failureReason)) {
                logger.Error("Failed verification: " + string.Join(", ", failureReason));
                await EnsurePackages(msgBoxProvider, progressHandler);
                await ExtractPackages(progressHandler);
            }
        }

        CreateSymlinks(progressHandler);

        // Run platform specific tasks
        if (OperatingSystem.IsWindows()) {
            // Write our PID to HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam (has full access to entire Steam key by all users, security nightmare?)
            //TODO: doesn't support machines where ValveSteam hasn't been installed atleast once (though this'll be handled by the installer)
            Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Valve\\Steam", "SteamPID", Environment.ProcessId);
        }

        if (OperatingSystem.IsLinux())
        {
            // Make ourselves XDG compliant
            MakeXDGCompliant();

            // Process the Steam runtime (needed for SteamVR and some tools steam ships with)
            await CheckSteamRuntime(progressHandler);

            if (!bootstrapperState.LinuxPermissionsSet)
            {
                using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.RunBootstrap - Linux: Set permissions");
                progressHandler.SetOperation($"Setting proper permissions (this may freeze)");
                progressHandler.SetThrobber();
                // Valve doesn't include permission info in the zips, so chmod them all to allow execute
                await Process.Start("/usr/bin/chmod", "-R +x " + '"' + installManager.InstallDir + '"').WaitForExitAsync();

                bootstrapperState.LinuxPermissionsSet = true;
            }

            //TODO: check for steam some other way (like trying to connect)
            Process[] runningSteamProcesses = Process.GetProcessesByName("steam");
            if (runningSteamProcesses.Length == 0) {
                using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.RunBootstrap - Linux: Update datalink");

                Directory.CreateDirectory(installManager.DatalinkDir);

                // This is ok, since steam automatically changes the target of these symlinks to it's own install path on start.
                List<(string name, string targetPath)> datalinkDirs = new()
                {
                    ("steam", installManager.InstallDir),
                    ("root", installManager.InstallDir),
                    ("sdk64", Path.Combine(installManager.InstallDir, "linux64")),
                    ("sdk32", Path.Combine(installManager.InstallDir, "linux32")),
                    ("bin64", Path.Combine(installManager.InstallDir, "ubuntu12_64")),
                    ("bin32", Path.Combine(installManager.InstallDir, "ubuntu12_32")),
                    ("bin", Path.Combine(installManager.DatalinkDir, "bin32")),
                };

                // Create needed directory structure
                foreach ((string name, string targetPath) in datalinkDirs)
                {
                    var linkPath = Path.Combine(installManager.DatalinkDir, name);
                    if (Directory.Exists(linkPath)) {
                        var fsinfo = Directory.ResolveLinkTarget(linkPath, false);
                        if (fsinfo != null) {
                            Directory.Delete(linkPath, false);
                            Directory.CreateSymbolicLink(linkPath, targetPath);
                        } else {
                            throw new InvalidOperationException(linkPath + " was not a symlink.");
                        }
                    } else {
                        Directory.CreateSymbolicLink(linkPath, targetPath);
                    }
                }

                var steampidPath = Path.Combine(installManager.DatalinkDir, "steam.pid");
                try
                {
                    if (File.Exists(steampidPath)) {
                        string pidstr = File.ReadAllText(steampidPath);
                        if (!Process.GetProcessById(int.Parse(pidstr)).HasExited) {
                            // Sure, whatever.
                            // I've heard exceptions as control flow is a bad idea. 
                            throw new Exception("Steam is still alive");
                        }
                    }

                }
                catch (System.Exception)
                {
                    // Previous instance has exited, safe to proceed
                    File.WriteAllText(steampidPath, Environment.ProcessId.ToString());
                }
            }
        }
            
        progressHandler.SetOperation($"Finalizing");
        progressHandler.SetThrobber();

        // Copy/Link our files over (steamserviced, 64-bit reaper and 64-bit steamlaunchwrapper, other platform specific niceties)
        CopyOpensteamFiles(progressHandler);

        bootstrapperState.CommitHash = GitInfo.GitCommit;
        bootstrapperState.InstalledVersion = VersionInfo.STEAM_MANIFEST_VERSION;
        await configManager.SaveAsync(bootstrapperState);

        await FinishBootstrap(progressHandler);
    }

    private void CreateSymlinks(IExtendedProgress<int> progressHandler)
    {
        progressHandler.SetSubOperation("Creating symlinks");
        logger.Info($"Creating symlinks");
        // Specify path mappings here to tell the files to link into another folder as well
        Dictionary<string, string> pathMappings = new() {
            {"ubuntu12_64/libsteamwebrtc.so", "libsteamwebrtc.so"},
        };

        foreach (var mapping in pathMappings)
        {
            string sourcePath = Path.Combine(installManager.InstallDir, mapping.Key);
            string targetPath = Path.Combine(installManager.InstallDir, mapping.Value);
            if (File.Exists(sourcePath)) {
                if (File.Exists(targetPath)) {
                    File.Delete(targetPath);
                }

                File.CreateSymbolicLink(targetPath, sourcePath);
                var info = new FileInfo(targetPath);
                bootstrapperState.InstalledFiles[targetPath] = info.Length;
            } else {
                logger.Info($"Not linking {mapping.Key} -> {mapping.Value}, source doesn't exist");
            }
        }

        // Link existing ValveSteam data if we can link over
        //TODO: settings to determine when to do this
        if (installManager.ValveSteamInstallDir != null) {
            var valveSteamPath = Path.Combine(installManager.ValveSteamInstallDir, "config", "libraryfolders.vdf");
            var valveSteamappsPath = Path.Combine(installManager.ValveSteamInstallDir, "steamapps");
            var openSteamPath = Path.Combine(installManager.ConfigDir, "libraryfolders.vdf");
            var openSteamappsPath = Path.Combine(installManager.InstallDir, "steamapps");
            var openSteamBackupPath = Path.Combine(installManager.ConfigDir, "libraryfolders_backup.vdf");
            var openSteamFileInfo = new FileInfo(openSteamPath);
            var openSteamappsDirectoryInfo = new DirectoryInfo(openSteamappsPath);

            bool shouldCopy = openSteamFileInfo.IsLink() || !openSteamFileInfo.Exists;
            bool shouldCopySteamapps = openSteamappsDirectoryInfo.IsLink() || !openSteamappsDirectoryInfo.Exists;
            bool windowsHasLinkedSteamapps = !OperatingSystem.IsWindows() || (OperatingSystem.IsWindows() && openSteamappsDirectoryInfo.IsLink());
            if (File.Exists(valveSteamPath)) {
                // Always copy and overwrite libraryfolders_backup.vdf
                File.Copy(valveSteamPath, openSteamBackupPath, true);

                if (windowsHasLinkedSteamapps) {
                    if (shouldCopy) {
                        // Link over libraryfolders.vdf
                        File.Delete(openSteamPath);
                        File.CreateSymbolicLink(openSteamPath, valveSteamPath);
                        logger.Info($"Linked libraryfolders.vdf from ValveSteam ({openSteamPath} -> {valveSteamPath})");
                    } else {
                        logger.Info("Not linking libraryfolders.vdf from ValveSteam, as shouldCopy is false");
                    }
                } else {
                    logger.Info("Not linking libraryfolders.vdf from ValveSteam, as steamapps isn't linked");
                }

                if (!openSteamappsDirectoryInfo.Exists) {
                    if (!OperatingSystem.IsWindows()) {
                        if (Directory.Exists(valveSteamappsPath)) {
                            if (shouldCopySteamapps) {
                                // Link over steamapps
                                Directory.CreateSymbolicLink(openSteamappsPath, valveSteamappsPath);
                                logger.Info("Linked steamapps from ValveSteam");
                            } else {
                                logger.Info("Not linking steamapps from ValveSteam as shouldCopySteamapps is false");
                            }
                        } else {
                            logger.Info("Not linking steamapps from ValveSteam, as ValveSteam doesn't contain steamapps");
                        }
                    } else {
                        logger.Info("Not linking steamapps from ValveSteam, as we're on Windows. To do this yourself, follow the wiki at https://github.com/OpenSteamClient/OpenSteamClient/wiki/Linking-steamapps-on-Windows");
                    }
                } else {
                    logger.Info("Not linking steamapps from ValveSteam, as it already exists.");
                }
                
                bootstrapperState.LastConfigLinkSuccess = true;
            } else if (!File.Exists(valveSteamPath) && bootstrapperState.LastConfigLinkSuccess) {
                // ValveSteam was deleted, remove libraryfolders.vdf link and copy over our backup (steam will auto fix the folders if they're broken)
                if (File.Exists(openSteamBackupPath))
                {
                    if (shouldCopy) {
                        File.Copy(openSteamBackupPath, openSteamPath, true);
                        logger.Info("Copied libraryfolders_backup.vdf -> libraryfolders.vdf");
                    } else {
                        logger.Warning("Not overwriting libraryfolders.vdf with backup, as libraryfolders.vdf is file and not link");
                    }
                }
                else
                {
                    logger.Warning("LastConfigLinkSuccess is true but libraryfolders_backup doesn't exist???");
                }

                if (openSteamappsDirectoryInfo.IsLink() && !Directory.Exists(valveSteamappsPath)) {
                    // ValveSteam was deleted, remove steamapps link
                    Directory.Delete(openSteamappsPath);
                    logger.Info("Removing steamapps link since ValveSteam was deleted");
                }

                // Mark last link as unsuccessful
                bootstrapperState.LastConfigLinkSuccess = false;
            } else {
                logger.Info("Not copying libraryfolders.vdf from ValveSteam, as ValveSteam is not installed");
            }
        } else {
            logger.Info("ValveSteam is not installed, skipping ValveSteam link");
        }
    }

    private async Task FinishBootstrap(IExtendedProgress<int> progressHandler) {
        // Currently only linux needs a restart (for LD_PRELOAD and LD_LIBRARY_PATH)
        var hasReran = UtilityFunctions.GetEnvironmentVariable("OPENSTEAM_RAN_EXECVP") == "1";
        restartRequired = OperatingSystem.IsLinux() && !hasReran;
        
        SetEnvsForSteamLoad();
        progressHandler.SetOperation("Bootstrapping Completed" + (restartRequired ? ", restarting" : ""));

        await RestartIfNeeded(progressHandler);
    }

    private async Task RestartIfNeeded(IExtendedProgress<int> progressHandler) {
        bool debuggerShouldReattach = UtilityFunctions.GetEnvironmentVariable("OPENSTEAM_REATTACH_DEBUGGER") == "1";

        if (restartRequired) {
            await Restart();
        } else {
            // Can't forcibly attach the debugger either
            if (debuggerShouldReattach) {
                progressHandler.SetOperation("Waiting for debugger to re-attach before continuing...");
                progressHandler.SetSubOperation("PID: " + Environment.ProcessId + ", Name: " + Process.GetCurrentProcess().ProcessName);
                await Task.Run(() =>
                {
                    while (!Debugger.IsAttached)
                    { 
                        logger.Info("Waiting for debugger...");
                        System.Threading.Thread.Sleep(500);
                    }
                });
            }
        }
    }

    [SupportedOSPlatform("linux")]
    private void MakeXDGCompliant()
    {
        using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.MakeXDGCompliant");

        var logsSymlink = Path.Combine(installManager.InstallDir, "logs");
        if (!Directory.Exists(logsSymlink))
            Directory.CreateSymbolicLink(logsSymlink, installManager.LogsDir);

        var configSymlink = Path.Combine(installManager.InstallDir, "config");
        if (!Directory.Exists(configSymlink))
            Directory.CreateSymbolicLink(configSymlink, installManager.ConfigDir);

        var cacheSymlink = Path.Combine(installManager.InstallDir, "appcache");
        if (!Directory.Exists(cacheSymlink))
            Directory.CreateSymbolicLink(cacheSymlink, installManager.CacheDir);
    }

    [SupportedOSPlatform("linux")]
    private void ReExecWithEnvs(bool withDebugger) {
        [DllImport("libc", SetLastError = true)]
        static extern int execvp([MarshalAs(UnmanagedType.LPUTF8Str)] string file, [MarshalAs(UnmanagedType.LPArray)] string?[] args);

        logger.Info("Re-execing");

        if (withDebugger) {
            UtilityFunctions.SetEnvironmentVariable("OPENSTEAM_REATTACH_DEBUGGER", "1");
        }
        UtilityFunctions.SetEnvironmentVariable("OPENSTEAM_RAN_EXECVP", "1");
        UtilityFunctions.SetEnvironmentVariable("LD_LIBRARY_PATH", $"{Path.Combine(installManager.InstallDir, "ubuntu12_64")}:{Path.Combine(installManager.InstallDir, "ubuntu12_32")}:{Path.Combine(installManager.InstallDir)}:{UtilityFunctions.GetEnvironmentVariable("LD_LIBRARY_PATH")}");

        string?[] fullArgs = Environment.GetCommandLineArgs();

        string executable = Directory.ResolveLinkTarget("/proc/self/exe", false)!.FullName;
        logger.Debug("Re-exec executable: " + executable);
        if (!executable.EndsWith("dotnet")) {
            fullArgs[0] = executable;
            fullArgs = fullArgs.Append(null).ToArray();
        } else {
            fullArgs = fullArgs.Prepend(executable).Append(null).ToArray();
        }
        
        foreach (var item in fullArgs)
        {
            logger.Debug("Re-exec argument: " + item);
        }

        // Program execution ends here, if execvp returns, it means re-execution failed
        int ret = execvp(executable, fullArgs);
        throw new Exception($"Execvp failed: {ret}, errno: {Marshal.GetLastWin32Error()}");
    }

    // Sets environment variables necessary for steamclient to work properly.
    private void SetEnvsForSteamLoad() {
       UtilityFunctions.SetEnvironmentVariable("SteamAppId", "");
       // These two should point to the current running Steam's path. (We can set these later from post-steamclient load code)
       UtilityFunctions.SetEnvironmentVariable("SteamPath", installManager.InstallDir);
       UtilityFunctions.SetEnvironmentVariable("ValvePlatformMutex", installManager.InstallDir.ToLowerInvariant());
       UtilityFunctions.SetEnvironmentVariable("BREAKPAD_DUMP_LOCATION", Path.Combine(installManager.InstallDir, "dumps"));
    }
    
    private bool VerifyFiles(IExtendedProgress<int> progressHandler, out IEnumerable<string> failureReason) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.VerifyFiles");

        var failureReasons = new List<string>();
        // Verify all files and skip this step if files are valid and version matches 
        bool failedSteamVer = bootstrapperState.InstalledVersion != VersionInfo.STEAM_MANIFEST_VERSION;
        bool failedCommit = bootstrapperState.CommitHash != GitInfo.GitCommit;
        bool failed =  failedSteamVer || failedCommit;
        if (failed) {
            if (failedSteamVer) {
                failureReasons.Add($"Installed manifest (${bootstrapperState.InstalledVersion}) does not match expected (${VersionInfo.STEAM_MANIFEST_VERSION})");
            }

            if (failedCommit) {
                failureReasons.Add($"Installed commit (${bootstrapperState.CommitHash}) does not match expected (${GitInfo.GitCommit})");
            }
        }
        int installedFilesLength = bootstrapperState.InstalledFiles.Count;
        int checkedFiles = 0;

        progressHandler.SetOperation("Checking files");

        // Convert absolute progress (files checked) into relative progress (0% - 100%)
        var relativeProgress = new Progress<long>(totalFiles => progressHandler.Report((int)(totalFiles / checkedFiles) * 100));
        
        foreach (var installedFile in bootstrapperState.InstalledFiles)
        {
            if (failed) {
                break;
            }

            checkedFiles++;
            var info = new FileInfo(Path.Combine(installManager.InstallDir, installedFile.Key));
            if (info.Exists) {
                if (info.Length != installedFile.Value) {
                    failureReasons.Add("File " + info.Name + " length was " + info.Length + " but expected " + installedFile.Value);
                    failed = true;
                }
            } else {
                failureReasons.Add("File " + info.Name + " doesn't exist");
                failed = true;
            }
        }

        failureReason = failureReasons;

        // Remove packages only in case of a steam version upgrade
        if (bootstrapperState.InstalledVersion != 0 && (bootstrapperState.InstalledVersion != VersionInfo.STEAM_MANIFEST_VERSION)) {
            Directory.Delete(PackageDir, true);
            Directory.CreateDirectory(PackageDir);
        }

        if (installedFilesLength > 0 && !failed) {
            progressHandler.SetProgress(progressHandler.MaxProgress);
            return true;
        }
        
        return false;
    }

    private async Task EnsurePackages(Action<string, string> msgBoxProvider, IExtendedProgress<int> progressHandler) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.EnsurePackages");

        downloadedPackages.Clear();

        // Fetch the manifests from OpenSteamworks.Client.dll
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        IFileInfo fileInfo = embeddedProvider.GetFileInfo($"{PlatformClientManifest}.vdf");
        if (!fileInfo.Exists) {
            throw new Exception($"Cannot find {PlatformClientManifest}.vdf as an embedded resource.");
        }

        List<string> verificationFailed = new List<string>();
        string text;
        using (var stream = fileInfo.CreateReadStream())
        {
            using (var reader = new StreamReader(stream))
            {
                text = await reader.ReadToEndAsync();
            }
        }

        KVObject data = KVTextDeserializer.Deserialize(text);

        progressHandler.SetOperation("Ensuring necessary packages");
        foreach (var package in data.Children)
        {
            // Blacklist children that aren't objects
            if (!package.HasChildren) {
                continue;
            }

            // Skip the bootstrapper package
            if (package.TryGetChild("IsBootstrapperPackage", out KVObject? isBootstrapperPackage)) {
                if (isBootstrapperPackage.GetValueAsBool()) {
                    continue;
                }
            }

            // We blacklist some packages since they aren't useful
            if (IsPackageBlacklisted(package.Name)) {
                continue;
            }

            if (!package.HasChild("file")) {
                continue;
            }

            if (!package.HasChild("sha2")) {
                continue;
            }

            if (!package.HasChild("size")) {
                continue;
            }

            string specialVersion = "";
            KVObject packageToDownload = package;
            if (OperatingSystem.IsWindowsVersionAtLeast(10)) {
                // Some packages have windows 10 versions
                if (package.TryGetChild("win10-64", out KVObject? win10)) {
                    specialVersion = "win10-64";
                    packageToDownload = win10;
                }
            } else if (OperatingSystem.IsWindowsVersionAtLeast(7)) {
                // Some packages have windows 7 versions
                if (package.TryGetChild("win7-64", out KVObject? win7)) {
                    specialVersion = "win7-64";
                    packageToDownload = win7;
                }
            } // There's also Windows 8 versions, but nobody uses W8 so it shouldn't be a problem


            string file = packageToDownload.GetChild("file")!.GetValueAsString();
            string url = BaseURL + file;
            string sha2_expected = packageToDownload.GetChild("sha2")!.GetValueAsString().ToUpperInvariant();
            long size_expected = packageToDownload.GetChild("size")!.GetValueAsLong();
            string saveLocation = Path.Combine(PackageDir, file);

            // Download the file if it doesn't exist
            if (!File.Exists(saveLocation)) {
                using (var stream = new FileStream(saveLocation, FileMode.Create, FileAccess.Write, FileShare.None)) {
                    progressHandler.SetSubOperation($"Downloading {package.Name}{(string.IsNullOrEmpty(specialVersion) ? "" : ' ' + specialVersion)}");
                    await Client.HttpClient.DownloadAsync(url, stream, progressHandler, size_expected, default);
                }
            }

            // Verify the SHA2
            bool verifySucceeded = false;
            using (SHA256 SHA256 = SHA256.Create())
            {
                progressHandler.SetSubOperation($"Verifying {package.Name}");
                using (var stream = new FileStream(saveLocation, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    var sha2_calculated = Convert.ToHexString(SHA256.ComputeHash(stream));
                    verifySucceeded = sha2_calculated == sha2_expected;
                }
            }

            // Add to array if successful
            if (verifySucceeded)
            {
                downloadedPackages[package.Name] = saveLocation;
            } else {
                // If not, add to failed array and delete
                // Bootstrapper will be rerun if atleast one file is failed
                verificationFailed.Add(package.Name);
                File.Delete(saveLocation);
            }
        }

        // Redownload if any packages or files fail verification
        if (verificationFailed.Count > 0) {
            if (RetryCount == 5) {
                string failed = "";
                foreach (var corruptedPackage in verificationFailed)
                {
                    failed += corruptedPackage + " ";
                }
                throw new Exception($"Some files ({failed.TrimEnd()}) were still corrupted after attempting to redownload {RetryCount} times. Check your disk and internet. ");
            }
            RetryCount++;
            await RunBootstrap(msgBoxProvider);
            return;
        }
    }

    private readonly static ReadOnlyCollection<string> blacklistedFiles = new(new List<string>() {
       
    });

    private async Task ExtractPackages(IExtendedProgress<int> progressHandler) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.ExtractPackages");

        // Extract all the packages
        progressHandler.SetOperation("Extracting packages");
        bootstrapperState.InstalledFiles.Clear();
        foreach (var zip in downloadedPackages)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zip.Value))
            {
                await archive.ExtractToDirectory(installManager.InstallDir, progressHandler, blacklistedFiles, (ZipArchiveEntry entry, string name) => {
                    bootstrapperState.InstalledFiles[name] = entry.Length;
                });
            }
        }

        progressHandler.SetOperation("Extracted packages");
    }
    
    [SupportedOSPlatform("linux")] 
    private async Task CheckSteamRuntime(IExtendedProgress<int> progressHandler) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.CheckSteamRuntime");
        await CheckSteamRuntimeSingle(progressHandler, Ubuntu12_32Dir);
        try
        {
            await CheckSteamRuntimeSingle(progressHandler, Ubuntu12_64Dir, "sniper");
            await CheckSteamRuntimeSingle(progressHandler, Ubuntu12_64Dir, "heavy");
        }
        catch (System.Exception e)
        {
            logger.Warning("Failed to process optional runtimes (sniper/heavy):");
            logger.Warning(e);
        }
    }

    [SupportedOSPlatform("linux")]
    private async Task CheckSteamRuntimeSingle(IExtendedProgress<int> progressHandler, string rootPath, string flavour = "") {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.CheckSteamRuntimeSingle");

        string flavourPrefix = string.Empty;

        if (!string.IsNullOrEmpty(flavour)) {
            flavourPrefix = $"-{flavour}";
        }

        // Vanilla is not real, we use it to refer to the regular 32-bit runtime
        if (string.IsNullOrEmpty(flavour)) {
            flavour = "vanilla";
        }

        progressHandler.SetOperation($"Processing Steam Runtime ({flavour})");

        string runtimeDir = Path.Combine(rootPath, $"steam-runtime{flavourPrefix}");
        
        progressHandler.SetThrobber();
        progressHandler.SetSubOperation("Checking for runtime version change...");

        bool extractRuntime = false;
        byte[] checksumBytes;
        var ckFile = Path.Combine(rootPath, $"steam-runtime{flavourPrefix}.tar.xz.checksum");
        if (!File.Exists(ckFile)) {
            var verFile = Path.Combine(rootPath, $"steam-runtime{flavourPrefix}.version.txt");
            checksumBytes = File.ReadAllBytes(verFile);
        } else {
            checksumBytes = File.ReadAllBytes(ckFile);
        }

        string runtime_checksum_md5_new = Convert.ToHexString(MD5.HashData(checksumBytes));
        if (!bootstrapperState.LinuxRuntimeChecksums.TryGetValue(flavour, out string? runtime_checksum_md5_old)) {
            logger.Info("Extract runtime due to: No saved checksum");
            extractRuntime = true;
        } else {
            extractRuntime = !(runtime_checksum_md5_new == runtime_checksum_md5_old);
            if (extractRuntime) {
                logger.Info("Extract runtime due to: Checksum mismatch");
            }
        }

        var setupScriptPath = Path.Combine(runtimeDir, "setup.sh");
        bool hasSetupScript = File.Exists(setupScriptPath);

        if (extractRuntime) {
            await ExtractSteamRuntime(progressHandler, rootPath, flavourPrefix);

            // If everything succeeds, record current hash to file
            bootstrapperState.LinuxRuntimeChecksums[flavour] = runtime_checksum_md5_new;
        }

        // SLR sniper has no setup script. Strange but let's handle that too.
        if (hasSetupScript)
        {
            // Always run setup.sh (if it exists)
            using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.CheckSteamRuntimeSingle - Runtime setup.sh");
            progressHandler.SetThrobber();
            progressHandler.SetSubOperation("Running runtime setup...");

            Process proc = new();
            proc.StartInfo.FileName = setupScriptPath;
            proc.StartInfo.Arguments = "";
            proc.StartInfo.WorkingDirectory = runtimeDir;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();

            await proc.WaitForExitAsync();

            // Only run pinning if we extracted a new runtime
            if (extractRuntime) {
                progressHandler.SetSubOperation("Pinning runtime libs");

                Process procpin = new();
                procpin.StartInfo.FileName = setupScriptPath;
                procpin.StartInfo.Arguments = "--force";
                procpin.StartInfo.WorkingDirectory = runtimeDir;
                procpin.StartInfo.CreateNoWindow = true;
                procpin.StartInfo.UseShellExecute = true;
                procpin.Start();
            }
        }
    }

    [SupportedOSPlatform("linux")]
    private async Task ExtractSteamRuntime(IExtendedProgress<int> progressHandler, string rootPath, string flavourPrefix) {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.ExtractSteamRuntime");
        string runtimeDir = Path.Combine(rootPath, $"steam-runtime{flavourPrefix}");

        Directory.CreateDirectory(runtimeDir);

        // Verify checksums if there's a checksum file. Not all runtimes come with one, so it's not a required check.
        Dictionary<string, string> checksums = new();

        var ckFile = Path.Combine(rootPath, $"steam-runtime{flavourPrefix}.tar.xz.checksum");
        if (File.Exists(ckFile)) {
            var lines = File.ReadLines(ckFile);
            foreach (var line in lines)
            {
                var split = line.Split("  ");
                checksums.Add(split[1].Trim(), split[0].Trim());
            }

            // Verify files defined in steam-runtime.checksum
            foreach (var item in checksums)
            {
                var file = Path.Combine(rootPath, item.Key);
                string runtime_md5_expected = item.Value;

                string runtime_md5_calculated = Convert.ToHexString(MD5.HashData(File.ReadAllBytes(file)));

                if (!string.Equals(runtime_md5_calculated, runtime_md5_expected, StringComparison.InvariantCultureIgnoreCase)) {
                    logger.Error($"SteamRT file {item.Key} hash verification failure, got: {runtime_md5_calculated}, expected: {runtime_md5_expected}");
                    throw new Exception($"MD5 mismatch. Steam Runtime File {file} is corrupted. {runtime_md5_expected} expected, got {runtime_md5_calculated}");
                } else {
                    logger.Info("SteamRT file " + item.Key + " hash verification success");
                }
            }
        }
        
        {
            using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.ExtractSteamRuntime - Unzip");

            Process proc = new();
            proc.StartInfo.FileName = "tar";
            proc.StartInfo.Arguments = $"-xvJf steam-runtime{flavourPrefix}.tar.xz -C steam-runtime{flavourPrefix} --strip-components=1";
            proc.StartInfo.WorkingDirectory = rootPath;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;

            progressHandler.SetThrobber();
            progressHandler.SetSubOperation($"Unzipping Steam Runtime");

            logger.Info($"Starting tar");
            proc.Start();

            logger.Info($"Waiting for tar to exit...");
            await proc.WaitForExitAsync();
            logger.Info($"tar exited with code {proc.ExitCode}");

            if (proc.ExitCode != 0)  {
                throw new Exception("tar exited with failure exitcode: " + proc.ExitCode);
            }
        }

        progressHandler.SetProgress(progressHandler.MaxProgress);
    }

    private void CopyOpensteamFiles(IExtendedProgress<int> progressHandler) {
        using var subScope = CProfiler.CurrentProfiler?.EnterScope("Bootstrapper.CopyOpensteamFiles");

        // By default we copy all files to the root install dir.
        // Specify path mappings here to tell the files to go into another
        Dictionary<string, string> pathMappings = new() {
            {"reaper", "linux64/reaper"},
            {"steam-launch-wrapper", "linux64/steam-launch-wrapper"},
            {"libSDL3.so", "linux64/libSDL3.so"},
            {"libSDL3.so.0", "linux64/libSDL3.so.0"},
            {"libSDL3.so.0.0.0", "linux64/libSDL3.so.0.0.0"},
            {"libSDL3_ttf.so", "linux64/libSDL3_ttf.so"},
            {"libSDL3_ttf.so.0", "linux64/libSDL3_ttf.so.0"},
            {"libSDL3_ttf.so.0.0.0", "linux64/libSDL3_ttf.so.0.0.0"},
            {"steamserviced.exe", "bin/steamserviced.exe"},
            {"steamserviced.pdb", "bin/steamserviced.pdb"},
            {"htmlhost", "ubuntu12_32/htmlhost"},
            {"steamservice.so", "linux64/steamservice.so"},
            {"libaudio.so", "libaudio.so"},
        };

        var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (assemblyFolder == null) {
            throw new Exception("assemblyFolder is null.");
        }
        string platformStr;
        if (OperatingSystem.IsWindows()) {
            platformStr = "win-x64";
        } else if (OperatingSystem.IsMacOS()) {
            platformStr = "osx-x64";
        } else if (OperatingSystem.IsLinux()) {
            platformStr = "linux-x64";
        } else {
            throw new PlatformNotSupportedException("Unsupported OS");
        }

        string baseNativesFolder = Path.Combine(assemblyFolder, "runtimes");
        string nativesFolder = Path.Combine(baseNativesFolder, platformStr, "native");
        if (!Directory.Exists(nativesFolder)) {
            throw new NotSupportedException($"This build has not been compiled with support for {platformStr}. Please rebuild or try another OS. \nAlternatively, if you're running on 64-bit Windows, 64-bit Linux or 64-bit MacOS file an issue if this is a release build.");
        }

        progressHandler.SetSubOperation("Copying OpenSteam files");
        var di = new DirectoryInfo(nativesFolder);
        foreach (var file in di.EnumerateFilesRecursively())
        {
            if (file.Extension == ".lib" || file.Extension == ".exp" || file.Extension == ".a") {
                continue;
            }

            string name = file.Name;
            if (pathMappings.ContainsKey(name)) {
                name = pathMappings[name];
            }

            logger.Info("Copying " + file.FullName + " to " + Path.Combine(installManager.InstallDir, name));
            File.Copy(file.FullName, Path.Combine(installManager.InstallDir, name), true);
        }
    }
}