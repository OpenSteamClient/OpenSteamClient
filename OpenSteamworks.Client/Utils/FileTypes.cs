namespace OpenSteamworks.Client.Utils;

/// <summary>
/// File types copied from dotnet source code
/// </summary>
public enum FileTypes : int
{
    S_IFMT = 0xF000,
    S_IFIFO = 0x1000,
    S_IFCHR = 0x2000,
    S_IFDIR = 0x4000,
    S_IFBLK = 0x6000,
    S_IFREG = 0x8000,
    S_IFLNK = 0xA000,
    S_IFSOCK = 0xC000
}

public static class FileTypesExtensions {
    public static string ToFriendlyString(this FileTypes fileType) {
        switch (fileType)
        {
            case FileTypes.S_IFIFO:
                return "FIFO (Named Pipe)";
                
            case FileTypes.S_IFCHR:
                return "Character Device";
                
            case FileTypes.S_IFDIR:
                return "Directory";
                
            case FileTypes.S_IFBLK:
                return "Block Device";
                
            case FileTypes.S_IFREG:
                return "Regular File";
                
            case FileTypes.S_IFLNK:
                return "Symbolic Link";
                
            case FileTypes.S_IFSOCK:
                return "Socket";
                
            default:
                return "Unknown";
                
        }
    }
}