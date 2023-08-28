using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Generated;

namespace ClientUI.ViewModels;

public partial class AccountPickerWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<SavedAccountViewModel> accounts = new ObservableCollection<SavedAccountViewModel>();
    private IClientUser iClientUser;
    public AccountPickerWindowViewModel(IClientUser iClientUser, LoginManager loginManager) {
        this.iClientUser = iClientUser;

        accounts.Add(new SavedAccountViewModel() {
            ProfilePicture = new Bitmap("avares://Assets/plus.png"),
        });
        foreach (var item in loginManager.GetSavedUsers())
        {
            accounts.Add(new SavedAccountViewModel() {
                LoginName = item.AccountName,
                ProfilePicture = new Bitmap("avares://Assets/unknown.png")
            });
        }
    }
}
