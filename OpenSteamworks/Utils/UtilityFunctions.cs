using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Linq;
using OpenSteamworks.Native;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenSteamworks.Utils;

public static class UtilityFunctions {
    public static string GetPlatformString() {
        if (OperatingSystem.IsWindows()) {
            return "windows";
        } else if (OperatingSystem.IsLinux()) {
            return "linux";
        } else if (OperatingSystem.IsMacOS()) {
            return "osx";
        } else {
            throw new Exception("Unsupported platform in GetPlatformString");
        }
    }

    public static string GetSteamPlatformString() {
        if (OperatingSystem.IsWindows()) {
            return "windows";
        } else if (OperatingSystem.IsLinux()) {
            return "linux";
        } else if (OperatingSystem.IsMacOS()) {
            return "macos";
        } else {
            throw new Exception("Unsupported platform in GetSteamPlatformString");
        }
    }

    public static void Assert([DoesNotReturnIf(false)] bool condition,
    [CallerArgumentExpression(nameof(condition))] string conditionStr = "", 
    [CallerFilePath] string sourceFilePath = "",
    [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!condition)
        {
            string message = $"{sourceFilePath} ({sourceLineNumber}) : Assertion failed: {conditionStr}";
            Console.WriteLine(message);
            throw new Exception(message);
        }
    }

    public static T AssertNotNull<T>([NotNull] T? val, 
    [CallerArgumentExpression(nameof(val))] string valStr = "", 
    [CallerFilePath] string sourceFilePath = "",
    [CallerLineNumber] int sourceLineNumber = 0) {
        Assert(val != null, $"{valStr} != null", sourceFilePath, sourceLineNumber);
        return val;
    }

    public static unsafe void AssertValidStringPtr(IntPtr ptr,
    [CallerArgumentExpression(nameof(ptr))] string valStr = "", 
    [CallerFilePath] string sourceFilePath = "",
    [CallerLineNumber] int sourceLineNumber = 0) {
        Assert(ptr != IntPtr.Zero, $"{valStr} != IntPtr.Zero", sourceFilePath, sourceLineNumber);
        Assert(((byte*)ptr)[0] != 0, $"{valStr}[0] != 0", sourceFilePath, sourceLineNumber);
    }

