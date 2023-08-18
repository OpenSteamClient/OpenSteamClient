using System.Diagnostics;
using System.Runtime.Versioning;
using Common.Autofac;
using Common.Managers;
using OpenSteamworks;

namespace Common.Startup;

public class SteamHTML : IHasStartupTasks {
    public bool ShouldStop = false;
    public Process? CurrentHTMLHost;
    public Thread? WatcherThread;
    public required SteamClient steamClient { protected get; init; }
    public required ConfigManager configManager { protected get; init; }

    [SupportedOSPlatform("linux")]
    public void StartHTMLHost(string pathToHost, string cacheDir) {
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

    public void Shutdown() {
        ShouldStop = true;
    }

    void IHasStartupTasks.RunStartup()
    {
        if (configManager.GlobalSettings.EnableWebHelper) {
            if (steamClient.NativeClient.ConnectedWith == SteamClient.ConnectionType.NewClient) {
                if (OperatingSystem.IsLinux()) {
                    try
                    {
                        File.Copy(Path.Combine(configManager.InstallDir, "libbootstrappershim32.so"), "/tmp/libbootstrappershim32.so", true);
                        File.Copy(Path.Combine(configManager.InstallDir, "libhtmlhost_fakepid.so"), "/tmp/libhtmlhost_fakepid.so", true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to copy " + Path.Combine(configManager.InstallDir, "libbootstrappershim32.so") + " to /tmp/libbootstrappershim32.so: " + e.ToString());
                    }
                    
                    this.StartHTMLHost(Path.Combine(configManager.InstallDir, "ubuntu12_32", "htmlhost"), Path.Combine(configManager.InstallDir, "appcache", "htmlcache"));
                } else {
                    //TODO: windows support (enable compile on windows, figure out how to hook getpid/it's variant on Windows)
                    Console.WriteLine("Not running SteamHTML due to unsupported OS");
                }
            } else {
                Console.WriteLine("Not running SteamHTML due to existing client connection");
            }
        } else {
            Console.WriteLine("Not running SteamHTML due to user preference");
        }
    }
}