using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace OpenSteamworks.Client.Utils;

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
    /// <exception cref="Exception"></exception>
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
}
