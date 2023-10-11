namespace OpenSteamworks.Attributes;

[System.AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
/// <summary>
/// Allows you to register callback handlers by ID. Does not support retrieving data.
/// </summary>
sealed public class CallbackIDListenerAttribute : System.Attribute
{
    public readonly int CallbackID;
    public CallbackIDListenerAttribute(int callbackID)
    {
        this.CallbackID = callbackID;
    }
}