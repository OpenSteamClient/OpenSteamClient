using Autofac;
using Common.Autofac;
using Common.Managers;
using Common.Startup;
using Common.Utils;
using OpenSteamworks;
using OpenSteamworks.Callbacks;
using System.Runtime.InteropServices;

namespace Common;

internal class CommonAutofacRegistrar : IAutofacRegistrar
{
    public static void Register(ref ContainerBuilder builder) {
        builder.Register(c => new SteamClient(c.Resolve<Bootstrapper>().SteamclientLibPath, c.Resolve<ConfigManager>().AdvancedConfig.EnabledConnectionTypes)).SingleInstance();

        builder.RegisterType<LoginManager>().As<IHasStartupTasks>().AsSelf().PropertiesAutowired().SingleInstance();
        builder.RegisterType<AppsManager>().As<IHasStartupTasks>().AsSelf().PropertiesAutowired().SingleInstance();
    }   
    public static void RegisterPreBootstrap(ref ContainerBuilder builder) {
        builder.RegisterType<ConfigManager>().As<IHasStartupTasks>().AsSelf().PropertiesAutowired().SingleInstance();
    }
}