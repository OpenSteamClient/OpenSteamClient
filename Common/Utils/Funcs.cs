using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Common.Utils;

public static class Funcs {
#if DEBUG 
    private const bool IS_DEBUG = true;
#else 
    private const bool IS_DEBUG = false;
#endif

    private const bool SHOULD_THROW_ON_ASSERT = IS_DEBUG;
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
    public static bool Assert(bool condition, 
    bool throwOnFailInReleaseBuilds = false,
    [CallerArgumentExpression(nameof(condition))] string conditionStr = "", 
    [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        if (!condition)
        {
            string message = $"{sourceFilePath} ({sourceLineNumber}) : Assertion failed: {conditionStr}";
            Console.WriteLine(message);
            if (SHOULD_THROW_ON_ASSERT || throwOnFailInReleaseBuilds) {
                throw new Exception(message);
            }
        }

        return condition;
    }
}
