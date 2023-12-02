using OpenSteamworks.Client.Managers;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Client.Apps.Compat;

public class CompatManager {
    private readonly IClientCompat clientCompat;
    private readonly InstallManager installManager;

    public CompatManager(IClientCompat clientCompat, InstallManager installManager) {
        this.clientCompat = clientCompat;
        this.installManager = installManager;
    }


}