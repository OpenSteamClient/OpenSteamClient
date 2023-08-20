using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;
using OpenSteamworks.Client.Utils;
using OpenSteamworks;
using OpenSteamworks.Callbacks;
using System.Runtime.InteropServices;
using OpenSteamworks.Client.Utils.Interfaces;
using System.Reflection;
using OpenSteamworks.Client.Config;

namespace OpenSteamworks.Client;

public class Client : Component
{
    private bool isSelfContainer = false;
    private IExtendedProgress<int>? bootstrapperProgress;
    public async override Task RunStartup()
    {        
        var args = Environment.GetCommandLineArgs();
        Container.GetComponent<SteamClient>().NativeClient.IClientEngine.SetClientCommandLine(args.Length, args);
    }

    public async override Task RunShutdown()
    {
        await Task.Run(() =>
        {
            Container.GetComponent<SteamClient>().Shutdown();
        });
    }

    public Client(IContainer? container = null, IExtendedProgress<int>? bootstrapperProgress = null) : base(container != null ? container : new Container()) {
        this.isSelfContainer = container == null;
        this.bootstrapperProgress = bootstrapperProgress;
        Container.ConstructAndRegisterComponent<ConfigManager>();
        Container.ConstructAndRegisterComponent<Bootstrapper>().SetProgressObject(bootstrapperProgress);

        // OpenSteamworks doesn't support the component API by design. It allows it to be leanly integrated wherever. So we do the registration here instead of with subcomponents.
        var steamclient = new SteamClient(Container.GetComponent<Bootstrapper>().SteamclientLibPath, Container.GetComponent<AdvancedConfig>().EnabledConnectionTypes);
        Container.RegisterComponentInstance(steamclient);
        Container.RegisterComponentInstance(steamclient.CallbackManager);
        Container.RegisterComponentInstance(steamclient.ClientApps);
        Container.RegisterComponentInstance(steamclient.ClientConfigStore);
        Container.RegisterComponentInstance(steamclient.ClientMessaging);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientAppDisableUpdate);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientAppManager);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientApps);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientAudio);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientBilling);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientCompat);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientConfigStore);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientDeviceAuth);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientEngine);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientFriends);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientGameStats);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientHTMLSurface);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientMatchmaking);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientMusic);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientNetworking);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientRemoteStorage);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientScreenshots);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientShader);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientSharedConnection);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientShortcuts);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientUGC);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientUnifiedMessages);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientUser);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientUserStats);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientUtils);
        Container.RegisterComponentInstance(steamclient.NativeClient.IClientVR);
        
        Container.ConstructAndRegisterComponent<LoginManager>();
        Container.ConstructAndRegisterComponent<AppsManager>();
        Container.ConstructAndRegisterComponent<SteamHTML>();
        Container.ConstructAndRegisterComponent<SteamService>();
        
    }
}