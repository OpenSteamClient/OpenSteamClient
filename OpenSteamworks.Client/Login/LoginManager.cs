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
using System.Security.Cryptography;
using System.Text;

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

public class SecondFactorNeededEventArgs : EventArgs
{
    public SecondFactorNeededEventArgs(IEnumerable<CAuthentication_AllowedConfirmation> allowedConfirmations) { AllowedConfirmations = allowedConfirmations; }
    public IEnumerable<CAuthentication_AllowedConfirmation> AllowedConfirmations { get; }
}

public class LoginManager : Component
{
    public delegate void LoggedOnEventHandler(object sender, LoggedOnEventArgs e);
    public delegate void LoggedOffEventHandler(object sender, LoggedOffEventArgs e);
    public delegate void LogOnStartedEventHandler(object sender, EventArgs e);
    public delegate void LogOnFailedEventHandler(object sender, LogOnFailedEventArgs e);
    public delegate void QRGeneratedEventHandler(object sender, QRGeneratedEventArgs e);
    public delegate void SecondFactorNeededEventHandler(object sender, SecondFactorNeededEventArgs e);
    public event LoggedOnEventHandler? LoggedOn;
    public event LoggedOffEventHandler? LoggedOff;
    public event LogOnStartedEventHandler? LogonStarted;
    public event LogOnFailedEventHandler? LogOnFailed;
    public event QRGeneratedEventHandler? QRGenerated;
    public event SecondFactorNeededEventHandler? SecondFactorNeeded;

    private SteamClient steamClient;
    private LoginUsers loginUsers;
    private ClientMessaging clientMessaging;
    private LoginPoll? QRPoller;
    private LoginPoll? CredentialsPoller;
    /// <summary>
    /// Used only when a logon is in progress. To get the logged in user's SteamID, use IClientUser::GetSteamID or LoginManager::CurrentUser::SteamID
    /// </summary>
    public CSteamID CurrentSteamID { get; private set; } = 0;
    public LoginUser? CurrentUser { get; private set; }

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
        if (QRPoller != null && QRPoller.IsPolling) {
            throw new InvalidOperationException("QR Auth loop already running");
        }

