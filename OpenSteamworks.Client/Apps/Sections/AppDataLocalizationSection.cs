using OpenSteamworks.Client.Utils;
using OpenSteamworks.Enums;
using ValveKeyValue;

namespace OpenSteamworks.Client.Apps.Sections;

public class AppDataLocalizationSection : KVObjectEx
{
    public class AppLocalization : KVObjectEx {
        public IDictionary<string, string> Tokens => EmptyStringDictionaryIfUnset("tokens"); 
        public AppLocalization(KVObject kv) : base(kv) { }
    }

    public AppLocalization? GetLocalization(ELanguage language) {
        string langStr = ELanguageConversion.APINameFromELanguage(language);
        return DefaultIfUnset(langStr, (kv) => new AppLocalization(kv));
    }

    public AppDataLocalizationSection(KVObject kv) : base(kv) { }
}