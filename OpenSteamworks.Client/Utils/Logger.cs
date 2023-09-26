using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.VisualBasic;

namespace OpenSteamworks.Client;

public class Logger {
    public enum Level
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    public static object ConsoleLock = new();
    public string Name { get; set; } = "";
    public string LogfilePath { get; init; } = "";
    private object logStreamLock = new();
    private FileStream? logStream;
    public void Message(Level level, string message, string category = "", params object?[] formatObjs) {
        // welp. we can't just use the system's date format, but we also need to use the system's time at the same time, which won't include milliseconds and will always have AM/PM appended, even on 24-hour clocks. So use the better formatting system dd/MM/yyyy and always use 24-hour time
        string formatted = string.Format("[{0} {1}{2}: {3}] {4}", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss.ff"), this.Name, string.IsNullOrEmpty(category) ? "" : $"/{category}", level.ToString(), string.Format(message, formatObjs));
        lock (ConsoleLock)
        {
            Console.WriteLine(formatted);
        }

        if (logStream != null) {
            lock (logStreamLock)
            {
                logStream.Write(Encoding.Default.GetBytes(formatted + Environment.NewLine));
            }
        }
    }

    public void Debug(string message, params object?[] formatObjs) {
        this.Message(Level.DEBUG, message, "", formatObjs);
    }

    public void Debug(string message, string category, params object?[] formatObjs) {
        this.Message(Level.DEBUG, message, category, formatObjs);
    }

    public void Info(string message, params object?[] formatObjs) {
        this.Message(Level.INFO, message, "", formatObjs);
    }

    public void Info(string message, string category, params object?[] formatObjs) {
        this.Message(Level.INFO, message, category, formatObjs);
    }

    public void Warning(string message, params object?[] formatObjs) {
        this.Message(Level.WARNING, message, "", formatObjs);
    }

    public void Warning(string message, string category, params object?[] formatObjs) {
        this.Message(Level.WARNING, message, category, formatObjs);
    }

    public void Error(string message, params object?[] formatObjs) {
        this.Message(Level.ERROR, message, "", formatObjs);
    }

    public void Error(string message, string category, params object?[] formatObjs) {
        this.Message(Level.ERROR, message, category, formatObjs);
    }

    public void Fatal(string message, params object?[] formatObjs) {
        this.Message(Level.FATAL, message, "", formatObjs);
    }

    public void Fatal(string message, string category, params object?[] formatObjs) {
        this.Message(Level.FATAL, message, category, formatObjs);
    }
    

    public Logger(string name, string filepath = "") {
        this.Name = name;
        this.LogfilePath = filepath;

        if (!string.IsNullOrEmpty(this.LogfilePath)) {
            if (File.Exists(filepath)) {
                // Delete if over 4MB
                var fi = new FileInfo(filepath);
                if ((fi.Length / 1024 / 1024) > 4) {
                    fi.Delete();
                }
            }
            logStream = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        }

        if (!hasRanWindowsHack && OperatingSystem.IsWindows()) {
            RunWindowsConsoleColorsHack();
        }
    }

    private bool hasRanWindowsHack = false;
    /// <summary>
    /// Windows is stuck using legacy settings unless you tell it explicitly to use "ENABLE_VIRTUAL_TERMINAL_PROCESSING". Why???
    /// </summary>
    [SupportedOSPlatform("windows")]
    private void RunWindowsConsoleColorsHack() {
        hasRanWindowsHack = true;
        const int STD_INPUT_HANDLE = -10;
        const int STD_OUTPUT_HANDLE = -11;
        const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
        const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        var iStdIn = GetStdHandle(STD_INPUT_HANDLE);
        var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

        if (!GetConsoleMode(iStdIn, out uint inConsoleMode))
        {
            Console.WriteLine("[Windows Console Color Hack] failed to get input console mode");
            return;
        }
        if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
        {
            Console.WriteLine("[Windows Console Color Hack] failed to get output console mode");
            return;
        }

        inConsoleMode |= ENABLE_VIRTUAL_TERMINAL_INPUT;
        outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

        if (!SetConsoleMode(iStdIn, inConsoleMode))
        {
            Console.WriteLine($"[Windows Console Color Hack] failed to set input console mode, error code: {GetLastError()}");
            return;
        }

        if (!SetConsoleMode(iStdOut, outConsoleMode))
        {
            Console.WriteLine($"[Windows Console Color Hack] failed to set output console mode, error code: {GetLastError()}");
            return;
        }
    }
}