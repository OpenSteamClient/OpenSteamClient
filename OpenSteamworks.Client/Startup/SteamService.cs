using System.Diagnostics;
using System.Runtime.Versioning;
using System.ServiceProcess;
using OpenSteamworks.Client.Managers;
using OpenSteamworks;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Utils;

namespace OpenSteamworks.Client.Startup;

public class SteamService : IClientLifetime {
    public event EventHandler? FailedToStartEvent;
    public bool FailedToStart { get; private set; } = false;
    public bool ShouldStop = false;
    public bool IsRunningAsHost = false;
    public object CurrentServiceHostLock = new();
    public Process? CurrentServiceHost;
    public ServiceController? CurrentWindowsService;
    public Thread? WatcherThread;
    private readonly SteamClient steamClient;
    private readonly InstallManager installManager;
    private readonly AdvancedConfig advancedConfig;
    private readonly Logger logger;

    public SteamService(SteamClient steamClient, InstallManager installManager, AdvancedConfig advancedConfig) {
        this.logger = new Logger("SteamServiceManager", installManager.GetLogPath("SteamServiceManager"));
        this.steamClient = steamClient;
        this.installManager = installManager;
        this.advancedConfig = advancedConfig;
    }

    private void OnFailedPermanently(object? sender, EventArgs e) {
        ShouldStop = true;
        FailedToStart = true;
        FailedToStartEvent?.Invoke(this, EventArgs.Empty);
    }

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("osx")]
    public void StartServiceAsHost(string pathToHost) {
        FailedToStart = false;
        lock (CurrentServiceHostLock)
        {
            IsRunningAsHost = true;
            CurrentServiceHost = new Process();
            CurrentServiceHost.StartInfo.WorkingDirectory = Path.GetDirectoryName(pathToHost);
            CurrentServiceHost.StartInfo.FileName = pathToHost;
            
            if (OperatingSystem.IsLinux()) {
                CurrentServiceHost.StartInfo.Environment.Add("LD_LIBRARY_PATH", $".:{Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");
                CurrentServiceHost.StartInfo.Environment.Add("OPENSTEAM_PID", Environment.ProcessId.ToString());
            }


            if (OperatingSystem.IsWindows()) {
                CurrentServiceHost.StartInfo.Verb = "runas";
                CurrentServiceHost.StartInfo.UseShellExecute = true;
            }

            CurrentServiceHost.Start();
            if (WatcherThread == null || !WatcherThread.IsAlive) {
                WatcherThread = new Thread(() => {
                    do
                    {
                        if (CurrentServiceHost.HasExited) {
                            logger.Error("steamserviced crashed! Restarting in 1s.");
                            System.Threading.Thread.Sleep(1000);
                            CurrentServiceHost.Start();
                        }
                        System.Threading.Thread.Sleep(50);
                    } while (!ShouldStop);
                    CurrentServiceHost.Kill();
                    WatcherThread = null;
                });
                
                WatcherThread.Start();
            }
        }
    }

    public void StopService() {
        ShouldStop = true;
    }

    public async Task RunShutdown() {
        this.StopService();
        await Task.CompletedTask;
    }

    public async Task RunStartup()
    {
        if (advancedConfig.EnableSteamService) {
            if (steamClient.NativeClient.ConnectedWith == SteamClient.ConnectionType.NewClient) {
                if (OperatingSystem.IsLinux()) {
                    try
                    {
                        await Task.Run(() =>
                        {
                            File.Copy(Path.Combine(installManager.InstallDir, "libbootstrappershim32.so"), "/tmp/libbootstrappershim32.so", true);
                            File.Copy(Path.Combine(installManager.InstallDir, "libhtmlhost_fakepid.so"), "/tmp/libhtmlhost_fakepid.so", true);
                        });
                    }
                    catch (Exception e)
                    {
                        logger.Warning("Failed to copy " + Path.Combine(installManager.InstallDir, "libbootstrappershim32.so") + " to /tmp/libbootstrappershim32.so: " + e.ToString());
                    }
                    
                    this.StartServiceAsHost(Path.Combine(installManager.InstallDir, "steamserviced"));
                } else if (OperatingSystem.IsWindows()) {
                    if (advancedConfig.ServiceAsAdminHostOnWindows) {
                        this.StartServiceAsHost(Path.Combine(installManager.InstallDir, "bin", "steamserviced.exe"));
                    } else {
                        // steamclient.dll auto starts the steamservice when needed, so starting the service here explicitly is unneeded. 
                    }
                } else {
                    logger.Warning("Not running Steam Service due to unsupported OS");
                }
            } else {
                logger.Info("Not running Steam Service due to existing client");
            }
        } else {
            logger.Info("Not running Steam Service due to user preference");
        }
    }
}