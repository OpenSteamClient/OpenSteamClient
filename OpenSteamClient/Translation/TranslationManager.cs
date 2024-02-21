using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Avalonia.VisualTree;
using OpenSteamClient.Extensions;

using OpenSteamworks;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Utils;

namespace OpenSteamClient.Translation;

public class Translation
{
    public ELanguage Language { get; set; } = ELanguage.None;
    public string LanguageFriendlyName { get; set; } = "";
    public Dictionary<string, string> TranslationKeys { get; set; } = new();
}

public class TranslationManager : ILogonLifetime
{
    public Translation CurrentTranslation = new();
    private readonly List<AvaloniaObject> RefreshableObjects = new();
    private readonly IClientUser clientUser;
    private readonly IClientUtils clientUtils;
    private readonly InstallManager installManager;
    private readonly Container container;
    private readonly ConfigManager configManager;
    private readonly Logger logger;

    private UserSettings userSettings
    {
        get
        {
            return container.Get<UserSettings>();
        }
    }

    public static IEnumerable<ELanguage> ValidUILanguages => new ELanguage[] { ELanguage.English, ELanguage.Finnish };
    public TranslationManager(IClientUser clientUser, IClientUtils clientUtils, InstallManager installManager, Container container, ConfigManager configManager)
    {
        this.logger = Logger.GetLogger("TranslationManager", installManager.GetLogPath("TranslationManager"));
        this.clientUser = clientUser;
        this.clientUtils = clientUtils;
        this.installManager = installManager;
        this.container = container;
        this.configManager = configManager;
    }

    public void SetLanguage(ELanguage language, bool save = true)
    {
        var lang = ELanguageToString(language);
        if (lang == null)
        {
            throw new ArgumentException($"Language {language} was not valid.");
        }

        // Allow setting steamclient language
        clientUser.SetLanguage(lang);

        // Even if we don't have a GUI translation
        var newTranslation = GetForLanguage(language, out bool failed);
        if (failed)
        {
            logger.Warning("Attempted to switch to unsupported language " + language + ", still switching steamclient");
        }
        else
        {
            CurrentTranslation = newTranslation;

            Dispatcher.UIThread.Invoke(() =>
            {
                foreach (var obj in RefreshableObjects)
                {
                    TranslateAvaloniaObject(obj);
                }
            });
        }

        if (save)
        {
            userSettings.Language = language;
            configManager.Save(userSettings);
        }
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

    private Translation GetForLanguage(ELanguage language, out bool failed)
    {
        failed = false;
        string? filename = ELanguageToString(language);
        if (filename == null)
        {
            throw new ArgumentOutOfRangeException("Invalid ELanguage " + language + " specified.");
        }

        string fullPath = Path.Combine(installManager.AssemblyDirectory, "Translations", filename + ".json");
        if (!File.Exists(fullPath))
        {
            if (language == ELanguage.English)
            {
                throw new Exception("Base language not found!");
            }

            failed = true;
            return CurrentTranslation;
        }

        return UtilityFunctions.AssertNotNull(JsonSerializer.Deserialize<Translation>(File.ReadAllText(fullPath)));
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

    async Task ILogonLifetime.OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        if (userSettings.Language != ELanguage.None) {
            await Task.Run(() => this.SetLanguage(userSettings.Language));
        } else {
            await Task.Run(() => this.SetLanguage(ELanguage.English));
        }
    }

    async Task ILogonLifetime.OnLoggingOff(IExtendedProgress<int> progress)
    {
        await Task.CompletedTask;
    }
}