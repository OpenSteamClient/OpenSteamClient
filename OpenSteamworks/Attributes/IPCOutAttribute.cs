namespace OpenSteamworks.Attributes;

/// <summary>
/// Indicates that the passed object will marshal data out, but it's input value is ignored.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
sealed class IPCOutAttribute : System.Attribute
{    
    public IPCOutAttribute() { }
}