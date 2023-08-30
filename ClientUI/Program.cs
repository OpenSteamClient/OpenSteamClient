using Avalonia;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ClientUI;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        MainAsync(args).Wait();
    }
    [STAThread]
    public static async Task MainAsync(string[] args)
    {
        //TODO: single instance and pipe logic
        try {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown); 
        } catch (Exception e) {
            if (Debugger.IsAttached) {
                throw;
            }
            
            MessageBox.Error("OpenSteamClient needs to close", "OpenSteamClient has encountered a fatal exception and will attempt to close gracefully. This may freeze. If it does, just kill the process manually. Exception message: " + e.Message, e.ToString());
            // This is stupid. TODO: Pending support for "await?" to clean up.
            await (App.Current == null ? Task.CompletedTask : App.Current.Exit(1));
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
