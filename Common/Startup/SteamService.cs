using System.Diagnostics;
using System.Runtime.Versioning;

namespace Common.Startup;

public static class SteamService {
    public static bool ShouldStop = false;
    public static bool IsRunningInHost = false;
    public static Process? CurrentServiceHost;
    public static Thread? WatcherThread;

    [UnsupportedOSPlatform("windows")]
    public static void StartServiceAsHost(string pathToHost) {
        IsRunningInHost = true;
        CurrentServiceHost = new Process();
        CurrentServiceHost.StartInfo.WorkingDirectory = Path.GetDirectoryName(pathToHost);
        CurrentServiceHost.StartInfo.FileName = pathToHost;
        CurrentServiceHost.StartInfo.Environment.Add("LD_LIBRARY_PATH", $".:{Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");
        CurrentServiceHost.Start();
        WatcherThread = new Thread(() => {
            do
            {
                if (CurrentServiceHost.HasExited) {
                    Console.WriteLine("steamserviced crashed! Restarting.");
                    StartServiceAsHost(pathToHost);
                }
                System.Threading.Thread.Sleep(50);
            } while (!ShouldStop);
            CurrentServiceHost.Kill();
            WatcherThread = null;
        });
        
        WatcherThread.Start();
    }

    [SupportedOSPlatform("windows")]
    public static void StartServiceAsWindowsService() {
        throw new NotImplementedException();
    }

    public static void Shutdown() {
        ShouldStop = true;
    }
}