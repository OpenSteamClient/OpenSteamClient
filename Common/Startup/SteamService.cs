using System.Diagnostics;
using System.Runtime.Versioning;
using System.ServiceProcess;

namespace Common.Startup;

public static class SteamService {
    public static bool ShouldStop = false;
    public static bool IsRunningAsHost = false;
    public static Process? CurrentServiceHost;
    public static ServiceController? CurrentWindowsService;
    public static Thread? WatcherThread;

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("osx")]
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
                    Console.WriteLine("steamserviced crashed! Restarting in 1s.");
                    System.Threading.Thread.Sleep(1000);
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
        //CurrentWindowsService = new ServiceController("Steam Client Service");
    }

    public static void StopService() {
        ShouldStop = true;
    }

    public static void Shutdown() {
        ShouldStop = true;
    }
}