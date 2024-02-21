using System.Windows.Input;
using Avalonia;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using OpenSteamClient.Translation;

namespace OpenSteamClient.Controls;

public class Translatable : AvaloniaObject
{
    public static readonly AttachedProperty<string> TranslationKeyProperty =
            AvaloniaProperty.RegisterAttached<Translatable, Visual, string>("TranslationKey", "", false, Avalonia.Data.BindingMode.OneTime);

    public static readonly AttachedProperty<string> DefaultTextProperty =
            AvaloniaProperty.RegisterAttached<Translatable, Visual, string>("DefaultText", "", false, Avalonia.Data.BindingMode.OneTime);

    public static void SetTranslationKey(AvaloniaObject element, string val)
    {
        element.SetValue(TranslationKeyProperty, val);
        AvaloniaApp.Container.GetNullable<TranslationManager>()?.TranslateAvaloniaObject(element);
    }

    public static string GetTranslationKey(AvaloniaObject element)
    {
        return element.GetValue(TranslationKeyProperty);
    }

    public static void SetDefaultText(AvaloniaObject element, string val)
    {
        element.SetValue(DefaultTextProperty, val);
        AvaloniaApp.Container.Get<TranslationManager>().TranslateAvaloniaObject(element);
    }

    public static string GetDefaultText(AvaloniaObject element)
    {
        return element.GetValue(DefaultTextProperty);
    }
}