    private static readonly Random random = new();
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Sets an environment variable properly. <br/> 
    /// The default runtime implementation just adds environment variables to an internal array. WTF Microsoft???
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="overwrite"></param>
    public static void SetEnvironmentVariable(string name, string? value, bool overwrite = true) {
        if (OperatingSystem.IsWindows()) {
            [DllImport("kernel32", SetLastError = true)]
            static extern bool SetEnvironmentVariable([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string? value);
            if (overwrite == false && GetEnvironmentVariable(name) != null) {
                return;
            }

            if (SetEnvironmentVariable(name, value) == false) {
                throw new Exception("Setting environment variable failed, errno: " + Marshal.GetLastWin32Error());
            }
        } else {
            [DllImport("libc", SetLastError = true)]
            static extern int setenv([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string? value, int overwrite);

            [DllImport("libc", SetLastError = true)]
            static extern int unsetenv([MarshalAs(UnmanagedType.LPUTF8Str)] string name);

            if (value == null) {
                if (unsetenv(name) == -1) {
                    throw new Exception("Setting environment variable failed, errno: " + Marshal.GetLastWin32Error());
                }
                return;
            }

            if (setenv(name, value, Convert.ToInt32(overwrite)) == -1) {
                throw new Exception("Setting environment variable failed, errno: " + Marshal.GetLastWin32Error());
            }
            return;
        }
    }
    
    /// <summary>
    /// Gets an environment variable properly. <br/> 
    /// The default runtime implementation just reads environment variables from an internal array (copied only at program startup). WTF Microsoft???
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="overwrite"></param>
    /// <exception cref="Exception"></exception>
    public static unsafe string? GetEnvironmentVariable(string name) {
        if (OperatingSystem.IsWindows()) {
            [DllImport("kernel32", SetLastError = true)]
            static extern DWORD GetEnvironmentVariable([MarshalAs(UnmanagedType.LPUTF8Str)] string name, StringBuilder buffer, DWORD size);
            StringBuilder buffer = new(1024);
            var length = GetEnvironmentVariable(name, buffer, (uint)buffer.Length);
            var err = Marshal.GetLastWin32Error();
        
        test:
            if (length == 0) {
                return null;
            }

            // ERROR_ENVVAR_NOT_FOUND
            if (err == 203) {
                return null;
            }

            if (length > (uint)buffer.Length) {
                buffer = new((int)length);
                length = GetEnvironmentVariable(name, buffer, length);
                goto test;
            }

            return buffer.ToString();

        } else {
            [DllImport("libc")]
            static extern void* getenv([MarshalAs(UnmanagedType.LPUTF8Str)] string name);
            var ret = getenv(name);
            if (ret == null) {
                return null;
            }

            return Marshal.PtrToStringUTF8((IntPtr)ret);
        }
    }

    public static unsafe IDictionary<string, string?> GetEnvironmentVariables() {
        var results = new Dictionary<string, string?>();
        if (OperatingSystem.IsWindows()) {
            // This implementation is copied from the runtime, they do it fine on Windows here
            [DllImport("kernel32", SetLastError = true)]
            static extern char* GetEnvironmentStringsW();

            [DllImport("kernel32", SetLastError = true)]
            static extern bool FreeEnvironmentStringsW(char* env);

            char* stringPtr = GetEnvironmentStringsW();
            if (stringPtr == null)
            {
                throw new OutOfMemoryException();
            }

            try
            {
                char* currentPtr = stringPtr;
                while (true)
                {
                    ReadOnlySpan<char> variable = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(currentPtr);
                    if (variable.IsEmpty)
                    {
                        break;
                    }

                    // Find the = separating the key and value. We skip entries that begin with =.  We also skip entries that don't
                    // have =, which can happen on some older OSes when the environment block gets corrupted.
                    int i = variable.IndexOf('=');
                    if (i > 0)
                    {
                        // Add the key and value.
                        string key = new(variable[..i]);
                        string value = new(variable[(i + 1)..]);
                        try
                        {
                            // Add may throw if the environment block was corrupted leading to duplicate entries.
                            // We allow such throws and eat them (rather than proactively checking for duplication)
                            // to provide a non-fatal notification about the corruption.
                            results.Add(key, value);
                        }
                        catch (ArgumentException) { }
                    }

                    // Move to the end of this variable, after its terminator.
                    currentPtr += variable.Length + 1;
                }

                return results;
            }
            finally
            {
                FreeEnvironmentStringsW(stringPtr);
            }

        } else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS()) {
            // And this is terrible. Why does the runtime fake environment variables on Linux????

            //TODO: the below code doesn't work (for some reason)
            // [DllImport("libc", SetLastError = true)]
            // static extern void* dlsym(void* handle, string symbol);

            // IntPtr environ = (IntPtr)dlsym((void*)0, "environ");

            // So we resort to using dotnet's internal API. This is bad and could break at any time.
            [DllImport("libSystem.Native.so", EntryPoint = "SystemNative_GetEnviron")]
            static extern IntPtr SystemNative_GetEnviron();

            var environ = SystemNative_GetEnviron();
            if (environ == IntPtr.Zero) {
                throw new Exception("Environ is null");
            }

            IntPtr block = environ;
            if (block != IntPtr.Zero)
            {
                IntPtr blockIterator = block;

                // Per man page, environment variables come back as an array of pointers to strings
                // Parse each pointer of strings individually
                while (ParseEntry(blockIterator, out string? key, out string? value))
                {
                    if (key != null && value != null)
                    {
                        try
                        {
                            // Add may throw if the environment block was corrupted leading to duplicate entries.
                            // We allow such throws and eat them (rather than proactively checking for duplication)
                            // to provide a non-fatal notification about the corruption.
                            results.Add(key, value);
                        }
                        catch (ArgumentException) { }
                    }

                    // Increment to next environment variable entry
                    blockIterator += IntPtr.Size;
                }
            }

            return results;

        } else {
            throw new NotSupportedException("os unsupported");
        }
    }

    // Use a local, unsafe function since we cannot use `yield return` inside of an `unsafe` block
    private static unsafe bool ParseEntry(IntPtr current, out string? key, out string? value)
    {
        // Setup
        key = null;
        value = null;

        // Point to current entry
        byte* entry = *(byte**)current;

        // Per man page, "The last pointer in this array has the value NULL"
        // Therefore, if entry is null then we're at the end and can bail
        if (entry == null)
            return false;

        // Parse each byte of the entry until we hit either the separator '=' or '\0'.
        // This finds the split point for creating key/value strings below.
        // On some old OS, the environment block can be corrupted.
        // Some will not have '=', so we need to check for '\0'.
        byte* splitpoint = entry;
        while (*splitpoint != '=' && *splitpoint != '\0')
            splitpoint++;

        // Skip over entries starting with '=' and entries with no value (just a null-terminating char '\0')
        if (splitpoint == entry || *splitpoint == '\0')
            return true;

        // The key is the bytes from start (0) until our splitpoint
        key = new string((sbyte*)entry, 0, checked((int)(splitpoint - entry)));
        // The value is the rest of the bytes starting after the splitpoint
        value = new string((sbyte*)(splitpoint + 1));

        return true;
    }

    /// <summary>
    /// Checks if a file exists on PATH or the current directory.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool FileExistsOnPath(string fileName)
    {
        return FindFileOnPath(fileName) != null;
    }
    
    /// <summary>
    /// Gets an executable given it's name from the PATH. Returns null if not found.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string? FindFileOnPath(string fileName)
    {
        if (File.Exists(fileName))
            return Path.GetFullPath(fileName);

        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(pathEnv)) {
            return null;
        }
        
        foreach (var path in pathEnv.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(path, fileName);
            if (File.Exists(fullPath)) {
                return fullPath;
            }

            // Also support .exe files
            var fullPathExe = Path.Combine(path, fileName + ".exe");
            
            if (File.Exists(fullPathExe)) {
                return fullPathExe;
            }
        }
        return null;
    }

