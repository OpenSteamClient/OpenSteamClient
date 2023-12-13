using Avalonia;
using System;
using System.Diagnostics;
using System.Linq;
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
            //TODO: better command line args system (maybe in OpenSteamworks.Client to hook into various steamclient things)
            if (args.Contains("-debug")) {
                AvaloniaApp.DebugEnabled = true;
            }
#if DEBUG
            Console.WriteLine("Running DEBUG build, debug mode forced on");
            AvaloniaApp.DebugEnabled = true;
#endif
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown); 
        } catch (Exception e) {
            if (Debugger.IsAttached) {
                throw;
            }
            
            MessageBox.Error("OpenSteamClient needs to close", "OpenSteamClient has encountered a fatal exception. Exception message: " + e.Message, e.ToString());
            Console.WriteLine(e.ToString());
            Environment.Exit(1);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<AvaloniaApp>()
            .UsePlatformDetect()
            .WithInterFont()
            .UseSkia()
            .LogToTrace();
}
