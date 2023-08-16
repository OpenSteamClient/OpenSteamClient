using System.Diagnostics;
using System.Runtime.Versioning;

namespace Common.Utils.OSSpecific;

[SupportedOSPlatform("windows")]
public class WindowsSpecifics : IOSSpecifics {
    public string SteamClientBinaryName => "steamclient64.dll";
    public string SteamClientManifestName => "steam_client_win32";
    public (int permissions, FileTypes fileType) ParseZipExternalAttributes(int externalAttributes) {
        var lowerByte = (byte)(externalAttributes & 0x00FF);
        var attributes = (FileAttributes)lowerByte;
        
        if (attributes.HasFlag(FileAttributes.Directory)) {
            return (0, FileTypes.S_IFDIR);
        }

        if (attributes.HasFlag(FileAttributes.Normal) || attributes == 0) {
            return (0, FileTypes.S_IFREG);
        }

        throw new NotSupportedException("Unsupported attributes " + attributes.ToString());
    }
}