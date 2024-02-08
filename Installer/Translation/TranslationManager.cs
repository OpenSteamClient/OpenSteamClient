using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Installer.Extensions;
using Microsoft.Extensions.FileProviders;

namespace Installer.Translation;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Translation))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

public class Translation
{
    public ELanguage Language { get; set; } = ELanguage.None;
    public string LanguageFriendlyName { get; set; } = "";
    public string ISOCode { get; set; } = "";
    public Dictionary<string, string> TranslationKeys { get; set; } = new();
}

public class TranslationManager
{
    public Translation CurrentTranslation = new();
    private readonly List<AvaloniaObject> RefreshableObjects = new();
    public static IEnumerable<ELanguage> ValidUILanguages => new ELanguage[] { ELanguage.English, ELanguage.Finnish };
    public TranslationManager()
    {

    }

    public void SetLanguage(ELanguage language)
    {
        var lang = ELanguageToString(language);
        if (lang == null)
        {
            throw new ArgumentException($"Language {language} was not valid.");
        }

        if (!ValidUILanguages.Contains(language)) {
            throw new InvalidOperationException("No translation for " + language);
        }

        CurrentTranslation = GetForLanguage(language);

        Dispatcher.UIThread.Invoke(() =>
        {
            foreach (var obj in RefreshableObjects)
            {
                TranslateAvaloniaObject(obj);
            }
        });
    }

    public string GetTranslationForKey(string key)
    {
        this.CurrentTranslation.TranslationKeys.TryGetValue(key, out string? val);
        if (val == null)
        {
            val = "TRANSLATION FAILED";
        }

        return val;
    }

    private Translation GetForLanguage(ELanguage language)
    {
        string? langname = ELanguageToString(language);
        if (langname == null)
        {
            throw new ArgumentOutOfRangeException("Invalid ELanguage " + language + " specified.");
        }

        // Fetch the translations from this assembly. The translations are embedded in the installer
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        string filename = "Translations/" + langname + ".json";
        IFileInfo fileInfo = embeddedProvider.GetFileInfo(filename);
        if (!fileInfo.Exists) {
            if (language == ELanguage.English)
            {
                throw new Exception("Base language not found!");
            }

            throw new Exception("Language " + language + " not found");
        }

        string text;
        using (var stream = fileInfo.CreateReadStream())
        {
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
        }

        var newTranslation = JsonSerializer.Deserialize(text, SourceGenerationContext.Default.Translation);
        ArgumentNullException.ThrowIfNull(newTranslation);
        return newTranslation;
    }

    /// <summary>
    /// This function is for creating and translating elements in code behind
    /// </summary>
    public T CreateTranslated<T>(T visualCreated, string translationKey, string? defaultStr = null) where T : Visual
    {
        visualCreated[Controls.Translatable.TranslationKeyProperty] = translationKey;
        visualCreated[Controls.Translatable.DefaultTextProperty] = defaultStr;
        TranslateVisual(visualCreated);
        return visualCreated;
    }

    public void TranslateVisual(Visual visual)
    {
        foreach (var vis in visual.GetAllVisualChildrenTree())
        {
            Console.WriteLine("vis " + vis.Name);
            TranslateAvaloniaObject(vis);
        }
    }

    public void TranslateTrayIcon(TrayIcon icon)
    {
        List<AvaloniaObject> objs = new()
        {
            icon
        };

        if (icon.Menu != null)
        {
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
    public void TranslateAvaloniaObject(AvaloniaObject obj)
    {
        if (!RefreshableObjects.Contains(obj))
        {
            RefreshableObjects.Add(obj);
        }

        string? translationKey = (string?)obj[Controls.Translatable.TranslationKeyProperty];
        string? defaultStr = (string?)obj[Controls.Translatable.DefaultTextProperty];
        if (!string.IsNullOrEmpty(translationKey))
        {
            bool translationFailed = false;
            string translatedText = "TRANSLATION FAILED";
            if (!this.CurrentTranslation.TranslationKeys.ContainsKey(translationKey))
            {
                translationFailed = true;
                Console.WriteLine("Cannot translate " + translationKey + ", no key!");
            }
            else
            {
                translatedText = this.CurrentTranslation.TranslationKeys[translationKey];
                Console.WriteLine("Got translation " + translatedText);
            }

            void TranslateTextInternal<T>(StyledProperty<T> property)
            {
                bool isEmptyOrNull = false;
                T val = obj.GetValue(property);
                if (val == null)
                {
                    isEmptyOrNull = true;
                }
                else if (val is string str)
                {
                    isEmptyOrNull = string.IsNullOrEmpty(str);
                }

                if (!translationKey.StartsWith('#'))
                {
                    // User probably meant to set the text directly. 
                    obj.SetValue(property, translationKey);
                    return;
                }

                // Don't replace text with TRANSLATION FAILED if there's pre-existing text in the control
                if (translationFailed && !isEmptyOrNull)
                {
                    return;
                }
                else
                {
                    // And if there's no pre existing text, but the translation failed, show the default string if it is not empty
                    if (!string.IsNullOrEmpty(defaultStr))
                    {
                        obj.SetValue(property, defaultStr);
                        return;
                    }
                }
                
                Console.WriteLine("Set property " + property + " to " + translatedText + " for " + obj.GetType());
                if (obj is Control c) {
                    Console.WriteLine("with name " + c.Name);
                }
               
                obj.SetValue(property, translatedText);
            }

            // Window eventually inherits from ContentControl, as do lots of other controls. We don't want to override a window's content...
            if (obj is Window)
            {
                TranslateTextInternal(Window.TitleProperty);
            }
            else if (obj is MenuItem)
            {
                TranslateTextInternal(MenuItem.HeaderProperty);
            }
            else if (obj is TextBox)
            {
                TranslateTextInternal(TextBox.WatermarkProperty);
            }
            else if (obj is ContentControl)
            {
                TranslateTextInternal(ContentControl.ContentProperty);
            }
            else if (obj is TextBlock)
            {
                TranslateTextInternal(TextBlock.TextProperty);
            }
            else if (obj is NativeMenuItem)
            {
                TranslateTextInternal(NativeMenuItem.HeaderProperty);
            }
            else if (obj is TrayIcon)
            {
                TranslateTextInternal(TrayIcon.ToolTipTextProperty);
            }
        }
    }

    public static string? ELanguageToString(ELanguage lang)
    {
        try
        {
            return ELanguageConversion.APINameFromELanguage(lang);
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public string GetPrettyNameForLanguage(ELanguage lang)
    {
        return GetForLanguage(lang).LanguageFriendlyName;
    }

    //TODO: we should copy this over to ClientUI as well
    public void LoadCurrentSystemTranslation()
    {
        if (ELanguageConversion.TwoLetterISOCodesToLanguages.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out ELanguage lang)) {
            SetLanguage(lang);
        } else {
            SetLanguage(ELanguage.English);
        }
    }
}