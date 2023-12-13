using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.ClientInterfaces;

namespace OpenSteamworks.Client.Apps.Library;

public class LibraryManager : ILogonLifetime
{
    private CloudConfigStore cloudConfigStore;
    private SteamClient steamClient;
    private ClientMessaging clientMessaging;
    private Logger logger;
    private InstallManager installManager;
    private LoginManager loginManager;
    private AppsManager appsManager;

    // This is bad, but the collections need to know about the library in order to be able to get apps.
    internal static Library? currentUserLibrary;

    public LibraryManager(SteamClient steamClient, CloudConfigStore cloudConfigStore, ClientMessaging clientMessaging, LoginManager loginManager, InstallManager installManager, AppsManager appsManager) {
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