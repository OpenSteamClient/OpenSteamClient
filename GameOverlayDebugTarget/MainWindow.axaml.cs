using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using GameOverlayUI.IPC;

namespace GameOverlayDebugTarget;

public partial class MainWindow : Window
{
    private Thread? thread;
    private readonly object inputsLock = new();
    private readonly List<InputData> inputs = new();

    public MainWindow()
    {
        InitializeComponent();
        this.thread = new Thread(MainThread);
        this.thread.Start();
        this.SystemDecorations = SystemDecorations.None;
        this.WindowState = WindowState.FullScreen;
    }

    private static RawInputModifiers KeyModifiersToRawInputModifiers(KeyModifiers keyModifiers) {
        var raw = RawInputModifiers.None;
        if (keyModifiers.HasFlag(KeyModifiers.Alt)) {
            raw |= RawInputModifiers.Alt;
        }

        if (keyModifiers.HasFlag(KeyModifiers.Control)) {
            raw |= RawInputModifiers.Control;
        }

        if (keyModifiers.HasFlag(KeyModifiers.Meta)) {
            raw |= RawInputModifiers.Meta;
        }

        if (keyModifiers.HasFlag(KeyModifiers.Shift)) {
            raw |= RawInputModifiers.Shift;
        }

        return raw;
    } 

    private static MouseButton PointerUpdateKindToMouseButton(PointerUpdateKind pointerUpdateKind) {
        return pointerUpdateKind switch
        {
            PointerUpdateKind.LeftButtonPressed => MouseButton.Left,
            PointerUpdateKind.LeftButtonReleased => MouseButton.Left,
            PointerUpdateKind.MiddleButtonPressed => MouseButton.Middle,
            PointerUpdateKind.MiddleButtonReleased => MouseButton.Middle,
            PointerUpdateKind.RightButtonPressed => MouseButton.Right,
            PointerUpdateKind.RightButtonReleased => MouseButton.Right,
            PointerUpdateKind.XButton1Pressed => MouseButton.XButton1,
            PointerUpdateKind.XButton1Released => MouseButton.XButton1,
            PointerUpdateKind.XButton2Pressed => MouseButton.XButton2,
            PointerUpdateKind.XButton2Released => MouseButton.XButton2,
            _ => MouseButton.None
        };
    }

