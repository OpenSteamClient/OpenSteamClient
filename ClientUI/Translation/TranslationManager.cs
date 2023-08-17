using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Common.Autofac;
using Common.Managers;
using OpenSteamworks;
using OpenSteamworks.Enums;

namespace ClientUI.Translation;

public class Translation {
    public ELanguage Language = ELanguage.None;
    public string LanguageFriendlyName = "";
    public Dictionary<string, string> TranslationKeys = new();
}

public class TranslationManager : IHasStartupTasks {
    public required SteamClient steamClient { protected get; init; }
    public required ConfigManager configManager { protected get; init; }
    public Translation? CurrentTranslation;
    public void SetLanguage(ELanguage language) {
        var lang = ELanguageToString(language);
        if (lang == null) {
            throw new ArgumentException($"Language {language} was not valid.");
        }
        steamClient.NativeClient.IClientUser.SetLanguage(lang);
        CurrentTranslation = GetForLanguage(language);
    }
    
    private Translation GetForLanguage(ELanguage language) {
        string? filename = TranslationManager.ELanguageToString(language);
        if (filename == null) {
            throw new ArgumentOutOfRangeException("Invalid ELanguage " + language + " specified.");
        }

        string fullPath = Path.Combine(configManager.InstallDir, "Translations", filename+".json");
        return Common.Utils.UtilityFunctions.AssertNotNull(JsonSerializer.Deserialize<Translation>(fullPath));
    } 

    public static string? ELanguageToString(ELanguage lang) {
        switch (lang)
        {
            case ELanguage.None:
                break;
            case ELanguage.English:
                return "english";
            case ELanguage.German:
                return "german";
            case ELanguage.French:
                return "french";
            case ELanguage.Italian:
                return "italian";
            case ELanguage.Korean:
                return "korean";
            case ELanguage.Spanish:
                return "spanish";
            case ELanguage.Simplified_Chinese:
                return "schinese"; // Is this correct?
            case ELanguage.Traditional_Chinese:
                return "tchinese"; // Is this correct?
            case ELanguage.Russian:
                return "russian";
            case ELanguage.Thai:
                return "thai";
            case ELanguage.Japanese:
                return "japanese";
            case ELanguage.Portuguese:
                return "portuguese";
            case ELanguage.Polish:
                return "polish";
            case ELanguage.Danish:
                return "danish";
            case ELanguage.Dutch:
                return "dutch";
            case ELanguage.Finnish:
                return "finnish";
            case ELanguage.Norwegian:
                return "norwegian";
            case ELanguage.Swedish:
                return "swedish";
            case ELanguage.Hungarian:
                return "hungarian";
            case ELanguage.Czech:
                return "czech";
            case ELanguage.Romanian:
                return "romanian";
            case ELanguage.Turkish:
                return "turkish";
            case ELanguage.Brazilian:
                return "brazilian";
            case ELanguage.Bulgarian:
                return "bulgarian";
            case ELanguage.Greek:
                return "greek";
            case ELanguage.Arabic:
                return "arabic";
            case ELanguage.Ukrainian:
                return "ukrainian";
            case ELanguage.Latam_Spanish:
                return "latam_spanish"; // Is this correct?
            case ELanguage.Vietnamese:
                return "vietnamese";
            case ELanguage.MAX:
                break;
            default:
                break;
        }
        return null;
    }

    void IHasStartupTasks.RunStartup()
    {
        this.SetLanguage(configManager.GlobalSettings.Language);
    }
}