using Autofac;
using Common.Autofac;
using Common.Managers;
using Common.Startup;
using Common.Utils;
using OpenSteamworks;
using System.Runtime.InteropServices;

namespace Common;

public class CommonAutofacRegistrar : IAutofacRegistrar
{
    public static void Register(ref ContainerBuilder builder)
    {
        builder.RegisterType<Bootstrapper>().SingleInstance();
        builder.RegisterType<SteamClient>().SingleInstance();
        builder.Register(c => new LoginManager()).SingleInstance();
    }
}