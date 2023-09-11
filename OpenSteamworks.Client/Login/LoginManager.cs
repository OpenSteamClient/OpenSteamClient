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
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Messaging;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using System.Net;
using OpenSteamworks.Client.Login;

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

public class QRGeneratedEventArgs : EventArgs
{
    public QRGeneratedEventArgs(string url) { URL = url; }
    public string URL { get; }
}

public class LoginManager : Component
{
    public delegate void LoggedOnEventHandler(object sender, LoggedOnEventArgs e);
    public delegate void LoggedOffEventHandler(object sender, LoggedOffEventArgs e);
    public delegate void LogOnStartedEventHandler(object sender, EventArgs e);
    public delegate void LogOnFailedEventHandler(object sender, LogOnFailedEventArgs e);
    public delegate void QRGeneratedEventHandler(object sender, QRGeneratedEventArgs e);
    public event LoggedOnEventHandler? LoggedOn;
    public event LoggedOffEventHandler? LoggedOff;
    public event LogOnStartedEventHandler? LogonStarted;
    public event LogOnFailedEventHandler? LogOnFailed;
    public event QRGeneratedEventHandler? QRGenerated;

    private SteamClient steamClient;
    private LoginUsers loginUsers;
    private ClientMessaging clientMessaging;
    private LoginPoll? QRPoller;
    private LoginPoll? CredentialsPoller;

    public LoginManager(SteamClient steamClient, LoginUsers loginUsers, IContainer container, ClientMessaging clientMessaging) : base(container)
    {
        this.steamClient = steamClient;
        this.steamClient.CallbackManager.RegisterHandlersFor(this);
        this.loginUsers = loginUsers;
        this.clientMessaging = clientMessaging;
    }

    public bool RemoveAccount(LoginUser loginUser) {
        var success = loginUsers.RemoveUser(loginUser);
        steamClient.NativeClient.IClientUser.DestroyCachedCredentials(loginUser.AccountName, (int)EAuthTokenRevokeAction.KEauthTokenRevokePermanent);
        loginUsers.Save();
        return success;
    }

    public bool AddAccount(LoginUser loginUser) {
        loginUser.Remembered = steamClient.NativeClient.IClientUser.BHasCachedCredentials(loginUser.AccountName);
        var success = loginUsers.AddUser(loginUser);
        loginUsers.Save();
        return success;
    }
    public void SetProgress(IExtendedProgress<int>? progress) {
        this.loginProgress = progress;
    }
    private IExtendedProgress<int>? loginProgress;

    /// <summary>
    /// Asynchronous background tasks may throw. If no exception handler is explicitly specified, C# will eat the errors and silently fail.
    /// </summary>
    public void SetExceptionHandler(Action<AggregateException> exceptionHandler) {
        this.exceptionHandler = exceptionHandler;
    }
    private Action<AggregateException>? exceptionHandler;

    /// <summary>
    /// Starts generating QR codes for login. Will call QRGenerated everytime a new QR code comes in. Call this method AFTER registering a handler for QRGenerated.
    /// </summary>
    public void StartQRAuthLoop() {
        if (QRPoller != null) {
            throw new InvalidOperationException("QR Auth loop already running");
        }

        Task.Run(async () =>
        {
            using (Connection conn = clientMessaging.AllocateConnection())
            {
                ProtoMsg<CAuthentication_BeginAuthSessionViaQR_Request> beginMsg = new("Authentication.BeginAuthSessionViaQR#1", true);
                beginMsg.body.DeviceDetails = DeviceDetails;

                ProtoMsg<CAuthentication_BeginAuthSessionViaQR_Response> beginResp = await conn.ProtobufSendMessageAndAwaitResponse<CAuthentication_BeginAuthSessionViaQR_Response, CAuthentication_BeginAuthSessionViaQR_Request>(beginMsg);

                QRGenerated?.Invoke(this, new QRGeneratedEventArgs(beginResp.body.ChallengeUrl));

                QRPoller = new LoginPoll(this.clientMessaging, beginResp.body.ClientId, beginResp.body.RequestId, beginResp.body.Interval);
                QRPoller.ChallengeUrlGenerated += (object sender, ChallengeUrlGeneratedEventArgs e) => this.QRGenerated?.Invoke(this, new QRGeneratedEventArgs(e.URL));
                QRPoller.AccessTokenGenerated += (object sender, TokenGeneratedEventArgs e) => BeginLogonToUser(new LoginUser(e.AccountName, e.Token));
                QRPoller.Error += (object sender, EResultEventArgs e) => exceptionHandler?.Invoke(new AggregateException("Error occurred in QR Poller: " + e.EResult));
                QRPoller.StartPolling();
            }
        });
    }

    public void StopQRAuthLoop() {
        QRPoller?.StopPolling();
    }

    private CAuthentication_DeviceDetails? deviceDetails;
    public CAuthentication_DeviceDetails DeviceDetails {
        get {
            if (deviceDetails == null) {
                DetermineDeviceDetails();
            }
            return deviceDetails;
        }
    }

