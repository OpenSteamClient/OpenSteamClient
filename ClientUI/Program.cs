using Avalonia;
using OpenSteamworks.KeyValues;
using OpenSteamworks.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        // MemoryStream stream = new(File.ReadAllBytes("/home/onni/.local/share/Steam/appcache/librarycache/assets.vdf"))
        // {
        //     Position = 0
        // };

        // var ogbytes = stream.ToArray();
        // KVObject obj = KVBinaryDeserializer.Deserialize(stream);
        // UtilityFunctions.Assert(obj.Equals(obj));
        // UtilityFunctions.Assert(obj.Equals(obj.Clone()));

        // var newbytes = KVBinarySerializer.SerializeToArray(obj);
        // File.WriteAllBytes("/home/onni/.local/share/Steam/appcache/librarycache/assets_opensteam.vdf", newbytes);
        // Console.WriteLine(ogbytes.Length + " == " + newbytes.Length);
        // var newbobj = KVBinaryDeserializer.Deserialize(newbytes);
        // UtilityFunctions.Assert(obj.Equals(newbobj));
        // UtilityFunctions.Assert(ogbytes.SequenceEqual(newbytes));

        // // KVObject obj = new("root", new System.Collections.Generic.List<KVObject>());
        // // obj.SetObject(new KVObject("test", "testvalue"));
        // // OpenSteamworks.Utils.UtilityFunctions.AssertNotNull(obj.GetObject("test"));
        // // obj.SetObject(new KVObject("testint", 256));
        // // obj.SetObject(new KVObject("test_conflictingnames", 256));
        // // obj.SetObject(new KVObject("testchildren", new System.Collections.Generic.List<KVObject>()));
        // // obj.GetObject("testchildren")?.Children.Add(new KVObject("testchild", "jaapo"));
        // //var root = obj;//.GetObject("")!;
        // //root.SetObject(new KVObject("15", new System.Collections.Generic.List<KVObject>()));
        // //root.SetChild(new KVObject("0", new System.Collections.Generic.List<KVObject>()));
        // var asText = KVTextSerializer.Serialize(obj);
        // //Console.WriteLine("serialized");
        // //Console.WriteLine(asText);
        // var newobj = KVTextDeserializer.Deserialize(asText);
        // //Console.WriteLine("deserialized");
        // //Console.WriteLine(KVTextSerializer.Serialize(newobj));

        // UtilityFunctions.Assert(obj.Equals(newobj, false));
        // Console.WriteLine("Assertions succeeded");
        // return;

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
            if (Debugger.IsAttached)
            {
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
