namespace OpenSteamworks.Client.Utils.DI;

[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ImplementsInterfaceAttribute<T> : System.Attribute
{
    public ImplementsInterfaceAttribute()
    {

    }
}