    private void OnPointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e) {
        var pos = PixelPoint.FromPoint(e.GetPosition(RenderedImage), this.RenderScaling);
        Console.WriteLine("RELEASED at " + pos);

        lock (inputsLock)
        {
            inputs.Add(new InputData(KeyModifiersToRawInputModifiers(e.KeyModifiers), (uint)pos.X, (uint)pos.Y, PointerUpdateKindToMouseButton(e.GetCurrentPoint(null).Properties.PointerUpdateKind), false));
        }
    }

    private void OnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
        var pos = PixelPoint.FromPoint(e.GetPosition(RenderedImage), this.RenderScaling);
        Console.WriteLine("PRESSED at " + pos);

        lock (inputsLock)
        {
            inputs.Add(new InputData(KeyModifiersToRawInputModifiers(e.KeyModifiers), (uint)pos.X, (uint)pos.Y, PointerUpdateKindToMouseButton(e.GetCurrentPoint(null).Properties.PointerUpdateKind), true));
        }
    }

    private void OnPointerMoved(object? sender, Avalonia.Input.PointerEventArgs e) {
        var pos = e.GetPosition(RenderedImage);
        Console.WriteLine("MOVED at " + pos);

        lock (inputsLock)
        {
            inputs.Add(new InputData(KeyModifiersToRawInputModifiers(e.KeyModifiers), (uint)pos.X, (uint)pos.Y));
        }
    }

    private void OnKeyDown(object? sender, Avalonia.Input.KeyEventArgs e) {
        Console.WriteLine("key down");
        lock (inputsLock)
        {
            inputs.Add(new InputData(KeyModifiersToRawInputModifiers(e.KeyModifiers), e.Key, e.PhysicalKey, true));
        }
    }

    private void OnKeyUp(object? sender, Avalonia.Input.KeyEventArgs e) {
        Console.WriteLine("key up");
        lock (inputsLock)
        {
            inputs.Add(new InputData(KeyModifiersToRawInputModifiers(e.KeyModifiers), e.Key, e.PhysicalKey, false));
        }
    }

    private EOverlayState State {
        get {
            unsafe {
                return Program.Container.Get<SharedMemoryManager>().ControlData.Data->State;
            }
        }

        set {
            unsafe {
                Program.Container.Get<SharedMemoryManager>().ControlData.Data->State = value;
            }
        }
    }

    private unsafe void MainThread() {
        var sharedMemoryManager = Program.Container.Get<SharedMemoryManager>();
        int fpsLimit = 0;
        double averageElapsed = 0;
        double averageFPS = 0;
        Stopwatch stopwatch = new();
        Stopwatch clientStopwatch = new();
        Stopwatch serverStopwatch = new();
        stopwatch.Start();
        while (true)
        {
            stopwatch.Reset();
            stopwatch.Start();
            serverStopwatch.Reset();
            clientStopwatch.Reset();
            clientStopwatch.Start();
            
            bool isChecked = false;
            
            Dispatcher.UIThread.Invoke(() =>
            {
                isChecked = ShouldRender.IsChecked != null && ShouldRender.IsChecked.Value;
                if (FPSLimitCheck.IsChecked != null && FPSLimitCheck.IsChecked.Value) {
                    if (int.TryParse(FPSLimitBox.Text, out int fpsLimit2)) {
                        fpsLimit = fpsLimit2;
                    }
                } else {
                    fpsLimit = 0;
                }
            });

            if (!isChecked) {
                Console.WriteLine("Render blocked");
                Thread.Sleep(1000);
                continue;
            }

            
            State = EOverlayState.ClientRequestLoopStart;

            clientStopwatch.Stop();
            serverStopwatch.Start();
            while (State != EOverlayState.ResponseAvailable)
            {
                if (State == EOverlayState.ServerRequestInputData) {
                    goto inputDataAnswer;
                }
            }

            while (State != EOverlayState.ServerRequestInputData)
            {
                
            }

inputDataAnswer:
            serverStopwatch.Stop();
            clientStopwatch.Start();

            List<InputData> inputsCopy;
            lock (inputsLock)
            {
                inputsCopy = inputs.ToList();
                inputs.Clear();
            }

            uint newLength = (uint)(DynInputData.CalculateDataLength((uint)inputsCopy.Count) + sizeof(DynInputData));
            if (sharedMemoryManager.InputData.Length < newLength) {
                sharedMemoryManager.InputData.Resize(newLength);
                sharedMemoryManager.ControlData.Data->MemoryResized = newLength;
            } else {
                sharedMemoryManager.ControlData.Data->MemoryResized = 0;
            }
            

            DynInputData.EnqueueAll(sharedMemoryManager.InputData.Data, inputsCopy);

            serverStopwatch.Start();
            clientStopwatch.Stop();
            State = EOverlayState.ResponseAvailable;
            while (State != EOverlayState.ServerRequestDisplayAllocation)
            {
                
            }

            serverStopwatch.Stop();
            clientStopwatch.Start();

            newLength = (uint)(DynDisplayData.CalculateDataLength(sharedMemoryManager.DisplayData.Data) + sizeof(DynDisplayData));
            sharedMemoryManager.DisplayData.Resize(newLength);
            sharedMemoryManager.ControlData.Data->MemoryResized = newLength;

            serverStopwatch.Start();
            clientStopwatch.Stop();
            State = EOverlayState.ResponseAvailable;

            while (State != EOverlayState.RenderDataAvailable)
            {
                
            }

            serverStopwatch.Stop();
            clientStopwatch.Start();

            Dispatcher.UIThread.Invoke(() =>
            {
                WriteableBitmap bitmap;

                if (RenderedImage.Source is WriteableBitmap mp)
                {
                    bitmap = mp;
                }
                else
                {
                    Vector dpi = new Vector(96, 96);

                    bitmap = new WriteableBitmap(
                        new PixelSize((int)sharedMemoryManager.DisplayData.Data->Width, (int)sharedMemoryManager.DisplayData.Data->Height),
                        dpi,
                        PixelFormat.Bgra8888,
                        AlphaFormat.Premul);

                    RenderedImage.Source = bitmap;
                }

                using (var frameBuffer = bitmap.Lock()) 
                {
                    NativeMemory.Copy(&sharedMemoryManager.DisplayData.Data->DynamicData, (void*)frameBuffer.Address, (nuint)DynDisplayData.CalculateDataLength(sharedMemoryManager.DisplayData.Data)); 
                }

                RenderedImage.InvalidateVisual();
            }, DispatcherPriority.Render);

            clientStopwatch.Stop();

            if (fpsLimit > 0) {
                double calculated = 1 / (double)fpsLimit * 1000;
                int sleepMS = (int)(calculated - stopwatch.Elapsed.TotalMilliseconds);
                if (sleepMS > 0) {
                    Thread.Sleep(sleepMS);
                }
            }
            
            var totalElapsed = stopwatch.Elapsed.TotalMilliseconds;
            averageElapsed = (averageElapsed + totalElapsed) / 2;
            averageFPS = (averageFPS + (1 / totalElapsed * 1000)) / 2;
            string debugStrFPS = $"FPS: {TruncateDouble(averageFPS)}";
            string debugStrFTAvg = $"Avg ft: {TruncateDouble(averageElapsed)}ms";
            string debugStrFTLast = $"Last ft: {TruncateDouble(totalElapsed)}ms";
            string debugStrProcessServer = $"Server process {TruncateDouble(serverStopwatch.Elapsed.TotalMilliseconds)}ms";
            string debugStrProcessClient = $"Client process: {TruncateDouble(clientStopwatch.Elapsed.TotalMilliseconds)}ms";

            Dispatcher.UIThread.Post(() =>
            {
                FramerateDebugText.Text = debugStrFPS;
                FrametimeAvgDebugText.Text = debugStrFTAvg;
                FrametimeLastDebugText.Text = debugStrFTLast;
                ProcessServerDebugText.Text = debugStrProcessServer;
                ProcessClientDebugText.Text = debugStrProcessClient;
            });
        }
    }

    private static string TruncateDouble(double val) {
        return (Math.Truncate(val * 100) / 100).ToString();
    }
}