        Task.Run(async () =>
        {
            using (Connection conn = clientMessaging.AllocateConnection())
            {
                ProtoMsg<CAuthentication_BeginAuthSessionViaQR_Request> beginMsg = new("Authentication.BeginAuthSessionViaQR#1", true);
                beginMsg.body.DeviceDetails = DeviceDetails;

                var beginResp = await conn.ProtobufSendMessageAndAwaitResponse<CAuthentication_BeginAuthSessionViaQR_Response, CAuthentication_BeginAuthSessionViaQR_Request>(beginMsg);

                QRGenerated?.Invoke(this, new QRGeneratedEventArgs(beginResp.body.ChallengeUrl));
                
                QRPoller = new LoginPoll(this.clientMessaging, beginResp.body.ClientId, beginResp.body.RequestId, beginResp.body.Interval);
                QRPoller.ChallengeUrlGenerated += (object sender, ChallengeUrlGeneratedEventArgs e) => this.QRGenerated?.Invoke(this, new QRGeneratedEventArgs(e.URL));
                QRPoller.RefreshTokenGenerated += OnRefreshTokenGenerated;
                QRPoller.Error += (object sender, EResultEventArgs e) => exceptionHandler?.Invoke(new AggregateException("Error occurred in QR Poller: " + e.EResult));
                QRPoller.StartPolling();
            }
        });
    }

    /// <summary>
    /// Starts an auth session via credentials. Add extra steam guard details, etc with other functions.
    /// Will fire events for needed extra details, etc
    /// </summary>
    /// <returns>True if initial credentials were ok, false if session failed to start</returns>
    public async Task<EResult> StartAuthSessionWithCredentials(string username, string password, bool rememberPassword) {
        if (CredentialsPoller != null && CredentialsPoller.IsPolling) {
            throw new InvalidOperationException("Credential logon already in progress");
        }

        using (Connection conn = clientMessaging.AllocateConnection())
        {
            // Get password RSA hash
            ProtoMsg<CAuthentication_GetPasswordRSAPublicKey_Request> rsaRequest = new("Authentication.GetPasswordRSAPublicKey#1", true);
            rsaRequest.body.AccountName = username;

            var rsaResp = await conn.ProtobufSendMessageAndAwaitResponse<CAuthentication_GetPasswordRSAPublicKey_Response, CAuthentication_GetPasswordRSAPublicKey_Request>(rsaRequest);

            // Encrypt the password (this simple in C#)
            string encryptedPassword;
            using (var rsa = RSA.Create())
            {
                
                var rsaParameters = new RSAParameters
                {
                    Modulus = Convert.FromHexString(rsaResp.body.PublickeyMod),
                    Exponent = Convert.FromHexString(rsaResp.body.PublickeyExp),
                };
                rsa.ImportParameters(rsaParameters);
                encryptedPassword = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(password), RSAEncryptionPadding.Pkcs1));
            }

            ProtoMsg<CAuthentication_BeginAuthSessionViaCredentials_Request> beginMsg = new("Authentication.BeginAuthSessionViaCredentials#1", true);
            beginMsg.body.DeviceDetails = DeviceDetails;
            beginMsg.body.AccountName = username;
            beginMsg.body.EncryptedPassword = encryptedPassword;
            beginMsg.body.EncryptionTimestamp = rsaResp.body.Timestamp;
            // This one is unused, but let's set it just in case
            beginMsg.body.RememberLogin = rememberPassword;
            // This determines password remembered yes/no
            beginMsg.body.Persistence = rememberPassword ? ESessionPersistence.KEsessionPersistencePersistent : ESessionPersistence.KEsessionPersistenceEphemeral;
            beginMsg.body.PlatformType = EAuthTokenPlatformType.KEauthTokenPlatformTypeSteamClient;
            beginMsg.body.WebsiteId = "Client";
            beginMsg.body.DeviceFriendlyName = DeviceDetails.DeviceFriendlyName;

            var beginResp = await conn.ProtobufSendMessageAndAwaitResponse<CAuthentication_BeginAuthSessionViaCredentials_Response, CAuthentication_BeginAuthSessionViaCredentials_Request>(beginMsg);
            Console.WriteLine(beginResp);
            if (beginResp.header.Eresult != (int)EResult.k_EResultOK) {
                return (EResult)beginResp.header.Eresult;
            }

            // This should always have a steamid here, otherwise we can't get it from anywhere
            CurrentSteamID = beginResp.body.Steamid;

            // Interval is 0.1 and/or allowedconfirmations is set to [k_EAuthSessionGuardType_None] when user has no 2fa
            if (beginResp.body.AllowedConfirmations.All(elem => elem.ConfirmationType == EAuthSessionGuardType.KEauthSessionGuardTypeNone)) {
                // Only one session type, and that's the None type
            } else {
                SecondFactorNeeded?.Invoke(this, new SecondFactorNeededEventArgs(beginResp.body.AllowedConfirmations));
            }

            CredentialsPoller = new LoginPoll(this.clientMessaging, beginResp.body.ClientId, beginResp.body.RequestId, beginResp.body.Interval);
            CredentialsPoller.RefreshTokenGenerated += OnRefreshTokenGenerated;
            CredentialsPoller.Error += (object sender, EResultEventArgs e) => exceptionHandler?.Invoke(new AggregateException("Error occurred in Credentials Poller: " + e.EResult));
            CredentialsPoller.StartPolling();
            return EResult.k_EResultOK;
        }
    }

    public async Task<EResult> UpdateAuthSessionWithTwoFactor(string code, EAuthSessionGuardType codeType) {
        if (CredentialsPoller == null || !CredentialsPoller.IsPolling) {
            throw new InvalidOperationException("Credential logon isn't running, cannot add steam guard code");
        }

        using (Connection conn = clientMessaging.AllocateConnection())
        {
            ProtoMsg<CAuthentication_UpdateAuthSessionWithSteamGuardCode_Request> updateMsg = new("Authentication.UpdateAuthSessionWithSteamGuardCode#1", true);
            updateMsg.body.ClientId = CredentialsPoller.ClientID;
            updateMsg.body.CodeType = codeType;
            updateMsg.body.Code = code;
            updateMsg.body.Steamid = CurrentSteamID;

            var updateResp = await conn.ProtobufSendMessageAndAwaitResponse<CAuthentication_UpdateAuthSessionWithSteamGuardCode_Response, CAuthentication_UpdateAuthSessionWithSteamGuardCode_Request>(updateMsg);
            return (EResult)updateResp.header.Eresult;
        }
    }

    public void StopQRAuthLoop() {
        QRPoller?.StopPolling();
        QRPoller = null;
    }

    private void OnRefreshTokenGenerated(object sender, TokenGeneratedEventArgs e) {
        BeginLogonToUser(new LoginUser(e.AccountName, e.Token));
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
            //TODO: allow user to specify this somewhere, maybe in a first-run wizard, in settings, config file etc
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

    private bool isLoggingOn = false;
    private EResult? loginFinishResult;
    [CallbackListener<SteamServerConnectFailure_t>]
    public void OnSteamServerConnectFailure(SteamServerConnectFailure_t failure) {
        if (isLoggingOn) {
            loginFinishResult = failure.m_EResult;
        }
    }

    [CallbackListener<SteamServersDisconnected_t>]
    public void OnSteamServersDisconnected(SteamServersDisconnected_t disconnect) {
        if (CurrentUser != null) {
            
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
                    UtilityFunctions.AssertNotNull(user.AccountName);
                    UtilityFunctions.AssertNotNull(user.Password);
                    steamClient.NativeClient.IClientUser.SetLoginInformation(user.AccountName, user.Password, user.Remembered);
                    if (user.SteamGuardCode != null) {
                        steamClient.NativeClient.IClientUser.SetTwoFactorCode(user.SteamGuardCode);
                    }

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
                loginUsers.SetUserAsMostRecent(user);
                if (user.AllowAutoLogin == true)
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
    public void LogoutForgetAccount() {
        UtilityFunctions.AssertNotNull(this.CurrentUser);
        this.Logout();
        this.loginUsers.RemoveUser(this.CurrentUser);
    }

    public async void Logout() {
        UtilityFunctions.AssertNotNull(this.CurrentUser);
        var oldUser = CurrentUser;
        this.steamClient.NativeClient.IClientUser.LogOff();
        await Task.Run(() => {
            while (this.steamClient.NativeClient.IClientUser.BLoggedOn())
            {
                System.Threading.Thread.Sleep(50);
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