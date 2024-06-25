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
using Avalonia.Input;
using GameOverlayUI.Impl;
using Avalonia.Platform.Storage;

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

        var mainViewViewModel = new MainViewViewModel();
        Program.Container.RegisterInstance(mainViewViewModel);

        var mainView = new MainView
        {
            DataContext = mainViewViewModel,
            TransparencyLevelHint = new List<WindowTransparencyLevel>() { WindowTransparencyLevel.Transparent }
        };

        Program.Container.RegisterInstance(mainView);
        
        ForceWindow(mainView);
        
        {
            using var baseScope = CProfiler.CurrentProfiler?.EnterScope("base.OnFrameworkInitializationCompleted");
            base.OnFrameworkInitializationCompleted();
        }

        serverThread = new(ServerThreadMain);
        serverThread.Start();
    }

    // #define LOGSPAM
    private Thread? serverThread;
    private readonly ManualResetEventSlim threadMainMre = new();
    private unsafe void ServerThreadMain() {
        var sharedMemoryManager = Program.Container.Get<SharedMemoryManager>();
        var mainView = Program.Container.Get<MainView>();
        RenderTargetBitmap? renderTarget = null;

        while (!threadMainMre.IsSet) {
            OverlayControlData.WaitForLoopStart(sharedMemoryManager.ControlData.Data);
            #if LOGSPAM
            Console.WriteLine("Client started loop");
            #endif

            sharedMemoryManager.ControlData.Data->State = EOverlayState.ResponseAvailable;

            // First resize the display and then run the inputs
            #if LOGSPAM
            Console.WriteLine("Input data requested");
            #endif
            
            OverlayControlData.RequestInputData(sharedMemoryManager.ControlData.Data, out uint newAllocation);
            if (newAllocation != 0) {
                sharedMemoryManager.InputData.Resize(newAllocation + sizeof(DynInputData));
            }
            
            #if LOGSPAM
            Console.WriteLine("Input data received");
            #endif
            

            var inputDataIn = sharedMemoryManager.GetInputData();
            List<InputData> inputData = new();
            int numMouseMoves = inputDataIn.Count(e => e.Type == EInputType.MouseMove);
            foreach (var item in inputDataIn)
            {
                if (item.Type == EInputType.MouseMove) {
                    numMouseMoves--;

                    // Allow 5 or so mouse moves per frame, anything extra will slow down everything to a halt
                    if (numMouseMoves > 5) {
                        continue;
                    }
                }

                inputData.Add(item);
            }

            Dispatcher.UIThread.Invoke(() => {
                for (int i = 0; i < inputData.Count; i++)
                {
                    var input = inputData[i];
                    // Avalonia headless input system is seriously slow (it wasn't intended to be used like this)
                    Console.WriteLine("Processing input type " + input.Type);
                    Console.WriteLine("Mod: " + input.Modifiers);
                    var pos = new Point(input.X, input.Y);
                    input.Modifiers = RawInputModifiers.None;

                    switch (input.Type)
                    {
                        case EInputType.MouseMove:
                            mainView.MouseMove(pos, input.Modifiers);
                            break;

                        case EInputType.MouseDown:
                            mainView.MouseDown(pos, input.MouseButton, input.Modifiers);
                            //mainView.MouseUp(clickPos, input.MouseButton, input.Modifiers);
                            break;

                        case EInputType.MouseUp:
                            mainView.MouseUp(pos, input.MouseButton, input.Modifiers);
                            break;

                        case EInputType.KeyDown:
                            mainView.KeyPress(input.Key, input.Modifiers, input.PhysicalKey, null);
                            break;

                        case EInputType.KeyUp:
                            mainView.KeyRelease(input.Key, input.Modifiers, input.PhysicalKey, null);
                            break;

                        case EInputType.MouseScrollDown:
                            mainView.MouseWheel(pos, new Vector(0, -1.0), input.Modifiers);
                            break;

                        case EInputType.MouseScrollUp:
                            mainView.MouseWheel(pos, new Vector(0, 1.0), input.Modifiers);
                            break;
                    }
                }

            }, DispatcherPriority.MaxValue);

            #if LOGSPAM
            Console.WriteLine("Display allocation requested");
            #endif
            
            OverlayControlData.RequestDisplayAllocation(sharedMemoryManager.ControlData.Data, out newAllocation);
            if (newAllocation != 0) {
                sharedMemoryManager.DisplayData.Resize(newAllocation + sizeof(DynDisplayData));
            }

            #if LOGSPAM
            Console.WriteLine("Display allocated");
            #endif

            if (renderTarget == null || sharedMemoryManager.DisplayData.Data->Width != renderTarget.PixelSize.Width || sharedMemoryManager.DisplayData.Data->Height != renderTarget.PixelSize.Height) {
                Dispatcher.UIThread.Invoke(() =>
                {
                    // Update the resolution of the window itself
                    Trace.Assert(mainView.PlatformImpl != null);

                    #if LOGSPAM
                    Console.WriteLine("Target W: " + sharedMemoryManager.DisplayData.Data->Width + ", H: " + sharedMemoryManager.DisplayData.Data->Height);
                    #endif

                    mainView.PlatformImpl.Resize(new Size(sharedMemoryManager.DisplayData.Data->Width, sharedMemoryManager.DisplayData.Data->Height), WindowResizeReason.User);
                }, DispatcherPriority.Render);
                
                renderTarget?.Dispose();
                
                renderTarget = new RenderTargetBitmap(PixelSize.FromSize(new Size(sharedMemoryManager.DisplayData.Data->Width, sharedMemoryManager.DisplayData.Data->Height), 1));
            }

            Dispatcher.UIThread.Invoke(() =>
            {
               renderTarget.Render(mainView);
            }, DispatcherPriority.MaxValue);

            
            // Console.WriteLine("ClientSize is now " + mainView.ClientSize);
            // Console.WriteLine("PixelSize is now " + );

            // AvaloniaHeadlessPlatform.ForceRenderTimerTick(1);
            // var frame = mainView.GetLastRenderedFrame();

            // if (frame != null) {
                sharedMemoryManager.SetPixels(renderTarget);
            // }

            #if LOGSPAM
            Console.WriteLine("rendered");
            #endif
            sharedMemoryManager.ControlData.Data->State = EOverlayState.RenderDataAvailable;
        }

        renderTarget?.Dispose();

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
