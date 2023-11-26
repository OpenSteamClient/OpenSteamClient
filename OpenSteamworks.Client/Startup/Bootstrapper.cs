using System;
using System.IO;
using static System.Environment;
using System.IO.Compression;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using OpenSteamworks.Generated;
using ValveKeyValue;
using OpenSteamworks.Client.Extensions;
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

namespace OpenSteamworks.Client.Startup;

//TODO: this whole thing needs a rewrite badly
public class Bootstrapper : IClientLifetime {

    //TODO: We shouldn't hardcode this but it will do...
    public const string BaseURL = "https://client-update.akamai.steamstatic.com/";
    private InstallManager installManager;
    public string SteamclientLibPath {
        get {
            return Path.Combine(MainBinaryDir, OSSpecifics.Instance.SteamClientBinaryName);
        }
    }
    public string PlatformClientManifest {
        get {
            return OSSpecifics.Instance.SteamClientManifestName;
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
    public string SteamRuntimeDir => Path.Combine(Ubuntu12_32Dir, "steam-runtime");
    
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

        if (packageName.StartsWith("public_")) {
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

    public Bootstrapper(InstallManager installManager, BootstrapperState bootstrapperState) {
        this.logger = Logger.GetLogger("Bootstrapper", installManager.GetLogPath("Bootstrapper"));
        this.installManager = installManager;
        this.bootstrapperState = bootstrapperState;
    }

    private async Task RunBootstrap() {
        if (progressHandler == null) {
            progressHandler = new ExtendedProgress<int>(0, 100);
        }

        progressHandler.SetOperation("Bootstrapping");

        Directory.CreateDirectory(installManager.InstallDir);

        // steamclient blindly dumps certain files to the CWD, so set it to the install dir
        Directory.SetCurrentDirectory(installManager.InstallDir);

        Directory.CreateDirectory(PackageDir);

        // Skip verification and package processing if user requests it
        if (!bootstrapperState.SkipVerification) {
            if (!VerifyFiles(progressHandler, out IEnumerable<string> failureReason)) {
                logger.Error("Failed verification: " + string.Join(", ", failureReason));
                await EnsurePackages(progressHandler);
                await ExtractPackages(progressHandler);
            }
        }

        // Run platform specific tasks
        if (OperatingSystem.IsWindows()) {
            // Write our PID to HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam (has full access to entire Steam key by all users, security nightmare?)
            //TODO: doesn't support machines where ValveSteam hasn't been installed atleast once
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
                progressHandler.SetOperation($"Setting proper permissions (this may freeze)");
                progressHandler.SetThrobber(true);
                // Valve doesn't include permission info in the zips, so chmod them all to allow execute
                await Process.Start("/usr/bin/chmod", "-R +x " + '"' + installManager.InstallDir + '"').WaitForExitAsync();

                progressHandler.SetThrobber(false);
                bootstrapperState.LinuxPermissionsSet = true;
            }

            //TODO: check for steam some other way (like trying to connect)
            Process[] runningSteamProcesses = Process.GetProcessesByName("steam");
            if (runningSteamProcesses.Length == 0) {
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
                    // bin points to bin32 on valve's install, we're 64-bit so we should probably point to bin64
                    ("bin", Path.Combine(installManager.DatalinkDir, "bin64")),
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

                File.WriteAllText(Path.Combine(installManager.DatalinkDir, "steam.pid"), Environment.ProcessId.ToString());
            }
        }
            
        progressHandler.SetOperation($"Finalizing");
        progressHandler.SetThrobber(true);

        // Copy/Link our files over (steamserviced, 64-bit reaper and 64-bit steamlaunchwrapper, other platform specific niceties)
        CopyOpensteamFiles(progressHandler);

        bootstrapperState.CommitHash = GitInfo.GitCommit;
        bootstrapperState.InstalledVersion = OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION;
        bootstrapperState.Save();

        await FinishBootstrap(progressHandler);
    }

    private async Task FinishBootstrap(IExtendedProgress<int> progressHandler) {
        // Currently only linux needs a restart (for LD_PRELOAD and LD_LIBRARY_PATH)
        var hasReran = GetEnvironmentVariable("OPENSTEAM_RAN_EXECVP") == "1";
        restartRequired = OperatingSystem.IsLinux() && !hasReran;
        
        SetEnvsForSteamLoad();
        progressHandler.SetOperation("Bootstrapping Completed" + (restartRequired ? ", restarting" : ""));

        await RestartIfNeeded(progressHandler);
    }

    private async Task RestartIfNeeded(IExtendedProgress<int> progressHandler) {
        bool hadDebugger = false;
        bool debuggerShouldReattach = GetEnvironmentVariable("OPENSTEAM_REATTACH_DEBUGGER") == "1";

        if (restartRequired) {
            // Can't forcibly detach the debugger, which is needed since:
            // - execvp breaks debugger, but is supposedly not a problem
            // - child processes also can't be debugged, which is also apparently not a problem
            // So that leaves us no way to "transfer" the debugger from the old process to the new one, unlike with the old C++ solution where gdb just does it when execvp:ing
            if (Debugger.IsAttached) {
                hadDebugger = true;
                progressHandler.SetOperation("Please detach your debugger. ");
                progressHandler.SetSubOperation("You should re-attach once the process has restarted.");
                Debugger.Log(5, "DetachDebugger", "Please detach your debugger. You should re-attach once the process has restarted.");
                await Task.Run(() =>
                {
                    while (Debugger.IsAttached)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                });
            }

            if (OperatingSystem.IsLinux()) {
                this.ReExecWithEnvs(hadDebugger);
            }
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
        [DllImport("libc")]
        static extern int execvp([MarshalAs(UnmanagedType.LPUTF8Str)] string file, [MarshalAs(UnmanagedType.LPArray)] string?[] args);

        logger.Info("Re-execing");

        if (withDebugger) {
            UtilityFunctions.SetEnvironmentVariable("OPENSTEAM_REATTACH_DEBUGGER", "1");
        }
        UtilityFunctions.SetEnvironmentVariable("OPENSTEAM_RAN_EXECVP", "1");
        UtilityFunctions.SetEnvironmentVariable("LD_LIBRARY_PATH", $"{Path.Combine(installManager.InstallDir, "ubuntu12_64")}:{Path.Combine(installManager.InstallDir, "ubuntu12_32")}:{Path.Combine(installManager.InstallDir)}:{GetEnvironmentVariable("LD_LIBRARY_PATH")}");

        string?[] fullArgs = Environment.GetCommandLineArgs();

        string executable = Directory.ResolveLinkTarget("/proc/self/exe", false)!.FullName;
        logger.Debug("Re-exec executable: " + executable);
        if (!executable.EndsWith("dotnet")) {
            fullArgs[0] = executable;
        } else {
            fullArgs = fullArgs.Prepend(executable).Append(null).ToArray();
        }
        
        foreach (var item in fullArgs)
        {
            logger.Debug("Re-exec argument: " + item);
        }

        // Program execution ends here, if execvp returns, it means re-execution failed
        int ret = execvp(executable, fullArgs);
        throw new Exception($"Execvp failed: {ret}");
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
            progressHandler.SetProgress(100);
            return true;
        }
        
        return false;
    }

    private async Task EnsurePackages(IExtendedProgress<int> progressHandler) {
        downloadedPackages.Clear();

        // Fetch the manifests from Common.dll
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        IFileInfo fileInfo = embeddedProvider.GetFileInfo($"{PlatformClientManifest}.vdf");
        if (!fileInfo.Exists) {
            throw new Exception($"Cannot find {PlatformClientManifest}.vdf as an embedded resource.");
        }

        List<string> verificationFailed = new List<string>();
        using (var reader = fileInfo.CreateReadStream())
        {
            var kv = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
            KVObject data = kv.Deserialize(reader);

            progressHandler.SetOperation("Ensuring necessary packages");
            foreach (var package in data.Children)
            {
                // Blacklist children that aren't objects
                if (package.Count() < 1) {
                    continue;
                }

                // Skip the bootstrapper package
                if (package["IsBootstrapperPackage"] != null && ((int)package["IsBootstrapperPackage"]) == 1) {
                    continue;
                }

                // We blacklist some packages since they aren't useful
                if (IsPackageBlacklisted(package.Name)) {
                    continue;
                }

                if (package["file"] == null) {
                    continue;
                }

                if (package["sha2"] == null) {
                    continue;
                }

                if (package["size"] == null) {
                    continue;
                }

                string specialVersion = "";
                dynamic packageToDownload = package;
                if (OperatingSystem.IsWindowsVersionAtLeast(10)) {
                    // Some packages have windows 10 versions
                    if (package["win10-64"] != null) {
                        specialVersion = "win10-64";
                        packageToDownload = package["win10-64"];
                    }
                } else if (OperatingSystem.IsWindowsVersionAtLeast(7)) {
                    // Some packages have windows 7 versions
                    if (package["win7-64"] != null) {
                        specialVersion = "win7-64";
                        packageToDownload = package["win7-64"];
                    }
                } // There's also Windows 8 versions, but nobody uses W8 so it shouldn't be a problem
                


                string url = BaseURL + (string)packageToDownload["file"];
                string sha2_expected = ((string)packageToDownload["sha2"]).ToUpperInvariant();
                long size_expected = (Int64)packageToDownload["size"];
                string saveLocation = Path.Combine(PackageDir, (string)packageToDownload["file"]);

                // Download the file if it doesn't exist
                if (!File.Exists(saveLocation)) {
                    // Start the download
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.ConnectionClose = true;
                        client.DefaultRequestHeaders.Add("User-Agent", $"opensteamclient {GitInfo.GitBranch}/{GitInfo.GitCommit}");
                        
                        // Create a file stream to store the downloaded data.
                        // This really can be any type of writeable stream.
                        using (var file = new FileStream(saveLocation, FileMode.Create, FileAccess.Write, FileShare.None)) {
                            progressHandler.SetSubOperation($"Downloading {package.Name}{(string.IsNullOrEmpty(specialVersion) ? "" : ' ' + specialVersion)}");
                            // Use the custom extension method below to download the data.
                            // The passed progress-instance will receive the download status updates.
                            await client.DownloadAsync(url, file, progressHandler, size_expected, default);
                        }
                    }
                }

                // Verify the SHA2
                bool verifySucceeded = false;
                using (SHA256 SHA256 = SHA256.Create())
                {
                    progressHandler.SetSubOperation($"Verifying {package.Name}");
                    using (var file = new FileStream(saveLocation, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        var sha2_calculated = Convert.ToHexString(SHA256.ComputeHash(file));
                        verifySucceeded = sha2_calculated == sha2_expected;
                    }
                }   
                
                // Add to array if successful
                if (verifySucceeded) {
                    downloadedPackages.Add(package.Name, saveLocation);
                } else {
                    // If not, add to failed array and delete
                    // Bootstrapper will be rerun if atleast one file is failed
                    verificationFailed.Add(package.Name);
                    File.Delete(saveLocation);
                }
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
            await RunBootstrap();
            return;
        }
    }

    private async Task ExtractPackages(IExtendedProgress<int> progressHandler) {
        // Extract all the packages
        progressHandler.SetOperation("Extracting packages");
        bootstrapperState.InstalledFiles.Clear();
        foreach (var zip in downloadedPackages)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zip.Value))
            {
                await archive.ExtractToDirectory(installManager.InstallDir, progressHandler, (ZipArchiveEntry entry, string name) => {
                    bootstrapperState.InstalledFiles.Add(name, entry.Length);
                });  
            } 
        }
    }
    [SupportedOSPlatform("linux")] 
    private async Task CheckSteamRuntime(IExtendedProgress<int> progressHandler) {
        progressHandler.SetOperation($"Processing Steam Runtime");
        progressHandler.SetThrobber(true);

        progressHandler.SetSubOperation("Checking for runtime version change...");

        var runtimeChecksumBytes = File.ReadAllBytes(Path.Combine(Ubuntu12_32Dir, "steam-runtime.checksum"));
        var runtime_checksum_md5_new = Convert.ToHexString(MD5.HashData(runtimeChecksumBytes));
        var runtime_checksum_md5_old = bootstrapperState.LinuxRuntimeChecksum;
        bool extractRuntime = !(runtime_checksum_md5_new == runtime_checksum_md5_old);

        var setupScriptPath = Path.Combine(SteamRuntimeDir, "setup.sh");
        if (!File.Exists(setupScriptPath)) {
            extractRuntime = true;
        }
        
        if (extractRuntime) {
            await ExtractSteamRuntime(progressHandler);

            // If everything succeeds, record current hash to file
            bootstrapperState.LinuxRuntimeChecksum = runtime_checksum_md5_new;
        }

        // Always run setup.sh
        progressHandler.SetThrobber(true);
        progressHandler.SetSubOperation("Running runtime setup...");

        Process proc = new Process();
        proc.StartInfo.FileName = setupScriptPath;
        proc.StartInfo.Arguments = "";
        proc.StartInfo.WorkingDirectory = SteamRuntimeDir;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.UseShellExecute = true;
        proc.Start();

        await proc.WaitForExitAsync();
    }

