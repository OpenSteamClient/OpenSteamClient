using System.Diagnostics;
using System.Text.RegularExpressions;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;

namespace GameOverlayUI.Impl;

/// <summary>
/// Mostly copied from Avalonia BclLauncher
/// </summary>
public class OverlayLauncher : ILauncher
{
    public virtual Task<bool> LaunchUriAsync(Uri uri)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));
        if (uri.IsAbsoluteUri)
        {
            return Task.FromResult(Exec(uri.AbsoluteUri));
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// This Process based implementation doesn't handle the case, when there is no app to handle link.
    /// It will still return true in this case.
    /// </summary>
    public virtual Task<bool> LaunchFileAsync(IStorageItem storageItem)
    {
        _ = storageItem ?? throw new ArgumentNullException(nameof(storageItem));
        if (storageItem.TryGetLocalPath() is { } localPath
            && CanOpenFileOrDirectory(localPath))
        {
            return Task.FromResult(Exec(localPath));
        }

        return Task.FromResult(false);
    }

    protected virtual bool CanOpenFileOrDirectory(string localPath) => true;
    
    private static bool Exec(string urlOrFile)
    {
        if (OperatingSystem.IsLinux())
        {
            // If no associated application/json MimeType is found xdg-open opens return error
            // but it tries to open it anyway using the console editor (nano, vim, other..)
            ShellExec($"xdg-open {urlOrFile}", waitForExit: false);
            return true;
        }
        else if (OperatingSystem.IsWindows() || OperatingSystem.IsMacOS())
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = OperatingSystem.IsWindows() ? urlOrFile : "open",
                Arguments = OperatingSystem.IsMacOS() ? $"{urlOrFile}" : "",
                CreateNoWindow = true,
                UseShellExecute = OperatingSystem.IsWindows()
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void ShellExec(string cmd, bool waitForExit = true)
    {
        var escapedArgs = Regex.Replace(cmd, "(?=[`~!#&*()|;'<>])", "\\")
            .Replace("\"", "\\\\\\\"");

        using (var process = Process.Start(
                   new ProcessStartInfo
                   {
                       FileName = "/bin/sh",
                       Arguments = $"-c \"{escapedArgs}\"",
                       RedirectStandardOutput = true,
                       UseShellExecute = false,
                       CreateNoWindow = true,
                       WindowStyle = ProcessWindowStyle.Hidden
                   }
               ))
        {
            if (waitForExit)
            {
                process?.WaitForExit();
            }
        }
    }
}