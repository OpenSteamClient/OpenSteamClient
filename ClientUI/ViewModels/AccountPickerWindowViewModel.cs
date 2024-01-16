using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Client.Config;
using OpenSteamworks.Client.Login;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Generated;

namespace ClientUI.ViewModels;

public partial class AccountPickerWindowViewModel : ViewModelBase
{
    public ObservableCollection<SavedAccountViewModel> Accounts { get; } = new();
    private IClientUser clientUser;
    private LoginManager loginManager;
    private static Bitmap unknownBitmap = new Bitmap(AssetLoader.Open(new Uri("avares://ClientUI/Assets/unknown.png")));
    public AccountPickerWindowViewModel(IClientUser clientUser, LoginManager loginManager, TranslationManager tm)
    {
        this.clientUser = clientUser;
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

            vm.ClickAction = new RelayCommand(() =>
            {
                if (this.loginManager.HasCachedCredentials(item))
                {
                    item.LoginMethod = LoginUser.ELoginMethod.Cached;
                    this.loginManager.BeginLogonToUser(item);
                }
                else
                {
                    this.OpenLoginDialog(item);
                }
            });

            this.Accounts.Add(vm);
        }
    }

    public void OpenLoginDialog(LoginUser? user)
    {
        AvaloniaApp.Current?.ForceLoginWindow(user);
    }

    public void OpenLoginDialog()
    {
        this.OpenLoginDialog(null);
    }
}
