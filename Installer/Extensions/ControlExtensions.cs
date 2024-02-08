using System;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Installer.Translation;

namespace Installer.Extensions;

public static class ControlExtensions
{
    //
    // Summary:
    //     Finds the named control under all of this Control's children
    //
    // Parameters:
    //   control:
    //     The control to look in.
    //
    //   name:
    //     The name of the control to find.
    //
    // Type parameters:
    //   T:
    //     The type of the control to find.
    //
    // Returns:
    //     The control or null if not found.
    public static T? FindControlNested<T>(this Control control, string name) where T : Control
    {
        foreach (var item in control.GetVisualChildren())
        {
            if (item is Control c) {
                var ctrl = FindControlNested<T>(c, name);
                if (ctrl != null) {
                    return ctrl;
                }
            }
        }

        return control.FindControl<T>(name);
    }

    public static bool TryTranslateSelf(this Control control, bool dueToLayoutChange = false)
    {
        if (AvaloniaApp.TranslationManager.CurrentTranslation.Language == ELanguage.None)
        {
            return false;
        }

        Console.WriteLine($"Translating control {control.GetType().Name}{(dueToLayoutChange ? " (due to layout change)" : "")}");
        AvaloniaApp.TranslationManager.TranslateVisual(control);
        return true;
    }

    public static void TranslatableInit(this Control control)
    {
        control.TryTranslateSelf();
        // Disable this for now, until I can find a way to filter out mouseover etc events from affecting this
        // Ideally we should never need to re-update translations during runtime except on language change, but if objects get added to the visual tree, they might not be translated yet (such as in ItemsControl cases, where you can't translate the fundamental template object since it gets removed at compile time)
        //control.LayoutUpdated += (object? sender, EventArgs e) => { control.TryTranslateSelf(true); };
        control.Initialized += (object? sender, EventArgs e) => { control.TryTranslateSelf(true); };

        // Register some events for auto-retranslate
        if (control is TopLevel)
        {
            (control as TopLevel)!.Opened += (object? sender, System.EventArgs e) => control.TryTranslateSelf();
        }
    }
}