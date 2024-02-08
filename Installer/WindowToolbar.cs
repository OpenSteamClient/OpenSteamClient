using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace Installer;

public static class WindowToolbar {
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x80000;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    public static void DisableCloseButton(Window window) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }

        var handle = window.TryGetPlatformHandle();
        if (handle == null) {
            return;
        }

        SetWindowLong(handle.Handle, GWL_STYLE, GetWindowLong(handle.Handle, GWL_STYLE) & ~WS_SYSMENU);
    }

    public static void EnableCloseButton(Window window) {
        if (AvaloniaApp.InLinuxDevelopment) {
            return;
        }
        
        var handle = window.TryGetPlatformHandle();
        if (handle == null) {
            return;
        }
        
        SetWindowLong(handle.Handle, GWL_STYLE, GetWindowLong(handle.Handle, GWL_STYLE) & WS_SYSMENU);
    }
}