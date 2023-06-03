#include "loginthread.h"
#include <steammessages_auth.steamclient.pb.h>
#include <openssl/rsa.h>
#include <openssl/pem.h>
#include <openssl/err.h>
#include <opensteamworks/EOSType.h>
#include <QByteArray>
#include <QBuffer>
#include "../messaging/protomsg.hpp"
#include "../globals.h"
#include "../threading/threadcontroller.h"
#include <thread>
#include <nlohmann/json.hpp>
#include <sys/utsname.h>
#include "../gui/application.h"
#include <openssl/evp.h>
#include <string>

#define WEBSITE_ID "Client"

std::string LoginThread::ThreadName() {
    return "LoginThread";
}
void LoginThread::ThreadMain() {
    
}
void LoginThread::StopThread() {
    
}

LoginThread::LoginThread(/* args */)
{
    this->qrCodePoller = new JobLoginPolling();
    this->qrCodePoller->type = k_EPollerTypeQR;

    this->credentialsPoller = new JobLoginPolling();
    this->credentialsPoller->type = K_EPollerTypeCredentials;

    auto thread = new QThread();
    this->moveToThread(thread);
    thread->start();
}

LoginThread::~LoginThread()
{
}

static std::string ToBase64(const char *in, int actualLength) {
    const auto pl = 4*((actualLength+2)/3);
    auto output = reinterpret_cast<char *>(calloc(pl+1, 1)); //+1 for the terminating null that EVP_EncodeBlock adds on
    const auto ol = EVP_EncodeBlock(reinterpret_cast<unsigned char *>(output), reinterpret_cast<const unsigned char *>(in), actualLength);
    if (pl != ol) { std::cerr << "[LoginThread] Whoops, encode predicted " << pl << " but we got " << ol << "\n"; }
    return std::string(output, ol);
}

//TODO: this should be moved somewhere else
void ConnectToCMs() {
    if (Global_SteamClientMgr->ClientUser->BConnected()) {
        std::cout << "[LoginThread] Already connected to CM's" << std::endl;
        return;
    }
    Global_SteamClientMgr->ClientUser->EConnect();
    while (!Global_SteamClientMgr->ClientUser->BConnected()) {
        DEBUG_MSG << "[LoginThread] Waiting for connection..." << std::endl;
        std::this_thread::sleep_for(std::chrono::milliseconds(50));
    }
    std::cout << "[LoginThread] Connected" << std::endl;
}

void LoginThread::steamServerConnected(SteamServersConnected_t connected) {
    // If "Remember password" is checked, we should then remember the username, otherwise not

    size_t bufSize = 256;
    char *usernameCStr = new char[bufSize];
    Global_SteamClientMgr->ClientUser->GetAccountName(usernameCStr, bufSize);

    this->username = std::string(usernameCStr);
    delete[] usernameCStr;

    if (isCachedCredentialLogin)
    {
        Application::GetApplication()->settings->setValue("LoginInfo/LoginUser", QString::fromStdString(this->username));
    }
    else if (bRememberPassword || isQRLogin)
    {
        Application::GetApplication()->settings->setValue("LoginInfo/LoginUser", QString::fromStdString(this->username));

        // Also save the user to all remembered users
        auto rememberedUsers = GetRememberedUsers();
        rememberedUsers.push_back(QString::fromStdString(this->username));
        Application::GetApplication()->settings->setValue("LoginInfo/RememberedAccounts", rememberedUsers);
    }

    // Save settings 
    Application::GetApplication()->settings->sync();

    isCachedCredentialLogin = false;
    bRememberPassword = false;
    isQRLogin = false;

    emit OnLogonFinished();
}

void LoginThread::steamServerConnectFailure(SteamServerConnectFailure_t connFailure) {
    isCachedCredentialLogin = false;
    emit OnLogonFailed("Failed to connect to CM's", connFailure.m_eResult);
}

void LoginThread::NewQRUrl(std::string url) {
    emit QRCodeReady(url);
}

