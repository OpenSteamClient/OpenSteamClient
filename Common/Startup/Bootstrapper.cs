using System;
using System.IO;
using static System.Environment;
using System.IO.Compression;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using OpenSteamworks.Generated;
using ValveKeyValue;
using Common.Extensions;
using System.Security.Cryptography;
using Common.Utils;
using System.Formats.Tar;
using System.Diagnostics;
using System.Runtime.Versioning;
using Common.Managers;
using System.Runtime.InteropServices;

namespace Common.Startup;

public class Bootstrapper {

    //TODO: We shouldn't hardcode this but it will do...
    public const string BaseURL = "https://client-update.akamai.steamstatic.com/";
    public string SteamclientLibPath {
        get {
            if (OperatingSystem.IsLinux()) {
                return Path.Combine(MainBinaryDir, "steamclient.so");
            }

            if (OperatingSystem.IsWindows()) {
                return Path.Combine(MainBinaryDir, "steamclient64.dll");
            }

            if (OperatingSystem.IsMacOS()) {
                return Path.Combine(MainBinaryDir, "steamclient.dylib");
            }

            throw new Exception("Unsupported OS in SteamclientLibPath_get");
        }
    }
    public string PlatformClientManifest {
        get {
            string prefix = "steam_client_";
            if (OperatingSystem.IsWindows()) {
                return prefix + "win32";
            } else if (OperatingSystem.IsLinux()) {
                return prefix + "ubuntu12";
            } else if (OperatingSystem.IsMacOS()) {
                return prefix + "osx";
            }
            
            throw new Exception("Unsupported platform");
        }
    }
    public string PackageDir => Path.Combine(configManager.InstallDir, "package");
    public string MainBinaryDir {
        get {
            if (OperatingSystem.IsLinux()) {
                return Path.Combine(configManager.InstallDir, "linux64");
            }

            return configManager.InstallDir;
        }
    }

    [SupportedOSPlatform("linux")]
    public string Ubuntu12_32Dir => Path.Combine(configManager.InstallDir, "ubuntu12_32");

    [SupportedOSPlatform("linux")]
    public string SteamRuntimeDir => Path.Combine(Ubuntu12_32Dir, "steam-runtime");
    
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

