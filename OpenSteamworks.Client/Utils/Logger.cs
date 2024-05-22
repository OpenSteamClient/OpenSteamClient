using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.VisualBasic;

namespace OpenSteamworks.Client;

public class Logger : ILogger {
    public enum Level
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    private struct LogData {
        public Logger logger;
        public DateTime Timestamp;
        public Level Level;
        public string Message;
        public string Category;
        public bool FullLine;

        public LogData(Logger logger, Level level, string msg, string category, bool fullLine) {
            this.logger = logger;
            this.Timestamp = DateTime.Now;
            this.Level = level;
            this.Message = msg;
            this.Category = category;
            this.FullLine = fullLine;
        }
    }

    public string Name { get; set; } = "";
    public string? LogfilePath { get; init; } = null;

    /// <summary>
    /// Should the logger prefix messages it receives?
    /// </summary>
    public bool AddPrefix { get; set; } = true;

    /// <summary>
    /// If this logger is a sublogger, this is it's name.
    /// </summary>
    private string subLoggerName { get; set; } = "";

    /// <summary>
    /// If this logger is a sublogger, this is it's parent.
    /// </summary>
    private Logger? parentLogger { get; set; }

    // https://no-color.org/
    private static bool disableColors = Environment.GetEnvironmentVariable("NO_COLOR") != null;
    private object logStreamLock = new();
    private FileStream? logStream;
    private static List<Logger> loggers = new();
    private static readonly Thread logThread;

    public class DataReceivedEventArgs : EventArgs {
        public Level Level { get; set; }
        public string Text { get; set; }
        public string AnsiColorSequence { get; set; }
        public string AnsiResetCode { get; set; }
        public bool FullLine { get; set; }
        public DataReceivedEventArgs(Level level, string text, bool fullLine, string ansiColorSequence, string ansiResetCode) {
            this.Level = level;
            this.Text = text;
            this.FullLine = fullLine;
            this.AnsiColorSequence = ansiColorSequence;
            this.AnsiResetCode = ansiResetCode;
        }
    }

    public static event EventHandler<DataReceivedEventArgs>? DataReceived;

    static Logger() {
        logThread = new(LogThreadMain);
        logThread.Name = "LogThread";
        logThread.Start();
    }

    private Logger(string name, string? filepath = "") {
        this.Name = name;
        this.LogfilePath = filepath;

        loggers.Add(this);
        if (!string.IsNullOrEmpty(filepath)) {
            if (File.Exists(filepath)) {
                // Delete if over 4MB
                var fi = new FileInfo(filepath);
                if ((fi.Length / 1024 / 1024) > 4) {
                    fi.Delete();
                }
            }
            logStream = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            logStream.Seek(logStream.Length, SeekOrigin.Begin);
        }
        
        if (!hasRanWindowsHack && OperatingSystem.IsWindows()) {
            RunWindowsConsoleColorsHack();
        }
    }

    private static readonly ConcurrentQueue<LogData> dataToLog = new();
    private static void LogThreadMain() {
        while (true)
        {
            if (dataToLog.IsEmpty) {
                System.Threading.Thread.Sleep(50);
                continue;
            }

            if (dataToLog.TryDequeue(out LogData data)) {
                if (data.FullLine) {
                    MessageInternal(data.logger, data.Timestamp, data.Level, data.Message, data.Category);
                } else {
                    WriteInternal(data.logger, data.Message);
                }
            }
        }
    }

    public static Logger GetLogger(string name, string? filepath = "") {
        foreach (var item in loggers)
        {
            if (item.Name == name && item.LogfilePath == filepath) {
                return item;
            }
        }

        return new Logger(name, filepath);
    }

    /// <summary>
    /// Creates a sub-logger. Uses the logstream of the current logger, and sets subname as a category name for each print.
    /// </summary>
    /// <param name="subName"></param>
    /// <returns></returns>
    public Logger CreateSubLogger(string subName) {
        var logger = new Logger("", "");
        logger.subLoggerName = subName;
        logger.parentLogger = this;
        return logger;
    }

    private static void MessageInternal(Logger logger, DateTime timestamp, Level level, string message, string category) {
        if (logger.parentLogger != null) {
            var actualCategory = logger.subLoggerName;
            if (!string.IsNullOrEmpty(category)) {
                actualCategory += "/" + category;
            }
            
            MessageInternal(logger.parentLogger, timestamp, level, message, actualCategory);
            return;
        }

        string formatted = message;
        if (logger.AddPrefix) {
            // welp. we can't just use the system's date format, but we also need to use the system's time at the same time, which won't include milliseconds and will always have AM/PM appended, even on 24-hour clocks. So use the objectively better formatting system of dd/MM/yyyy and always use 24-hour time (which will also make it easier for the devs reviewing bug reports)
            formatted = string.Format("[{0} {1}{2}: {3}] {4}", timestamp.ToString("dd/MM/yyyy HH:mm:ss.ff"), logger.Name, string.IsNullOrEmpty(category) ? "" : $"/{category}", level.ToString(), message);
        }

        string ansiColorCode = string.Empty;
        string ansiResetCode = string.Empty;

        if (!disableColors) {
            ansiResetCode = "\x1b[0m";
            if (level == Level.FATAL) {
                ansiColorCode = "\x1b[91m";
            } else if (level == Level.ERROR) {
                ansiColorCode = "\x1b[31m";
            } else if (level == Level.WARNING) {
                ansiColorCode = "\x1b[33m";
            } else if (level == Level.INFO) {
                //ansiColorCode = "\x1b[37m";
            } else if (level == Level.DEBUG) {
                ansiColorCode = "\x1b[2;37m";
            }
        }

        Console.WriteLine(ansiColorCode + formatted + ansiResetCode);

        if (logger.logStream != null) {
            lock (logger.logStreamLock)
            {
                logger.logStream.Write(Encoding.Default.GetBytes(formatted + Environment.NewLine));
            }
        }

        DataReceived?.Invoke(logger, new(level, formatted + Environment.NewLine, true, ansiColorCode, ansiResetCode));
    }

