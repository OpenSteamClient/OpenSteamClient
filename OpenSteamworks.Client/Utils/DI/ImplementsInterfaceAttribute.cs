namespace OpenSteamworks.Client.Utils.DI;

[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class DIRegisterInterfaceAttribute<T> : System.Attribute
{
    public DIRegisterInterfaceAttribute()
    {

    }
}