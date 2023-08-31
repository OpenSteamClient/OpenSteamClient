using System.Diagnostics.CodeAnalysis;
using OpenSteamworks.Client.Config;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;
using OpenSteamworks.Client.Utils.Interfaces;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.CommonEventArgs;

namespace OpenSteamworks.Client.Managers;

public class LoggedOnEventArgs : EventArgs
{
    public LoggedOnEventArgs(LoginUser user) { User = user; }
    public LoginUser User { get; } 
}

public class LoggedOffEventArgs : EventArgs
{
    public LoggedOffEventArgs(LoginUser user, EResult? error = null) { User = user; Error = error; }
    public LoginUser User { get; } 
    public EResult? Error { get; }
}

public class LogOnFailedEventArgs : EventArgs
{
    public LogOnFailedEventArgs(LoginUser user, EResult error) { User = user; Error = error; }
    public LoginUser User { get; } 
    public EResult Error { get; }
}

public class LoginManager : Component
{
    public delegate void LoggedOnEventHandler(object sender, LoggedOnEventArgs e);
    public delegate void LoggedOffEventHandler(object sender, LoggedOffEventArgs e);
    public delegate void LogOnFailedEventHandler(object sender, LogOnFailedEventArgs e);
    public event LoggedOnEventHandler? LoggedOn;
    public event LoggedOffEventHandler? LoggedOff;
    public event LogOnFailedEventHandler? LogOnFailed;

    private SteamClient steamClient;
    private LoginUsers loginUsers;

    public LoginManager(SteamClient steamClient, LoginUsers loginUsers, IContainer container) : base(container)
    {
        this.steamClient = steamClient;
        this.steamClient.CallbackManager.RegisterHandlersFor(this);
        this.loginUsers = loginUsers;
    }

    public bool RemoveAccount(LoginUser loginUser) {
        var success = loginUsers.Users.Remove(loginUser);
        loginUsers.Save();
        return success;
    }

    /// <summary>
    /// Gets all saved users. Does not need remember password.
    /// </summary>
    public IEnumerable<LoginUser> GetSavedUsers() {
        return loginUsers.Users;
    }

    public bool TryAutologin(IExtendedProgress<int> loginProgress) {
        var autologinUser = this.loginUsers.GetAutologin();
        if (autologinUser == null) {
            Console.WriteLine("Cannot autologin, no autologin user.");
            return false;
        }

        autologinUser.LoginMethod = LoginMethod.Cached;
        BeginLogonToUser(autologinUser, loginProgress);
        return true;
    }

    private void SetUserAsLastLogin(LoginUser user) {
        this.loginUsers.SetUserAsMostRecent(user);
    }

    private bool isLoggingOn = false;
    private IExtendedProgress<int>? loginProgress;
    private EResult? loginFinishResult;
    [CallbackListener<SteamServerConnectFailure_t>]
    public void OnSteamServerConnectFailure(SteamServerConnectFailure_t failure) {
        if (isLoggingOn) {
            loginFinishResult = failure.m_EResult;
        }
    }


    [CallbackListener<PostLogonState_t>]
    // This callback doesn't really mean anything. Just used for cosmetic purposes
    public void OnPostLogonState(PostLogonState_t stateUpdate) {
        if (isLoggingOn) {
            if (loginProgress != null) {
                if (stateUpdate.logonComplete) {
                    loginProgress.SetProgress(100);
                }
                //TODO: figure out the correct field to use for progress updates (or is it guessed?)
            }
        }
    }

    [CallbackListener<SteamServersConnected_t>]
    public void OnSteamServersConnected(SteamServersConnected_t connected) {
        if (isLoggingOn) {
            loginFinishResult = EResult.k_EResultOK;
        }
    }

    // [CallbackListener<SteamServersDisconnected_t>]
    // public void OnSteamServersConnected(SteamServersConnected_t connected) {
    //     if (isLoggingOn) {
    //         loginFinishResult = EResult.k_EResultOK;
    //     }
    // }
    
    public void SetMachineName(string machineName) {
        steamClient.NativeClient.IClientUser.SetUserMachineName(machineName);
    }

    public bool HasCachedCredentials(LoginUser user) {
        return steamClient.NativeClient.IClientUser.BHasCachedCredentials(user.AccountName);
    }

