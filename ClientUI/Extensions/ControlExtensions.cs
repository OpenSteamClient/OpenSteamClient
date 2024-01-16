using System;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace ClientUI.Extensions;

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
        foreach (var item in control.GetVisualDescendants())
        {
            if (item.Name == name)
            {
                return (T)item;
            }
        }
        return null;
    }

    public static bool TryTranslateSelf(this Control control, bool dueToLayoutChange = false)
    {
        var tm = AvaloniaApp.Container.Get<Translation.TranslationManager>();
        if (tm.CurrentTranslation.Language == OpenSteamworks.Enums.ELanguage.None)
        {
            return false;
        }

        Console.WriteLine($"Translating control {control.GetType().Name}{(dueToLayoutChange ? " (due to layout change)" : "")}");
        tm.TranslateVisual(control);
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