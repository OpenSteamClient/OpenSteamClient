namespace OpenSteamworks.Attributes;

[System.AttributeUsage(System.AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
/// <summary>
/// Allows you to serialize custom value types properly for JIT. Requires a field named _value (can be private), and implicit operators for to-from conversion to the native type.
/// </summary>
sealed public class CustomValueTypeAttribute : System.Attribute
{
    public CustomValueTypeAttribute()
    {
        
    }
}