    public void BeginLogonToUser(LoginUser user, IExtendedProgress<int>? loginProgress) {
        if (this.isLoggingOn) {
            throw new InvalidOperationException("Logon already in progress");
        }

        if (user.LoginMethod == null)
        {
            throw new ArgumentException("LoginMethod must be set");
        }

        Task.Run(async () =>
        {
            // TODO: we don't yet support tracking the progress (though we could estimate based on the order callbacks fire...)
            loginProgress?.SetThrobber(true);
            this.loginProgress = loginProgress;

            switch (user.LoginMethod)
            {
                case LoginMethod.UsernamePassword:
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(user.AccountName);
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(user.Password);
                    steamClient.NativeClient.IClientUser.SetLoginInformation(user.AccountName, user.Password, user.Remembered);
                    if (user.SteamGuardCode != null) {
                        steamClient.NativeClient.IClientUser.SetTwoFactorCode(user.SteamGuardCode);
                    }
                    //TODO: use Protobuf here to get SteamID with SharedConnection. Alternatively we could handle the whole process manually, and just send the token to steamclient (which we need to do for QR flow regardless)
                    break;
                case LoginMethod.Cached:
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(user.AccountName);

                    if (!steamClient.NativeClient.IClientUser.BHasCachedCredentials(user.AccountName))
                    {
                        OnLogonFailed(new LogOnFailedEventArgs(user, EResult.k_EResultCachedCredentialIsInvalid));
                        return;
                    }

                    steamClient.NativeClient.IClientUser.SetAccountNameForCachedCredentialLogin(user.AccountName, false);
                    break;
                case LoginMethod.JWT:
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(user.LoginToken);
                    // For the JWT, we can extract the user's SteamID and username from it.
                    throw new NotImplementedException("Logging in using JWT is not yet implemented.");
                    //steamClient.NativeClient.IClientUser.SetLoginToken(user.LoginToken, "");
                    //break;
            }

            this.loginFinishResult = null;
            this.isLoggingOn = true;

            loginProgress?.SetOperation("Logging on " + user.AccountName);

            if (!user.SteamID.HasValue) {
                Console.WriteLine("SteamID is null!");
                OnLogonFailed(new LogOnFailedEventArgs(user, EResult.k_EResultInvalidSteamID));
                return;
            }
            
            // This nullability system is stupid.
            EResult beginLogonResult = steamClient.NativeClient.IClientUser.LogOn(user.SteamID.HasValue ? user.SteamID.Value : 0);

            if (beginLogonResult != EResult.k_EResultOK) {
                this.isLoggingOn = false;
                OnLogonFailed(new LogOnFailedEventArgs(user, beginLogonResult));
                return;
            }

            loginProgress?.SetSubOperation("Waiting for steamclient...");

            EResult result = await WaitForLogonToFinish();

            if (result == EResult.k_EResultOK)
            {
                if (loginUsers.Users.Contains(user))
                {
                    loginUsers.SetUserAsMostRecent(user);
                }
                else if (user.AllowAutoLogin == true)
                {
                    loginUsers.Users.Add(user);
                    loginUsers.SetUserAsAutologin(user);
                }
                OnLoggedOn(new LoggedOnEventArgs(user));
            } else {
                OnLogonFailed(new LogOnFailedEventArgs(user, result));
            }
        });
    }
    private void OnLogonFailed(LogOnFailedEventArgs e) {
        this.loginFinishResult = null;
        this.isLoggingOn = false;
        LogOnFailed?.Invoke(this, e);
    }

    private void OnLoggedOn(LoggedOnEventArgs e) {
        this.loginFinishResult = null;
        this.isLoggingOn = false;
        LoggedOn?.Invoke(this, e);
    }
    private async Task<EResult> WaitForLogonToFinish() {
        return await Task.Run<EResult>(() =>
        {
            do
            {
                System.Threading.Thread.Sleep(30);
            } while (this.loginFinishResult == null);
            this.isLoggingOn = false;
            return this.loginFinishResult.Value;
        });
    }

    public override async Task RunStartup()
    {
        await EmptyAwaitable();
    }

    public override async Task RunShutdown()
    {
        await EmptyAwaitable();
    }
}