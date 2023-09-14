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
using Avalonia.Controls;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ClientUI;

public partial class AvaloniaApp : Application
{
    public static Container Container = new Container();
    public new static AvaloniaApp? Current;
    public new IClassicDesktopStyleApplicationLifetime ApplicationLifetime => (base.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
    public static bool DebugEnabled = false;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        this.DataContext = new AvaloniaAppViewModel();
    }

    private ExtendedProgress<int> loginProgress = new ExtendedProgress<int>(0, 100);
    public override async void OnFrameworkInitializationCompleted()
    {
        ExtendedProgress<int> bootstrapperProgress = new ExtendedProgress<int>(0, 100);
        var progVm = new ProgressWindowViewModel(bootstrapperProgress, "Bootstrapper progress");
        ForceProgressWindow(progVm);

        Container.RegisterComponentInstance(new Client(Container, bootstrapperProgress));
        Container.ConstructAndRegisterComponentImmediate<TranslationManager>();
        Container.RegisterComponentInstance(this);
        await Container.RunStartupForComponents();

        // This will stay for the lifetime of the application.
        Container.GetComponent<LoginManager>().LoggedOn += (object sender, LoggedOnEventArgs e) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(() =>
            {
                ForceMainWindow();
            });
        };

        Container.GetComponent<LoginManager>().LogOnFailed += (object sender, LogOnFailedEventArgs e) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(() => {
                MessageBox.Show("Failed to log on", "Failed with result code: " + e.Error.ToString());
                ForceAccountPickerWindow();
            });
        };
        
        Container.GetComponent<LoginManager>().LoggedOff += (object sender, LoggedOffEventArgs e) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(() => {
                if (e.Error != null) {
                    // What can cause a sudden log off?
                    MessageBox.Show("Session terminated", "You were forcibly logged off with an error code: " + e.Error.ToString());
                }
                ForceAccountPickerWindow();
            });
        };

        // This is kept for the lifetime of the application, which is fine
        Container.GetComponent<LoginManager>().SetProgress(loginProgress);
        Container.GetComponent<LoginManager>().SetExceptionHandler(e => MessageBox.Error(e));
        Container.GetComponent<LoginManager>().LogonStarted += (object sender, EventArgs e) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(() =>
            {
                this.ForceProgressWindow(new ProgressWindowViewModel(loginProgress, "Login progress"));
            });
        };


        if (!Container.GetComponent<LoginManager>().TryAutologin()) {
            ForceAccountPickerWindow();
        }

        base.OnFrameworkInitializationCompleted();
        var icons = TrayIcon.GetIcons(this);
        UtilityFunctions.AssertNotNull(icons);
        foreach (var icon in icons)
        {
            Container.GetComponent<TranslationManager>().TranslateTrayIcon(icon);
        }
    }

    /// <summary>
    /// Closes the current MainWindow (if exists) and replaces it with the account picker
    /// </summary>
    public void ForceAccountPickerWindow() {
        ForceWindow(new AccountPickerWindow
        {
            DataContext = AvaloniaApp.Container.ConstructOnly<AccountPickerWindowViewModel>()
        });
    }

    /// <summary>
    /// Closes the current MainWindow (if exists) and replaces it with a new MainWindow
    /// </summary>
    public void ForceMainWindow() {
        ForceWindow(new MainWindow
        {
            DataContext = AvaloniaApp.Container.ConstructOnly<MainWindowViewModel>(new object[] { (Action)OpenSettingsWindow })
        });
    }

    private SettingsWindow? CurrentSettingsWindow;
    public void OpenSettingsWindow() {
        if (CurrentSettingsWindow != null) {
            if (CurrentSettingsWindow.PlatformImpl != null) {
                // Not closed but maybe hidden, maybe shown in background
                CurrentSettingsWindow.Show();
                CurrentSettingsWindow.Activate();
                return;
            }
        }

        CurrentSettingsWindow = new()
        {
            DataContext = AvaloniaApp.Container.ConstructOnly<SettingsWindowViewModel>()
        };

        CurrentSettingsWindow.Show();
        CurrentSettingsWindow.Closed += (object? sender, EventArgs e) =>
        {
            CurrentSettingsWindow = null;
        };
    }

    private InterfaceList? CurrentInterfaceListWindow;
    public void OpenInterfaceList() {
        if (CurrentInterfaceListWindow != null) {
            if (CurrentInterfaceListWindow.PlatformImpl != null) {
                // Not closed but maybe hidden, maybe shown in background
                CurrentInterfaceListWindow.Show();
                CurrentInterfaceListWindow.Activate();
                return;
            }
        }

        CurrentInterfaceListWindow = new();

        CurrentInterfaceListWindow.Show();
        CurrentInterfaceListWindow.Closed += (object? sender, EventArgs e) =>
        {
            CurrentInterfaceListWindow = null;
        };
    }

    public void OpenMainWindow() {
        if (ApplicationLifetime.MainWindow != null) {
            if (ApplicationLifetime.MainWindow.IsActive) {
                // Not closed but maybe hidden, maybe shown in background
                ApplicationLifetime.MainWindow.Show();
                ApplicationLifetime.MainWindow.Activate();
                return;
            }
        }

        ForceMainWindow();
    }

    /// <summary>
    /// Closes the current MainWindow (if exists) and replaces it with the login screen
    /// </summary>
    public void ForceLoginWindow(LoginUser? user) {
        LoginWindowViewModel vm;
        if (user == null) {
            vm = AvaloniaApp.Container.ConstructOnly<LoginWindowViewModel>();
        } else {
            vm = AvaloniaApp.Container.ConstructOnly<LoginWindowViewModel>(new object[1] { user });
        }

        var window = new LoginWindow();
        //TODO: hacky
        vm.ShowSecondFactorDialog = window.ShowSecondFactorDialog;
        window.DataContext = vm;

        ForceWindow(window);
    }

    /// <summary>
    /// Closes the current MainWindow (if exists) and replaces it with a new Progress Window, with the user specified ProgressWindowViewModel
    /// </summary>
    public void ForceProgressWindow(ProgressWindowViewModel progVm) {
        var progressWindow = new ProgressWindow(progVm);

        ForceWindow(new ProgressWindow(progVm));
    }

    /// <summary>
    /// Closes the current MainWindow (if exists) and replaces it with a user specified window
    /// </summary>
    public void ForceWindow(Window window) {
        ApplicationLifetime.MainWindow?.Close();
        ApplicationLifetime.MainWindow = window;
        ApplicationLifetime.MainWindow.Show();
    }

    public AvaloniaApp()
    {
        Current = this;
    }

    public async Task Exit(int exitCode = 0) {
        await Container.RunShutdownForComponents();
        ApplicationLifetime.Shutdown(exitCode);
    }

    /// <summary>
    /// A synchronous exit function. Simply calls Task.Run. 
    /// </summary>
    /// <param name="exitCode"></param>
    public void ExitEventually(int exitCode = 0) {
        Task.Run(async () => await this.Exit(exitCode));
    }
}