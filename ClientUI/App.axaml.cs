using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.Markup.Xaml;
using ClientUI.Translation;
using ClientUI.ViewModels;
using ClientUI.Views;



using OpenSteamworks.Client.Utils;
using OpenSteamworks;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Utils.Interfaces;

namespace ClientUI;

public partial class App : Application
{
    public static Container Container = new Container();
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

        var progressWindow = new ProgressWindow(new ProgressWindowViewModel(prog, "Bootstrapper progress"));

        ApplicationLifetime.MainWindow = progressWindow;
        progressWindow.Show();

        Container.RegisterComponentInstance(new Client(Container, prog));
        Container.ConstructAndRegisterComponent<TranslationManager>();
        Container.RegisterComponentInstance(this);
        await Container.RunStartupForComponents();

        if (true) {
            ApplicationLifetime.MainWindow = new LoginWindow
            {
                DataContext = new LoginWindowViewModel()
            };
        } else {
            ApplicationLifetime.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
        }

        progressWindow.Close();

        ApplicationLifetime.MainWindow.Show();

        base.OnFrameworkInitializationCompleted();
    }

    public App()
    {
        Current = this;
    }

    public void Exit(int exitCode = 0) {
        Container.RunShutdownForComponents().Wait();
        ApplicationLifetime.Shutdown(exitCode);
    }
}