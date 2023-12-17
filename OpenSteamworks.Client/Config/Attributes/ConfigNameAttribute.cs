namespace OpenSteamworks.Client.Config.Attributes;

[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class ConfigNameAttribute : System.Attribute
{
    public string Name { get; init; }
    public string TranslationKey { get; init; }

    public ConfigNameAttribute(string name, string translationKey)
    {
        this.Name = name;
        this.TranslationKey = translationKey;
    }
}