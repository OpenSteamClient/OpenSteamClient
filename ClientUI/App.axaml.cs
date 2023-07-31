using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Markup.Xaml;
using ClientUI.ViewModels;
using ClientUI.Views;
using Common;
using Common.Startup;
using Common.Utils;
using OpenSteamworks;

namespace ClientUI;

public partial class App : Application
{
    public static IContainer? DIContainer;
    public new static App? Current;
    public new IClassicDesktopStyleApplicationLifetime ApplicationLifetime => (base.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        this.DataContext = new AppViewModel();
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        ExtendedProgress<int> prog = new ExtendedProgress<int>(0, 100);
        
        var progressWindow = new ProgressWindow 
        {
            DataContext = new ProgressWindowViewModel(prog, "Bootstrapper progress"),
        };

        ApplicationLifetime.MainWindow = progressWindow;
        progressWindow.Show();
        
        App.DIContainer = await StartupController.Bootstrap<ClientUIAutofacRegistrar>(prog);

        ApplicationLifetime.MainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        progressWindow.Close();

        ApplicationLifetime.MainWindow.Show();

        base.OnFrameworkInitializationCompleted();
    }

    public App()
    {
        Current = this;
    }

    public void Exit(int exitCode = 0) {
        DIContainer?.Resolve<SteamClient>().Shutdown();
        ApplicationLifetime.Shutdown(exitCode);
    }
}