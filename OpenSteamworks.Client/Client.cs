using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;
using OpenSteamworks.Client.Utils;
using OpenSteamworks;
using OpenSteamworks.Callbacks;
using System.Runtime.InteropServices;
using System.Reflection;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Generated;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Apps.Library;
using System.Net;
using OpenSteamworks.Client.Friends;
using OpenSteamworks.Client.Apps.Compat;
using OpenSteamworks.Downloads;
using Profiler;

namespace OpenSteamworks.Client;

public class Client : IClientLifetime
{
    // HttpClient is intended to be instantiated once per application, rather than per-use. We define this here, you are free to use this for any web requests you may need.
    public static readonly HttpClient HttpClient = new()
    {
        Timeout = Timeout.InfiniteTimeSpan
    };

    static Client() {
        ServicePointManager.DefaultConnectionLimit = 50;
        HttpClient.DefaultRequestHeaders.ConnectionClose = false;
        //HttpClient.DefaultRequestHeaders.Add("User-Agent", $"opensteamclient {GitInfo.GitBranch}/{GitInfo.GitCommit}");
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "Valve Steam Client");
    }

    internal static Client? Instance { get; private set; }
    internal Container Container { get; init; }
    public async Task RunStartup()
    {   
        await Task.Run(() => {
            var args = Environment.GetCommandLineArgs();
            Container.Get<IClientEngine>().SetClientCommandLine(args.Length, args);
        });
    }

    public async Task RunShutdown()
    {
        await Task.Run(() => {
            Container.Get<ISteamClient>().Shutdown();
        });
    }

    public Client(Container container)
    {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("OpenSteamworks.Client - Client construction");

        Instance = this;
        this.Container = container;

        container.RegisterFactoryMethod<ISteamClient>((Bootstrapper bootstrapper, AdvancedConfig advancedConfig, InstallManager im) =>
        {
            Logging.GeneralLogger = Logger.GetLogger("OpenSteamworks", im.GetLogPath("OpenSteamworks"));
            var nativeClientLogger = Logger.GetLogger("OpenSteamworks-NativeClient"); // im.GetLogPath("OpenSteamworks_NativeClient")
            nativeClientLogger.AddPrefix = false;
            Logging.NativeClientLogger = nativeClientLogger;
            Logging.IPCLogger = Logger.GetLogger("OpenSteamworks-IPCClient", im.GetLogPath("OpenSteamworks_IPCClient"));
            Logging.CallbackLogger = Logger.GetLogger("OpenSteamworks-Callbacks", im.GetLogPath("OpenSteamworks_Callbacks"));
            Logging.JITLogger = Logger.GetLogger("OpenSteamworks-JIT", im.GetLogPath("OpenSteamworks_JIT"));
            Logging.ConCommandsLogger = Logger.GetLogger("OpenSteamworks-ConCommands", im.GetLogPath("OpenSteamworks_ConCommands"));
            Logging.MessagingLogger = Logger.GetLogger("OpenSteamworks-Messaging", im.GetLogPath("OpenSteamworks_Messaging"));
            Logging.CUtlLogger = Logger.GetLogger("OpenSteamworks-CUtl", im.GetLogPath("OpenSteamworks_CUtl"));
            Logging.LogIncomingCallbacks = advancedConfig.LogIncomingCallbacks;
            Logging.LogCallbackContents = advancedConfig.LogCallbackContents;
            if (advancedConfig.EnabledConnectionTypes == ConnectionType.ExperimentalIPCClient) {
                // return new IPCSteamClient(advancedConfig.SteamClientSpew);
            }

            return new SteamClient(bootstrapper.SteamclientLibPath, advancedConfig.EnabledConnectionTypes, advancedConfig.SteamClientSpew);
        });
        
        container.RegisterFactoryMethod<CallbackManager>((ISteamClient client) => client.CallbackManager);
        container.RegisterFactoryMethod<ClientApps>((ISteamClient client) => client.ClientApps);
        container.RegisterFactoryMethod<ClientConfigStore>((ISteamClient client) => client.ClientConfigStore);
        container.RegisterFactoryMethod<ClientMessaging>((ISteamClient client) => client.ClientMessaging);
        container.RegisterFactoryMethod<ClientRemoteStorage>((ISteamClient client) => client.ClientRemoteStorage);
        container.RegisterFactoryMethod<DownloadManager>((ISteamClient client) => client.DownloadManager);
        container.RegisterFactoryMethod<IClientAppDisableUpdate>((ISteamClient client) => client.IClientAppDisableUpdate);
        container.RegisterFactoryMethod<IClientAppManager>((ISteamClient client) => client.IClientAppManager);
        container.RegisterFactoryMethod<IClientApps>((ISteamClient client) => client.IClientApps);
        container.RegisterFactoryMethod<IClientAudio>((ISteamClient client) => client.IClientAudio);
        container.RegisterFactoryMethod<IClientBilling>((ISteamClient client) => client.IClientBilling);
        container.RegisterFactoryMethod<IClientCompat>((ISteamClient client) => client.IClientCompat);
        container.RegisterFactoryMethod<IClientConfigStore>((ISteamClient client) => client.IClientConfigStore);
        container.RegisterFactoryMethod<IClientDeviceAuth>((ISteamClient client) => client.IClientDeviceAuth);
        container.RegisterFactoryMethod<IClientEngine>((ISteamClient client) => client.IClientEngine);
        container.RegisterFactoryMethod<IClientFriends>((ISteamClient client) => client.IClientFriends);
        container.RegisterFactoryMethod<IClientGameStats>((ISteamClient client) => client.IClientGameStats);
        container.RegisterFactoryMethod<IClientHTMLSurface>((ISteamClient client) => client.IClientHTMLSurface);
        container.RegisterFactoryMethod<IClientMatchmaking>((ISteamClient client) => client.IClientMatchmaking);
        container.RegisterFactoryMethod<IClientMusic>((ISteamClient client) => client.IClientMusic);
        container.RegisterFactoryMethod<IClientNetworking>((ISteamClient client) => client.IClientNetworking);
        container.RegisterFactoryMethod<IClientRemoteStorage>((ISteamClient client) => client.IClientRemoteStorage);
        container.RegisterFactoryMethod<IClientScreenshots>((ISteamClient client) => client.IClientScreenshots);
        container.RegisterFactoryMethod<IClientShader>((ISteamClient client) => client.IClientShader);
        container.RegisterFactoryMethod<IClientSharedConnection>((ISteamClient client) => client.IClientSharedConnection);
        container.RegisterFactoryMethod<IClientShortcuts>((ISteamClient client) => client.IClientShortcuts);
        container.RegisterFactoryMethod<IClientUGC>((ISteamClient client) => client.IClientUGC);
        container.RegisterFactoryMethod<IClientUnifiedMessages>((ISteamClient client) => client.IClientUnifiedMessages);
        container.RegisterFactoryMethod<IClientUser>((ISteamClient client) => client.IClientUser);
        container.RegisterFactoryMethod<IClientUserStats>((ISteamClient client) => client.IClientUserStats);
        container.RegisterFactoryMethod<IClientUtils>((ISteamClient client) => client.IClientUtils);
        container.RegisterFactoryMethod<IClientVR>((ISteamClient client) => client.IClientVR);

        container.ConstructAndRegister<ShaderManager>();
        container.ConstructAndRegister<CompatManager>();
        container.ConstructAndRegister<LoginManager>();
        container.ConstructAndRegister<CloudConfigStore>();
        container.ConstructAndRegister<AppsManager>();
        container.ConstructAndRegister<LibraryManager>();
        container.ConstructAndRegister<SteamHTML>();
        container.ConstructAndRegister<SteamService>();
        container.ConstructAndRegister<FriendsManager>();
    }
}