namespace OpenSteamworks.Attributes;

[System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
/// <summary>
/// Allows you to register callback handlers in a ClientInterface inheriting class.
/// </summary>
sealed public class CallbackListenerAttribute<TCallback> : System.Attribute
{
    public CallbackListenerAttribute()
    {
        
    }
}