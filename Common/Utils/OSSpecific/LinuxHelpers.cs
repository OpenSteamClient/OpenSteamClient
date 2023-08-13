using System.Runtime.Versioning;

namespace Common.Utils.OSSpecific;

[SupportedOSPlatform("linux")]
public static class LinuxHelpers {

    /// <summary>
    /// File types copied from dotnet source code
    /// </summary>
    public static class FileTypes
    {
        public const int S_IFMT = 0xF000;
        public const int S_IFIFO = 0x1000;
        public const int S_IFCHR = 0x2000;
        public const int S_IFDIR = 0x4000;
        public const int S_IFBLK = 0x6000;
        public const int S_IFREG = 0x8000;
        public const int S_IFLNK = 0xA000;
        public const int S_IFSOCK = 0xC000;
    }

    public static string GetPermissionString(int permissions)
    {
        char[] perms = new char[9];

        perms[0] = (permissions & 0x100) != 0 ? 'r' : '-';
        perms[1] = (permissions & 0x80) != 0 ? 'w' : '-';
        perms[2] = (permissions & 0x40) != 0 ? 'x' : '-';
        perms[3] = (permissions & 0x20) != 0 ? 'r' : '-';
        perms[4] = (permissions & 0x10) != 0 ? 'w' : '-';
        perms[5] = (permissions & 0x8) != 0 ? 'x' : '-';
        perms[6] = (permissions & 0x4) != 0 ? 'r' : '-';
        perms[7] = (permissions & 0x2) != 0 ? 'w' : '-';
        perms[8] = (permissions & 0x1) != 0 ? 'x' : '-';

        return new string(perms);
    }
    
    public static (int permissions, int fileType) ParseZipExternalAttributes(int externalAttributes) {
        int permissionsAndType = externalAttributes >> 16;
        int permissions = permissionsAndType & 0xFFF; // Mask to get the last 12 bits
        int fileType = permissionsAndType & LinuxHelpers.FileTypes.S_IFMT; // Mask using the S_IFMT mask
        return (permissions, fileType);
    }

    public static string GetNameOfFileType(int fileType) {
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