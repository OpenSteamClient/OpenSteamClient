using OpenSteamworks.Client.Utils;
using System.IO.Compression;
using System.Text;

namespace OpenSteamworks.Client.Extensions;

public static class ZipArchiveExtensions
{
    public static async Task ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, IExtendedProgress<int> prog, IEnumerable<string> blacklistedFiles, Action<ZipArchiveEntry, string>? afterExtractHook = null) {
        foreach (ZipArchiveEntry entry in source.Entries)
        {
            var FullNameFixed = entry.FullName;

            // Fixup slashes for Non-Windows platforms
            if (!OperatingSystem.IsWindows()) {
                FullNameFixed = FullNameFixed.Replace('\\', '/');
            }

            var FullPath = Path.Combine(destinationDirectoryName, FullNameFixed);

            if (entry.Length == 0 && (FullNameFixed.EndsWith('/') || FullNameFixed.EndsWith('\\'))) {
                Directory.CreateDirectory(FullPath);
                continue;
            }
            
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath)!);

            if (blacklistedFiles.Any() && blacklistedFiles.Contains(entry.Name)) {
                continue;
            }

            using (Stream zipstream = entry.Open()) {
                if (entry.IsSymlink()) {
                        // The content of a file that should be a symlink is a path to the target
                        StreamReader reader = new StreamReader(zipstream);
                        var target = reader.ReadToEnd();
                        prog.SetSubOperation($"Linking {FullNameFixed} to " + target);

                        if (File.Exists(FullNameFixed)) {
                            File.Delete(FullNameFixed);
                        }
                        
                        File.CreateSymbolicLink(FullNameFixed, target);
                } else {
                    using (var file = new FileStream(FullPath, FileMode.Create, FileAccess.Write, FileShare.None)) {
                        prog.SetSubOperation($"Extracting {FullNameFixed}");
                        // Convert absolute progress (bytes unzipped) into relative progress (0% - 100%)
                        var relativeProgress = new Progress<long>(totalBytes => prog.Report((int)(((float)totalBytes / entry.Length)*100)));
                        
                        // Use extension method to report progress while downloading
                        await zipstream.CopyToAsync(file, 81920, relativeProgress, default);
                        afterExtractHook?.Invoke(entry, FullNameFixed);
                    }
                }
            }
        }   
    }
}