    [MemberNotNull("deviceDetails")]
    private void DetermineDeviceDetails() {
        //TODO: allow users to opt out of this data collection. Not that it matters anyway, as steamclient collects this data itself anyway (does it also rewrite our message?).
        deviceDetails = new();

        // This means Steam Deck/Steam Link
        // Desktop platforms is 0
        deviceDetails.GamingDeviceType = 0;

        if (OperatingSystem.IsWindows()) {
            //TODO: more accurate logic here
            if (OperatingSystem.IsWindowsVersionAtLeast(10)) {
                deviceDetails.OsType = (int)EOSType.k_EOSTypeWin10;
            } else if (OperatingSystem.IsWindowsVersionAtLeast(8, 1)) {
                deviceDetails.OsType = (int)EOSType.k_EOSTypeWin81;
            } else if (OperatingSystem.IsWindowsVersionAtLeast(7)) {
                deviceDetails.OsType = (int)EOSType.k_EOSTypeWin7;
            } else {
                deviceDetails.OsType = (int)EOSType.k_EOSTypeWindows;
            }
        } else if (OperatingSystem.IsMacOS()) {
            //TODO: logic for determining OSX version
            deviceDetails.OsType = (int)EOSType.k_EOSTypeMacos;
        } else if (OperatingSystem.IsLinux()) {
            //TODO: logic for determining kernel version
            deviceDetails.OsType = (int)EOSType.k_EOSTypeLinux;
        }
        
        // If this isn't specified, the auth tokens we receive will not let us login
        deviceDetails.PlatformType = EAuthTokenPlatformType.KEauthTokenPlatformTypeSteamClient;

        try
        {
            deviceDetails.DeviceFriendlyName = Dns.GetHostName();
        }
        catch (System.Exception)
        {
            Console.WriteLine("Failed to determine hostname.");
        }
    }

    /// <summary>
    /// Gets all saved users. Does not need remember password.
    /// </summary>
    public IEnumerable<LoginUser> GetSavedUsers() {
        return loginUsers.Users;
    }

    public bool TryAutologin() {
        var autologinUser = this.loginUsers.GetAutologin();
        if (autologinUser == null) {
            Console.WriteLine("Cannot autologin, no autologin user.");
            return false;
        }

        autologinUser.LoginMethod = LoginMethod.Cached;
        BeginLogonToUser(autologinUser);
        return true;
    }

    private void SetUserAsLastLogin(LoginUser user) {
        this.loginUsers.SetUserAsMostRecent(user);
    }

    private bool isLoggingOn = false;
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

    public void BeginLogonToUser(LoginUser user) {
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
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(user.AccountName);
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(user.LoginToken);
                    steamClient.NativeClient.IClientUser.SetLoginToken(user.LoginToken, user.AccountName);

                    // Parse the JWT (format roughly XXXXXX.XXXXXXXXXXXXXXXXXXXXXXXXXXXX.XXXXXXXXX where X is just base64 encoded json data)
                    string middleBase64 = user.LoginToken.Split('.')[1];

                    // The base64 is not always padded properly... Add padding if necessary (FromBase64String is standards-enforcing, meaning it doesn't take in anything but perfect strings)
                    switch (middleBase64.Length % 4) // Pad with trailing '='s
                    {
                        case 0: break; // No pad chars in this case
                        case 2: middleBase64 += "=="; break; // Two pad chars
                        case 3: middleBase64 += "="; break; // One pad char
                        default: throw new System.ArgumentOutOfRangeException("middleBase64", "Illegal base64url string!");
                    }

                    JsonNode? obj = JsonNode.Parse(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(middleBase64)));
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(obj);
                    string? steamidStr = (string?)obj["sub"];
                    OpenSteamworks.Client.Utils.UtilityFunctions.AssertNotNull(steamidStr);

                    user.SteamID = new CSteamID(steamidStr);

                    break;
            }

            this.loginFinishResult = null;
            this.isLoggingOn = true;
            LogonStarted?.Invoke(this, EventArgs.Empty);

            loginProgress?.SetOperation("Logging on " + user.AccountName);

            if (!user.SteamID.HasValue) {
                Console.WriteLine("SteamID is null!");
                OnLogonFailed(new LogOnFailedEventArgs(user, EResult.k_EResultInvalidSteamID));
                return;
            }
            
            EResult beginLogonResult = steamClient.NativeClient.IClientUser.LogOn(user.SteamID.Value);

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
                    loginUsers.SetUserAsAutologin(user);
                }
                OnLoggedOn(new LoggedOnEventArgs(user));
            } else {
                OnLogonFailed(new LogOnFailedEventArgs(user, result));
            }
        }).ContinueWith((t) =>
        {
            if (t.IsFaulted) {
                exceptionHandler?.Invoke(t.Exception!);
            }
        });
    }
    private void OnLogonFailed(LogOnFailedEventArgs e) {
        this.loginFinishResult = null;
        this.isLoggingOn = false;
        LogOnFailed?.Invoke(this, e);
    }

    private void OnLoggedOn(LoggedOnEventArgs e) {
        AddAccount(e.User);
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