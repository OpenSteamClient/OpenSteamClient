using ClientUI.Translation;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.Client.Managers;

namespace ClientUI.ViewModels;

public partial class SecondFactorNeededDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool canLoginSteamGuardMobile;
    [ObservableProperty]
    private bool canLoginSteamGuardCode;
    [ObservableProperty]
    private bool canLoginSteamGuardEmailCode;
    [ObservableProperty]
    private bool canLoginSteamEmailConfirmation;

    [ObservableProperty]
    private string steamGuardEmailCodeDescription = "";
    [ObservableProperty]
    private string steamEmailConfirmationDescription = "";

    [ObservableProperty]
    private string steamGuardCode = "";
    [ObservableProperty]
    private string steamGuardEmailCode = "";

    private LoginManager loginManager;
    public SecondFactorNeededDialogViewModel(TranslationManager tm, LoginManager loginManager, SecondFactorNeededEventArgs e) {
        this.loginManager = loginManager;
        foreach (var confirmation in e.AllowedConfirmations)
        {
            switch (confirmation.ConfirmationType)
            {
                case EAuthSessionGuardType.KEauthSessionGuardTypeDeviceCode:
                    CanLoginSteamGuardCode = true;
                    break;
                case EAuthSessionGuardType.KEauthSessionGuardTypeDeviceConfirmation:
                    CanLoginSteamGuardMobile = true;
                    break;
                case EAuthSessionGuardType.KEauthSessionGuardTypeEmailCode:
                    CanLoginSteamGuardEmailCode = true;
                    SteamGuardEmailCodeDescription = string.Format(tm.GetTranslationForKey("#SecondFactorDialog_SteamGuardEmailCode"), confirmation.AssociatedMessage);
                    break;
                case EAuthSessionGuardType.KEauthSessionGuardTypeEmailConfirmation:
                    CanLoginSteamEmailConfirmation = true;
                    SteamEmailConfirmationDescription = string.Format(tm.GetTranslationForKey("#SecondFactorDialog_SteamEmailConfirmation"), confirmation.AssociatedMessage);
                    break;
            }
        }
    }

    public async void LoginSteamGuardCode() {
        var result = await this.loginManager.UpdateAuthSessionWithTwoFactor(SteamGuardCode, EAuthSessionGuardType.KEauthSessionGuardTypeDeviceCode);
        MessageBox.Show("result", result.ToString());
    }

    public async void LoginSteamGuardEmailCode() {
        var result = await this.loginManager.UpdateAuthSessionWithTwoFactor(SteamGuardEmailCode, EAuthSessionGuardType.KEauthSessionGuardTypeEmailCode);
        MessageBox.Show("result", result.ToString());
    }
}
