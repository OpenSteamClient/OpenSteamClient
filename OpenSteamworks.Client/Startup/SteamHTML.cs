using System.Diagnostics;
using System.Runtime.Versioning;
using OpenSteamworks.Client.Managers;
using OpenSteamworks;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.Client.Config;

namespace OpenSteamworks.Client.Startup;

public class SteamHTML : IClientLifetime {
    public bool ShouldStop = false;
    public object CurrentHTMLHostLock = new();
    public Process? CurrentHTMLHost;
    public Thread? WatcherThread;
    private SteamClient steamClient;
    private ConfigManager configManager;
    private GlobalSettings globalSettings;

    public SteamHTML(SteamClient steamClient, ConfigManager configManager, GlobalSettings globalSettings) {
        this.steamClient = steamClient;
        this.configManager = configManager;
        this.globalSettings = globalSettings;
    }

    [SupportedOSPlatform("linux")]
    public void StartHTMLHost(string pathToHost, string cacheDir) {
        lock (CurrentHTMLHostLock)
        {
            CurrentHTMLHost = new Process();
            CurrentHTMLHost.StartInfo.WorkingDirectory = Path.GetDirectoryName(pathToHost);
            CurrentHTMLHost.StartInfo.FileName = pathToHost;
            var steampath = Directory.ResolveLinkTarget("/proc/self/exe", false)!.FullName;

            // Necessary for libhtmlhost_fakepid (to get it to connect to master steam process)
            CurrentHTMLHost.StartInfo.Environment.Add("OPENSTEAM_EXE_PATH", steampath);
            CurrentHTMLHost.StartInfo.Environment.Add("OPENSTEAM_PID", Environment.ProcessId.ToString());
            CurrentHTMLHost.StartInfo.Environment.Add("LD_LIBRARY_PATH", $".:{Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");
            CurrentHTMLHost.StartInfo.Environment.Add("LD_PRELOAD", $"/tmp/libhtmlhost_fakepid.so:/tmp/libbootstrappershim32.so:{Environment.GetEnvironmentVariable("LD_PRELOAD")}");
            
            // We don't use steam-runtime-heavy
            CurrentHTMLHost.StartInfo.Environment.Add("STEAM_RUNTIME", "0");
            
            CurrentHTMLHost.StartInfo.ArgumentList.Add(cacheDir);
            CurrentHTMLHost.StartInfo.ArgumentList.Add(steampath);

            
            CurrentHTMLHost.Start();

            if (WatcherThread == null) {
                WatcherThread = new Thread(() => {
                    do
                    {
                        if (CurrentHTMLHost.HasExited) {
                            Console.WriteLine("htmlhost crashed! Restarting in 1s.");
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
    }

    public async Task RunShutdown() {
        ShouldStop = true;
        await Task.CompletedTask;
    }

    public async Task RunStartup()
    {
        if (globalSettings.EnableWebHelper) {
            if (steamClient.NativeClient.ConnectedWith == SteamClient.ConnectionType.NewClient) {
                if (OperatingSystem.IsLinux()) {
                    try
                    {
                        await Task.Run(() =>
                        {
                            File.Copy(Path.Combine(configManager.InstallDir, "libbootstrappershim32.so"), "/tmp/libbootstrappershim32.so", true);
                            File.Copy(Path.Combine(configManager.InstallDir, "libhtmlhost_fakepid.so"), "/tmp/libhtmlhost_fakepid.so", true);
                        });
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