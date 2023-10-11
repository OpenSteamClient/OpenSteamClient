using System.Runtime.InteropServices;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client;
using OpenSteamworks;

namespace ClientConsole;

public static class Program
{
    public static Container Container = new Container();

    [STAThread]
    public static void Main(string[] args)
    {
        MainAsync(args).Wait();
    }
    [STAThread]
    public static async Task MainAsync(string[] args)
    {
        ExtendedProgress<int> prog = new ExtendedProgress<int>(0, 100);
        prog.ProgressChanged += (object? sender, int current) =>
        {
            string endPart = "";
            if (!prog.Throbber) {
                endPart = " with progress " + current + " of " + prog.MaxProgress;
            }
            Console.WriteLine("Bootstrapper is " + prog.Operation + ", " + prog.SubOperation + endPart);
        };

        Container.RegisterComponentInstance(new Client(Container, prog));
        await Container.RunStartupForComponents();
        Console.WriteLine("Started up");
        
        Container.Get<SteamClient>().LogClientState();

        do
        {
            System.Threading.Thread.Sleep(10);
        } while (true);
    }

}