    [SupportedOSPlatform("linux")]
    private async Task ExtractSteamRuntime(IExtendedProgress<int> progressHandler) {
        progressHandler.SetSubOperation($"Combining Steam Runtime parts");

        Directory.CreateDirectory(SteamRuntimeDir);

        // Create a place in memory to store the runtime for combining and unzipping
        using (var fullFile = new MemoryStream()) {
            // First get all the parts
            List<string> parts = new(Directory.EnumerateFiles(Ubuntu12_32Dir, "steam-runtime.tar.xz.part*"));
            
            // Sort them to get an order like part0, part1, part2, part3
            parts.Sort();

            // Then combine all the parts to one zip file 
            foreach (var part in parts)
            {
                using (var file = new FileStream(part, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                    file.CopyTo(fullFile);
                }
            }

            // Seek to the beginning so we can read
            fullFile.Seek(0, SeekOrigin.Begin);

            // Get checksums from the checksum file
            Dictionary<string, string> checksums = new();

            var lines = File.ReadLines(Path.Combine(Ubuntu12_32Dir, "steam-runtime.checksum"));
            foreach (var line in lines)
            {
                var split = line.Split("  ");
                checksums.Add(split[1].Trim(), split[0].Trim());
            }

            // Verify files defined in steam-runtime.checksum
            foreach (var item in checksums)
            {
                var file = Path.Combine(Ubuntu12_32Dir, item.Key);
                string runtime_md5_calculated = "";
                string runtime_md5_expected = item.Value.ToUpper();

                // This file is never saved on disk, so do it specially
                if (file.EndsWith("steam-runtime.tar.xz")) {
                    runtime_md5_calculated = Convert.ToHexString(MD5.HashData(fullFile));
                } else {
                    runtime_md5_calculated = Convert.ToHexString(MD5.HashData(File.ReadAllBytes(file)));
                }

                runtime_md5_calculated = runtime_md5_calculated.ToUpper();

                if (runtime_md5_calculated != runtime_md5_expected) {
                    if (file.EndsWith("steam-runtime.tar.xz")) {
                        file += " (saved in-memory)";
                    }
                    throw new Exception($"MD5 mismatch. Steam Runtime File {file} is corrupted. {runtime_md5_expected} expected, got {runtime_md5_calculated}");
                }
            }

            Process proc = new Process();
            proc.StartInfo.FileName = "tar";
            proc.StartInfo.Arguments = "-xJ";
            proc.StartInfo.WorkingDirectory = Ubuntu12_32Dir;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            progressHandler.SetSubOperation($"Unzipping Steam Runtime");
            progressHandler.SetThrobber(false);

            bool result = proc.Start();

            if (!result) {
                throw new Exception("Failed to start tar. Is tar installed?");
            }

            // Seek to the beginning again, just in case
            fullFile.Seek(0, SeekOrigin.Begin);

            var outPiper = StreamPiper<Stream, Stream>.CreateAndStartPiping(proc.StandardOutput.BaseStream, Console.OpenStandardOutput());
            var errPiper = StreamPiper<Stream, Stream>.CreateAndStartPiping(proc.StandardError.BaseStream, Console.OpenStandardError());
            var inPiper = StreamPiper<MemoryStream, Stream>.CreateAndStartPiping(fullFile, proc.StandardInput.BaseStream);

            // Convert absolute progress (bytes streamed) into relative progress (0% - 100%)
            var length = inPiper.Source.Length;
            var relativeProgress = new Progress<long>(bytesStreamed => progressHandler.Report((int)((float)((float)bytesStreamed / (float)length)*100)));

            inPiper.StreamPositionChanged += (object? sender, EventArgs args) => {
                (relativeProgress as IProgress<long>).Report(inPiper.Source.Position);
                if (length == inPiper.Source.Position) {
                    // No way to send a SIGTERM instead of a SIGKILL
                    proc.Kill();
                }
            };

            await proc.WaitForExitAsync();

            // The exitcode is 137 when we explicitly kill above
            if (proc.ExitCode != 137)  {
                throw new Exception("tar exited with unknown exitcode: " + proc.ExitCode);
            }

            // For some reason the streams are still readable after the process terminates, so stop pipers explicitly
            outPiper.StopPiping();
            errPiper.StopPiping();
            inPiper.StopPiping();

            progressHandler.SetProgress(100);
        }
    }

    private void CopyOpensteamFiles(IExtendedProgress<int> progressHandler) {
        // By default we copy all files to the root install dir.
        // Specify path mappings here to tell the files to go into another
        Dictionary<string, string> pathMappings = new() {
            {"reaper", "linux64/reaper"},
            {"libSDL3.so", "linux64/libSDL3.so"},
            {"libSDL3.so.0", "linux64/libSDL3.so.0"},
            {"libSDL3.so.0.0.0", "linux64/libSDL3.so.0.0.0"},
            {"libSDL3_ttf.so", "linux64/libSDL3_ttf.so"},
            {"libSDL3_ttf.so.0", "linux64/libSDL3_ttf.so.0"},
            {"libSDL3_ttf.so.0.0.0", "linux64/libSDL3_ttf.so.0.0.0"},
            {"steam-launch-wrapper", "linux64/reaper"},
            {"steamserviced.exe", "bin/steamserviced.exe"},
            {"steamserviced.pdb", "bin/steamserviced.pdb"},
            {"htmlhost", "ubuntu12_32/htmlhost"},
            {"steamservice.so", "linux64/steamservice.so"},
        };

        var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (assemblyFolder == null) {
            throw new Exception("assemblyFolder is null.");
        }
        string platformStr = UtilityFunctions.GetPlatformString();
        string baseNativesFolder = Path.Combine(assemblyFolder, "Natives");
        string nativesFolder = Path.Combine(baseNativesFolder, platformStr);
        if (!Directory.Exists(nativesFolder)) {
            throw new NotSupportedException($"This build has not been compiled with support for {platformStr}. Please rebuild or try another OS.");
        }

        var oldTimestamp = bootstrapperState.NativeBuildDate;
        var newTimestamp = Convert.ToUInt32(File.ReadAllText(Path.Combine(baseNativesFolder, "build_timestamp")));
        //TODO: we force this copy every time for now due to files not sometimes copying over
        if (true || newTimestamp > oldTimestamp) {
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
            bootstrapperState.NativeBuildDate = newTimestamp;
        }
    }

    public async Task RunStartup()
    {
        await RunBootstrap();
    }

    public async Task RunShutdown()
    {
        await Task.CompletedTask;
    }
}