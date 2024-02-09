namespace Installer.Core;

//TODO: copy this to OpenSteamworks?
[System.Serializable]
public class LocalizedException : System.Exception
{
    public LocalizedException() { }
    public LocalizedException(string loctoken) : base(AvaloniaApp.TranslationManager.GetTranslationForKey(loctoken)) { }
    public LocalizedException(string loctoken, System.Exception inner) : base(AvaloniaApp.TranslationManager.GetTranslationForKey(loctoken), inner) { }
    protected LocalizedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}