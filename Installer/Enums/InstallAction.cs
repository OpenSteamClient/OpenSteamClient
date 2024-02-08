using System;

namespace Installer.Enums;

[Flags]
public enum InstallAction {
    None = 0,
    Install = 1 << 1,
    Repair = 1 << 2,
    Uninstall = 1 << 3
}