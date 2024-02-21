using System.Windows.Input;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Login;

namespace OpenSteamClient.ViewModels;

public partial class SavedAccountViewModel : AvaloniaCommon.ViewModelBase
{
    [ObservableProperty]
    private string loginName = "";

    [ObservableProperty]
    private Bitmap? profilePicture;

    [ObservableProperty]
    private ICommand? clickAction;

    [ObservableProperty]
    private ICommand? removeAction;

    public LoginUser user;

    public SavedAccountViewModel(LoginUser user)
    {
        this.user = user;
        this.LoginName = user.AccountName;
    }
}