        if (packageName.StartsWith("webkit_")) {
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
    Dictionary<string, string> downloadedPackages = new Dictionary<string, string>();
    public required ConfigManager configManager { protected get; init; }
    private bool restartRequired = false;

    public async Task RunBootstrap(IExtendedProgress<int> progressHandler) {
        progressHandler.SetOperation("Bootstrapping");

        Directory.CreateDirectory(configManager.InstallDir);

        // steamclient blindly dumps certain files to the CWD, so set it to the install dir
        Directory.SetCurrentDirectory(configManager.InstallDir);

        Directory.CreateDirectory(PackageDir);

        // Skip verification and package processing if user requests it
        if (!configManager.BootstrapperState.SkipVerification) {
            if (!VerifyFiles(progressHandler)) {
                await EnsurePackages(progressHandler);
                await ExtractPackages(progressHandler);
            }
        }

        // Run platform specific tasks

        if (OperatingSystem.IsLinux()) {
            // Process the Steam runtime (needed for SteamVR and some tools steam ships with)
            await CheckSteamRuntime(progressHandler);

            if (!configManager.BootstrapperState.LinuxPermissionsSet) {
                progressHandler.SetOperation($"Setting proper permissions (this may freeze)");
                progressHandler.SetThrobber(true);
                // Valve doesn't include permission info in the zips, so chmod them all to allow execute
                await Process.Start("/usr/bin/chmod", "-R +x " + '"' + configManager.InstallDir + '"').WaitForExitAsync();
                
                progressHandler.SetThrobber(false);
                configManager.BootstrapperState.LinuxPermissionsSet = true;
            }

            // Create fakehome
            Directory.CreateDirectory(Path.Combine(configManager.InstallDir, "fakehome"));

            // Create fakehome symlinks
            var fakehomeSubdir = Path.Combine(configManager.InstallDir, "fakehome", "Steam");
            if (!Directory.Exists(fakehomeSubdir))
                Directory.CreateSymbolicLink(fakehomeSubdir, configManager.InstallDir);

            MakeXDGCompliant();
        }

        if (OperatingSystem.IsWindows()) {
            //TODO: test for steamservice, install if not installed
        }
            
        progressHandler.SetOperation($"Finalizing");
        progressHandler.SetThrobber(true);

        // Copy/Link our files over (steamserviced, 64-bit reaper and 64-bit steamlaunchwrapper, other platform specific niceties)
        CopyOpensteamFiles(progressHandler);

        configManager.BootstrapperState.CommitHash = GitInfo.GitCommit;
        configManager.BootstrapperState.InstalledVersion = OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION;
        configManager.FlushToDisk();

        await FinishBootstrap(progressHandler);
    }

    private async Task FinishBootstrap(IExtendedProgress<int> progressHandler) {
        // Currently only linux needs a restart (for LD_PRELOAD and LD_LIBRARY_PATH)
        var hasReran = GetEnvironmentVariable("OPENSTEAM_RAN_EXECVP") == "1";
        restartRequired = OperatingSystem.IsLinux() && !hasReran;

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
                        Console.WriteLine("Waiting for debugger...");
                        System.Threading.Thread.Sleep(500);
                    }
                });
            }
        }
    }

    [SupportedOSPlatform("linux")]
    private void MakeXDGCompliant()
    {
        var logsSymlink = Path.Combine(configManager.InstallDir, "logs");
        if (!Directory.Exists(logsSymlink))
            Directory.CreateSymbolicLink(logsSymlink, configManager.LogsDir);

        var configSymlink = Path.Combine(configManager.InstallDir, "config");
        if (!Directory.Exists(configSymlink))
            Directory.CreateSymbolicLink(configSymlink, configManager.ConfigDir);

        var cacheSymlink = Path.Combine(configManager.InstallDir, "appcache");
        if (!Directory.Exists(cacheSymlink))
            Directory.CreateSymbolicLink(cacheSymlink, configManager.CacheDir);
    }

    [SupportedOSPlatform("linux")]
    private void ReExecWithEnvs(bool withDebugger) {
        [DllImport("libc")]
        static extern int execvp([MarshalAs(UnmanagedType.LPUTF8Str)] string file, [MarshalAs(UnmanagedType.LPArray)] string?[] args);

        // C#'s SetEnvironmentVariable doesn't work here, but we might as well use this since we're in linux specific code anyway
        [DllImport("libc")]
        static extern int setenv([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, int overwrite);

        Console.WriteLine("Re-execing");

        // Unfortunately we can't kindly coerce the steamclient to not use $HOME/.steam/Steam, and we can't just set HOME as it'd fuck with games as well
        // So instead we made a LD_PRELOADable shim that contains a getenv hook and some bootstrapper functions
        // Now the steamclient always calls SteamBootstrapper_GetEUniverse as one of the first functions, which is when we take the memory map (all loaded modules and their start-end addresses)
        // And then when something hits getenv we check if it's caller address is in the memory of steamclient.so
        // If it is, then we fake the HOME dir to be inside .local/share/OpenSteam/fakehome
        // Where steamclient will then dump all it's files into fakehome/Steam (for some reason, instead of fakehome/.steam/steam)
        File.Copy(Path.Combine(configManager.InstallDir, "libbootstrappershim.so"), "/tmp/opensteambootstrappershim.so", true);

        if (withDebugger) {
            setenv("OPENSTEAM_REATTACH_DEBUGGER", "1", 1);
        }
        setenv("OPENSTEAM_RAN_EXECVP", "1", 1);
        setenv("LD_PRELOAD", $"/tmp/opensteambootstrappershim.so:{GetEnvironmentVariable("LD_PRELOAD")}", 1);
        setenv("LD_LIBRARY_PATH", $"{Path.Combine(configManager.InstallDir, "ubuntu12_64")}:{Path.Combine(configManager.InstallDir, "ubuntu12_32")}:{Path.Combine(configManager.InstallDir)}:{GetEnvironmentVariable("LD_LIBRARY_PATH")}", 1);

        string?[] fullArgs = Environment.GetCommandLineArgs();

        string executable = Directory.ResolveLinkTarget("/proc/self/exe", false)!.FullName;
        Console.WriteLine("executable: " + executable);
        if (!executable.EndsWith("dotnet")) {
            fullArgs[0] = executable;
        } else {
            fullArgs = fullArgs.Prepend(executable).Append(null).ToArray();
        }
        
        foreach (var item in fullArgs)
        {
            Console.WriteLine("item: " + item);
        }

        // Program execution ends here, if execvp returns, it means re-execution failed
        int ret = execvp(executable, fullArgs);
        throw new Exception($"Execvp failed: {ret}");
    }
    
    private bool VerifyFiles(IExtendedProgress<int> progressHandler) {
        // Verify all files and skip this step if files are valid and version matches 
        bool failedInitialCheck = configManager.BootstrapperState.InstalledVersion != OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION || configManager.BootstrapperState.CommitHash != GitInfo.GitCommit;
        int installedFilesLength = configManager.BootstrapperState.InstalledFiles.Count;
        int checkedFiles = 0;

        progressHandler.SetOperation("Checking files");

        // Convert absolute progress (files checked) into relative progress (0% - 100%)
        var relativeProgress = new Progress<long>(totalFiles => progressHandler.Report((int)(totalFiles / checkedFiles) * 100));
        
        foreach (var installedFile in configManager.BootstrapperState.InstalledFiles)
        {
            checkedFiles++;
            var info = new FileInfo(Path.Combine(configManager.InstallDir, installedFile.Key));
            if (info.Exists) {
                if (info.Length != installedFile.Value) {
                    failedInitialCheck = true;
                }
            } else {
                failedInitialCheck = true;
            }
           
            if (failedInitialCheck) {
                break;
            }
        }   

        // Remove packages only in case of an upgrade
        if (configManager.BootstrapperState.InstalledVersion != 0 && (configManager.BootstrapperState.InstalledVersion != OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION)) {
            Directory.Delete(PackageDir, true);
            Directory.CreateDirectory(PackageDir);

        }

        if (installedFilesLength > 0 && !failedInitialCheck) {
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


                string url = BaseURL + package["file"].ToString();
                string sha2_expected = package["sha2"].ToString()!.ToUpperInvariant();
                long size_expected = package["size"].ToInt64(default)!;
                string saveLocation = Path.Combine(PackageDir, package["file"].ToString()!);

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
                            progressHandler.SetSubOperation($"Downloading {package.Name}");
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
            await RunBootstrap(progressHandler);
            return;
        }
    }

    private async Task ExtractPackages(IExtendedProgress<int> progressHandler) {
        // Extract all the packages
        progressHandler.SetOperation("Extracting packages");
        foreach (var zip in downloadedPackages)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zip.Value))
            {
                await archive.ExtractToDirectory(configManager.InstallDir, progressHandler, (ZipArchiveEntry entry, string name) => {
                    configManager.BootstrapperState.InstalledFiles.Add(name, entry.Length);
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
        var runtime_checksum_md5_old = configManager.BootstrapperState.LinuxRuntimeChecksum;
        bool extractRuntime = !(runtime_checksum_md5_new == runtime_checksum_md5_old);

        var setupScriptPath = Path.Combine(SteamRuntimeDir, "setup.sh");
        if (!File.Exists(setupScriptPath)) {
            extractRuntime = true;
        }
        
        if (extractRuntime) {
            await ExtractSteamRuntime(progressHandler);

            // If everything succeeds, record current hash to file
            configManager.BootstrapperState.LinuxRuntimeChecksum = runtime_checksum_md5_new;
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
        var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (assemblyFolder == null) {
            throw new Exception("assemblyFolder is null.");
        }
        string platformStr = Utils.UtilityFunctions.GetPlatformString();
        string baseNativesFolder = Path.Combine(assemblyFolder, "Natives");
        string nativesFolder = Path.Combine(baseNativesFolder, platformStr);
        if (!Directory.Exists(nativesFolder)) {
            throw new NotSupportedException($"This build has not been compiled with support for {platformStr}. Please rebuild or try another OS.");
        }

        var oldTimestamp = configManager.BootstrapperState.NativeBuildDate;
        var newTimestamp = Convert.ToUInt32(File.ReadAllText(Path.Combine(baseNativesFolder, "build_timestamp")));
        if (newTimestamp > oldTimestamp) {
            progressHandler.SetSubOperation("Copying OpenSteam files");
            var di = new DirectoryInfo(nativesFolder);
            di.CopyFilesRecursively(new DirectoryInfo(configManager.InstallDir), true);
            configManager.BootstrapperState.NativeBuildDate = newTimestamp;
        }
    }
}