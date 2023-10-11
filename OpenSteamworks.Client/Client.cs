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

public class Client : IClientLifetime
{
    private Container container;
    public async Task RunStartup()
    {        
        await Task.Run(() => {
            var args = Environment.GetCommandLineArgs();
            container.Get<IClientEngine>().SetClientCommandLine(args.Length, args);
        });
    }

    public async Task RunShutdown()
    {
        await Task.Run(() => {
            container.Get<SteamClient>().Shutdown();
        });
    }

    public Client(Container container, IExtendedProgress<int>? bootstrapperProgress = null) {
        this.container = container;
        container.ConstructAndRegisterImmediate<ConfigManager>();
        container.ConstructAndRegisterImmediate<Bootstrapper>().SetProgressObject(bootstrapperProgress);

        // OpenSteamworks doesn't support the component API by design. It allows it to be leanly integrated wherever. So we do the registration here instead of with subcomponents.
        container.RegisterFactoryMethod<SteamClient>(() => new SteamClient(container.Get<Bootstrapper>().SteamclientLibPath, container.Get<AdvancedConfig>().EnabledConnectionTypes));
        
        container.RegisterFactoryMethod<CallbackManager>((SteamClient client) => client.CallbackManager);
        container.RegisterFactoryMethod<ClientConfigStore>((SteamClient client) => client.ClientConfigStore);
        container.RegisterFactoryMethod<ClientMessaging>((SteamClient client) => client.ClientMessaging);
        container.RegisterFactoryMethod<IClientAppDisableUpdate>((SteamClient client) => client.NativeClient.IClientAppDisableUpdate);
        container.RegisterFactoryMethod<IClientAppManager>((SteamClient client) => client.NativeClient.IClientAppManager);
        container.RegisterFactoryMethod<IClientApps>((SteamClient client) => client.NativeClient.IClientApps);
        container.RegisterFactoryMethod<IClientAudio>((SteamClient client) => client.NativeClient.IClientAudio);
        container.RegisterFactoryMethod<IClientBilling>((SteamClient client) => client.NativeClient.IClientBilling);
        container.RegisterFactoryMethod<IClientCompat>((SteamClient client) => client.NativeClient.IClientCompat);
        container.RegisterFactoryMethod<IClientConfigStore>((SteamClient client) => client.NativeClient.IClientConfigStore);
        container.RegisterFactoryMethod<IClientDeviceAuth>((SteamClient client) => client.NativeClient.IClientDeviceAuth);
        container.RegisterFactoryMethod<IClientEngine>((SteamClient client) => client.NativeClient.IClientEngine);
        container.RegisterFactoryMethod<IClientFriends>((SteamClient client) => client.NativeClient.IClientFriends);
        container.RegisterFactoryMethod<IClientGameStats>((SteamClient client) => client.NativeClient.IClientGameStats);
        container.RegisterFactoryMethod<IClientHTMLSurface>((SteamClient client) => client.NativeClient.IClientHTMLSurface);
        container.RegisterFactoryMethod<IClientMatchmaking>((SteamClient client) => client.NativeClient.IClientMatchmaking);
        container.RegisterFactoryMethod<IClientMusic>((SteamClient client) => client.NativeClient.IClientMusic);
        container.RegisterFactoryMethod<IClientNetworking>((SteamClient client) => client.NativeClient.IClientNetworking);
        container.RegisterFactoryMethod<IClientRemoteStorage>((SteamClient client) => client.NativeClient.IClientRemoteStorage);
        container.RegisterFactoryMethod<IClientScreenshots>((SteamClient client) => client.NativeClient.IClientScreenshots);
        container.RegisterFactoryMethod<IClientShader>((SteamClient client) => client.NativeClient.IClientShader);
        container.RegisterFactoryMethod<IClientSharedConnection>((SteamClient client) => client.NativeClient.IClientSharedConnection);
        container.RegisterFactoryMethod<IClientShortcuts>((SteamClient client) => client.NativeClient.IClientShortcuts);
        container.RegisterFactoryMethod<IClientUGC>((SteamClient client) => client.NativeClient.IClientUGC);
        container.RegisterFactoryMethod<IClientUnifiedMessages>((SteamClient client) => client.NativeClient.IClientUnifiedMessages);
        container.RegisterFactoryMethod<IClientUser>((SteamClient client) => client.NativeClient.IClientUser);
        container.RegisterFactoryMethod<IClientUserStats>((SteamClient client) => client.NativeClient.IClientUserStats);
        container.RegisterFactoryMethod<IClientUtils>((SteamClient client) => client.NativeClient.IClientUtils);
        container.RegisterFactoryMethod<IClientVR>((SteamClient client) => client.NativeClient.IClientVR);

        container.ConstructAndRegister<LoginManager>();
        container.ConstructAndRegister<CloudConfigStore>();
        container.ConstructAndRegister<AppsManager>();
        container.ConstructAndRegister<SteamHTML>();
        container.ConstructAndRegister<SteamService>();
    }
}