CAuthentication_DeviceDetails FigureOutDeviceDetails() {
    CAuthentication_DeviceDetails deviceDetails;
    
    // Determine the device hostname
    // We could also let the user decide
    {
        char hostnameCStr[HOST_NAME_MAX];
        gethostname(hostnameCStr, sizeof(hostnameCStr));
        deviceDetails.set_device_friendly_name(std::string(hostnameCStr));
    }
    // This means Steam Deck/Steam Link
    // Desktop platforms is 0
    deviceDetails.set_gaming_device_type(0);

    // As this is a linux only project:
    // - Read the kernel version
    // - Maps it to an enum
    // - If it fails, just use EOSTypeLinux
    //TODO: some minor versions also have their own enums, support those?
    {
        utsname info;

        if (uname(&info) != 0) {
            perror("uname");
            deviceDetails.set_os_type(k_EOSTypeLinux);
        } else {
            char* major_cstr = strtok(info.release, ".");
            auto major = std::string(major_cstr);

            if (major == "7") {
                deviceDetails.set_os_type(k_EOSTypeLinux7x);
            } else if (major == "6") {
                deviceDetails.set_os_type(k_EOSTypeLinux6x);
            } else if (major == "5") {
                deviceDetails.set_os_type(k_EOSTypeLinux5x);
            } else if (major == "4") {
                deviceDetails.set_os_type(k_EOSTypeLinux4x);
            } else if (major == "3") {
                deviceDetails.set_os_type(k_EOSTypeLinux3x);
            } else {
                // Other unsupported kernel version
                deviceDetails.set_os_type(k_EOSTypeLinux);
            }
        }
    }

    // We are a installed client, duh
    deviceDetails.set_platform_type(k_EAuthTokenPlatformType_SteamClient);

    return deviceDetails;
}

void LoginThread::StartGeneratingQRCodes() {
    ConnectToCMs();

    ProtoMsg<CAuthentication_BeginAuthSessionViaQR_Request> *beginAuthSessionViaQRMsg = new ProtoMsg<CAuthentication_BeginAuthSessionViaQR_Request>(true);
    beginAuthSessionViaQRMsg->SetJobName("Authentication.BeginAuthSessionViaQR#1");

    beginAuthSessionViaQRMsg->body.mutable_device_details()->CopyFrom(FigureOutDeviceDetails());
    beginAuthSessionViaQRMsg->body.set_device_friendly_name(beginAuthSessionViaQRMsg->body.device_details().device_friendly_name());
    beginAuthSessionViaQRMsg->body.set_platform_type(beginAuthSessionViaQRMsg->body.device_details().platform_type());
    beginAuthSessionViaQRMsg->body.set_website_id(WEBSITE_ID);
    

    ProtoMsg<CAuthentication_BeginAuthSessionViaQR_Response> beginAuthSessionViaQRResp = beginAuthSessionViaQRMsg->SendMessageAndAwaitResponse<CAuthentication_BeginAuthSessionViaQR_Response>();

    DEBUG_MSG << "[LoginThread] response qr url " << beginAuthSessionViaQRResp.body.challenge_url() << std::endl;
    emit QRCodeReady(beginAuthSessionViaQRResp.body.challenge_url());

    connect(this->qrCodePoller, &JobLoginPolling::OnTokenAvailable, this, &LoginThread::TokenReceived);
    connect(this->qrCodePoller, &JobLoginPolling::OnError, this, &LoginThread::PollingError);
    connect(this->qrCodePoller, &JobLoginPolling::NewChallengeUrl, this, &LoginThread::NewQRUrl);

    this->qrCodePoller->StartPolling(beginAuthSessionViaQRResp.body.interval(), beginAuthSessionViaQRResp.body.client_id(), beginAuthSessionViaQRResp.body.request_id());
}

void LoginThread::LogInWithToken(uint64 steamId, std::string username, std::string token) {
    connect(Global_ThreadController->callbackThread, &CallbackThread::SteamServerConnectFailure, this, &LoginThread::steamServerConnectFailure);
    connect(Global_ThreadController->callbackThread, &CallbackThread::SteamServersConnected, this, &LoginThread::steamServerConnected);
    Global_SteamClientMgr->ClientUser->SetLoginToken(token.c_str(), username.c_str());

    Application::GetApplication()->currentUserSteamID = steamId;

    std::cout << "Logging in with a JWT" << std::endl;
    Global_SteamClientMgr->ClientUser->LogOn(steamId, true);
    bIsLogonStarted = false;
    this->qrCodePoller->StopPolling();
    this->credentialsPoller->StopPolling();
}

