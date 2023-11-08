using System.Diagnostics;
using System.Runtime.Versioning;
using OpenSteamworks.Client.Managers;
using OpenSteamworks;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Client.Config;
using System.Text;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Client.Startup;

public class SteamHTML : IClientLifetime {
    public bool ShouldStop = false;
    public object CurrentHTMLHostLock = new();
    public Process? CurrentHTMLHost;
    public Thread? WatcherThread;
    private readonly SteamClient steamClient;
    private readonly InstallManager installManager;
    private readonly GlobalSettings globalSettings;
    private readonly Logger logger;

    public SteamHTML(SteamClient steamClient, InstallManager installManager, GlobalSettings globalSettings) {
        this.logger = new Logger("SteamHTMLManager", installManager.GetLogPath("SteamHTMLManager"));
        this.steamClient = steamClient;
        this.installManager = installManager;
        this.globalSettings = globalSettings;
    }

    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("windows")]
    public void StartHTMLHost(string pathToHost, string cacheDir) {
        lock (CurrentHTMLHostLock)
        {
            CurrentHTMLHost = new Process();
            CurrentHTMLHost.StartInfo.WorkingDirectory = installManager.InstallDir;
            CurrentHTMLHost.StartInfo.FileName = pathToHost;

            string steampath;
            if (OperatingSystem.IsLinux()) {
                steampath = Directory.ResolveLinkTarget("/proc/self/exe", false)!.FullName;
            } else {
                steampath = Environment.ProcessPath!;
            }

            // Necessary for hooking some funcs (to get it to connect to master steam process)
            CurrentHTMLHost.StartInfo.Environment.Add("OPENSTEAM_EXE_PATH", steampath);
            CurrentHTMLHost.StartInfo.Environment.Add("OPENSTEAM_PID", Environment.ProcessId.ToString());
            
            if (OperatingSystem.IsLinux()) {
                CurrentHTMLHost.StartInfo.Environment.Add("LD_LIBRARY_PATH", $".:{Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");
                CurrentHTMLHost.StartInfo.Environment.Add("LD_PRELOAD", $"/tmp/libhtmlhost_fakepid.so:/tmp/libbootstrappershim32.so:{Environment.GetEnvironmentVariable("LD_PRELOAD")}");
                
                // We don't use steam-runtime-heavy
                CurrentHTMLHost.StartInfo.Environment.Add("STEAM_RUNTIME", "0");
            }
            
            // [cachedir, steampath, universe, realm, language, uimode, enablegpuacceleration, enablesmoothscrolling, enablegpuvideodecode, enablehighdpi, proxyserver, bypassproxyforlocalhost, composermode, ignoregpublocklist, allowworkarounds]
            CurrentHTMLHost.StartInfo.ArgumentList.Add(cacheDir);
            CurrentHTMLHost.StartInfo.ArgumentList.Add(steampath);
            CurrentHTMLHost.StartInfo.ArgumentList.Add(((int)this.steamClient.NativeClient.IClientUtils.GetConnectedUniverse()).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(((int)this.steamClient.NativeClient.IClientUtils.GetSteamRealm()).ToString());
            StringBuilder builder = new(128);
            this.steamClient.NativeClient.IClientUser.GetLanguage(builder, 128);
            CurrentHTMLHost.StartInfo.ArgumentList.Add(((int)ELanguageConversion.ELanguageFromAPIName(builder.ToString())).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(((int)this.steamClient.NativeClient.IClientUtils.GetCurrentUIMode()).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperGPUAcceleration).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperSmoothScrolling).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperGPUVideoDecode).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperHighDPI).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(this.globalSettings.WebhelperProxy);
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperIgnoreProxyForLocalhost).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(this.globalSettings.WebhelperComposerMode.ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperIgnoreGPUBlocklist).ToString());
            CurrentHTMLHost.StartInfo.ArgumentList.Add(Convert.ToInt32(this.globalSettings.WebhelperAllowWorkarounds).ToString());


            // Corresponds to --v=4 in CEF terms
            CurrentHTMLHost.StartInfo.ArgumentList.Add("-cef-verbose-logging 4");
            
            CurrentHTMLHost.Start();

            if (WatcherThread == null) {
                WatcherThread = new Thread(() => {
                    do
                    {
                        if (CurrentHTMLHost.HasExited) {
                            logger.Error("htmlhost crashed! Restarting in 1s.");
                            Thread.Sleep(1000);
                            StartHTMLHost(pathToHost, cacheDir);
                        }
                        Thread.Sleep(50);
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
                            File.Copy(Path.Combine(installManager.InstallDir, "libbootstrappershim32.so"), "/tmp/libbootstrappershim32.so", true);
                            File.Copy(Path.Combine(installManager.InstallDir, "libhtmlhost_fakepid.so"), "/tmp/libhtmlhost_fakepid.so", true);
                        });
                    }
                    catch (Exception e)
                    {
                        logger.Warning("Failed to copy " + Path.Combine(installManager.InstallDir, "libbootstrappershim32.so") + " to /tmp/libbootstrappershim32.so: " + e.ToString());
                    }
                    
                    this.StartHTMLHost(Path.Combine(installManager.InstallDir, "ubuntu12_32", "htmlhost"), Path.Combine(installManager.InstallDir, "appcache", "htmlcache"));
                } else if (OperatingSystem.IsWindows()) {                  
                    this.StartHTMLHost(Path.Combine(installManager.InstallDir, "htmlhost.exe"), Path.Combine(installManager.InstallDir, "appcache", "htmlcache"));
                } else {
                    //TODO: macos
                    logger.Warning("Not running SteamHTML due to unsupported OS");
                }
            } else {
                logger.Info("Not running SteamHTML due to existing client connection");
            }
        } else {
            logger.Info("Not running SteamHTML due to user preference");
        }
    }
}