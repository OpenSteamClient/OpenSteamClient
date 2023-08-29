using System.Windows.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Config;

namespace ClientUI.ViewModels;

public partial class SavedAccountViewModel : ViewModelBase {
    [ObservableProperty]
    private string loginName = "";

    [ObservableProperty]
    private Bitmap? profilePicture;

    [ObservableProperty]
    private ICommand? clickAction;

    [ObservableProperty]
    private ICommand? removeAction;

    public LoginUser user;

    public SavedAccountViewModel(LoginUser user) {
        this.user = user;
        this.LoginName = user.AccountName;
    } 
}