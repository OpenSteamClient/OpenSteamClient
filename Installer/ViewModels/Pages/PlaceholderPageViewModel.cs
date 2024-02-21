using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Installer.ViewModels.Pages;

public partial class PlaceholderPageViewModel : AvaloniaCommon.ViewModelBase {
    [ObservableProperty]
    private Control? internalControl;
}