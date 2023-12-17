namespace OpenSteamworks.Client.Config.Attributes;

/// <summary>
/// This config property will never be shown to the end user. 
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class ConfigNeverVisibleAttribute : System.Attribute
{
    public ConfigNeverVisibleAttribute() { }
}