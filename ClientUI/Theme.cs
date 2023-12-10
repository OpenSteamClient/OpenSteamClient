using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Media;
using Avalonia.Styling;

namespace ClientUI;

public class Theme {
    public IBrush AccentButtonBackground {
        get {
            return (IBrush)GetResource("AccentButtonBackground");
        }
    }

    public IBrush AccentButtonForeground {
        get {
            return (IBrush)GetResource("AccentButtonForeground");
        }
    }

    public IBrush ButtonBackground {
        get {
            return (IBrush)GetResource("ButtonBackground");
        }
    }

    public IBrush ButtonForeground {
        get {
            return (IBrush)GetResource("ButtonForeground");
        }
    }

    private object GetResource(string key) {
        app.TryGetResource(key, ThemeVariant.Default, out object? val);
        ThrowIfNull(val);
        return val;
    } 

    private void ThrowIfNull([NotNull] object? val) {
        if (val == null) {
            throw new Exception("No styles where there should be");
        }
    }

    private AvaloniaApp app;
    internal Theme(AvaloniaApp app) {
        this.app = app;
    }
}