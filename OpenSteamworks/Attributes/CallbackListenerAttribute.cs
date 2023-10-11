namespace OpenSteamworks.Attributes;

[System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
/// <summary>
/// Allows you to register callback handlers by type. Needs the function to have exactly one parameter, which matches TCallback.
/// </summary>
sealed public class CallbackListenerAttribute<TCallback> : System.Attribute
{
    public CallbackListenerAttribute()
    {
        
    }
}