using System.Collections.ObjectModel;
using ClientUI.Translation;
using OpenSteamworks.Generated;

namespace ClientUI.ViewModels;

public class LoginWindowViewModel : ViewModelBase
{
    public ObservableCollection<AccountViewModel> Accounts { get; set; } = new ObservableCollection<AccountViewModel>();
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool RememberPassword { get; set; } = true;
    private IClientUser iClientUser;
    private TranslationManager tm;
    public LoginWindowViewModel(IClientUser iClientUser, TranslationManager tm) {
        this.iClientUser = iClientUser;
        this.tm = tm;
    }

    public bool HasSavedAccounts { 
        get {
            return this.Accounts.Count > 0;
        }
    } 

    public void OnClosed() {
        
    }

    public void RegisterPressed() {
        MessageBox.Show(tm.GetTranslationForKey("#Unsupported"), string.Format(tm.GetTranslationForKey("#LoginWindow_AccountCreationUnsupported"), "https://store.steampowered.com/join/"));
    }
    public void LoginPressed() {
        
    }
}
