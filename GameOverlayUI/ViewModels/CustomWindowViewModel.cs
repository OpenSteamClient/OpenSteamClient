using Avalonia;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Reactive;
using AvaloniaCommon;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameOverlayUI.ViewModels;

public partial class CustomWindowViewModel : ViewModelBase {
    [ObservableProperty]
    private object? content;

    [ObservableProperty]
    private double x;

    [ObservableProperty]
    private double y;

    [ObservableProperty]
    private int z = 0;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IconVisible))]
    private Bitmap? icon = null;
    public bool IconVisible => Icon != null;

    private readonly Window underlyingWindow;

    public void Close() {
        var mainViewViewModel = Program.Container.Get<MainViewViewModel>();
        Console.WriteLine("Close");
        mainViewViewModel.RemoveWindow(this);
        underlyingWindow.Close();
    }

    public void LostFocus() {
        Z = 0;
    }

    public void GotFocus() {
        Z = 1;
    }

    public CustomWindowViewModel(Window underlyingWindow) {
        this.underlyingWindow = underlyingWindow;

        underlyingWindow.GetObservable(Window.TitleProperty).Subscribe(new AnonymousObserver<string?>(TitleChange));
        underlyingWindow.GetObservable(Window.IconProperty).Subscribe(new AnonymousObserver<WindowIcon?>(IconChange));
        underlyingWindow.GetObservable(ContentControl.ContentProperty).Subscribe(new AnonymousObserver<object?>(ContentChange));
        TitleChange(underlyingWindow.Title);
        ContentChange(underlyingWindow.Content);
        IconChange(underlyingWindow.Icon);
    }

    private void ContentChange(object? obj)
    {
        Content = underlyingWindow.Content;
    }

    private void IconChange(WindowIcon? icon)
    {
        if (icon != null) {
            using var stream = new MemoryStream();
            icon.Save(stream);
            Icon = new Bitmap(stream);
        } else {
            Console.WriteLine("No icon");
        }
    }

    private void TitleChange(string? newTitle)
    {
        Title = underlyingWindow.Title ?? string.Empty;
    }
}