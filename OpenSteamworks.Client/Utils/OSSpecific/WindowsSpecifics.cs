using System.Diagnostics;
using System.Runtime.Versioning;

namespace OpenSteamworks.Client.Utils.OSSpecific;

[SupportedOSPlatform("windows")]
public class WindowsSpecifics : IOSSpecifics {
    public string SteamClientBinaryName => "steamclient64.dll";
    public string SteamClientManifestName => "steam_client_win32";
}