void LoginThread::StartLogonWithCredentials(std::string username, std::string password, bool rememberPassword) {
    if (bIsLogonStarted) {
        std::cout << "[LoginThread] Can't start a logon twice!" << std::endl;
        return;
    }
    bIsLogonStarted = true;
    
    this->username = username;
    this->password = password;
    this->bRememberPassword = rememberPassword;

    ConnectToCMs();

    if (Global_SteamClientMgr->ClientUser->BHasCachedCredentials(username.c_str())) {
        isCachedCredentialLogin = true;
        
        connect(Global_ThreadController->callbackThread, &CallbackThread::SteamServerConnectFailure, this, &LoginThread::steamServerConnectFailure);
        connect(Global_ThreadController->callbackThread, &CallbackThread::SteamServersConnected, this, &LoginThread::steamServerConnected);
        
        Global_SteamClientMgr->ClientUser->SetLogonNameForCachedCredentialLogin(username.c_str());

        CSteamID steamid = Global_SteamClientMgr->ClientUser->GetSteamId(username.c_str());
        Application::GetApplication()->currentUserSteamID = steamid.ConvertToUint64();
        std::cout << "[LoginThread] Logging on with cached credentials" << std::endl;
        Global_SteamClientMgr->ClientUser->LogOn(steamid, true);

        bIsLogonStarted = false;
        this->qrCodePoller->StopPolling();
        this->credentialsPoller->StopPolling();
        return;
    }

    ProtoMsg<CAuthentication_GetPasswordRSAPublicKey_Request> *getPasswordRSAKeyMsg = new ProtoMsg<CAuthentication_GetPasswordRSAPublicKey_Request>(true);
    getPasswordRSAKeyMsg->SetJobName("Authentication.GetPasswordRSAPublicKey#1");
    getPasswordRSAKeyMsg->body.set_account_name(username);

    ProtoMsg<CAuthentication_GetPasswordRSAPublicKey_Response> getPasswordRSAKeyResp = getPasswordRSAKeyMsg->SendMessageAndAwaitResponse<CAuthentication_GetPasswordRSAPublicKey_Response>();

    if (!getPasswordRSAKeyResp.success) {
        std::cout << "[LoginThread] Failed to get public key, eResult " << getPasswordRSAKeyResp.header.eresult() << std::endl;
        emit OnLogonFailed("Failed to get public key", (EResult)getPasswordRSAKeyResp.header.eresult());
        return;
    }

    // These functions are deprecated but all the guides use these 
    // So basically as long as these exist we'll use them, I'm sure there's nothing wrong with these
    RSA* rsa = RSA_new();

    // Create a new empty BIGNUM to hold the modulus
    BIGNUM *publickey_mod = BN_new();

    // We can't add .c_str() here as it doesn't work (maybe a memory management issue?)
    auto mod_as_str = getPasswordRSAKeyResp.body.publickey_mod();

    // Convert the strange looking long string (modulus apparently) to a BIGNUM (this is some black magic)
    BN_hex2bn(&publickey_mod, mod_as_str.c_str()); 

    // Convert the binary number (010001) we get to an int (17)
    // And then we convert it back to a string
    auto exp_as_str = std::to_string(std::stoi(getPasswordRSAKeyResp.body.publickey_exp(), nullptr, 2));

    // Then we feed it into this function which turns it once again into an int and then a BIGNUM
    BIGNUM *publickey_exp = BN_new();
    //publickey_exp = BN_bin2bn((unsigned char*)getPasswordRSAKeyResp.body.publickey_exp().data(), getPasswordRSAKeyResp.body.publickey_exp().length(), NULL);
    BN_hex2bn(&publickey_exp, getPasswordRSAKeyResp.body.publickey_exp().c_str()); 

    // Is this sensitive info to expose?
    #ifdef DEV_BUILD
        std::cout << "[LoginThread] EXP as string is " << exp_as_str << std::endl;
        std::cout << "[LoginThread] EXP is ";
        BN_print_fp(stdout, publickey_exp);
        std::cout << std::endl;

        std::cout << "[LoginThread] MOD is ";
        BN_print_fp(stdout, publickey_mod);
        std::cout << std::endl;
    #endif

    // This makes the modulus and exponent create a key somehow (more dark magic)
    RSA_set0_key(rsa, publickey_mod, publickey_exp, NULL);

    int actualLength = 0;
    void *encryptedPassword = malloc(RSA_size(rsa));
    
    // Initialize encryptedPassword with zeroes
    memset(encryptedPassword, 0, RSA_size(rsa));

    // Encrypt the password into encryptedPassword
    auto retun = RSA_public_encrypt(password.length(), reinterpret_cast<unsigned char *>(password.data()), reinterpret_cast<unsigned char *>(encryptedPassword), rsa, RSA_PKCS1_PADDING);

    // Record the actual length
    actualLength = retun;

    // If an error occurred, we log it
    auto err = ERR_get_error();

    if (err != 0) {
        char error[1024];
        ERR_error_string_n(err, error, 1024);
        std::cerr << "[LoginThread] error is " << error << std::endl;
    }

    RSA_free(rsa);

    // Converts the encrypted password to base64
    std::string encodedPassword = ToBase64(reinterpret_cast<char *>(encryptedPassword), actualLength);


    ProtoMsg<CAuthentication_BeginAuthSessionViaCredentials_Request> *beginAuthSessionViaCredentialsMsg = new ProtoMsg<CAuthentication_BeginAuthSessionViaCredentials_Request>(true);
    beginAuthSessionViaCredentialsMsg->SetJobName("Authentication.BeginAuthSessionViaCredentials#1");

    beginAuthSessionViaCredentialsMsg->body.set_account_name(username);
    beginAuthSessionViaCredentialsMsg->body.set_encrypted_password(encodedPassword);
    beginAuthSessionViaCredentialsMsg->body.set_encryption_timestamp(getPasswordRSAKeyResp.body.timestamp());
    beginAuthSessionViaCredentialsMsg->body.set_remember_login(rememberPassword);
    beginAuthSessionViaCredentialsMsg->body.set_website_id(WEBSITE_ID);
    beginAuthSessionViaCredentialsMsg->body.set_persistence(k_ESessionPersistence_Persistent);

    beginAuthSessionViaCredentialsMsg->body.mutable_device_details()->CopyFrom(FigureOutDeviceDetails());
    beginAuthSessionViaCredentialsMsg->body.set_device_friendly_name(beginAuthSessionViaCredentialsMsg->body.device_details().device_friendly_name());
    beginAuthSessionViaCredentialsMsg->body.set_platform_type(beginAuthSessionViaCredentialsMsg->body.device_details().platform_type());

    beginAuthSessionViaCredentialsMsg->body.set_qos_level(2);

    //TODO: What are the language codes?
    beginAuthSessionViaCredentialsMsg->body.set_language(1);

    ProtoMsg<CAuthentication_BeginAuthSessionViaCredentials_Response> beginAuthSessionViaCredentialsResp = beginAuthSessionViaCredentialsMsg->SendMessageAndAwaitResponse<CAuthentication_BeginAuthSessionViaCredentials_Response>();
    if (beginAuthSessionViaCredentialsResp.success) {
        if (beginAuthSessionViaCredentialsResp.header.eresult() != k_EResultOK) {
            emit OnLogonFailed("Error response from CM", (EResult)beginAuthSessionViaCredentialsResp.header.eresult());
            bIsLogonStarted = false;
            return;
        }
        if (beginAuthSessionViaCredentialsResp.body.allowed_confirmations_size() > 0) {
            auto allowedConfirmations = beginAuthSessionViaCredentialsResp.body.allowed_confirmations();
            for (auto confirmation : allowedConfirmations) {
                DEBUG_MSG << "[LoginThread] Confirmation type " << confirmation.confirmation_type() << " is allowed" << std::endl;
                this->allowedConfirmations.push_back(confirmation);
            }
            emit OnNeedsSecondFactor();
        }
        if (beginAuthSessionViaCredentialsResp.body.has_steamid()) {
            twofactorSteamId = beginAuthSessionViaCredentialsResp.body.steamid();
        }

        connect(this->credentialsPoller, &JobLoginPolling::OnTokenAvailable, this, &LoginThread::TokenReceived);
        connect(this->credentialsPoller, &JobLoginPolling::OnError, this, &LoginThread::PollingError);
        this->credentialsPoller->StartPolling(beginAuthSessionViaCredentialsResp.body.interval(), beginAuthSessionViaCredentialsResp.body.client_id(), beginAuthSessionViaCredentialsResp.body.request_id());
    } else {
        bIsLogonStarted = false;
    }
}

