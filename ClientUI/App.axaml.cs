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
using OpenSteamworks.Client.Managers;
using System.Threading.Tasks;

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
        Container.ConstructAndRegisterComponentImmediate<TranslationManager>();
        Container.RegisterComponentInstance(this);
        await Container.RunStartupForComponents();

        ExtendedProgress<int> loginProgress = new ExtendedProgress<int>(0, 100);
        if (!Container.GetComponent<LoginManager>().TryAutologin(loginProgress, out Task? loginTask)) {
            ApplicationLifetime.MainWindow = new LoginWindow
            {
                DataContext = App.Container.ConstructOnly<LoginWindowViewModel>()
            };
        } else {
            ApplicationLifetime.MainWindow = new ProgressWindow(new ProgressWindowViewModel(loginProgress, "Login Progress"));
            await loginTask.ContinueWith(delegate {
                ApplicationLifetime.MainWindow = new MainWindow
                {
                    DataContext = App.Container.ConstructOnly<MainWindowViewModel>()
                };
                
            });
        }

        progressWindow.Close();

        ApplicationLifetime.MainWindow.Show();

        base.OnFrameworkInitializationCompleted();
    }

    public App()
    {
        Current = this;
    }

    public async Task Exit(int exitCode = 0) {
        await Container.RunShutdownForComponents();
        ApplicationLifetime.Shutdown(exitCode);
    }
}