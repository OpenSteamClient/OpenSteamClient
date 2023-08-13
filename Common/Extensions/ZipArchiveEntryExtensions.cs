using System.IO.Compression;
using Common.Utils.OSSpecific;

namespace Common.Extensions;

public static class ZipArchiveEntryExtensions {
    public static bool IsSymlink(this ZipArchiveEntry entry) {
        var type = GetFileType(entry);
        Console.WriteLine(entry.FullName + " " + entry.ExternalAttributes + " " + type);
        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS()) {
            return type == LinuxHelpers.FileTypes.S_IFLNK;
        }
        return false;
    }

    public static bool IsDirectory(this ZipArchiveEntry entry) {
        return GetFileType(entry) == LinuxHelpers.FileTypes.S_IFDIR;
    }

    public static int GetFileType(this ZipArchiveEntry entry) {
        if (OperatingSystem.IsLinux()) {
            return LinuxHelpers.ParseZipExternalAttributes(entry.ExternalAttributes).fileType;
        }
        throw new Exception();
    }

}

