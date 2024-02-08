using System.Collections.Generic;
using Avalonia;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Installer.Extensions;

public static class VisualExtensions
{
    public static IEnumerable<Visual> GetAllVisualChildrenTree(this Visual visual)
    {
        List<Visual> allVisuals = new()
        {
            visual
        };

        // First get all logical children
        foreach (var item in visual.GetLogicalChildren())
        {
            // Then try to cast them to Visual (most of the time should be valid)
            if (item is Visual)
            {
                allVisuals.AddRange(GetAllVisualChildrenTree((Visual)item!));
            }
        }

        return allVisuals;
    }
}