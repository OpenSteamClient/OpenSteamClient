using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Installer.Translation;
using Microsoft.Win32;

namespace Installer.Core;

public static class InstallationManager {
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

    public static void CreateRegKeys() {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }
        
        var node = Registry.LocalMachine.OpenSubKey("SOFTWARE")!.OpenSubKey("WOW6432Node")!;
        var steamSecurity = new RegistrySecurity();
        RegistryAccessRule rule = new("Users", 
           RegistryRights.ReadKey | RegistryRights.SetValue | RegistryRights.QueryValues | RegistryRights.ReadPermissions 
               | RegistryRights.Delete, 
           InheritanceFlags.ContainerInherit, 
           PropagationFlags.None, 
           AccessControlType.Allow
        );

        steamSecurity.AddAccessRule(rule);
        var opensteamnode = node.CreateSubKey("OpenSteamClient", RegistryKeyPermissionCheck.Default, RegistryOptions.None, steamSecurity);
        var valvesteamnode = node.CreateSubKey("Valve", RegistryKeyPermissionCheck.Default, RegistryOptions.None, steamSecurity).CreateSubKey("Steam", RegistryKeyPermissionCheck.Default, RegistryOptions.None, steamSecurity);
        CreateSteamRegKeys(opensteamnode);
        CreateSteamRegKeys(valvesteamnode);
    }

    private static void CreateSteamRegKeys(RegistryKey key) {
        key.SetValue("InstallPath", "", RegistryValueKind.String);
        key.SetValue("language", ELanguageConversion.APINameFromELanguage(AvaloniaApp.TranslationManager.CurrentTranslation.Language), RegistryValueKind.String);
        key.SetValue("SteamPID", 0, RegistryValueKind.DWord);
        key.SetValue("TempAppCmdLine", "", RegistryValueKind.String);
        key.SetValue("ReLaunchCmdLine", "", RegistryValueKind.String);
        key.SetValue("ClientLauncherType", 0, RegistryValueKind.DWord);
        key.SetValue("Universe", "Public", RegistryValueKind.String);
        key.SetValue("BetaName", "", RegistryValueKind.String);
    }
}