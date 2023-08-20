namespace OpenSteamworks.Client.Utils.OSSpecific;

// Common OS-specific things
public interface IOSSpecifics {
    public string SteamClientBinaryName { get; }
    public string SteamClientManifestName { get; }
    public (int permissions, FileTypes fileType) ParseZipExternalAttributes(int externalAttributes);
}