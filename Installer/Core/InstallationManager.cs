using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading.Tasks;
using Installer.Extensions;
using Installer.Translation;
using Microsoft.Win32;

namespace Installer.Core;

public static class InstallationManager {
    public const string CommonFilesPath = "C:\\Program Files (x86)\\Common Files\\Steam";
    public static string InstallDir {
        get {
            if (AvaloniaApp.InLinuxDevelopment) {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenSteamClient_Installer_TargetDir_Dev");
            }

            var val = (string?)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\OpenSteamClient", "InstallPath", "");
            if (string.IsNullOrEmpty(val)) {
                return "";
            }

            return val;
        }

        set {
            if (AvaloniaApp.InLinuxDevelopment) {
                // Don't save it anywhere on linux
                return;
            }

            Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\OpenSteamClient", "InstallPath", value);
        }
    }

    public static void CreateRegKeys(string installPath) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }
        
        var node = Registry.LocalMachine.OpenSubKey("SOFTWARE")!;
        var steamSecurity = new RegistrySecurity();
        RegistryAccessRule rule = new("Users", 
           RegistryRights.ReadKey | RegistryRights.SetValue | RegistryRights.QueryValues | RegistryRights.ReadPermissions 
               | RegistryRights.Delete | RegistryRights.FullControl | RegistryRights.CreateSubKey | RegistryRights.EnumerateSubKeys | RegistryRights.Notify | RegistryRights.CreateLink | RegistryRights.WriteKey | RegistryRights.ExecuteKey, 
           InheritanceFlags.ContainerInherit, 
           PropagationFlags.None, 
           AccessControlType.Allow
        );

        steamSecurity.AddAccessRule(rule);
        var opensteamnode = node.CreateOrOpen("OpenSteamClient", RegistryKeyPermissionCheck.Default, RegistryOptions.None, out bool _, steamSecurity);
        var valvenode = node.OpenSubKey("WOW6432Node")!.CreateOrOpen("Valve", RegistryKeyPermissionCheck.Default, RegistryOptions.None, out bool _);
        CreateSteamRegKeys(valvenode, steamSecurity, "Steam", "SteamService", "");
        CreateSteamRegKeys(opensteamnode, steamSecurity, "thiskey", serviceSubkey: "", installPath);
    }

    private static void CreateSteamRegKeys(RegistryKey key, RegistrySecurity steamSecurity, string clientSubkey, string serviceSubkey, string installPath) {
        if (!string.IsNullOrEmpty(serviceSubkey)) {
            var serviceKey = key.CreateOrOpen(serviceSubkey, RegistryKeyPermissionCheck.Default, RegistryOptions.None, out bool createdService);
            if (createdService) {
                serviceKey.SetValue("installpath_default", installPath, RegistryValueKind.String);
            }
        }

        if (!string.IsNullOrEmpty(clientSubkey)) {
            bool createdClient = true;
            RegistryKey? clientKey;
            if (clientSubkey == "thiskey") {
                clientKey = key;
            } else {
                clientKey = key.CreateOrOpen(clientSubkey, RegistryKeyPermissionCheck.Default, RegistryOptions.None, out createdClient, steamSecurity);
            }

            if (createdClient) {
                clientKey.SetValue("InstallPath", installPath, RegistryValueKind.String);
                clientKey.SetValue("language", ELanguageConversion.APINameFromELanguage(AvaloniaApp.TranslationManager.CurrentTranslation.Language), RegistryValueKind.String);
                // This is the important one, this allows the service to start up
                clientKey.SetValue("SteamPID", 0, RegistryValueKind.DWord);
                clientKey.SetValue("TempAppCmdLine", "", RegistryValueKind.String);
                clientKey.SetValue("ReLaunchCmdLine", "", RegistryValueKind.String);
                clientKey.SetValue("ClientLauncherType", 0, RegistryValueKind.DWord);
                clientKey.SetValue("Universe", "Public", RegistryValueKind.String);
                clientKey.SetValue("BetaName", "", RegistryValueKind.String);
            }
        }
        
        
    }

    /// <summary>
    /// Install the Steam Service.
    /// </summary>
    /// <param name="steamServiceExePath">The path to the extracted/existing Program Files (x86)/Common Files/Steam/steamservice.exe</param>
    public static async Task InstallSteamService(string steamServiceExePath) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }

        if (DoesServiceExist()) {
            return;
        }

        if (!File.Exists(steamServiceExePath)) {
            throw new LocalizedException("#InstallError_ServiceInstallFailed");
        }

        var installproc = Process.Start(steamServiceExePath, "/install");
        await installproc.WaitForExitAsync();
        if (installproc.ExitCode != 0) {
            // Uninstall and reinstall to fix permission issues
            var uninstallproc = Process.Start(steamServiceExePath, "/uninstall");
            await  uninstallproc.WaitForExitAsync();

            // Try to install a second time
            installproc = Process.Start(steamServiceExePath, "/install");
            await installproc.WaitForExitAsync();
            if (installproc.ExitCode != 0) {
                // Give up after the second error
                throw new LocalizedException("#InstallError_ServiceInstallFailed");
            }
        }
    }

    public static bool DoesServiceExist()
    {
        ServiceController[] services = ServiceController.GetServices();
        var service = services.FirstOrDefault(s => s.ServiceName == "Steam Client Service");
        return service != null;
    }


    public static void CreateUninstallRegs(string uninstallExe) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }
        
        var node = Registry.LocalMachine.OpenSubKey("SOFTWARE")!.OpenSubKey("Microsoft")!.OpenSubKey("Windows")!.OpenSubKey("CurrentVersion")!.OpenSubKey("Uninstall")!;
        node.SetValue("DisplayName", "OpenSteamClient", RegistryValueKind.String);
        node.SetValue("DisplayVersion", "", RegistryValueKind.String);
        node.SetValue("Publisher", "Rosentti", RegistryValueKind.String);
        node.SetValue("URLInfoAbout", "https://github.com/OpenSteamClient/OpenSteamClient", RegistryValueKind.String);
        node.SetValue("HelpLink", "https://github.com/OpenSteamClient/OpenSteamClient/issues", RegistryValueKind.String);
        node.SetValue("DisplayIcon", uninstallExe, RegistryValueKind.String);
        node.SetValue("UninstallString", uninstallExe, RegistryValueKind.String);
        node.SetValue("NoModify", 1, RegistryValueKind.DWord);
    }

    public static async Task ExtractClientUIToPath(string path, IProgress<int> progress) {
        
    }

    // If other files ever start mattering, copy them over too, but for now this seems to be enough
    // drivers.exe, steamservice.dll and others just magically appear after Steam starts
    public static readonly ReadOnlyCollection<string> CommonFiles = new(new List<string>()
    {
        "steamservice.exe",
    });

    public static bool CommonFilesNeedsSetup() {
        if (Directory.Exists(CommonFilesPath)) {
            foreach (var item in CommonFiles)
            {
                if (!File.Exists(Path.Combine(CommonFilesPath, item))) {
                    return true;
                }
            }
        } else {
            return true;
        }

        return false;
    }

    public static async Task SetupCommonFiles(IProgress<int> progress, IProgress<string> currentOperationLocToken) {
        currentOperationLocToken.Report("#InstallProgress_DownloadingService");
        using var stream = new MemoryStream();
        await HttpClient.DownloadAsync(SteamManifest.bins_win32, stream, progress);
        Directory.CreateDirectory(CommonFilesPath);
        
        currentOperationLocToken.Report("#InstallProgress_ExtractingService");
        progress.Report(0);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true);
        await archive.ExtractToDirectory(CommonFilesPath, progress);  
    }

    public static readonly HttpClient HttpClient = new();

    static InstallationManager() {
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "opensteamclient-installer/1.0");
    }

    public static async Task RunAllInstallSteps(string targetPath, IProgress<int> progress, IProgress<string> currentOperationLocToken) {
        // Do the things that are more likely to fail first
        if (CommonFilesNeedsSetup()) {
            await SetupCommonFiles(progress, currentOperationLocToken);
        }

        if (!DoesServiceExist()) {
            currentOperationLocToken.Report("#InstallProgress_InstallingService");
            await InstallSteamService(Path.Combine(CommonFilesPath, "steamservice.exe"));
        }


    }
}