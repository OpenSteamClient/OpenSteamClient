/// <summary>
/// Guards against calling API's that are blacklisted in cross-process scenarios
/// Without this, calling the function will crash the remote process.
/// This intelligently detects whether you're in a cross process scenario and will throw an exception instead.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class BlacklistedInCrossProcessIPCAttribute : System.Attribute
{    
    public BlacklistedInCrossProcessIPCAttribute()
    {

    }
}