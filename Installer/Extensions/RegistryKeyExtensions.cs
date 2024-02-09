using System.Security.AccessControl;
using Microsoft.Win32;

namespace Installer.Extensions;

public static class RegistryKeyExtensions {
    public static RegistryKey CreateOrOpen(this RegistryKey regkey, string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, out bool created, RegistrySecurity? security = null) {
        var key = regkey.OpenSubKey(subkey, permissionCheck);

        created = key == null;
        if (key == null) {
            key = regkey.CreateSubKey(subkey, permissionCheck, registryOptions, security);
        }
        
        return key;
    }
}