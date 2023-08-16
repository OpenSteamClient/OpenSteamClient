using System.IO.Compression;
using Common.Utils;
using Common.Utils.OSSpecific;

namespace Common.Extensions;

public static class ZipArchiveEntryExtensions {
    public static bool IsSymlink(this ZipArchiveEntry entry) {
        return GetFileType(entry) == FileTypes.S_IFLNK;
    }

    public static bool IsDirectory(this ZipArchiveEntry entry) {
        return GetFileType(entry) == FileTypes.S_IFDIR;
    }

    public static bool IsRegularFile(this ZipArchiveEntry entry) {
        return GetFileType(entry) == FileTypes.S_IFREG;
    }

    public static FileTypes GetFileType(this ZipArchiveEntry entry) {
        return OSSpecifics.Instance.ParseZipExternalAttributes(entry.ExternalAttributes).fileType;
    }

}

