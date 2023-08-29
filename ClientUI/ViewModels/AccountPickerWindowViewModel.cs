using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Generated;

namespace ClientUI.ViewModels;

public partial class AccountPickerWindowViewModel : ViewModelBase
{
    public ObservableCollection<SavedAccountViewModel> Accounts { get; } = new();
    private IClientUser iClientUser;
    private LoginManager loginManager;
    private static Bitmap unknownBitmap = new Bitmap(AssetLoader.Open(new Uri("avares://ClientUI/Assets/unknown.png")));
    public AccountPickerWindowViewModel(IClientUser iClientUser, LoginManager loginManager, TranslationManager tm) {
        this.iClientUser = iClientUser;
        this.loginManager = loginManager;

        foreach (var item in loginManager.GetSavedUsers())
        {
            var vm = new SavedAccountViewModel(item)
            {
                ProfilePicture = unknownBitmap,
            };

            vm.RemoveAction = new RelayCommand(() =>
            {
                if (!this.loginManager.RemoveAccount(item))
                {
                    MessageBox.Show(tm.GetTranslationForKey("#Error"), "Failed to remove account");
                }
                else
                {
                    this.Accounts.Remove(vm);
                }

            });

            this.Accounts.Add(vm);
        }
    }

    public void OpenLoginDialog() {

    }
}
