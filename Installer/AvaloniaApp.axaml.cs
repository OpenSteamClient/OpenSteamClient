using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Markup.Xaml;
using Installer.Translation;
using Installer.ViewModels;
using Installer.Views;
using System.Threading.Tasks;
using System;
using Avalonia.Controls;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Input;
using Avalonia.Styling;
using System.Text;
using Avalonia.Threading;

namespace Installer;

public class AvaloniaApp : Application
{
    public static TranslationManager TranslationManager { get; } = new();
    public new static AvaloniaApp? Current;
    public new IClassicDesktopStyleApplicationLifetime ApplicationLifetime => (base.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
    public static bool DebugEnabled = false;
    public static bool InLinuxDevelopment = false;
    public Window? MainWindow => ApplicationLifetime.MainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        TranslationManager.LoadCurrentSystemTranslation();
        base.OnFrameworkInitializationCompleted();
        OpenMainWindow();
    }

    public void OpenMainWindow()
    {
        if (ApplicationLifetime.MainWindow != null)
        {
            if (ApplicationLifetime.MainWindow.IsActive)
            {
                // Not closed but maybe hidden, maybe shown in background
                ApplicationLifetime.MainWindow.Show();
                ApplicationLifetime.MainWindow.Activate();
                return;
            }
        } else {
            var wnd = new MainWindow();
            wnd.DataContext = new MainWindowViewModel(wnd);
            ApplicationLifetime.MainWindow = wnd;
            ApplicationLifetime.MainWindow.Show();
        }
    }

    public AvaloniaApp()
    {
        Current = this;
        Name = "Installer";
        DataContext = new AvaloniaAppViewModel();
    }
}
