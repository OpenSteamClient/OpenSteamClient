using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using OpenSteamClient.Translation;

namespace OpenSteamClient.Controls;

public class Translatable : AvaloniaObject
{
    static Translatable()
    {
        TranslationKeyProperty.Changed.AddClassHandler<Visual>(TranslationPropertiesChanged);
        DefaultTextProperty.Changed.AddClassHandler<Visual>(TranslationPropertiesChanged);
    }

    private static void TranslationPropertiesChanged(Visual visual, AvaloniaPropertyChangedEventArgs args)
    {
        AvaloniaApp.Container.Get<TranslationManager>().TranslateAvaloniaObject(visual);
    }

    public static readonly AttachedProperty<string> TranslationKeyProperty =
            AvaloniaProperty.RegisterAttached<Translatable, Visual, string>("TranslationKey", "", false, Avalonia.Data.BindingMode.OneWay);

    public static readonly AttachedProperty<string> DefaultTextProperty =
            AvaloniaProperty.RegisterAttached<Translatable, Visual, string>("DefaultText", "", false, Avalonia.Data.BindingMode.OneWay);

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
    }

    public static string GetDefaultText(AvaloniaObject element)
    {
        return element.GetValue(DefaultTextProperty);
    }
}