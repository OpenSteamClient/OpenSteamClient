using System.Collections.ObjectModel;

namespace ClientUI.ViewModels;

public class LoginWindowViewModel : ViewModelBase
{
    public ObservableCollection<AccountViewModel> Accounts { get; set; } = new ObservableCollection<AccountViewModel>();
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool RememberPassword { get; set; } = true;

    public bool HasSavedAccounts { 
        get {
            return this.Accounts.Count > 0;
        }
    } 

    public void OnClosed() {
        
    }

    public void RegisterPressed() {
        
    }
    public void LoginPressed() {
        
    }
}
