using OpenSteamworks.Client.Utils.OSSpecific;

namespace OpenSteamworks.Client.Utils;

public static class OSSpecifics {
    public static readonly IOSSpecifics Instance;
    static OSSpecifics() {
        if (OperatingSystem.IsLinux()) {
            Instance = new LinuxSpecifics();
        } else if (OperatingSystem.IsWindows()) {
            Instance = new WindowsSpecifics();
        } else if (OperatingSystem.IsMacOS()) {
            Instance = new MacOSSpecifics();
        } else {
            throw new PlatformNotSupportedException();
        }
    }
}