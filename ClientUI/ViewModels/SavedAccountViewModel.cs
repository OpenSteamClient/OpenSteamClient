using System.Windows.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels;

public partial class SavedAccountViewModel : ViewModelBase {
    [ObservableProperty]
    private string loginName = "";

    [ObservableProperty]
    private Bitmap? profilePicture;

    [ObservableProperty]
    private ICommand? clickAction;
}