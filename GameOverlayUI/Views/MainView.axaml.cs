using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using GameOverlayUI.ViewModels;

namespace GameOverlayUI.Views;

public partial class MainView : Window {
    private MainViewViewModel vm => (DataContext as MainViewViewModel)!;
    public MainView() {
        InitializeComponent();
    }

    [MemberNotNull(nameof(draggingWindowViewModel))]
    private CustomWindowView? draggingWindow { get; set; }

    // The corner point where the dragging was initiated
    private Point dragPoint;
    private CustomWindowViewModel? draggingWindowViewModel => draggingWindow?.DataContext as CustomWindowViewModel;

    private void OnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
        if (!e.GetCurrentPoint(null).Properties.IsLeftButtonPressed) {
            return;
        }

        var pos = e.GetPosition(Desktop);
        Visual? vis = Desktop.GetVisualAt(pos);
        Console.WriteLine("PRESSED at " + pos);
        Console.WriteLine("Element is " + vis);
        CustomWindowView? window = null;
        bool isDrag = false;
        while (window == null && vis != null)
        {
            Console.WriteLine("DC: " + vis.DataContext?.GetType());
            if (vis.DataContext is CustomWindowViewModel) {
                isDrag = vis.Classes.Contains("draggable");
                window = vis.FindAncestorOfType<CustomWindowView>();
                break;
            }
            
            vis = vis?.Parent as Visual;
        }

        var vm = window?.DataContext as CustomWindowViewModel;

        Console.WriteLine("Window is " + vm?.Title);

        if (window != null) {
            ChangeFocusedWindow(vm);
        }

        if (isDrag) {
            draggingWindow = window;

            // This stops the window "teleporting" when it's being moved, and allows it to move wherever from a draggable area
            dragPoint = e.GetPosition(Desktop) - new Point(draggingWindowViewModel.X, draggingWindowViewModel.Y);
        }
    }

    private void OnPointerMoved(object? sender, Avalonia.Input.PointerEventArgs e) {
        var pos = e.GetPosition(Desktop);
        if (draggingWindow != null) {
            Console.WriteLine("MOVED to " + pos);
            Console.WriteLine("W: " + draggingWindow.Bounds.Width);
            Console.WriteLine("H: " + draggingWindow.Bounds.Height);

            Console.WriteLine("MW: " + Desktop.Bounds.Width);
            Console.WriteLine("MH: " + Desktop.Bounds.Height);

            Console.WriteLine("DPX: " + dragPoint.X + ", DPY: " + dragPoint.Y);

            // This function does some advanced math to:
            // 1. Ensure the window doesn't clip out of bounds at all
            // 2. Move the window to the correct location upon dragging
            // 3. Some other things I can't decipher from this code
            // Don't ask me about this math, I wrote it half asleep at 3:30am, but work it does! Improve if needed.
            double targetX = pos.X + draggingWindow.Bounds.Width - dragPoint.X;
            double targetY = pos.Y + draggingWindow.Bounds.Height - dragPoint.Y;

            double X = Math.Clamp(targetX, 0.0 + draggingWindow.Bounds.Width, Desktop.Bounds.Width) - draggingWindow.Bounds.Width;
            double Y = Math.Clamp(targetY, 0.0 + draggingWindow.Bounds.Height, Desktop.Bounds.Height) - draggingWindow.Bounds.Height;
            draggingWindowViewModel.X = X;
            draggingWindowViewModel.Y = Y;

            Console.WriteLine("CX: " + draggingWindowViewModel.X);
            Console.WriteLine("CY: " + draggingWindowViewModel.Y);
        }
    }

    private CustomWindowViewModel? focusedWindow;
    private void ChangeFocusedWindow(CustomWindowViewModel? newWindow) {
        if (newWindow == focusedWindow) {
            return;
        }

        if (newWindow != null) {
            newWindow.GotFocus();
            focusedWindow?.LostFocus();

            focusedWindow = newWindow;
        }
    }

    private void OnPointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e) {
        var pos = e.GetPosition(Desktop);
        Console.WriteLine("RELEASED at " + pos);
        draggingWindow = null;
        dragPoint = new Point(0, 0);
    }
}