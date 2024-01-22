using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;
using OpenSteamworks.KeyValues;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataLocalizationSection : TypedKVObject
{
    public class AppLocalization : TypedKVObject {
        public IDictionary<string, string> Tokens => EmptyStringDictionaryIfUnset("tokens"); 
        public AppLocalization(KVObject kv) : base(kv) { }
    }

    public AppLocalization? GetLocalization(ELanguage language) {
        string langStr = ELanguageConversion.APINameFromELanguage(language);
        return DefaultIfUnset(langStr, (kv) => new AppLocalization(kv));
    }

    public AppDataLocalizationSection(KVObject kv) : base(kv) { }
}