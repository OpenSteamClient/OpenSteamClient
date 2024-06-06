using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OpenSteamClient.ViewModels;

public partial class MenuItemViewModel : AvaloniaCommon.ViewModelBase {
    [ObservableProperty]
    private string header = string.Empty;

    [ObservableProperty]
    private string translationKey = string.Empty;

    [ObservableProperty]
    private RelayCommand? command;

    public ObservableCollection<MenuItemViewModel> SubItems { get; init; } = new();

    public MenuItemViewModel() { }

    public MenuItemViewModel(string header, string translationKey) {
        this.Header = header;
        this.TranslationKey = translationKey;
    }

    public MenuItemViewModel(string header, string translationKey, Action command) : this(header, translationKey) {
        this.Command = new(command);
    }
}