// least complicated c++ function
//TODO: make a string extensions class
std::vector<std::string> SplitString(std::string str, char delim) {
    std::replace(str.begin(), str.end(), delim, ' '); 

    std::vector<std::string> array;
    std::stringstream ss(str);
    std::string temp;
    while (ss >> temp) {
        array.push_back(temp);
    }
    return array;
}

void LoginThread::TokenReceived(std::string username, std::string token) {
    JobLoginPolling *poller = qobject_cast<JobLoginPolling*>(sender());
    if (poller->type == k_EPollerTypeQR) {
        isQRLogin = true;
    } else {
        isQRLogin = false;
    }

    int i = 0;
    nlohmann::json tokenInfo;
    
    for (auto part : SplitString(token, '.'))
    {
        // First substring part after the dot
        if (i == 1) {
            QString str = QString::fromStdString(std::string{part});
            QByteArray barray = QByteArray::fromBase64(str.toUtf8());
            tokenInfo = nlohmann::json::parse(barray.toStdString());
        }
        i++;
    }

    // sub field of a token has the steamid of the user
    LogInWithToken(std::stoul(tokenInfo["sub"].get<std::string>(), nullptr, 0), username, token);
}

void LoginThread::PollingError(std::string error, EResult eResult) {
    emit OnLogonFailed("Polling error occurred: " + error, eResult);
}

