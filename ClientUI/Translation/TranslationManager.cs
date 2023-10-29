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

public class TranslationManager : IClientLifetime {
    public Translation CurrentTranslation = new();
    private readonly List<AvaloniaObject> RefreshableObjects = new();
    private readonly IClientUser iClientUser;
    private readonly ConfigManager configManager;
    private readonly GlobalSettings globalSettings;

    public TranslationManager(IClientUser iClientUser, ConfigManager configManager, GlobalSettings globalSettings) {
        this.iClientUser = iClientUser;
        this.configManager = configManager;
        this.globalSettings = globalSettings;
    }

    public void SetLanguage(ELanguage language) {
        var lang = ELanguageToString(language);
        if (lang == null) {
            throw new ArgumentException($"Language {language} was not valid.");
        }

        iClientUser.SetLanguage(lang);
        CurrentTranslation = GetForLanguage(language);
        foreach (var obj in RefreshableObjects)
        {
            TranslateAvaloniaObject(obj);
        }

        globalSettings.Language = language;
        globalSettings.Save();
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

        string fullPath = Path.Combine(configManager.AssemblyDirectory, "Translations", filename+".json");
        return OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(JsonSerializer.Deserialize<Translation>(File.ReadAllText(fullPath)));
    } 
    public void TranslateVisual(Visual visual) {
        foreach (var vis in visual.GetAllVisualChildrenTree())
        {
            TranslateAvaloniaObject(vis);
        }
    }
    public void TranslateTrayIcon(TrayIcon icon) {
        List<AvaloniaObject> objs = new()
        {
            icon
        };

        if (icon.Menu != null) {
            objs.Add(icon.Menu);
            foreach (var item in icon.Menu.Items)
            {
                objs.Add(item);
            }
        }

        foreach (var item in objs)
        {
            TranslateAvaloniaObject(item);
        }
    }
    public void TranslateAvaloniaObject(AvaloniaObject obj) {
        if (!RefreshableObjects.Contains(obj)) {
            RefreshableObjects.Add(obj);
        }
        
        string? translationKey = (string?)obj[Controls.Translatable.TranslationKeyProperty];
        if (!string.IsNullOrEmpty(translationKey)) {
            bool translationFailed = false;
            string translatedText = "TRANSLATION FAILED";
            if (!this.CurrentTranslation.TranslationKeys.ContainsKey(translationKey)) {
                translationFailed = true;
                Console.WriteLine("Cannot translate " + translationKey + ", no key!");
            } else {
                translatedText = this.CurrentTranslation.TranslationKeys[translationKey];
            }

            void TranslateTextInternal<T>(StyledProperty<T> property) {
                bool isEmptyOrNull = false;
                T val = obj.GetValue(property);
                if (val == null) {
                    isEmptyOrNull = true;
                } else if (val is string) {
                    isEmptyOrNull = string.IsNullOrEmpty(val as string);
                }

                // Don't replace text with TRANSLATION FAILED if there's pre-existing text in the control
                if (translationFailed && !isEmptyOrNull) {
                    return;
                }

                obj.SetValue(property, translatedText);
            }

            // Window eventually inherits from ContentControl, as do lots of other controls. We don't want to override a window's content...
            if (obj is Window) {
                TranslateTextInternal(Window.TitleProperty);
            } else if (obj is MenuItem) {
                TranslateTextInternal(MenuItem.HeaderProperty);
            } else if (obj is TextBox) {
                TranslateTextInternal(TextBox.WatermarkProperty);
            } else if (obj is ContentControl) {
                TranslateTextInternal(ContentControl.ContentProperty);
            } else if (obj is TextBlock) {
                TranslateTextInternal(TextBlock.TextProperty);
            } else if (obj is NativeMenuItem) {
                TranslateTextInternal(NativeMenuItem.HeaderProperty);
            } else if (obj is TrayIcon) {
                TranslateTextInternal(TrayIcon.ToolTipTextProperty);
            }
        }
    }
    
    public static string? ELanguageToString(ELanguage lang) {
        try
        {
            return ELanguageConversion.APINameFromELanguage(lang);
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public async Task RunStartup()
    {
        await Task.Run(() => this.SetLanguage(globalSettings.Language));
    }

    public async Task RunShutdown()
    {
        await Task.CompletedTask;
    }
}