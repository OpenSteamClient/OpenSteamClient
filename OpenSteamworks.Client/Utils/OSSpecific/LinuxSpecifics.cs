using System.Runtime.Versioning;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Utils.OSSpecific;

[SupportedOSPlatform("linux")]
public class LinuxSpecifics : IOSSpecifics {
    public string SteamClientBinaryName => "steamclient.so";
    public string SteamClientManifestName => "steam_client_ubuntu12";
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

    public static string GetXDGSpecPath(string varName, string defaultIfNotDefined, string append = "") {
        string? path = Environment.GetEnvironmentVariable(varName);
        if (path == null) {
            var home = Environment.GetEnvironmentVariable("HOME");
            UtilityFunctions.AssertNotNull(home);
            path = Path.Combine(home, defaultIfNotDefined);
        }
        path = Path.Combine(path, append);
        return path;
    }
    
    public (int permissions, FileTypes fileType) ParseZipExternalAttributes(int externalAttributes) {
        int permissionsAndType = externalAttributes >> 16;
        int permissions = permissionsAndType & 0xFFF; // Mask to get the last 12 bits
        FileTypes fileType = (FileTypes)(permissionsAndType & (int)FileTypes.S_IFMT); // Mask using the S_IFMT mask
        return (permissions, fileType);
    }
}