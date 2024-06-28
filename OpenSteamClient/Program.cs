using Avalonia;
using OpenSteamworks.KeyValue;
using OpenSteamworks.KeyValue.ObjectGraph;
using OpenSteamworks.KeyValue.Deserializers;
using OpenSteamworks.KeyValue.Serializers;
using OpenSteamworks.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaCommon;

namespace OpenSteamClient;

public static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        //TODO: single instance and pipe logic
        try
        {
            //TODO: better command line args system (maybe in OpenSteamworks.Client to hook into various steamclient things)
            if (args.Contains("-debug"))
            {
                AvaloniaApp.DebugEnabled = true;
            }
#if DEBUG
            Console.WriteLine("Running DEBUG build, debug mode forced on");
            AvaloniaApp.DebugEnabled = true;
#endif
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown);
        }
        catch (Exception e)
        {
            FatalException(e);
        }
    }

    public static void FatalException(Exception e) {
        if (Debugger.IsAttached)
        {
            throw e;
        }

        Console.WriteLine(e.ToString());
        try
        {
            MessageBox.Error("OpenSteamClient needs to close", "OpenSteamClient has encountered a fatal exception. Exception message: " + e.Message, e.ToString());
        }
        catch (System.Exception)
        {
            
        }
        
        Environment.FailFast(null, e);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<AvaloniaApp>()
            .UsePlatformDetect()
            .WithInterFont()
            .UseSkia()
            .LogToTrace();
}
