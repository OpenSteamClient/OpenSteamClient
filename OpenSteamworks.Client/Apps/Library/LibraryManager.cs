using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.ClientInterfaces;

namespace OpenSteamworks.Client.Apps.Library;

public class LibraryManager : ILogonLifetime
{
    private readonly CloudConfigStore cloudConfigStore;
    private readonly ISteamClient steamClient;
    private readonly ClientMessaging clientMessaging;
    private readonly Logger logger;
    private readonly InstallManager installManager;
    private readonly LoginManager loginManager;
    private readonly AppsManager appsManager;
    private Library? currentUserLibrary;

    public LibraryManager(ISteamClient steamClient, CloudConfigStore cloudConfigStore, ClientMessaging clientMessaging, LoginManager loginManager, InstallManager installManager, AppsManager appsManager) {
        this.logger = Logger.GetLogger("LibraryManager", installManager.GetLogPath("LibraryManager"));
        this.installManager = installManager;
        this.steamClient = steamClient;
        this.loginManager = loginManager;
        this.cloudConfigStore = cloudConfigStore;
        this.clientMessaging = clientMessaging;
        this.appsManager = appsManager;
    }

    public async Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        Library library = new(steamClient, cloudConfigStore, loginManager, appsManager, installManager);
        await library.InitializeLibrary();
        currentUserLibrary = library;
    }

    public Library GetLibrary()
    {
        if (currentUserLibrary == null) {
            throw new NullReferenceException("Attempted to get library before logon has finished.");
        }

        return currentUserLibrary;
    }

    public async Task OnLoggingOff(IExtendedProgress<int> progress) {
        if (currentUserLibrary != null) {
            await currentUserLibrary.SaveLibrary();
            currentUserLibrary = null;
        }
    }
}