using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

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
}
