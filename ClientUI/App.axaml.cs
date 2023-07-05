using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClientUI.ViewModels;
using ClientUI.Views;

namespace ClientUI;

public partial class App : Application
{
    public new IClassicDesktopStyleApplicationLifetime ApplicationLifetime;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // This hack lets us use ApplicationLifetime without having to cast it every time
        if (base.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime) {
            ApplicationLifetime = lifetime;
        } else {
            throw new Exception("Invalid lifetime");
        }

        ApplicationLifetime.MainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel(),
            IsVisible = false,
        };

        base.OnFrameworkInitializationCompleted();
    }

    public void Exit(int exitCode = 0) {
        ApplicationLifetime.Shutdown(exitCode);
    }
    
}