    private void AddLine(Level level, string message, string category = "") {
        dataToLog.Enqueue(new LogData(this, level, message, category, true));
    }

    private void AddData(string message) {
        dataToLog.Enqueue(new LogData(this, Level.INFO, message, string.Empty, false));
    }

    private static void WriteInternal(Logger logger, string message) {
        Console.Write(message);
        if (logger.logStream != null) {
            lock (logger.logStreamLock)
            {
                logger.logStream.Write(Encoding.Default.GetBytes(message));
            }
        }

        DataReceived?.Invoke(logger, new(Level.INFO, message, false, string.Empty, string.Empty));
    }

    /// <inheritdoc/>
    public void Write(string message) {
        AddData(message);
    }

    public void Message(Level level, string message) {
        AddLine(level, message);
    }
    
    public void Message(Level level, string message, string category) {
        AddLine(level, message, category);
    }
    
    public void Message(Level level, string message, params object?[] formatObjs) {
        AddLine(level, string.Format(message, formatObjs));
    }

    public void Message(Level level, string message, string category, params object?[] formatObjs) {
        AddLine(level, string.Format(message, formatObjs), category);
    }

    public void Trace(string message) {
        this.Debug(message);
    }

    public void Trace(string message, string category) {
        this.Debug(message, category);
    }

    public void Trace(string message, params object?[] formatObjs) {
        this.Debug(message, formatObjs);
    }

    public void Trace(string message, string category, params object?[] formatObjs) {
        this.Debug(message, category, formatObjs);
    }

    public void Debug(string message) {
        this.Message(Level.DEBUG, message);
    }

    public void Debug(string message, string category) {
        this.Message(Level.DEBUG, message, category);
    }

    public void Debug(string message, params object?[] formatObjs) {
        this.Message(Level.DEBUG, message, formatObjs);
    }

    public void Debug(string message, string category, params object?[] formatObjs) {
        this.Message(Level.DEBUG, message, category, formatObjs);
    }

    public void Info(string message) {
        this.Message(Level.INFO, message);
    }

    public void Info(string message, string category) {
        this.Message(Level.INFO, message, category);
    }

    public void Info(string message, params object?[] formatObjs) {
        this.Message(Level.INFO, message, formatObjs);
    }

    public void Info(string message, string category, params object?[] formatObjs) {
        this.Message(Level.INFO, message, category, formatObjs);
    }

    public void Warning(string message) {
        this.Message(Level.WARNING, message);
    }

    public void Warning(string message, string category) {
        this.Message(Level.WARNING, message, category);
    }

    public void Warning(string message, params object?[] formatObjs) {
        this.Message(Level.WARNING, message, formatObjs);
    }

    public void Warning(string message, string category, params object?[] formatObjs) {
        this.Message(Level.WARNING, message, category, formatObjs);
    }

    public void Warning(Exception e) {
        this.Message(Level.WARNING, e.ToString());
    }

    public void Error(string message) {
        this.Message(Level.ERROR, message);
    }

    public void Error(string message, string category) {
        this.Message(Level.ERROR, message, category);
    }

    public void Error(string message, params object?[] formatObjs) {
        this.Message(Level.ERROR, message, formatObjs);
    }

    public void Error(string message, string category, params object?[] formatObjs) {
        this.Message(Level.ERROR, message, category, formatObjs);
    }

    public void Error(Exception e) {
        this.Message(Level.ERROR, e.ToString());
    }

    public void Fatal(string message) {
        this.Message(Level.FATAL, message);
    }

    public void Fatal(string message, string category) {
        this.Message(Level.FATAL, message, category);
    }

    public void Fatal(string message, params object?[] formatObjs) {
        this.Message(Level.FATAL, message, formatObjs);
    }

    public void Fatal(string message, string category, params object?[] formatObjs) {
        this.Message(Level.FATAL, message, category, formatObjs);
    }

    public void Fatal(Exception e) {
        this.Message(Level.FATAL, e.ToString());
    }

    ~Logger() {
        loggers.Remove(this);
    }

    private static bool hasRanWindowsHack = false;

    /// <summary>
    /// Windows is stuck using legacy settings unless you tell it explicitly to use "ENABLE_VIRTUAL_TERMINAL_PROCESSING". Why???
    /// </summary>
    [SupportedOSPlatform("windows")]
    private static void RunWindowsConsoleColorsHack() {
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
            disableColors = true;
            return;
        }
        if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
        {
            Console.WriteLine("[Windows Console Color Hack] failed to get output console mode");
            disableColors = true;
            return;
        }

        inConsoleMode |= ENABLE_VIRTUAL_TERMINAL_INPUT;
        outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

        if (!SetConsoleMode(iStdIn, inConsoleMode))
        {
            Console.WriteLine($"[Windows Console Color Hack] failed to set input console mode, error code: {GetLastError()}");
            disableColors = true;
            return;
        }

        if (!SetConsoleMode(iStdOut, outConsoleMode))
        {
            Console.WriteLine($"[Windows Console Color Hack] failed to set output console mode, error code: {GetLastError()}");
            disableColors = true;
            return;
        }
    }
}