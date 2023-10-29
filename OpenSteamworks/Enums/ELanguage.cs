using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenSteamworks.Enums;

public enum ELanguage
{
	None = -1,
	English = 0,
	German = 1,
	French = 2,
	Italian = 3,
	Korean = 4,
	Spanish = 5,
	Simplified_Chinese = 6,
	Traditional_Chinese = 7,
	Russian = 8,
	Thai = 9,
	Japanese = 10,
	Portuguese = 11,
	Polish = 12,
	Danish = 13,
	Dutch = 14,
	Finnish = 15,
	Norwegian = 16,
	Swedish = 17,
	Hungarian = 18,
	Czech = 19,
	Romanian = 20,
	Turkish = 21,
	Brazilian = 22,
	Bulgarian = 23,
	Greek = 24,
	Arabic = 25,
	Ukrainian = 26,
	Latam_Spanish = 27,
	Vietnamese = 28,
	MAX = 29,
}

public static class ELanguageConversion {
    private static readonly Dictionary<ELanguage, string> ELanguageToAPINameMap = new() {
        {ELanguage.None, ""},
        {ELanguage.English, "english"},
        {ELanguage.German, "german"},
        {ELanguage.French, "french"},
        {ELanguage.Italian, "italian"},
        {ELanguage.Korean, "korean"},
        {ELanguage.Spanish, "spanish"},
        {ELanguage.Simplified_Chinese, "schinese"}, // Is this correct?
        {ELanguage.Traditional_Chinese, "tchinese"}, // Is this correct?
        {ELanguage.Russian, "russian"},
        {ELanguage.Thai, "thai"},
        {ELanguage.Japanese, "japanese"},
        {ELanguage.Portuguese, "portuguese"},
        {ELanguage.Polish, "polish"},
        {ELanguage.Danish, "danish"},
        {ELanguage.Dutch, "dutch"},
        {ELanguage.Finnish, "finnish"},
        {ELanguage.Norwegian, "norwegian"},
        {ELanguage.Swedish, "swedish"},
        {ELanguage.Hungarian, "hungarian"},
        {ELanguage.Czech, "czech"},
        {ELanguage.Romanian, "romanian"},
        {ELanguage.Turkish, "turkish"},
        {ELanguage.Brazilian, "brazilian"},
        {ELanguage.Bulgarian, "bulgarian"},
        {ELanguage.Greek, "greek"},
        {ELanguage.Arabic, "arabic"},
        {ELanguage.Ukrainian, "ukrainian"},
        {ELanguage.Latam_Spanish, "latam_spanish"},
        {ELanguage.Vietnamese, "vietnamese"},
    };

	public readonly static Dictionary<string, ELanguage> APINameToELanguageMap = ELanguageToAPINameMap.ToDictionary(x => x.Value, x => x.Key);

	public static ELanguage ELanguageFromAPIName(string apiName) {
        return APINameToELanguageMap[apiName];
    }

	public static string APINameFromELanguage(ELanguage eLanguage) {
        return ELanguageToAPINameMap[eLanguage];
    }
}