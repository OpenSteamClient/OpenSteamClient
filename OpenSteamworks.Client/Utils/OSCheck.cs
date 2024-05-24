namespace OpenSteamworks.Client.Utils;

public static class OSCheck {
    public static bool IsWindows11() {
        if (!OperatingSystem.IsWindows()) {
            return false;
        }

        return Environment.OSVersion.Version.Build >= 22000;
    }

    public static bool IsArchLinux() {
        if (!OperatingSystem.IsLinux()) {
            return false;
        }

        try
        {
            var lines = File.ReadLines("/etc/os-release");
            var id = lines.FirstOrDefault(l => l.StartsWith("ID="));
            if (id != null) {
                return id.Replace("ID=", string.Empty) == "arch";
            }
        }
        catch (System.Exception)
        {
            return false;
        }

        return false;
    }
}