using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OpenSteamworks.Client.Managers;
using Avalonia.Controls;
using OpenSteamworks.Client.Utils.DI;
using Avalonia.Threading;
using AvaloniaCommon;
using Profiler;
using System.Diagnostics;
using GameOverlayUI.ViewModels;
using GameOverlayUI.IPC;
using GameOverlayUI.Views;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Runtime.InteropServices;

namespace GameOverlayUI;

public class AvaloniaApp : Application
{
    public new static AvaloniaApp? Current;
    public static Theme? Theme;
    public new IClassicDesktopStyleApplicationLifetime ApplicationLifetime => (base.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

    public override void Initialize()
    {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("AvaloniaXamlLoader.Load");
        AvaloniaXamlLoader.Load(this);
    }

    private static void InvokeOnUIThread(Action callback)
    {
        if (!Program.Container.IsShuttingDown)
        {
            Avalonia.Threading.Dispatcher.UIThread.Invoke(callback);
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("OnFrameworkInitializationCompleted");
        Theme = new Theme(this);

        var mainView = new MainView() { DataContext = new MainViewViewModel() };
        Program.Container.RegisterInstance(mainView);
        ForceWindow(mainView);
        
        {
            using var baseScope = CProfiler.CurrentProfiler?.EnterScope("base.OnFrameworkInitializationCompleted");
            base.OnFrameworkInitializationCompleted();
        }

        if (!Program.IsUITestMode) {
            serverThread = new(ServerThreadMain);
            serverThread.Start();
        }

        
    }

    private Thread? serverThread;
    private readonly ManualResetEventSlim threadMainMre = new();
    private unsafe void ServerThreadMain() {
        var sharedMemory = Program.Container.Get<SharedMemory>();
        var mainView = Program.Container.Get<MainView>();

        byte[] renderTarget = new byte[(int)(mainView.Bounds.Size.Height * mainView.Bounds.Size.Width * 4)];
        // void* buf = NativeMemory.Alloc(sharedMemory.DisplayData->Width * sharedMemory.DisplayData->Height * 4);
        // Bitmap frame = new(PixelFormat.Bgra8888, AlphaFormat.Premul, (nint)buf, new PixelSize((int)sharedMemory.DisplayData->Width, (int)sharedMemory.DisplayData->Height), new Vector(96, 96), (int)(sharedMemory.DisplayData->Width * 4));

        while (!threadMainMre.IsSet) {
            // TODO: while (overlayIsReady)
            sharedMemory.RunInputFrame();
            AvaloniaHeadlessPlatform.ForceRenderTimerTick();
            var frame = mainView.GetLastRenderedFrame();
            if (frame != null) {
                sharedMemory.SetPixels(frame);
            }

            System.Threading.Thread.Sleep(8);
        }

        serverThread = null;
    }

    public void ActivateMainWindow() {
        ApplicationLifetime.MainWindow?.Show();
        ApplicationLifetime.MainWindow?.Activate();
    }

    /// <summary>
    /// Tries to show the window as a dialog
    /// </summary>
    /// <param name="dialog"></param>
    public void TryShowDialog(Window dialog) {
        dialog.Show();
    }

    /// <summary>
    /// Tries to show the window as a dialog. If the window is already open, it focus on it.
    /// </summary>
    /// <param name="dialog"></param>
    public void TryShowDialogSingle<T>(Func<T> dialogFactory) where T: Window {
        RunOnUIThread(DispatcherPriority.Send, () =>
        {
            var existingDialog = TryGetDialogSingle<T>();
            if (existingDialog != null) {
                existingDialog.Show();
                existingDialog.Activate();
                return;
            }

            dialogFactory().Show();
        });
    }

    /// <summary>
    /// Runs the given action on the UI thread.
    /// If we're already on the UI thread, executes the function inline.
    /// Blocks until the function has been executed.
    /// </summary>
    /// <param name="func"></param>
    public void RunOnUIThread(DispatcherPriority priority, Action func) {
        if (Dispatcher.UIThread.CheckAccess()) {
            func();
            return;
        }
        
        Dispatcher.UIThread.Invoke(func, priority);
    }

    public T? TryGetDialogSingle<T>() where T: Window {
        var existingDialogs = ApplicationLifetime.Windows.Where(w => w.GetType() == typeof(T));
        if (existingDialogs.Any()) {
            return (T)existingDialogs.First();
        }

        return null;
    }

    /// <summary>
    /// Closes the current MainWindow (if exists) and replaces it with a user specified window
    /// </summary>
    public T ForceWindow<T>(T window) where T : Window
    {
        if (!Program.Container.IsShuttingDown)
        {
            ApplicationLifetime.MainWindow?.Close();
            ApplicationLifetime.MainWindow = window;
            ApplicationLifetime.MainWindow.Show();
        }

        return window;
    }

    public AvaloniaApp()
    {
        Current = this;
        Name = "OpenSteamClient";
        DataContext = new AvaloniaAppViewModel();
    }

    /// <summary>
    /// Async exit function. Will hang in certain cases for some unknown reason.
    /// </summary>
    public async Task Exit(int exitCode = 0)
    {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("AvaloniaApp.Exit");
        Progress<string> operation = new();
        Progress<string> subOperation = new();
        await Program.Container.RunClientShutdown(operation, subOperation);
        Console.WriteLine("Shutting down Avalonia");
    }

    /// <summary>
    /// A synchronous exit function. Simply calls Task.Run. 
    /// </summary>
    public void ExitEventually(int exitCode = 0)
    {
        Dispatcher.UIThread.InvokeAsync(async () => await this.Exit(exitCode));
    }
}
