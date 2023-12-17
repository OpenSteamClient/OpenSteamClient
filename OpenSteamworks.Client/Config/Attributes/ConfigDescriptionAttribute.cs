namespace OpenSteamworks.Client.Config.Attributes;

[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class ConfigDescriptionAttribute : System.Attribute
{
    public string Description { get; init; }
    public string TranslationKey { get; init; }

    public ConfigDescriptionAttribute(string description, string translationKey)
    {
        this.Description = description;
        this.TranslationKey = translationKey;
    }
}