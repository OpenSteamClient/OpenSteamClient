using System.Windows.Input;
using Avalonia;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace ClientUI.Controls;

public class Translatable : AvaloniaObject
{
    public static readonly AttachedProperty<string> TranslationKeyProperty =
            AvaloniaProperty.RegisterAttached<Translatable, Visual, string>("TranslationKey", "", false, Avalonia.Data.BindingMode.OneTime);

    public static void SetTranslationKey(AvaloniaObject element, string val)
    {
        element.SetValue(TranslationKeyProperty, val);
    }

    public static string GetTranslationKey(AvaloniaObject element)
    {
        return element.GetValue(TranslationKeyProperty);
    }
}