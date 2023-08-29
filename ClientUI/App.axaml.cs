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
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.CommonEventArgs;
using System;

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
        var progVm = new ProgressWindowViewModel(prog, "Bootstrapper progress");
        var progressWindow = new ProgressWindow(progVm);

        ApplicationLifetime.MainWindow = progressWindow;
        progressWindow.Show();

        Container.RegisterComponentInstance(new Client(Container, prog));
        Container.ConstructAndRegisterComponentImmediate<TranslationManager>();
        Container.RegisterComponentInstance(this);
        await Container.RunStartupForComponents();

        progVm.Title = "Login Progress";

        // This will stay for the lifetime of the application.
        Container.GetComponent<LoginManager>().LoggedOn += (object sender, LoggedOnEventArgs eventArgs) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(() =>
            {
                ApplicationLifetime.MainWindow = new MainWindow
                {
                    DataContext = App.Container.ConstructOnly<MainWindowViewModel>()
                };

                progressWindow.Close();
                ApplicationLifetime.MainWindow.Show();
            });
        };

        Container.GetComponent<LoginManager>().LogOnFailed += (object sender, EResultEventArgs eventArgs) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(() => {
                MessageBox.Show("Failed to log on", "Failed with result code: " + eventArgs.EResult.ToString());
                ApplicationLifetime.MainWindow = new AccountPickerWindow
                {
                    DataContext = App.Container.ConstructOnly<AccountPickerWindowViewModel>()
                };

                progressWindow.Close();
                ApplicationLifetime.MainWindow.Show();
            });
        };

        if (!Container.GetComponent<LoginManager>().TryAutologin(prog)) {
            ApplicationLifetime.MainWindow = new AccountPickerWindow
            {
                DataContext = App.Container.ConstructOnly<AccountPickerWindowViewModel>()
            };
        }

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