using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using OpenSteamworks;
using System;

namespace ClientUI;

public static class Program
{
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)  
    {
        var builder = new ContainerBuilder();

        //TODO: this is something that the user should be able to pick. Needs a config system
        builder.Register(c => OpenSteamworks.SteamClient.ConnectionType.ExistingClient | OpenSteamworks.SteamClient.ConnectionType.NewClient).SingleInstance();

        // Registers everything into autofac and basically initializes the whole app
        ClientUIAutofacRegistrar.Register(ref builder);

        var container = builder.Build();
        container.Resolve<SteamClient>().LogClientState();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown);
    }
        

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
