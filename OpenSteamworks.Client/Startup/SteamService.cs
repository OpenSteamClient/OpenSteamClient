using System.Diagnostics;
using System.Runtime.Versioning;
using System.ServiceProcess;
using OpenSteamworks.Client.Managers;
using OpenSteamworks;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Utils;

namespace OpenSteamworks.Client.Startup;

public class SteamService : Component {
    public event EventHandler? FailedToStartEvent;
    public bool FailedToStart { get; private set; } = false;
    public bool ShouldStop = false;
    public bool IsRunningAsHost = false;
    public object CurrentServiceHostLock = new();
    public Process? CurrentServiceHost;
    public ServiceController? CurrentWindowsService;
    public Thread? WatcherThread;
    private SteamClient steamClient;
    private ConfigManager configManager;
    private Backoff backoff;

    public SteamService(SteamClient steamClient, ConfigManager configManager, IContainer container) : base(container) {
        this.steamClient = steamClient;
        this.configManager = configManager;
        this.backoff = new Backoff(10);
        this.backoff.OnFailedPermanently += OnFailedPermanently;
    }

    private void OnFailedPermanently(object? sender, EventArgs e) {
        ShouldStop = true;
        FailedToStart = true;
        FailedToStartEvent?.Invoke(this, EventArgs.Empty);
    }

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("osx")]
    public void StartServiceAsHost(string pathToHost) {
        FailedToStart = false;
        lock (CurrentServiceHostLock)
        {
            IsRunningAsHost = true;
            CurrentServiceHost = new Process();
            CurrentServiceHost.StartInfo.WorkingDirectory = Path.GetDirectoryName(pathToHost);
            CurrentServiceHost.StartInfo.FileName = pathToHost;
            CurrentServiceHost.StartInfo.Environment.Add("LD_LIBRARY_PATH", $".:{Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");
            CurrentServiceHost.StartInfo.Environment.Add("OPENSTEAM_PID", Environment.ProcessId.ToString());
            CurrentServiceHost.Start();
            if (WatcherThread == null || !WatcherThread.IsAlive) {
                WatcherThread = new Thread(() => {
                    do
                    {
                        if (CurrentServiceHost.HasExited) {
                            Console.WriteLine("steamserviced crashed! Restarting in 1s.");
                            backoff.OnError();
                            System.Threading.Thread.Sleep(1000);
                            CurrentServiceHost.Start();
                        }
                        System.Threading.Thread.Sleep(50);
                    } while (!ShouldStop);
                    CurrentServiceHost.Kill();
                    WatcherThread = null;
                    backoff.OnSuccess();
                });
                
                WatcherThread.Start();
            }
        }
    }

    [SupportedOSPlatform("windows")]
    public void StartServiceAsWindowsService() {
        //CurrentWindowsService = new ServiceController("Steam Client Service");
    }

    public void StopService() {
        ShouldStop = true;
    }

    public override async Task RunShutdown() {
        this.StopService();
        await EmptyAwaitable();
    }

    public override async Task RunStartup()
    {
        if (GetComponent<AdvancedConfig>().EnableSteamService) {
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