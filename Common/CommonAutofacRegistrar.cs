using Autofac;
using Common.Autofac;
using Common.Managers;
using OpenSteamworks;
using System.Runtime.InteropServices;

namespace Common;

public class CommonAutofacRegistrar : IAutofacRegistrar
{
    public static void Register(ref ContainerBuilder builder)
    {
        builder.Register(c => GetOSPlatform()).SingleInstance();
        builder.RegisterType<InstallInfo>().SingleInstance();
        builder.RegisterType<SteamClient>().SingleInstance();
        builder.Register(c => new LoginManager()).SingleInstance();
    }
    public static OpenSteamworks.OSPlatform GetOSPlatform() {
        if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)) {
            return OpenSteamworks.OSPlatform.Windows;
        } else if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux)) {
            return OpenSteamworks.OSPlatform.Linux;
        } else if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD)) {
            return OpenSteamworks.OSPlatform.FreeBSD;
        } else if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX)) {
            return OpenSteamworks.OSPlatform.OSX;
        } else {
            throw new Exception("Not running on Windows, Linux, FreeBSD or OSX. WTF?");
        }
    }
}