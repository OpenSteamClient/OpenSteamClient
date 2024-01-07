namespace OpenSteamworks.Attributes;

/// <summary>
/// Indicates that the passed object will marshal data in, but it's output value should be ignored.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
sealed class IPCInAttribute : System.Attribute
{    
    public IPCInAttribute() { }
}