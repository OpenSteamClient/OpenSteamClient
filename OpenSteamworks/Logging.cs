namespace OpenSteamworks;

public static class Logging {
    // Logging
    public static ILogger GeneralLogger { internal get; set; } = new DefaultConsoleLogger();
    /// <summary>
    /// The logger used explicitly for messages coming straight from the underlying steamclient library.
    /// Or in the case of IPCClient, messages from IPCClient
    /// </summary>
    public static ILogger NativeClientLogger { internal get; set; } = new DefaultConsoleLogger();
    /// <summary>
    /// Logs all IPC activity from IPCClient
    /// </summary>
    public static ILogger IPCLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger CallbackLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger JITLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger ConCommandsLogger { internal get; set; } = new DefaultConsoleLogger();
    public static ILogger MessagingLogger { internal get; set; } = new DefaultConsoleLogger();
    /// <summary>
    /// The logger used for CUtl types
    /// </summary>
    public static ILogger CUtlLogger { internal get; set; } = new DefaultConsoleLogger();
    public static bool LogIncomingCallbacks { internal get; set; } = false;
    public static bool LogCallbackContents { internal get; set; } = false;
}