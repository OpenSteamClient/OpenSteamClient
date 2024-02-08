using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace Installer;

public static class WindowToolbar {
    [DllImport("user32")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32")]
    public static extern bool EnableMenuItem(IntPtr hMenu, uint itemId, long uEnable);

    public const int SC_CLOSE = 0xF060;
    public const long MF_DISABLED = 0x00000002L;
    public const long MF_ENABLED = 0x00000000L;
    public const long MF_GRAYED = 0x00000001L;

    public static void DisableCloseButton(Window window) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }

        var handle = window.TryGetPlatformHandle();
        if (handle == null) {
            return;
        }

        EnableMenuItem(GetSystemMenu(handle.Handle, false), SC_CLOSE, MF_DISABLED | MF_GRAYED);
    }

    public static void EnableCloseButton(Window window) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }
        
        var handle = window.TryGetPlatformHandle();
        if (handle == null) {
            return;
        }
        
        EnableMenuItem(GetSystemMenu(handle.Handle, false), SC_CLOSE, MF_ENABLED);
    }
}