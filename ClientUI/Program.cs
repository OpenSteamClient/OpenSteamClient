using Autofac;
using Avalonia;
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
        //TODO: single instance and pipe logic
        
        try {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown); 
        } catch (Exception e) {
            MessageBox.Error("OpenSteamClient needs to close", "We encountered a fatal exception: " + e.Message, e.ToString());
            App.Current?.Exit(1);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
