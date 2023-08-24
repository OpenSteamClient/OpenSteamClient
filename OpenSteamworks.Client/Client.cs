using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;
using OpenSteamworks.Client.Utils;
using OpenSteamworks;
using OpenSteamworks.Callbacks;
using System.Runtime.InteropServices;
using OpenSteamworks.Client.Utils.Interfaces;
using System.Reflection;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Generated;
using OpenSteamworks.ClientInterfaces;

namespace OpenSteamworks.Client;

public class Client : Component
{
    private bool isSelfContainer = false;
    private IExtendedProgress<int>? bootstrapperProgress;
    public async override Task RunStartup()
    {        
        await Task.Run(() => {
            var args = Environment.GetCommandLineArgs();
            Container.GetComponent<IClientEngine>().SetClientCommandLine(args.Length, args);
        });
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
        Container.ConstructAndRegisterComponentImmediate<ConfigManager>();
        Container.ConstructAndRegisterComponentImmediate<Bootstrapper>().SetProgressObject(bootstrapperProgress);

        // OpenSteamworks doesn't support the component API by design. It allows it to be leanly integrated wherever. So we do the registration here instead of with subcomponents.
        Container.RegisterComponentFactoryMethod<SteamClient>(() => new SteamClient(Container.GetComponent<Bootstrapper>().SteamclientLibPath, Container.GetComponent<AdvancedConfig>().EnabledConnectionTypes));
        
        Container.RegisterComponentFactoryMethod<CallbackManager>((SteamClient client) => client.CallbackManager);
        Container.RegisterComponentFactoryMethod<ClientApps>((SteamClient client) => client.ClientApps);
        Container.RegisterComponentFactoryMethod<ClientConfigStore>((SteamClient client) => client.ClientConfigStore);
        Container.RegisterComponentFactoryMethod<ClientMessaging>((SteamClient client) => client.ClientMessaging);
        Container.RegisterComponentFactoryMethod<IClientAppDisableUpdate>((SteamClient client) => client.NativeClient.IClientAppDisableUpdate);
        Container.RegisterComponentFactoryMethod<IClientAppManager>((SteamClient client) => client.NativeClient.IClientAppManager);
        Container.RegisterComponentFactoryMethod<IClientApps>((SteamClient client) => client.NativeClient.IClientApps);
        Container.RegisterComponentFactoryMethod<IClientAudio>((SteamClient client) => client.NativeClient.IClientAudio);
        Container.RegisterComponentFactoryMethod<IClientBilling>((SteamClient client) => client.NativeClient.IClientBilling);
        Container.RegisterComponentFactoryMethod<IClientCompat>((SteamClient client) => client.NativeClient.IClientCompat);
        Container.RegisterComponentFactoryMethod<IClientConfigStore>((SteamClient client) => client.NativeClient.IClientConfigStore);
        Container.RegisterComponentFactoryMethod<IClientDeviceAuth>((SteamClient client) => client.NativeClient.IClientDeviceAuth);
        Container.RegisterComponentFactoryMethod<IClientEngine>((SteamClient client) => client.NativeClient.IClientEngine);
        Container.RegisterComponentFactoryMethod<IClientFriends>((SteamClient client) => client.NativeClient.IClientFriends);
        Container.RegisterComponentFactoryMethod<IClientGameStats>((SteamClient client) => client.NativeClient.IClientGameStats);
        Container.RegisterComponentFactoryMethod<IClientHTMLSurface>((SteamClient client) => client.NativeClient.IClientHTMLSurface);
        Container.RegisterComponentFactoryMethod<IClientMatchmaking>((SteamClient client) => client.NativeClient.IClientMatchmaking);
        Container.RegisterComponentFactoryMethod<IClientMusic>((SteamClient client) => client.NativeClient.IClientMusic);
        Container.RegisterComponentFactoryMethod<IClientNetworking>((SteamClient client) => client.NativeClient.IClientNetworking);
        Container.RegisterComponentFactoryMethod<IClientRemoteStorage>((SteamClient client) => client.NativeClient.IClientRemoteStorage);
        Container.RegisterComponentFactoryMethod<IClientScreenshots>((SteamClient client) => client.NativeClient.IClientScreenshots);
        Container.RegisterComponentFactoryMethod<IClientShader>((SteamClient client) => client.NativeClient.IClientShader);
        Container.RegisterComponentFactoryMethod<IClientSharedConnection>((SteamClient client) => client.NativeClient.IClientSharedConnection);
        Container.RegisterComponentFactoryMethod<IClientShortcuts>((SteamClient client) => client.NativeClient.IClientShortcuts);
        Container.RegisterComponentFactoryMethod<IClientUGC>((SteamClient client) => client.NativeClient.IClientUGC);
        Container.RegisterComponentFactoryMethod<IClientUnifiedMessages>((SteamClient client) => client.NativeClient.IClientUnifiedMessages);
        Container.RegisterComponentFactoryMethod<IClientUser>((SteamClient client) => client.NativeClient.IClientUser);
        Container.RegisterComponentFactoryMethod<IClientUserStats>((SteamClient client) => client.NativeClient.IClientUserStats);
        Container.RegisterComponentFactoryMethod<IClientUtils>((SteamClient client) => client.NativeClient.IClientUtils);
        Container.RegisterComponentFactoryMethod<IClientVR>((SteamClient client) => client.NativeClient.IClientVR);

        Container.ConstructAndRegisterComponent<LoginManager>();
        Container.ConstructAndRegisterComponent<AppsManager>();
        Container.ConstructAndRegisterComponent<SteamHTML>();
        Container.ConstructAndRegisterComponent<SteamService>();
        
    }
}