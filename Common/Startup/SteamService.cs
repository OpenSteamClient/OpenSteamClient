using System.Diagnostics;
using System.Runtime.Versioning;
using System.ServiceProcess;
using Common.Autofac;
using Common.Managers;
using OpenSteamworks;

namespace Common.Startup;

public class SteamService : IHasStartupTasks {
    public bool ShouldStop = false;
    public bool IsRunningAsHost = false;
    public Process? CurrentServiceHost;
    public ServiceController? CurrentWindowsService;
    public Thread? WatcherThread;
    public required SteamClient steamClient { protected get; init; }
    public required ConfigManager configManager { protected get; init; }

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("osx")]
    public void StartServiceAsHost(string pathToHost) {
        IsRunningAsHost = true;
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
    public void StartServiceAsWindowsService() {
        //CurrentWindowsService = new ServiceController("Steam Client Service");
    }

    public void StopService() {
        ShouldStop = true;
    }

    public void Shutdown() {
        ShouldStop = true;
    }

    void IHasStartupTasks.RunStartup()
    {
        if (configManager.AdvancedConfig.EnableSteamService) {
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
                    
                    this.StartServiceAsHost(Path.Combine(configManager.InstallDir, "steamserviced"));
                } else if (OperatingSystem.IsWindows()) {
                    this.StartServiceAsWindowsService();
                } else {
                    Console.WriteLine("Not running Steam Service due to unsupported OS");
                }
            } else {
                Console.WriteLine("Not running Steam Service due to existing client");
            }
        } else {
            Console.WriteLine("Not running Steam Service due to user preference");
        }
    }
}