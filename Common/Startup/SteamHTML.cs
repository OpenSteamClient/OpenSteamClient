using System.Diagnostics;
using System.Runtime.Versioning;

namespace Common.Startup;

public static class SteamHTML {
    public static bool ShouldStop = false;
    public static Process? CurrentHTMLHost;
    public static Thread? WatcherThread;

    [SupportedOSPlatform("linux")]
    public static void StartHTMLHost(string pathToHost, string cacheDir) {
        CurrentHTMLHost = new Process();
        CurrentHTMLHost.StartInfo.WorkingDirectory = Path.GetDirectoryName(pathToHost);
        CurrentHTMLHost.StartInfo.FileName = pathToHost;
        CurrentHTMLHost.StartInfo.Environment.Add("LD_LIBRARY_PATH", $".:{Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");
        CurrentHTMLHost.StartInfo.Environment.Add("LD_PRELOAD", $"/tmp/libhtmlhost_fakepid.so:/tmp/libbootstrappershim32.so:{Environment.GetEnvironmentVariable("LD_PRELOAD")}");
        CurrentHTMLHost.StartInfo.ArgumentList.Add(cacheDir);

        // We don't use steam-runtime-heavy
        CurrentHTMLHost.StartInfo.Environment.Add("STEAM_RUNTIME", $"0");
        
        CurrentHTMLHost.Start();

        if (WatcherThread == null) {
            WatcherThread = new Thread(() => {
                do
                {
                    if (CurrentHTMLHost.HasExited) {
                        Console.WriteLine("htmlhost crashed! Restarting.");
                        System.Threading.Thread.Sleep(1000);
                        StartHTMLHost(pathToHost, cacheDir);
                    }
                    System.Threading.Thread.Sleep(50);
                } while (!ShouldStop);
                CurrentHTMLHost.Kill();
                WatcherThread = null;
            });
            WatcherThread.Start();
        }
        
    }

    public static void Shutdown() {
        ShouldStop = true;
    }
}