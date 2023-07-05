namespace OpenSteamworks;

//TODO: these shouldn't be here...
public class InstallInfo
{
    private OSPlatform os;
    public string OSLibrarySuffix { get; private set; }
    public string InstallPath { get; private set; }
    public string Steamclient64Path { get; private set; }

    public InstallInfo(OSPlatform os) {
        this.os = os;
        this.InstallPath = DetermineInstallPath();
        this.OSLibrarySuffix = DetermineOSLibrarySuffix();
        this.Steamclient64Path = DetermineSteamclient64Path();
    }

    public string DetermineInstallPath() {
        //TODO: hard coded for my machines.
        return "/home/onni/.steam/steam";
    }

    public string DetermineOSLibrarySuffix() {
        switch (os)
        {
            case OSPlatform.Windows:
                return "dll";
            case OSPlatform.OSX:
                return "dylib";
            case OSPlatform.Linux:
            case OSPlatform.FreeBSD:
            default:
                return "so";
        }
    }

    public string DetermineSteamclient64Path() {
        //TODO: only works for linux.
        return InstallPath + "/linux64/steamclient." + OSLibrarySuffix;
    }

    public string DetermineSteamclient32Path() {
        //TODO: only works for linux.
        return InstallPath + "/linux32/steamclient." + OSLibrarySuffix;
    }
}