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

namespace Common.Startup;

public class Bootstrapper {

    //TODO: We shouldn't hardcode this but it will do...
    public const string BaseURL = "https://client-update.akamai.steamstatic.com/";

    public string InstallDir {
        get {
            //TODO: use registry to get installdir on Windows if we ever build an installer.
            // On all platforms, just use LocalApplicationData, which maps:
            // Linux: .local/share/OpenSteam
            // Windows: C:\Users\USERNAME\AppData\Local
            // Mac: /Users/USERNAME/.local/share
            var localShare = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localShare, "OpenSteam");
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
    public string PackageDir {
        get {
            return Path.Combine(InstallDir, "package");
        }
    }
    public string MainBinaryDir {
        get {
            if (OperatingSystem.IsLinux()) {
                return Path.Combine(InstallDir, "linux64");
            }
            return InstallDir;
        }
    }

    [SupportedOSPlatform("linux")]
    public string Ubuntu12_32Dir {
        get {
            return Path.Combine(InstallDir, "ubuntu12_32");
        }
    }

    public string BootstrapStateFile {
        get {
            return Path.Combine(InstallDir, "bootstrapper_state.json");
        }
    }
    
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
    public async void RunBootstrap(IExtendedProgress<int> progressHandler) {
        progressHandler.SetOperation("Bootstrapping");

        Directory.CreateDirectory(InstallDir);

        // steamclient blindly dumps certain files to the CWD, so set it
        Directory.SetCurrentDirectory(InstallDir);

        Directory.CreateDirectory(PackageDir);

        // Load previous state file if exists, create new if not
        BootstrapperState bootstrapperState;
        if (File.Exists(BootstrapStateFile)) {
            try {
                bootstrapperState = BootstrapperState.LoadFromFile(BootstrapStateFile);
            } catch (Exception) {
                bootstrapperState = new BootstrapperState();
            }
        } else {
            bootstrapperState = new BootstrapperState();
        }

        // Skip verification if user requests it
        if (bootstrapperState.SkipVerification) {
            progressHandler.SetProgress(100);
            return;
        }

        // Verify all files and skip bootstrap if files are valid and version matches 
        bool failedInitialCheck = bootstrapperState.InstalledVersion != OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION;
        int installedFilesLength = bootstrapperState.InstalledFiles.Count;
        int checkedFiles = 0;

        progressHandler.SetOperation("Checking files");

        // Convert absolute progress (files checked) into relative progress (0% - 100%)
        var relativeProgress = new Progress<long>(totalFiles => progressHandler.Report((int)(totalFiles / checkedFiles) * 100));
        
        foreach (var installedFile in bootstrapperState.InstalledFiles)
        {
            checkedFiles++;
            var info = new FileInfo(Path.Combine(InstallDir, installedFile.Key));
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

        if (bootstrapperState.InstalledVersion != OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION) {
            Directory.Delete(PackageDir, true);
            Directory.CreateDirectory(PackageDir);

            if (File.Exists(BootstrapStateFile)) {
                File.Delete(BootstrapStateFile);
            }
            bootstrapperState = new BootstrapperState();
        }

        if (installedFilesLength > 0 && !failedInitialCheck) {
            progressHandler.SetProgress(100);
            return;
        }

        Dictionary<string, string> zipPaths = new Dictionary<string, string>();

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
                    zipPaths.Add(package.Name, saveLocation);
                } else {
                    verificationFailed.Add(package.Name);
                }
            }
        }

        // Redownload if any packages or files fail verification
        if (verificationFailed.Count > 0) {
            if (RetryCount == 5) {
                throw new Exception($"Some files were still corrupted after attempting to redownload ${RetryCount} times. Check your disk and internet. ");
            }
            RetryCount++;
            RunBootstrap(progressHandler);
            return;
        }

        // Extract all the packages
        progressHandler.SetOperation("Extracting packages");
        foreach (var zip in zipPaths)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zip.Value))
            {
                await archive.ExtractToDirectory(InstallDir, progressHandler, (ZipArchiveEntry entry, string name) => {
                    bootstrapperState.InstalledFiles.Add(name, entry.Length);
                });  
            } 
        }

        // Process the Steam runtime if running on Linux (needed for SteamVR and some tools steam ships with)
        if (OperatingSystem.IsLinux()) {
            progressHandler.SetOperation($"Processing Steam Runtime");
            progressHandler.SetThrobber(true);
            progressHandler.SetSubOperation($"Combining Steam Runtime parts");

            string fullRuntimeFolder = Path.Combine(Ubuntu12_32Dir, "steam-runtime");
            Directory.CreateDirectory(fullRuntimeFolder);

            // Create a place in memory to store the runtime for combining and unzipping
            using (var fullFile = new MemoryStream()) {
                // First get all the parts
                List<string> parts = new List<string>(Directory.EnumerateFiles(Ubuntu12_32Dir, "steam-runtime.tar.xz.part*"));
                
                // Sort them to get an order like part3, part2, part1, part0
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
                Dictionary<string, string> checksums = new Dictionary<string, string>();

                var lines = File.ReadLines(Path.Combine(Ubuntu12_32Dir, "steam-runtime.checksum"));
                foreach (var line in lines)
                {
                    var split = line.Split("  ");
                    checksums.Add(split[1].Trim(), split[0].Trim());
                }

                // Verify files defined in steam-runtime.checksum
                foreach (var item in checksums)
                {
                    Console.WriteLine(item);
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
                        throw new Exception($"MD5 mismatch. File {file} is corrupted. {runtime_md5_expected} expected, got {runtime_md5_calculated}");
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
                bool result = proc.Start();

                if (!result) {
                    throw new Exception("Failed to start tar. Is tar installed?");
                }

                // Seek to the beginning again, just in case
                fullFile.Seek(0, SeekOrigin.Begin);

                StreamPiper.StartPiping(proc.StandardOutput.BaseStream, Console.OpenStandardOutput());
                StreamPiper.StartPiping(proc.StandardError.BaseStream, Console.OpenStandardError());
                StreamPiper.StartPiping(fullFile, proc.StandardInput.BaseStream);

                //BLOCKER: This never exits...
                proc.WaitForExit();
                Console.WriteLine("exited with: " + proc.ExitCode);
                progressHandler.SetProgress(100);
            }
        }
        progressHandler.SetOperation($"Copying OpenSteam files");
        progressHandler.SetThrobber(false);

        // Copy our files over (steamserviced, 64-bit reaper and 64-bit steamlaunchwrapper)
        //BLOCKER: CMake MSBuild integration (tomorrow?)

        bootstrapperState.InstalledVersion = OpenSteamworks.Generated.VersionInfo.STEAM_MANIFEST_VERSION;
        bootstrapperState.SaveToFile(BootstrapStateFile);
    }
    public Bootstrapper() {}
}