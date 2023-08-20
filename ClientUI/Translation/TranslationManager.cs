using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using ClientUI.Extensions;

using OpenSteamworks;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;

namespace ClientUI.Translation;

public class Translation {
    public ELanguage Language { get; set; } = ELanguage.None;
    public string LanguageFriendlyName { get; set; } = "";
    public Dictionary<string, string> TranslationKeys { get; set; } = new();
}

public class TranslationManager : Component {
    public Translation CurrentTranslation = new Translation();
    private List<Visual> RefreshableVisuals = new();

    public TranslationManager(IContainer container) : base(container) {
        
    }
    public void SetLanguage(ELanguage language) {
        var lang = ELanguageToString(language);
        if (lang == null) {
            throw new ArgumentException($"Language {language} was not valid.");
        }

        this.GetComponent<IClientUser>().SetLanguage(lang);
        CurrentTranslation = GetForLanguage(language);
        foreach (var visual in RefreshableVisuals)
        {
            TranslateVisual(visual);
        }

        this.GetComponent<GlobalSettings>().Language = language;
    }

    public string GetTranslationForKey(string key) {
        this.CurrentTranslation.TranslationKeys.TryGetValue(key, out string? val);
        if (val == null) {
            throw new ArgumentException("Key " + key + " is not valid or not specified in current translation.");
        }
        
        return val;
    }
    
    private Translation GetForLanguage(ELanguage language) {
        string? filename = TranslationManager.ELanguageToString(language);
        if (filename == null) {
            throw new ArgumentOutOfRangeException("Invalid ELanguage " + language + " specified.");
        }

        string fullPath = Path.Combine(this.GetComponent<ConfigManager>().AssemblyDirectory, "Translations", filename+".json");
        return OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(JsonSerializer.Deserialize<Translation>(File.ReadAllText(fullPath)));
    } 

    public void TranslateVisual(Visual visual) {
        if (!RefreshableVisuals.Contains(visual)) {
            RefreshableVisuals.Add(visual);
        }
        
        foreach (var vis in visual.GetAllVisualChildrenTree())
        {
            string? translationKey = (string?)(vis[Controls.Translatable.TranslationKeyProperty]);
            if (!string.IsNullOrEmpty(translationKey)) {
                string translatedText = "TRANSLATION FAILED";
                if (!this.CurrentTranslation.TranslationKeys.ContainsKey(translationKey)) {
                    Console.WriteLine("Cannot translate " + translationKey + ", no key!");
                } else {
                    translatedText = this.CurrentTranslation.TranslationKeys[translationKey];
                }

                // Window eventually inherits from ContentControl. We don't want to override a window's content...
                if (vis is Window)
                {
                    (vis as Window)!.Title = translatedText;
                } else if (vis is TextBox) {
                    (vis as TextBox)!.Watermark = translatedText;
                } else if (vis is ContentControl) {
                    (vis as ContentControl)!.Content = translatedText;
                } else if (vis is TextBlock) {
                    (vis as TextBlock)!.Text = translatedText;
                }
            }
        }
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

    public override async Task RunStartup()
    {
        this.SetLanguage(this.GetComponent<GlobalSettings>().Language);
        await EmptyAwaitable();
    }

    public override async Task RunShutdown()
    {
        await EmptyAwaitable();
    }
}