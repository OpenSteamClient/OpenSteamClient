#include <QObject>
#include <QList>
#include <QString>
#include <vector>
#include <string>
#include <steammessages_auth.steamclient.pb.h>
#include "../ext/steamclient.h"
#include "loginpolljob.h"
#include "../threading/thread.h"

#pragma once

class LoginThread : public QObject
{
    Q_OBJECT
private:
    JobLoginPolling *credentialsPoller;
    JobLoginPolling *qrCodePoller;
    bool bIsLogonStarted = false;
    bool bRememberPassword = false;
    bool isQRLogin = false;
    bool isCachedCredentialLogin = false;
    std::string username = "";
    std::string password = "";
    std::string sgCode = "";

    // Don't trust this, used only for Steam Guard code auth
    uint64_t twofactorSteamId = 0;

public:
    LoginThread(/* args */);
    ~LoginThread();
    std::string ThreadName();
    void ThreadMain();
    void StopThread();
    void RemoveCachedCredentials(std::string username);
    QList<QString> GetRememberedUsers();
    std::vector<CAuthentication_AllowedConfirmation> allowedConfirmations;

private slots:
    void TokenReceived(std::string username, std::string token);
    void PollingError(std::string error, EResult eResult);
    void LogInWithToken(uint64 steamId, std::string username, std::string token);
    void NewQRUrl(std::string);

public slots:
    void CancelLogin();
    void StartGeneratingQRCodes();
    void StartLogonWithCredentials(std::string username, std::string password, bool rememberPassword);
    void AddSteamGuardCode(std::string sgCode, EAuthSessionGuardType codeType);
    void steamServerConnectFailure(SteamServerConnectFailure_t);
    void steamServerConnected(SteamServersConnected_t);

signals:
    void QRCodeReady(std::string qrUrl);
    void OnLogonFinished();
    void OnLogonFailed(std::string message, EResult eResult);
    
    // When this signal is emitted, allowedConfirmations is set
    void OnNeedsSecondFactor();
};
