using System.Collections.ObjectModel;
using Avalonia.Dialogs;
using Avalonia.Platform.Storage;
using AvaloniaCommon;
using GameOverlayUI.Views;

namespace GameOverlayUI.ViewModels;

public partial class MainViewViewModel : ViewModelBase {
    public ObservableCollection<CustomWindowView> Windows { get; } = new();

    public MainViewViewModel() {
        Windows.Add(new CustomWindowView() { DataContext = new CustomWindowViewModel(new AboutAvaloniaDialog()) });
        Windows.Add(new CustomWindowView() { DataContext = new CustomWindowViewModel(new AboutAvaloniaDialog()) });
        Windows.Add(new CustomWindowView() { DataContext = new CustomWindowViewModel(new AboutAvaloniaDialog()) });
    }

    public void RemoveWindow(CustomWindowViewModel window) {
        var windowsCopy = Windows.ToList();
        foreach (var item in windowsCopy)
        {
            if (item.DataContext == window) {
                Windows.Remove(item);
            }
        }
    }
}