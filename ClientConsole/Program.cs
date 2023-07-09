using Autofac;
using Common.Startup;
using Common.Utils;
using OpenSteamworks;

namespace ClientConsole;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = new ContainerBuilder();

        //TODO: this is something that the user should be able to pick. Needs a config system
        builder.Register(c => OpenSteamworks.SteamClient.ConnectionType.ExistingClient | OpenSteamworks.SteamClient.ConnectionType.NewClient).SingleInstance();

        // Registers everything into autofac and basically initializes the whole app
        ClientConsoleAutofacRegistrar.Register(ref builder);

        var container = builder.Build();
        ExtendedProgress<int> handler = new ExtendedProgress<int>(0, 100);
        handler.ProgressChanged += (object? sender, int current) =>
        {
            Console.WriteLine("Bootstrapper state is now: " + handler.Operation + ": " + handler.SubOperation + " with progress " + current + " of " + handler.MaxProgress);
        };

        container.Resolve<Bootstrapper>().RunBootstrap(handler);
        do
        {
            System.Threading.Thread.Sleep(10);
        } while (true);
    }

}