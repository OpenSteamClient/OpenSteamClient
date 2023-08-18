using System.Collections.Generic;
using Avalonia;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace ClientUI.Extensions;

public static class VisualExtensions {
    public static IEnumerable<Visual> GetAllVisualChildrenTree(this Visual visual) {
        List<Visual> allVisuals = new();
        allVisuals.Add(visual);

        // First get all logical children
        foreach (var item in visual.GetLogicalChildren())
        {
            // Then try to cast them to Visual (most of the time should be valid)
            if (item is Visual) {
                var asVisual = (Visual)item!;
                allVisuals.Add(asVisual);
                allVisuals.AddRange(GetAllVisualChildrenTree(asVisual));
            }
        }

        return allVisuals;
    }
}