void LoginThread::AddSteamGuardCode(std::string sgCode, EAuthSessionGuardType codeType) {
    if (codeType == k_EAuthSessionGuardType_None) {
        std::cout << "No EAuthSessionGuardType specified" << std::endl;
        return;
    }
    ProtoMsg<CAuthentication_UpdateAuthSessionWithSteamGuardCode_Request> *updateAuthSessionWithSteamGuardCodeMsg = new ProtoMsg<CAuthentication_UpdateAuthSessionWithSteamGuardCode_Request>(true);
    updateAuthSessionWithSteamGuardCodeMsg->SetJobName("Authentication.UpdateAuthSessionWithSteamGuardCode#1");
    updateAuthSessionWithSteamGuardCodeMsg->body.set_code_type(codeType);
    updateAuthSessionWithSteamGuardCodeMsg->body.set_code(sgCode);
    updateAuthSessionWithSteamGuardCodeMsg->body.set_client_id(this->credentialsPoller->client_id);
    updateAuthSessionWithSteamGuardCodeMsg->body.set_steamid(this->twofactorSteamId);
    ProtoMsg<CAuthentication_UpdateAuthSessionWithSteamGuardCode_Response> updateAuthSessionWithSteamGuardCodeResp = updateAuthSessionWithSteamGuardCodeMsg->SendMessageAndAwaitResponse<CAuthentication_UpdateAuthSessionWithSteamGuardCode_Response>();
    if (updateAuthSessionWithSteamGuardCodeResp.success) { 
        //TODO: what to do with agreement_session_url here?
    }
}

void LoginThread::CancelLogin() {
    if (!bIsLogonStarted) {
        return;
    }

    this->credentialsPoller->StopPolling();
    bIsLogonStarted = false;
    emit OnLogonFailed("User canceled the login", k_EResultRequestHasBeenCanceled);
}

void LoginThread::RemoveCachedCredentials(std::string username) {
    QList<QString> newList;
    
    for (auto &&i : Application::GetApplication()->settings->value("LoginInfo/RememberedAccounts").value<QList<QString>>())
    {
        if (i.toStdString() != username) {
            newList.push_back(i);
        }
    }

    Application::GetApplication()->settings->setValue("LoginInfo/RememberedAccounts", newList);

    if (Application::GetApplication()->settings->value("LoginInfo/LoginUser").value<QString>() == QString::fromStdString(username)) {
        Application::GetApplication()->settings->remove("LoginInfo/LoginUser");
    }

    Global_SteamClientMgr->ClientUser->DestroyCachedCredentials(username.c_str());
    Application::GetApplication()->settings->sync();
}

QList<QString> LoginThread::GetRememberedUsers() {
    return Application::GetApplication()->settings->value("LoginInfo/RememberedAccounts").value<QList<QString>>();
}