    public static string? FindFileOnLibraryPath(string libraryName) {
        if (File.Exists(libraryName))
            return Path.GetFullPath(libraryName);

        string? pathEnv;
        string systemFileExtension;
        if (OperatingSystem.IsLinux()) {
            pathEnv = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
            systemFileExtension = ".so";
        } else if (OperatingSystem.IsMacOS()) {
            pathEnv = Environment.GetEnvironmentVariable("DYLD_LIBRARY_PATH");
            systemFileExtension = ".dylib";
        } else if (OperatingSystem.IsWindows()) {
            //TODO: windows has like 8 different sources DLLs can be found from. WTF Microsoft? (we should probably support those eventually)
            pathEnv = $"C:\\Windows\\System32{Path.PathSeparator}";
            systemFileExtension = ".dll";
        } else {
            throw new NotSupportedException("OS unsupported");
        }

        if (pathEnv == null) {
            return "";
        }

        if (string.IsNullOrEmpty(pathEnv)) {
            return null;
        }
        
        foreach (var path in pathEnv.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(path, libraryName);
            if (File.Exists(fullPath)) {
                return fullPath;
            }

            // Also support .exe files
            var fullPathDll = Path.Combine(path, libraryName + systemFileExtension);
            
            if (File.Exists(fullPathDll)) {
                return fullPathDll;
            }
        }
        return null;
    }

    public unsafe static string FormatPtr(void* ptr) {
        return FormatPtr((IntPtr)ptr);
    }

    public unsafe static string FormatPtr(IntPtr ptr) {
        return string.Format("0x{0:x}", (IntPtr)ptr);
    }
}
