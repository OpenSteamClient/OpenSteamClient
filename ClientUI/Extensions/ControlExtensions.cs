using Avalonia.Controls;
using Avalonia.VisualTree;

namespace ClientUI.Extensions;

public static class ControlExtensions {
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
    public static T? FindControlNested<T>(this Control control, string name) where T : Control {
        foreach (var item in control.GetVisualDescendants())
        {
            if (item.Name == name) {
                return (T)item;
            }
        }
        return null;
    }
}