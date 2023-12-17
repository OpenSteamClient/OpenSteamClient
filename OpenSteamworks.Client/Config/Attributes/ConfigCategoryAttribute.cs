namespace OpenSteamworks.Client.Config.Attributes;

[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
sealed class ConfigCategoryAttribute : System.Attribute
{
    public string CategoryName { get; init; }
    public string TranslationKey { get; init; }

    public ConfigCategoryAttribute(string categoryName, string translationKey)
    {
        this.CategoryName = categoryName;
        this.TranslationKey = translationKey;
    }
}